using DeterministicPdf;

// The Samples entry for SkipPdfNormalization only proves the setting round-trips through a
// verification. These assert the wiring it is actually there for: with the setting the snapshotted
// bytes are the producer's own, and without it they are the normalized ones.
[TestFixture]
public class SkipPdfNormalizationTests
{
    [Test]
    public void GeneratedPdfIsNotAlreadyNormalized()
    {
        // The premise the pair below rests on. QuestPDF pins the metadata dates before generating,
        // so the document is close to deterministic already, but the normalizer still zeroes those
        // pinned dates. If it did not, both snapshots would be identical and assert nothing.
        var raw = Samples.GenerateDocument().GeneratePdf();

        Assert.That(PdfNormalizer.Normalize(raw), Is.Not.EqualTo(raw));
    }

    [Test]
    public Task SkippedSnapshotHoldsTheProducerBytes() =>
        Verify(Samples.GenerateDocument())
            .SkipPdfNormalization();

    [Test]
    public Task NormalizedSnapshotHoldsTheNeutralizedBytes() =>
        Verify(Samples.GenerateDocument());
}
