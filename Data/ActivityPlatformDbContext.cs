using System;
using System.Collections.Generic;
using Hackaton.Models;
using Microsoft.EntityFrameworkCore;

namespace Hackaton.Data;

public partial class ActivityPlatformDbContext : DbContext
{
    public ActivityPlatformDbContext()
    {
    }

    public ActivityPlatformDbContext(DbContextOptions<ActivityPlatformDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<EventReward> EventRewards { get; set; }

    public virtual DbSet<EventTable> EventTables { get; set; }

    public virtual DbSet<OrganizerReview> OrganizerReviews { get; set; }

    public virtual DbSet<Participation> Participations { get; set; }

    public virtual DbSet<PointsHistory> PointsHistories { get; set; }

    public virtual DbSet<Reward> Rewards { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<StatusEvent> StatusEvents { get; set; }

    public virtual DbSet<StatusParticipation> StatusParticipations { get; set; }

    public virtual DbSet<UserTable> UserTables { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=activity_platform_db;Username=postgres;Password=1");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.IdCategory).HasName("categories_pkey");

            entity.ToTable("categories");

            entity.HasIndex(e => e.CategoryName, "categories_category_name_key").IsUnique();

            entity.Property(e => e.IdCategory).HasColumnName("id_category");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(100)
                .HasColumnName("category_name");
        });

        modelBuilder.Entity<EventReward>(entity =>
        {
            entity.HasKey(e => e.IdEventRewards).HasName("event_rewards_pkey");

            entity.ToTable("event_rewards");

            entity.Property(e => e.IdEventRewards).HasColumnName("id_event_rewards");
            entity.Property(e => e.IdEvent).HasColumnName("id_event");
            entity.Property(e => e.IdReward).HasColumnName("id_reward");
            entity.Property(e => e.Quantity)
                .HasDefaultValue(1)
                .HasColumnName("quantity");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");

            entity.HasOne(d => d.IdEventNavigation).WithMany(p => p.EventRewards)
                .HasForeignKey(d => d.IdEvent)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("event_rewards_id_event_fkey");

            entity.HasOne(d => d.IdRewardNavigation).WithMany(p => p.EventRewards)
                .HasForeignKey(d => d.IdReward)
                .HasConstraintName("event_rewards_id_reward_fkey");
        });

        modelBuilder.Entity<EventTable>(entity =>
        {
            entity.HasKey(e => e.IdEvent).HasName("event_table_pkey");

            entity.ToTable("event_table");

            entity.HasIndex(e => e.EventDate, "idx_events_date");

            entity.Property(e => e.IdEvent).HasColumnName("id_event");
            entity.Property(e => e.BasePoints)
                .HasDefaultValue(10)
                .HasColumnName("base_points");
            entity.Property(e => e.ComplexityCoeff)
                .HasDefaultValueSql("1.0")
                .HasColumnName("complexity_coeff");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.EventDate).HasColumnName("event_date");
            entity.Property(e => e.IdCategory).HasColumnName("id_category");
            entity.Property(e => e.IdOrganizer).HasColumnName("id_organizer");
            entity.Property(e => e.IdStatus).HasColumnName("id_status");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");

            entity.HasOne(d => d.IdCategoryNavigation).WithMany(p => p.EventTables)
                .HasForeignKey(d => d.IdCategory)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("event_table_id_category_fkey");

            entity.HasOne(d => d.IdOrganizerNavigation).WithMany(p => p.EventTables)
                .HasForeignKey(d => d.IdOrganizer)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("event_table_id_organizer_fkey");

            entity.HasOne(d => d.IdStatusNavigation).WithMany(p => p.EventTables)
                .HasForeignKey(d => d.IdStatus)
                .HasConstraintName("event_table_id_status_fkey");
        });

