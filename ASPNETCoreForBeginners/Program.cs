using ASPNETCoreForBeginners.Authentication;
using ASPNETCoreForBeginners.Authorization;
using ASPNETCoreForBeginners.Data;
using ASPNETCoreForBeginners.Entities;
using ASPNETCoreForBeginners.Filters;
using ASPNETCoreForBeginners.Middleware;
using ASPNETCoreForBeginners.Middlewares;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(cfg => cfg.AddDebug());

builder.Services.AddDbContext<ApplicationDbContext>(cfg =>
cfg.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.Configure<AttachmentOptions>(builder.Configuration.GetSection("Attachments"));

//var attachmentOptions = builder.Configuration.GetSection("Attachments").Get<AttachmentOptions>();
//builder.Services.AddSingleton(attachmentOptions);

//var attachmentOptions = new AttachmentOptions();
//builder.Configuration.GetSection("Attachments").Bind(attachmentOptions);
//builder.Services.AddSingleton(attachmentOptions);


// Add services to the container.

//builder.Configuration.AddJsonFile("config.json");

builder.Services.AddControllers(options =>
{
    options.Filters.Add<LogActivityFilter>();
    options.Filters.Add<PermissionBasedAuthorizationFilter>();
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();



var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();
builder.Services.AddSingleton(jwtOptions);


builder.Services.AddSingleton<IAuthorizationHandler, AgeAuthorizationHeadler>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AgeGreaterThan25",
        builder => builder.AddRequirements(new AgeGreaterThan25Requirment()));


    //options.AddPolicy("SuperUsersOnly", builder =>
    //{
    //    builder.RequireRole("SuperUser", "Admin");
    //});

    //options.AddPolicy("EmployeesOnly", builder =>
    //{
    //    builder.RequireClaim("UserTypes", "Employee");
    //});
});


builder.Services.AddAuthentication()
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),
        };
    });
// using Basic Authentication
//.AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", null);



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseMiddleware<RateLimitingMaddleware>();
//app.UseMiddleware<ProfilingMaddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
