using APIAuthentificationUsingJWT.Models;

namespace APIAuthentificationUsingJWT.Services
{
    public interface IUserRepository
    {
        User Create(User user);
        User GetByEmail(string email);
        User GetById(int id);
    }
}
