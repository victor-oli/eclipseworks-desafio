namespace EclipseworksTaskManager.Api.ViewModels
{
    public class BaseRespose<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }

        public static BaseRespose<T> GetSuccess()
        {
            return new BaseRespose<T>
            {
                Success = true
            };
        }

        public static BaseRespose<T> GetSuccess(T result)
        {
            return new BaseRespose<T>
            {
                Success = true,
                Result = result
            };
        }
    }
}