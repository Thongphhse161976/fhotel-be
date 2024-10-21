using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Services.DTOs.Countries;
using FHotel.Services.DTOs.Orders;
using FHotel.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
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
        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
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

        public async Task<OrderResponse> Create(OrderRequest request)
        {
            try
            {
                var order = _mapper.Map<OrderRequest, Order>(request);
                order.OrderId = Guid.NewGuid();
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
                await _unitOfWork.Repository<Role>().HardDeleteGuid(order.OrderId);
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
                    throw new Exception();
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
    }
}
