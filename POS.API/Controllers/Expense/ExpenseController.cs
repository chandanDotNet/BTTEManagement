using POS.Data.Resources;
using POS.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using POS.API.Helpers;
using POS.MediatR.Commands;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Commands;

namespace POS.API.Controllers.Expense
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ExpenseController : BaseController
    {
        private IMediator _mediator;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        public ExpenseController(
            IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Add Expenses
        /// </summary>
        /// <param name="addExpenseCommand"></param>
        /// <returns></returns>
        [HttpPost]
        [ClaimCheck("EXP_ADD_EXPENSE")]
        public async Task<IActionResult> AddExpense([FromBody] AddExpenseCommand addExpenseCommand)
        {
            var result = await _mediator.Send(addExpenseCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Add Master Expenses
        /// </summary>
        /// <param name="addExpenseCommand"></param>
        /// <returns></returns>
        [HttpPost("AddExpenseWithDetails")]
        //[ClaimCheck("EXP_ADD_EXPENSE")]
        public async Task<IActionResult> AddMasterExpense([FromBody] AddMasterExpenseCommand addMasterExpenseCommand)
        {
            GetNewExpenseNumberCommand getNewExpenseNumber = new GetNewExpenseNumberCommand();
            string ExpenseNo = await _mediator.Send(getNewExpenseNumber);
            addMasterExpenseCommand.ExpenseNo = ExpenseNo;
            var result = await _mediator.Send(addMasterExpenseCommand);
            if (result.Success)
            {
                Guid id = result.Data.Id;
                foreach(var  item in addMasterExpenseCommand.ExpenseDetails)
                {
                    AddExpenseCommand addExpenseCommand=new AddExpenseCommand();                   
                    addExpenseCommand=item;
                    addExpenseCommand.MasterExpenseId = id;
                    addExpenseCommand.TripId = result.Data.TripId;
                    var result2 = await _mediator.Send(addExpenseCommand);
                }
            }
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Update Master Expenses
        /// </summary>
        /// <param name="updateMasterExpenseCommand"></param>
        /// <returns></returns>
        [HttpPost("UpdateExpenseWithDetails")]
        //[ClaimCheck("EXP_ADD_EXPENSE")]
        public async Task<IActionResult> UpdateMasterExpense([FromBody] UpdateMasterExpenseCommand updateMasterExpenseCommand)
        {
            var result = await _mediator.Send(updateMasterExpenseCommand);
            if (result.Success)
            {
               // Guid id = result.Data.Id;
                foreach (var item in updateMasterExpenseCommand.ExpenseDetails)
                {
                    UpdateExpenseCommand updateExpenseCommand = new UpdateExpenseCommand();
                    updateExpenseCommand = item;                    
                    var result2 = await _mediator.Send(updateExpenseCommand);
                }
            }
            return ReturnFormattedResponse(result);
        }


        /// <summary>
        /// Update Expense.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateExpenseCommand"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ClaimCheck("EXP_UPDATE_EXPENSE")]
        public async Task<IActionResult> UpdateExpense(Guid id, [FromBody] UpdateExpenseCommand updateExpenseCommand)
        {
            updateExpenseCommand.Id = id;
            var result = await _mediator.Send(updateExpenseCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Update Expense Status.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateExpenseStatusCommand"></param>
        /// <returns></returns>
        [HttpPut("UpdateExpenseStatus/{id}")]
        //[ClaimCheck("EXP_UPDATE_EXPENSE")]
        public async Task<IActionResult> UpdateExpenseStatus(Guid id, [FromBody] UpdateExpenseStatusCommand updateExpenseStatusCommand)
        {
            updateExpenseStatusCommand.Id = id;
            var result = await _mediator.Send(updateExpenseStatusCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Update Master Expense Status.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateMasterExpenseStatusCommand"></param>
        /// <returns></returns>
        [HttpPut("UpdateMasterExpenseStatus/{id}")]
        //[ClaimCheck("EXP_UPDATE_EXPENSE")]
        public async Task<IActionResult> UpdateMasterExpenseStatus(Guid id, [FromBody] UpdateMasterExpenseStatusCommand updateMasterExpenseStatusCommand)
        {
            updateMasterExpenseStatusCommand.Id = id;
            var result = await _mediator.Send(updateMasterExpenseStatusCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Get Expenses 
        /// </summary>
        /// <param name="expenseResource"></param>
        /// <returns></returns>
        [HttpGet]
        [ClaimCheck("EXP_VIEW_EXPENSES")]
        public async Task<IActionResult> GetExpenses([FromQuery] ExpenseResource expenseResource)
        {

            var getAllExpenseQuery = new GetAllExpenseQuery
            {
                ExpenseResource = expenseResource
            };

            var result = await _mediator.Send(getAllExpenseQuery);

            var paginationMetadata = new
            {
                totalCount = result.TotalCount,
                pageSize = result.PageSize,
                skip = result.Skip,
                totalPages = result.TotalPages,
                totalAmount=result.TotalAmount
            };
            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));
            return Ok(result);
        }


        /// <summary>
        /// Get All Master Expenses 
        /// </summary>
        /// <param name="masterExpenseResource"></param>
        /// <returns></returns>
        [HttpGet("GetAllExpensesDetailsList")]
        //[ClaimCheck("EXP_VIEW_EXPENSES")]
        public async Task<IActionResult> GetAllExpensesDetailsList([FromQuery] ExpenseResource masterExpenseResource)
        {

            var getAllMasterExpenseQuery = new GetAllMasterExpenseQuery
            {
                ExpenseResource = masterExpenseResource
            };

            var result = await _mediator.Send(getAllMasterExpenseQuery);

            var paginationMetadata = new
            {
                totalCount = result.TotalCount,
                pageSize = result.PageSize,
                skip = result.Skip,
                totalPages = result.TotalPages,
                totalAmount = result.TotalAmount
            };
            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));
            return Ok(result);
        }

        /// <summary>
        /// Get Expenses 
        /// </summary>
        /// <param name="expenseResource"></param>
        /// <returns></returns>
        [HttpGet("report")]
        [ClaimCheck("EXP_VIEW_EXPENSES")]
        public async Task<IActionResult> GetExpensesReport([FromQuery] ExpenseResource expenseResource)
        {

            var getAllExpenseQuery = new GetAllExpenseReportQuery
            {
                ExpenseResource = expenseResource
            };

            var result = await _mediator.Send(getAllExpenseQuery);

            var paginationMetadata = new
            {
                totalCount = result.TotalCount,
                pageSize = result.PageSize,
                skip = result.Skip,
                totalPages = result.TotalPages,
                totalAmount = result.TotalAmount
            };
            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));
            return Ok(result);
        }

        /// <summary>
        /// Gets the Expense by Id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetExpense")]
        [ClaimCheck("EXP_VIEW_EXPENSES")]
        public async Task<IActionResult> GetExpense(Guid id)
        {
            var query = new GetExpenseQuery { Id = id };
            var result = await _mediator.Send(query);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Deletes the Expense.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        //[ClaimCheck("EXP_DELETE_EXPENSE")]
        public async Task<IActionResult> DeleteExpense(Guid id)
        {
            var command = new DeleteExpenseCommand() { Id = id };
            var result = await _mediator.Send(command);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Deletes the Master Expense.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete("DeleteMasterExpense/{id}")]
        //[ClaimCheck("EXP_DELETE_EXPENSE")]
        public async Task<IActionResult> DeleteMasterExpense(Guid id)
        {
            var command = new DeleteMasterExpenseCommand() { Id = id };
            var result = await _mediator.Send(command);
            return ReturnFormattedResponse(result);
        }


        /// <summary>
        /// Download Receipt
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/download")]
        public async Task<IActionResult> DownloadFile(Guid id)
        {
            var commnad = new DonwloadExpenseReceiptCommand
            {
                Id = id,
            };
            var path = await _mediator.Send(commnad);

            if (string.IsNullOrWhiteSpace(path) || !System.IO.File.Exists(path))
                return NotFound("File not found.");

            byte[] newBytes;
            await using (var stream = new FileStream(path, FileMode.Open))
            {
                byte[] bytes = new byte[stream.Length];
                int numBytesToRead = (int)stream.Length;
                int numBytesRead = 0;
                while (numBytesToRead > 0)
                {
                    // Read may return anything from 0 to numBytesToRead.
                    int n = stream.Read(bytes, numBytesRead, numBytesToRead);

                    // Break when the end of the file is reached.
                    if (n == 0)
                        break;

                    numBytesRead += n;
                    numBytesToRead -= n;
                }
                newBytes = bytes;
            }
            return File(newBytes, GetContentType(path), path);
        }

        private string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(path, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }

        /// <summary>
        /// Add  Travel Document
        /// </summary>
        /// <param name="addTravelDocumentCommand"></param>
        /// <returns></returns>
        [HttpPost("AddTravelDocument")]
        //[ClaimCheck("EXP_ADD_EXPENSE")]
        public async Task<IActionResult> AddTravelDocument([FromBody] AddTravelDocumentCommand addTravelDocumentCommand)
        {
            var result = await _mediator.Send(addTravelDocumentCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Gets Travel Document
        /// </summary>
        /// <param name="userid">The identifier.</param>
        /// <returns></returns>
        [HttpGet("GetTravelDocument/{userid}")]
        //[ClaimCheck("EXP_VIEW_EXPENSES")]
        public async Task<IActionResult> GetTravelDocument(Guid? userid)
        {
            var query = new GetTravelDocumentQuery {UserId= userid };
            var result = await _mediator.Send(query);
            //return ReturnFormattedResponse(result);
            return Ok(result);
        }

        /// <summary>
        /// Download Travel Document
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/downloadTravelDocument")]
        public async Task<IActionResult> DownloadTravelDocumentFile(Guid id)
        {
            var commnad = new DonwloadTravelDocumentCommand
            {
                Id = id,
            };
            var path = await _mediator.Send(commnad);

            if (string.IsNullOrWhiteSpace(path) || !System.IO.File.Exists(path))
                return NotFound("File not found.");

            byte[] newBytes;
            await using (var stream = new FileStream(path, FileMode.Open))
            {
                byte[] bytes = new byte[stream.Length];
                int numBytesToRead = (int)stream.Length;
                int numBytesRead = 0;
                while (numBytesToRead > 0)
                {
                    // Read may return anything from 0 to numBytesToRead.
                    int n = stream.Read(bytes, numBytesRead, numBytesToRead);

                    // Break when the end of the file is reached.
                    if (n == 0)
                        break;

                    numBytesRead += n;
                    numBytesToRead -= n;
                }
                newBytes = bytes;
            }
            return File(newBytes, GetContentType(path), path);
        }



        /// <summary>
        /// Add Wallet Expenses
        /// </summary>
        /// <param name="addWalletCommand"></param>
        /// <returns></returns>
        [HttpPost("AddWallet")]
        //[ClaimCheck("EXP_ADD_EXPENSE")]
        public async Task<IActionResult> AddWallet([FromBody] AddWalletCommand addWalletCommand)
        {
            
            var result = await _mediator.Send(addWalletCommand);            
            return ReturnFormattedResponse(result);
        }


        /// <summary>
        /// Gets Wallet 
        /// </summary>
        /// <param name="userid">The identifier.</param>
        /// <returns></returns>
        [HttpGet("GetWallet/{userid}")]
        //[ClaimCheck("EXP_VIEW_EXPENSES")]
        public async Task<IActionResult> GetWallet(Guid userid)
        {
            var query = new GetWalletQuery { UserId = userid };
            var result = await _mediator.Send(query);
            //return ReturnFormattedResponse(result);
            return Ok(result);
        }
    }
}
