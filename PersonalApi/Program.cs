using Microsoft.EntityFrameworkCore;
using PersonalApi.Datenbank;
using System.ComponentModel;
using System;
using PersonalApi.Zuordnung;
using PersonalApi.Repositories;
using PersonalApi.Geschaeftslogik;

namespace PersonalApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // NEU: Repository im DI-Container registrieren (Scoped = Einmal pro HTTP-Request)
        builder.Services.AddScoped<IPersonalRepository, PersonalRepository>();

        builder.Services.AddScoped<IPersonalService, PersonalService>();

        // Datenbankkontext zum Dependency Injection Container hinzufügen.
        // Er liest den Connection-String aus der appsettings.json aus.
        builder.Services.AddDbContext<PersonalKontext>(optionen =>  optionen.UseSqlServer(builder.Configuration.GetConnectionString("PersonalDatenbankVerbindung")));

        builder.Services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<PersonalProfil>();
        });

        // Controller registrieren
        builder.Services.AddControllers(optionen =>
        {
            // ASP.NET Core entfernt standardmäßig die Endung "Async" aus den Aktionsnamen
            // (aus "Mitarbeiter_Abrufen_Async" wird "Mitarbeiter_Abrufen_").
            // Dann findet CreatedAtAction(nameof(..._Async), ...) keine passende Route.
            // Mit "false" bleiben die Aktionsnamen identisch mit den Methodennamen.
            optionen.SuppressAsyncSuffixInActionNames = false;
        });

        // Swagger registrieren
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        var app = builder.Build();

        // Swagger aktivieren
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


        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}