using Microsoft.EntityFrameworkCore;
using PersonalApi.Datenbank;
using System.ComponentModel;
using System;
using PersonalApi.Zuordnung;

namespace PersonalApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Datenbankkontext zum Dependency Injection Container hinzufügen.
        // Er liest den Connection-String aus der appsettings.json aus.
        builder.Services.AddDbContext<PersonalKontext>(
            optionen =>  optionen.UseSqlServer(builder.Configuration.GetConnectionString("PersonalDatenbankVerbindung")));

        builder.Services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<PersonalProfil>();
        });

        // Controller registrieren
        builder.Services.AddControllers();

        // Swagger registrieren
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        var app = builder.Build();

        // Swagger aktivieren
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}