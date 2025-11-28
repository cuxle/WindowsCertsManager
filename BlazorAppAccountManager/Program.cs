using BlazorAppAccountManager.Components;
using BlazorAppAccountManager.Components.Account;
using BlazorAppAccountManager.Components.Configs;
using BlazorAppAccountManager.Components.Data;
using BlazorAppAccountManager.Components.Services;
using Blazored.LocalStorage;
using MatBlazor;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Threading.Tasks;


var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services));

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<SupplierService>();
builder.Services.AddScoped<CertificateService>();
builder.Services.AddScoped<PdfProcessingService>();
builder.Services.AddScoped<ProjectService>();

builder.Services.AddScoped<CertificateReferenceService>();


builder.Services.Configure<ModelGlobalSettings>(
    builder.Configuration.GetSection("ModelGlobalSettings")
);

builder.Services.Configure<TesseractConfig>(builder.Configuration.GetSection("Tesseract"));


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// 1. 注册 HTTP 客户端（用于 CherryGPT 调用）
builder.Services.AddHttpClient();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireRole("Admin"));
});

// Register MySQL DbContext using Pomelo provider
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("MySqlConnection") 
        ?? throw new InvalidOperationException("Connection string 'MySqlConnection' not found.");
    
    // Configure MySQL provider
    options.UseMySql(
        connectionString,
        new MySqlServerVersion(new Version(8, 0, 43)) // MySQL version
    );
    
    // Enable sensitive data logging only in development environment
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
    }
}, ServiceLifetime.Scoped);

// Register IDbContextFactory
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("MySqlConnection") 
        ?? throw new InvalidOperationException("Connection string 'MySqlConnection' not found.");
    
    options.UseMySql(
        connectionString,
        new MySqlServerVersion(new Version(8, 0, 43)) // Same version as above
    );
}, ServiceLifetime.Scoped);

builder.Services.AddMatBlazor();

builder.Services.AddMatToaster(config =>
{
    config.Position = MatToastPosition.BottomRight;
    config.PreventDuplicates = true;
    config.NewestOnTop = true;
    config.ShowCloseButton = true;
    config.MaximumOpacity = 95;
    config.VisibleStateDuration = 3000;
});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();


// Correct and single registration for Identity services.
// It includes both ApplicationUser and IdentityRole for user and role management.
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager<SignInManager<ApplicationUser>>() // Add SignInManager for proper sign-in functionality
    .AddDefaultTokenProviders();

var app = builder.Build();

async Task InitializeIdentityData(IServiceProvider services)
{
    using (var scope = services.CreateScope())
    {
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // Define roles
        string[] roleNames = { "SuperAdmin", "Admin", "User" };
        IdentityResult roleResult;

        foreach (var roleName in roleNames)
        {
            // Create role if it doesn't exist
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                if (!roleResult.Succeeded)
                {
                    Log.Error("Failed to create role {RoleName}: {Errors}", roleName, string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                }
            }
        }

        // Create initial SuperAdmin user if it doesn't exist
        // NOTE: In production, use environment variables or secure configuration for passwords
        var superAdminUsername = builder.Configuration["InitialUser:Username"] ?? "superadmin";
        var superAdminEmail = builder.Configuration["InitialUser:Email"] ?? "superadmin@example.com";
        var superAdminPassword = builder.Configuration["InitialUser:Password"] ?? "ChangeThisInProduction123!";
        
        var superAdminUser = await userManager.FindByNameAsync(superAdminUsername);
        
        if (superAdminUser == null)
        {
            superAdminUser = new ApplicationUser 
            {
                UserName = superAdminUsername,
                Email = superAdminEmail,
                EmailConfirmed = true
            };
            
            var createUserResult = await userManager.CreateAsync(superAdminUser, superAdminPassword);
            
            if (createUserResult.Succeeded)
            {
                await userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
                Log.Information("Created initial SuperAdmin user: {Username}", superAdminUsername);
            }
            else
            {
                Log.Error("Failed to create SuperAdmin user: {Errors}", string.Join(", ", createUserResult.Errors.Select(e => e.Description)));
            }
        }
    }
}

// Run initialization
await InitializeIdentityData(app.Services);

// Configure the HTTP request pipeline.
try
{
    Log.Information("Application starting up...");

    // Configure environment-specific settings
    if (app.Environment.IsDevelopment())
    {
        app.UseMigrationsEndPoint();
    }
    else
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        // The default HSTS value is 30 days. You may want to change this for production scenarios
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthorization();
    app.UseAntiforgery();

    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();

    // Add additional endpoints required by the Identity /Account Razor components
    app.MapAdditionalIdentityEndpoints();

    Log.Information("Application started successfully.");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
    throw; // Re-throw to ensure proper exit code
}
finally
{
    Log.CloseAndFlush();
}
