

namespace CMOC.Services.Test;

public class LocationRepositoryIntegrationTest
{
    private AppDbContext _db;
    private ILocationRepository _locDb;
    
    [OneTimeSetUp]
    public void EnvironmentSetup()
    {
        
    }
    
    [SetUp]
    public void Setup()
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseInMemoryDatabase("test_db");
        optionsBuilder.EnableSensitiveDataLogging();
        _db = new AppDbContext(optionsBuilder.Options);

        _db.Database.EnsureCreated();
        _db.Locations.Add(new Location
        {
            Id = 1,
            Name = "Test Locale"
        });

        _db.SaveChanges();
        _db.ChangeTracker.Clear();

        _locDb = new LocationRepository(_db);
    }

    [Test]
    public async Task CanGetLocation()
    {
        var existingLocation = await _locDb.GetAsync(l => l.Name == "Test Locale");
        
        Assert.Multiple(() =>
        {
            Assert.That(() => existingLocation != null);
            Assert.That(() => existingLocation?.Id == 1);
        });
    }

    [Test]
    public async Task CanGetMultipleLocations()
    {
        _db.Locations.AddRange(new []
        {
            new Location
            {
                Id = 0,
                Name = "Honduras"
            },
            new Location
            {
                Id = 0,
                Name = "Japan"
            }
        });

        _db.SaveChanges();

        var results = await _locDb.GetManyAsync();
        
        Assert.That(results, Has.Count.EqualTo(3));
    }
    
    [Test]
    public async Task CanGetMultipleFilteredLocations()
    {
        _db.Locations.AddRange(new []
        {
            new Location
            {
                Id = 0,
                Name = "Honduras"
            },
            new Location
            {
                Id = 0,
                Name = "Japan"
            }
        });

        _db.SaveChanges();

        var results = await _locDb.GetManyAsync(l => l.Name != "Honduras");
        
        Assert.Multiple(() =>
        {
            Assert.That(results, Has.Count.EqualTo(2));
            Assert.That(results.FirstOrDefault(l => l.Name == "Honduras"), Is.Null);
        });
    }

    [Test]
    public async Task CanAddLocations()
    {
        Assert.That(() => _db.Locations.Count() == 1);

        var newLocation = await _locDb.AddAsync(new LocationDto
        {
            Id = 0,
            Name = "Test addition"
        });
        
        Assert.Multiple(() =>
        {
            Assert.That(_db.Locations.Count(),Is.EqualTo(2));
            Assert.That(_db.Locations.FirstOrDefault(l => l.Name == "Test addition"), Is.Not.Null);
            Assert.That(newLocation.Id, Is.Not.Zero);
        });
    }

    [Test]
    public async Task CanUpdateLocation()
    {
        var updatedLocation = await _locDb.UpdateAsync(new LocationDto
        {
            Id = 1,
            Name = "Jamaica"
        });
        
        Assert.Multiple(() =>
        {
            Assert.That(updatedLocation.Id, Is.EqualTo(1));
            Assert.That(updatedLocation.Name, Is.EqualTo("Jamaica"));
            Assert.That(_db.Locations.Count(), Is.EqualTo(1));
            Assert.That(_db.Locations.FirstOrDefault(l => l.Name == "Jamaica"), Is.Not.Null);
        });
    }

    [Test]
    public async Task CanRemoveLocation()
    {
        var result = await _locDb.RemoveAsync(1);
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.True);
            Assert.That(_db.Locations.Count(), Is.Zero);
        });
    }

    [Test]
    public async Task CannotRemoveLocationInUse()
    {
        _db.Equipment.Add(new Equipment
        {
            Id = 1,
            LocationId = 1,
            Notes = "",
            SerialNumber = ""
        });

        _db.SaveChanges();

        var result = await _locDb.RemoveAsync(1);
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.False);
            Assert.That(_db.Locations.Count(), Is.EqualTo(1));
        });
    }

    [Test]
    public async Task RemoveNonExistingLocationIsFalse()
    {
        var result = await _locDb.RemoveAsync(2);
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.False);
            Assert.That(_db.Locations.Count(), Is.EqualTo(1));
        });
    }

    [TearDown]
    public void Teardown()
    {
        _db.Database.EnsureDeleted();
    }
}