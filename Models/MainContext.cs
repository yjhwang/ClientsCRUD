using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ClientsCRUD.Models
{    public class MainContext : IdentityDbContext<MainUser>
    {
        public MainContext(DbContextOptions<MainContext> options)
            : base(options)
        {
        }
    }
}
