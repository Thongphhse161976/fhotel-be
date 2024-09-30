using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Services.DTOs.Countries;
using FHotel.Services.DTOs.ReservationDetails;
using FHotel.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Implementations
{
    public class ReservationDetailService : IReservationDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public ReservationDetailService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ReservationDetailResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<ReservationDetail>().GetAll()
                                            .ProjectTo<ReservationDetailResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<ReservationDetailResponse> Get(Guid id)
        {
            try
            {
                ReservationDetail reservationDetail = null;
                reservationDetail = await _unitOfWork.Repository<ReservationDetail>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.ReservationDetailId == id)
                    .FirstOrDefaultAsync();

                if (reservationDetail == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<ReservationDetail, ReservationDetailResponse>(reservationDetail);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<ReservationDetailResponse> Create(ReservationDetailRequest request)
        {
            try
            {
                var reservationDetail = _mapper.Map<ReservationDetailRequest, ReservationDetail>(request);
                reservationDetail.ReservationDetailId = Guid.NewGuid();
                await _unitOfWork.Repository<ReservationDetail>().InsertAsync(reservationDetail);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<ReservationDetail, ReservationDetailResponse>(reservationDetail);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<ReservationDetailResponse> Delete(Guid id)
        {
            try
            {
                ReservationDetail reservationDetail = null;
                reservationDetail = _unitOfWork.Repository<ReservationDetail>()
                    .Find(p => p.ReservationDetailId == id);
                if (reservationDetail == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<Role>().HardDeleteGuid(reservationDetail.ReservationDetailId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<ReservationDetail, ReservationDetailResponse>(reservationDetail);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ReservationDetailResponse> Update(Guid id, ReservationDetailRequest request)
        {
            try
            {
                ReservationDetail reservationDetail = _unitOfWork.Repository<ReservationDetail>()
                            .Find(x => x.ReservationDetailId == id);
                if (reservationDetail == null)
                {
                    throw new Exception();
                }
                reservationDetail = _mapper.Map(request, reservationDetail);

                await _unitOfWork.Repository<ReservationDetail>().UpdateDetached(reservationDetail);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<ReservationDetail, ReservationDetailResponse>(reservationDetail);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
