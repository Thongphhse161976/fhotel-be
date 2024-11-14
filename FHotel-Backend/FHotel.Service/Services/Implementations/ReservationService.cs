using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.Bills;
using FHotel.Service.DTOs.Orders;
using FHotel.Service.DTOs.Reservations;
using FHotel.Service.DTOs.RoomStayHistories;
using FHotel.Service.DTOs.RoomTypes;
using FHotel.Service.DTOs.Transactions;
using FHotel.Service.DTOs.Wallets;
using FHotel.Service.Services.Implementations;
using FHotel.Service.Services.Interfaces;
using FHotel.Service.Validators.ReservationValidator;
using FHotel.Services.DTOs.OrderDetails;
using FHotel.Services.DTOs.Reservations;
using FHotel.Services.Services.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FHotel.Repository.SMTPs.Models;
using Microsoft.Extensions.Configuration;

namespace FHotel.Services.Services.Implementations
{
    public class ReservationService : IReservationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private readonly IRoomTypeService _roomTypeService;
        private readonly ITypePricingService _typePricingService;
        private readonly Lazy<IRoomStayHistoryService> _roomStayHistoryService;
        private readonly IServiceProvider _serviceProvider;
        private IWalletService _walletService;
        private readonly object _lockObject = new object();

        //private readonly IBillService _billService;
        public ReservationService(IUnitOfWork unitOfWork, IMapper mapper, IRoomTypeService roomTypeService,
            ITypePricingService typePricingService, Lazy<IRoomStayHistoryService> roomStayHistoryService, IServiceProvider serviceProvider, IWalletService walletService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _roomTypeService = roomTypeService;
            _typePricingService = typePricingService;
            _roomStayHistoryService = roomStayHistoryService;
            _serviceProvider = serviceProvider;
            _walletService = walletService;
        }

