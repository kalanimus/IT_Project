using Infrastructure.Data;
using Infrastructure.Parsers;
using Infrastructure.Repositories;
using Application.Services; // Для сервисов из Application
using Core.Interfaces; // Для интерфейсов репозиториев
using Microsoft.EntityFrameworkCore;
using Infrastructure.Seeders; // Для DataSeeder
using Application.Mappings;
using AutoMapper; // Для регистрации AutoMapper
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using API.Middleware;
using System.Text;
using Microsoft.OpenApi.Models; // Добавлено для Swagger
using System.Reflection; // Добавлено для XML-документации

var builder = WebApplication.CreateBuilder(args);

// Регистрация контекста базы данных
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Регистрация репозиториев
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IGroupTeacherRepository, GroupTeacherRepository>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();

// Регистрация сервисов
builder.Services.AddScoped<IUserService, UserService>(); // UserService из Application.Services
builder.Services.AddScoped<IGroupService, GroupService>(); // GroupService из Application.Services
builder.Services.AddScoped<IPermissionService, PermissionService>(); // PermissionService из Application.Services
builder.Services.AddScoped<IRoleService, RoleService>(); // RoleService из Application.Services
builder.Services.AddScoped<ISubjectService, SubjectService>(); // SubjectService из Application.Services
builder.Services.AddScoped<IUsernameGeneratorService, UsernameGeneratorService>();
builder.Services.AddScoped<ITransliterationService, TransliterationService>();
builder.Services.AddScoped<IScheduleService, ScheduleService>();
builder.Services.AddScoped<IScheduleParser, ScheduleParser>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddTransient<Random>();

// Регистрация AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile)); // Убедитесь, что у вас есть MappingProfile

// Регистрация контроллеров
builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Администратор"));
});

builder.Services.AddAuthorization();

// === Добавление Swagger ===
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Schedule API",
        Version = "v1",
        Description = "API для управления расписанием и аутентификации",
    });

    // Включение детализации ошибок (если используются исключения)
    // c.OperationFilter<DefaultResponseOperationFilter>(); // Показывает стандартные ошибки (400, 500 и т.д.)

    // JWT в Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });

    // XML-документация (если включена в .csproj)
    // var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    // var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    // if (File.Exists(xmlPath))
    // {
    //     c.IncludeXmlComments(xmlPath);
    // }
});

var app = builder.Build();

// Настройка middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Вызов сидера для заполнения БД начальными данными
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();

    // Примените миграции (если они еще не применены)
    context.Database.Migrate();

    // Заполните БД начальными данными
    DataSeeder.SeedData(context);
}

// Swagger (работает в любом окружении)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Schedule API v1");
    c.RoutePrefix = "swagger"; // Доступ по /swagger
});


app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
// Регистрация middleware для обработки исключений
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();

