using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Tests;

[TestFixture]
class VerifySettingsSamples
{
    [Test]
    public Task VerifyFirstPage()
    {
        var document = GenerateDocumentWithTwoPages();
        var settings = new VerifySettings();
        settings.PagesToInclude(0);
        return Verify(document, settings);
    }

    [Test]
    public Task VerifySecondPage()
    {
        var document = GenerateDocumentWithTwoPages();
        var settings = new VerifySettings();
        settings.PagesToInclude(1);
        return Verify(document, settings);
    }

    [Test]
    public Task VerifyAllPages()
    {
        var document = GenerateDocumentWithTwoPages();
        return Verify(document);
    }

    static IDocument GenerateDocumentWithTwoPages() =>
        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A5);
                page.Margin(1, Unit.Centimetre);
                page.PageColor(Colors.Grey.Lighten3);
                page.DefaultTextStyle(x => x.FontSize(20));

                page.Header()
                    .Text("Hello 1st page!")
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
            container.Page(page =>
            {
                page.Size(PageSizes.A5);
                page.Margin(1, Unit.Centimetre);
                page.PageColor(Colors.Grey.Lighten3);
                page.DefaultTextStyle(x => x.FontSize(20));

                page.Header()
                    .Text("Hello 2nd page!")
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
}
