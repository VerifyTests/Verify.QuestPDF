namespace VerifyTests;

public static class QuestPDFSettings
{
    /// <summary>
    /// Limits the rendered <c>png</c> page snapshots to the first <paramref name="count"/> pages.
    /// The <c>pdf</c> snapshot is unaffected and always contains the full source document.
    /// </summary>
    public static void PagesToInclude(this VerifySettings settings, int count) =>
        settings.PagesToInclude(_ => _ <= count);

    /// <inheritdoc cref="PagesToInclude(VerifySettings, int)"/>
    public static SettingsTask PagesToInclude(this SettingsTask settings, int count)
    {
        settings.CurrentSettings.PagesToInclude(_ => _ <= count);
        return settings;
    }

    /// <summary>
    /// Limits the rendered <c>png</c> page snapshots to the pages for which <paramref name="include"/>
    /// returns true. The <c>pdf</c> snapshot is unaffected and always contains the full source
    /// document.
    /// </summary>
    public static void PagesToInclude(this VerifySettings settings, ShouldIncludePage include) =>
        settings.Context["QuestPDF.PagesToInclude"] = include;

    /// <inheritdoc cref="PagesToInclude(VerifySettings, ShouldIncludePage)"/>
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