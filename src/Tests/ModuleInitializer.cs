public static class ModuleInitializer
{
    #region enable

    [ModuleInitializer]
    public static void Init()
    {
        VerifyImageMagick.RegisterComparers(0.015);
        VerifyQuestPdf.Initialize();
    }

    #endregion

    [ModuleInitializer]
    public static void InitOther()
    {
        QuestPDF.Settings.License = LicenseType.Community;
        VerifierSettings.InitializePlugins();
    }
}