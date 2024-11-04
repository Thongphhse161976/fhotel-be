using FHotel.Service.DTOs.Holidays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Interfaces
{
    public interface IHolidayService
    {
        public Task<List<HolidayResponse>> GetAll();

        public Task<HolidayResponse> Get(Guid id);

        public Task<HolidayResponse> Create(HolidayRequest request);

        public Task<HolidayResponse> Delete(Guid id);

        public Task<HolidayResponse> Update(Guid id, HolidayRequest request);
    }
}
