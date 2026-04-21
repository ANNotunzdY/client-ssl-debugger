using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientSslDebugWinForms;

public partial class MainForm : Form
{
    private CancellationTokenSource? _cts;

    public MainForm()
    {
        InitializeComponent();
        txtUrl.Text = "";
        txtTimeoutSeconds.Text = "100";
        txtRequestBody.Text = "";
        cmbMethod.SelectedIndex = 0;
        cmbClientCertKind.SelectedIndex = 0;
        cmbServerCertKind.SelectedIndex = 0;
    }

    private void BtnBrowseClientCert_Click(object sender, EventArgs e)
    {
        using var dialog = new OpenFileDialog
        {
            Filter = "証明書ファイル|*.pfx;*.p12;*.pem;*.crt;*.cer|すべてのファイル|*.*"
        };

        if (dialog.ShowDialog(this) == DialogResult.OK)
        {
            txtClientCertPath.Text = dialog.FileName;
        }
    }

    private void BtnBrowseServerCert_Click(object sender, EventArgs e)
    {
        using var dialog = new OpenFileDialog
        {
            Filter = "証明書ファイル|*.pem;*.crt;*.cer|すべてのファイル|*.*"
        };

        if (dialog.ShowDialog(this) == DialogResult.OK)
        {
            txtServerCertPath.Text = dialog.FileName;
        }
    }

