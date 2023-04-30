using ServiceContract;
using Services;
using StockPrice;

var builder = WebApplication.CreateBuilder(args);
//services
builder.Services.AddControllersWithViews();
builder.Services.Configure<StockOptions>(builder.Configuration.GetSection("StockOptions"));
builder.Services.AddSingleton<IFinnhubService, FinnhubService>();

builder.Services.AddHttpClient();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
