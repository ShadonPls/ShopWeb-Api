namespace ShopWeb_Api.Models.DTO
{
    public class ExcelResponseDTO
    {
        public byte[] FileContent { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
    }
}
