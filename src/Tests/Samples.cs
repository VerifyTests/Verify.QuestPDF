using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

[TestFixture]
public class Samples
{
    #region VerifyDocument

    [Test]
    public Task VerifyDocument() =>
        Verify(GenerateDocument());

    #endregion

    #region GenerateDocument

    static IDocument GenerateDocument() =>
        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A5);
                page.Margin(1, Unit.Centimetre);
                page.PageColor(Colors.Grey.Lighten3);
                page.DefaultTextStyle(x => x.FontSize(20));

                page.Header()
                    .Text("Hello PDF!")
                    .SemiBold().FontSize(36);

                page.Content()
                    .Column(x =>
                    {
                        x.Item()
                            .Text(Placeholders.LoremIpsum());
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                    });
            });
        });

    #endregion
}