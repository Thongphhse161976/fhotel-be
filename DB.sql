CREATE DATABASE [FHotel]
GO

CREATE TABLE [Role] (
  [RoleID] UNIQUEIDENTIFIER PRIMARY KEY,
  [RoleName] NVARCHAR(50)
)
GO

CREATE TABLE [User] (
  [UserID] UNIQUEIDENTIFIER PRIMARY KEY,
  [FirstName] NVARCHAR(50),
  [LastName] NVARCHAR(50),
  [Email] NVARCHAR(100) UNIQUE,
  [Password] NVARCHAR(255),
  [Image] NVARCHAR(MAX),
  [IdentificationNumber] NVARCHAR(50),
  [PhoneNumber] NVARCHAR(10) UNIQUE,
  [Address] NVARCHAR(255),
  [Sex] BIT,
  [HotelID] UNIQUEIDENTIFIER,
  [RoleID] UNIQUEIDENTIFIER,
  [CreatedDate] DATETIME,
  [UpdatedDate] DATETIME,
  [IsActive] BIT
)
GO

CREATE TABLE [Document] (
  [DocumentID] UNIQUEIDENTIFIER PRIMARY KEY,
  [DocumentName] NVARCHAR(50),
  [Image] NVARCHAR(MAX),
  [CreatedDate] DATETIME,
  [UpdatedDate] DATETIME,
  [UserID] UNIQUEIDENTIFIER
)
GO

CREATE TABLE [Country] (
  [CountryID] UNIQUEIDENTIFIER PRIMARY KEY,
  [CountryName] NVARCHAR(255)
)
GO

CREATE TABLE [City] (
  [CityID] UNIQUEIDENTIFIER PRIMARY KEY,
  [CityName] NVARCHAR(255),
  [PostalCode] VARCHAR(16),
  [CountryID] UNIQUEIDENTIFIER
)
GO

CREATE TABLE [HotelRegistration] (
  [HotelRegistrationID] UNIQUEIDENTIFIER PRIMARY KEY,
  [OwnerID] UNIQUEIDENTIFIER,
  [NumberOfHotels] INT,
  [Description] NVARCHAR(MAX),
  [RegistrationDate] DATETIME,
  [RegistrationStatus] NVARCHAR(50)
)
GO

CREATE TABLE [Hotel] (
  [HotelID] UNIQUEIDENTIFIER PRIMARY KEY,
  [HotelName] NVARCHAR(255),
  [Address] NVARCHAR(MAX),
  [Phone] NVARCHAR(50),
  [Email] NVARCHAR(255),
  [Description] NVARCHAR(MAX),
  [Image] NVARCHAR(MAX),
  [Star] INT,
  [CityID] UNIQUEIDENTIFIER,
  [OwnerID] UNIQUEIDENTIFIER,
  [CreatedDate] DATETIME,
  [UpdatedDate] DATETIME,
  [isActive] BIT
)
GO

CREATE TABLE [HotelAmenity] (
  [HotelAmenityID] UNIQUEIDENTIFIER PRIMARY KEY,
  [HotelID] UNIQUEIDENTIFIER,
  [Image] NVARCHAR(MAX)
)
GO

CREATE TABLE [RoomType] (
  [RoomTypeID] UNIQUEIDENTIFIER PRIMARY KEY,
  [HotelID] UNIQUEIDENTIFIER,
  [TypeName] NVARCHAR(100),
  [Description] NVARCHAR(MAX),
  [RoomSize] DECIMAL(10,2),
  [Image] NVARCHAR(MAX),
  [BasePrice] DECIMAL(10,2),
  [MaxOccupancy] INT,
  [TotalRooms] INT,
  [AvailableRooms] INT,
  [CreatedDate] DATETIME,
  [UpdatedDate] DATETIME,
  [Note] NVARCHAR(MAX)
)
GO

CREATE TABLE [RoomImage] (
  [RoomImageID] UNIQUEIDENTIFIER PRIMARY KEY,
  [RoomTypeID] UNIQUEIDENTIFIER,
  [Image] NVARCHAR(MAX)
)
GO

CREATE TABLE [RoomFacility] (
  [RoomFacilityID] UNIQUEIDENTIFIER PRIMARY KEY,
  [RoomFacilityName] NVARCHAR(100),
  [Price] DECIMAL(10,2),
  [RoomTypeID] UNIQUEIDENTIFIER
)
GO

CREATE TABLE [Room] (
  [RoomID] UNIQUEIDENTIFIER PRIMARY KEY,
  [RoomNumber] INT,
  [RoomTypeID] UNIQUEIDENTIFIER,
  [Status] NVARCHAR(50),
  [CreatedDate] DATETIME,
  [UpdatedDate] DATETIME,
  [Note] NVARCHAR(MAX)
)
GO

