using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace VerifyTests;

public static class VerifyQuestPdf
{
    public static void Initialize()
    {
        VerifierSettings
            .AddExtraSettings(
                _ => _.Converters.Add(new DocumentMetadataConverter()));
        VerifierSettings.RegisterFileConverter<IDocument>(
            conversion: (document, settings) =>
            {
                var pages = document.GenerateImages().ToList();

                var targets = new List<Target>();
                var pagesToInclude = settings.GetPagesToInclude(pages.Count);

                foreach (var pageIndex in pagesToInclude)
                {
                    var page = pages[pageIndex];
                    var stream = new MemoryStream(page);
                    targets.Add(new("png", stream));
                }

                return new(
                    info: new
                    {
                        Pages = pagesToInclude.Length,
                        Metadata = document.GetMetadata(),
                    },
                    targets);
            });
    }
}