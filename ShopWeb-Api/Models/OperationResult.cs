namespace ShopWeb_Api.Models
{
    /// <summary>
    /// Результат операции с флагом успеха и сообщением
    /// </summary>
    public class OperationResult
    {
        /// <summary>
        /// Флаг успешного выполнения операции
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// Сообщение, описывающее результат операции
        /// Может быть null для успешных операций
        /// </summary>
        public string Message { get; }

        private OperationResult(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        /// <summary>
        /// Создает успешный результат операции
        /// </summary>
        /// <param name="message">Необязательное сообщение об успехе</param>
        /// <returns>Успешный результат операции</returns>
        public static OperationResult Success(string message = null)
            => new OperationResult(true, message);

        /// <summary>
        /// Создает неуспешный результат операции
        /// </summary>
        /// <param name="message">Обязательное сообщение об ошибке</param>
        /// <returns>Неуспешный результат операции</returns>
        public static OperationResult Failure(string message)
            => new OperationResult(false, message);
    }
}
