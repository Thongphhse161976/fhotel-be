using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
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

        public OrderDetailService(IUnitOfWork unitOfWork, IMapper mapper, IServiceProvider serviceProvider)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _serviceProvider = serviceProvider;
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

        //public async Task<OrderDetailResponse> Create(OrderDetailRequest request)
        //{
        //    try
        //    {
        //        var orderDetail = _mapper.Map<OrderDetailRequest, OrderDetail>(request);
        //        orderDetail.OrderDetailId = Guid.NewGuid();
        //        var order = await _orderService.Get(request.OrderId.Value);
        //        await _unitOfWork.Repository<OrderDetail>().InsertAsync(orderDetail);
        //        await _unitOfWork.CommitAsync();
        //        var orderDetailResponse = await Get(orderDetail.OrderDetailId);
        //        var updateOrder = new OrderRequest
        //        {
        //            OrderId = order.OrderId,
        //            ReservationId = order.OrderId,
        //            OrderedDate = order.OrderedDate,
        //            OrderStatus = order.OrderStatus,
        //            TotalAmount = orderDetailResponse.Service.Price * request.Quantity
        //        };
        //        await _orderService.Update(orderDetailResponse.OrderId.Value, updateOrder);
        //        return _mapper.Map<OrderDetail, OrderDetailResponse>(orderDetail);
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception(e.Message);
        //    }
        //}

        public async Task<OrderDetailResponse> Create(OrderDetailRequest request)
        {
            try
            {
                var orderDetail = _mapper.Map<OrderDetailRequest, OrderDetail>(request);
                orderDetail.OrderDetailId = Guid.NewGuid();

                // Resolve IOrderService dynamically at runtime
                var orderService = _serviceProvider.GetService<IOrderService>();

                var order = await orderService.Get(request.OrderId.Value);
                await _unitOfWork.Repository<OrderDetail>().InsertAsync(orderDetail);
                await _unitOfWork.CommitAsync();

                var orderDetailResponse = await Get(orderDetail.OrderDetailId);

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

                await orderService.Update(orderDetailResponse.OrderId.Value, updateOrder);
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

    }
}
