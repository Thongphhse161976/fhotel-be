using FHotel.Service.DTOs.Facilities;
using FHotel.Service.DTOs.RoomTypes;
using FHotel.Service.DTOs.TypePricings;
using FHotel.Service.Services.Interfaces;
using FHotel.Services.DTOs.Hotels;
using FHotel.Services.DTOs.RoomFacilities;
using FHotel.Services.DTOs.RoomImages;
using FHotel.Services.DTOs.RoomTypes;
using FHotel.Services.Services.Implementations;
using FHotel.Services.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    /// <summary>
    /// Controller for managing room-type.
    /// </summary>
    [Route("api/room-types")]
    [ApiController]
    public class RoomTypesController : ControllerBase
    {
        private readonly IRoomTypeService _roomTypeService;
        private readonly IRoomImageService _roomImageService;
        private readonly IRoomFacilityService _roomFacilityService;
        private readonly ITypePricingService _typePricingService;

        public RoomTypesController(IRoomTypeService roomTypeService, IRoomImageService roomImageService, IRoomFacilityService roomFacilityService, ITypePricingService typePricingService)
        {
            _roomTypeService = roomTypeService;
            _roomImageService = roomImageService;
            _roomFacilityService = roomFacilityService;
            _typePricingService = typePricingService;
        }

        /// <summary>
        /// Get a list of all room-types.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RoomTypeResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<RoomTypeResponse>>> GetAll()
        {
            try
            {
                var rs = await _roomTypeService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get room-type by room-type id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RoomTypeResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<RoomTypeResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _roomTypeService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new room-type.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RoomTypeResponse>> Create([FromBody] RoomTypeCreateRequest request)
        {
            try
            {
                var result = await _roomTypeService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (ValidationException ex)
            {
                // Access validation errors from ex.Errors
                return BadRequest(new
                {
                    message = "Validation failed",
                    errors = ex.Errors.Select(e => e.ErrorMessage).ToList()
                });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete room-type by room-type id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<RoomTypeResponse>> Delete(Guid id)
        {
            var rs = await _roomTypeService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update room-type by room-type id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<RoomTypeResponse>> Update(Guid id, [FromBody] RoomTypeUpdateRequest request)
        {
            try
            {
                var rs = await _roomTypeService.Update(id, request);
                return Ok(rs);
            }
            catch (ValidationException ex)
            {
                // Access validation errors from ex.Errors
                return BadRequest(new
                {
                    message = "Validation failed",
                    errors = ex.Errors.Select(e => e.ErrorMessage).ToList()
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

       


        /// <summary>
        /// Get all room type images room type ID.
        /// </summary>
        /// <param name="roomTypeId">The ID of the Room Type.</param>
        /// <returns>A list of room images.</returns>
        [HttpGet("{roomTypeId}/room-images")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<RoomImageResponse>>> GetAllRoomImageByRoomTypeId(Guid roomTypeId)
        {
            try
            {
                var roomTypeList = await _roomImageService.GetAllRoomImageByRoomTypeId(roomTypeId);

                if (roomTypeList == null || !roomTypeList.Any())
                {
                    return NotFound(new { message = "No image found for this room type." });
                }

                return Ok(roomTypeList);
            }
            catch (Exception ex)
            {
                // Log the exception if you have logging set up
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        /// <summary>
        /// Get a list of all facilities by room-type id.
        /// </summary>
        [HttpGet("{id}/room-facilities")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RoomFacilityResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<RoomFacilityResponse>>> GetAllRoomFacilityByRoomTypeId(Guid id)
        {
            try
            {
                var rs = await _roomFacilityService.GetAllRoomFacilityByRoomTypeId(id);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("search")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<HotelResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<HotelResponse>>> SearchRoomTypesWithQuantities([FromBody] List<RoomSearchRequest> searchRequests, [FromQuery] string? cityName)
        {
            try
            {
                // Call the service to search with multiple room types and quantities
                var result = await _roomTypeService.SearchHotelsWithRoomTypes(searchRequests, cityName);

                if (result == null || !result.Any())
                {
                    return NotFound("No rooms found matching the search criteria.");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Get a list of all type-pricings by room-type id.
        /// </summary>
        [HttpGet("{id}/type-pricings")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TypePricingResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<TypePricingResponse>>> GetAllTypePriceByTypeId(Guid id)
        {
            try
            {
                var rs = await _typePricingService.GetAllByRoomTypeId(id);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
