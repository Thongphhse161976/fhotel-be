﻿using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Interfaces
{
    public interface IOrderService
    {
        public Task<List<OrderResponse>> GetAll();

        public Task<OrderResponse> Get(Guid id);

        public Task<OrderResponse> Create(OrderRequest request);

        public Task<OrderResponse> Delete(Guid id);

        public Task<OrderResponse> Update(Guid id, OrderRequest request);

        public Task<List<OrderResponse>> GetAllByReservationId(Guid id);
    }
}
