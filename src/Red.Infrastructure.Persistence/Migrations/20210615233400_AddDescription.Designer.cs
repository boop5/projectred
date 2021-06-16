﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Red.Infrastructure.Persistence;

namespace Red.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(LibraryContext))]
    [Migration("20210615233400_AddDescription")]
    partial class AddDescription
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Red.Core.Domain.Models.SwitchGame", b =>
                {
                    b.Property<string>("ProductCode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Region")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Categories")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Colors")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContentRating")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("{}");

                    b.Property<bool?>("Coop")
                        .HasColumnType("bit");

                    b.Property<bool?>("DemoAvailable")
                        .HasColumnType("bit");

                    b.Property<string>("Description")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("{}");

                    b.Property<string>("Developer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("DownloadSize")
                        .HasColumnType("int");

                    b.Property<string>("EshopUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FsId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Languages")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("MaxPlayers")
                        .HasColumnType("int");

                    b.Property<string>("Media")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("MinPlayers")
                        .HasColumnType("int");

                    b.Property<string>("Nsuids")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PlayModes")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Popularity")
                        .HasColumnType("int");

                    b.Property<string>("Price")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Publisher")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ReleaseDate")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("RemovedFromEshop")
                        .HasColumnType("bit");

                    b.Property<string>("Slug")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool?>("SupportsCloudSave")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool?>("VoucherPossible")
                        .HasColumnType("bit");

                    b.HasKey("ProductCode", "Region")
                        .HasName("PK_SwitchGame_ProductCodeRegion");

                    b.HasIndex("ProductCode")
                        .IsUnique()
                        .HasDatabaseName("IX_SwitchGameProductCode");

                    b.HasIndex("Slug")
                        .HasDatabaseName("IX_SwitchGameSlug");

                    b.HasIndex("Title")
                        .HasDatabaseName("IX_SwitchGameTitle");

                    b.ToTable("Games");
                });
#pragma warning restore 612, 618
        }
    }
}
