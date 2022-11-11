using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Models;

namespace SocialNetwork.Data
{
    public class SocialNetworkContext : DbContext
    {
        public SocialNetworkContext (DbContextOptions<SocialNetworkContext> options)
            : base(options)
        {
        }

        public DbSet<SocialNetwork.Models.User> User { get; set; } = default!;
    }
}
