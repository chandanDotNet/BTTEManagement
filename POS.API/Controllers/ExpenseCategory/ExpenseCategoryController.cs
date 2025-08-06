using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS.API.Controllers.ExpenseCategory
{
    [Route("api")]
    [ApiController]
    [Authorize]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ExpenseCategoryController'
    public class ExpenseCategoryController : BaseController
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ExpenseCategoryController'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ExpenseCategoryController._mediator'
        public IMediator _mediator { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ExpenseCategoryController._mediator'

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ExpenseCategoryController.ExpenseCategoryController(IMediator)'
        public ExpenseCategoryController(IMediator mediator)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ExpenseCategoryController.ExpenseCategoryController(IMediator)'
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get Expense type.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("ExpenseCategory/{id}", Name = "GetExpenseCategory")]
        [Produces("application/json", "application/xml", Type = typeof(ExpenseCategoryDto))]
        public async Task<IActionResult> GetExpenseCategory(Guid id)
        {
            var getExpenseCategoryQuery = new GetExpenseCategoryQuery { Id = id };
            var result = await _mediator.Send(getExpenseCategoryQuery);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Get Expense Types.
        /// </summary>
        /// <returns></returns>
        [HttpGet("ExpenseCategories")]
        [Produces("application/json", "application/xml", Type = typeof(List<ExpenseCategoryDto>))]
        public async Task<IActionResult> ExpenseCategories()
        {
            var getAllExpenseCategoryQuery = new GetAllExpenseCategoryQuery { };
            var result = await _mediator.Send(getAllExpenseCategoryQuery);
            return Ok(result);
        }

        /// <summary>
        /// Create Expense Type.
        /// </summary>
        /// <param name="addExpenseCategoryCommand"></param>
        /// <returns></returns>
        [HttpPost("ExpenseCategory")]
        [Produces("application/json", "application/xml", Type = typeof(ExpenseCategoryDto))]
        public async Task<IActionResult> AddExpenseCategory(AddExpenseCategoryCommand addExpenseCategoryCommand)
        {
            var response = await _mediator.Send(addExpenseCategoryCommand);
            if (!response.Success)
            {
                return ReturnFormattedResponse(response);
            }
            return CreatedAtAction("GetExpenseCategory", new { id = response.Data.Id }, response.Data);
        }

        /// <summary>
        /// Update Expense Type.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="updateExpenseCategoryCommand"></param>
        /// <returns></returns>
        [HttpPut("ExpenseCategory/{Id}")]
        [Produces("application/json", "application/xml", Type = typeof(ExpenseCategoryDto))]
        public async Task<IActionResult> UpdateExpenseCategory(Guid Id, UpdateExpenseCategoryCommand updateExpenseCategoryCommand)
        {
            updateExpenseCategoryCommand.Id = Id;
            var result = await _mediator.Send(updateExpenseCategoryCommand);
            return ReturnFormattedResponse(result);

        }

        /// <summary>
        /// Delete Expense Type.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("ExpenseCategory/{Id}")]
        public async Task<IActionResult> DeleteExpenseCategory(Guid Id)
        {
            var deleteExpenseCategoryCommand = new DeleteExpenseCategoryCommand { Id = Id };
            var result = await _mediator.Send(deleteExpenseCategoryCommand);
            return ReturnFormattedResponse(result);
        }
    }
}
