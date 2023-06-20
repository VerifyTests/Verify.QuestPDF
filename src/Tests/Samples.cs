using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

[TestFixture]
public class Samples
{
    #region VerifyDocument

    [Test]
    public Task VerifyDocument()
    {
        var document = GenerateDocument();
        return Verify(document);
    }

    #endregion

    #region PagesToInclude

    [Test]
    public Task PagesToInclude()
    {
        var document = GenerateDocument();
        return Verify(document)
            .PagesToInclude(1);
    }

    #endregion

    #region PagesToIncludeDynamic

    [Test]
    public Task PagesToIncludeDynamic()
    {
        var document = GenerateDocument();
        return Verify(document)
            .PagesToInclude(pageNumber => pageNumber == 2);
    }

    #endregion

    #region GenerateDocument

    static IDocument GenerateDocument() =>
        Document.Create(container =>
        {
            container.Page(AddPage);
            container.Page(AddPage);
        });

    static void AddPage(PageDescriptor page)
    {
        page.Size(PageSizes.A5);
        page.Margin(1, Unit.Centimetre);
        page.PageColor(Colors.Grey.Lighten3);
        page.DefaultTextStyle(_ => _.FontSize(20));

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
    }

    #endregion
}