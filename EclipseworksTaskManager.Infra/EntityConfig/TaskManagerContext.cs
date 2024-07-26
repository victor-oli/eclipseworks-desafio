using EclipseworksTaskManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EclipseworksTaskManager.Infra.EntityConfig
{
    public class TaskManagerContext : DbContext
    {
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<JobEvent> JobEvents { get; set; }

        public TaskManagerContext(DbContextOptions<TaskManagerContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<Project>()
                .HasMany(x => x.Jobs)
                .WithOne(x => x.Project)
                .HasForeignKey(x => x.ProjectId)
                .HasPrincipalKey(x => x.Id)
                .IsRequired();

            modelBuilder
                .Entity<Job>()
                .HasOne(x => x.Project)
                .WithMany(x => x.Jobs)
                .HasForeignKey(x => x.ProjectId)
                .HasPrincipalKey(x => x.Id)
                .IsRequired();

            modelBuilder
                .Entity<Job>()
                .HasMany(x => x.JobEvents)
                .WithOne(x => x.Job)
                .HasForeignKey(x => x.JobId)
                .HasPrincipalKey(x => x.Id);

            modelBuilder
                .Entity<JobEvent>()
                .HasOne(x => x.Job)
                .WithMany(x => x.JobEvents)
                .HasForeignKey(x => x.JobId)
                .HasPrincipalKey(x => x.Id);

            modelBuilder.Entity<Job>(x =>
            {
                x.HasKey("Id");

                x.Property(x => x.DueDate)
                .HasColumnType("timestamp")
                .IsRequired();
            });

            modelBuilder.Entity<Project>(x =>
            {
                x.HasKey("Id");
            });

            modelBuilder.Entity<JobEvent>(x =>
            {
                x.HasKey("Id");

                x.Property(x => x.CreationDate)
                .HasColumnType("timestamp")
                .IsRequired();
            });
        }
    }
}