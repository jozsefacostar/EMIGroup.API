namespace Shared
{
    public class RequestResult<T> : IRequestResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public dynamic? Data { get; set; }
        public string Module { get; set; }

        // Constructor por defecto
        public RequestResult() { }

        // Constructor con parámetros
        public RequestResult(dynamic? data, bool success, string message, bool isWarning = false)
        {
            Data = data;
            Success = success;
            Message = message;
        }

        // Metodo estatico para devolver consulta realizada con éxito
        public static RequestResult<T> SuccessResult(dynamic? data = null, string message = "Registros consultados con éxito", string module = "")
        {
            return new RequestResult<T>(data, true, message, false);
        }

        // Metodo estatico para devolver registro creado con éxito
        public static RequestResult<T> SuccessRecord(dynamic? data = null, string message = "Registro creado con éxito", string module = "")
        {
            return new RequestResult<T>(data, true, message, false);
        }

        // Metodo estatico para devolver registro modificado con éxito
        public static RequestResult<T> SuccessUpdate(dynamic? data = null, string message = "Registro modificado con éxito", string module = "")
        {
            return new RequestResult<T>(data, true, message, false);
        }


        // Método estático para devolver un registro eliminado
        public static RequestResult<T> SuccessOperation(string message = "Operación exitosa", string module = "")
        {
            return new RequestResult<T>
            {
                Success = true,
                Message = message,
                Module = module
            };
        }

        // Método estático para devolver error de consulta
        public static RequestResult<T> ErrorResult(string message = "Ocurrió un error al consultar la información", string module = "")
        {
            return new RequestResult<T>
            {
                Success = false,
                Message = message,
                Module = module
            };
        }

        // Método estático para devolver error de guardado
        public static RequestResult<T> ErrorRecord(string message = "Ocurrió un error al crear el registro", string module = "")
        {
            return new RequestResult<T>
            {
                Success = false,
                Message = message,
                Module = module
            };
        }

        // Metodo estatico para devolver consulta realizada con éxito
        public static RequestResult<T> SuccessResultNoRecords(string message = "No se encontraron registros", string module = "")
        {
            return new RequestResult<T>(null, true, message, false);
        }

        // Método estático para devolver un registro eliminado
        public static RequestResult<T> SuccessDelete(string message = "Registro eliminado con éxito", string module = "")
        {
            return new RequestResult<T>
            {
                Success = true,
                Message = message,
                Module = module
            };
        }
    }

    public interface IRequestResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public dynamic? Data { get; set; }
        public string Module { get; set; }
    }
}
