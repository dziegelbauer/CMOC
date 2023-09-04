

namespace CMOC.Services.Test;

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
            Assert.That(() => existingIssue != null);
            Assert.That(() => existingIssue?.Id == 1);
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

        _db.SaveChanges();

        var results = await _issueDb.GetManyAsync();
        
        Assert.That(results, Has.Count.EqualTo(3));
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

        _db.SaveChanges();

        var results = await _issueDb.GetManyAsync(i => i.Notes != "Another Issue");
        
        Assert.Multiple(() =>
        {
            Assert.That(results, Has.Count.EqualTo(2));
            Assert.That(results.FirstOrDefault(i => i.Notes == "Another Issue"), Is.Null);
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
            Assert.That(newIssue.Id, Is.Not.Zero);
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
            Assert.That(updatedIssue.Id, Is.EqualTo(1));
            Assert.That(updatedIssue.Notes, Is.EqualTo("An Old Issue"));
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
            Assert.That(result, Is.True);
            Assert.That(_db.Issues.Count(), Is.Zero);
        });
    }

    [Test]
    public async Task CannotRemoveIssueInUse()
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
        
        _db.Equipment.Add(new Equipment
        {
            Id = 1,
            LocationId = 1,
            Notes = "",
            SerialNumber = "",
            TypeId = 1,
            IssueId = 1
        });

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

        _db.Components.Add(new Component
        {
            Id = 1,
            ComponentOfId = 1,
            Operational = true,
            TypeId = 1,
            SerialNumber = "1",
            IssueId = 2
        });

        _db.SaveChanges();

        var result = await _issueDb.RemoveAsync(1);
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.False);
            Assert.That(_db.Issues.Count(), Is.EqualTo(2));
        });
        
        _db.Components.RemoveRange(_db.Components);
        _db.ComponentTypes.RemoveRange(_db.ComponentTypes);
        _db.ComponentRelationships.RemoveRange(_db.ComponentRelationships);
        _db.Equipment.RemoveRange(_db.Equipment);
        _db.EquipmentTypes.RemoveRange(_db.EquipmentTypes);

        _db.SaveChanges();
    }

    [Test]
    public async Task RemoveNonExistingIssueIsFalse()
    {
        var result = await _issueDb.RemoveAsync(2);
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.False);
            Assert.That(_db.Issues.Count(), Is.EqualTo(1));
        });
    }

    [TearDown]
    public void Teardown()
    {
        _db.Issues.RemoveRange(_db.Issues);
        _db.SaveChanges();
    }
}