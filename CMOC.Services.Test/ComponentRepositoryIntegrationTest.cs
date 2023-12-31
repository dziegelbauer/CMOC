﻿namespace CMOC.Services.Test;

[TestFixture]
public class ComponentRepositoryIntegrationTest
{
    private AppDbContext _db;
    private IComponentRepository _componentDb;

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

        _db.ComponentTypes.Add(new ComponentType
        {
            Id = 1,
            Name = "Test Component Type"
        });

        _db.EquipmentTypes.Add(new EquipmentType
        {
            Id = 1,
            Name = "Test Equipment"
        });

        _db.Locations.Add(new Location
        {
            Id = 1,
            Name = "West Texas"
        });

        _db.Equipment.Add(new Equipment
        {
            Id = 1,
            LocationId = 1,
            Notes = "",
            SerialNumber = "1",
            TypeId = 1
        });

        _db.ComponentRelationships.Add(new ComponentRelationship
        {
            Id = 1,
            EquipmentId = 1,
            TypeId = 1,
            FailureThreshold = 1
        });

        _db.Components.Add(new Component
        {
            Id = 1,
            SerialNumber = "Test Component",
            ComponentOfId = 1,
            TypeId = 1,
            Operational = true
        });

        _db.SaveChanges();
        _db.ChangeTracker.Clear();

        _componentDb = new ComponentRepository(_db);
    }

    [Test]
    public async Task CanGetComponent()
    {
        var existingComponent = await _componentDb.GetAsync(c => c.SerialNumber == "Test Component");

        Assert.Multiple(() =>
        {
            Assert.That(existingComponent, Is.Not.Null);
            Assert.That(existingComponent.Payload?.Id, Is.EqualTo(1));
        });
    }


    [Test]
    public async Task CanGetMultipleComponents()
    {
        _db.Components.AddRange(new[]
        {
            new Component
            {
                Id = 0,
                SerialNumber = "Test Component 1",
                ComponentOfId = 1,
                TypeId = 1
            },
            new Component
            {
                Id = 0,
                SerialNumber = "Test Component 2",
                ComponentOfId = 1,
                TypeId = 1
            }
        });

        await _db.SaveChangesAsync();

        var results = await _componentDb.GetManyAsync();

        Assert.That(results.Payload, Has.Count.EqualTo(3));
    }

    [Test]
    public async Task CanGetMultipleFilteredComponents()
    {
        _db.Components.AddRange(new[]
        {
            new Component
            {
                Id = 0,
                SerialNumber = "Test Component 1",
                ComponentOfId = 1,
                TypeId = 1
            },
            new Component
            {
                Id = 0,
                SerialNumber = "Test Component 2",
                ComponentOfId = 1,
                TypeId = 1
            }
        });

        await _db.SaveChangesAsync();

        var results = await _componentDb.GetManyAsync(c => c.SerialNumber != "Test Component 2");

        Assert.Multiple(() =>
        {
            Assert.That(results.Payload, Has.Count.EqualTo(2));
            Assert.That(results.Payload?.FirstOrDefault(c => c.SerialNumber == "Test Component 2"), Is.Null);
        });
    }

    [Test]
    public async Task CanAddComponents()
    {
        Assert.That(() => _db.Components.Count() == 1);

        var newComponent = await _componentDb.AddAsync(new ComponentDto
        {
            Id = 0,
            SerialNumber = "Test addition",
            ComponentOfId = 1,
            TypeId = 1,
            EquipmentId = 1
        });

        Assert.Multiple(() =>
        {
            Assert.That(_db.Components.Count(), Is.EqualTo(2));
            Assert.That(_db.Components.FirstOrDefault(c => c.SerialNumber == "Test addition"), Is.Not.Null);
            Assert.That(newComponent.Payload?.Id, Is.Not.Zero);
        });
    }

    [Test]
    public async Task CanUpdateComponent()
    {
        var updatedComponent = await _componentDb.UpdateAsync(new ComponentDto
        {
            Id = 1,
            SerialNumber = "Updated Component",
            ComponentOfId = 1,
            TypeId = 1,
            EquipmentId = 1
        });

        Assert.Multiple(() =>
        {
            Assert.That(updatedComponent.Payload?.Id, Is.EqualTo(1));
            Assert.That(updatedComponent.Payload?.SerialNumber, Is.EqualTo("Updated Component"));
            Assert.That(_db.Components.Count(), Is.EqualTo(1));
            Assert.That(_db.Components.FirstOrDefault(c => c.SerialNumber == "Updated Component"), Is.Not.Null);
        });
    }

    [Test]
    public async Task CanRemoveComponent()
    {
        var result = await _componentDb.RemoveAsync(1);

        Assert.Multiple(() =>
        {
            Assert.That(result.Result, Is.EqualTo(ServiceResult.Success));
            Assert.That(_db.Components.Count(), Is.Zero);
        });
    }

    [Test]
    public async Task RemoveNonExistingComponentIsUnsuccessful()
    {
        var result = await _componentDb.RemoveAsync(2);

        Assert.Multiple(() =>
        {
            Assert.That(result.Result, Is.Not.EqualTo(ServiceResult.Success));
            Assert.That(_db.Components.Count(), Is.EqualTo(1));
        });
    }

    [TearDown]
    public void Teardown()
    {
        _db.Database.EnsureDeleted();
    }
}