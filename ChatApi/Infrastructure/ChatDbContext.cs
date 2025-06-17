using ChatApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatApi.Infrastructure;

public class ChatDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupUser> GroupUsers { get; set; }

    public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {   
        // Cấu hình mối quan hệ cho GroupUser
        modelBuilder.Entity<GroupUser>()
            .HasKey(gu => new { gu.UserId, gu.GroupId });

        modelBuilder.Entity<GroupUser>()
            .HasOne(gu => gu.User)
            .WithMany(u => u.GroupUsers)
            .HasForeignKey(gu => gu.UserId);

        modelBuilder.Entity<GroupUser>()
            .HasOne(gu => gu.Group)
            .WithMany(g => g.GroupUsers)
            .HasForeignKey(gu => gu.GroupId);

        // Cấu hình mối quan hệ cho Message
        modelBuilder.Entity<Message>()
            .HasOne(m => m.Sender)
            .WithMany(u => u.Messages)
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.Restrict); // Ngăn xóa người gửi khi có tin nhắn

        modelBuilder.Entity<Message>()
            .HasOne(m => m.Receiver)
            .WithMany() // Không cần thuộc tính điều hướng ngược lại nếu không có
            .HasForeignKey(m => m.ReceiverId)
            .OnDelete(DeleteBehavior.SetNull); // Đặt ReceiverId thành null nếu Receiver bị xóa

        modelBuilder.Entity<Message>()
            .HasOne(m => m.Group)
            .WithMany(g => g.Messages)
            .HasForeignKey(m => m.GroupId)
            .OnDelete(DeleteBehavior.Cascade); // Xóa tin nhắn khi nhóm bị xóa
    }
}