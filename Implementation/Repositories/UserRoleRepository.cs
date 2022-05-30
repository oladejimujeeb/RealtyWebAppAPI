using RealtyWebApp.Context;
using RealtyWebApp.Entities.Identity;
using RealtyWebApp.Interface.IRepositories;

namespace RealtyWebApp.Implementation.Repositories
{
    public class UserRoleRepository:BaseRepository<UserRole>,IUserRoleRepository
    {
        public UserRoleRepository(ApplicationContext context)
        {
            Context = context;
        }
    }
}