# Semantic Kernel Demo
Semantic Kernel の Planner, Plugin, Kernel を試しに使ってみるためのサンプルコードです。

# 事前準備
事前に必要なクラウド上のリソースは以下の通りです。
- Azure Subscription
- Azure OpenAI Account
- Azure OpenAI の ChatGPT のデプロイメント
  - デプロイメント名
  - キー
  - API エンドポイント URL
  
# How to use
1. まずはこのリポジトリをフォーク
2. ローカルにクローンして VSCode で開く
3. 以下のコマンドを打ってシークレットストアを作成
   1. dotnet user-secrets init
4. その後シークレットストアに Azure OpenAI に繋ぎに行くための情報を格納
   1. dotnet user-secrets set "DeployName" "デプロイ名"
   2. dotnet user-secrets set "Endpoint" "エンドポイント URL"
   3. dotnet user-secrets set "APIKey" "キー"
5. VSCode の「デバッグ」から実行
