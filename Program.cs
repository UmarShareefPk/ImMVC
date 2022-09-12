using IM.Helper;
using IM.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddSessionStateTempDataProvider();
builder.Services.AddSession(
//options =>
//{
//    options.IdleTimeout = TimeSpan.FromMinutes(60);
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;

//}
);

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    // options.DefaultChallengeScheme = "facebook";//FacebookDefaults.AuthenticationScheme;
    // options.DefaultChallengeScheme = "GoogleOpenID";// GoogleDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/Account/login";
    options.AccessDeniedPath = "/Account/login";
    options.Events = new CookieAuthenticationEvents()
    {
        OnSigningIn = async context =>
        {
            var scheme = context.Properties.Items.Where(k => k.Key == ".AuthScheme").FirstOrDefault();
            var claim = new Claim(scheme.Key, scheme.Value);
            var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
            claimsIdentity.AddClaim(claim);
            await Task.CompletedTask;
        }
    };
});

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddSession(
    //options =>
    //{
    //    options.IdleTimeout = TimeSpan.FromMinutes(60);
    //    options.Cookie.HttpOnly = true;
    //    options.Cookie.IsEssential = true;
    
    //}
);

builder.Services.AddHttpClient();

builder.Services.AddScoped<ICommon, Common>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IIncidentService, IncidentService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}",
    defaults: "{controller=Account}/{action=Login}"
    );

app.Run();
