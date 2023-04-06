using TaskAPI.Model;

namespace TaskAPI.Data
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<int>> Register(Users user, string password);
        Task<ServiceResponse<string>> Login(string username, string password);
        
    }
}
