var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var app = builder.Build();

app.UseStaticFiles();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();