using FHotel.Service.DTOs.RoomStayHistories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Interfaces
{
    public interface IRoomStayHistoryService
    {
        public Task<List<RoomStayHistoryResponse>> GetAll();

        public Task<RoomStayHistoryResponse> Get(Guid id);

        public Task<RoomStayHistoryResponse> Create(RoomStayHistoryRequest request);

        public Task<RoomStayHistoryResponse> Delete(Guid id);

        public Task<RoomStayHistoryResponse> Update(Guid id, RoomStayHistoryRequest request);

        public Task<List<RoomStayHistoryResponse>> GetAllByReservationId(Guid id);
    }
}
