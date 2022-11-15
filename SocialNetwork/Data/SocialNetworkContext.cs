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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserFriendRelation>()
                .HasKey(fr => new { fr.UserLogin, fr.FriendLogin });

            modelBuilder.Entity<UserFriendRelation>()
                .HasOne(uf => uf.Friend)
                .WithMany(f => f.FriendOf)
                .HasForeignKey(uf => uf.FriendLogin)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<UserFriendRelation>()
                .HasOne(uf => uf.User)
                .WithMany(f => f.Friends)
                .HasForeignKey(uf => uf.UserLogin);
        }
    }
}
