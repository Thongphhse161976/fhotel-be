using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.Bills;
using FHotel.Service.Services.Interfaces;
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
        public BillService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
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
            try
            {
                var bill = _mapper.Map<BillRequest, Bill>(request);
                bill.BillId = Guid.NewGuid();
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
                await _unitOfWork.Repository<Role>().HardDeleteGuid(bill.BillId);
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
