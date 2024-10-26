using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.BillTransactionImages;
using FHotel.Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Implementations
{
    public class BillTransactionImageService: IBillTransactionImageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public BillTransactionImageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<BillTransactionImageResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<BillTransactionImage>().GetAll()
                                            .ProjectTo<BillTransactionImageResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<BillTransactionImageResponse> Get(Guid id)
        {
            try
            {
                BillTransactionImage billTransactionImage = null;
                billTransactionImage = await _unitOfWork.Repository<BillTransactionImage>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.BillTransactionImageId == id)
                    .FirstOrDefaultAsync();

                if (billTransactionImage == null)
                {
                    throw new Exception("BillTransactionImage not found");
                }

                return _mapper.Map<BillTransactionImage, BillTransactionImageResponse>(billTransactionImage);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<BillTransactionImageResponse> Create(BillTransactionImageRequest request)
        {
            try
            {
                var billTransactionImage = _mapper.Map<BillTransactionImageRequest, BillTransactionImage>(request);
                billTransactionImage.BillTransactionImageId = Guid.NewGuid();
                await _unitOfWork.Repository<BillTransactionImage>().InsertAsync(billTransactionImage);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<BillTransactionImage, BillTransactionImageResponse>(billTransactionImage);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<BillTransactionImageResponse> Delete(Guid id)
        {
            try
            {
                BillTransactionImage billTransactionImage = null;
                billTransactionImage = _unitOfWork.Repository<BillTransactionImage>()
                    .Find(p => p.BillTransactionImageId == id);
                if (billTransactionImage == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<BillTransactionImage>().HardDeleteGuid(billTransactionImage.BillTransactionImageId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<BillTransactionImage, BillTransactionImageResponse>(billTransactionImage);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BillTransactionImageResponse> Update(Guid id, BillTransactionImageRequest request)
        {
            try
            {
                BillTransactionImage billTransactionImage = _unitOfWork.Repository<BillTransactionImage>()
                            .Find(x => x.BillTransactionImageId == id);
                if (billTransactionImage == null)
                {
                    throw new Exception();
                }
                billTransactionImage = _mapper.Map(request, billTransactionImage);

                await _unitOfWork.Repository<BillTransactionImage>().UpdateDetached(billTransactionImage);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<BillTransactionImage, BillTransactionImageResponse>(billTransactionImage);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
