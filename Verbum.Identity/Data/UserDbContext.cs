using Microsoft.EntityFrameworkCore;
using Verbum.Identity.Models;

namespace Verbum.Identity.Data
{
    public class UserDbContext :DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
           
        }
       public  DbSet<VerbumUser> Users { get; set; } 
    }
}
