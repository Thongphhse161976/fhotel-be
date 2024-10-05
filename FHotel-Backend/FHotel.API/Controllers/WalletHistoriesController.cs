using FHotel.Service.DTOs.WalletHistories;
using FHotel.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    /// <summary>
    /// Controller for managing wallet-history.
    /// </summary>
    [Route("api/wallet-histories")]
    [ApiController]
    public class WalletHistoriesController : ControllerBase
    {
        private readonly IWalletHistoryService _walletHistoryService;

        public WalletHistoriesController(IWalletHistoryService walletHistoryService)
        {
            _walletHistoryService = walletHistoryService;
        }

        /// <summary>
        /// Get a list of all wallet-histories.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<WalletHistoryResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<WalletHistoryResponse>>> GetAll()
        {
            try
            {
                var rs = await _walletHistoryService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get wallet-history by wallet-history id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WalletHistoryResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<WalletHistoryResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _walletHistoryService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new wallet-history.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<WalletHistoryResponse>> Create([FromBody] WalletHistoryRequest request)
        {
            try
            {
                var result = await _walletHistoryService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete wallet-history by wallet-history id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<WalletHistoryResponse>> Delete(Guid id)
        {
            var rs = await _walletHistoryService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update wallet-history by wallet-history id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<WalletHistoryResponse>> Update(Guid id, [FromBody] WalletHistoryRequest request)
        {
            try
            {
                var rs = await _walletHistoryService.Update(id, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
