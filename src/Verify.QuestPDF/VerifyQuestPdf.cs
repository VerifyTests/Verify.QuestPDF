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
                var max = pages.Count;
                if (settings.GetPagesToInclude(out var pagesToInclude))
                {
                    max = pagesToInclude.Value;
                }

                for (var index = 0; index < max; index++)
                {
                    var page = pages[index];
                    var stream = new MemoryStream(page);
                    targets.Add(new("png", stream, null));
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