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

    /// <summary>
    /// Snapshots the pdf bytes exactly as produced, skipping the normalization that neutralizes the
    /// trailer <c>/ID</c>, the <c>/CreationDate</c> and <c>/ModDate</c>, and the XMP dates and
    /// identifiers. Use it when the producer already emits byte-deterministic documents, since
    /// normalizing them again copies the whole buffer, rescans it, and — when the XMP packet is
    /// canonicalized — rebuilds it and repairs the cross-reference table, all to change nothing.
    /// </summary>
    /// <remarks>
    /// The metadata dates are already pinned to a fixed value before generation, so QuestPDF output
    /// is close to deterministic already. The normalization additionally covers the trailer
    /// <c>/ID</c> and any XMP packet, should a future QuestPDF or Skia version start emitting them.
    /// <para>
    /// The XMP canonicalization is worth calling out because it is the pass that changes bytes for
    /// an already-deterministic producer: it collapses the packet's whitespace, so enabling or
    /// disabling this setting on an existing suite shifts the stored <c>.verified.pdf</c> even
    /// though nothing about the document changed. Expect to re-accept those snapshots once.
    /// </para>
    /// </remarks>
    public static void SkipPdfNormalization(this VerifySettings settings) =>
        settings.Context["QuestPDF.SkipNormalization"] = true;

    /// <inheritdoc cref="SkipPdfNormalization(VerifySettings)"/>
    public static SettingsTask SkipPdfNormalization(this SettingsTask settings)
    {
        settings.CurrentSettings.SkipPdfNormalization();
        return settings;
    }

    internal static bool Normalize(this IReadOnlyDictionary<string, object> context) =>
        !context.TryGetValue("QuestPDF.SkipNormalization", out var value) ||
        value is not true;
}
