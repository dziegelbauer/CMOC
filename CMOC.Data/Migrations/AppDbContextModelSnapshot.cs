﻿// <auto-generated />
using System;
using CMOC.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CMOC.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.10");

            modelBuilder.Entity("CMOC.Domain.Capability", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("CAPABILITIES", (string)null);
                });

            modelBuilder.Entity("CMOC.Domain.CapabilitySupportRelationship", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CapabilityId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("RedundantWithId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ServiceId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CapabilityId");

                    b.HasIndex("RedundantWithId");

                    b.HasIndex("ServiceId");

                    b.ToTable("CAPABILITY_SUPPORT_RELATIONSHIPS", (string)null);
                });

            modelBuilder.Entity("CMOC.Domain.Component", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ComponentOfId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IssueId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Operational")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SerialNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("TypeId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ComponentOfId");

                    b.HasIndex("IssueId");

                    b.HasIndex("TypeId");

                    b.ToTable("COMPONENTS", (string)null);
                });

            modelBuilder.Entity("CMOC.Domain.ComponentRelationship", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("EquipmentId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FailureThreshold")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TypeId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("EquipmentId");

                    b.HasIndex("TypeId");

                    b.ToTable("COMPONENT_RELATIONSHIPS", (string)null);
                });

            modelBuilder.Entity("CMOC.Domain.ComponentType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("COMPONENT_TYPES", (string)null);
                });

            modelBuilder.Entity("CMOC.Domain.Equipment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IssueId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LocationId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool?>("OperationalOverride")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SerialNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("TypeId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("IssueId");

                    b.HasIndex("LocationId");

                    b.HasIndex("TypeId");

                    b.ToTable("EQUIPMENT", (string)null);
                });

            modelBuilder.Entity("CMOC.Domain.EquipmentRedundancy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("EQUIPMENT_REDUNDANCIES", (string)null);
                });

            modelBuilder.Entity("CMOC.Domain.EquipmentType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("EQUIPMENT_TYPES", (string)null);
                });

            modelBuilder.Entity("CMOC.Domain.Issue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ExpectedCompletion")
                        .HasColumnType("TEXT");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("TicketNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ISSUES", (string)null);
                });

            modelBuilder.Entity("CMOC.Domain.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("LOCATIONS", (string)null);
                });

            modelBuilder.Entity("CMOC.Domain.Service", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("SERVICES", (string)null);
                });

            modelBuilder.Entity("CMOC.Domain.ServiceRedundancy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("SERVICE_REDUNDANCIES", (string)null);
                });

            modelBuilder.Entity("CMOC.Domain.ServiceSupportRelationship", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("EquipmentId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FailureThreshold")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("RedundantWithId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ServiceId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TypeId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("EquipmentId");

                    b.HasIndex("RedundantWithId");

                    b.HasIndex("ServiceId");

                    b.HasIndex("TypeId");

                    b.ToTable("SERVICE_SUPPORT_RELATIONSHIPS", (string)null);
                });

            modelBuilder.Entity("CMOC.Domain.CapabilitySupportRelationship", b =>
                {
                    b.HasOne("CMOC.Domain.Capability", "Capability")
                        .WithMany("SupportedBy")
                        .HasForeignKey("CapabilityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CMOC.Domain.ServiceRedundancy", "RedundantWith")
                        .WithMany("Redundancies")
                        .HasForeignKey("RedundantWithId");

                    b.HasOne("CMOC.Domain.Service", "Service")
                        .WithMany("Supports")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Capability");

                    b.Navigation("RedundantWith");

                    b.Navigation("Service");
                });

            modelBuilder.Entity("CMOC.Domain.Component", b =>
                {
                    b.HasOne("CMOC.Domain.ComponentRelationship", "ComponentOf")
                        .WithMany("Components")
                        .HasForeignKey("ComponentOfId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CMOC.Domain.Issue", "Issue")
                        .WithMany()
                        .HasForeignKey("IssueId");

                    b.HasOne("CMOC.Domain.ComponentType", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ComponentOf");

                    b.Navigation("Issue");

                    b.Navigation("Type");
                });

            modelBuilder.Entity("CMOC.Domain.ComponentRelationship", b =>
                {
                    b.HasOne("CMOC.Domain.Equipment", "Equipment")
                        .WithMany("Components")
                        .HasForeignKey("EquipmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CMOC.Domain.ComponentType", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Equipment");

                    b.Navigation("Type");
                });

            modelBuilder.Entity("CMOC.Domain.Equipment", b =>
                {
                    b.HasOne("CMOC.Domain.Issue", "Issue")
                        .WithMany()
                        .HasForeignKey("IssueId");

                    b.HasOne("CMOC.Domain.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CMOC.Domain.EquipmentType", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Issue");

                    b.Navigation("Location");

                    b.Navigation("Type");
                });

            modelBuilder.Entity("CMOC.Domain.ServiceSupportRelationship", b =>
                {
                    b.HasOne("CMOC.Domain.Equipment", "Equipment")
                        .WithMany("Relationships")
                        .HasForeignKey("EquipmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CMOC.Domain.EquipmentRedundancy", "RedundantWith")
                        .WithMany("Redundancies")
                        .HasForeignKey("RedundantWithId");

                    b.HasOne("CMOC.Domain.Service", "Service")
                        .WithMany("SupportedBy")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CMOC.Domain.EquipmentType", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Equipment");

                    b.Navigation("RedundantWith");

                    b.Navigation("Service");

                    b.Navigation("Type");
                });

            modelBuilder.Entity("CMOC.Domain.Capability", b =>
                {
                    b.Navigation("SupportedBy");
                });

            modelBuilder.Entity("CMOC.Domain.ComponentRelationship", b =>
                {
                    b.Navigation("Components");
                });

            modelBuilder.Entity("CMOC.Domain.Equipment", b =>
                {
                    b.Navigation("Components");

                    b.Navigation("Relationships");
                });

            modelBuilder.Entity("CMOC.Domain.EquipmentRedundancy", b =>
                {
                    b.Navigation("Redundancies");
                });

            modelBuilder.Entity("CMOC.Domain.Service", b =>
                {
                    b.Navigation("SupportedBy");

                    b.Navigation("Supports");
                });

            modelBuilder.Entity("CMOC.Domain.ServiceRedundancy", b =>
                {
                    b.Navigation("Redundancies");
                });
#pragma warning restore 612, 618
        }
    }
}
