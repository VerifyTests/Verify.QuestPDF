public static class ModuleInitializer
{
    #region enable

    [ModuleInitializer]
    public static void Init()
    {
        VerifyQuestPdf.Initialize();

        #endregion

        VerifyDiffPlex.Initialize();
    }
}