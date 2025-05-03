using Microsoft.EntityFrameworkCore; // لإضافة DbContext و DbSet
using smartcube.model;
using smartcube.Model; // يجب أن تحتوي على الكلاس Users

namespace smartcube.DataContext
{
    public class EntityFramework : DbContext
    {
        private readonly IConfiguration _connectionString;

        public EntityFramework(IConfiguration config)
        {
            _connectionString = config;
        }

        public virtual DbSet<Department> Departments { get; set; } // تأكد من وجود الكلاس Users في models
        public virtual DbSet<Users> Users { get; set; } // تأكد من وجود الكلاس Users في models
        public virtual DbSet<Test> Test { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseSqlServer(_connectionString.GetConnectionString("DefaultConnection"),
                    sqlOptions => sqlOptions.EnableRetryOnFailure());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().HasKey(u => u.user_id);
            modelBuilder.Entity<Users>().Property(u => u.first_name).HasMaxLength(50);

            modelBuilder.Entity<Users>().HasOne(u => u.Department)
                                        .WithMany(d => d.Users)
                                        .HasForeignKey(u => u.DepartmentId);


            modelBuilder.Entity<Department>().HasKey(u => u.Id);
            modelBuilder.Entity<Department>().HasMany(u => u.Users).WithOne(u => u.Department)
                .HasForeignKey(u => u.DepartmentId);
            modelBuilder.Entity<Test>().HasKey(u => u.id);



        }
    }
}
