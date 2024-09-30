using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Services.DTOs.Countries;
using FHotel.Services.DTOs.RoomImages;
using FHotel.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Implementations
{
    public class RoomImageService : IRoomImageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public RoomImageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<RoomImageResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<RoomImage>().GetAll()
                                            .ProjectTo<RoomImageResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<RoomImageResponse> Get(Guid id)
        {
            try
            {
                RoomImage roomImage = null;
                roomImage = await _unitOfWork.Repository<RoomImage>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.RoomImageId == id)
                    .FirstOrDefaultAsync();

                if (roomImage == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<RoomImage, RoomImageResponse>(roomImage);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<RoomImageResponse> Create(RoomImageRequest request)
        {
            try
            {
                var roomImage = _mapper.Map<RoomImageRequest, RoomImage>(request);
                roomImage.RoomImageId = Guid.NewGuid();
                await _unitOfWork.Repository<RoomImage>().InsertAsync(roomImage);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<RoomImage, RoomImageResponse>(roomImage);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<RoomImageResponse> Delete(Guid id)
        {
            try
            {
                RoomImage roomImage = null;
                roomImage = _unitOfWork.Repository<RoomImage>()
                    .Find(p => p.RoomImageId == id);
                if (roomImage == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<Role>().HardDeleteGuid(roomImage.RoomImageId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<RoomImage, RoomImageResponse>(roomImage);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RoomImageResponse> Update(Guid id, RoomImageRequest request)
        {
            try
            {
                RoomImage roomImage = _unitOfWork.Repository<RoomImage>()
                            .Find(x => x.RoomImageId == id);
                if (roomImage == null)
                {
                    throw new Exception();
                }
                roomImage = _mapper.Map(request, roomImage);

                await _unitOfWork.Repository<RoomImage>().UpdateDetached(roomImage);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<RoomImage, RoomImageResponse>(roomImage);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
