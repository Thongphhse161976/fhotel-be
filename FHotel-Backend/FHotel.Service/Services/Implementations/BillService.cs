using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.Bills;
using FHotel.Service.DTOs.Transactions;
using FHotel.Service.DTOs.Wallets;
using FHotel.Service.Services.Interfaces;
using FHotel.Services.DTOs.Orders;
using FHotel.Services.DTOs.Reservations;
using FHotel.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Implementations
{
    public class BillService : IBillService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private IOrderService _orderService;
        private IReservationService _reservationService;
        private IWalletService _walletService;
        private readonly object _lockObject = new object();
        public BillService(IUnitOfWork unitOfWork, IMapper mapper, IOrderService orderService, IReservationService reservationService, IWalletService walletService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _orderService = orderService;
            _reservationService = reservationService;
            _walletService = walletService;
        }

        public async Task<List<BillResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<Bill>().GetAll()
                                            .ProjectTo<BillResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<BillResponse> Get(Guid id)
        {
            try
            {
                Bill bill = null;
                bill = await _unitOfWork.Repository<Bill>().GetAll()
                     .AsNoTracking()
                     .Include(x => x.Reservation)
                    .Where(x => x.BillId == id)
                    .FirstOrDefaultAsync();

                if (bill == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<Bill, BillResponse>(bill);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<BillResponse> Create(BillRequest request)
        {
            // Set the UTC offset for UTC+7
            TimeSpan utcOffset = TimeSpan.FromHours(7);

            // Get the current UTC time
            DateTime utcNow = DateTime.UtcNow;

            // Convert the UTC time to UTC+7
            DateTime localTime = utcNow + utcOffset;
            try
            {
                var bill = _mapper.Map<BillRequest, Bill>(request);
                bill.BillId = Guid.NewGuid();
                bill.CreatedDate = localTime;
                bill.BillStatus = "Pending";
                await _unitOfWork.Repository<Bill>().InsertAsync(bill);
                await _unitOfWork.CommitAsync();

                

                return _mapper.Map<Bill, BillResponse>(bill);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<BillResponse> Delete(Guid id)
        {
            try
            {
                Bill bill = null;
                bill = _unitOfWork.Repository<Bill>()
                    .Find(p => p.BillId == id);
                if (bill == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<Bill>().HardDeleteGuid(bill.BillId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<Bill, BillResponse>(bill);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BillResponse> Update(Guid id, BillRequest request)
        {
            try
            {
                Bill bill = _unitOfWork.Repository<Bill>()
                            .Find(x => x.BillId == id);
                if (bill == null)
                {
                    throw new Exception();
                }
                bill = _mapper.Map(request, bill);


                await _unitOfWork.Repository<Bill>().UpdateDetached(bill);
                await _unitOfWork.CommitAsync();

                if(bill.BillStatus == "Paid")
                {
                    var orders = await _orderService.GetAllByReservationId(bill.ReservationId.Value);
                    if (orders.Count > 0)
                    {
                        foreach (var order in orders)
                        {
                            var updateOrder = new OrderRequest()
                            {
                                OrderId = order.OrderId,
                                ReservationId = order.ReservationId,
                                BillId = bill.BillId,
                                OrderedDate = order.OrderedDate,
                                OrderStatus = order.OrderStatus,
                                TotalAmount = order.TotalAmount,
                            };
                            await _orderService.Update(order.OrderId, updateOrder);
                        }
                        
                    }
                }

                return _mapper.Map<Bill, BillResponse>(bill);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BillResponse> GetBillByReservation(Guid id)
        {
            var bill = await _unitOfWork.Repository<Bill>().GetAll()
                                           .ProjectTo<BillResponse>(_mapper.ConfigurationProvider)
                                           .FirstOrDefaultAsync(d => d.ReservationId == id);

            return bill;
        }
        public async Task<List<BillResponse>> GetAllBillByStaffId(Guid staffId)
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
            var bills = await _unitOfWork.Repository<Bill>()
                                                .GetAll()
                                                .Include(x => x.Reservation)
                                                .Where(r => r.Reservation.RoomType.HotelId == hotelId)
                                                .ProjectTo<BillResponse>(_mapper.ConfigurationProvider)
                                                .ToListAsync();

            // Check if any reservations were found
            if (bills == null || !bills.Any())
            {
                throw new Exception("No orders found for this staff's hotel.");
            }

            return bills;
        }
        public async Task<List<BillResponse>> GetAllByOwnerId(Guid id)
        {

            var list = await _unitOfWork.Repository<Bill>().GetAll()
                                            .Where(r => r.Reservation.RoomType.Hotel.OwnerId == id)
                                            .ProjectTo<BillResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            // Check if list is null or empty
            if (list == null || !list.Any())
            {
                throw new Exception("No bills found.");
            }
            return list;
        }

        //public async Task CheckAndProcessBillsAsync()
        //{
        //    IEnumerable<BillResponse> bills = null;
        //    try
        //    {
        //        bills = await GetEligibleBillsAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exceptions here
        //        Console.WriteLine($"An error occurred while querying bills: {ex.Message}");
        //        return; // Exit the method if an error occurs
        //    }

        //    List<BillResponse> billsToProcess = new List<BillResponse>();

        //    foreach (var bill in bills)
        //    {
               
        //            if (bill.BillStatus == "Paid" && Is60SecondsAfterBill(bill))
        //            {
        //                billsToProcess.Add(bill);
        //            }
               
        //    }

        //    // Only one thread should execute this part at a time
        //    lock (_lockObject)
        //    {
        //        try
        //        {
        //            foreach (var bill in billsToProcess)
        //            {
        //                // Process each enrollment synchronously
        //                ProcessBillAsync(bill).Wait();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            // Handle exceptions here
        //            Console.WriteLine($"An error occurred while processing bills: {ex.Message}");
        //        }
        //    }
        //}

        ////bill da paid + amount reservation (if pre-paid)
        //public async Task<BillResponse[]> GetEligibleBillsAsync()
        //{

        //    // Query enrollments where RefundStatus is false and 60 seconds have passed since enrollment
        //    var eligibleBills = await _unitOfWork.Repository<Bill>().GetAll()
        //        .Where(bill => bill.BillStatus == "Paid")
        //        .ProjectTo<BillResponse>(_mapper.ConfigurationProvider)
        //        .ToArrayAsync();

        //    return eligibleBills;
        //}

        ////check cuoi tuan
        //public bool Is60SecondsAfterBill(BillResponse bill)
        //{
        //    // Set the UTC offset for UTC+7
        //    TimeSpan utcOffset = TimeSpan.FromHours(7);

        //    // Get the current UTC time
        //    DateTime utcNow = DateTime.UtcNow;

        //    // Convert the UTC time to UTC+7
        //    DateTime localTime = utcNow + utcOffset;

        //    if (bill.CreatedDate.HasValue)
        //    {
        //        return localTime >= bill.CreatedDate.Value.AddSeconds(60);
        //    }
        //    else
        //    {
        //        // Handle null case (e.g., return false or throw an exception)
        //        return false;
        //    }
        //}

        ////Tranfer money
        //public async Task ProcessBillAsync(BillResponse bill)
        //{
        //    var wallets = await _walletService.GetAll();
        //    var adminWallet = wallets.Find(x => x.User.Role.RoleName == "Admin");
        //    var reservation = await _reservationService.Get(bill.ReservationId.Value);
        //    var hotelOwnerWallet = wallets.Find(x => x.UserId == reservation.RoomType.Hotel.Owner.UserId);
        //    if (adminWallet == null || hotelOwnerWallet == null)
        //    {
        //        throw new Exception("Không tìm thấy tài khoản FHotel hoặc chủ khách sạn");
        //    }
        //    else
        //    {
        //        var updateAdminWallet = new WalletRequest
        //        {
        //            Balance = 1000,
        //            UserId = adminWallet.UserId,
        //            BankAccountNumber = adminWallet.BankAccountNumber,
        //            BankName = adminWallet.BankName
        //        };
        //        await _walletService.Update(adminWallet.WalletId, updateAdminWallet);
        //        var updateHotelOwnerWallet = new WalletRequest
        //        {
        //            Balance = 3000,
        //            UserId = hotelOwnerWallet.UserId,
        //            BankAccountNumber = hotelOwnerWallet.BankAccountNumber,
        //            BankName = hotelOwnerWallet.BankName
        //        };
        //        await _walletService.Update(hotelOwnerWallet.WalletId, updateHotelOwnerWallet);
        //    }
        //}

    }
}
