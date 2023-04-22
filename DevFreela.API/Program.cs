using DevFreela.API.Filters;
using DevFreela.API.Models;
using DevFreela.Application.Commands.CreateProject;
using DevFreela.Application.Services.Implementations;
using DevFreela.Application.Services.Interfaces;
using DevFreela.Application.Validators;
using DevFreela.Core.Repositories;
using DevFreela.Core.Services.Interfaces;
using DevFreela.Infrastructure.Auth;
using DevFreela.Infrastructure.Persistence;
using DevFreela.Infrastructure.Persistence.Repositories;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Pegando uma cessão do arquivo de configuração, criando uma instancia e disponibilizando no projeto
builder.Services.Configure<OpeningTimeOption>(builder.Configuration.GetSection("OpeningTime"));

// Exemplo Injeção de Dependencia
//services.AddSingleton<ExampleClass>(e => new ExampleClass { Name = "Initial Stage" });
builder.Services.AddScoped<ExampleClass>(e => new ExampleClass { Name = "Initial Stage" });

// Utilizando um banco local
// builder.Services.AddSingleton<DevFreelaDbContext>();

// Utilizando um banco SqlServer
var connectionString = builder.Configuration.GetConnectionString("DevFreelaCs");
builder.Services.AddDbContext<DevFreelaDbContext>(options => options.UseSqlServer(connectionString));


// É possivel criar as injeções de Dependencias em uma classe especifica de extensions
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISkillService, SkillService>();

builder.Services.AddScoped<IProjectRepository, ProjectRepositoy>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ISkillRepository, SkillRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();

// Para configurar o Filter passamos o tipo dele na listagem de filters das controllers
builder.Services.AddControllers(options => options.Filters.Add(typeof(ValidatorFilter)))
    //Configurando todas classes de Validators que estão no mesmo Assembly/Projeto que o Validator passado como parametro
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateProjectCommandValidator>());

// Busca por todas classes/commands que implementem IRequest e associalos a todos commandsHandlers que implementem IRequestHandler
// Obs.: Ele ja registra TODOS Commands/Queries e seus Handlers
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateProjectCommand).Assembly));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services
    .AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "DevFreela.API", Version = "v1" });

        // Definicao de Seguranca - Tipo Bearer
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            // Schema de Seguranca
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Authorization header usando o esquema Bearer."
        });

        // Falando que precisa utilizar o authorization
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
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

builder.Services
    // Schema de Autenticacao (Bearer)
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // O que vai ser validado quando o usuario enviar o token para o servidor
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Autenticacao e autorizaçao (NESSA ORDEM!!!)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
