# <img src="/src/icon.png" height="30px"> Verify.QuestPDF

[![Discussions](https://img.shields.io/badge/Verify-Discussions-yellow?svg=true&label=)](https://github.com/orgs/VerifyTests/discussions)
[![Build status](https://github.com/VerifyTests/Verify.QuestPDF/actions/workflows/build.yml/badge.svg)](https://github.com/VerifyTests/Verify.QuestPDF/actions/workflows/build.yml)
[![NuGet Status](https://img.shields.io/nuget/v/Verify.QuestPDF.svg)](https://www.nuget.org/packages/Verify.QuestPDF/)

Extends [Verify](https://github.com/VerifyTests/Verify) to allow verification of documents via [QuestPDF](https://www.questpdf.com/).<!-- singleLineInclude: intro. path: /docs/intro.include.md -->

**See [Milestones](../../milestones?state=closed) for release notes.**

Designed to help assert the output of projects using QuestPDF to generate PDFs.


## Sponsors


### Entity Framework Extensions<!-- include: sponsors. path: /docs/sponsors.include.md -->

[Entity Framework Extensions](https://entityframework-extensions.net/?utm_source=simoncropp&utm_medium=Verify.QuestPDF) is a major sponsor and is proud to contribute to the development this project.

[![Entity Framework Extensions](https://raw.githubusercontent.com/VerifyTests/Verify.QuestPDF/refs/heads/main/docs/zzz.png)](https://entityframework-extensions.net/?utm_source=simoncropp&utm_medium=Verify.QuestPDF)

### Developed using JetBrains IDEs

[![JetBrains logo.](https://raw.githubusercontent.com/VerifyTests/Verify.QuestPDF/main/docs/jetbrains.png)](https://jb.gg/OpenSourceSupport)<!-- endInclude -->


## NuGet

 * https://nuget.org/packages/Verify.QuestPDF


## Usage

<!-- snippet: enable -->
<a id='snippet-enable'></a>
```cs
[ModuleInitializer]
public static void Init()
{
    VerifierSettings.UseSsimForPng();
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
internal static IDocument GenerateDocument() =>
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
<sup><a href='/src/Tests/Samples.cs#L107-L140' title='Snippet source file'>snippet source</a> | <a href='#snippet-GenerateDocument' title='Start of snippet'>anchor</a></sup>
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
<sup><a href='/src/Tests/Samples.cs#L11-L20' title='Snippet source file'>snippet source</a> | <a href='#snippet-VerifyDocument' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### Results

Verifying a document produces:

 * The pdf itself as `.verified.pdf`. This can be omitted with [`ExcludeTargets`](#exclude-the-pdf).
 * An info file as `.verified.txt`, with the metadata and settings of the document and its page count.
 * A png of every page as `#page_0001.verified.png`, `#page_0002.verified.png`, etc. These can be omitted with [`ExcludeDerivedTargets`](#exclude-the-page-images).

The page files are named by Verify's [paged documents](https://github.com/VerifyTests/Verify/blob/main/docs/paged-documents.md) support, which every Verify plugin that splits a document into pages shares. So are the settings that [choose what is verified](#choosing-what-is-verified).


#### Metadata

<!-- snippet: Samples.VerifyDocument.verified.txt -->
<a id='snippet-Samples.VerifyDocument.verified.txt'></a>
```txt
{
  Document: {
    Settings: {
      ContentDirection: LeftToRight,
      PDFA_Conformance: None,
      PDFUA_Conformance: None,
      ImageCompressionQuality: High,
      ImageRasterDpi: 288
    }
  },
  PageCount: 2
}
```
<sup><a href='/src/Tests/Samples.VerifyDocument.verified.txt#L1-L12' title='Snippet source file'>snippet source</a> | <a href='#snippet-Samples.VerifyDocument.verified.txt' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


#### Pdf as image

<img src="src/Tests/Samples.VerifyDocument%23page_0001.verified.png" width="300px">


## Choosing what is verified

What a document is split into is controlled by Verify's settings for [paged documents](https://github.com/VerifyTests/Verify/blob/main/docs/paged-documents.md). Each can be set on a verification, or for every test on `VerifierSettings` at initialization.

No text is read from a document, so `PageText` has no effect.


### Exclude the page images

`ExcludeDerivedTargets("png")` leaves out the png of every page, keeping the pdf and the info file. Rasterizing pages is expensive, and it is then skipped altogether:

<!-- snippet: ExcludePng -->
<a id='snippet-ExcludePng'></a>
```cs
[Test]
public Task ExcludePng()
{
    var document = GenerateDocument();
    return Verify(document)
        .ExcludeDerivedTargets("png");
}
```
<sup><a href='/src/Tests/Samples.cs#L71-L81' title='Snippet source file'>snippet source</a> | <a href='#snippet-ExcludePng' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

To leave out the page images for every test:

<!-- snippet: InitializeOutputs -->
<a id='snippet-InitializeOutputs'></a>
```cs
[ModuleInitializer]
public static void Init()
{
    QuestPDF.Settings.License = LicenseType.Community;
    VerifyQuestPdf.Initialize();

    // For every test: no page images, so pages are not rendered to png
    VerifierSettings.ExcludeDerivedTargets("png");
}
```
<sup><a href='/src/StaticSettingsTests/ModuleInitializer.cs#L3-L15' title='Snippet source file'>snippet source</a> | <a href='#snippet-InitializeOutputs' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### Exclude the pdf

QuestPDF renders the source pdf, and it is included in the snapshot as a `.verified.pdf`. Generating it is expensive, and committing it is not always wanted. [`ExcludeTargets`](https://github.com/VerifyTests/Verify/blob/main/docs/converter.md#excluding-targets) drops it from a verification and skips the generation, while the rendered pages and info still verify:

<!-- snippet: ExcludePdf -->
<a id='snippet-ExcludePdf'></a>
```cs
[Test]
public Task ExcludePdf()
{
    var document = GenerateDocument();
    return Verify(document)
        .ExcludeTargets("pdf");
}
```
<sup><a href='/src/Tests/Samples.cs#L59-L69' title='Snippet source file'>snippet source</a> | <a href='#snippet-ExcludePdf' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

To exclude the pdf for every test, call `VerifierSettings.ExcludeTargets("pdf")` at initialization.


### PagesToInclude

To verify only a defined number of pages at the start of a document:

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
<sup><a href='/src/Tests/Samples.cs#L83-L93' title='Snippet source file'>snippet source</a> | <a href='#snippet-PagesToInclude' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

The pdf is still verified whole, and `PageCount` in the info file is still the number of pages the document has.


#### Dynamic

To dynamically control what pages are verified, pass a delegate that takes the 1 based number of a page:

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
<sup><a href='/src/Tests/Samples.cs#L95-L105' title='Snippet source file'>snippet source</a> | <a href='#snippet-PagesToIncludeDynamic' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

A page keeps its number when other pages are left out. The above verifies the second page as `#page_0002.verified.png`.

QuestPDF draws all the pages of a document in one pass, so the pages that are left out are still rendered. `PagesToInclude` limits what is verified, not the work done.


## Reviewing changes

A change to a document is a change to several files: the pdf, its info file, and every page. Verify tells the diff tool that the pages and the info file were derived from the pdf, and [DiffEngineViewer](https://github.com/VerifyTests/DiffEngine/blob/main/docs/viewer.md#files-derived-from-a-document), which draws a pdf's pages itself, shows them as one row and accepts them together. Other diff tools are given each file, as before.

When the pdf has changed, its pages are compared exactly, skipping any [comparer](https://github.com/VerifyTests/Verify/blob/main/docs/comparer.md) registered for `png`. A comparer exists to tolerate rendering differences, and a pdf that has changed is the one case where its pages should not be given the benefit of the doubt.


## Migrating from 2.x

Version 3 moves to the paged document support in Verify 33.3. The settings for choosing what is verified are now those of Verify:

| 2.x | 3.x |
| --- | --- |
| `VerifyQuestPdf.Initialize(QuestPdfOutputs.None)` | `VerifyQuestPdf.Initialize()` and `VerifierSettings.ExcludeDerivedTargets("png")` |
| `VerifyQuestPdf.Initialize(QuestPdfOutputs.Png)` or `QuestPdfOutputs.All` | `VerifyQuestPdf.Initialize()` |
| `.PagesToInclude(2)` | Unchanged. It is now a member of `VerifySettings` and `SettingsTask`, and of `VerifierSettings` for every test |
| `.PagesToInclude(pageNumber => pageNumber == 2)` | Unchanged for a lambda. The delegate `VerifyQuestPDF.ShouldIncludePage` is replaced by `VerifyTests.IncludePage` |

The page images are renamed. The number of a page is now 1 based, is always present, and is the number the page has in the document:

| 2.x | 3.x |
| --- | --- |
| `Tests.Report#00.verified.png` | `Tests.Report#page_0001.verified.png` |
| `Tests.Report#01.verified.png` | `Tests.Report#page_0002.verified.png` |
| `Tests.Report.verified.png` for a document with one page | `Tests.Report#page_0001.verified.png` |
| `Tests.Report.verified.png` for `PagesToInclude(pageNumber => pageNumber == 2)` | `Tests.Report#page_0002.verified.png` |
| `Tests.Report.verified.pdf` | Unchanged |
| `Tests.Report.verified.txt` | The same name, with the content below |

In 2.x the pages `PagesToInclude` kept were numbered again from zero, so the name of a page file depended on which other pages were verified.

The info file has the shape every paged document has: the metadata and settings under `Document`, and `Pages` as `PageCount`.

```
{                                     {
  Pages: 2,                             Document: {
  Metadata: {                             Metadata: {
    Title: The Title                        Title: The Title
  },                                      },
  Settings: {                             Settings: {
    ContentDirection: LeftToRight           ContentDirection: LeftToRight
  }                                       }
}                                       },
                                        PageCount: 2
                                      }
```

Renamed snapshots show as a new file and a pending delete. Accepting both, or running once with [AutoVerify](https://github.com/VerifyTests/Verify/blob/main/docs/autoverify.md), moves a test over.
