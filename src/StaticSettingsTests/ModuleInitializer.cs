public static class ModuleInitializer
{
    #region InitializeOutputs

    [ModuleInitializer]
    public static void Init()
    {
        QuestPDF.Settings.License = LicenseType.Community;
        // Skip rendering pages to png
        VerifyQuestPdf.Initialize(QuestPdfOutputs.All & ~QuestPdfOutputs.Png);
    }

    #endregion
}
