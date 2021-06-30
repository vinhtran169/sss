using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace sss.Models
{
    public partial class sssContext : DbContext
    {
        public sssContext()
        {
        }

        public sssContext(DbContextOptions<sssContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Reward> Rewards { get; set; }
        public virtual DbSet<Suggestion> Suggestions { get; set; }
        public virtual DbSet<Systemuser> Systemusers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=TVVINH-NEWPC\\SQLEXPRESS;Database=sss;User Id=sa;password=khongco@12;Trusted_Connection=False;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("account");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("password");

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .HasColumnName("status");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<Reward>(entity =>
            {
                entity.ToTable("reward");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.RewardMoney).HasColumnName("reward_money");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.TypeOfSuggest)
                    .HasMaxLength(10)
                    .HasColumnName("type_of_suggest")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Suggestion>(entity =>
            {
                entity.ToTable("suggestion");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("date")
                    .HasColumnName("created_date");

                entity.Property(e => e.Creator)
                    .HasMaxLength(10)
                    .HasColumnName("creator")
                    .IsFixedLength(true);

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.ImplementDate)
                    .HasColumnType("date")
                    .HasColumnName("implement_date");

                entity.Property(e => e.RemarkFromApprover).HasColumnName("remark_from_approver");

                entity.Property(e => e.RewardMoney).HasColumnName("reward_money");

                entity.Property(e => e.StatusType)
                    .HasMaxLength(10)
                    .HasColumnName("status_type")
                    .IsFixedLength(true);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("title");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("date")
                    .HasColumnName("updated_date");

                entity.Property(e => e.Userid)
                    .HasMaxLength(10)
                    .HasColumnName("userid")
                    .IsFixedLength(true);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Suggestions)
                    .HasForeignKey(d => d.Userid)
                    .HasConstraintName("FK_suggestion_systemuser");
            });

            modelBuilder.Entity<Systemuser>(entity =>
            {
                entity.HasKey(e => e.Userid);

                entity.ToTable("systemuser");

                entity.Property(e => e.Userid)
                    .HasMaxLength(10)
                    .HasColumnName("userid")
                    .IsFixedLength(true);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("date")
                    .HasColumnName("created_date");

                entity.Property(e => e.Department)
                    .HasMaxLength(10)
                    .HasColumnName("department")
                    .IsFixedLength(true);

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Email)
                    .HasMaxLength(10)
                    .HasColumnName("email")
                    .IsFixedLength(true);

                entity.Property(e => e.Role)
                    .HasMaxLength(10)
                    .HasColumnName("role")
                    .IsFixedLength(true);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("username")
                    .IsFixedLength(true);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
