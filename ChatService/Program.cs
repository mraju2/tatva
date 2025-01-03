using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Cryptography;
using ChatService.Repositories;
using ChatService.Services.Interfaces;
using ChatService.Services;
using ChatService.Utilities;
using ChatService.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Register HttpClient for ChatMessageService
builder.Services.AddHttpClient<IChatMessageService, ChatMessageService>();

// Register DatabaseRepository with DI using local connection string from appsettings.json
builder.Services.AddSingleton<IDatabaseRepository>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    return new DatabaseRepository(connectionString);
});

// Configure MongoDbSettings and MongoRepository
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDb"));
builder.Services.AddSingleton<IMongoRepository, MongoRepository>();

// Register MarkdownTableCreator
builder.Services.AddSingleton<MarkdownTableCreator>();

// Configure CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // Allow the frontend origin
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configure Authentication using JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    // Retrieve public key from appsettings.json
    string pubKey = builder.Configuration["Jwt:PublicKey"] ?? "";
    RSA rsa = RSA.Create();
    rsa.ImportSubjectPublicKeyInfo(Convert.FromBase64String(pubKey), out _);
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true, // Validate token expiration
        ClockSkew = TimeSpan.Zero, // Reduce clock skew tolerance
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new RsaSecurityKey(rsa),
        RequireSignedTokens = true
    };
});

// Add Swagger/OpenAPI with JWT security
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and your token."
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Add Health Checks
builder.Services.AddHealthChecks();
builder.Logging.AddConsole();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); // Enforce HTTPS
app.UseCors("AllowFrontend"); // Enable CORS
app.UseAuthentication(); // Add Authentication Middleware
app.UseAuthorization(); // Add Authorization Middleware
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
