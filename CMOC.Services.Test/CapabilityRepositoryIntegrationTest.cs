namespace CMOC.Services.Test;

public class CapabilityRepositoryIntegrationTest
{
    private AppDbContext _db;
    private ICapabilityRepository _capeDb;
    
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
        _db.Capabilities.Add(new Capability
        {
            Id = 1,
            Name = "Test Capability"
        });

        _db.SaveChanges();
        _db.ChangeTracker.Clear();

        _capeDb = new CapabilityRepository(_db);
    }

    [Test]
    public async Task CanGetCapability()
    {
        var existingCapability = await _capeDb.GetAsync(l => l.Name == "Test Capability");
        
        Assert.Multiple(() =>
        {
            Assert.That(() => existingCapability != null);
            Assert.That(() => existingCapability?.Id == 1);
        });
    }

    [Test]
    public async Task CanGetMultipleCapabilities()
    {
        _db.Capabilities.AddRange(new []
        {
            new Capability
            {
                Id = 0,
                Name = "Magic"
            },
            new Capability
            {
                Id = 0,
                Name = "Melee Combat"
            }
        });

        _db.SaveChanges();

        var results = await _capeDb.GetManyAsync();
        
        Assert.That(results, Has.Count.EqualTo(3));
    }
    
    [Test]
    public async Task CanGetMultipleFilteredCapabilities()
    {
        _db.Capabilities.AddRange(new []
        {
            new Capability
            {
                Id = 0,
                Name = "Magic"
            },
            new Capability
            {
                Id = 0,
                Name = "Melee Combat"
            }
        });

        _db.SaveChanges();

        var results = await _capeDb.GetManyAsync(l => l.Name != "Magic");
        
        Assert.Multiple(() =>
        {
            Assert.That(results, Has.Count.EqualTo(2));
            Assert.That(results.FirstOrDefault(l => l.Name == "Magic"), Is.Null);
        });
    }

    [Test]
    public async Task CanAddCapabilities()
    {
        Assert.That(() => _db.Capabilities.Count() == 1);

        var newCapability = await _capeDb.AddAsync(new CapabilityDto
        {
            Id = 0,
            Name = "Test Capability"
        });
        
        Assert.Multiple(() =>
        {
            Assert.That(_db.Capabilities.Count(),Is.EqualTo(2));
            Assert.That(_db.Capabilities.FirstOrDefault(l => l.Name == "Test Capability"), Is.Not.Null);
            Assert.That(newCapability.Id, Is.Not.Zero);
        });
    }

    [Test]
    public async Task CanUpdateCapability()
    {
        var updatedCapability = await _capeDb.UpdateAsync(new CapabilityDto
        {
            Id = 1,
            Name = "Capability 2"
        });
        
        Assert.Multiple(() =>
        {
            Assert.That(updatedCapability.Id, Is.EqualTo(1));
            Assert.That(updatedCapability.Name, Is.EqualTo("Capability 2"));
            Assert.That(_db.Capabilities.Count(), Is.EqualTo(1));
            Assert.That(_db.Capabilities.FirstOrDefault(l => l.Name == "Capability 2"), Is.Not.Null);
        });
    }

    [Test]
    public async Task CanRemoveCapability()
    {
        var result = await _capeDb.RemoveAsync(1);
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.True);
            Assert.That(_db.Capabilities.Count(), Is.Zero);
        });
    }

    [Test]
    public async Task CannotRemoveCapabilityInUse()
    {
        _db.Services.Add(new Service
        {
            Id = 1,
            Name = "Test Service"
        });
        
        _db.CapabilitySupportRelationships.Add(new CapabilitySupportRelationship
        {
            Id = 1,
            CapabilityId = 1,
            ServiceId = 1
        });

        _db.SaveChanges();

        var result = await _capeDb.RemoveAsync(1);
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.False);
            Assert.That(_db.Capabilities.Count(), Is.EqualTo(1));
        });
    }

    [Test]
    public async Task RemoveNonExistingCapabilityIsFalse()
    {
        var result = await _capeDb.RemoveAsync(2);
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.False);
            Assert.That(_db.Capabilities.Count(), Is.EqualTo(1));
        });
    }

    [TearDown]
    public void Teardown()
    {
        _db.Capabilities.RemoveRange(_db.Capabilities);
        _db.SaveChanges();
    }
}