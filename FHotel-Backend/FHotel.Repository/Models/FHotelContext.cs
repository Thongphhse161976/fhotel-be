using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

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

        public virtual DbSet<Bill> Bills { get; set; } = null!;
        public virtual DbSet<BillLateCheckOutCharge> BillLateCheckOutCharges { get; set; } = null!;
        public virtual DbSet<BillOrder> BillOrders { get; set; } = null!;
        public virtual DbSet<BillPayment> BillPayments { get; set; } = null!;
        public virtual DbSet<City> Cities { get; set; } = null!;
        public virtual DbSet<Country> Countries { get; set; } = null!;
        public virtual DbSet<Document> Documents { get; set; } = null!;
        public virtual DbSet<Feedback> Feedbacks { get; set; } = null!;
        public virtual DbSet<Hotel> Hotels { get; set; } = null!;
        public virtual DbSet<HotelAmenity> HotelAmenities { get; set; } = null!;
        public virtual DbSet<HotelStaff> HotelStaffs { get; set; } = null!;
        public virtual DbSet<LateCheckOutCharge> LateCheckOutCharges { get; set; } = null!;
        public virtual DbSet<LateCheckOutPolicy> LateCheckOutPolicies { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<PaymentMethod> PaymentMethods { get; set; } = null!;
        public virtual DbSet<Refund> Refunds { get; set; } = null!;
        public virtual DbSet<RefundPolicy> RefundPolicies { get; set; } = null!;
        public virtual DbSet<Reservation> Reservations { get; set; } = null!;
        public virtual DbSet<ReservationDetail> ReservationDetails { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Room> Rooms { get; set; } = null!;
        public virtual DbSet<RoomFacility> RoomFacilities { get; set; } = null!;
        public virtual DbSet<RoomImage> RoomImages { get; set; } = null!;
        public virtual DbSet<RoomStayHistory> RoomStayHistories { get; set; } = null!;
        public virtual DbSet<RoomType> RoomTypes { get; set; } = null!;
        public virtual DbSet<RoomTypePrice> RoomTypePrices { get; set; } = null!;
        public virtual DbSet<Service> Services { get; set; } = null!;
        public virtual DbSet<ServiceType> ServiceTypes { get; set; } = null!;
        public virtual DbSet<Timetable> Timetables { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Wallet> Wallets { get; set; } = null!;
        public virtual DbSet<WalletHistory> WalletHistories { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json", true, true);
                IConfigurationRoot configuration = builder.Build();
                string _connectionString = configuration.GetConnectionString("MyCnn");
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bill>(entity =>
            {
                entity.ToTable("Bill");

                entity.Property(e => e.BillId)
                    .ValueGeneratedNever()
                    .HasColumnName("BillID");

                entity.Property(e => e.BillDate).HasColumnType("datetime");

                entity.Property(e => e.BillStatus).HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ReservationId).HasColumnName("ReservationID");

                entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Reservation)
                    .WithMany(p => p.Bills)
                    .HasForeignKey(d => d.ReservationId)
                    .HasConstraintName("FK__Bill__Reservatio__1FEDB87C");
            });

            modelBuilder.Entity<BillLateCheckOutCharge>(entity =>
            {
                entity.ToTable("BillLateCheckOutCharge");

                entity.Property(e => e.BillLateCheckOutChargeId)
                    .ValueGeneratedNever()
                    .HasColumnName("BillLateCheckOutChargeID");

                entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.BillId).HasColumnName("BillID");

                entity.Property(e => e.LateCheckOutChargeId).HasColumnName("LateCheckOutChargeID");

                entity.HasOne(d => d.Bill)
                    .WithMany(p => p.BillLateCheckOutCharges)
                    .HasForeignKey(d => d.BillId)
                    .HasConstraintName("FK__BillLateC__BillI__22CA2527");

                entity.HasOne(d => d.LateCheckOutCharge)
                    .WithMany(p => p.BillLateCheckOutCharges)
                    .HasForeignKey(d => d.LateCheckOutChargeId)
                    .HasConstraintName("FK__BillLateC__LateC__25A691D2");
            });

            modelBuilder.Entity<BillOrder>(entity =>
            {
                entity.ToTable("BillOrder");

                entity.Property(e => e.BillOrderId)
                    .ValueGeneratedNever()
                    .HasColumnName("BillOrderID");

                entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.BillId).HasColumnName("BillID");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.HasOne(d => d.Bill)
                    .WithMany(p => p.BillOrders)
                    .HasForeignKey(d => d.BillId)
                    .HasConstraintName("FK__BillOrder__BillI__21D600EE");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.BillOrders)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK__BillOrder__Order__24B26D99");
            });

            modelBuilder.Entity<BillPayment>(entity =>
            {
                entity.ToTable("BillPayment");

                entity.Property(e => e.BillPaymentId)
                    .ValueGeneratedNever()
                    .HasColumnName("BillPaymentID");

                entity.Property(e => e.AmountPaid).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.BillId).HasColumnName("BillID");

                entity.Property(e => e.PaymentDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentId).HasColumnName("PaymentID");

                entity.HasOne(d => d.Bill)
                    .WithMany(p => p.BillPayments)
                    .HasForeignKey(d => d.BillId)
                    .HasConstraintName("FK__BillPayme__BillI__20E1DCB5");

                entity.HasOne(d => d.Payment)
                    .WithMany(p => p.BillPayments)
                    .HasForeignKey(d => d.PaymentId)
                    .HasConstraintName("FK__BillPayme__Payme__23BE4960");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("City");

                entity.Property(e => e.CityId)
                    .ValueGeneratedNever()
                    .HasColumnName("CityID");

                entity.Property(e => e.CityName).HasMaxLength(255);

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.PostalCode)
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Cities)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK__City__CountryID__10AB74EC");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("Country");

                entity.Property(e => e.CountryId)
                    .ValueGeneratedNever()
                    .HasColumnName("CountryID");

                entity.Property(e => e.CountryName).HasMaxLength(255);
            });

            modelBuilder.Entity<Document>(entity =>
            {
                entity.ToTable("Document");

                entity.Property(e => e.DocumentId)
                    .ValueGeneratedNever()
                    .HasColumnName("DocumentID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DocumentName).HasMaxLength(50);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Documents)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Document__UserID__1293BD5E");
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
                    .HasConstraintName("FK__Feedback__Reserv__090A5324");
            });

            modelBuilder.Entity<Hotel>(entity =>
            {
                entity.ToTable("Hotel");

                entity.Property(e => e.HotelId)
                    .ValueGeneratedNever()
                    .HasColumnName("HotelID");

                entity.Property(e => e.BusinessLicenseNumber).HasMaxLength(100);

                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.HotelName).HasMaxLength(255);

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.OwnerId).HasColumnName("OwnerID");

                entity.Property(e => e.OwnerName).HasMaxLength(255);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.TaxIdentificationNumber).HasMaxLength(100);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Hotels)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK__Hotel__CityID__119F9925");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.Hotels)
                    .HasForeignKey(d => d.OwnerId)
                    .HasConstraintName("FK__Hotel__OwnerID__15702A09");
            });

            modelBuilder.Entity<HotelAmenity>(entity =>
            {
                entity.ToTable("HotelAmenity");

                entity.Property(e => e.HotelAmenityId)
                    .ValueGeneratedNever()
                    .HasColumnName("HotelAmenityID");

                entity.Property(e => e.HotelId).HasColumnName("HotelID");

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.HotelAmenities)
                    .HasForeignKey(d => d.HotelId)
                    .HasConstraintName("FK__HotelAmen__Hotel__1387E197");
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
                    .HasConstraintName("FK__HotelStaf__Hotel__2A6B46EF");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HotelStaffs)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__HotelStaf__UserI__297722B6");
            });

            modelBuilder.Entity<LateCheckOutCharge>(entity =>
            {
                entity.ToTable("LateCheckOutCharge");

                entity.Property(e => e.LateCheckOutChargeId)
                    .ValueGeneratedNever()
                    .HasColumnName("LateCheckOutChargeID");

                entity.Property(e => e.ChargeAmount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.ChargeDate).HasColumnType("datetime");

                entity.Property(e => e.LateCheckOutPolicyId).HasColumnName("LateCheckOutPolicyID");

                entity.Property(e => e.ReservationId).HasColumnName("ReservationID");

                entity.Property(e => e.RoomId).HasColumnName("RoomID");

                entity.HasOne(d => d.LateCheckOutPolicy)
                    .WithMany(p => p.LateCheckOutCharges)
                    .HasForeignKey(d => d.LateCheckOutPolicyId)
                    .HasConstraintName("FK__LateCheck__LateC__1940BAED");

                entity.HasOne(d => d.Reservation)
                    .WithMany(p => p.LateCheckOutCharges)
                    .HasForeignKey(d => d.ReservationId)
                    .HasConstraintName("FK__LateCheck__Reser__1A34DF26");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.LateCheckOutCharges)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("FK__LateCheck__RoomI__2882FE7D");
            });

            modelBuilder.Entity<LateCheckOutPolicy>(entity =>
            {
                entity.ToTable("LateCheckOutPolicy");

                entity.Property(e => e.LateCheckOutPolicyId)
                    .ValueGeneratedNever()
                    .HasColumnName("LateCheckOutPolicyID");

                entity.Property(e => e.ChargePercentage).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.OrderId)
                    .ValueGeneratedNever()
                    .HasColumnName("OrderID");

                entity.Property(e => e.OrderStatus).HasMaxLength(50);

                entity.Property(e => e.OrderedDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentMethodId).HasColumnName("PaymentMethodID");

                entity.Property(e => e.ReservationId).HasColumnName("ReservationID");

                entity.HasOne(d => d.PaymentMethod)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.PaymentMethodId)
                    .HasConstraintName("FK__Order__PaymentMe__0DCF0841");

                entity.HasOne(d => d.Reservation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ReservationId)
                    .HasConstraintName("FK__Order__Reservati__09FE775D");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("OrderDetail");

                entity.Property(e => e.OrderDetailId)
                    .ValueGeneratedNever()
                    .HasColumnName("OrderDetailID");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.RoomFacilityId).HasColumnName("RoomFacilityID");

                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK__OrderDeta__Order__0AF29B96");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK__OrderDeta__Servi__0CDAE408");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.PaymentId)
                    .ValueGeneratedNever()
                    .HasColumnName("PaymentID");

                entity.Property(e => e.AmountPaid).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.PaymentDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentMethodId).HasColumnName("PaymentMethodID");

                entity.Property(e => e.PaymentStatus).HasMaxLength(50);

                entity.Property(e => e.ReservationId).HasColumnName("ReservationID");

                entity.HasOne(d => d.PaymentMethod)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.PaymentMethodId)
                    .HasConstraintName("FK__Payment__Payment__0EC32C7A");

                entity.HasOne(d => d.Reservation)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.ReservationId)
                    .HasConstraintName("FK__Payment__Reserva__0FB750B3");
            });

            modelBuilder.Entity<PaymentMethod>(entity =>
            {
                entity.ToTable("PaymentMethod");

                entity.Property(e => e.PaymentMethodId)
                    .ValueGeneratedNever()
                    .HasColumnName("PaymentMethodID");

                entity.Property(e => e.PaymentMethodName).HasMaxLength(255);
            });

            modelBuilder.Entity<Refund>(entity =>
            {
                entity.ToTable("Refund");

                entity.Property(e => e.RefundId)
                    .ValueGeneratedNever()
                    .HasColumnName("RefundID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentId).HasColumnName("PaymentID");

                entity.Property(e => e.RefundAmount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.RefundDate).HasColumnType("datetime");

                entity.Property(e => e.RefundPolicyId).HasColumnName("RefundPolicyID");

                entity.Property(e => e.RefundStatus).HasMaxLength(50);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Payment)
                    .WithMany(p => p.Refunds)
                    .HasForeignKey(d => d.PaymentId)
                    .HasConstraintName("FK__Refund__PaymentI__1B29035F");

                entity.HasOne(d => d.RefundPolicy)
                    .WithMany(p => p.Refunds)
                    .HasForeignKey(d => d.RefundPolicyId)
                    .HasConstraintName("FK__Refund__RefundPo__184C96B4");
            });

            modelBuilder.Entity<RefundPolicy>(entity =>
            {
                entity.ToTable("RefundPolicy");

                entity.Property(e => e.RefundPolicyId)
                    .ValueGeneratedNever()
                    .HasColumnName("RefundPolicyID");

                entity.Property(e => e.CancellationTime).HasMaxLength(255);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.RefundPercentage).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.ToTable("Reservation");

                entity.Property(e => e.ReservationId)
                    .ValueGeneratedNever()
                    .HasColumnName("ReservationID");

                entity.Property(e => e.ActualCheckInTime).HasColumnType("datetime");

                entity.Property(e => e.ActualCheckOutDate).HasColumnType("datetime");

                entity.Property(e => e.CheckInDate).HasColumnType("datetime");

                entity.Property(e => e.CheckOutDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.ReservationStatus).HasMaxLength(50);

                entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__Reservati__Custo__062DE679");
            });

            modelBuilder.Entity<ReservationDetail>(entity =>
            {
                entity.ToTable("ReservationDetail");

                entity.HasIndex(e => e.RoomTypeId, "UQ__Reservat__BCC89610F8AC69CC")
                    .IsUnique();

                entity.Property(e => e.ReservationDetailId)
                    .ValueGeneratedNever()
                    .HasColumnName("ReservationDetailID");

                entity.Property(e => e.ReservationId).HasColumnName("ReservationID");

                entity.Property(e => e.RoomTypeId).HasColumnName("RoomTypeID");

                entity.HasOne(d => d.Reservation)
                    .WithMany(p => p.ReservationDetails)
                    .HasForeignKey(d => d.ReservationId)
                    .HasConstraintName("FK__Reservati__Reser__07220AB2");

                entity.HasOne(d => d.RoomType)
                    .WithOne(p => p.ReservationDetail)
                    .HasForeignKey<ReservationDetail>(d => d.RoomTypeId)
                    .HasConstraintName("FK__Reservati__RoomT__08162EEB");
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
                    .HasConstraintName("FK__Room__RoomTypeID__16644E42");
            });

            modelBuilder.Entity<RoomFacility>(entity =>
            {
                entity.ToTable("RoomFacility");

                entity.Property(e => e.RoomFacilityId)
                    .ValueGeneratedNever()
                    .HasColumnName("RoomFacilityID");

                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.RoomFacilityName).HasMaxLength(100);

                entity.Property(e => e.RoomTypeId).HasColumnName("RoomTypeID");

                entity.HasOne(d => d.RoomType)
                    .WithMany(p => p.RoomFacilities)
                    .HasForeignKey(d => d.RoomTypeId)
                    .HasConstraintName("FK__RoomFacil__RoomT__147C05D0");
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
                    .HasConstraintName("FK__RoomImage__RoomT__0539C240");
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
                    .HasConstraintName("FK__RoomStayH__Reser__1E05700A");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.RoomStayHistories)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("FK__RoomStayH__RoomI__1EF99443");
            });

            modelBuilder.Entity<RoomType>(entity =>
            {
                entity.ToTable("RoomType");

                entity.Property(e => e.RoomTypeId)
                    .ValueGeneratedNever()
                    .HasColumnName("RoomTypeID");

                entity.Property(e => e.BasePrice).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.HotelId).HasColumnName("HotelID");

                entity.Property(e => e.RoomSize).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.TypeName).HasMaxLength(100);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.RoomTypes)
                    .HasForeignKey(d => d.HotelId)
                    .HasConstraintName("FK__RoomType__HotelI__04459E07");
            });

            modelBuilder.Entity<RoomTypePrice>(entity =>
            {
                entity.ToTable("RoomTypePrice");

                entity.Property(e => e.RoomTypePriceId)
                    .ValueGeneratedNever()
                    .HasColumnName("RoomTypePriceID");

                entity.Property(e => e.DayOfWeek).HasMaxLength(10);

                entity.Property(e => e.PercentageIncrease).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.RoomTypeId).HasColumnName("RoomTypeID");

                entity.HasOne(d => d.RoomType)
                    .WithMany(p => p.RoomTypePrices)
                    .HasForeignKey(d => d.RoomTypeId)
                    .HasConstraintName("FK__RoomTypeP__RoomT__1758727B");
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
                    .HasConstraintName("FK__Service__Service__0BE6BFCF");
            });

            modelBuilder.Entity<ServiceType>(entity =>
            {
                entity.ToTable("ServiceType");

                entity.Property(e => e.ServiceTypeId)
                    .ValueGeneratedNever()
                    .HasColumnName("ServiceTypeID");

                entity.Property(e => e.ServiceTypeName).HasMaxLength(255);
            });

            modelBuilder.Entity<Timetable>(entity =>
            {
                entity.ToTable("Timetable");

                entity.Property(e => e.TimetableId)
                    .ValueGeneratedNever()
                    .HasColumnName("TimetableID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.HotelId).HasColumnName("HotelID");

                entity.Property(e => e.Role).HasMaxLength(50);

                entity.Property(e => e.ShiftDate).HasColumnType("datetime");

                entity.Property(e => e.ShiftEnd).HasColumnType("datetime");

                entity.Property(e => e.ShiftStart).HasColumnType("datetime");

                entity.Property(e => e.StaffId).HasColumnName("StaffID");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.Timetables)
                    .HasForeignKey(d => d.HotelId)
                    .HasConstraintName("FK__Timetable__Hotel__269AB60B");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.Timetables)
                    .HasForeignKey(d => d.StaffId)
                    .HasConstraintName("FK__Timetable__Staff__278EDA44");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.PhoneNumber, "UQ__User__85FB4E38014FB5E8")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "UQ__User__A9D10534D155B369")
                    .IsUnique();

                entity.Property(e => e.UserId)
                    .ValueGeneratedNever()
                    .HasColumnName("UserID");

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.IdentificationNumber).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(255);

                entity.Property(e => e.PhoneNumber).HasMaxLength(10);

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__User__RoleID__035179CE");
            });

            modelBuilder.Entity<Wallet>(entity =>
            {
                entity.ToTable("Wallet");

                entity.HasIndex(e => e.UserId, "UQ__Wallet__1788CCAD6E5793B4")
                    .IsUnique();

                entity.Property(e => e.WalletId)
                    .ValueGeneratedNever()
                    .HasColumnName("WalletID");

                entity.Property(e => e.Balance).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.Wallet)
                    .HasForeignKey<Wallet>(d => d.UserId)
                    .HasConstraintName("FK__Wallet__UserID__1C1D2798");
            });

            modelBuilder.Entity<WalletHistory>(entity =>
            {
                entity.ToTable("WalletHistory");

                entity.Property(e => e.WalletHistoryId)
                    .ValueGeneratedNever()
                    .HasColumnName("WalletHistoryID");

                entity.Property(e => e.Note).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.TransactionDate).HasColumnType("datetime");

                entity.Property(e => e.WalletId).HasColumnName("WalletID");

                entity.HasOne(d => d.Wallet)
                    .WithMany(p => p.WalletHistories)
                    .HasForeignKey(d => d.WalletId)
                    .HasConstraintName("FK__WalletHis__Walle__1D114BD1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
