﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.Bills;
using FHotel.Service.DTOs.Orders;
using FHotel.Service.DTOs.Reservations;
using FHotel.Service.DTOs.Transactions;
using FHotel.Service.DTOs.Wallets;
using FHotel.Service.Services.Implementations;
using FHotel.Service.Services.Interfaces;
using FHotel.Services.DTOs.OrderDetails;
using FHotel.Services.DTOs.Orders;
using FHotel.Services.DTOs.Reservations;
using FHotel.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private IOrderDetailService _orderDetailService;
        private readonly IServiceProvider _serviceProvider;
        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IOrderDetailService orderDetailService, IServiceProvider serviceProvider)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _orderDetailService = orderDetailService;
            _serviceProvider = serviceProvider;
        }

        public async Task<List<OrderResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<Order>().GetAll()
                                            .ProjectTo<OrderResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            if (list == null)
            {
                throw new Exception("Order not found");
            }
            return list;
        }

        public async Task<OrderResponse> Get(Guid id)
        {
            try
            {
                Order order = null;
                order = await _unitOfWork.Repository<Order>().GetAll()
                     .AsNoTracking()
                     .Include(o=> o.Reservation)
                    .Where(x => x.OrderId == id)
                    .FirstOrDefaultAsync();

                if (order == null)
                {
                    throw new Exception("Order not found");
                }

                return _mapper.Map<Order, OrderResponse>(order);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<OrderResponse> Create(OrderCreateRequest request)
        {
            // Set the UTC offset for UTC+7
            TimeSpan utcOffset = TimeSpan.FromHours(7);

            // Get the current UTC time
            DateTime utcNow = DateTime.UtcNow;

            // Convert the UTC time to UTC+7
            DateTime localTime = utcNow + utcOffset;
            try
            {
                var order = _mapper.Map<OrderCreateRequest, Order>(request);
                order.OrderId = Guid.NewGuid();
                order.OrderedDate = localTime;
                //order.OrderStatus = "Pending";
                await _unitOfWork.Repository<Order>().InsertAsync(order);
                await _unitOfWork.CommitAsync();
                
                return _mapper.Map<Order, OrderResponse>(order);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<OrderResponse> Delete(Guid id)
        {
            try
            {
                Order order = null;
                order = _unitOfWork.Repository<Order>()
                    .Find(p => p.OrderId == id);
                if (order == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<Order>().HardDeleteGuid(order.OrderId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<Order, OrderResponse>(order);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<OrderResponse> Update(Guid id, OrderRequest request)
        {
            try
            {
                Order order = _unitOfWork.Repository<Order>()
                            .Find(x => x.OrderId == id);
                if (order == null)
                {
                    throw new Exception("Not Found");
                }
                order = _mapper.Map(request, order);

                await _unitOfWork.Repository<Order>().UpdateDetached(order);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Order, OrderResponse>(order);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<OrderResponse>> GetAllByReservationId(Guid id)
        {

            var list = await _unitOfWork.Repository<Order>().GetAll()
                                            .Where(o=> o.ReservationId == id)
                                            .ProjectTo<OrderResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            if (list == null)
            {
                throw new Exception("Order not found");
            }
            return list;
        }

        public async Task<List<OrderResponse>> GetAllOrderByStaffId(Guid staffId)
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
            var orders = await _unitOfWork.Repository<Order>()
                                                .GetAll()
                                                .Include(x => x.Reservation)
                                                .Where(r => r.Reservation.RoomType.HotelId == hotelId) 
                                                .ProjectTo<OrderResponse>(_mapper.ConfigurationProvider)
                                                .ToListAsync();

            // Check if any reservations were found
            if (orders == null || !orders.Any())
            {
                throw new Exception("No orders found for this staff's hotel.");
            }

            return orders;
        }

        public async Task<List<OrderResponse>> GetAllOrderByCustomerId(Guid id)
        {

            var list = await _unitOfWork.Repository<Order>().GetAll()
                                            .Where(o => o.Reservation.CustomerId == id)
                                            .ProjectTo<OrderResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            if (list == null)
            {
                throw new Exception("Order not found");
            }
            return list;
        }

        public async Task<List<OrderResponse>> GetAllOrderByBillId(Guid id)
        {

            var list = await _unitOfWork.Repository<Order>().GetAll()
                                            .ProjectTo<OrderResponse>(_mapper.ConfigurationProvider)
                                            .Where(d => d.BillId == id)
                                            .ToListAsync();
            return list;
        }

        public async Task<OrderResponse> AcceptRefund(Guid id)
        {
            var billService = _serviceProvider.GetService<IBillService>();
            var reservationService = _serviceProvider.GetService<IReservationService>();
            var transactionService = _serviceProvider.GetService<ITransactionService>();
            var walletService = _serviceProvider.GetService<IWalletService>();
            // Set the UTC offset for UTC+7
            TimeSpan utcOffset = TimeSpan.FromHours(7);

            // Get the current UTC time
            DateTime utcNow = DateTime.UtcNow;

            // Convert the UTC time to UTC+7
            DateTime localTime = utcNow + utcOffset;
            try
            {
                
                var order = await Get(id);
                var createBill = new BillRequest
                {
                    ReservationId = order.ReservationId,
                    TotalAmount = order.TotalAmount,
                };
                var createBillResponse = await billService.Create(createBill);
                var updateOrder = new OrderRequest
                {
                    BillId = createBillResponse.BillId,
                    OrderId = order.OrderId,
                    ReservationId = order.ReservationId,
                    TotalAmount = order.TotalAmount,
                    OrderedDate = order.OrderedDate,
                    OrderStatus = "Paid"
                };
               await Update(order.OrderId, updateOrder);
                var bill = await billService.Get(createBillResponse.BillId);
                var updateBill = new BillRequest
                {
                    BillStatus = "Paid",
                    CreatedDate = bill.CreatedDate,
                    LastUpdated = localTime,
                    ReservationId = bill.ReservationId,
                    TotalAmount = bill.TotalAmount,
                };
                await billService.Update(bill.BillId, updateBill);
                //transfer 
                var wallets = await walletService.GetAll();
                var customerWallet = wallets.Find(x => x.UserId == order.Reservation.CustomerId);
                // Verify that the required wallets exist
                if (customerWallet == null)
                {
                    throw new Exception("Không tìm thấy tài khoản của khách hàng");
                }
                var amount = order.TotalAmount;
                var createTransaction = new TransactionRequest
                {
                    BillId = bill.BillId,
                    Amount = amount,
                    Description = $@"Nhận {amount} từ hoàn tiền của đặt phòng {order.Reservation.Code} lúc {localTime}",
                    TransactionDate = localTime,
                    WalletId = customerWallet.WalletId
                };
                await transactionService.Create(createTransaction);
                var updateWallet = new WalletRequest
                {
                    Balance = customerWallet.Balance += amount,
                    UserId = customerWallet.UserId,
                    BankAccountNumber = customerWallet.BankAccountNumber,
                    BankName = customerWallet.BankName
                };
                await walletService.Update(customerWallet.WalletId, updateWallet);
                //update reservation status
                var reservation = await reservationService.Get((Guid)order.ReservationId);
                var updateReservation = new ReservationUpdateRequest
                {
                    ReservationId = reservation.ReservationId,
                    CheckInDate = reservation.CheckInDate,
                    CheckOutDate = reservation.CheckOutDate,
                    Code = reservation.Code,
                    CreatedDate = reservation.CreatedDate,
                    CustomerId = reservation.CustomerId,
                    NumberOfRooms = reservation.NumberOfRooms,
                    PaymentMethodId = reservation.PaymentMethodId,
                    PaymentStatus = reservation.PaymentStatus,
                    RoomTypeId = reservation.RoomTypeId,
                    TotalAmount = reservation.TotalAmount,
                    ReservationStatus = "Cancelled"
                };
                await reservationService.Update(reservation.ReservationId, updateReservation);
                return order;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
