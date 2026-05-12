using user_api.cs.Models;

namespace user_api.cs;

public class UserRepository(UserDbContext context) : GenericRepository<User>(context)
{
}
