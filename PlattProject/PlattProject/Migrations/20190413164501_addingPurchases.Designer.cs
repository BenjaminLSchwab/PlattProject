﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using PlattProject.Models;
using System;

namespace PlattProject.Migrations
{
    [DbContext(typeof(PlattContext))]
    [Migration("20190413164501_addingPurchases")]
    partial class addingPurchases
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PlattProject.Models.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Cost");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<int>("Weight");

                    b.HasKey("Id");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("PlattProject.Models.ItemStock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ItemCount");

                    b.Property<int>("ItemId");

                    b.Property<int>("WarehouseId");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("WarehouseId");

                    b.ToTable("ItemStocks");
                });

            modelBuilder.Entity("PlattProject.Models.Purchase", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ItemId");

                    b.Property<int>("NumberPurchased");

                    b.Property<DateTime>("PurchaseDate");

                    b.Property<DateTime?>("ShipDate");

                    b.Property<int>("UserId");

                    b.Property<int?>("WarehouseId");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("UserId");

                    b.ToTable("Purchases");
                });

            modelBuilder.Entity("PlattProject.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PlattProject.Models.Warehouse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<int>("MyProperty");

                    b.HasKey("Id");

                    b.ToTable("Warehouses");
                });

            modelBuilder.Entity("PlattProject.Models.ItemStock", b =>
                {
                    b.HasOne("PlattProject.Models.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PlattProject.Models.Warehouse", "Warehouse")
                        .WithMany("ItemStocks")
                        .HasForeignKey("WarehouseId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PlattProject.Models.Purchase", b =>
                {
                    b.HasOne("PlattProject.Models.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PlattProject.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}