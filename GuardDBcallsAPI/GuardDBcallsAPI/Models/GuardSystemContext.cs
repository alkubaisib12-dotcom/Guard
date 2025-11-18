using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GuardDBcallsAPI.Models;

public partial class GuardSystemContext : DbContext
{
    public GuardSystemContext()
    {
    }

    public GuardSystemContext(DbContextOptions<GuardSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<HazardLog> HazardLogs { get; set; }

    public virtual DbSet<Irblaster> Irblasters { get; set; }

    public virtual DbSet<RecommendationsLog> RecommendationsLogs { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<SmartPlug> SmartPlugs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("data source =MAS\\SQLEXPRESS;initial catalog= GuardSystem;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<HazardLog>(entity =>
        {
            entity.HasKey(e => e.HazardLogId).HasName("PK__HazardLo__65A8DF107410F4D7");

            entity.Property(e => e.Time)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Room).WithMany(p => p.HazardLogs)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK__HazardLog__RoomI__59FA5E80");
        });

        modelBuilder.Entity<Irblaster>(entity =>
        {
            entity.HasKey(e => e.Irid).HasName("PK__IRBlaste__8E354EB8CCCACCA5");

            entity.ToTable("IRBlaster");

            entity.Property(e => e.Irid).HasColumnName("IRID");
            entity.Property(e => e.TotalDevices).HasDefaultValue(0);

            entity.HasOne(d => d.Room).WithMany(p => p.Irblasters)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK__IRBlaster__RoomI__534D60F1");

            entity.HasOne(d => d.User).WithMany(p => p.Irblasters)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__IRBlaster__UserI__52593CB8");
        });

        modelBuilder.Entity<RecommendationsLog>(entity =>
        {
            entity.HasKey(e => e.RecommendationId).HasName("PK__Recommen__AA15BEE4235BDB6F");

            entity.ToTable("RecommendationsLog");

            entity.HasOne(d => d.HazardLog).WithMany(p => p.RecommendationsLogs)
                .HasForeignKey(d => d.HazardLogId)
                .HasConstraintName("FK__Recommend__Hazar__5DCAEF64");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.RoomId).HasName("PK__Rooms__32863939CFBB08EA");

            entity.Property(e => e.AirSensorApikey)
                .HasMaxLength(255)
                .HasColumnName("AirSensor_APIKey");
            entity.Property(e => e.CameraIpaddress)
                .HasMaxLength(100)
                .HasColumnName("Camera_IPAddress");
            entity.Property(e => e.CameraPassword)
                .HasMaxLength(255)
                .HasColumnName("Camera_Password");
            entity.Property(e => e.CameraUsername)
                .HasMaxLength(100)
                .HasColumnName("Camera_Username");
            entity.Property(e => e.Irid).HasColumnName("IRID");
            entity.Property(e => e.RoomName).HasMaxLength(100);
            entity.Property(e => e.ShellyFloodDeviceIp)
                .HasMaxLength(100)
                .HasColumnName("ShellyFlood_DeviceIP");
            entity.Property(e => e.ShellyPmminiDeviceIp)
                .HasMaxLength(100)
                .HasColumnName("ShellyPMMini_DeviceIP");
            entity.Property(e => e.TotalDevices).HasDefaultValue(0);
            entity.Property(e => e.TotalSmartPlugs).HasDefaultValue(0);

            entity.HasOne(d => d.User).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Rooms__UserId__4D94879B");
        });

        modelBuilder.Entity<SmartPlug>(entity =>
        {
            entity.HasKey(e => e.SmartPlugId).HasName("PK__SmartPlu__15A71BAAA66FECBE");

            entity.ToTable("SmartPlug");

            entity.Property(e => e.SmartPlugId).HasColumnName("SmartPlugID");
            entity.Property(e => e.LocationInRoom).HasMaxLength(100);
            entity.Property(e => e.SmartPlugDeviceId).HasMaxLength(100);
            entity.Property(e => e.SmartPlugRegion).HasMaxLength(50);

            entity.HasOne(d => d.Room).WithMany(p => p.SmartPlugsNavigation)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK__SmartPlug__RoomI__571DF1D5");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C6FD3CA0D");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E421E87E6D").IsUnique();

            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.TotalRooms).HasDefaultValue(0);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
