using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.Bills;
using FHotel.Service.Services.Interfaces;
using FHotel.Services.DTOs.Orders;
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
        public BillService(IUnitOfWork unitOfWork, IMapper mapper, IOrderService orderService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _orderService = orderService;
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

                return _mapper.Map<Bill, BillResponse>(bill);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
