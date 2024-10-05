using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.Feedbacks;
using FHotel.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    /// <summary>
    /// Controller for managing feedback.
    /// </summary>
    [Route("api/feedbacks")]
    [ApiController]
    public class FeedbacksController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbacksController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        /// <summary>
        /// Get a list of all feedbacks.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FeedbackResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<FeedbackResponse>>> GetAll()
        {
            try
            {
                var rs = await _feedbackService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get feedback by feedback id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FeedbackResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<FeedbackResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _feedbackService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new feedback.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<FeedbackResponse>> Create([FromBody] FeedbackRequest request)
        {
            try
            {
                var result = await _feedbackService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete feedback by feedback id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<FeedbackResponse>> Delete(Guid id)
        {
            var rs = await _feedbackService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update feedback by feedback id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<FeedbackResponse>> Update(Guid id, [FromBody] FeedbackRequest request)
        {
            try
            {
                var rs = await _feedbackService.Update(id, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
