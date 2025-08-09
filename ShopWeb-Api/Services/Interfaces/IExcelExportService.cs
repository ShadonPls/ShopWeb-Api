using ShopWeb_Api.Models.DTO;
using System.Data;

namespace ShopWeb_Api.Services.Interfaces
{
    public interface IExcelExportService
    {
        Task<ExcelResponseDTO> ExportAllTablesToExcelAsync(CancellationToken cancellationToken = default);
    }
}
