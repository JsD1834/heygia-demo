using HeyGiaDemo.Domain;
using Microsoft.EntityFrameworkCore;
using System;

namespace HeyGiaDemo.Context
{
    public class HeyGiaDemoDbContext: DbContext
    {
        public DbSet<Conversation> Conversations => Set<Conversation>();
        public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();

        public HeyGiaDemoDbContext(DbContextOptions<HeyGiaDemoDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Conversation>()
                .HasMany(c => c.Messages)
                .WithOne(m => m.Conversation)
                .HasForeignKey(m => m.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);

            // Index: updated at for admin sorting.
            modelBuilder.Entity<Conversation>()
                .HasIndex(c => c.UpdatedAt);
        }
    }
}
