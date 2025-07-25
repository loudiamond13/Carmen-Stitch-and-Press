﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CSP.Domain.Entities;
using CSP.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace CSP.DAL.DbContexts;

public partial class CarmenStitchAndPressDBContext : IdentityDbContext<CarmenStitchAndPressUserModel>
{
    public CarmenStitchAndPressDBContext()
    {
        
    }
    public CarmenStitchAndPressDBContext(DbContextOptions<CarmenStitchAndPressDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<Expense> Expenses { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }
    public virtual DbSet<MoneyTransfer> MoneyTransfers { get; set; }
    public virtual DbSet<CarmenStitchAndPressUserModel> CarmenStitchAndPressUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var configuration = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json")
                                .Build();
            var connection = configuration.GetConnectionString("CarmenStitchAndPressContextConnection");
            optionsBuilder.UseSqlServer(connection);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.HasKey(e => e.OrderDiscountId).HasName("PK__Discount__5EF1877EF77DDDE4");

            entity.Property(e => e.Amount).HasColumnType("money");
            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.DiscountedBy)
                .IsRequired()
                .HasMaxLength(30);
            entity.Property(e => e.UpdatedBy).HasMaxLength(30);

            entity.HasOne(d => d.Order).WithMany(p => p.Discounts)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Discounts__Updat__6477ECF3");
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.ExpensesId).HasName("PK__Expenses__DFC8A05C476D7D93");

            entity.Property(e => e.Amount).HasColumnType("money");
            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.IsCompanyExpenses).HasColumnName("isCompanyExpenses");
            entity.Property(e => e.PaidBy)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.SpendDate).HasColumnType("datetime");

            entity.HasOne(d => d.Order).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Expenses__PaidBy__6754599E");
        });

        modelBuilder.Entity<MoneyTransfer>(entity =>
        {
            entity.HasKey(e => e.MoneyTransfersId).HasName("PK__MoneyTra__606AAEC67688FCF7");

            entity.Property(e => e.TransferAmount).HasColumnType("money");
            entity.Property(e => e.TransferDate).HasColumnType("datetime");
            entity.Property(e => e.TransferFrom)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.TransferTo)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BCF8C8255AE");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DeletedBy).HasMaxLength(50);
            entity.Property(e => e.Note).HasMaxLength(100);
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.OrderName).HasMaxLength(100);
            entity.Property(e => e.PaidAmount).HasColumnType("money");
            entity.Property(e => e.TotalAmount).HasColumnType("money");
            entity.Property(e => e.TotalBalance).HasColumnType("money");
            entity.Property(e => e.TotalDiscount).HasColumnType("money");
            entity.Property(e => e.TotalExpenses).HasColumnType("money");
            entity.Property(e => e.UpdatedBy).HasMaxLength(50);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.OrderItemId).HasName("PK__OrderIte__57ED0681D6FB2133");

            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("money");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderItem__Order__6EF57B66");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A3857512FC8");

            entity.Property(e => e.Amount).HasColumnType("money");
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.DeletedBy).HasMaxLength(50);
            entity.Property(e => e.PayTo)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.PayerName).HasMaxLength(30);
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy).HasMaxLength(50);

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payments__PayTo__6A30C649");
        });

        OnModelCreatingPartial(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}