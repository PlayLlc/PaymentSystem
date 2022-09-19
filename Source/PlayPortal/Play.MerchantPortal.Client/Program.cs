var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
//Configure authentication for our merchant portal client
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "cookie";
    options.DefaultSignInScheme = "cookie";
    options.DefaultChallengeScheme = "oidc";
})
.AddCookie("cookie")
.AddOpenIdConnect("oidc", options =>
{
    options.Authority = "https://localhost:7191";
    options.ClientId = "merchantportal_client";
    options.ClientSecret = "cb17c97c-0910-41c0-aafb-2b77a5838852"; //this is a confidential client, meaning we can take advantage of knowing the secret on the client as well.
    options.ResponseType = "code";
    options.ResponseMode = "form_post"; //we take advatange of Id server tls guarding (extra security).
    options.CallbackPath = "/signin-oidc";
    options.SaveTokens = true;

    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("merchantportal");

    //Enable PKCE(authorization code flow only) . We use it for nonce transaction behaviour thus disabling replays.
    options.UsePkce = true;
});

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