    private async void BtnSend_Click(object sender, EventArgs e)
    {
        try
        {
            ToggleBusy(true);
            _cts = new CancellationTokenSource();
            AppendLog("=== リクエスト開始 ===");

            var request = await BuildAndSendAsync(_cts.Token);

            txtStatus.Text = $"{(int)request.StatusCode} {request.StatusCode}";
            txtResponseHeaders.Text = request.Headers;
            txtResponseBody.Text = request.Body;

            AppendLog("=== リクエスト完了 ===");
        }
        catch (OperationCanceledException)
        {
            AppendLog("キャンセルされました。");
        }
        catch (Exception ex)
        {
            txtStatus.Text = "ERROR";
            AppendLog(ex.ToString());
            MessageBox.Show(this, ex.Message, "通信エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            ToggleBusy(false);
            _cts?.Dispose();
            _cts = null;
        }
    }

    private void BtnCancel_Click(object sender, EventArgs e)
    {
        _cts?.Cancel();
    }

    private void BtnPrettyJson_Click(object sender, EventArgs e)
    {
        try
        {
            using var doc = JsonDocument.Parse(txtResponseBody.Text);
            txtResponseBody.Text = JsonSerializer.Serialize(doc.RootElement, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, $"JSON 整形に失敗しました。{Environment.NewLine}{ex.Message}", "JSON整形", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void BtnCopyCurl_Click(object sender, EventArgs e)
    {
        try
        {
            var sb = new StringBuilder();
            sb.Append("curl ");
            sb.Append("--request ").Append(cmbMethod.Text).Append(' ');
            sb.Append("--url \"").Append(txtUrl.Text.Trim()).Append("\" ");

            if (!string.IsNullOrWhiteSpace(txtClientCertPath.Text))
            {
                sb.Append("--cert \"").Append(txtClientCertPath.Text.Trim()).Append("\" ");
            }

            if (!string.IsNullOrWhiteSpace(txtServerCertPath.Text))
            {
                sb.Append("--cacert \"").Append(txtServerCertPath.Text.Trim()).Append("\" ");
            }

            foreach (var header in ParseHeaders(txtHeaders.Text))
            {
                sb.Append("-H \"").Append(header.Key).Append(": ").Append(header.Value).Append("\" ");
            }

            if (!string.IsNullOrWhiteSpace(txtRequestBody.Text))
            {
                sb.Append("--data \"").Append(txtRequestBody.Text.Replace("\"", "\\\"")).Append("\"");
            }

            Clipboard.SetText(sb.ToString().Trim());
            AppendLog("curl コマンドをクリップボードへコピーしました。");
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "コピー失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task<ResponseResult> BuildAndSendAsync(CancellationToken cancellationToken)
    {
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;

        using var handler = new HttpClientHandler();
        handler.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13;
        handler.ClientCertificateOptions = ClientCertificateOption.Manual;

        var customServerCert = LoadCustomServerCertificate();
        var skipServerValidation = chkSkipServerValidation.Checked;
        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
        {
            AppendLog($"サーバー証明書検証: {errors}");

            if (skipServerValidation)
            {
                AppendLog("サーバー証明書検証をスキップしました。");
                return true;
            }

            if (customServerCert is null)
            {
                return errors == System.Net.Security.SslPolicyErrors.None;
            }

            if (cert is null)
            {
                return false;
            }

            var remote = new X509Certificate2(cert);
            var chainIsValid = ValidateServerCertificate(remote, customServerCert, out var chainLog);
            AppendLog($"サーバー証明書検証詳細: remote={remote.Thumbprint}, trustedCA={customServerCert.Thumbprint}, chainValid={chainIsValid}, detail={chainLog}");
            return chainIsValid;
        };

        var clientCertificate = LoadClientCertificate();
        if (clientCertificate is not null)
        {
            handler.ClientCertificates.Add(clientCertificate);
            AppendLog($"クライアント証明書読み込み成功: subject={clientCertificate.Subject}, thumbprint={clientCertificate.Thumbprint}, hasPrivateKey={clientCertificate.HasPrivateKey}");
        }
        else
        {
            AppendLog("クライアント証明書未設定で送信します。");
        }

        using var httpClient = new HttpClient(handler)
        {
            Timeout = TimeSpan.FromSeconds(ParseTimeoutSeconds())
        };

        using var requestMessage = new HttpRequestMessage(new HttpMethod(cmbMethod.Text), txtUrl.Text.Trim());

        foreach (var header in ParseHeaders(txtHeaders.Text))
        {
            if (!requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value))
            {
                requestMessage.Content ??= new StringContent(string.Empty);
                requestMessage.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        var body = txtRequestBody.Text;
        if (!string.IsNullOrWhiteSpace(body) && requestMessage.Method != HttpMethod.Get)
        {
            var mediaType = txtContentType.Text.Trim();
            requestMessage.Content = new StringContent(body, Encoding.UTF8, string.IsNullOrWhiteSpace(mediaType) ? "application/x-www-form-urlencoded" : mediaType);
        }

        AppendLog($"URL: {requestMessage.RequestUri}");
        AppendLog($"Method: {requestMessage.Method}");
        AppendLog($"Body: {body}");

        using var response = await httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

        var headerDump = new StringBuilder();
        foreach (var header in response.Headers)
        {
            headerDump.AppendLine($"{header.Key}: {string.Join(", ", header.Value)}");
        }
        foreach (var header in response.Content.Headers)
        {
            headerDump.AppendLine($"{header.Key}: {string.Join(", ", header.Value)}");
        }

        AppendLog($"HTTP {(int)response.StatusCode} {response.StatusCode}");

        return new ResponseResult(response.StatusCode, headerDump.ToString(), responseBody);
    }

    private X509Certificate2? LoadClientCertificate()
    {
        var path = txtClientCertPath.Text.Trim();
        if (string.IsNullOrWhiteSpace(path))
        {
            return null;
        }

        if (!File.Exists(path))
        {
            throw new FileNotFoundException("クライアント証明書ファイルが見つかりません。", path);
        }

        var kind = cmbClientCertKind.Text;
        var password = txtClientCertPassword.Text;

        var certificate = kind switch
        {
            "PFX/P12" => LoadClientCertificateFromPfx(path, password),
            "PEM" => LoadClientCertificateFromPem(path),
            _ => new X509Certificate2(path)
        };

        ValidateClientCertificate(certificate, path, kind);
        return certificate;
    }

    private X509Certificate2? LoadCustomServerCertificate()
    {
        var path = txtServerCertPath.Text.Trim();
        if (string.IsNullOrWhiteSpace(path))
        {
            return null;
        }

        if (!File.Exists(path))
        {
            throw new FileNotFoundException("サーバー証明書ファイルが見つかりません。", path);
        }

        var kind = cmbServerCertKind.Text;
        return kind switch
        {
            "PEM" => LoadServerCertificateFromPem(path),
            _ => new X509Certificate2(path)
        };
    }

    private int ParseTimeoutSeconds()
    {
        if (!int.TryParse(txtTimeoutSeconds.Text.Trim(), out var seconds) || seconds <= 0)
        {
            throw new InvalidOperationException("タイムアウト秒数は 1 以上の整数で入力してください。");
        }

        return seconds;
    }

    private static Dictionary<string, string> ParseHeaders(string raw)
    {
        var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var lines = raw.Replace("\r\n", "\n").Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        foreach (var line in lines)
        {
            var index = line.IndexOf(':');
            if (index <= 0)
            {
                continue;
            }

            var key = line[..index].Trim();
            var value = line[(index + 1)..].Trim();
            if (!string.IsNullOrWhiteSpace(key))
            {
                headers[key] = value;
            }
        }

        return headers;
    }

    private void ToggleBusy(bool busy)
    {
        btnSend.Enabled = !busy;
        btnCancel.Enabled = busy;
        UseWaitCursor = busy;
    }

    private void AppendLog(string message)
    {
        if (IsDisposed || Disposing)
        {
            return;
        }

        if (txtLog.InvokeRequired)
        {
            try
            {
                txtLog.BeginInvoke(new Action<string>(AppendLog), message);
            }
            catch (InvalidOperationException)
            {
                // Ignore logging when the form/control is shutting down.
            }

            return;
        }

        txtLog.AppendText($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {message}{Environment.NewLine}");
    }

    private static X509Certificate2 LoadClientCertificateFromPfx(string path, string? password)
    {
        var importAttempts = new[]
        {
            X509KeyStorageFlags.UserKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable,
            X509KeyStorageFlags.UserKeySet | X509KeyStorageFlags.Exportable,
            X509KeyStorageFlags.DefaultKeySet | X509KeyStorageFlags.Exportable,
            X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable
        };

        Exception? lastException = null;
        foreach (var flags in importAttempts)
        {
            try
            {
                return new X509Certificate2(path, password, flags);
            }
            catch (CryptographicException ex)
            {
                lastException = ex;
            }
        }

        throw new CryptographicException("PFX/P12 クライアント証明書の読み込みに失敗しました。パスワードと秘密鍵の有無を確認してください。", lastException);
    }

    private static X509Certificate2 LoadClientCertificateFromPem(string path)
    {
        var pemText = File.ReadAllText(path);
        if (!pemText.Contains("-----BEGIN PRIVATE KEY-----", StringComparison.Ordinal) &&
            !pemText.Contains("-----BEGIN RSA PRIVATE KEY-----", StringComparison.Ordinal) &&
            !pemText.Contains("-----BEGIN EC PRIVATE KEY-----", StringComparison.Ordinal) &&
            !pemText.Contains("-----BEGIN ENCRYPTED PRIVATE KEY-----", StringComparison.Ordinal))
        {
            throw new InvalidOperationException($"PEM に秘密鍵ブロックが見つかりません。file={path}");
        }

        try
        {
            using var ephemeralCertificate = X509Certificate2.CreateFromPemFile(path, path);
            var pfxBytes = ephemeralCertificate.Export(X509ContentType.Pkcs12);
            return new X509Certificate2(
                pfxBytes,
                (string?)null,
                X509KeyStorageFlags.UserKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
        }
        catch (CryptographicException ex)
        {
            throw new CryptographicException($"PEM クライアント証明書の読み込みに失敗しました。file={path}. 証明書と秘密鍵が同じファイル内にあるか、暗号化されていないことを確認してください。", ex);
        }
    }

    private static X509Certificate2 LoadServerCertificateFromPem(string path)
    {
        var pemText = File.ReadAllText(path);
        if (!pemText.Contains("-----BEGIN CERTIFICATE-----", StringComparison.Ordinal))
        {
            throw new InvalidOperationException($"サーバー PEM に証明書ブロックが見つかりません。file={path}");
        }

        try
        {
            return X509Certificate2.CreateFromPem(pemText);
        }
        catch (CryptographicException ex)
        {
            throw new CryptographicException($"サーバー PEM 証明書の読み込みに失敗しました。file={path}. CA/サーバー証明書だけを含む PEM を指定してください。", ex);
        }
    }

    private static bool ValidateServerCertificate(X509Certificate2 remoteCertificate, X509Certificate2 trustedCertificate, out string detail)
    {
        using var chain = new X509Chain();
        chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
        chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority;
        chain.ChainPolicy.TrustMode = X509ChainTrustMode.CustomRootTrust;
        chain.ChainPolicy.CustomTrustStore.Add(trustedCertificate);

        var chainBuilt = chain.Build(remoteCertificate);
        var statuses = new List<string>();
        foreach (var status in chain.ChainStatus)
        {
            if (status.Status == X509ChainStatusFlags.NoError)
            {
                continue;
            }

            statuses.Add($"{status.Status}: {status.StatusInformation.Trim()}");
        }

        var rootThumbprint = chain.ChainElements.Count > 0
            ? chain.ChainElements[^1].Certificate.Thumbprint
            : "(none)";
        var rootMatchesTrusted = string.Equals(rootThumbprint, trustedCertificate.Thumbprint, StringComparison.OrdinalIgnoreCase);

        detail = statuses.Count == 0
            ? $"root={rootThumbprint}"
            : string.Join(" | ", statuses) + $", root={rootThumbprint}";

        return chainBuilt && rootMatchesTrusted;
    }

    private static void ValidateClientCertificate(X509Certificate2 certificate, string path, string kind)
    {
        if (!certificate.HasPrivateKey)
        {
            var detail = kind == "PEM"
                ? "PEM を選んだ場合、このアプリは証明書本体しか読まず秘密鍵を読み込めません。秘密鍵付きの PFX/P12 を使うか、PEM の秘密鍵読み込み対応が必要です。"
                : "TLS クライアント認証には秘密鍵付き証明書が必要です。";
            throw new InvalidOperationException($"クライアント証明書に秘密鍵がありません。file={path}. {detail}");
        }

        using RSA? rsa = certificate.GetRSAPrivateKey();
        using ECDsa? ecdsa = certificate.GetECDsaPrivateKey();
        if (rsa is null && ecdsa is null)
        {
            throw new InvalidOperationException($"クライアント証明書の秘密鍵を開けません。file={path}");
        }
    }

    private sealed record ResponseResult(HttpStatusCode StatusCode, string Headers, string Body);
}
