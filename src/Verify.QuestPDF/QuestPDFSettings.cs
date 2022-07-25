using System.Diagnostics.CodeAnalysis;

namespace VerifyTests;

public static class QuestPDFSettings
{
    private const string PAGESTOINCLUDE = "QuestPDF.PagesToInclude";

    /// <summary>
    /// Specify the pages you want to include using a zero based index.
    /// If no pages are specified, all pages will be included.
    /// </summary>
    /// <param name="settings"></param>
    /// <param name="pages"></param>
    public static void PagesToInclude(this VerifySettings settings, params uint[] pages) =>
        settings.Context[PAGESTOINCLUDE] = pages;

    public static SettingsTask PagesToInclude(this SettingsTask settings, params uint[] pages)
    {
        settings.CurrentSettings.PagesToInclude(pages);
        return settings;
    }

    internal static int[] GetPagesToInclude(
        this IReadOnlyDictionary<string, object> context,
        int maxPages)
    {
        if (context.TryGetValue(PAGESTOINCLUDE, out var value))
        {
            var result = (int[])value;
            if (result.Any(p => p >= maxPages))
            {
                throw new ArgumentOutOfRangeException($"PagesToInclude contains a page number that is greater than the maximum number of pages ({maxPages}).");
            }
            return result;
        }
        return IncludeAllPages(maxPages);
    }

    private static int[] IncludeAllPages(int maxPages)
    {
        var defaultResult = new int[maxPages];
        for (int i = 1; i < maxPages; i++)
        {
            defaultResult[i] = i;
        }
        return defaultResult;
    }
}