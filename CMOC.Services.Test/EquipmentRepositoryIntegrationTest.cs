namespace CMOC.Services.Test;

[TestFixture]
public class EquipmentRepositoryIntegrationTest
{
    private AppDbContext _db;
    private IEquipmentRepository _locDb;
    
    [OneTimeSetUp]
    public void EnvironmentSetup()
    {
        MapsterConfig.RegisterMapsterConfiguration();
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
            Name = "Test Location"
        });

        _db.EquipmentTypes.Add(new EquipmentType
        {
            Id = 1,
            Name = "Test Equipment"
        });
        
        _db.Equipment.Add(new Equipment
        {
            Id = 1,
            SerialNumber = "Test Equipment",
            LocationId = 1,
            Notes = "",
            TypeId = 1
        });

        _db.SaveChanges();
        _db.ChangeTracker.Clear();

        _locDb = new EquipmentRepository(_db);
    }

    [Test]
    public async Task CanGetEquipment()
    {
        var existingEquipment = await _locDb.GetAsync(e => e.SerialNumber == "Test Equipment");
        
        Assert.Multiple(() =>
        {
            Assert.That(() => existingEquipment != null);
            Assert.That(() => existingEquipment?.Id == 1);
        });
    }

    [Test]
    public async Task CanGetMultipleEquipment()
    {
        _db.Equipment.AddRange(new []
        {
            new Equipment
            {
                Id = 0,
                SerialNumber = "Test Equipment 2",
                LocationId = 1,
                Notes = "",
                TypeId = 1
            },
            new Equipment
            {
                Id = 0,
                SerialNumber = "Test Equipment 3",
                LocationId = 1,
                Notes = "",
                TypeId = 1
            }
        });

        _db.SaveChanges();

        var results = await _locDb.GetManyAsync();
        
        Assert.That(results, Has.Count.EqualTo(3));
    }
    
    [Test]
    public async Task CanGetMultipleFilteredEquipment()
    {
        _db.Equipment.AddRange(new []
        {
            new Equipment
            {
                Id = 0,
                SerialNumber = "Test Equipment 2",
                LocationId = 1,
                Notes = "",
                TypeId = 1
            },
            new Equipment
            {
                Id = 0,
                SerialNumber = "Test Equipment 3",
                LocationId = 1,
                Notes = "",
                TypeId = 1
            }
        });

        _db.SaveChanges();

        var results = await _locDb.GetManyAsync(e => e.SerialNumber != "Test Equipment 2");
        
        Assert.Multiple(() =>
        {
            Assert.That(results, Has.Count.EqualTo(2));
            Assert.That(results.FirstOrDefault(e => e.SerialNumber == "Test Equipment 2"), Is.Null);
        });
    }

    [Test]
    public async Task CanAddEquipment()
    {
        Assert.That(() => _db.Equipment.Count() == 1);

        var newEquipment = await _locDb.AddAsync(new EquipmentDto
        {
            Id = 0,
            SerialNumber = "Test Equipment 3",
            LocationId = 1,
            Notes = "",
            TypeId = 1
        });
        
        Assert.Multiple(() =>
        {
            Assert.That(_db.Equipment.Count(),Is.EqualTo(2));
            Assert.That(_db.Equipment.FirstOrDefault(e => e.SerialNumber == "Test Equipment 3"), Is.Not.Null);
            Assert.That(newEquipment.Id, Is.Not.Zero);
        });
    }

    [Test]
    public async Task CanUpdateEquipment()
    {
        var updatedEquipment = await _locDb.UpdateAsync(new EquipmentDto
        {
            Id = 1,
            SerialNumber = "Test Equipment 3",
            LocationId = 1,
            Notes = "",
            TypeId = 1
        });
        
        Assert.Multiple(() =>
        {
            Assert.That(updatedEquipment.Id, Is.EqualTo(1));
            Assert.That(updatedEquipment.SerialNumber, Is.EqualTo("Test Equipment 3"));
            Assert.That(_db.Equipment.Count(), Is.EqualTo(1));
            Assert.That(_db.Equipment.FirstOrDefault(l => l.SerialNumber == "Test Equipment 3"), Is.Not.Null);
        });
    }

    [Test]
    public async Task CanRemoveEquipment()
    {
        var result = await _locDb.RemoveAsync(1);
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.True);
            Assert.That(_db.Equipment.Count(), Is.Zero);
        });
    }

    [Test]
    public async Task CannotRemoveEquipmentInUse()
    {
        _db.Services.Add(new Service
        {
            Id = 1,
            Name = "Test Service"
        });

        _db.ServiceSupportRelationships.Add(new ServiceSupportRelationship
        {
            Id = 1,
            EquipmentId = 1,
            ServiceId = 1,
            TypeId = 1
        });

        _db.SaveChanges();

        var result = await _locDb.RemoveAsync(1);
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.False);
            Assert.That(_db.Equipment.Count(), Is.EqualTo(1));
        });
    }

    [Test]
    public async Task RemoveNonExistingEquipmentIsFalse()
    {
        var result = await _locDb.RemoveAsync(2);
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.False);
            Assert.That(_db.Equipment.Count(), Is.EqualTo(1));
        });
    }

    [TearDown]
    public void Teardown()
    {
        _db.Database.EnsureDeleted();
    }
}