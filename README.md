# ClientSslDebugWinForms

クライアント SSL 認証付きの HTTP 通信を WinForms から手動送信してデバッグするためのサンプルです。

## 主な機能
- URL / Method / Header / Body を画面から編集
- クライアント証明書の指定（PEM / PFX / CER/CRT）
- サーバー証明書または CA 証明書ファイルの指定
- レスポンスヘッダー / ボディ表示
- ログ表示
- curl コマンド生成
- JSON 整形

## 想定環境
- Windows 11
- .NET 8 SDK

## 実行方法
```powershell
cd .\ClientSslDebugWinForms
dotnet run
```

## 補足
- PEM のクライアント証明書は秘密鍵が別ファイルの場合、このサンプルのままでは読み込めません。必要なら `CreateFromPemFile(certPath, keyPath)` へ拡張してください。
- サーバー証明書検証スキップはデバッグ専用です。本番では無効化してください。
