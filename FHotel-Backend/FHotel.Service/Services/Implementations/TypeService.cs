using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.Types;
using FHotel.Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Type = FHotel.Repository.Models.Type;

namespace FHotel.Service.Services.Implementations
{
    public class TypeService: ITypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public TypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<TypeResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<Type>().GetAll()
                                            .ProjectTo<TypeResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<TypeResponse> Get(Guid id)
        {
            try
            {
                Type type = null;
                type = await _unitOfWork.Repository<Type>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.TypeId == id)
                    .FirstOrDefaultAsync();

                if (type == null)
                {
                    throw new Exception("Type not found");
                }

                return _mapper.Map<Type, TypeResponse>(type);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<TypeResponse> Create(TypeCreateRequest request)
        {
            // Set the UTC offset for UTC+7
            TimeSpan utcOffset = TimeSpan.FromHours(7);

            // Get the current UTC time
            DateTime utcNow = DateTime.UtcNow;

            // Convert the UTC time to UTC+7
            DateTime localTime = utcNow + utcOffset;
            try
            {
                var type = _mapper.Map<TypeCreateRequest, Type>(request);
                type.TypeId = Guid.NewGuid();
                type.CreatedDate = localTime;
                await _unitOfWork.Repository<Type>().InsertAsync(type);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Type, TypeResponse>(type);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<TypeResponse> Delete(Guid id)
        {
            try
            {
                Type type = null;
                type = _unitOfWork.Repository<Type>()
                    .Find(p => p.TypeId == id);
                if (type == null)
                {
                    throw new Exception("Type not found");
                }
                await _unitOfWork.Repository<Role>().HardDeleteGuid(type.TypeId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<Type, TypeResponse>(type);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<TypeResponse> Update(Guid id, TypeUpdateRequest request)
        {
            // Set the UTC offset for UTC+7
            TimeSpan utcOffset = TimeSpan.FromHours(7);

            // Get the current UTC time
            DateTime utcNow = DateTime.UtcNow;

            // Convert the UTC time to UTC+7
            DateTime localTime = utcNow + utcOffset;
            try
            {
                Type type = _unitOfWork.Repository<Type>()
                            .Find(x => x.TypeId == id);
                if (type == null)
                {
                    throw new Exception();
                }
                type = _mapper.Map(request, type);
                type.UpdatedDate = localTime;
                await _unitOfWork.Repository<Type>().UpdateDetached(type);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Type, TypeResponse>(type);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
