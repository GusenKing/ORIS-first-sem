using System.Text.Json;
using MyHTTPServer.Configuration;
using MyHTTPServer.Services;

namespace MyHTTPServer;

public static class ConfigLoader
{
    const string configFilePath = @".\appsettings.json";
    
    public static AppSettingConfig Config { get; private set; }
    public static EmailSenderService EmailSender { get; private set; }
    
    private static void CheckConfig()
    {
        if (!File.Exists(configFilePath))
        {
            Console.WriteLine($"File {configFilePath} not found");
            throw new Exception();
        }
    }

    static ConfigLoader()
    {
        CheckConfig();
        var config = new AppSettingConfig();
        using (StreamReader jsonStream = new StreamReader(configFilePath))
        {
            config = JsonSerializer.Deserialize<AppSettingConfig>(jsonStream.BaseStream);
        }
        using (StreamReader jsonStream = new StreamReader(configFilePath))
        {
            EmailSender = JsonSerializer.Deserialize<EmailSenderService>(jsonStream.BaseStream);
        }
        if (!Directory.Exists(config.StaticFilesPath))
        {
            Directory.CreateDirectory(config.StaticFilesPath);
        }
        
        if (!File.Exists(config.StaticFilesPath + "\\" + "index.html"))
        {
            Console.WriteLine("Файл index.html не найден");
        }

        Config = config;
    }
}