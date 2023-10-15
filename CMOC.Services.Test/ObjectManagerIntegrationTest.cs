namespace CMOC.Services.Test;

[TestFixture]
public class ObjectManagerIntegrationTest
{
    private AppDbContext _db;
    private IObjectManager _objectManager;

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
            SerialNumber = "Test Equipment",
            LocationId = 1,
            Notes = "",
            TypeId = 1
        });
        
        _db.ComponentTypes.Add(new ComponentType
        {
            Id = 1,
            Name = "Test Component Type"
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
            TypeId = 1
        });

        _db.Issues.Add(new Issue
        {
            Id = 1,
            ExpectedCompletion = DateTime.Now,
            Notes = "",
            TicketNumber = "1"
        });

        _db.SaveChanges();
        _db.ChangeTracker.Clear();

        _objectManager = new ObjectManager(
            _db,
            new CapabilityRepository(_db),
            new ServiceRepository(_db),
            new EquipmentRepository(_db),
            new LocationRepository(_db),
            new ComponentRepository(_db),
            new IssueRepository(_db));
    }

    [Test]
    public async Task CanAssignTicketToEquipment()
    {
        var equipment = await _objectManager.AssignIssueToEquipment(1, 1);
        
        Assert.Multiple(() =>
        {
            Assert.That(equipment, Is.Not.Null);
            Assert.That(equipment!.IssueId, Is.EqualTo(1));
        });
    }
    
    [Test]
    public async Task CanAssignTicketToComponent()
    {
        var component = await _objectManager.AssignIssueToComponent(1, 1);
        
        Assert.Multiple(() =>
        {
            Assert.That(component, Is.Not.Null);
            Assert.That(component!.IssueId, Is.EqualTo(1));
        });
    }
    
    [TearDown]
    public void Teardown()
    {
        _db.Database.EnsureDeleted();
    }
}