        modelBuilder.Entity<OrganizerReview>(entity =>
        {
            entity.HasKey(e => e.IdOrganizerReview).HasName("organizer_reviews_pkey");

            entity.ToTable("organizer_reviews");

            entity.Property(e => e.IdOrganizerReview).HasColumnName("id_organizer_review");
            entity.Property(e => e.CommentReview).HasColumnName("comment_review");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.IdAuthor).HasColumnName("id_author");
            entity.Property(e => e.IdOrganizer).HasColumnName("id_organizer");
            entity.Property(e => e.Rating).HasColumnName("rating");

            entity.HasOne(d => d.IdAuthorNavigation).WithMany(p => p.OrganizerReviewIdAuthorNavigations)
                .HasForeignKey(d => d.IdAuthor)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("organizer_reviews_id_author_fkey");

            entity.HasOne(d => d.IdOrganizerNavigation).WithMany(p => p.OrganizerReviewIdOrganizerNavigations)
                .HasForeignKey(d => d.IdOrganizer)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("organizer_reviews_id_organizer_fkey");
        });

        modelBuilder.Entity<Participation>(entity =>
        {
            entity.HasKey(e => e.IdParticipation).HasName("participation_pkey");

            entity.ToTable("participation");

            entity.HasIndex(e => e.IdUser, "idx_participations_user");

            entity.HasIndex(e => new { e.IdUser, e.IdEvent }, "participation_id_user_id_event_key").IsUnique();

            entity.HasIndex(e => e.QrCodeHash, "participation_qr_code_hash_key").IsUnique();

            entity.Property(e => e.IdParticipation).HasColumnName("id_participation");
            entity.Property(e => e.ConfirmedAt).HasColumnName("confirmed_at");
            entity.Property(e => e.IdEvent).HasColumnName("id_event");
            entity.Property(e => e.IdStatusParticipation).HasColumnName("id_status_participation");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.PointsEarned)
                .HasDefaultValueSql("0.0")
                .HasColumnName("points_earned");
            entity.Property(e => e.QrCodeHash)
                .HasMaxLength(255)
                .HasColumnName("qr_code_hash");

            entity.HasOne(d => d.IdEventNavigation).WithMany(p => p.Participations)
                .HasForeignKey(d => d.IdEvent)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("participation_id_event_fkey");

            entity.HasOne(d => d.IdStatusParticipationNavigation).WithMany(p => p.Participations)
                .HasForeignKey(d => d.IdStatusParticipation)
                .HasConstraintName("participation_id_status_participation_fkey");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Participations)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("participation_id_user_fkey");
        });

        modelBuilder.Entity<PointsHistory>(entity =>
        {
            entity.HasKey(e => e.IdPointHistory).HasName("points_history_pkey");

            entity.ToTable("points_history");

            entity.Property(e => e.IdPointHistory).HasColumnName("id_point_history");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.PointsChange).HasColumnName("points_change");
            entity.Property(e => e.Reason)
                .HasMaxLength(255)
                .HasColumnName("reason");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.PointsHistories)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("points_history_id_user_fkey");
        });

        modelBuilder.Entity<Reward>(entity =>
        {
            entity.HasKey(e => e.IdReward).HasName("reward_pkey");

            entity.ToTable("reward");

            entity.Property(e => e.IdReward).HasColumnName("id_reward");
            entity.Property(e => e.RewardName)
                .HasMaxLength(50)
                .HasColumnName("reward_name");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRole).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.Property(e => e.IdRole).HasColumnName("id_role");
            entity.Property(e => e.RoleName)
                .HasMaxLength(20)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<StatusEvent>(entity =>
        {
            entity.HasKey(e => e.IdStatus).HasName("status_pkey");

            entity.ToTable("status_event");

            entity.Property(e => e.IdStatus)
                .HasDefaultValueSql("nextval('status_id_status_seq'::regclass)")
                .HasColumnName("id_status");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'published'::character varying")
                .HasColumnName("status");
        });

        modelBuilder.Entity<StatusParticipation>(entity =>
        {
            entity.HasKey(e => e.IdStatusParticipation).HasName("status_participation_pkey");

            entity.ToTable("status_participation");

            entity.Property(e => e.IdStatusParticipation).HasColumnName("id_status_participation");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("status");
        });

        modelBuilder.Entity<UserTable>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("user_table_pkey");

            entity.ToTable("user_table");

            entity.HasIndex(e => e.TotalPoints, "idx_users_total_points").IsDescending();

            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.Birthday).HasColumnName("birthday");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(320)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasColumnName("full_name");
            entity.Property(e => e.IdRole).HasColumnName("id_role");
            entity.Property(e => e.Pwd)
                .HasMaxLength(256)
                .HasColumnName("pwd");
            entity.Property(e => e.TotalPoints).HasColumnName("total_points");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.UserTables)
                .HasForeignKey(d => d.IdRole)
                .HasConstraintName("user_table_id_role_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
