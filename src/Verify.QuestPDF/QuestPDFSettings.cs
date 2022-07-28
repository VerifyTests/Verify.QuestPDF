using System.Diagnostics.CodeAnalysis;
using VerifyQuestPDF;

namespace VerifyTests;

public static class QuestPDFSettings
{
    public static void PagesToInclude(this VerifySettings settings, int count) =>
        settings.PagesToInclude(_ => _ <= count);

    public static SettingsTask PagesToInclude(this SettingsTask settings, int count)
    {
        settings.CurrentSettings.PagesToInclude(_ => _ <= count);
        return settings;
    }

    public static void PagesToInclude(this VerifySettings settings, ShouldIncludePage include) =>
        settings.Context["QuestPDF.PagesToInclude"] = include;

    public static SettingsTask PagesToInclude(this SettingsTask settings, ShouldIncludePage include)
    {
        settings.CurrentSettings.PagesToInclude(include);
        return settings;
    }

    internal static bool GetPagesToInclude(this IReadOnlyDictionary<string, object> context, [NotNullWhen(true)] out ShouldIncludePage? include)
    {
        if (context.TryGetValue("QuestPDF.PagesToInclude", out var value))
        {
            include = (ShouldIncludePage) value;
            return true;
        }

        include = null;
        return false;
    }
}