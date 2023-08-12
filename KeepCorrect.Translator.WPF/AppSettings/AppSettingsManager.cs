using System;

namespace KeepCorrect.Translator.WPF.AppSettings;

public static class AppSettingsManager
{
    public static bool ShowSourceText => Get<bool>(AppSettingKeyEnum.ShowSourceText);

    private static T? Get<T>(AppSettingKeyEnum appSettingKey)
    {
        var key = appSettingKey.ToString();
        var result = AppSettingsManagerInternal.ReadSetting(key);
        if(typeof(T) == typeof(bool))
            return (T)(object)(result == null || bool.Parse(result));
        return result == null ? default : (T)(object)result;
    }

    public static void Set<T>(AppSettingKeyEnum appSettingKey, T value)
    {
        if (value == null) throw new InvalidOperationException();
        AppSettingsManagerInternal.AddUpdateAppSettings(appSettingKey.ToString(), value.ToString()!);
    }
}