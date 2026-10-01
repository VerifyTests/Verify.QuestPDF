public class Tests
{
    [Test]
    public Task VerifyDocument()
    {
        var document = Document.Create(container =>
        {
            container.Page(AddPage);
            container.Page(AddPage);
        });
        return Verify(document);
    }

    static void AddPage(PageDescriptor page)
    {
        page.Size(PageSizes.A5);
        page.Margin(1, Unit.Centimetre);
        page.DefaultTextStyle(_ => _.FontSize(20));
        page.Content()
            .Text("Hello World");
    }
}
