using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.Timetable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Interfaces
{
    public interface ITimetableService
    {
        public Task<List<TimetableResponse>> GetAll();

        public Task<TimetableResponse> Get(Guid id);

        public Task<TimetableResponse> Create(TimetableRequest request);

        public Task<TimetableResponse> Delete(Guid id);

        public Task<TimetableResponse> Update(Guid id, TimetableRequest request);
    }
}
