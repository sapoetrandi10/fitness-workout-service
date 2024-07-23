using fitness_db.Data;
using fitness_db.Interfaces;
using fitness_db.Models;

namespace fitness_db.Repositories
{
    public class UserRepository : IUserRepository
    {
        private FitnessContext _fitnessCtx;
        public UserRepository(FitnessContext context)
        {
            _fitnessCtx = context;
        }

        public bool CreateUser(User User)
        {
            _fitnessCtx.users.Add(User);
            var saved = _fitnessCtx.SaveChanges();

            return saved > 0 ? true : false;
        }

        public User UpdateUser(User User)
        {
            var updated = _fitnessCtx.SaveChanges();

            if (updated < 0)
            {
                return User = null;
            }

            return User;
        }


        public bool DeleteUser(User User)
        {
            _fitnessCtx.Remove(User);
            var saved = _fitnessCtx.SaveChanges();

            return saved > 0 ? true : false;
        }

        public ICollection<User> GetUsers()
        {
            return _fitnessCtx.users.ToList();
        }

        public User GetUser(int userId)
        {
            var user = _fitnessCtx.users.Find(userId);
            if (user == null)
            {
                return null;
            }
            return user;
        }
    }
}
