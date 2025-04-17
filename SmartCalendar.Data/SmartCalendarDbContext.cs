using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
namespace SmartCalendar.Data
{
        public class SmartCalendarDbContext : IdentityDbContext
        {
            public SmartCalendarDbContext(DbContextOptions<SmartCalendarDbContext> options)
                : base(options)
            {
            }

            
            public DbSet<RefreshToken> RefreshTokens { get; set; }
        }

    

}
