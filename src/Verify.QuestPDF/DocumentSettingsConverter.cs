class DocumentSettingsConverter :
    WriteOnlyJsonConverter<DocumentSettings>
{
    public override void Write(VerifyJsonWriter writer, DocumentSettings value)
    {
        writer.WriteStartObject();
        writer.WriteMember(value, value.ContentDirection, "ContentDirection");
        writer.WriteMember(value, value.PdfA, "PdfA");
        writer.WriteMember(value, value.ImageCompressionQuality, "ImageCompressionQuality");
        writer.WriteMember(value, value.ImageRasterDpi, "ImageRasterDpi");
        writer.WriteEnd();
    }
}