using FHotel.Service.DTOs.Transactions;
using FHotel.Service.DTOs.Wallets;
using FHotel.Service.Services.Interfaces;
using FHotel.Services.DTOs.Orders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    /// <summary>
    /// Controller for managing wallet.
    /// </summary>
    [Route("api/wallets")]
    [ApiController]
    public class WalletsController : ControllerBase
    {
        private readonly IWalletService _walletService;
        private readonly ITransactionService _transactionService;

        public WalletsController(IWalletService walletService, ITransactionService transactionService)
        {
            _walletService = walletService;
            _transactionService = transactionService;
        }

        /// <summary>
        /// Get a list of all wallets.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<WalletResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<WalletResponse>>> GetAll()
        {
            try
            {
                var rs = await _walletService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get wallet by wallet id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WalletResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<WalletResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _walletService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new wallet.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<WalletResponse>> Create([FromBody] WalletRequest request)
        {
            try
            {
                var result = await _walletService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete wallet by wallet id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<WalletResponse>> Delete(Guid id)
        {
            var rs = await _walletService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update wallet by wallet id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<WalletResponse>> Update(Guid id, [FromBody] WalletRequest request)
        {
            try
            {
                var rs = await _walletService.Update(id, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get all transaction by wallet id.
        /// </summary>
        [HttpGet("{id}/transactions")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TransactionResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<TransactionResponse>>> GetAllTransactionByWallet(Guid id)
        {
            try
            {
                var transactions = await _transactionService.GetAllTransactionByWalletId(id);
                return Ok(transactions);
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