CREATE TABLE [RoomStayHistory] (
  [RoomStayHistoryID] UNIQUEIDENTIFIER PRIMARY KEY,
  [ReservationID] UNIQUEIDENTIFIER,
  [RoomID] UNIQUEIDENTIFIER,
  [CheckInDate] DATETIME,
  [CheckOutDate] DATETIME,
  [CreatedDate] DATETIME,
  [UpdatedDate] DATETIME
)
GO

CREATE TABLE [Reservation] (
  [ReservationID] UNIQUEIDENTIFIER PRIMARY KEY,
  [CustomerID] UNIQUEIDENTIFIER,
  [CheckInDate] DATETIME,
  [CheckOutDate] DATETIME,
  [TotalAmount] DECIMAL(10,2),
  [ReservationStatus] NVARCHAR(50),
  [CreatedDate] DATETIME,
  [ActualCheckInTime] DATETIME,
  [ActualCheckOutDate] DATETIME
)
GO

CREATE TABLE [ReservationDetail] (
  [ReservationDetailID] UNIQUEIDENTIFIER PRIMARY KEY,
  [ReservationID] UNIQUEIDENTIFIER,
  [RoomTypeID] UNIQUEIDENTIFIER UNIQUE,
  [NumberOfRooms] INT
)
GO

CREATE TABLE [Payment] (
  [PaymentID] UNIQUEIDENTIFIER PRIMARY KEY,
  [ReservationID] UNIQUEIDENTIFIER,
  [AmountPaid] DECIMAL(10,2),
  [PaymentDate] DATETIME,
  [PaymentMethodID] UNIQUEIDENTIFIER,
  [PaymentStatus] NVARCHAR(50)
)
GO

CREATE TABLE [Feedback] (
  [FeedbackID] UNIQUEIDENTIFIER PRIMARY KEY,
  [ReservationID] UNIQUEIDENTIFIER,
  [HotelRating] INT,
  [Comment] NVARCHAR(MAX),
  [CreatedDate] DATETIME
)
GO

CREATE TABLE [Timetable] (
  [TimetableID] UNIQUEIDENTIFIER PRIMARY KEY,
  [HotelID] UNIQUEIDENTIFIER,
  [StaffID] UNIQUEIDENTIFIER,
  [ShiftDate] DATETIME,
  [ShiftStart] DATETIME,
  [ShiftEnd] DATETIME,
  [Role] NVARCHAR(50),
  [CreatedDate] DATETIME,
  [UpdatedDate] DATETIME
)
GO

CREATE TABLE [ServiceType] (
  [ServiceTypeID] UNIQUEIDENTIFIER PRIMARY KEY,
  [ServiceTypeName] NVARCHAR(255)
)
GO

CREATE TABLE [Service] (
  [ServiceID] UNIQUEIDENTIFIER PRIMARY KEY,
  [ServiceName] NVARCHAR(255),
  [Price] DECIMAL(10,2),
  [Image] NVARCHAR(255),
  [Description] NVARCHAR(255),
  [ServiceTypeID] UNIQUEIDENTIFIER
)
GO

CREATE TABLE [Order] (
  [OrderID] UNIQUEIDENTIFIER PRIMARY KEY,
  [ReservationID] UNIQUEIDENTIFIER,
  [PaymentMethodID] UNIQUEIDENTIFIER,
  [OrderedDate] DATETIME,
  [OrderStatus] NVARCHAR(50)
)
GO

CREATE TABLE [OrderDetail] (
  [OrderDetailID] UNIQUEIDENTIFIER PRIMARY KEY,
  [OrderID] UNIQUEIDENTIFIER,
  [ServiceID] UNIQUEIDENTIFIER,
  [RoomFacilityID] UNIQUEIDENTIFIER,
  [Quantity] INT
)
GO

CREATE TABLE [PaymentMethod] (
  [PaymentMethodID] UNIQUEIDENTIFIER PRIMARY KEY,
  [PaymentMethodName] NVARCHAR(255)
)
GO

CREATE TABLE [RoomTypePrice] (
  [RoomTypePriceID] UNIQUEIDENTIFIER PRIMARY KEY,
  [RoomTypeID] UNIQUEIDENTIFIER,
  [DayOfWeek] NVARCHAR(10),
  [PercentageIncrease] DECIMAL(5,2)
)
GO

CREATE TABLE [RefundPolicy] (
  [RefundPolicyID] UNIQUEIDENTIFIER PRIMARY KEY,
  [CancellationTime] NVARCHAR(255),
  [RefundPercentage] DECIMAL(5,2),
  [Description] NVARCHAR(MAX),
  [CreatedDate] DATETIME,
  [UpdatedDate] DATETIME
)
GO

