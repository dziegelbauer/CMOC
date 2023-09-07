namespace CMOC.Services.Test;

[TestFixture]
public class ServiceRepositoryIntegrationTest
{
    private AppDbContext _db;
    private IServiceRepository _serviceDb;
    
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
        _db.Services.Add(new Service
        {
            Id = 1,
            Name = "Test Service"
        });

        _db.SaveChanges();
        _db.ChangeTracker.Clear();

        _serviceDb = new ServiceRepository(_db);
    }

    [Test]
    public async Task CanGetService()
    {
        var existingService = await _serviceDb.GetAsync(l => l.Name == "Test Service");
        
        Assert.Multiple(() =>
        {
            Assert.That(() => existingService != null);
            Assert.That(() => existingService?.Id == 1);
        });
    }

    [Test]
    public async Task CanGetMultipleServices()
    {
        _db.Services.AddRange(new []
        {
            new Service
            {
                Id = 0,
                Name = "Service 2"
            },
            new Service
            {
                Id = 0,
                Name = "Service 3"
            }
        });

        _db.SaveChanges();

        var results = await _serviceDb.GetManyAsync();
        
        Assert.That(results, Has.Count.EqualTo(3));
    }
    
    [Test]
    public async Task CanGetMultipleFilteredServices()
    {
        _db.Services.AddRange(new []
        {
            new Service
            {
                Id = 0,
                Name = "Service 2"
            },
            new Service
            {
                Id = 0,
                Name = "Service 3"
            }
        });

        _db.SaveChanges();

        var results = await _serviceDb.GetManyAsync(l => l.Name != "Service 2");
        
        Assert.Multiple(() =>
        {
            Assert.That(results, Has.Count.EqualTo(2));
            Assert.That(results.FirstOrDefault(l => l.Name == "Service 2"), Is.Null);
        });
    }

    [Test]
    public async Task CanAddServices()
    {
        Assert.That(() => _db.Services.Count() == 1);

        var newService = await _serviceDb.AddAsync(new ServiceDto
        {
            Id = 0,
            Name = "Test addition"
        });
        
        Assert.Multiple(() =>
        {
            Assert.That(_db.Services.Count(),Is.EqualTo(2));
            Assert.That(_db.Services.FirstOrDefault(s => s.Name == "Test addition"), Is.Not.Null);
            Assert.That(newService.Id, Is.Not.Zero);
        });
    }

    [Test]
    public async Task CanUpdateService()
    {
        var updatedService = await _serviceDb.UpdateAsync(new ServiceDto
        {
            Id = 1,
            Name = "Service 2"
        });
        
        Assert.Multiple(() =>
        {
            Assert.That(updatedService.Id, Is.EqualTo(1));
            Assert.That(updatedService.Name, Is.EqualTo("Service 2"));
            Assert.That(_db.Services.Count(), Is.EqualTo(1));
            Assert.That(_db.Services.FirstOrDefault(s => s.Name == "Service 2"), Is.Not.Null);
        });
    }

    [Test]
    public async Task CanRemoveService()
    {
        var result = await _serviceDb.RemoveAsync(1);
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.True);
            Assert.That(_db.Services.Count(), Is.Zero);
        });
    }

    [Test]
    public async Task CannotRemoveServiceInUse()
    {
        _db.Capabilities.Add(new Capability
        {
            Id = 1,
            Name = "Test"
        });

        _db.CapabilitySupportRelationships.Add(new CapabilitySupportRelationship
        {
            Id = 1,
            CapabilityId = 1,
            ServiceId = 1
        });

        _db.Services.Add(new Service
        {
            Id = 2,
            Name = "Service 2"
        });

        _db.EquipmentTypes.Add(new EquipmentType
        {
            Id = 1,
            Name = ""
        });
        
        _db.Equipment.Add(new Equipment
        {
            Id = 1,
            LocationId = 1,
            Notes = "",
            SerialNumber = "",
            TypeId = 1
        });
        
        _db.ServiceSupportRelationships.Add(new ServiceSupportRelationship
        {
            Id = 1,
            EquipmentId = 1,
            ServiceId = 2,
            FailureThreshold = 1
        });
        
        _db.SaveChanges();

        var result = await _serviceDb.RemoveAsync(1);
        var result2 = await _serviceDb.RemoveAsync(2);
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.False);
            Assert.That(result2, Is.False);
            Assert.That(_db.Services.Count(), Is.EqualTo(2));
        });
    }

    [Test]
    public async Task RemoveNonExistingServiceIsFalse()
    {
        var result = await _serviceDb.RemoveAsync(2);
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.False);
            Assert.That(_db.Services.Count(), Is.EqualTo(1));
        });
    }

    [TearDown]
    public void Teardown()
    {
        _db.Database.EnsureDeleted();
    }
}