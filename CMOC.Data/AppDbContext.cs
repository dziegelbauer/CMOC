using CMOC.Domain;
using Microsoft.EntityFrameworkCore;

namespace CMOC.Data;

public class AppDbContext : DbContext
{
    public DbSet<Capability> Capabilities { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Equipment> Equipment { get; set; }
    public DbSet<Component> Components { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<EquipmentType> EquipmentTypes { get; set; }
    public DbSet<ComponentType> ComponentTypes { get; set; }
    public DbSet<ComponentRelationship> ComponentRelationships { get; set; }
    public DbSet<ServiceSupportRelationship> ServiceSupportRelationships { get; set; }
    public DbSet<CapabilitySupportRelationship> CapabilitySupportRelationships { get; set; }
    public DbSet<EquipmentRedundancy> EquipmentRedundancies { get; set; }
    public DbSet<ServiceRedundancy> ServiceRedundancies { get; set; }
    public DbSet<Issue> Issues { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Component>()
            .ToTable("COMPONENTS");
        modelBuilder.Entity<Component>()
            .HasOne<ComponentType>(c => c.Type)
            .WithMany()
            .HasForeignKey(c => c.TypeId);
        modelBuilder.Entity<Component>()
            .HasOne<ComponentRelationship>(c => c.ComponentOf)
            .WithMany(cr => cr.Components)
            .HasForeignKey(c => c.ComponentOfId);
        modelBuilder.Entity<Component>()
            .HasOne<Issue>(e => e.Issue)
            .WithMany()
            .HasForeignKey(e => e.IssueId)
            .IsRequired(false);
        modelBuilder.Entity<Component>()
            .Navigation(c => c.Type)
            .AutoInclude();

        modelBuilder.Entity<ComponentType>()
            .ToTable("COMPONENT_TYPES");

        modelBuilder.Entity<ComponentRelationship>()
            .ToTable("COMPONENT_RELATIONSHIPS");
        modelBuilder.Entity<ComponentRelationship>()
            .HasOne<Equipment>(cr => cr.Equipment)
            .WithMany(e => e.Components)
            .HasForeignKey(cr => cr.EquipmentId);
        modelBuilder.Entity<ComponentRelationship>()
            .HasOne<ComponentType>(cr => cr.Type)
            .WithMany()
            .HasForeignKey(cr => cr.TypeId);
        modelBuilder.Entity<ComponentRelationship>()
            .Navigation(cr => cr.Type)
            .AutoInclude();

        modelBuilder.Entity<Equipment>()
            .ToTable("EQUIPMENT");
        modelBuilder.Entity<Equipment>()
            .HasOne<EquipmentType>(e => e.Type)
            .WithMany()
            .HasForeignKey(e => e.TypeId);
        modelBuilder.Entity<Equipment>()
            .HasOne<Location>(e => e.Location)
            .WithMany()
            .HasForeignKey(e => e.LocationId);
        modelBuilder.Entity<Equipment>()
            .HasOne<Issue>(e => e.Issue)
            .WithMany()
            .HasForeignKey(e => e.IssueId)
            .IsRequired(false);
        modelBuilder.Entity<Equipment>()
            .Navigation(e => e.Type)
            .AutoInclude();
        modelBuilder.Entity<Equipment>()
            .Navigation(e => e.Location)
            .AutoInclude();

        modelBuilder.Entity<EquipmentType>()
            .ToTable("EQUIPMENT_TYPES");

        modelBuilder.Entity<Location>()
            .ToTable("LOCATIONS");

        modelBuilder.Entity<ServiceSupportRelationship>()
            .ToTable("SERVICE_SUPPORT_RELATIONSHIPS");
        modelBuilder.Entity<ServiceSupportRelationship>()
            .HasOne<Service>(ssr => ssr.Service)
            .WithMany(s => s.SupportedBy)
            .HasForeignKey(ssr => ssr.ServiceId);
        modelBuilder.Entity<ServiceSupportRelationship>()
            .HasOne<Equipment>(ssr => ssr.Equipment)
            .WithMany(e => e.Relationships)
            .HasForeignKey(ssr => ssr.EquipmentId);
        modelBuilder.Entity<ServiceSupportRelationship>()
            .HasOne<EquipmentRedundancy>(ssr => ssr.RedundantWith)
            .WithMany(er => er.Redundancies)
            .HasForeignKey(ssr => ssr.RedundantWithId)
            .IsRequired(false);
        modelBuilder.Entity<ServiceSupportRelationship>()
            .HasOne<EquipmentType>(ssr => ssr.Type)
            .WithMany()
            .HasForeignKey(ssr => ssr.TypeId);

        modelBuilder.Entity<EquipmentRedundancy>()
            .ToTable("EQUIPMENT_REDUNDANCIES");

        modelBuilder.Entity<Service>()
            .ToTable("SERVICES");

        modelBuilder.Entity<CapabilitySupportRelationship>()
            .ToTable("CAPABILITY_SUPPORT_RELATIONSHIPS");
        modelBuilder.Entity<CapabilitySupportRelationship>()
            .HasOne<Service>(csr => csr.Service)
            .WithMany(s => s.Supports)
            .HasForeignKey(csr => csr.ServiceId);
        modelBuilder.Entity<CapabilitySupportRelationship>()
            .HasOne<Capability>(csr => csr.Capability)
            .WithMany(c => c.SupportedBy)
            .HasForeignKey(csr => csr.CapabilityId);
        modelBuilder.Entity<CapabilitySupportRelationship>()
            .HasOne<ServiceRedundancy>(csr => csr.RedundantWith)
            .WithMany(s => s.Redundancies)
            .HasForeignKey(csr => csr.RedundantWithId)
            .IsRequired(false);

        modelBuilder.Entity<Capability>()
            .ToTable("CAPABILITIES");

        modelBuilder.Entity<ServiceRedundancy>()
            .ToTable("SERVICE_REDUNDANCIES");

        modelBuilder.Entity<Issue>()
            .ToTable("ISSUES");
    }
}