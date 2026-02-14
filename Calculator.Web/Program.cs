using Calculator.Domain;
using Calculator.Infrastructure;
using Calculator.UseCase;
using Calculator.Web.Components;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------
// サービスの登録
// ---------------------------

// Razor Components の追加（Blazor Server コンポーネントを使用可能にする）
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// ユースケースクラスの DI 登録（Scoped：HTTP リクエスト単位でインスタンス生成）
builder.Services.AddScoped<CalculatorUseCase>();

// リポジトリクラスの DI 登録（Interface → 実装クラスを紐付け）
builder.Services.AddScoped<ICalculationHistoryRepository, CalculationHistoryRepository>();

// DB 接続設定（PostgreSQL）
// 接続文字列は appsettings.json または User Secrets から取得
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

var app = builder.Build();

// ---------------------------
// HTTP リクエストパイプラインの構成
// ---------------------------

// 本番環境での例外ハンドリングと HSTS の設定
if (!app.Environment.IsDevelopment())
{
    // エラー発生時は /Error ページにリダイレクト
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // HSTS（HTTP Strict Transport Security）の有効化
    app.UseHsts();
}

// ステータスコードごとのページ再実行（404 など）
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

// HTTPS リダイレクト
app.UseHttpsRedirection();

// CSRF 対策のアンチフォージェリを有効化
app.UseAntiforgery();

// 静的ファイル（CSS/JS など）の配信を有効化
app.MapStaticAssets();

// Razor Components のルーティング設定とインタラクティブサーバーモードを追加
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// アプリケーションの起動
app.Run();
