﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using YourBudget.API.Models;

namespace YourBudget.API.Migrations
{
    [DbContext(typeof(BudgetContext))]
    partial class BudgetContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("YourBudget.API.Models.Budget", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Balance");

                    b.Property<DateTime>("Opened");

                    b.Property<Guid>("OwnerId");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Budgets");
                });

            modelBuilder.Entity("YourBudget.API.Models.BudgetItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("BudgetId");

                    b.Property<double>("Credit");

                    b.Property<double>("Debet");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<int>("Order");

                    b.Property<double>("Planned");

                    b.Property<bool>("Сumulative");

                    b.HasKey("Id");

                    b.HasIndex("BudgetId");

                    b.ToTable("BudgetItems");
                });

            modelBuilder.Entity("YourBudget.API.Models.BudgetUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Allowed");

                    b.Property<Guid>("BudgetId");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("BudgetId");

                    b.HasIndex("UserId");

                    b.ToTable("BudgetUsers");
                });

            modelBuilder.Entity("YourBudget.API.Models.Operation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("ActorId");

                    b.Property<double>("Amount");

                    b.Property<Guid>("BudgetItemId");

                    b.Property<string>("Category")
                        .HasMaxLength(250);

                    b.Property<DateTime>("Commited")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Discription");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("ActorId");

                    b.HasIndex("BudgetItemId");

                    b.ToTable("Operations");
                });

            modelBuilder.Entity("YourBudget.API.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Language")
                        .HasMaxLength(100);

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Mail")
                        .HasMaxLength(100);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<string>("PassHash")
                        .HasMaxLength(200);

                    b.Property<string>("ResetToken")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("YourBudget.API.Models.Budget", b =>
                {
                    b.HasOne("YourBudget.API.Models.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("YourBudget.API.Models.BudgetItem", b =>
                {
                    b.HasOne("YourBudget.API.Models.Budget", "Budget")
                        .WithMany("Items")
                        .HasForeignKey("BudgetId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("YourBudget.API.Models.BudgetUser", b =>
                {
                    b.HasOne("YourBudget.API.Models.Budget", "Budget")
                        .WithMany("Users")
                        .HasForeignKey("BudgetId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("YourBudget.API.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("YourBudget.API.Models.Operation", b =>
                {
                    b.HasOne("YourBudget.API.Models.User", "Actor")
                        .WithMany()
                        .HasForeignKey("ActorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("YourBudget.API.Models.BudgetItem", "BudgetItem")
                        .WithMany("Operations")
                        .HasForeignKey("BudgetItemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
