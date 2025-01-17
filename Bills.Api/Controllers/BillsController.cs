﻿using Bills.Api.Filters;
using Bills.Domain.Dto;
using Bills.Domain.Dto.Bills;
using Bills.Domain.Entities;
using Bills.Service.Interface;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Bill>> CreateBill([FromBody] BillDto dto)
        {
            try
            {
                dto.userId = Convert.ToInt32(HttpContext.User.FindFirst("UserId").Value);

                await _billsService.CreateBill(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }
        [ServiceFilter(typeof(InjectUserIdFilter))]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllBills([FromQuery] FilterDto dto)
        {
            try
            {
                if (dto.userId == null)
                    return Unauthorized("Your token expired!");
                

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

        [Authorize]
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
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }


        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBill(int id, BillDto bill)
        {
            try
            {
                var billReturn = await _billsService.UpdateBill(id, bill);

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

    }
}
