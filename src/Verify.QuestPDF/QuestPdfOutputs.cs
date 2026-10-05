namespace VerifyTests;

/// <summary>
/// What <c>Initialize</c> used to be given to choose the outputs a document is split into. Settings
/// of Verify choose that now, and nothing reads this: it is here so that code still naming it is
/// told what to use in its place.
/// </summary>
[Obsolete(
    "QuestPdfOutputs and the outputs argument of Initialize are replaced by settings of Verify: VerifierSettings.ExcludeDerivedTargets(\"png\") to leave out the page images. See https://github.com/VerifyTests/Verify.QuestPDF#migrating-from-2x",
    true)]
[Flags]
public enum QuestPdfOutputs
{
    /// <summary>
    /// No outputs. Only the source document (and info) is emitted.
    /// </summary>
    None = 0,

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