        public async Task<List<ReservationResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<Reservation>().GetAll()
                                            .ProjectTo<ReservationResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            // Check if list is null or empty
            if (list == null || !list.Any())
            {
                throw new Exception("No reservations found.");
            }
            return list;
        }

        public async Task<ReservationResponse> Get(Guid id)
        {
            try
            {
                ReservationResponse reservation = null;
                reservation = await _unitOfWork.Repository<Reservation>().GetAll()
                     .AsNoTracking()
                     .ProjectTo<ReservationResponse>(_mapper.ConfigurationProvider)
                    .Where(x => x.ReservationId == id)
                    .FirstOrDefaultAsync();

                if (reservation == null)
                {
                    throw new Exception("Reservation not found");
                }

                return reservation;
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<ReservationResponse> Create(ReservationCreateRequest request)
        {
            var validator = new ReservationCreateRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            // Set the UTC offset for UTC+7
            TimeSpan utcOffset = TimeSpan.FromHours(7);

            // Get the current UTC time
            DateTime utcNow = DateTime.UtcNow;

            // Convert the UTC time to UTC+7
            DateTime localTime = utcNow + utcOffset;
            string userCode = await GetUserCode(request.CustomerId.Value); // Assume this method gets or generates the user's code
            string formattedTime = localTime.ToString("yyyyMMddHHmmss");
            string reservationCode = $"FRSVT{userCode}{formattedTime}";
            try
            {
                int availableRooms = await _roomTypeService.CountAvailableRoomsInRangeAsync((Guid)request.RoomTypeId, (DateTime)request.CheckInDate, (DateTime)request.CheckOutDate);

                // Check if there are enough available rooms
                if (availableRooms < request.NumberOfRooms)
                {
                    throw new Exception("Not enough available rooms.");
                }

                int index = 1; // or however you want to start the index
                // Proceed with creating the reservation
                var reservation = _mapper.Map<ReservationCreateRequest, Reservation>(request);
                reservation.ReservationId = Guid.NewGuid();
                reservation.Code = reservationCode; // Generates a unique code each time
                reservation.CreatedDate = localTime;
                reservation.ReservationStatus = "Pending";
                reservation.PaymentStatus = "Not Paid";
                reservation.IsPrePaid = false;

                await _unitOfWork.Repository<Reservation>().InsertAsync(reservation);

                // Commit both changes: the room type update and the new reservation
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Reservation, ReservationResponse>(reservation);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        private async Task<string> GetUserCode(Guid userId)
        {
            // Assuming you have a method to fetch or generate the user code from the user entity
            var user = await _unitOfWork.Repository<User>().FindAsync(u => u.UserId == userId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }
            return user.Code; // or generate a code based on user info, e.g., username initials
        }





        public async Task<ReservationResponse> Delete(Guid id)
        {
            try
            {
                Reservation reservation = null;
                reservation = _unitOfWork.Repository<Reservation>()
                    .Find(p => p.ReservationId == id);
                if (reservation == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<Reservation>().HardDeleteGuid(reservation.ReservationId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<Reservation, ReservationResponse>(reservation);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ReservationResponse> Update(Guid id, ReservationUpdateRequest request)
        {
            // Set the UTC offset for UTC+7
            TimeSpan utcOffset = TimeSpan.FromHours(7);

            // Get the current UTC time
            DateTime utcNow = DateTime.UtcNow;

            // Convert the UTC time to UTC+7
            DateTime localTime = utcNow + utcOffset;
            // Fetch the existing reservation
            var reservation = await _unitOfWork.Repository<Reservation>().FindAsync(x => x.ReservationId == id);

            // Check if the reservation exists
            if (reservation == null)
            {
                throw new KeyNotFoundException($"Reservation with ID {id} does not exist.");
            }

            // Validate the request (consider using a separate validator)
            var validator = new ReservationUpdateRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            try
            {
                var updateReservation = _mapper.Map(request, reservation);

                if (updateReservation.ReservationStatus == "Cancelled")
                {
                    var roomType = await _roomTypeService.Get(updateReservation.RoomTypeId.Value);
                    //roomType.AvailableRooms += updateReservation.NumberOfRooms;
                    await _roomTypeService.Update(roomType.RoomTypeId, new RoomTypeUpdateRequest
                    {
                        RoomTypeId = roomType.RoomTypeId,
                        AvailableRooms = roomType.AvailableRooms,
                        TotalRooms = roomType.TotalRooms,
                        HotelId = roomType.HotelId,
                        TypeId = roomType.TypeId,
                        Description = roomType.Description,
                        RoomSize = roomType.RoomSize,
                        IsActive = roomType.IsActive,
                        Note = roomType.Note,
                    });
                }
                else if (updateReservation.ReservationStatus == "CheckIn")
                {
                    var roomType = await _roomTypeService.Get(updateReservation.RoomTypeId.Value);
                    //roomType.AvailableRooms -= updateReservation.NumberOfRooms;
                    await _roomTypeService.Update(roomType.RoomTypeId, new RoomTypeUpdateRequest
                    {
                        RoomTypeId = roomType.RoomTypeId,
                        AvailableRooms = roomType.AvailableRooms,
                        TotalRooms = roomType.TotalRooms,
                        HotelId = roomType.HotelId,
                        TypeId = roomType.TypeId,
                        Description = roomType.Description,
                        RoomSize = roomType.RoomSize,
                        IsActive = roomType.IsActive,
                        Note = roomType.Note,
                    });
                }
                else if (updateReservation.ReservationStatus == "CheckOut")
                {
                    var roomType = await _roomTypeService.Get(updateReservation.RoomTypeId.Value);
                    await _roomTypeService.Update(roomType.RoomTypeId, new RoomTypeUpdateRequest
                    {
                        RoomTypeId = roomType.RoomTypeId,
                        AvailableRooms = roomType.AvailableRooms,
                        TotalRooms = roomType.TotalRooms,
                        HotelId = roomType.HotelId,
                        TypeId = roomType.TypeId,
                        Description = roomType.Description,
                        RoomSize = roomType.RoomSize,
                        IsActive = roomType.IsActive,
                        Note = roomType.Note,
                    });

                    // Retrieve all room stay histories associated with the reservation
                    var roomStayHistories = await _roomStayHistoryService.Value.GetAllByReservationId(id);

                    // Iterate over each room stay history and update individually
                    foreach (var history in roomStayHistories)
                    {
                        var roomStayHistoryUpdate = new RoomStayHistoryRequest()
                        {
                            CheckInDate = history.CheckInDate,
                            CheckOutDate = localTime,
                            RoomId = history.RoomId,
                            CreatedDate = history.CreatedDate,
                            ReservationId = history.ReservationId,
                            UpdatedDate = history.UpdatedDate
                        };

                        // Update each history one by one
                        await _roomStayHistoryService.Value.Update(history.RoomStayHistoryId, roomStayHistoryUpdate);
                    }
                }

                await _unitOfWork.Repository<Reservation>().UpdateDetached(updateReservation);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Reservation, ReservationResponse>(reservation);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<decimal> CalculateTotalAmount(Guid roomTypeId, DateTime checkInDate, DateTime checkOutDate, int numberOfRooms)
        {
            var roomType = await _roomTypeService.Get(roomTypeId);

            if (roomType == null || roomType.IsActive != true || numberOfRooms <= 0)
            {
                throw new ArgumentException("Invalid room type or number of rooms.");
            }

            decimal totalAmount = 0;

            // Get the district ID from the room type
            var districtId = roomType.Hotel.DistrictId;

            for (DateTime currentDate = checkInDate.Date; currentDate < checkOutDate.Date; currentDate = currentDate.AddDays(1))
            {
                int dayOfWeek = (int)currentDate.DayOfWeek == 0 ? 7 : (int)currentDate.DayOfWeek;

                var dailyPricing = await _typePricingService.GetPricingByTypeAndDistrict(roomType.TypeId ?? Guid.Empty, districtId ?? Guid.Empty, dayOfWeek);

                if (dailyPricing == null)
                {
                    throw new Exception($"No pricing available for {currentDate.ToShortDateString()}.");
                }

                totalAmount += dailyPricing.Price.Value * numberOfRooms;
            }

            return totalAmount;
        }

        public async Task<List<ReservationResponse>> GetAllByHotelId(Guid id)
        {

            var list = await _unitOfWork.Repository<Reservation>().GetAll()
                                            .Where(r => r.RoomType.HotelId == id)
                                            .ProjectTo<ReservationResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            // Check if list is null or empty
            if (list == null || !list.Any())
            {
                throw new Exception("No reservations found.");
            }
            return list;
        }
        public async Task<List<ReservationResponse>> GetAllByUserId(Guid id)
        {

            var list = await _unitOfWork.Repository<Reservation>().GetAll()
                                            .Where(r => r.CustomerId == id)
                                            .ProjectTo<ReservationResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            // Check if list is null or empty
            if (list == null || !list.Any())
            {
                throw new Exception("No reservations found.");
            }
            return list;
        }
        public async Task<List<ReservationResponse>> GetAllByOwnerId(Guid id)
        {

            var list = await _unitOfWork.Repository<Reservation>().GetAll()
                                            .Where(r => r.RoomType.Hotel.OwnerId == id)
                                            .ProjectTo<ReservationResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            // Check if list is null or empty
            if (list == null || !list.Any())
            {
                throw new Exception("No reservations found.");
            }
            return list;
        }


        public async Task<List<ReservationResponse>> GetAllReservationByStaffId(Guid staffId)
        {
            // Retrieve the HotelID associated with the HotelStaff
            var hotelStaff = await _unitOfWork.Repository<HotelStaff>()
                                              .GetAll()
                                              .Where(hs => hs.UserId == staffId)
                                              .FirstOrDefaultAsync();

            if (hotelStaff == null)
            {
                throw new Exception("Staff not found or not associated with any hotel.");
            }

            var hotelId = hotelStaff.HotelId;

            // Retrieve all reservations for the hotel associated with the staff member
            var reservations = await _unitOfWork.Repository<Reservation>()
                                                .GetAll()
                                                .Where(r => r.RoomType.HotelId == hotelId) // Assuming RoomTypeID or some other way links to the hotel
                                                .ProjectTo<ReservationResponse>(_mapper.ConfigurationProvider)
                                                .ToListAsync();

            // Check if any reservations were found
            if (reservations == null || !reservations.Any())
            {
                throw new Exception("No reservations found for this staff's hotel.");
            }

            return reservations;
        }

        public async Task<List<ReservationResponse>> GetAllByRoomTypeId(Guid id)
        {

            var list = await _unitOfWork.Repository<Reservation>().GetAll()
                                            .Where(r => r.RoomTypeId == id)
                                            .ProjectTo<ReservationResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            // Check if list is null or empty
            if (list == null || !list.Any())
            {
                throw new Exception("No reservations found.");
            }
            return list;
        }

        public async Task<List<ReservationResponse>> SearchReservations(Guid staffId, string? query)
        {
            if (string.IsNullOrEmpty(query))
                return new List<ReservationResponse>();

            query = query.ToLower();
            var reservations = await GetAllReservationByStaffId(staffId); // Await the task to get List<Reservation>

            var filteredList = reservations
                                .Where(r => r.Code.ToLower().Contains(query)
                                        || (r.Customer.Name?.ToLower().Contains(query) ?? false)
                                        || (r.Customer.Email?.ToLower().Contains(query) ?? false)
                                        || (r.Customer.PhoneNumber?.ToLower().Contains(query) ?? false)
                                        || (r.Customer.IdentificationNumber?.ToLower().Contains(query) ?? false))
                                .AsQueryable()
                                .ToList();

            return filteredList;
        }

        public async Task<string> Refund(Guid id)
        {
            string message = string.Empty;
            // Set the UTC offset for UTC+7
            TimeSpan utcOffset = TimeSpan.FromHours(7);

            // Get the current UTC time
            DateTime utcNow = DateTime.UtcNow;

            // Convert the UTC time to UTC+7
            DateTime localTime = utcNow + utcOffset;
            // Fetch the existing reservation
            var reservation = await _unitOfWork.Repository<Reservation>().FindAsync(x => x.ReservationId == id);

            // Check if the reservation exists
            if (reservation == null)
            {
                throw new KeyNotFoundException($"Reservation with ID {id} does not exist.");
            }

            try
            {

                if (reservation.ReservationStatus != "CheckIn" || reservation.ReservationStatus != "CheckOut" || reservation.ReservationStatus != "Cancelled")
                {
                    if (reservation.PaymentStatus == "Paid")
                    {
                        var _serviceService = _serviceProvider.GetService<IServiceService>();
                        var _orderService = _serviceProvider.GetService<IOrderService>();
                        var _orderdetailService = _serviceProvider.GetService<IOrderDetailService>();
                        var serviceList = await _serviceService.GetAll();
                        var service = serviceList.Find(s => s.ServiceName == "Hoàn tiền");
                        var order = new OrderCreateRequest
                        {
                            ReservationId = reservation.ReservationId,
                            OrderedDate = localTime,
                            OrderStatus = "Pending",
                            TotalAmount = reservation.TotalAmount
                        };
                        var orderResponse = await _orderService.Create(order);
                        var orderDetail = new OrderDetailRequest
                        {
                            OrderId = orderResponse.OrderId,
                            Price = reservation.TotalAmount,
                            Quantity = 1,
                            ServiceId = service.ServiceId

                        };
                        var orderDetailResponse = await _orderdetailService.Create(orderDetail);
                        if (orderDetailResponse != null)
                        {
                            message = "Bạn đã gửi yêu cầu thành công! Vui lòng chờ ...";
                        }
                        else
                        {
                            message = "Có lỗi xảy ra trong quá trình gửi yêu cầu ...";
                        }
                    }
                    else
                    {
                        throw new Exception("Not found");
                    }
                }
                else
                {
                    throw new Exception("Not found");
                }


                return message;
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task CheckAndProcessReservationsAsync()
        {
            IEnumerable<ReservationResponse> reservations = null;
            IEnumerable<ReservationResponse> pendingReservations = null;

            try
            {
                // Lấy các reservation đủ điều kiện để xử lý
                reservations = await GetEligibleReservationsAsync();
                // Lấy các reservation ở trạng thái "Pending" để gửi thông báo
                pendingReservations = await GetNotifyReservationsAsync();
            }
            catch (Exception ex)
            {
                // Xử lý lỗi khi truy vấn
                Console.WriteLine($"An error occurred while querying bills: {ex.Message}");
                return; // Thoát nếu có lỗi xảy ra
            }

            List<ReservationResponse> reservationsToProcess = new List<ReservationResponse>();

            foreach (var reservation in reservations)
            {
                if (reservation.ReservationStatus == "CheckOut"
                    && Is60SecondsAfterReservation(reservation))
                {
                    reservationsToProcess.Add(reservation);
                }
            }

            // Chỉ một luồng duy nhất thực thi phần này tại một thời điểm
            lock (_lockObject)
            {
                try
                {
                    foreach (var reservation in reservationsToProcess)
                    {
                        // Xử lý mỗi reservation đồng bộ
                        ProcessReservationAsync(reservation).Wait();
                    }
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi khi xử lý reservation
                    Console.WriteLine($"An error occurred while processing bills: {ex.Message}");
                }
            }

            // Gửi thông báo sắp hết hạn cho các reservation ở trạng thái "Pending"
            foreach (var pendingReservation in pendingReservations)
            {
                try
                {
                    await NotifyExpiration(pendingReservation);
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi khi gửi thông báo hết hạn
                    Console.WriteLine($"An error occurred while notifying expiration: {ex.Message}");
                }
            }
        }


        //bill da paid + amount reservation (if pre-paid)
        public async Task<ReservationResponse[]> GetEligibleReservationsAsync()
        {
            //chi lay reservation da CHECKOUT
            var eligibleReservations = await _unitOfWork.Repository<Reservation>().GetAll()
                .Where(reservation => reservation.ReservationStatus == "CheckOut")
                .ProjectTo<ReservationResponse>(_mapper.ConfigurationProvider)
                .ToArrayAsync();

            return eligibleReservations;
        }

        //check cuoi tuan
        public bool Is60SecondsAfterReservation(ReservationResponse reservation)
        {
            // Set the UTC offset for UTC+7
            TimeSpan utcOffset = TimeSpan.FromHours(7);

            // Get the current UTC time
            DateTime utcNow = DateTime.UtcNow;

            // Convert the UTC time to UTC+7
            DateTime localTime = utcNow + utcOffset;

            if (reservation.CreatedDate.HasValue)
            {
                return localTime >= reservation.CreatedDate.Value.AddSeconds(60);
            }
            else
            {
                // Handle null case (e.g., return false or throw an exception)
                return false;
            }
        }

        //Tranfer money
        public async Task ProcessReservationAsync(ReservationResponse reservation)
        {
            // Set the UTC offset for UTC+7
            TimeSpan utcOffset = TimeSpan.FromHours(7);

            // Get the current UTC time
            DateTime utcNow = DateTime.UtcNow;

            // Convert the UTC time to UTC+7
            DateTime localTime = utcNow + utcOffset;
            // Retrieve services using dependency injection
            var billService = _serviceProvider.GetService<IBillService>();
            var orderService = _serviceProvider.GetService<IOrderService>();
            var transactionService = _serviceProvider.GetService<ITransactionService>();

            // Retrieve all wallets and identify the admin and hotel owner wallets
            var wallets = await _walletService.GetAll();
            var adminWallet = wallets.Find(x => x.User.Role.RoleName == "Admin");
            var hotelOwnerWallet = wallets.Find(x => x.UserId == reservation.RoomType.Hotel.Owner.UserId);

            // Verify that the required wallets exist
            if (adminWallet == null || hotelOwnerWallet == null)
            {
                throw new Exception("Không tìm thấy tài khoản FHotel hoặc chủ khách sạn");
            }

            decimal amount = 0;

            // Calculate the amount based on the payment status of the reservation
            if (reservation.PaymentStatus == "Paid")
            {
                // If the reservation is paid, calculate the amount from the reservation's total and orders
                amount = (decimal)reservation.TotalAmount; // Assuming TotalAmount is part of ReservationResponse

                // Retrieve all orders linked to this reservation to add any applicable amounts
                var orders = await orderService.GetAllByReservationId(reservation.ReservationId);
                foreach (var order in orders)
                {
                    amount += order.TotalAmount.Value; // Assuming each order has a TotalAmount property
                }

                // Retrieve and process the bill if needed
                var bill = await billService.GetBillByReservation(reservation.ReservationId);
                if (bill != null)
                {
                    var existingTransactionAdmin = await transactionService.GetTransactionByWalletAndBillId(adminWallet.WalletId, bill.BillId);
                    if (existingTransactionAdmin == null)
                    {
                        var transactionAdmin = new TransactionRequest
                        {
                            BillId = bill.BillId,
                            Amount = bill.TotalAmount * 0.2m,
                            Description = $@"Nhận {bill.TotalAmount * 0.2m} từ đặt phòng {reservation.Code} lúc {localTime}",
                            TransactionDate = localTime,
                            WalletId = adminWallet.WalletId
                        };
                        await transactionService.Create(transactionAdmin);
                        var updateAdminWallet = new WalletRequest
                        {
                            Balance = adminWallet.Balance += (amount * 0.2m),
                            UserId = adminWallet.UserId,
                            BankAccountNumber = adminWallet.BankAccountNumber,
                            BankName = adminWallet.BankName
                        };
                        await _walletService.Update(adminWallet.WalletId, updateAdminWallet);
                    }
                    else
                    {
                        Console.WriteLine("Transaction already exists. Creation stopped.");
                    }
                    var existingTransactionHotelOwner = await transactionService.GetTransactionByWalletAndBillId(hotelOwnerWallet.WalletId, bill.BillId);
                    if (existingTransactionHotelOwner == null)
                    {
                        var transactionHotelOwner = new TransactionRequest
                        {
                            BillId = bill.BillId,
                            Amount = bill.TotalAmount * 0.8m,
                            Description = $@"Nhận {bill.TotalAmount * 0.8m} từ đặt phòng {reservation.Code} lúc {localTime}",
                            TransactionDate = localTime,
                            WalletId = hotelOwnerWallet.WalletId
                        };
                        await transactionService.Create(transactionHotelOwner);

                        var updateHotelOwnerWallet = new WalletRequest
                        {
                            Balance = hotelOwnerWallet.Balance += (amount * 0.8m),
                            UserId = hotelOwnerWallet.UserId,
                            BankAccountNumber = hotelOwnerWallet.BankAccountNumber,
                            BankName = hotelOwnerWallet.BankName
                        };
                        await _walletService.Update(hotelOwnerWallet.WalletId, updateHotelOwnerWallet);
                    }
                }
                else
                {
                    //if bill is null
                    var createBill = new BillRequest
                    {
                        ReservationId = reservation.ReservationId,
                        BillStatus = "Paid",
                        CreatedDate = localTime,
                        TotalAmount = amount
                    };
                    var createBillResponse = await billService.Create(createBill);
                    var updateBill = new BillRequest
                    {
                        ReservationId = reservation.ReservationId,
                        BillStatus = "Paid",
                        CreatedDate = localTime,
                        TotalAmount = amount
                    };
                    var updateBillResponse = await billService.Update(createBillResponse.BillId, updateBill);
                    //transfer
                    var existingTransactionAdmin = await transactionService.GetTransactionByWalletAndBillId(adminWallet.WalletId, createBillResponse.BillId);
                    if (existingTransactionAdmin == null)
                    {
                        var transactionAdmin = new TransactionRequest
                        {
                            BillId = createBillResponse.BillId,
                            Amount = updateBillResponse.TotalAmount * 0.2m,
                            Description = $@"Nhận {updateBillResponse.TotalAmount * 0.2m} từ đặt phòng {reservation.Code} lúc {localTime}",
                            TransactionDate = localTime,
                            WalletId = adminWallet.WalletId
                        };
                        await transactionService.Create(transactionAdmin);
                        var updateAdminWallet = new WalletRequest
                        {
                            Balance = adminWallet.Balance += (amount * 0.2m),
                            UserId = adminWallet.UserId,
                            BankAccountNumber = adminWallet.BankAccountNumber,
                            BankName = adminWallet.BankName
                        };
                        await _walletService.Update(adminWallet.WalletId, updateAdminWallet);
                    }
                    var existingTransactionHotelOwner = await transactionService.GetTransactionByWalletAndBillId(hotelOwnerWallet.WalletId, createBillResponse.BillId);
                    if (existingTransactionHotelOwner == null)
                    {
                        var transactionHotelOwner = new TransactionRequest
                        {
                            BillId = createBillResponse.BillId,
                            Amount = updateBillResponse.TotalAmount * 0.8m,
                            Description = $@"Nhận {updateBillResponse.TotalAmount * 0.8m} từ đặt phòng {reservation.Code} lúc {localTime}",
                            TransactionDate = localTime,
                            WalletId = hotelOwnerWallet.WalletId
                        };
                        await transactionService.Create(transactionHotelOwner);

                        var updateHotelOwnerWallet = new WalletRequest
                        {
                            Balance = hotelOwnerWallet.Balance += (amount * 0.8m),
                            UserId = hotelOwnerWallet.UserId,
                            BankAccountNumber = hotelOwnerWallet.BankAccountNumber,
                            BankName = hotelOwnerWallet.BankName
                        };
                        await _walletService.Update(hotelOwnerWallet.WalletId, updateHotelOwnerWallet);
                    }
                    else
                    {
                        Console.WriteLine("Transaction already exists. Creation stopped.");
                    }
                }
            }
            else
            {
                // If not paid, the amount could include reservation total + bill total
                amount = reservation.TotalAmount.Value;

                var orders = await orderService.GetAllByReservationId(reservation.ReservationId);
                foreach (var order in orders)
                {
                    amount += order.TotalAmount.Value; // Assuming each order has a TotalAmount property
                }
                // Retrieve and process the bill if needed
                var bill = await billService.GetBillByReservation(reservation.ReservationId);
                if (bill != null)
                {
                    var existingTransactionAdmin = await transactionService.GetTransactionByWalletAndBillId(adminWallet.WalletId, bill.BillId);
                    if (existingTransactionAdmin == null)
                    {
                        var transactionAdmin = new TransactionRequest
                        {
                            BillId = bill.BillId,
                            Amount = bill.TotalAmount * 0.2m,
                            Description = $@"Nhận {bill.TotalAmount * 0.2m} từ đặt phòng {reservation.Code} lúc {localTime}",
                            TransactionDate = localTime,
                            WalletId = adminWallet.WalletId
                        };
                        await transactionService.Create(transactionAdmin);
                        var updateAdminWallet = new WalletRequest
                        {
                            Balance = adminWallet.Balance += (amount * 0.2m),
                            UserId = adminWallet.UserId,
                            BankAccountNumber = adminWallet.BankAccountNumber,
                            BankName = adminWallet.BankName
                        };
                        await _walletService.Update(adminWallet.WalletId, updateAdminWallet);
                    }
                    else
                    {
                        Console.WriteLine("Transaction already exists. Creation stopped.");
                    }
                    var existingTransactionHotelOwner = await transactionService.GetTransactionByWalletAndBillId(hotelOwnerWallet.WalletId, bill.BillId);
                    if (existingTransactionHotelOwner == null)
                    {
                        var transactionHotelOwner = new TransactionRequest
                        {
                            BillId = bill.BillId,
                            Amount = bill.TotalAmount * 0.8m,
                            Description = $@"Nhận {bill.TotalAmount * 0.8m} từ đặt phòng {reservation.Code} lúc {localTime}",
                            TransactionDate = localTime,
                            WalletId = hotelOwnerWallet.WalletId
                        };
                        await transactionService.Create(transactionHotelOwner);

                        var updateHotelOwnerWallet = new WalletRequest
                        {
                            Balance = hotelOwnerWallet.Balance += (amount * 0.8m),
                            UserId = hotelOwnerWallet.UserId,
                            BankAccountNumber = hotelOwnerWallet.BankAccountNumber,
                            BankName = hotelOwnerWallet.BankName
                        };
                        await _walletService.Update(hotelOwnerWallet.WalletId, updateHotelOwnerWallet);
                    }
                }
                else
                {
                    //if bill is null
                    var createBill = new BillRequest
                    {
                        ReservationId = reservation.ReservationId,
                        BillStatus = "Paid",
                        CreatedDate = localTime,
                        TotalAmount = amount
                    };
                    var createBillResponse = await billService.Create(createBill);
                    var updateBill = new BillRequest
                    {
                        ReservationId = reservation.ReservationId,
                        BillStatus = "Paid",
                        CreatedDate = localTime,
                        TotalAmount = amount
                    };
                    await billService.Update(createBillResponse.BillId, updateBill);
                    var nowBill = await billService.Get(createBillResponse.BillId);
                    //transfer
                    var existingTransactionAdmin = await transactionService.GetTransactionByWalletAndBillId(adminWallet.WalletId, createBillResponse.BillId);
                    if (existingTransactionAdmin == null)
                    {
                        var transactionAdmin = new TransactionRequest
                        {
                            BillId = createBillResponse.BillId,
                            Amount = nowBill.TotalAmount * 0.2m,
                            Description = $@"Nhận {nowBill.TotalAmount * 0.2m} từ đặt phòng {reservation.Code} lúc {localTime}",
                            TransactionDate = localTime,
                            WalletId = adminWallet.WalletId
                        };
                        await transactionService.Create(transactionAdmin);
                        var updateAdminWallet = new WalletRequest
                        {
                            Balance = adminWallet.Balance += (amount * 0.2m),
                            UserId = adminWallet.UserId,
                            BankAccountNumber = adminWallet.BankAccountNumber,
                            BankName = adminWallet.BankName
                        };
                        await _walletService.Update(adminWallet.WalletId, updateAdminWallet);
                    }
                    var existingTransactionHotelOwner = await transactionService.GetTransactionByWalletAndBillId(hotelOwnerWallet.WalletId, createBillResponse.BillId);
                    if (existingTransactionHotelOwner == null)
                    {
                        var transactionHotelOwner = new TransactionRequest
                        {
                            BillId = createBillResponse.BillId,
                            Amount = nowBill.TotalAmount * 0.8m,
                            Description = $@"Nhận {nowBill.TotalAmount * 0.8m} từ đặt phòng {reservation.Code} lúc {localTime}",
                            TransactionDate = localTime,
                            WalletId = hotelOwnerWallet.WalletId
                        };
                        await transactionService.Create(transactionHotelOwner);

                        var updateHotelOwnerWallet = new WalletRequest
                        {
                            Balance = hotelOwnerWallet.Balance += (amount * 0.8m),
                            UserId = hotelOwnerWallet.UserId,
                            BankAccountNumber = hotelOwnerWallet.BankAccountNumber,
                            BankName = hotelOwnerWallet.BankName
                        };
                        await _walletService.Update(hotelOwnerWallet.WalletId, updateHotelOwnerWallet);
                    }
                    else
                    {
                        Console.WriteLine("Transaction already exists. Creation stopped.");
                    }
                }
            }

            // Continue with further processing, like transferring funds between wallets
            Console.WriteLine($"Calculated amount for the reservation: {amount}");

        }

        //NOTIFY
        public async Task<ReservationResponse[]> GetNotifyReservationsAsync()
        {
            //chi lay reservation dang pending
            var eligibleReservations = await _unitOfWork.Repository<Reservation>().GetAll()
                .Where(reservation => reservation.ReservationStatus == "Pending")
                .ProjectTo<ReservationResponse>(_mapper.ConfigurationProvider)
                .ToArrayAsync();

            return eligibleReservations;
        }

        public async Task NotifyExpiration(ReservationResponse reservation)
        {
            // Đặt ngưỡng thông báo trước 2 ngày
            var expirationThreshold = reservation.CheckInDate?.AddDays(-2);

            if (expirationThreshold.HasValue && DateTime.UtcNow >= expirationThreshold.Value)
            {
                await SendEmail(reservation); // Gửi email thông báo
            }
        }

        public async Task SendEmail(ReservationResponse reservation)
        {
            // Retrieve email settings from appsettings.json
            var emailSettings = GetEmailSettings();

            var fromAddress = new MailAddress(emailSettings.Sender, emailSettings.SystemName);
            var toAddress = new MailAddress(reservation.Customer.Email);
            const string subject = "Sắp tới ngày nhận phòng!"; // Email subject

            // Construct the email body with HTML template
            string body = $@"
        <h1>Thông tin Đặt Phòng</h1>
        <p>Kính gửi {reservation.Customer.Name},</p>
        <p>Cảm ơn đã dành thời gian với chúng tôi.</p>
        <p>Còn 2 ngày nữa là tới ngày nhận phòng của mã đặt phòng này: {reservation.Code}</p>
        <p>Ngày nhận phòng: {reservation.CheckInDate}</p>
        <p>Ngày trả phòng: {reservation.CheckOutDate}</p>     
        <p>Số lượng phòng đặt: {reservation.NumberOfRooms}</p>     
        <p>Trân trọng nhất,<br>FHotel company.</p>";

            // Set up the SMTP client
            var smtp = new SmtpClient
            {
                Host = emailSettings.Host,
                Port = emailSettings.Port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, emailSettings.Password)
            };

            // Configure and send the email
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true // Specify that the email body is HTML
            })
            {
                await smtp.SendMailAsync(message);
            }
        }
        private Email GetEmailSettings()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                                          .SetBasePath(Directory.GetCurrentDirectory())
                                          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();

            return new Email()
            {
                SystemName = configuration.GetSection("Email:SystemName").Value,
                Sender = configuration.GetSection("Email:Sender").Value,
                Password = configuration.GetSection("Email:Password").Value,
                Port = int.Parse(configuration.GetSection("Email:Port").Value),
                Host = configuration.GetSection("Email:Host").Value
            };
        }
    }
}
