using APIAuthentificationUsingJWT.DAL;
using APIAuthentificationUsingJWT.Models;
using System.Linq;

namespace APIAuthentificationUsingJWT.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public User Create(User user)
        {
            _db.Users.Add(user);
            user.Id = _db.SaveChanges();

            return user;
        }

        public User GetByEmail(string email)
        {
            return _db.Users.FirstOrDefault(x => x.Email == email);
        }

        public User GetById(int id)
        {
            return _db.Users.FirstOrDefault(x => x.Id == id);
        }
    }
}
