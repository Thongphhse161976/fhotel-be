using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.Services.Interfaces;
using FHotel.Services.DTOs.OrderDetails;
using FHotel.Services.DTOs.Orders;
using FHotel.Services.DTOs.UserDocuments;
using FHotel.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Implementations
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private readonly IServiceProvider _serviceProvider;
        private readonly IServiceService _serviceService;
        private readonly IReservationService _reservationService;
        private readonly ITypePricingService _typePricingService;

        public OrderDetailService(IUnitOfWork unitOfWork, IMapper mapper, IServiceProvider serviceProvider,
            IServiceService serviceService , IReservationService reservationService, ITypePricingService typePricingService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _serviceProvider = serviceProvider;
            _serviceService = serviceService;
            _reservationService = reservationService;
            _typePricingService = typePricingService;
        }

        public async Task<List<OrderDetailResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<OrderDetail>().GetAll()
                 .Include(x => x.Service)
                     .Include(x => x.Order)
                                            .ProjectTo<OrderDetailResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<OrderDetailResponse> Get(Guid id)
        {
            try
            {
                OrderDetail orderDetail = null;
                orderDetail = await _unitOfWork.Repository<OrderDetail>().GetAll()
                     .AsNoTracking()
                     .Include(x => x.Service)   
                        .ThenInclude(x => x.ServiceType)
                    .Where(x => x.OrderDetailId == id)
                    .FirstOrDefaultAsync();

                if (orderDetail == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<OrderDetail, OrderDetailResponse>(orderDetail);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }



        public async Task<OrderDetailResponse> Create(OrderDetailRequest request)
        {
            try
            {
                var orderDetail = _mapper.Map<OrderDetailRequest, OrderDetail>(request);
                orderDetail.OrderDetailId = Guid.NewGuid();

                // Resolve IOrderService dynamically at runtime
                var orderService = _serviceProvider.GetService<IOrderService>();

                // Get the order details
                var order = await orderService.Get(request.OrderId.Value);

                // Insert the new order detail record
                await _unitOfWork.Repository<OrderDetail>().InsertAsync(orderDetail);
                await _unitOfWork.CommitAsync();

                // Retrieve the created order detail to access the service details
                var orderDetailResponse = await Get(orderDetail.OrderDetailId);

                

                // Check if the service type is for late checkout
                if (orderDetailResponse.Service.ServiceType.ServiceTypeName == "Trả phòng muộn")
                {
                    // Initialize total amount to zero
                    decimal totalAmount = 0;
                    // Retrieve the reservation to calculate late checkout fees
                    var reservation = await _reservationService.Get(order.ReservationId.Value);

                    // Parse the serviceName as the number of late days
                    if (int.TryParse(orderDetailResponse.Service.ServiceName, out int lateDays) && lateDays > 0)
                    {
                        DateTime lateCheckoutDate = reservation.CheckOutDate.Value;

                        // Calculate the additional fees for each late day
                        for (int i = 0; i < lateDays; i++)
                        {
                            lateCheckoutDate = lateCheckoutDate.AddDays(1);

                            // Fetch the daily pricing for this room type and date
                            decimal dailyRate = await _typePricingService.GetPricingByRoomTypeAndDate(reservation.RoomTypeId.Value, lateCheckoutDate);

                            // Accumulate the fee for each late day into totalAmount
                            totalAmount += dailyRate * request.Quantity.Value;
                        }
                    }
                    // Update the order's total amount with late checkout fees included
                    var updateOrder = new OrderRequest
                    {
                        OrderId = order.OrderId,
                        ReservationId = order.ReservationId,
                        OrderedDate = order.OrderedDate,
                        OrderStatus = "Confirmed",
                        TotalAmount = totalAmount // Set the accumulated total amount
                    };

                    var updateOrderDetail = new OrderDetailRequest
                    {
                        OrderId = orderDetail.OrderId,
                        ServiceId = orderDetail.ServiceId,
                        Quantity = orderDetail.Quantity,
                        Price = totalAmount // Set the accumulated total amount
                    };

                    // Update the order with the new total amount
                    await orderService.Update(orderDetailResponse.OrderId.Value, updateOrder);
                    await Update(orderDetail.OrderDetailId, updateOrderDetail);
                }
                if (orderDetailResponse.Service.ServiceType.ServiceTypeName != "Trả phòng muộn")
                {
                    if (orderDetailResponse.Service.ServiceType.ServiceTypeName == "Hoàn tiền hủy đặt phòng")
                    {
                        return _mapper.Map<OrderDetail, OrderDetailResponse>(orderDetail);

                    }
                    // Use the calculation service for TotalAmount calculation
                    var totalAmount = orderDetailResponse.Service.Price * request.Quantity;

                    var updateOrder = new OrderRequest
                    {
                        OrderId = order.OrderId,
                        ReservationId = order.ReservationId,
                        OrderedDate = order.OrderedDate,
                        OrderStatus = order.OrderStatus,
                        TotalAmount = totalAmount
                    };

                    var updateOrderDetail = new OrderDetailRequest
                    {
                        OrderId = orderDetail.OrderId,
                        ServiceId = orderDetail.ServiceId,
                        Quantity = orderDetail.Quantity,
                        Price = totalAmount // Set the accumulated total amount
                    };

                    await orderService.Update(orderDetailResponse.OrderId.Value, updateOrder);
                    await Update(orderDetail.OrderDetailId, updateOrderDetail);

                }
                



                return _mapper.Map<OrderDetail, OrderDetailResponse>(orderDetail);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }



        public async Task<OrderDetailResponse> Delete(Guid id)
        {
            try
            {
                OrderDetail orderDetail = null;
                orderDetail = _unitOfWork.Repository<OrderDetail>()
                    .Find(p => p.OrderDetailId == id);
                if (orderDetail == null)
                {
                    throw new Exception("Bi trung id");
                }
                // Resolve IOrderService dynamically at runtime
                var orderService = _serviceProvider.GetService<IOrderService>();

                // Get the order details
                var order = await orderService.Get(orderDetail.OrderId.Value);
                if (order != null)
                {
                    await orderService.Delete(order.OrderId);
                }

                await _unitOfWork.Repository<OrderDetail>().HardDeleteGuid(orderDetail.OrderDetailId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<OrderDetail, OrderDetailResponse>(orderDetail);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<OrderDetailResponse> Update(Guid id, OrderDetailRequest request)
        {
            try
            {
                OrderDetail orderDetail = _unitOfWork.Repository<OrderDetail>()
                            .Find(x => x.OrderDetailId == id);
                if (orderDetail == null)
                {
                    throw new Exception();
                }
                orderDetail = _mapper.Map(request, orderDetail);

                await _unitOfWork.Repository<OrderDetail>().UpdateDetached(orderDetail);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<OrderDetail, OrderDetailResponse>(orderDetail);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<OrderDetailResponse>> GetAllOrderDetailByOrder(Guid orderId)
        {
            var orderDetails = await _unitOfWork.Repository<OrderDetail>().GetAll()
                     .AsNoTracking()
                      .Include(x => x.Service)
                            .ThenInclude(x => x.ServiceType)
                        .Include(x => x.Order)
                    .Where(x => x.OrderId == orderId)
                    .ToListAsync();

            return _mapper.Map<IEnumerable<OrderDetail>, IEnumerable<OrderDetailResponse>>(orderDetails);
        }

        public async Task<IEnumerable<OrderDetailResponse>> GetAllOrderDetailByReservation(Guid reservationId)
        {
            var orderDetails = await _unitOfWork.Repository<OrderDetail>().GetAll()
                     .AsNoTracking()
                      .Include(x => x.Service)
                            .ThenInclude(x => x.ServiceType)
                        .Include(x => x.Order)
                    .Where(x => x.Order.ReservationId == reservationId)
                    .ToListAsync();

            return _mapper.Map<IEnumerable<OrderDetail>, IEnumerable<OrderDetailResponse>>(orderDetails);
        }
        public async Task<IEnumerable<OrderDetailResponse>> GetAllOrderDetailByUser(Guid userId)
        {
            var orderDetails = await _unitOfWork.Repository<OrderDetail>().GetAll()
                     .AsNoTracking()
                      .Include(x => x.Service)
                            .ThenInclude(x => x.ServiceType)
                        .Include(x => x.Order)
                            .ThenInclude(x=> x.Reservation)
                    .Where(x => x.Order.Reservation.CustomerId == userId)
                    .ToListAsync();

            return _mapper.Map<IEnumerable<OrderDetail>, IEnumerable<OrderDetailResponse>>(orderDetails);
        }
        

    }
}
