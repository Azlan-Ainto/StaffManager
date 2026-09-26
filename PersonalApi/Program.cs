using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using PersonalApi.Datenbank;
using PersonalApi.Geschaeftslogik;
using PersonalApi.Repositories;
using PersonalApi.Zuordnung;
using System.Text;

namespace PersonalApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // ------------------------------------------------------------
        // Dependency Injection
        // ------------------------------------------------------------

        builder.Services.AddScoped<IPersonalRepository, PersonalRepository>();

        builder.Services.AddScoped<IPersonalService, PersonalService>();

        builder.Services.AddDbContext<PersonalKontext>(
            optionen =>
                optionen.UseSqlServer(
                    builder.Configuration.GetConnectionString(
                        "PersonalDatenbankVerbindung"
                    )
                )
        );

        // ------------------------------------------------------------
        // JWT-Authentifizierung
        // ------------------------------------------------------------

        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(
                optionen =>
                {
                    optionen.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer =
                            builder.Configuration["JwtEinstellungen:Aussteller"],

                        ValidAudience =
                            builder.Configuration["JwtEinstellungen:Zuschauer"],

                        IssuerSigningKey =
                            new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(
                                    builder.Configuration[
                                        "JwtEinstellungen:Sicherheitsschluessel"
                                    ]!
                                )
                            )
                    };
                }
            );

        // ------------------------------------------------------------
        // AutoMapper
        // ------------------------------------------------------------

        builder.Services.AddAutoMapper(
            cfg =>
            {
                cfg.AddProfile<PersonalProfil>();
            }
        );

        // ------------------------------------------------------------
        // Controller
        // ------------------------------------------------------------

        builder.Services.AddControllers(
            optionen =>
            {
                optionen.SuppressAsyncSuffixInActionNames = false;
            }
        );

        // ------------------------------------------------------------
        // Swagger
        // ------------------------------------------------------------

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(
            options =>
            {
                options.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "Gib deinen JWT Token ein."
                    }
                );

                options.AddSecurityRequirement(
                    document =>
                        new OpenApiSecurityRequirement
                        {
                            [new OpenApiSecuritySchemeReference(
                                "Bearer",
                                document
                            )] = []
                        }
                );
            }
        );

        // ------------------------------------------------------------
        // Anwendung erstellen
        // ------------------------------------------------------------

        var app = builder.Build();

        // ------------------------------------------------------------
        // Swagger
        // ------------------------------------------------------------

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Fehler");
        }

        // ------------------------------------------------------------
        // HTTPS
        // ------------------------------------------------------------

        app.UseHttpsRedirection();

        // ------------------------------------------------------------
        // Authentifizierung + Autorisierung
        // ------------------------------------------------------------

        app.UseAuthentication();

        app.UseAuthorization();

        // ------------------------------------------------------------
        // Controller
        // ------------------------------------------------------------

        app.MapControllers();

        // ------------------------------------------------------------
        // Anwendung starten
        // ------------------------------------------------------------

        app.Run();
    }
}