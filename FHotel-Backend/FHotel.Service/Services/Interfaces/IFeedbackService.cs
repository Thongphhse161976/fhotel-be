using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.Feedbacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Interfaces
{
    public interface IFeedbackService
    {
        public Task<List<FeedbackResponse>> GetAll();

        public Task<FeedbackResponse> Get(Guid id);

        public Task<FeedbackResponse> Create(FeedbackRequest request);

        public Task<FeedbackResponse> Delete(Guid id);

        public Task<FeedbackResponse> Update(Guid id, FeedbackRequest request);
        public Task<IEnumerable<FeedbackResponse>> GetAllFeedbackByReservationId(Guid reservationId);
    }
}
