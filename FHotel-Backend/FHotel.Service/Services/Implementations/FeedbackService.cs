﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Services.DTOs.Feedbacks;
using FHotel.Services.DTOs.OrderDetails;
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
                     .Include(x => x.Reservation)
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
            // Set the UTC offset for UTC+7
            TimeSpan utcOffset = TimeSpan.FromHours(7);

            // Get the current UTC time
            DateTime utcNow = DateTime.UtcNow;

            // Convert the UTC time to UTC+7
            DateTime localTime = utcNow + utcOffset;
            try
            {
                var feedback = _mapper.Map<FeedbackRequest, Feedback>(request);
                feedback.FeedbackId = Guid.NewGuid();
                feedback.CreatedDate = localTime;
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
                await _unitOfWork.Repository<Feedback>().HardDeleteGuid(feedback.FeedbackId);
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

        public async Task<IEnumerable<FeedbackResponse>> GetAllFeedbackByReservationId(Guid reservationId)
        {
            var feedbacks = await _unitOfWork.Repository<Feedback>().GetAll()
                     .AsNoTracking()
                     .Include(x=> x.Reservation)
                        .ThenInclude(x=> x.Customer)
                    .Include(x => x.Reservation)
                        .ThenInclude(x => x.RoomType)
                            .ThenInclude(x=> x.Type)
                    .Where(x => x.ReservationId == reservationId)
                    .ToListAsync();

            return _mapper.Map<IEnumerable<Feedback>, IEnumerable<FeedbackResponse>>(feedbacks);
        }

        public async Task<IEnumerable<FeedbackResponse>> GetAllFeedbackByHotelId(Guid hotelId)
        {
            var feedbacks = await _unitOfWork.Repository<Feedback>().GetAll()
                     .AsNoTracking()
                     .Include(x => x.Reservation)
                        .ThenInclude(x => x.Customer)
                    .Include(x => x.Reservation)
                        .ThenInclude(x => x.RoomType)
                            .ThenInclude(x => x.Type)
                    .Where(x => x.Reservation.RoomType.HotelId == hotelId)
                    .ToListAsync();

            return _mapper.Map<IEnumerable<Feedback>, IEnumerable<FeedbackResponse>>(feedbacks);
        }

        public async Task<IEnumerable<FeedbackResponse>> GetAllFeedbackByOwnerId(Guid ownerId)
        {
            var feedbacks = await _unitOfWork.Repository<Feedback>().GetAll()
                     .AsNoTracking()
                     .Include(x => x.Reservation)
                        .ThenInclude(x => x.Customer)
                    .Include(x => x.Reservation)
                        .ThenInclude(x => x.RoomType)
                            .ThenInclude(x => x.Hotel)
                    .Where(x => x.Reservation.RoomType.Hotel.OwnerId == ownerId)
                    .ToListAsync();

            return _mapper.Map<IEnumerable<Feedback>, IEnumerable<FeedbackResponse>>(feedbacks);
        }
        
        public async Task<IEnumerable<FeedbackResponse>> GetAllFeedbackByStaffId(Guid staffId)
        {
            // Retrieve the HotelID associated with the HotelStaff
            var hotelStaff = await _unitOfWork.Repository<HotelStaff>()
                                              .GetAll()
                                              .Where(hs => hs.UserId == staffId)
                                              .FirstOrDefaultAsync();

            if (hotelStaff == null)
            {
                throw new Exception("Staff not found or not associated with any hotel.");
            }

            var hotelId = hotelStaff.HotelId;
            var feedbacks = await _unitOfWork.Repository<Feedback>().GetAll()
                     .AsNoTracking()
                     .Include(x => x.Reservation)
                        .ThenInclude(x => x.Customer)
                    .Include(x => x.Reservation)
                        .ThenInclude(x => x.RoomType)
                            .ThenInclude(x => x.Hotel)
                    .Where(x => x.Reservation.RoomType.HotelId == hotelId)
                    .ToListAsync();

            return _mapper.Map<IEnumerable<Feedback>, IEnumerable<FeedbackResponse>>(feedbacks);
        }


    }
}
