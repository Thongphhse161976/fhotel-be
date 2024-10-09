﻿using FHotel.Service.DTOs.Hotels;
using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.Hotels;
using FHotel.Services.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    /// <summary>
    /// Controller for managing hotel.
    /// </summary>
    [Route("api/hotels")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelService _hotelService;

        public HotelsController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        /// <summary>
        /// Get a list of all hotels.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<HotelResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<HotelResponse>>> GetAll()
        {
            try
            {
                var rs = await _hotelService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get hotel by hotel id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HotelResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<HotelResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _hotelService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new hotel.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<HotelResponse>> Create([FromBody] HotelCreateRequest request)
        {
            try
            {
                var result = await _hotelService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (ValidationException ex)
            {
                // Access validation errors from ex.Errors
                return BadRequest(new { message = "Validation failed", errors = ex.Errors.Select(e => e.ErrorMessage) });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete hotel by hotel id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<HotelResponse>> Delete(Guid id)
        {
            var rs = await _hotelService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update hotel by hotel id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<HotelResponse>> Update(Guid id, [FromBody] HotelUpdateRequest request)
        {
            try
            {
                var rs = await _hotelService.Update(id, request);
                return Ok(rs);
            }
            catch (ValidationException ex)
            {
                // Return validation errors as a list of error messages
                return BadRequest(new { message = "Validation failed", errors = ex.Errors.Select(e => e.ErrorMessage) });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
