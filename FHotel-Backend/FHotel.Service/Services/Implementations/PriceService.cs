using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Services.DTOs.Countries;
using FHotel.Services.DTOs.Prices;
using FHotel.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Implementations
{
    public class PriceService : IPriceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public PriceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<PriceResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<Price>().GetAll()
                                            .ProjectTo<PriceResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<PriceResponse> Get(Guid id)
        {
            try
            {
                Price price = null;
                price = await _unitOfWork.Repository<Price>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.PriceId == id)
                    .FirstOrDefaultAsync();

                if (price == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<Price, PriceResponse>(price);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<PriceResponse> Create(PriceRequest request)
        {
            try
            {
                var price = _mapper.Map<PriceRequest, Price>(request);
                price.PriceId = Guid.NewGuid();
                await _unitOfWork.Repository<Price>().InsertAsync(price);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Price, PriceResponse>(price);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<PriceResponse> Delete(Guid id)
        {
            try
            {
                Price price = null;
                price = _unitOfWork.Repository<Price>()
                    .Find(p => p.PriceId == id);
                if (price == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<Role>().HardDeleteGuid(price.PriceId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<Price, PriceResponse>(price);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PriceResponse> Update(Guid id, PriceRequest request)
        {
            try
            {
                Price price = _unitOfWork.Repository<Price>()
                            .Find(x => x.PriceId == id);
                if (price == null)
                {
                    throw new Exception();
                }
                price = _mapper.Map(request, price);

                await _unitOfWork.Repository<Price>().UpdateDetached(price);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Price, PriceResponse>(price);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
