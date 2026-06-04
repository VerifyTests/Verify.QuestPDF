namespace VerifyTests;

public static class VerifyQuestPdf
{
    static readonly DateTimeOffset deterministicDate = new(2000, 1, 1, 0, 0, 0, TimeSpan.Zero);

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
                _ =>
                {
                    _.Converters.Add(new DocumentMetadataConverter());
                    _.Converters.Add(new DocumentSettingsConverter());
                });
        VerifierSettings.RegisterFileConverter<IDocument>(
            conversion: (document, settings) =>
            {
                var pages = document.GenerateImages().ToList();
                if (!settings.GetPagesToInclude(out var pagesToInclude))
                {
                    pagesToInclude = _ => true;
                }
                // QuestPDF stamps DateTimeOffset.Now into the PDF CreationDate/ModifiedDate on
                // every generation. Pin them to a fixed value so the pdf target is byte-stable.
                var metadata = document.GetMetadata();
                metadata.CreationDate = deterministicDate;
                metadata.ModifiedDate = deterministicDate;
                var pdf = document.GeneratePdf();
                List<Target> targets =
                [
                    new("pdf", new MemoryStream(pdf), performConversion:false)
                    {
                        BypassComparersForSubsequentOnDifference = true
                    }
                ];

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
                        Settings = document.GetSettings(),
                    },
                    targets);
            });
    }
}
