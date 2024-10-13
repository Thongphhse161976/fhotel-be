using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.HotelStaffs;
using FHotel.Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Implementations
{
    public class HotelStaffService: IHotelStaffService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public HotelStaffService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<HotelStaffResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<HotelStaff>().GetAll()
                                            .ProjectTo<HotelStaffResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<HotelStaffResponse> Get(Guid id)
        {
            try
            {
                HotelStaff hotelStaff = null;
                hotelStaff = await _unitOfWork.Repository<HotelStaff>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.HotelStaffId == id)
                    .FirstOrDefaultAsync();

                if (hotelStaff == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<HotelStaff, HotelStaffResponse>(hotelStaff);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<HotelStaffResponse> Create(HotelStaffCreateteRequest request)
        {
            try
            {
                var hotelStaff = _mapper.Map<HotelStaffCreateteRequest, HotelStaff>(request);
                hotelStaff.HotelStaffId = Guid.NewGuid();
                await _unitOfWork.Repository<HotelStaff>().InsertAsync(hotelStaff);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<HotelStaff, HotelStaffResponse>(hotelStaff);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<HotelStaffResponse> Delete(Guid id)
        {
            try
            {
                HotelStaff hotelStaff = null;
                hotelStaff = _unitOfWork.Repository<HotelStaff>()
                    .Find(p => p.HotelStaffId == id);
                if (hotelStaff == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<Role>().HardDeleteGuid(hotelStaff.HotelStaffId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<HotelStaff, HotelStaffResponse>(hotelStaff);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<HotelStaffResponse> Update(Guid id, HotelStaffCreateteRequest request)
        {
            try
            {
                HotelStaff hotelStaff = _unitOfWork.Repository<HotelStaff>()
                            .Find(x => x.HotelStaffId == id);
                if (hotelStaff == null)
                {
                    throw new Exception();
                }
                hotelStaff = _mapper.Map(request, hotelStaff);

                await _unitOfWork.Repository<HotelStaff>().UpdateDetached(hotelStaff);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<HotelStaff, HotelStaffResponse>(hotelStaff);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
