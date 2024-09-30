using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Services.DTOs.Countries;
using FHotel.Services.DTOs.Feedbacks;
using FHotel.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Implementations
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public FeedbackService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<FeedbackResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<Feedback>().GetAll()
                                            .ProjectTo<FeedbackResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<FeedbackResponse> Get(Guid id)
        {
            try
            {
                Feedback feedback = null;
                feedback = await _unitOfWork.Repository<Feedback>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.FeedbackId == id)
                    .FirstOrDefaultAsync();

                if (feedback == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<Feedback, FeedbackResponse>(feedback);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<FeedbackResponse> Create(FeedbackRequest request)
        {
            try
            {
                var feedback = _mapper.Map<FeedbackRequest, Feedback>(request);
                feedback.FeedbackId = Guid.NewGuid();
                await _unitOfWork.Repository<Feedback>().InsertAsync(feedback);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Feedback, FeedbackResponse>(feedback);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<FeedbackResponse> Delete(Guid id)
        {
            try
            {
                Feedback feedback = null;
                feedback = _unitOfWork.Repository<Feedback>()
                    .Find(p => p.FeedbackId == id);
                if (feedback == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<Role>().HardDeleteGuid(feedback.FeedbackId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<Feedback, FeedbackResponse>(feedback);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<FeedbackResponse> Update(Guid id, FeedbackRequest request)
        {
            try
            {
                Feedback feedback = _unitOfWork.Repository<Feedback>()
                            .Find(x => x.FeedbackId == id);
                if (feedback == null)
                {
                    throw new Exception();
                }
                feedback = _mapper.Map(request, feedback);

                await _unitOfWork.Repository<Feedback>().UpdateDetached(feedback);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Feedback, FeedbackResponse>(feedback);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
