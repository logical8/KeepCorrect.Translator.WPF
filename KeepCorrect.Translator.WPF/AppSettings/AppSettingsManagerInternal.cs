using System;
using System.Configuration;

namespace KeepCorrect.Translator.WPF.AppSettings;

internal static class AppSettingsManagerInternal
{
    public static string? ReadSetting(string key)  
    {  
        try  
        {  
            var appSettings = ConfigurationManager.AppSettings;  
            return appSettings[key] ?? default;  
        }  
        catch (ConfigurationErrorsException)  
        {  
            Console.WriteLine("Error reading app settings");  
            throw;
        }  
    }  
        
    public static void AddUpdateAppSettings(string key, string value)  
    {  
        try  
        {  
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);  
            var settings = configFile.AppSettings.Settings;  
            if (settings[key] == null)  
            {  
                settings.Add(key, value);  
            }  
            else  
            {  
                settings[key].Value = value;  
            }  
            configFile.Save(ConfigurationSaveMode.Modified);  
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);  
        }  
        catch (ConfigurationErrorsException)  
        {  
            Console.WriteLine("Error writing app settings");  
        }  
    }  
}