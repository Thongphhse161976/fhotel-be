using FHotel.Service.DTOs.EscrowWallets;
using FHotel.Service.DTOs.Transactions;
using FHotel.Service.Services.Implementations;
using FHotel.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHotel.API.Controllers
{
    /// <summary>
    /// Controller for managing escrow wallet.
    /// </summary>
    [Route("api/escrow-wallets")]
    [ApiController]
    public class EscrowWalletsController : ControllerBase
    {
        private readonly IEscrowWalletService _escrowWalletService;
        private readonly ITransactionService _transactionService;

        public EscrowWalletsController(IEscrowWalletService escrowWalletService, ITransactionService transactionService)
        {
            _escrowWalletService = escrowWalletService;
            _transactionService = transactionService;
        }

        /// <summary>
        /// Get a list of all escrowWallets.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<EscrowWalletResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<EscrowWalletResponse>>> GetAll()
        {
            try
            {
                var rs = await _escrowWalletService.GetAll();
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get escrowWallet by escrowWallet id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EscrowWalletResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<EscrowWalletResponse>> Get(Guid id)
        {
            try
            {
                var rs = await _escrowWalletService.Get(id);
                return Ok(rs);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create new escrowWallet.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EscrowWalletResponse>> Create([FromBody] EscrowWalletRequest request)
        {
            try
            {
                var result = await _escrowWalletService.Create(request);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete escrowWallet by escrowWallet id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<EscrowWalletResponse>> Delete(Guid id)
        {
            var rs = await _escrowWalletService.Delete(id);
            return Ok(rs);
        }

        /// <summary>
        /// Update escrowWallet by escrowWallet id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<EscrowWalletResponse>> Update(Guid id, [FromBody] EscrowWalletRequest request)
        {
            try
            {
                var rs = await _escrowWalletService.Update(id, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Increase Escro wallet balance.
        /// </summary>
        [HttpPost("increase-balance")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EscrowWalletResponse>> IncreaseBalance([FromBody] IncreaseBalanceRequest request)
        {
            try
            {
                var result = await _escrowWalletService.IncreaseBalance(request.ReservationId, request.Amount);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public class IncreaseBalanceRequest
        {
            public Guid ReservationId { get; set; }
            public decimal Amount { get; set; }
        }

        /// <summary>
        /// Get all transaction by escrow wallet.
        /// </summary>
        [HttpGet("transactions")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TransactionResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<TransactionResponse>>> GetAllTransactionByEscrowWallet()
        {
            try
            {
                var transactions = await _transactionService.GetAllTransactionByEscrowWallet();
                return Ok(transactions);
            }
            catch
            {
                return NotFound();
            }
        }

    }
}
