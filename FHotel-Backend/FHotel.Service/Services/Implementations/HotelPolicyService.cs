using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.HotelPolicies;
using FHotel.Service.DTOs.Transactions;
using FHotel.Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Implementations
{
    public class HotelPolicyService : IHotelPolicyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public HotelPolicyService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<HotelPolicyResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<HotelPolicy>().GetAll()
                                            .ProjectTo<HotelPolicyResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<HotelPolicyResponse> Get(Guid id)
        {
            try
            {
                HotelPolicy hotelPolicy = null;
                hotelPolicy = await _unitOfWork.Repository<HotelPolicy>().GetAll()
                     .AsNoTracking()
                     .Include(x => x.Policy)
                    .Where(x => x.HotelPolicyId == id)
                    .FirstOrDefaultAsync();

                if (hotelPolicy == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<HotelPolicy, HotelPolicyResponse>(hotelPolicy);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<HotelPolicyResponse> Create(HotelPolicyRequest request)
        {
            try
            {
                var hotelPolicy = _mapper.Map<HotelPolicyRequest, HotelPolicy>(request);
                hotelPolicy.HotelPolicyId = Guid.NewGuid();
                await _unitOfWork.Repository<HotelPolicy>().InsertAsync(hotelPolicy);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<HotelPolicy, HotelPolicyResponse>(hotelPolicy);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<HotelPolicyResponse> Delete(Guid id)
        {
            try
            {
                HotelPolicy hotelPolicy = null;
                hotelPolicy = _unitOfWork.Repository<HotelPolicy>()
                    .Find(p => p.HotelPolicyId == id);
                if (hotelPolicy == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<HotelPolicy>().HardDeleteGuid(hotelPolicy.HotelPolicyId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<HotelPolicy, HotelPolicyResponse>(hotelPolicy);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<HotelPolicyResponse> Update(Guid id, HotelPolicyRequest request)
        {
            try
            {
                HotelPolicy hotelPolicy = _unitOfWork.Repository<HotelPolicy>()
                            .Find(x => x.HotelPolicyId == id);
                if (hotelPolicy == null)
                {
                    throw new Exception();
                }
                hotelPolicy = _mapper.Map(request, hotelPolicy);

                await _unitOfWork.Repository<HotelPolicy>().UpdateDetached(hotelPolicy);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<HotelPolicy, HotelPolicyResponse>(hotelPolicy);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<HotelPolicyResponse>> GetAllHotelPolicyByHotelId(Guid id)
        {

            var list = await _unitOfWork.Repository<HotelPolicy>().GetAll()
                                            .ProjectTo<HotelPolicyResponse>(_mapper.ConfigurationProvider)
                                            .Where(d => d.HotelId == id)
                                            .ToListAsync();
            return list;
        }

        public async Task<List<HotelPolicyResponse>> GetAllHotelPolicyByPolicyId(Guid id)
        {

            var list = await _unitOfWork.Repository<HotelPolicy>().GetAll()
                                            .ProjectTo<HotelPolicyResponse>(_mapper.ConfigurationProvider)
                                            .Where(d => d.PolicyId == id)
                                            .ToListAsync();
            return list;
        }
    }
}
