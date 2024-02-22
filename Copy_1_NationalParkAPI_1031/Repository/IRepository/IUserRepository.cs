using Copy_1_NationalParkAPI_1031.Models;

namespace Copy_1_NationalParkAPI_1031.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string userName);
        User Authenticate(string userName, string password);
        User Register(string userName, string password);

    }
}
