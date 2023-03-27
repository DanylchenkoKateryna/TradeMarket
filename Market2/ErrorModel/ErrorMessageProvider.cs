
namespace Market2.ErrorModel
{
    public class ErrorMessageProvider : IErrorMessageProvider
    {
        public string NotFoundMessage<T>(int id)
            => $"{typeof(T).Name} with id: \"{id}\" doesn't exist in database";

        public string BadRequestMessage<T>()
            => $"{typeof(T).Name} object is null";
    }
}
