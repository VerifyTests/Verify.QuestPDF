using QuestPDF.Drawing;

class DocumentMetadataConverter :
    WriteOnlyJsonConverter<DocumentMetadata>
{
    public override void Write(VerifyJsonWriter writer, DocumentMetadata value)
    {
        writer.WriteStartObject();
        writer.WriteMember(value, value.Author, "Author");
        writer.WriteMember(value, value.Creator, "Creator");
        writer.WriteMember(value, value.Keywords, "Keywords");
        writer.WriteMember(value, value.Producer, "Producer");
        writer.WriteMember(value, value.Title, "Title");
        writer.WriteMember(value, value.Subject, "Subject");
        writer.WriteMember(value, value.CreationDate, "CreationDate");
        writer.WriteMember(value, value.ModifiedDate, "ModifiedDate");
        writer.WriteMember(value, value.ImageQuality, "ImageQuality");
        writer.WriteMember(value, value.PdfA, "PdfA");
        writer.WriteMember(value, value.RasterDpi, "RasterDpi");
        writer.WriteEnd();
    }
}