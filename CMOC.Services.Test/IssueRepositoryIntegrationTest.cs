namespace CMOC.Services.Test;

[TestFixture]
public class IssueRepositoryIntegrationTest
{
    private AppDbContext _db;
    private IIssueRepository _issueDb;
    
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
        _db.Issues.Add(new Issue
        {
            Id = 1,
            TicketNumber = "1",
            Notes = "Test Issue",
            ExpectedCompletion = DateTime.Now
        });

        _db.SaveChanges();
        _db.ChangeTracker.Clear();

        _issueDb = new IssueRepository(_db);
    }

    [Test]
    public async Task CanGetIssue()
    {
        var existingIssue = await _issueDb.GetAsync(i => i.Notes == "Test Issue");
        
        Assert.Multiple(() =>
        {
            Assert.That(existingIssue.Payload, Is.Not.Null);
            Assert.That(existingIssue.Payload?.Id, Is.EqualTo(1));
        });
    }

    [Test]
    public async Task CanGetMultipleIssues()
    {
        _db.Issues.AddRange(new []
        {
            new Issue
            {
                Id = 0,
                TicketNumber = "2",
                Notes = "Another Issue",
                ExpectedCompletion = DateTime.Now
            },
            new Issue
            {
                Id = 0,
                TicketNumber = "3",
                Notes = "Yet Another Issue",
                ExpectedCompletion = DateTime.Now
            }
        });

        await _db.SaveChangesAsync();

        var results = await _issueDb.GetManyAsync();
        
        Assert.That(results.Payload, Has.Count.EqualTo(3));
    }
    
    [Test]
    public async Task CanGetMultipleFilteredIssues()
    {
        _db.Issues.AddRange(new []
        {
            new Issue
            {
                Id = 0,
                TicketNumber = "2",
                Notes = "Another Issue",
                ExpectedCompletion = DateTime.Now
            },
            new Issue
            {
                Id = 0,
                TicketNumber = "3",
                Notes = "Yet Another Issue",
                ExpectedCompletion = DateTime.Now
            }
        });

        await _db.SaveChangesAsync();

        var results = await _issueDb.GetManyAsync(i => i.Notes != "Another Issue");
        
        Assert.Multiple(() =>
        {
            Assert.That(results.Payload, Has.Count.EqualTo(2));
            Assert.That(results.Payload?.FirstOrDefault(i => i.Notes == "Another Issue"), Is.Null);
        });
    }

    [Test]
    public async Task CanAddIssues()
    {
        Assert.That(() => _db.Issues.Count() == 1);

        var newIssue = await _issueDb.AddAsync(new IssueDto
        {
            Id = 0,
            TicketNumber = "2",
            Notes = "Another Issue",
            ExpectedCompletion = DateTime.Now
        });
        
        Assert.Multiple(() =>
        {
            Assert.That(_db.Issues.Count(),Is.EqualTo(2));
            Assert.That(_db.Issues.FirstOrDefault(i => i.Notes == "Another Issue"), Is.Not.Null);
            Assert.That(newIssue.Payload?.Id, Is.Not.Zero);
        });
    }

    [Test]
    public async Task CanUpdateLocation()
    {
        var updatedIssue = await _issueDb.UpdateAsync(new IssueDto
        {
            Id = 1,
            TicketNumber = "2",
            Notes = "An Old Issue",
            ExpectedCompletion = DateTime.Now
        });
        
        Assert.Multiple(() =>
        {
            Assert.That(updatedIssue.Payload?.Id, Is.EqualTo(1));
            Assert.That(updatedIssue.Payload?.Notes, Is.EqualTo("An Old Issue"));
            Assert.That(_db.Issues.Count(), Is.EqualTo(1));
            Assert.That(_db.Issues.FirstOrDefault(i => i.Notes == "An Old Issue"), Is.Not.Null);
        });
    }

    [Test]
    public async Task CanRemoveIssue()
    {
        var result = await _issueDb.RemoveAsync(1);
        
        Assert.Multiple(() =>
        {
            Assert.That(result.Result, Is.EqualTo(ServiceResult.Success));
            Assert.That(_db.Issues.Count(), Is.Zero);
        });
    }

    [Test]
    public async Task RemovingIssueInUseClearsIds()
    {
        _db.Issues.Add(new Issue
        {
            Id = 2,
            TicketNumber = "5",
            Notes = "A component issue",
            ExpectedCompletion = DateTime.Now
        });
        
        _db.EquipmentTypes.Add(new EquipmentType
        {
            Id = 1,
            Name = "Test Equipment"
        });
        
        var usingEquipment = _db.Equipment.Add(new Equipment
        {
            Id = 1,
            LocationId = 1,
            Notes = "",
            SerialNumber = "",
            TypeId = 1,
            IssueId = 1
        }).Entity;

        _db.ComponentRelationships.Add(new ComponentRelationship
        {
            Id = 1,
            EquipmentId = 1,
            FailureThreshold = 0,
            TypeId = 1
        });

        _db.ComponentTypes.Add(new ComponentType
        {
            Id = 1,
            Name = "Test Component"
        });

        var usingComponent = _db.Components.Add(new Component
        {
            Id = 1,
            ComponentOfId = 1,
            Operational = true,
            TypeId = 1,
            SerialNumber = "1",
            IssueId = 2
        }).Entity;

        await _db.SaveChangesAsync();

        var result = await _issueDb.RemoveAsync(1);
        var result2 = await _issueDb.RemoveAsync(2);
        
        Assert.Multiple(() =>
        {
            Assert.That(result.Result, Is.EqualTo(ServiceResult.Success));
            Assert.That(result2.Result, Is.EqualTo(ServiceResult.Success));
            Assert.That(_db.Issues.Count(), Is.EqualTo(0));
            Assert.That(usingComponent.IssueId, Is.Null);
            Assert.That(usingEquipment.IssueId, Is.Null);
        });
    }

    [Test]
    public async Task RemoveNonExistingIssueIsUnsuccessful()
    {
        var result = await _issueDb.RemoveAsync(2);
        
        Assert.Multiple(() =>
        {
            Assert.That(result.Result, Is.Not.EqualTo(ServiceResult.Success));
            Assert.That(_db.Issues.Count(), Is.EqualTo(1));
        });
    }

    [TearDown]
    public void Teardown()
    {
        _db.Database.EnsureDeleted();
    }
}