namespace ClientSslDebugWinForms;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.TextBox txtUrl;
    private System.Windows.Forms.ComboBox cmbMethod;
    private System.Windows.Forms.TextBox txtClientCertPath;
    private System.Windows.Forms.TextBox txtClientCertPassword;
    private System.Windows.Forms.ComboBox cmbClientCertKind;
    private System.Windows.Forms.TextBox txtServerCertPath;
    private System.Windows.Forms.ComboBox cmbServerCertKind;
    private System.Windows.Forms.TextBox txtHeaders;
    private System.Windows.Forms.TextBox txtContentType;
    private System.Windows.Forms.TextBox txtRequestBody;
    private System.Windows.Forms.TextBox txtResponseHeaders;
    private System.Windows.Forms.TextBox txtResponseBody;
    private System.Windows.Forms.TextBox txtLog;
    private System.Windows.Forms.TextBox txtStatus;
    private System.Windows.Forms.TextBox txtTimeoutSeconds;
    private System.Windows.Forms.Button btnBrowseClientCert;
    private System.Windows.Forms.Button btnBrowseServerCert;
    private System.Windows.Forms.Button btnSend;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnPrettyJson;
    private System.Windows.Forms.Button btnCopyCurl;
    private System.Windows.Forms.CheckBox chkSkipServerValidation;
    private System.Windows.Forms.TableLayoutPanel root;
    private System.Windows.Forms.TabControl tabs;
    private System.Windows.Forms.TabPage tabRequest;
    private System.Windows.Forms.TabPage tabResponse;
    private System.Windows.Forms.TabPage tabLog;
    private System.Windows.Forms.SplitContainer splitResponse;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        root = new System.Windows.Forms.TableLayoutPanel();
        tabs = new System.Windows.Forms.TabControl();
        tabRequest = new System.Windows.Forms.TabPage();
        tabResponse = new System.Windows.Forms.TabPage();
        tabLog = new System.Windows.Forms.TabPage();
        splitResponse = new System.Windows.Forms.SplitContainer();
        txtUrl = new System.Windows.Forms.TextBox();
        cmbMethod = new System.Windows.Forms.ComboBox();
        txtClientCertPath = new System.Windows.Forms.TextBox();
        txtClientCertPassword = new System.Windows.Forms.TextBox();
        cmbClientCertKind = new System.Windows.Forms.ComboBox();
        txtServerCertPath = new System.Windows.Forms.TextBox();
        cmbServerCertKind = new System.Windows.Forms.ComboBox();
        txtHeaders = new System.Windows.Forms.TextBox();
        txtContentType = new System.Windows.Forms.TextBox();
        txtRequestBody = new System.Windows.Forms.TextBox();
        txtResponseHeaders = new System.Windows.Forms.TextBox();
        txtResponseBody = new System.Windows.Forms.TextBox();
        txtLog = new System.Windows.Forms.TextBox();
        txtStatus = new System.Windows.Forms.TextBox();
        txtTimeoutSeconds = new System.Windows.Forms.TextBox();
        btnBrowseClientCert = new System.Windows.Forms.Button();
        btnBrowseServerCert = new System.Windows.Forms.Button();
        btnSend = new System.Windows.Forms.Button();
        btnCancel = new System.Windows.Forms.Button();
        btnPrettyJson = new System.Windows.Forms.Button();
        btnCopyCurl = new System.Windows.Forms.Button();
        chkSkipServerValidation = new System.Windows.Forms.CheckBox();
        var requestLayout = new System.Windows.Forms.TableLayoutPanel();
        var actionPanel = new System.Windows.Forms.FlowLayoutPanel();
        var lblUrl = new System.Windows.Forms.Label();
        var lblMethod = new System.Windows.Forms.Label();
        var lblClientCertPath = new System.Windows.Forms.Label();
        var lblClientCertKind = new System.Windows.Forms.Label();
        var lblClientCertPassword = new System.Windows.Forms.Label();
        var lblServerCertPath = new System.Windows.Forms.Label();
        var lblServerCertKind = new System.Windows.Forms.Label();
        var lblHeaders = new System.Windows.Forms.Label();
        var lblContentType = new System.Windows.Forms.Label();
        var lblBody = new System.Windows.Forms.Label();
        var lblTimeout = new System.Windows.Forms.Label();
        var lblStatus = new System.Windows.Forms.Label();
        var responseTopPanel = new System.Windows.Forms.FlowLayoutPanel();
        var lblResponseHeaders = new System.Windows.Forms.Label();
        var lblResponseBody = new System.Windows.Forms.Label();

        SuspendLayout();

        Text = "Client SSL Debug WinForms";
        Width = 1400;
        Height = 900;
        StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

        root.ColumnCount = 1;
        root.RowCount = 2;
        root.Dock = System.Windows.Forms.DockStyle.Fill;
        root.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
        root.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        Controls.Add(root);

        actionPanel.Dock = System.Windows.Forms.DockStyle.Fill;
        actionPanel.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
        actionPanel.Padding = new System.Windows.Forms.Padding(8);

        lblStatus.Text = "Status";
        lblStatus.AutoSize = true;
        lblStatus.Margin = new System.Windows.Forms.Padding(8, 12, 4, 0);
        txtStatus.Width = 180;
        txtStatus.ReadOnly = true;

        btnSend.Text = "送信";
        btnSend.AutoSize = true;
        btnSend.Click += BtnSend_Click;
        btnCancel.Text = "キャンセル";
        btnCancel.AutoSize = true;
        btnCancel.Enabled = false;
        btnCancel.Click += BtnCancel_Click;
        btnPrettyJson.Text = "JSON整形";
        btnPrettyJson.AutoSize = true;
        btnPrettyJson.Click += BtnPrettyJson_Click;
        btnCopyCurl.Text = "curlコピー";
        btnCopyCurl.AutoSize = true;
        btnCopyCurl.Click += BtnCopyCurl_Click;

        actionPanel.Controls.Add(btnSend);
        actionPanel.Controls.Add(btnCancel);
        actionPanel.Controls.Add(btnPrettyJson);
        actionPanel.Controls.Add(btnCopyCurl);
        actionPanel.Controls.Add(lblStatus);
        actionPanel.Controls.Add(txtStatus);
        root.Controls.Add(actionPanel, 0, 0);

        tabs.Dock = System.Windows.Forms.DockStyle.Fill;
        tabs.TabPages.Add(tabRequest);
        tabs.TabPages.Add(tabResponse);
        tabs.TabPages.Add(tabLog);
        root.Controls.Add(tabs, 0, 1);

        tabRequest.Text = "Request";
        tabResponse.Text = "Response";
        tabLog.Text = "Log";

        requestLayout.ColumnCount = 3;
        requestLayout.RowCount = 11;
        requestLayout.Dock = System.Windows.Forms.DockStyle.Fill;
        requestLayout.Padding = new System.Windows.Forms.Padding(12);
        requestLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
        requestLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
        requestLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));

        for (int i = 0; i < 10; i++)
        {
            requestLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
        }
        requestLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        tabRequest.Controls.Add(requestLayout);

        lblUrl.Text = "URL";
        lblUrl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        lblUrl.Dock = System.Windows.Forms.DockStyle.Fill;
        txtUrl.Dock = System.Windows.Forms.DockStyle.Fill;

        lblMethod.Text = "Method";
        lblMethod.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        lblMethod.Dock = System.Windows.Forms.DockStyle.Fill;
        cmbMethod.Dock = System.Windows.Forms.DockStyle.Left;
        cmbMethod.Width = 120;
        cmbMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        cmbMethod.Items.AddRange(new object[] { "POST", "GET", "PUT", "DELETE" });

        lblClientCertPath.Text = "Client Cert";
        lblClientCertPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        lblClientCertPath.Dock = System.Windows.Forms.DockStyle.Fill;
        txtClientCertPath.Dock = System.Windows.Forms.DockStyle.Fill;
        btnBrowseClientCert.Text = "参照";
        btnBrowseClientCert.Dock = System.Windows.Forms.DockStyle.Fill;
        btnBrowseClientCert.Click += BtnBrowseClientCert_Click;

        lblClientCertKind.Text = "Client Cert種別";
        lblClientCertKind.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        lblClientCertKind.Dock = System.Windows.Forms.DockStyle.Fill;
        cmbClientCertKind.Dock = System.Windows.Forms.DockStyle.Left;
        cmbClientCertKind.Width = 120;
        cmbClientCertKind.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        cmbClientCertKind.Items.AddRange(new object[] { "PEM", "PFX/P12", "CER/CRT" });

        lblClientCertPassword.Text = "Client Cert PW";
        lblClientCertPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        lblClientCertPassword.Dock = System.Windows.Forms.DockStyle.Fill;
        txtClientCertPassword.Dock = System.Windows.Forms.DockStyle.Left;
        txtClientCertPassword.Width = 240;
        txtClientCertPassword.UseSystemPasswordChar = true;

        lblServerCertPath.Text = "Server Cert/CA";
        lblServerCertPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        lblServerCertPath.Dock = System.Windows.Forms.DockStyle.Fill;
        txtServerCertPath.Dock = System.Windows.Forms.DockStyle.Fill;
        btnBrowseServerCert.Text = "参照";
        btnBrowseServerCert.Dock = System.Windows.Forms.DockStyle.Fill;
        btnBrowseServerCert.Click += BtnBrowseServerCert_Click;

        lblServerCertKind.Text = "Server Cert種別";
        lblServerCertKind.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        lblServerCertKind.Dock = System.Windows.Forms.DockStyle.Fill;
        cmbServerCertKind.Dock = System.Windows.Forms.DockStyle.Left;
        cmbServerCertKind.Width = 120;
        cmbServerCertKind.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        cmbServerCertKind.Items.AddRange(new object[] { "PEM", "CER/CRT" });

        lblTimeout.Text = "Timeout(秒)";
        lblTimeout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        lblTimeout.Dock = System.Windows.Forms.DockStyle.Fill;
        txtTimeoutSeconds.Dock = System.Windows.Forms.DockStyle.Left;
        txtTimeoutSeconds.Width = 120;

        chkSkipServerValidation.Text = "サーバー証明書検証をスキップ";
        chkSkipServerValidation.AutoSize = true;
        chkSkipServerValidation.Dock = System.Windows.Forms.DockStyle.Left;

        lblHeaders.Text = "Headers";
        lblHeaders.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        lblHeaders.Dock = System.Windows.Forms.DockStyle.Fill;
        txtHeaders.Dock = System.Windows.Forms.DockStyle.Fill;
        txtHeaders.Multiline = true;
        txtHeaders.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        txtHeaders.Text = "Accept: application/json";

        lblContentType.Text = "Content-Type";
        lblContentType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        lblContentType.Dock = System.Windows.Forms.DockStyle.Fill;
        txtContentType.Dock = System.Windows.Forms.DockStyle.Fill;
        txtContentType.Text = "application/x-www-form-urlencoded";

        lblBody.Text = "Body";
        lblBody.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        lblBody.Dock = System.Windows.Forms.DockStyle.Fill;
        txtRequestBody.Dock = System.Windows.Forms.DockStyle.Fill;
        txtRequestBody.Multiline = true;
        txtRequestBody.ScrollBars = System.Windows.Forms.ScrollBars.Both;
        txtRequestBody.WordWrap = false;

        requestLayout.Controls.Add(lblUrl, 0, 0);
        requestLayout.Controls.Add(txtUrl, 1, 0);
        requestLayout.SetColumnSpan(txtUrl, 2);

        requestLayout.Controls.Add(lblMethod, 0, 1);
        requestLayout.Controls.Add(cmbMethod, 1, 1);

        requestLayout.Controls.Add(lblClientCertPath, 0, 2);
        requestLayout.Controls.Add(txtClientCertPath, 1, 2);
        requestLayout.Controls.Add(btnBrowseClientCert, 2, 2);

        requestLayout.Controls.Add(lblClientCertKind, 0, 3);
        requestLayout.Controls.Add(cmbClientCertKind, 1, 3);

        requestLayout.Controls.Add(lblClientCertPassword, 0, 4);
        requestLayout.Controls.Add(txtClientCertPassword, 1, 4);

        requestLayout.Controls.Add(lblServerCertPath, 0, 5);
        requestLayout.Controls.Add(txtServerCertPath, 1, 5);
        requestLayout.Controls.Add(btnBrowseServerCert, 2, 5);

        requestLayout.Controls.Add(lblServerCertKind, 0, 6);
        requestLayout.Controls.Add(cmbServerCertKind, 1, 6);

        requestLayout.Controls.Add(lblTimeout, 0, 7);
        requestLayout.Controls.Add(txtTimeoutSeconds, 1, 7);

        requestLayout.Controls.Add(new System.Windows.Forms.Label { Text = "SSL", Dock = System.Windows.Forms.DockStyle.Fill, TextAlign = System.Drawing.ContentAlignment.MiddleLeft }, 0, 8);
        requestLayout.Controls.Add(chkSkipServerValidation, 1, 8);

        requestLayout.Controls.Add(lblContentType, 0, 9);
        requestLayout.Controls.Add(txtContentType, 1, 9);
        requestLayout.SetColumnSpan(txtContentType, 2);

        var requestSplit = new System.Windows.Forms.SplitContainer();
        requestSplit.Dock = System.Windows.Forms.DockStyle.Fill;
        requestSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
        requestSplit.SplitterDistance = 180;
        requestLayout.Controls.Add(lblHeaders, 0, 10);
        requestLayout.Controls.Add(requestSplit, 1, 10);
        requestLayout.SetColumnSpan(requestSplit, 2);

        var headersPanel = new System.Windows.Forms.Panel { Dock = System.Windows.Forms.DockStyle.Fill };
        headersPanel.Controls.Add(txtHeaders);
        requestSplit.Panel1.Controls.Add(headersPanel);

        var bodyPanel = new System.Windows.Forms.Panel { Dock = System.Windows.Forms.DockStyle.Fill };
        bodyPanel.Controls.Add(txtRequestBody);
        requestSplit.Panel2.Controls.Add(bodyPanel);
        requestSplit.Panel2.Controls.Add(new System.Windows.Forms.Label { Text = "Body", Dock = System.Windows.Forms.DockStyle.Top, Height = 20 });

        tabResponse.Controls.Add(splitResponse);
        splitResponse.Dock = System.Windows.Forms.DockStyle.Fill;
        splitResponse.Orientation = System.Windows.Forms.Orientation.Horizontal;
        splitResponse.SplitterDistance = 220;

        responseTopPanel.Dock = System.Windows.Forms.DockStyle.Top;
        responseTopPanel.Height = 24;
        responseTopPanel.Padding = new System.Windows.Forms.Padding(8, 4, 8, 4);
        lblResponseHeaders.Text = "Response Headers";
        lblResponseHeaders.AutoSize = true;
        responseTopPanel.Controls.Add(lblResponseHeaders);
        txtResponseHeaders.Dock = System.Windows.Forms.DockStyle.Fill;
        txtResponseHeaders.Multiline = true;
        txtResponseHeaders.ScrollBars = System.Windows.Forms.ScrollBars.Both;
        txtResponseHeaders.WordWrap = false;
        splitResponse.Panel1.Controls.Add(txtResponseHeaders);
        splitResponse.Panel1.Controls.Add(responseTopPanel);

        txtResponseBody.Dock = System.Windows.Forms.DockStyle.Fill;
        txtResponseBody.Multiline = true;
        txtResponseBody.ScrollBars = System.Windows.Forms.ScrollBars.Both;
        txtResponseBody.WordWrap = false;
        var responseBodyLabel = new System.Windows.Forms.Label { Text = "Response Body", Dock = System.Windows.Forms.DockStyle.Top, Height = 24, Padding = new System.Windows.Forms.Padding(8, 4, 8, 4) };
        splitResponse.Panel2.Controls.Add(txtResponseBody);
        splitResponse.Panel2.Controls.Add(responseBodyLabel);

        txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
        txtLog.Multiline = true;
        txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
        txtLog.WordWrap = false;
        tabLog.Controls.Add(txtLog);

        ResumeLayout(false);
    }
}
