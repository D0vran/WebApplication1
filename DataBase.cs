using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
public class DataBase : DbContext
{
    public static int id { get; set; } = 0;
    public DbSet<Person> Users { get; set; } = null;
    public DataBase(DbContextOptions<DataBase> options) 
        : base(options)
    {
        //Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>().HasKey(x => x.Id);
        modelBuilder.Entity<Person>()
                    .HasData(new Person { Id = -1, Name = "Dowlet", Age = 22 },
                             new Person { Id = -2, Name = "Dowran", Age = 20 });
        
        /*modelBuilder.Entity<Person>().HasData(new Person() 
        { 
            Id = id++, 
            Name = "Dowran Kadyrow", 
            Age = 20
        });*/
        /*modelBuilder.Entity<Info>().HasData(new Info
        {
            InfoId = id,
            Adress = "Ashgabat, Oguz Han street, 95/1",
            PhoneNumber = "+99363145332",
            Id = id
        });
        modelBuilder.Entity<Person>()
                    .HasOne(u => u.PersonInfo)
                    .WithOne(p => p.Person)
                    .HasForeignKey<Info>(p => p.Id);*/

    }
}
