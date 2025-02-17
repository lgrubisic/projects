﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EmployeeTimeTracker.Models
{
    public partial class EmployeeManagerTimeTrackContext : DbContext
    {
        public IConfiguration Configuration { get; }

        public EmployeeManagerTimeTrackContext(DbContextOptions<EmployeeManagerTimeTrackContext> options, IConfiguration configuration): base(options)
        {
            Configuration = configuration;
        }

        public virtual DbSet<EmployeeInfo> EmployeeInfo { get; set; }
        public virtual DbSet<TimeTrackings> TimeTrackings { get; set; }
        public virtual DbSet<EmployeeManager> EmployeeManager { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DBConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeInfo>(entity =>
            {
                entity.HasKey(e => e.id_num);

                entity.Property(e => e.id_num).HasColumnName("id_num");

                entity.Property(e => e.first_name)
                    .IsRequired()
                    .HasColumnName("first_name")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.last_name)
                    .IsRequired()
                    .HasColumnName("last_name")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(90)
                    .IsUnicode(false);

                entity.Property(e => e.user_privileges)
                    .IsRequired()
                    .HasColumnName("user_privileges")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.manager_id).HasColumnName("manager_id");

                entity.HasOne(d => d.EmployeeManag)
                    .WithMany(p => p.EmployeeInfo)
                    .HasForeignKey(d => d.manager_id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Manager__emplo__31H62524");
            });

            modelBuilder.Entity<TimeTrackings>(entity =>
            {
                entity.HasKey(e => e.timer_id);

                entity.Property(e => e.timer_id).HasColumnName("timer_id");

                entity.Property(e => e.date_of_work)
                    .HasColumnName("date_of_work")
                    .HasColumnType("date");

                entity.Property(e => e.employee_init_id).HasColumnName("employee_init_id");

                entity.Property(e => e.time_in)
                    .HasColumnName("time_in")
                    .HasColumnType("time(0)");

                entity.Property(e => e.time_out)
                    .HasColumnName("time_out")
                    .HasColumnType("time(0)");

                entity.HasOne(d => d.EmployeeInit)
                    .WithMany(p => p.TimeTrackings)
                    .HasForeignKey(d => d.employee_init_id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TimeTrack__emplo__36B12243");
            });
            modelBuilder.Entity<EmployeeManager>(entity =>
            {
                entity.HasKey(e => e.manager_id);
                entity.Property(e => e.manager_id).HasColumnName("manager_id");

                entity.Property(e => e.first_name)
                    .IsRequired()
                    .HasColumnName("first_name")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.last_name)
                    .IsRequired()
                    .HasColumnName("last_name")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(90)
                    .IsUnicode(false);

                entity.Property(e => e.username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
