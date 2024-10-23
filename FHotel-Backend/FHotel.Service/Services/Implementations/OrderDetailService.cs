using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Services.DTOs.Countries;
using FHotel.Services.DTOs.OrderDetails;
using FHotel.Services.DTOs.UserDocuments;
using FHotel.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
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
        public OrderDetailService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<OrderDetailResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<OrderDetail>().GetAll()
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
                await _unitOfWork.Repository<OrderDetail>().InsertAsync(orderDetail);
                await _unitOfWork.CommitAsync();

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
                    .Where(x => x.OrderId == orderId)
                    .ToListAsync();

            return _mapper.Map<IEnumerable<OrderDetail>, IEnumerable<OrderDetailResponse>>(orderDetails);
        }

    }
}
