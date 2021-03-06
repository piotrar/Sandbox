﻿using System;
using System.IO;
using JetBrains.Annotations;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json.Linq;


namespace ErikTheCoder.Sandbox.Dapper.Service
{
    [UsedImplicitly]
    public static class Program
    {
        public static IAppSettings AppSettings { get; private set; }


        public static void Main(string[] Args)
        {
            // Parse configuration file.
            AppSettings = ParseConfigurationFile();
            // Build and run web host.
            IWebHostBuilder webHostBuilder = WebHost.CreateDefaultBuilder(Args);
            webHostBuilder.UseKestrel();
            webHostBuilder.UseStartup<Startup>();
            IWebHost webHost = webHostBuilder.Build();
            webHost.Run();
        }


        private static IAppSettings ParseConfigurationFile()
        {
            const string environmentalVariableName = "ASPNETCORE_ENVIRONMENT";
            string environment = Environment.GetEnvironmentVariable(environmentalVariableName) ?? "Dev";
            string directory = Path.GetDirectoryName(typeof(Program).Assembly.Location) ?? string.Empty;
            string configurationFile = Path.Combine(directory, "appSettings.json");
            if (!File.Exists(configurationFile)) throw new Exception($"Configuration file not found at {configurationFile}.");
            JObject configuration = JObject.Parse(File.ReadAllText(configurationFile));
            return configuration.GetValue(environment).ToObject<AppSettings>();
        }
    }
}