CREATE TABLE [Refund] (
  [RefundID] UNIQUEIDENTIFIER PRIMARY KEY,
  [PaymentID] UNIQUEIDENTIFIER,
  [RefundAmount] DECIMAL(10,2),
  [RefundStatus] NVARCHAR(50),
  [RefundDate] DATETIME,
  [RefundPolicyID] UNIQUEIDENTIFIER,
  [CreatedDate] DATETIME,
  [UpdatedDate] DATETIME
)
GO

CREATE TABLE [LateCheckOutPolicy] (
  [LateCheckOutPolicyID] UNIQUEIDENTIFIER PRIMARY KEY,
  [Description] NVARCHAR(MAX),
  [ChargePercentage] DECIMAL(5,2),
  [CreatedDate] DATETIME,
  [UpdatedDate] DATETIME
)
GO

CREATE TABLE [LateCheckOutCharge] (
  [LateCheckOutChargeID] UNIQUEIDENTIFIER PRIMARY KEY,
  [ReservationID] UNIQUEIDENTIFIER,
  [RoomID] UNIQUEIDENTIFIER,
  [LateCheckOutPolicyID] UNIQUEIDENTIFIER,
  [ChargeAmount] DECIMAL(10,2),
  [ChargeDate] DATETIME
)
GO

CREATE TABLE [Wallet] (
  [WalletID] UNIQUEIDENTIFIER PRIMARY KEY,
  [UserID] UNIQUEIDENTIFIER UNIQUE,
  [Balance] DECIMAL(10,2),
  [UpdatedDate] DATETIME
)
GO

CREATE TABLE [WalletHistory] (
  [WalletHistoryID] UNIQUEIDENTIFIER PRIMARY KEY,
  [WalletID] UNIQUEIDENTIFIER,
  [Note] DECIMAL(10,2),
  [TransactionDate] DATETIME
)
GO

CREATE TABLE [Bill] (
  [BillID] UNIQUEIDENTIFIER PRIMARY KEY,
  [ReservationID] UNIQUEIDENTIFIER,
  [TotalAmount] DECIMAL(10,2),
  [BillDate] DATETIME,
  [CreatedDate] DATETIME,
  [UpdatedDate] DATETIME,
  [BillStatus] NVARCHAR(50)
)
GO

CREATE TABLE [BillPayment] (
  [BillPaymentID] UNIQUEIDENTIFIER PRIMARY KEY,
  [BillID] UNIQUEIDENTIFIER,
  [PaymentID] UNIQUEIDENTIFIER,
  [AmountPaid] DECIMAL(10,2),
  [PaymentDate] DATETIME
)
GO

CREATE TABLE [BillOrder] (
  [BillOrderID] UNIQUEIDENTIFIER PRIMARY KEY,
  [BillID] UNIQUEIDENTIFIER,
  [OrderID] UNIQUEIDENTIFIER,
  [Amount] DECIMAL(10,2)
)
GO

CREATE TABLE [BillLateCheckOutCharge] (
  [BillLateCheckOutChargeID] UNIQUEIDENTIFIER PRIMARY KEY,
  [BillID] UNIQUEIDENTIFIER,
  [LateCheckOutChargeID] UNIQUEIDENTIFIER,
  [Amount] DECIMAL(10,2)
)
GO

ALTER TABLE [User] ADD FOREIGN KEY ([RoleID]) REFERENCES [Role] ([RoleID])
GO

ALTER TABLE [RoomType] ADD FOREIGN KEY ([HotelID]) REFERENCES [Hotel] ([HotelID])
GO

ALTER TABLE [RoomImage] ADD FOREIGN KEY ([RoomTypeID]) REFERENCES [RoomType] ([RoomTypeID])
GO

ALTER TABLE [Reservation] ADD FOREIGN KEY ([CustomerID]) REFERENCES [User] ([UserID])
GO

ALTER TABLE [ReservationDetail] ADD FOREIGN KEY ([ReservationID]) REFERENCES [Reservation] ([ReservationID])
GO

ALTER TABLE [ReservationDetail] ADD FOREIGN KEY ([RoomTypeID]) REFERENCES [RoomType] ([RoomTypeID])
GO

ALTER TABLE [Feedback] ADD FOREIGN KEY ([ReservationID]) REFERENCES [Reservation] ([ReservationID])
GO

ALTER TABLE [Order] ADD FOREIGN KEY ([ReservationID]) REFERENCES [Reservation] ([ReservationID])
GO

ALTER TABLE [OrderDetail] ADD FOREIGN KEY ([OrderID]) REFERENCES [Order] ([OrderID])
GO

ALTER TABLE [Service] ADD FOREIGN KEY ([ServiceTypeID]) REFERENCES [ServiceType] ([ServiceTypeID])
GO

