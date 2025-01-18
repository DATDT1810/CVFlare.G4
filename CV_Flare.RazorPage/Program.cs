using CV_Flare.RazorPage.Helper;
using CV_Flare.RazorPage.Service;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()  // Cho phép tất cả các nguồn
                   .AllowAnyMethod()  // Cho phép tất cả các phương thức (GET, POST, v.v.)
                   .AllowAnyHeader(); // Cho phép tất cả các tiêu đề
        });
});
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<TokenServices>();
builder.Services.AddTransient<TokenHandler>();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient("DefaultClient").AddHttpMessageHandler<TokenHandler>();
builder.Services.AddHttpContextAccessor();

// Cấu hình cho Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian hết hạn Session
    options.Cookie.HttpOnly = true; // Đảm bảo chỉ có thể truy cập Session cookie qua HTTP
    options.Cookie.IsEssential = true; // Cần thiết cho hoạt động của ứng dụng
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();
app.MapRazorPages();

app.Run();
