
namespace Market2.ErrorModel
{
    public interface IErrorMessageProvider
    {
        string NotFoundMessage<T>(int id);
        string BadRequestMessage<T>();
    }
}
