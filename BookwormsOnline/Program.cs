using BookwormsOnline.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using GoogleReCaptcha.V3;
using GoogleReCaptcha.V3.Interface;
using Ganss.Xss;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<AuthDbContext>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>

{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;

    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 12;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
    options.Lockout.MaxFailedAccessAttempts = 3;
}

).AddEntityFrameworkStores<AuthDbContext>();
builder.Services.AddDataProtection();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Login";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(1); // Set the desired timeout here
    options.SlidingExpiration = true; // This will reset the expiration time if the user is active
    options.Cookie.HttpOnly = true; // Prevents access to the cookie from client-side scripts
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Ensures the cookie is only sent over HTTPS
});



builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddHttpClient<ICaptchaValidator, GoogleReCaptchaValidator>();
builder.Services.AddDistributedMemoryCache(); //save session in memory
//session management    
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(20);
    options.Cookie.IsEssential = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    // Ensure the session cookie is sent with every request
});

// For the Prevent Injection (e.g SQLi, CSRF and XSS attack.
builder.Services.AddSingleton<HtmlSanitizer>();



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

// For directing to 404 page
app.UseStatusCodePagesWithRedirects("/errors/{0}");


// For 500 error
app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == 500)
    {
        context.Response.Redirect("/errors/500");
    }
});


app.UseSession();
app.UseRouting();


app.UseAuthorization();

app.MapRazorPages();

app.Run();

//CSP for XSS Attacks
app.Use(async (context, next) =>
{
	context.Response.Headers.Add("Content-Security-Policy",
								 "default-src 'self'; " +
								 "script-src 'self' 'unsafe-inline' 'unsafe-eval' https://www.google.com https://www.gstatic.com; " +
								 "style-src 'self' 'unsafe-inline' https://fonts.googleapis.com; " +
								 "font-src 'self' https://fonts.gstatic.com; " +
								 "img-src 'self' data:; " +
								 "frame-src 'self' https://www.google.com; " +
								 "connect-src 'self' wss://localhost:44331 wss://localhost:44332 wss://localhost:44356 wss://localhost:44392 wss://localhost:44347");
	await next();
});

