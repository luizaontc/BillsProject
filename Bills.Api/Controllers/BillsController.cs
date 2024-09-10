using Bills.Domain.Dto;
using Bills.Domain.Entities;
using Bills.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bills.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillsController : ControllerBase
    {
        private readonly IBillsService _billsService;

        public BillsController(IBillsService billsService)
        {
            _billsService = billsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBills([FromBody] FilterDto dto)
        {
            try
            {
                var bills = await _billsService.GetAllBills(dto);

                return Ok(bills);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Bill>> GetBill([FromRoute] int id)
        {
            try
            {
                var bill = await _billsService.GetBillAsync(id, 1);

                if (bill == null)
                {
                    return NotFound($"Bill with ID {id} not found.");
                }

                return Ok(bill);
            }
            catch (Exception ex)
            {
                // Pode-se retornar um status code diferente baseado no tipo de erro
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost]
        public async Task<ActionResult<Bill>> CreateBill(Bill bill)
        {
            try
            {
                _billsService.CreateBill(bill);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBill(int id, Bill bill)
        {
            try
            {
                var billReturn = _billsService.UpdateBill(id, bill);

                if (billReturn == "Ok")
                    return Ok(billReturn);

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBill(int id)
        {
            try
            {
                var bill = _billsService.DeleteBill(id);

                if (bill == "Ok")
                    return Ok(bill);

                return NoContent();

            }
            catch (ArgumentException ex)
            {
                return NotFound(ex);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
