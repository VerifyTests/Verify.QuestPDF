# <img src="/src/icon.png" height="30px"> Verify.QuestPDF

[![Discussions](https://img.shields.io/badge/Verify-Discussions-yellow?svg=true&label=)](https://github.com/orgs/VerifyTests/discussions)
[![Build status](https://ci.appveyor.com/api/projects/status/au00qrkik2isl8vw?svg=true)](https://ci.appveyor.com/project/SimonCropp/Verify-QuestPDF)
[![NuGet Status](https://img.shields.io/nuget/v/Verify.QuestPDF.svg)](https://www.nuget.org/packages/Verify.QuestPDF/)

Extends [Verify](https://github.com/VerifyTests/Verify) to allow verification of documents via [QuestPDF](https://www.questpdf.com/).<!-- singleLineInclude: intro. path: /docs/intro.include.md -->

**See [Milestones](../../milestones?state=closed) for release notes.**

Designed to help assert the output of projects using QuestPDF to generate PDFs.


## Sponsors


### Entity Framework Extensions<!-- include: zzz. path: /docs/zzz.include.md -->

[Entity Framework Extensions](https://entityframework-extensions.net/?utm_source=simoncropp&utm_medium=Verify.QuestPDF) is a major sponsor and is proud to contribute to the development this project.

[![Entity Framework Extensions](https://raw.githubusercontent.com/VerifyTests/Verify.QuestPDF/refs/heads/main/docs/zzz.png)](https://entityframework-extensions.net/?utm_source=simoncropp&utm_medium=Verify.QuestPDF)<!-- endInclude -->


## NuGet

 * https://nuget.org/packages/Verify.QuestPDF


## Usage

<!-- snippet: enable -->
<a id='snippet-enable'></a>
```cs
[ModuleInitializer]
public static void Init()
{
    VerifyImageMagick.RegisterComparers(0.015);
    VerifyQuestPdf.Initialize();
}
```
<sup><a href='/src/Tests/ModuleInitializer.cs#L3-L12' title='Snippet source file'>snippet source</a> | <a href='#snippet-enable' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

This sample uses [Verify.ImageMagick](https://github.com/VerifyTests/Verify.ImageMagick) to ignore small rendering differences that are expected between differens operating systesm.

Other [compares](https://github.com/VerifyTests/Verify/blob/main/docs/comparer.md) options: 

 * https://github.com/VerifyTests/Verify.ImageHash
 * https://github.com/VerifyTests/Verify.ImageMagick
 * https://github.com/VerifyTests/Verify.Phash
 * https://github.com/VerifyTests/Verify.ImageSharp.Compare


### Code that generates a document 

<!-- snippet: GenerateDocument -->
<a id='snippet-GenerateDocument'></a>
```cs
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
        .Column(_ => _.Item()
            .Text(Placeholders.LoremIpsum()));

    page.Footer()
        .AlignCenter()
        .Text(_ =>
        {
            _.Span("Page ");
            _.CurrentPageNumber();
        });
}
```
<sup><a href='/src/Tests/Samples.cs#L39-L72' title='Snippet source file'>snippet source</a> | <a href='#snippet-GenerateDocument' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### Verify a Document

<!-- snippet: VerifyDocument -->
<a id='snippet-VerifyDocument'></a>
```cs
[Test]
public Task VerifyDocument()
{
    var document = GenerateDocument();
    return Verify(document);
}
```
<sup><a href='/src/Tests/Samples.cs#L4-L13' title='Snippet source file'>snippet source</a> | <a href='#snippet-VerifyDocument' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### Results


#### Metadata

<!-- snippet: Samples.VerifyDocument.verified.txt -->
<a id='snippet-Samples.VerifyDocument.verified.txt'></a>
```txt
{
  Pages: 2,
  Metadata: {
    CreationDate: DateTimeOffset_1,
    ModifiedDate: DateTimeOffset_2
  },
  Settings: {
    ContentDirection: LeftToRight,
    PdfA: false,
    ImageCompressionQuality: High,
    ImageRasterDpi: 288
  }
}
```
<sup><a href='/src/Tests/Samples.VerifyDocument.verified.txt#L1-L13' title='Snippet source file'>snippet source</a> | <a href='#snippet-Samples.VerifyDocument.verified.txt' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


#### Pdf as image

<img src="src/Tests/Samples.VerifyDocument%2300.verified.png" width="300px">


## PagesToInclude

To render only a defined number of pages at the start of a document:

<!-- snippet: PagesToInclude -->
<a id='snippet-PagesToInclude'></a>
```cs
[Test]
public Task PagesToInclude()
{
    var document = GenerateDocument();
    return Verify(document)
        .PagesToInclude(1);
}
```
<sup><a href='/src/Tests/Samples.cs#L15-L25' title='Snippet source file'>snippet source</a> | <a href='#snippet-PagesToInclude' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### Dynamic 

To dynamically control what pages are rendered:

<!-- snippet: PagesToIncludeDynamic -->
<a id='snippet-PagesToIncludeDynamic'></a>
```cs
[Test]
public Task PagesToIncludeDynamic()
{
    var document = GenerateDocument();
    return Verify(document)
        .PagesToInclude(pageNumber => pageNumber == 2);
}
```
<sup><a href='/src/Tests/Samples.cs#L27-L37' title='Snippet source file'>snippet source</a> | <a href='#snippet-PagesToIncludeDynamic' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->
