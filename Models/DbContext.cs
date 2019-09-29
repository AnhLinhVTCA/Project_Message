using System;
using Microsoft.EntityFrameworkCore;

namespace Project_Email_MVC.Models
{
    public class MyDbContext : DbContext
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<Inbox> Inbox { get; set; }
        public DbSet<Outbox> Outbox { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            optionBuilder.UseMySQL("server=127.0.0.1;uid=root;pwd=Hanhphuc1;database=Project_Email_MVC");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(u => u.Username).IsUnique();
                entity.Property(x => x.Password);
            });     
            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Title);
                entity.Property(x => x.Content);
                entity.Property(x => x.SenderId);
                entity.Property(x => x.SendTime);
            });
            modelBuilder.Entity<Inbox>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.MessageId);
                entity.Property(x => x.ReceiverId);
                entity.Property(x => x.IsDeleted);
            });
            modelBuilder.Entity<Outbox>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.MessageId);
                entity.Property(x => x.SenderId);
                entity.Property(x => x.IsDeleted);
            });
        }
    }
}