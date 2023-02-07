using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace VerifyTests;

public static class VerifyQuestPdf
{
    public static bool Initialized { get; private set; }

    public static void Initialize()
    {
        if (Initialized)
        {
            throw new("Already Initialized");
        }

        Initialized = true;

        InnerVerifier.ThrowIfVerifyHasBeenRun();
        VerifierSettings
            .AddExtraSettings(
                _ => _.Converters.Add(new DocumentMetadataConverter()));
        VerifierSettings.RegisterFileConverter<IDocument>(
            conversion: (document, settings) =>
            {
                var pages = document.GenerateImages().ToList();

                var targets = new List<Target>();
                if (!settings.GetPagesToInclude(out var pagesToInclude))
                {
                    pagesToInclude = _ => true;
                }

                for (var index = 0; index < pages.Count; index++)
                {
                    if (pagesToInclude(index + 1))
                    {
                        var page = pages[index];
                        var stream = new MemoryStream(page);
                        targets.Add(new("png", stream));
                    }
                }

                return new(
                    info: new
                    {
                        Pages = pages.Count,
                        Metadata = document.GetMetadata(),
                    },
                    targets);
            });
    }
}