ALTER TABLE [OrderDetail] ADD FOREIGN KEY ([ServiceID]) REFERENCES [Service] ([ServiceID])
GO

ALTER TABLE [Order] ADD FOREIGN KEY ([PaymentMethodID]) REFERENCES [PaymentMethod] ([PaymentMethodID])
GO

ALTER TABLE [Payment] ADD FOREIGN KEY ([PaymentMethodID]) REFERENCES [PaymentMethod] ([PaymentMethodID])
GO

ALTER TABLE [Payment] ADD FOREIGN KEY ([ReservationID]) REFERENCES [Reservation] ([ReservationID])
GO

ALTER TABLE [City] ADD FOREIGN KEY ([CountryID]) REFERENCES [Country] ([CountryID])
GO

ALTER TABLE [Hotel] ADD FOREIGN KEY ([CityID]) REFERENCES [City] ([CityID])
GO

ALTER TABLE [Document] ADD FOREIGN KEY ([UserID]) REFERENCES [User] ([UserID])
GO

ALTER TABLE [HotelAmenity] ADD FOREIGN KEY ([HotelID]) REFERENCES [Hotel] ([HotelID])
GO

ALTER TABLE [RoomFacility] ADD FOREIGN KEY ([RoomTypeID]) REFERENCES [RoomType] ([RoomTypeID])
GO

ALTER TABLE [Hotel] ADD FOREIGN KEY ([OwnerID]) REFERENCES [User] ([UserID])
GO

ALTER TABLE [Room] ADD FOREIGN KEY ([RoomTypeID]) REFERENCES [RoomType] ([RoomTypeID])
GO

ALTER TABLE [RoomTypePrice] ADD FOREIGN KEY ([RoomTypeID]) REFERENCES [RoomType] ([RoomTypeID])
GO

ALTER TABLE [HotelRegistration] ADD FOREIGN KEY ([OwnerID]) REFERENCES [User] ([UserID])
GO

ALTER TABLE [Refund] ADD FOREIGN KEY ([RefundPolicyID]) REFERENCES [RefundPolicy] ([RefundPolicyID])
GO

ALTER TABLE [LateCheckOutCharge] ADD FOREIGN KEY ([LateCheckOutPolicyID]) REFERENCES [LateCheckOutPolicy] ([LateCheckOutPolicyID])
GO

ALTER TABLE [LateCheckOutCharge] ADD FOREIGN KEY ([ReservationID]) REFERENCES [Reservation] ([ReservationID])
GO

ALTER TABLE [Refund] ADD FOREIGN KEY ([PaymentID]) REFERENCES [Payment] ([PaymentID])
GO

ALTER TABLE [Wallet] ADD FOREIGN KEY ([UserID]) REFERENCES [User] ([UserID])
GO

ALTER TABLE [WalletHistory] ADD FOREIGN KEY ([WalletID]) REFERENCES [Wallet] ([WalletID])
GO

ALTER TABLE [RoomStayHistory] ADD FOREIGN KEY ([ReservationID]) REFERENCES [Reservation] ([ReservationID])
GO

ALTER TABLE [RoomStayHistory] ADD FOREIGN KEY ([RoomID]) REFERENCES [Room] ([RoomID])
GO

ALTER TABLE [Bill] ADD FOREIGN KEY ([ReservationID]) REFERENCES [Reservation] ([ReservationID])
GO

ALTER TABLE [BillPayment] ADD FOREIGN KEY ([BillID]) REFERENCES [Bill] ([BillID])
GO

ALTER TABLE [BillOrder] ADD FOREIGN KEY ([BillID]) REFERENCES [Bill] ([BillID])
GO

ALTER TABLE [BillLateCheckOutCharge] ADD FOREIGN KEY ([BillID]) REFERENCES [Bill] ([BillID])
GO

ALTER TABLE [BillPayment] ADD FOREIGN KEY ([PaymentID]) REFERENCES [Payment] ([PaymentID])
GO

ALTER TABLE [BillOrder] ADD FOREIGN KEY ([OrderID]) REFERENCES [Order] ([OrderID])
GO

ALTER TABLE [BillLateCheckOutCharge] ADD FOREIGN KEY ([LateCheckOutChargeID]) REFERENCES [LateCheckOutCharge] ([LateCheckOutChargeID])
GO

ALTER TABLE [Timetable] ADD FOREIGN KEY ([HotelID]) REFERENCES [Hotel] ([HotelID])
GO

ALTER TABLE [Timetable] ADD FOREIGN KEY ([StaffID]) REFERENCES [User] ([UserID])
GO

ALTER TABLE [LateCheckOutCharge] ADD FOREIGN KEY ([RoomID]) REFERENCES [Room] ([RoomID])
GO
