using Microsoft.AspNetCore.Mvc;
using ShopWeb_Api.Services;
using ShopWeb_Api.Services.Interfaces;

namespace ShopWeb_Api.Controllers
{
    [ApiController]
    [Route("api/export")]
    public class ExportController : ControllerBase
    {
        private readonly IExcelExportService _exportService;

        public ExportController(IExcelExportService exportService)
        {
            _exportService = exportService;
        }

        /// <summary>
        /// Экспортирует все данные БД в Excel файл
        /// </summary>
        [HttpGet("excel")]
        public async Task<IActionResult> ExportToExcel(CancellationToken cancellationToken)
        {
            var stream = await _exportService.ExportAllTablesToExcelAsync(cancellationToken);
            return File(stream.FileContent, stream.ContentType ,stream.FileName );
        }
    }
}
