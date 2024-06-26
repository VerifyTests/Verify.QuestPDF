public static class ModuleInitializer
{
    #region enable

    [ModuleInitializer]
    public static void Init() =>
        VerifyQuestPdf.Initialize();

    #endregion

    [ModuleInitializer]
    public static void InitOther()
    {
        QuestPDF.Settings.License = LicenseType.Community;
        VerifierSettings.InitializePlugins();
    }
}