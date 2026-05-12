using user_api.cs.Models;

namespace user_api.cs.Repositories;

public class UserRepository(UserDbContext ctx) : GenericRepository<User>(ctx);
