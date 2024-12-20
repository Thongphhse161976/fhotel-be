﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FHotel.Repository.Models
{
    public partial class FHotelContext : DbContext
    {
        public FHotelContext()
        {
        }

        public FHotelContext(DbContextOptions<FHotelContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Amenity> Amenities { get; set; } = null!;
        public virtual DbSet<Bill> Bills { get; set; } = null!;
        public virtual DbSet<BillTransactionImage> BillTransactionImages { get; set; } = null!;
        public virtual DbSet<CancellationPolicy> CancellationPolicies { get; set; } = null!;
        public virtual DbSet<City> Cities { get; set; } = null!;
        public virtual DbSet<District> Districts { get; set; } = null!;
        public virtual DbSet<Document> Documents { get; set; } = null!;
        public virtual DbSet<EscrowWallet> EscrowWallets { get; set; } = null!;
        public virtual DbSet<Facility> Facilities { get; set; } = null!;
        public virtual DbSet<Feedback> Feedbacks { get; set; } = null!;
        public virtual DbSet<Hotel> Hotels { get; set; } = null!;
        public virtual DbSet<HotelAmenity> HotelAmenities { get; set; } = null!;
        public virtual DbSet<HotelDocument> HotelDocuments { get; set; } = null!;
        public virtual DbSet<HotelImage> HotelImages { get; set; } = null!;
        public virtual DbSet<HotelStaff> HotelStaffs { get; set; } = null!;
        public virtual DbSet<HotelVerification> HotelVerifications { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<PaymentMethod> PaymentMethods { get; set; } = null!;
        public virtual DbSet<Reservation> Reservations { get; set; } = null!;
        public virtual DbSet<RevenuePolicy> RevenuePolicies { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Room> Rooms { get; set; } = null!;
        public virtual DbSet<RoomFacility> RoomFacilities { get; set; } = null!;
        public virtual DbSet<RoomImage> RoomImages { get; set; } = null!;
        public virtual DbSet<RoomStayHistory> RoomStayHistories { get; set; } = null!;
        public virtual DbSet<RoomType> RoomTypes { get; set; } = null!;
        public virtual DbSet<Service> Services { get; set; } = null!;
        public virtual DbSet<ServiceType> ServiceTypes { get; set; } = null!;
        public virtual DbSet<Transaction> Transactions { get; set; } = null!;
        public virtual DbSet<Type> Types { get; set; } = null!;
        public virtual DbSet<TypePricing> TypePricings { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserDocument> UserDocuments { get; set; } = null!;
        public virtual DbSet<Wallet> Wallets { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=database.techtheworld.id.vn;uid=fhotelbe;pwd=8A4C6220-D581-4798-BCF8-6A986D41D024;database=FHotel;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Amenity>(entity =>
            {
                entity.ToTable("Amenity");

                entity.Property(e => e.AmenityId)
                    .ValueGeneratedNever()
                    .HasColumnName("AmenityID");

                entity.Property(e => e.AmenityName).HasMaxLength(100);
            });

            modelBuilder.Entity<Bill>(entity =>
            {
                entity.ToTable("Bill");

                entity.HasIndex(e => e.ReservationId, "UQ__Bill__B7EE5F05E8B0708D")
                    .IsUnique();

                entity.Property(e => e.BillId)
                    .ValueGeneratedNever()
                    .HasColumnName("BillID");

                entity.Property(e => e.BillStatus).HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.LastUpdated).HasColumnType("datetime");

                entity.Property(e => e.ReservationId).HasColumnName("ReservationID");

                entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Reservation)
                    .WithOne(p => p.Bill)
                    .HasForeignKey<Bill>(d => d.ReservationId)
                    .HasConstraintName("FK__Bill__Reservatio__343573A2");
            });

            modelBuilder.Entity<BillTransactionImage>(entity =>
            {
                entity.ToTable("BillTransactionImage");

                entity.Property(e => e.BillTransactionImageId)
                    .ValueGeneratedNever()
                    .HasColumnName("BillTransactionImageID");

                entity.Property(e => e.BillId).HasColumnName("BillID");

                entity.Property(e => e.UploadedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Bill)
                    .WithMany(p => p.BillTransactionImages)
                    .HasForeignKey(d => d.BillId)
                    .HasConstraintName("FK__BillTrans__BillI__3711E04D");
            });

            modelBuilder.Entity<CancellationPolicy>(entity =>
            {
                entity.ToTable("CancellationPolicy");

                entity.Property(e => e.CancellationPolicyId)
                    .ValueGeneratedNever()
                    .HasColumnName("CancellationPolicyID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.HotelId).HasColumnName("HotelID");

                entity.Property(e => e.RefundPercentage).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.CancellationPolicies)
                    .HasForeignKey(d => d.HotelId)
                    .HasConstraintName("FK__Cancellat__Hotel__39EE4CF8");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("City");

                entity.Property(e => e.CityId)
                    .ValueGeneratedNever()
                    .HasColumnName("CityID");

                entity.Property(e => e.CityName).HasMaxLength(255);

                entity.Property(e => e.PostalCode)
                    .HasMaxLength(16)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<District>(entity =>
            {
                entity.ToTable("District");

                entity.Property(e => e.DistrictId)
                    .ValueGeneratedNever()
                    .HasColumnName("DistrictID");

                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.Property(e => e.DistrictName).HasMaxLength(255);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Districts)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK__District__CityID__1E463283");
            });

            modelBuilder.Entity<Document>(entity =>
            {
                entity.ToTable("Document");

                entity.Property(e => e.DocumentId)
                    .ValueGeneratedNever()
                    .HasColumnName("DocumentID");

                entity.Property(e => e.DocumentName).HasMaxLength(50);
            });

            modelBuilder.Entity<EscrowWallet>(entity =>
            {
                entity.ToTable("EscrowWallet");

                entity.Property(e => e.EscrowWalletId)
                    .ValueGeneratedNever()
                    .HasColumnName("EscrowWalletID");

                entity.Property(e => e.Balance).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Facility>(entity =>
            {
                entity.ToTable("Facility");

                entity.Property(e => e.FacilityId)
                    .ValueGeneratedNever()
                    .HasColumnName("FacilityID");

                entity.Property(e => e.FacilityName).HasMaxLength(100);
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.ToTable("Feedback");

                entity.Property(e => e.FeedbackId)
                    .ValueGeneratedNever()
                    .HasColumnName("FeedbackID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ReservationId).HasColumnName("ReservationID");

                entity.HasOne(d => d.Reservation)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.ReservationId)
                    .HasConstraintName("FK__Feedback__Reserv__16A510BB");
            });

            modelBuilder.Entity<Hotel>(entity =>
            {
                entity.ToTable("Hotel");

                entity.Property(e => e.HotelId)
                    .ValueGeneratedNever()
                    .HasColumnName("HotelID");

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DistrictId).HasColumnName("DistrictID");

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.HotelName).HasMaxLength(255);

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.OwnerEmail).HasMaxLength(255);

                entity.Property(e => e.OwnerId).HasColumnName("OwnerID");

                entity.Property(e => e.OwnerIdentificationNumber).HasMaxLength(50);

                entity.Property(e => e.OwnerName).HasMaxLength(255);

                entity.Property(e => e.OwnerPhoneNumber).HasMaxLength(10);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.VerifyStatus).HasMaxLength(50);

                entity.HasOne(d => d.District)
                    .WithMany(p => p.Hotels)
                    .HasForeignKey(d => d.DistrictId)
                    .HasConstraintName("FK__Hotel__DistrictI__1F3A56BC");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.Hotels)
                    .HasForeignKey(d => d.OwnerId)
                    .HasConstraintName("FK__Hotel__OwnerID__23FF0BD9");
            });

            modelBuilder.Entity<HotelAmenity>(entity =>
            {
                entity.ToTable("HotelAmenity");

                entity.Property(e => e.HotelAmenityId)
                    .ValueGeneratedNever()
                    .HasColumnName("HotelAmenityID");

                entity.Property(e => e.AmenityId).HasColumnName("AmenityID");

                entity.Property(e => e.HotelId).HasColumnName("HotelID");

                entity.HasOne(d => d.Amenity)
                    .WithMany(p => p.HotelAmenities)
                    .HasForeignKey(d => d.AmenityId)
                    .HasConstraintName("FK__HotelAmen__Ameni__2C9451DA");

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.HotelAmenities)
                    .HasForeignKey(d => d.HotelId)
                    .HasConstraintName("FK__HotelAmen__Hotel__2216C367");
            });

            modelBuilder.Entity<HotelDocument>(entity =>
            {
                entity.ToTable("HotelDocument");

                entity.Property(e => e.HotelDocumentId)
                    .ValueGeneratedNever()
                    .HasColumnName("HotelDocumentID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DocumentId).HasColumnName("DocumentID");

                entity.Property(e => e.HotelId).HasColumnName("HotelID");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Document)
                    .WithMany(p => p.HotelDocuments)
                    .HasForeignKey(d => d.DocumentId)
                    .HasConstraintName("FK__HotelDocu__Docum__33414F69");

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.HotelDocuments)
                    .HasForeignKey(d => d.HotelId)
                    .HasConstraintName("FK__HotelDocu__Hotel__324D2B30");
            });

            modelBuilder.Entity<HotelImage>(entity =>
            {
                entity.ToTable("HotelImage");

                entity.Property(e => e.HotelImageId)
                    .ValueGeneratedNever()
                    .HasColumnName("HotelImageID");

                entity.Property(e => e.HotelId).HasColumnName("HotelID");

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.HotelImages)
                    .HasForeignKey(d => d.HotelId)
                    .HasConstraintName("FK__HotelImag__Hotel__315906F7");
            });

            modelBuilder.Entity<HotelStaff>(entity =>
            {
                entity.ToTable("HotelStaff");

                entity.Property(e => e.HotelStaffId)
                    .ValueGeneratedNever()
                    .HasColumnName("HotelStaffID");

                entity.Property(e => e.HotelId).HasColumnName("HotelID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.HotelStaffs)
                    .HasForeignKey(d => d.HotelId)
                    .HasConstraintName("FK__HotelStaf__Hotel__2BA02DA1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HotelStaffs)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__HotelStaf__UserI__2AAC0968");
            });

            modelBuilder.Entity<HotelVerification>(entity =>
            {
                entity.ToTable("HotelVerification");

                entity.Property(e => e.HotelVerificationId)
                    .ValueGeneratedNever()
                    .HasColumnName("HotelVerificationID");

                entity.Property(e => e.AssignedManagerId).HasColumnName("AssignedManagerID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.HotelId).HasColumnName("HotelID");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.VerificationDate).HasColumnType("datetime");

                entity.Property(e => e.VerificationStatus).HasMaxLength(50);

                entity.HasOne(d => d.AssignedManager)
                    .WithMany(p => p.HotelVerifications)
                    .HasForeignKey(d => d.AssignedManagerId)
                    .HasConstraintName("FK__HotelVeri__Assig__352997DB");

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.HotelVerifications)
                    .HasForeignKey(d => d.HotelId)
                    .HasConstraintName("FK__HotelVeri__Hotel__361DBC14");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.OrderId)
                    .ValueGeneratedNever()
                    .HasColumnName("OrderID");

                entity.Property(e => e.BillId).HasColumnName("BillID");

                entity.Property(e => e.OrderStatus).HasMaxLength(50);

                entity.Property(e => e.OrderedDate).HasColumnType("datetime");

                entity.Property(e => e.ReservationId).HasColumnName("ReservationID");

                entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Bill)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.BillId)
                    .HasConstraintName("FK__Order__BillID__29B7E52F");

                entity.HasOne(d => d.Reservation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ReservationId)
                    .HasConstraintName("FK__Order__Reservati__179934F4");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("OrderDetail");

                entity.Property(e => e.OrderDetailId)
                    .ValueGeneratedNever()
                    .HasColumnName("OrderDetailID");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK__OrderDeta__Order__188D592D");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK__OrderDeta__Servi__1A75A19F");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.PaymentId)
                    .ValueGeneratedNever()
                    .HasColumnName("PaymentID");

                entity.Property(e => e.BillId).HasColumnName("BillID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentMethodId).HasColumnName("PaymentMethodID");

                entity.Property(e => e.PaymentStatus).HasMaxLength(50);

                entity.HasOne(d => d.Bill)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.BillId)
                    .HasConstraintName("FK__Payment__BillID__1C5DEA11");

                entity.HasOne(d => d.PaymentMethod)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.PaymentMethodId)
                    .HasConstraintName("FK__Payment__Payment__1D520E4A");
            });

            modelBuilder.Entity<PaymentMethod>(entity =>
            {
                entity.ToTable("PaymentMethod");

                entity.Property(e => e.PaymentMethodId)
                    .ValueGeneratedNever()
                    .HasColumnName("PaymentMethodID");

                entity.Property(e => e.PaymentMethodName).HasMaxLength(255);
            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.ToTable("Reservation");

                entity.Property(e => e.ReservationId)
                    .ValueGeneratedNever()
                    .HasColumnName("ReservationID");

                entity.Property(e => e.CheckInDate).HasColumnType("datetime");

                entity.Property(e => e.CheckOutDate).HasColumnType("datetime");

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.PaymentMethodId).HasColumnName("PaymentMethodID");

                entity.Property(e => e.PaymentStatus).HasMaxLength(50);

                entity.Property(e => e.ReservationStatus).HasMaxLength(50);

                entity.Property(e => e.RoomTypeId).HasColumnName("RoomTypeID");

                entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__Reservati__Custo__14BCC849");

                entity.HasOne(d => d.PaymentMethod)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.PaymentMethodId)
                    .HasConstraintName("FK__Reservati__Payme__1B69C5D8");

                entity.HasOne(d => d.RoomType)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.RoomTypeId)
                    .HasConstraintName("FK__Reservati__RoomT__15B0EC82");
            });

            modelBuilder.Entity<RevenuePolicy>(entity =>
            {
                entity.ToTable("RevenuePolicy");

                entity.Property(e => e.RevenuePolicyId)
                    .ValueGeneratedNever()
                    .HasColumnName("RevenuePolicyID");

                entity.Property(e => e.AdminPercentage).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveDate).HasColumnType("datetime");

                entity.Property(e => e.ExpiryDate).HasColumnType("datetime");

                entity.Property(e => e.HotelId).HasColumnName("HotelID");

                entity.Property(e => e.HotelPercentage).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.RevenuePolicies)
                    .HasForeignKey(d => d.HotelId)
                    .HasConstraintName("FK__RevenuePo__Hotel__38FA28BF");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.RoleId)
                    .ValueGeneratedNever()
                    .HasColumnName("RoleID");

                entity.Property(e => e.RoleName).HasMaxLength(50);
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.ToTable("Room");

                entity.Property(e => e.RoomId)
                    .ValueGeneratedNever()
                    .HasColumnName("RoomID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.RoomTypeId).HasColumnName("RoomTypeID");

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.RoomType)
                    .WithMany(p => p.Rooms)
                    .HasForeignKey(d => d.RoomTypeId)
                    .HasConstraintName("FK__Room__RoomTypeID__24F33012");
            });

            modelBuilder.Entity<RoomFacility>(entity =>
            {
                entity.ToTable("RoomFacility");

                entity.Property(e => e.RoomFacilityId)
                    .ValueGeneratedNever()
                    .HasColumnName("RoomFacilityID");

                entity.Property(e => e.FacilityId).HasColumnName("FacilityID");

                entity.Property(e => e.RoomTypeId).HasColumnName("RoomTypeID");

                entity.HasOne(d => d.Facility)
                    .WithMany(p => p.RoomFacilities)
                    .HasForeignKey(d => d.FacilityId)
                    .HasConstraintName("FK__RoomFacil__Facil__2D887613");

                entity.HasOne(d => d.RoomType)
                    .WithMany(p => p.RoomFacilities)
                    .HasForeignKey(d => d.RoomTypeId)
                    .HasConstraintName("FK__RoomFacil__RoomT__230AE7A0");
            });

            modelBuilder.Entity<RoomImage>(entity =>
            {
                entity.ToTable("RoomImage");

                entity.Property(e => e.RoomImageId)
                    .ValueGeneratedNever()
                    .HasColumnName("RoomImageID");

                entity.Property(e => e.RoomTypeId).HasColumnName("RoomTypeID");

                entity.HasOne(d => d.RoomType)
                    .WithMany(p => p.RoomImages)
                    .HasForeignKey(d => d.RoomTypeId)
                    .HasConstraintName("FK__RoomImage__RoomT__13C8A410");
            });

            modelBuilder.Entity<RoomStayHistory>(entity =>
            {
                entity.ToTable("RoomStayHistory");

                entity.Property(e => e.RoomStayHistoryId)
                    .ValueGeneratedNever()
                    .HasColumnName("RoomStayHistoryID");

                entity.Property(e => e.CheckInDate).HasColumnType("datetime");

                entity.Property(e => e.CheckOutDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ReservationId).HasColumnName("ReservationID");

                entity.Property(e => e.RoomId).HasColumnName("RoomID");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Reservation)
                    .WithMany(p => p.RoomStayHistories)
                    .HasForeignKey(d => d.ReservationId)
                    .HasConstraintName("FK__RoomStayH__Reser__27CF9CBD");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.RoomStayHistories)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("FK__RoomStayH__RoomI__28C3C0F6");
            });

            modelBuilder.Entity<RoomType>(entity =>
            {
                entity.ToTable("RoomType");

                entity.Property(e => e.RoomTypeId)
                    .ValueGeneratedNever()
                    .HasColumnName("RoomTypeID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.HotelId).HasColumnName("HotelID");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.RoomSize).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.TypeId).HasColumnName("TypeID");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.RoomTypes)
                    .HasForeignKey(d => d.HotelId)
                    .HasConstraintName("FK__RoomType__HotelI__12D47FD7");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.RoomTypes)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK__RoomType__TypeID__2E7C9A4C");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("Service");

                entity.Property(e => e.ServiceId)
                    .ValueGeneratedNever()
                    .HasColumnName("ServiceID");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.Image).HasMaxLength(255);

                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.ServiceName).HasMaxLength(255);

                entity.Property(e => e.ServiceTypeId).HasColumnName("ServiceTypeID");

                entity.HasOne(d => d.ServiceType)
                    .WithMany(p => p.Services)
                    .HasForeignKey(d => d.ServiceTypeId)
                    .HasConstraintName("FK__Service__Service__19817D66");
            });

            modelBuilder.Entity<ServiceType>(entity =>
            {
                entity.ToTable("ServiceType");

                entity.Property(e => e.ServiceTypeId)
                    .ValueGeneratedNever()
                    .HasColumnName("ServiceTypeID");

                entity.Property(e => e.ServiceTypeName).HasMaxLength(255);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("Transaction");

                entity.Property(e => e.TransactionId)
                    .ValueGeneratedNever()
                    .HasColumnName("TransactionID");

                entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.BillId).HasColumnName("BillID");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.EscrowWalletId).HasColumnName("EscrowWalletID");

                entity.Property(e => e.TransactionDate).HasColumnType("datetime");

                entity.Property(e => e.WalletId).HasColumnName("WalletID");

                entity.HasOne(d => d.Bill)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.BillId)
                    .HasConstraintName("FK__Transacti__BillI__38060486");

                entity.HasOne(d => d.EscrowWallet)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.EscrowWalletId)
                    .HasConstraintName("FK__Transacti__Escro__3AE27131");

                entity.HasOne(d => d.Wallet)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.WalletId)
                    .HasConstraintName("FK__Transacti__Walle__26DB7884");
            });

            modelBuilder.Entity<Type>(entity =>
            {
                entity.ToTable("Type");

                entity.Property(e => e.TypeId)
                    .ValueGeneratedNever()
                    .HasColumnName("TypeID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.MaxSize).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.MinSize).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.TypeName).HasMaxLength(100);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TypePricing>(entity =>
            {
                entity.ToTable("TypePricing");

                entity.Property(e => e.TypePricingId)
                    .ValueGeneratedNever()
                    .HasColumnName("TypePricingID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.DistrictId).HasColumnName("DistrictID");

                entity.Property(e => e.From).HasColumnType("datetime");

                entity.Property(e => e.PercentageIncrease).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.To).HasColumnType("datetime");

                entity.Property(e => e.TypeId).HasColumnName("TypeID");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.TypePricings)
                    .HasForeignKey(d => d.DistrictId)
                    .HasConstraintName("FK__TypePrici__Distr__3064E2BE");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.TypePricings)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK__TypePrici__TypeI__2F70BE85");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.PhoneNumber, "UQ__User__85FB4E38C1399895")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "UQ__User__A9D10534C699B0A5")
                    .IsUnique();

                entity.Property(e => e.UserId)
                    .ValueGeneratedNever()
                    .HasColumnName("UserID");

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.IdentificationNumber).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(255);

                entity.Property(e => e.PhoneNumber).HasMaxLength(10);

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__User__RoleID__11E05B9E");
            });

            modelBuilder.Entity<UserDocument>(entity =>
            {
                entity.ToTable("UserDocument");

                entity.Property(e => e.UserDocumentId)
                    .ValueGeneratedNever()
                    .HasColumnName("UserDocumentID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DocumentId).HasColumnName("DocumentID");

                entity.Property(e => e.ReservationId).HasColumnName("ReservationID");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Document)
                    .WithMany(p => p.UserDocuments)
                    .HasForeignKey(d => d.DocumentId)
                    .HasConstraintName("FK__UserDocum__Docum__21229F2E");

                entity.HasOne(d => d.Reservation)
                    .WithMany(p => p.UserDocuments)
                    .HasForeignKey(d => d.ReservationId)
                    .HasConstraintName("FK__UserDocum__Reser__202E7AF5");
            });

            modelBuilder.Entity<Wallet>(entity =>
            {
                entity.ToTable("Wallet");

                entity.Property(e => e.WalletId)
                    .ValueGeneratedNever()
                    .HasColumnName("WalletID");

                entity.Property(e => e.Balance).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.BankAccountNumber).HasMaxLength(255);

                entity.Property(e => e.BankName).HasMaxLength(250);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Wallets)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Wallet__UserID__25E7544B");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
