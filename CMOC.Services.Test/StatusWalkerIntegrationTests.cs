namespace CMOC.Services.Test;

public class StatusWalkerIntegrationTests
{
    private AppDbContext _db;

    [SetUp]
    public void Setup()
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseInMemoryDatabase("test_db");
        optionsBuilder.EnableSensitiveDataLogging();
        _db = new AppDbContext(optionsBuilder.Options);
        
        _db.Database.EnsureCreated();
        _db.Services.Add(new Service
        {
            Id = 1,
            Name = "Test Service"
        });

        _db.SaveChanges();
        _db.ChangeTracker.Clear();
    }

    [TearDown]
    public void Teardown()
    {
        _db.Database.EnsureDeleted();
    }
}