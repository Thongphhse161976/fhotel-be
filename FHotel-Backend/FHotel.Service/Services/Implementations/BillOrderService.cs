using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.BillOrders;
using FHotel.Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Implementations
{
    public class BillOrderService: IBillOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public BillOrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<BillOrderResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<BillOrder>().GetAll()
                                            .ProjectTo<BillOrderResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<BillOrderResponse> Get(Guid id)
        {
            try
            {
                BillOrder billOrder = null;
                billOrder = await _unitOfWork.Repository<BillOrder>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.BillOrderId == id)
                    .FirstOrDefaultAsync();

                if (billOrder == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<BillOrder, BillOrderResponse>(billOrder);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<BillOrderResponse> Create(BillOrderRequest request)
        {
            try
            {
                var billOrder = _mapper.Map<BillOrderRequest, BillOrder>(request);
                billOrder.BillOrderId = Guid.NewGuid();
                await _unitOfWork.Repository<BillOrder>().InsertAsync(billOrder);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<BillOrder, BillOrderResponse>(billOrder);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<BillOrderResponse> Delete(Guid id)
        {
            try
            {
                BillOrder billOrder = null;
                billOrder = _unitOfWork.Repository<BillOrder>()
                    .Find(p => p.BillOrderId == id);
                if (billOrder == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<Role>().HardDeleteGuid(billOrder.BillOrderId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<BillOrder, BillOrderResponse>(billOrder);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BillOrderResponse> Update(Guid id, BillOrderRequest request)
        {
            try
            {
                BillOrder billOrder = _unitOfWork.Repository<BillOrder>()
                            .Find(x => x.BillOrderId == id);
                if (billOrder == null)
                {
                    throw new Exception();
                }
                billOrder = _mapper.Map(request, billOrder);

                await _unitOfWork.Repository<BillOrder>().UpdateDetached(billOrder);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<BillOrder, BillOrderResponse>(billOrder);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
