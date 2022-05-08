using QuestPDF.Drawing;
using VerifyTests;

class DocumentMetadataConverter :
    WriteOnlyJsonConverter<DocumentMetadata>
{
    public override void Write(VerifyJsonWriter writer, DocumentMetadata value)
    {
        writer.WriteStartObject();
        writer.WriteProperty(value, value.Author, "Author");
        writer.WriteProperty(value, value.Creator, "Creator");
        writer.WriteProperty(value, value.Keywords, "Keywords");
        writer.WriteProperty(value, value.Producer, "Producer");
        writer.WriteProperty(value, value.Title, "Title");
        writer.WriteProperty(value, value.Subject, "Subject");
        writer.WriteProperty(value, value.CreationDate, "CreationDate");
        writer.WriteProperty(value, value.ModifiedDate, "ModifiedDate");
        writer.WriteProperty(value, value.ImageQuality, "ImageQuality");
        writer.WriteProperty(value, value.PdfA, "PdfA");
        writer.WriteProperty(value, value.RasterDpi, "RasterDpi");
        writer.WriteEnd();
    }
}