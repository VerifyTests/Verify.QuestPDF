namespace VerifyTests;

/// <summary>
/// Controls which outputs a QuestPDF document is split into when verified.
/// The pdf target is not controlled here; use <c>VerifierSettings.ExcludeTargets("pdf")</c> for that.
/// </summary>
[Flags]
public enum QuestPdfOutputs
{
    /// <summary>
    /// Render each page to a png target.
    /// When omitted, page images are not rendered at all.
    /// </summary>
    Png = 1,

    /// <summary>
    /// All outputs.
    /// </summary>
    All = Png
}
