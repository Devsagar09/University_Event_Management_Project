using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EventsMVC.Models
{
    public partial class UniversityEventManagementContext : DbContext
    {
        public UniversityEventManagementContext()
        {
        }

        public UniversityEventManagementContext(DbContextOptions<UniversityEventManagementContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ContactTable> ContactTables { get; set; } = null!;
        public virtual DbSet<EventCategory> EventCategories { get; set; } = null!;
        public virtual DbSet<EventMaster> EventMasters { get; set; } = null!;
        public virtual DbSet<FeedbackTable> FeedbackTables { get; set; } = null!;
        public virtual DbSet<HistoryTable> HistoryTables { get; set; } = null!;
        public virtual DbSet<ParticipantTable> ParticipantTables { get; set; } = null!;
        public virtual DbSet<PaymentTable> PaymentTables { get; set; } = null!;
        public virtual DbSet<RoleTable> RoleTables { get; set; } = null!;
        public virtual DbSet<UserMaster> UserMasters { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=laptop-8tcmrkva;Initial Catalog=UniversityEventManagement;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContactTable>(entity =>
            {
                entity.HasKey(e => e.ContactId);

                entity.ToTable("ContactTable");

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Query)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EventCategory>(entity =>
            {
                entity.HasKey(e => e.CategoryId);

                entity.ToTable("EventCategory");

                entity.Property(e => e.CategoryId).HasColumnName("Category_ID");

                entity.Property(e => e.CategoryName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("Category_Name");
            });

            modelBuilder.Entity<EventMaster>(entity =>
            {
                entity.HasKey(e => e.EventId);

                entity.ToTable("EventMaster");

                entity.Property(e => e.EventId).HasColumnName("Event_ID");

                entity.Property(e => e.CategoryId).HasColumnName("Category_ID");

                entity.Property(e => e.Date)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.EventImage)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.EventName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.EventMasters)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_EventMaster_EventCategory");
            });

            modelBuilder.Entity<FeedbackTable>(entity =>
            {
                entity.HasKey(e => e.FeedbackId);

                entity.ToTable("FeedbackTable");

                entity.Property(e => e.FeedbackId).HasColumnName("Feedback_ID");

                entity.Property(e => e.Message)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.FeedbackTables)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FeedbackTable_UserMaster");
            });

            modelBuilder.Entity<HistoryTable>(entity =>
            {
                entity.HasKey(e => e.HistoryId);

                entity.ToTable("HistoryTable");

                entity.Property(e => e.HistoryId).HasColumnName("History_ID");

                entity.Property(e => e.EventId).HasColumnName("EventID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.HistoryTables)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK_HistoryTable_EventMaster");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HistoryTables)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_HistoryTable_UserMaster");
            });

            modelBuilder.Entity<ParticipantTable>(entity =>
            {
                entity.HasKey(e => e.ParticipantId);

                entity.ToTable("ParticipantTable");

                entity.Property(e => e.ParticipantId).HasColumnName("Participant_ID");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.EventId).HasColumnName("EventID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.ParticipantTables)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK_ParticipantTable_EventMaster");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ParticipantTables)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_ParticipantTable_UserMaster");
            });

            modelBuilder.Entity<PaymentTable>(entity =>
            {
                entity.HasKey(e => e.PaymentId);

                entity.ToTable("PaymentTable");

                entity.Property(e => e.PaymentId).HasColumnName("Payment_ID");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.EventId).HasColumnName("EventID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.PaymentTables)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK_PaymentTable_EventMaster");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PaymentTables)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_PaymentTable_UserMaster");
            });

            modelBuilder.Entity<RoleTable>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.ToTable("RoleTable");

                entity.Property(e => e.RoleId).HasColumnName("Role_ID");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("Role_Name");
            });

            modelBuilder.Entity<UserMaster>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("UserMaster");

                entity.Property(e => e.UserId).HasColumnName("User_ID");

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RoleId).HasColumnName("Role_ID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserMasters)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_UserMaster_RoleTable");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
