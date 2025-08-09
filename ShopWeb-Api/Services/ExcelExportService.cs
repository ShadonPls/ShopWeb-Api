using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ShopWeb_Api.Models.DTO;
using ShopWeb_Api.Services.Interfaces;
using System.Data;

namespace ShopWeb_Api.Services
{
    /// <summary>
    /// Сервис для экспорта данных БД в Excel
    /// </summary>
    public class ExcelExportService : IExcelExportService
    {
        private readonly Data.AppContext _db;
        private readonly string _excelMimeType;

        public ExcelExportService(Data.AppContext dbContext, string excelMimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        {
            _db = dbContext;
            _excelMimeType = excelMimeType;
        }

        /// <summary>
        /// Экспортирует все таблицы из базы данных в Excel файл
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>DTO с содержимым файла Excel</returns>
        public async Task<ExcelResponseDTO> ExportAllTablesToExcelAsync(CancellationToken cancellationToken = default)
        {
            var tables = await ExtractDatabaseTablesAsync();
            var excelBytes = ConvertToExcel(tables);

            return new ExcelResponseDTO
            {
                FileContent = excelBytes,
                ContentType = _excelMimeType,
                FileName = $"DB_Backup_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
            };
        }

        /// <summary>
        /// Извлекает данные из всех таблиц базы данных
        /// </summary>
        /// <returns>Список DTO с данными таблиц</returns>
        public async Task<IReadOnlyList<TableDataDTO>> ExtractDatabaseTablesAsync()
        {
            var result = new List<TableDataDTO>();
            var entityTypes = _db.Model.GetEntityTypes();

            foreach (var entityType in entityTypes)
            {
                var records = await GetEntityRecords(entityType);
                result.Add(new TableDataDTO
                {
                    TableName = entityType.GetTableName() ?? entityType.ClrType.Name,
                    Records = records
                });
            }

            return result;
        }

        /// <summary>
        /// Получает записи для указанного типа сущности
        /// </summary>
        /// <param name="entityType">Тип сущности EF</param>
        /// <returns>Список записей сущности</returns>
        private async Task<List<object>> GetEntityRecords(IEntityType entityType)
        {
            var query = (IQueryable<object>)_db.GetType()
                .GetMethod("Set", Type.EmptyTypes)
                .MakeGenericMethod(entityType.ClrType)
                .Invoke(_db, null);

            return await query.AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Конвертирует данные таблиц в Excel файл
        /// </summary>
        /// <param name="tables">Список DTO с данными таблиц</param>
        /// <returns>Массив байтов с содержимым Excel файла</returns>
        private byte[] ConvertToExcel(IReadOnlyList<TableDataDTO> tables)
        {
            using var memoryStream = new MemoryStream();
            using var workbook = new XLWorkbook();

            foreach (var table in tables)
            {
                if (table.Records.Count == 0) continue;

                var sheet = workbook.Worksheets.Add(table.TableName);
                GenerateSheetContent(sheet, table.Records);
            }

            workbook.SaveAs(memoryStream);
            return memoryStream.ToArray();
        }

        /// <summary>
        /// Генерирует содержимое листа Excel на основе данных записей
        /// </summary>
        /// <param name="sheet">Рабочий лист Excel</param>
        /// <param name="records">Список записей для экспорта</param>
        private void GenerateSheetContent(IXLWorksheet sheet, IReadOnlyList<object> records)
        {
            var properties = records.First().GetType().GetProperties()
                .Where(p => IsSupportedType(p.PropertyType))
                .ToList();

            for (int i = 0; i < properties.Count; i++)
            {
                sheet.Cell(1, i + 1).Value = properties[i].Name;
                sheet.Cell(1, i + 1).Style.Font.Bold = true;
            }

            for (int rowIdx = 0; rowIdx < records.Count; rowIdx++)
            {
                var record = records[rowIdx];
                for (int colIdx = 0; colIdx < properties.Count; colIdx++)
                {
                    sheet.Cell(rowIdx + 2, colIdx + 1).Value = properties[colIdx].GetValue(record)?.ToString();
                }
            }

            sheet.Columns().AdjustToContents();
        }

        /// <summary>
        /// Проверяет, поддерживается ли тип данных для экспорта в Excel
        /// </summary>
        /// <param name="type">Тип данных для проверки</param>
        /// <returns>True, если тип поддерживается, иначе False</returns>
        private bool IsSupportedType(Type type)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;

            return type.IsPrimitive ||
                   type.IsEnum ||
                   type == typeof(string) ||
                   type == typeof(decimal) ||
                   type == typeof(DateTime) ||
                   type == typeof(TimeSpan) ||
                   type == typeof(Guid);
        }
    }
}
