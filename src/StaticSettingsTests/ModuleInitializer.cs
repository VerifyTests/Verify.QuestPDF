public static class ModuleInitializer
{
    #region InitializeOutputs

    [ModuleInitializer]
    public static void Init()
    {
        QuestPDF.Settings.License = LicenseType.Community;
        VerifyQuestPdf.Initialize();

        // For every test: no page images, so pages are not rendered to png
        VerifierSettings.ExcludeDerivedTargets("png");
    }

    #endregion
}
