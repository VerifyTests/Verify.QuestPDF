using System.Diagnostics.CodeAnalysis;

namespace VerifyTests;

public static class QuestPDFSettings
{
    public static void PagesToInclude(this VerifySettings settings, int count) =>
        settings.Context["QuestPDF.PagesToInclude"] = count;

    public static SettingsTask PagesToInclude(this SettingsTask settings, int count)
    {
        settings.CurrentSettings.PagesToInclude(count);
        return settings;
    }

    internal static bool GetPagesToInclude(this IReadOnlyDictionary<string, object> context, [NotNullWhen(true)] out int? pages)
    {
        if (context.TryGetValue("QuestPDF.PagesToInclude", out var value))
        {
            pages = (int) value;
            return true;
        }

        pages = null;
        return false;
    }
}