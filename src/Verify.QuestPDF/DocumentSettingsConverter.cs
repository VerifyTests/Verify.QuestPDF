class DocumentSettingsConverter :
    WriteOnlyJsonConverter<DocumentSettings>
{
    public override void Write(VerifyJsonWriter writer, DocumentSettings value)
    {
        writer.WriteStartObject();
        writer.WriteMember(value, value.ContentDirection, "ContentDirection");
        writer.WriteMember(value, value.PDFA_Conformance, "PDFA_Conformance");
        writer.WriteMember(value, value.PDFUA_Conformance, "PDFUA_Conformance");
        writer.WriteMember(value, value.ImageCompressionQuality, "ImageCompressionQuality");
        writer.WriteMember(value, value.ImageRasterDpi, "ImageRasterDpi");
        writer.WriteEnd();
    }
}