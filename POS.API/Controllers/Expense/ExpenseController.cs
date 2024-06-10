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
using BTTEM.MediatR.Expense.Commands;
using BTTEM.Data.Resources;
using POS.Data.Dto;
using POS.Repository;
using BTTEM.Repository;

namespace POS.API.Controllers.Expense
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ExpenseController : BaseController
    {
        private IMediator _mediator;
        private readonly UserInfoToken _userInfoToken;
        private readonly IExpenseRepository _expenseRepository;
        private readonly IMasterExpenseRepository _masterExpenseRepository;
        private readonly IUserRepository _userRepository;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        public ExpenseController(
            IMediator mediator, UserInfoToken userInfoToken, IExpenseRepository expenseRepository,
            IMasterExpenseRepository masterExpenseRepository, IUserRepository userRepository)
        {
            _mediator = mediator;
            _userInfoToken = userInfoToken;
            _expenseRepository = expenseRepository;
            _masterExpenseRepository = masterExpenseRepository;
            _userRepository = userRepository;
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
            if (result.Success)
            {
                var userResult = _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id)).Result;
                var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                {
                    ExpenseId = result.Data.Id,
                    MasterExpenseId = result.Data.MasterExpenseId.Value,
                    ExpenseTypeName = addExpenseCommand.Name,
                    ActionType = "Activity",
                    Remarks = addExpenseCommand.Name + " Expense Added By  " + userResult.FirstName + " " + userResult.LastName,
                    Status = "Expense Added By " + userResult.FirstName + " " + userResult.LastName,
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now,
                };
                var response = await _mediator.Send(addExpenseTrackingCommand);
            }

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
                var userResult = _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id)).Result;
                var addMasterExpenseTrackingCommand = new AddExpenseTrackingCommand()
                {
                    MasterExpenseId = id,
                    ExpenseTypeName = addMasterExpenseCommand.Name,
                    ActionType = "Activity",
                    Remarks = addMasterExpenseCommand.Name + " Master Expense Added By " + userResult.FirstName + " " + userResult.LastName,
                    Status = "Master Expense Added By " + userResult.FirstName + " " + userResult.LastName,
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now,
                };

                var masterResponse = await _mediator.Send(addMasterExpenseTrackingCommand);

                foreach (var item in addMasterExpenseCommand.ExpenseDetails)
                {
                    AddExpenseCommand addExpenseCommand = new AddExpenseCommand();
                    addExpenseCommand = item;
                    addExpenseCommand.MasterExpenseId = id;
                    addExpenseCommand.TripId = result.Data.TripId;
                    var result2 = await _mediator.Send(addExpenseCommand);
                    result.Data.ExpenseId = result2.Data.Id;

                    var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                    {
                        MasterExpenseId = id,
                        ExpenseId = result.Data.ExpenseId,
                        ExpenseTypeName = addExpenseCommand.Name,
                        ActionType = "Activity",
                        Remarks = addExpenseCommand.Name + " Expense Added By " + userResult.FirstName + " " + userResult.LastName,
                        Status = "Expense Added By " + userResult.FirstName + " " + userResult.LastName,
                        ActionBy = Guid.Parse(_userInfoToken.Id),
                        ActionDate = DateTime.Now,
                    };
                    var response = await _mediator.Send(addExpenseTrackingCommand);
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
                var masterResponseData = _masterExpenseRepository.FindAsync(updateMasterExpenseCommand.Id);
                var userResult = _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id)).Result;
                var addMasterExpenseTrackingCommand = new AddExpenseTrackingCommand()
                {
                    MasterExpenseId = updateMasterExpenseCommand.Id,
                    ExpenseTypeName = masterResponseData.Result.Name,
                    ActionType = "Activity",
                    Remarks = masterResponseData.Result.Name + " Master Expense Updated By " + userResult.FirstName + " " + userResult.LastName,
                    Status = "Master Expense Updated By " + userResult.FirstName + " " + userResult.LastName,
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now,
                };
                var masterResponse = await _mediator.Send(addMasterExpenseTrackingCommand);

                // Guid id = result.Data.Id;
                foreach (var item in updateMasterExpenseCommand.ExpenseDetails)
                {
                    UpdateExpenseCommand updateExpenseCommand = new UpdateExpenseCommand();
                    if (item.Id == null || item.Id == Guid.Empty)
                    {
                        item.Id = Guid.NewGuid();
                    }
                    updateExpenseCommand = item;
                    var result2 = await _mediator.Send(updateExpenseCommand);

                    var responseData = _expenseRepository.FindAsync(item.Id);

                    var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                    {
                        MasterExpenseId = updateMasterExpenseCommand.Id,
                        ExpenseId = item.Id,
                        ExpenseTypeName = responseData.Result.Name,
                        ActionType = "Activity",
                        Remarks = responseData.Result.Name + " Expense Updated By " + userResult.FirstName + " " + userResult.LastName,
                        Status = "Expense Updated By " + userResult.FirstName + " " + userResult.LastName,
                        ActionBy = Guid.Parse(_userInfoToken.Id),
                        ActionDate = DateTime.Now,
                    };
                    var response = await _mediator.Send(addMasterExpenseTrackingCommand);
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
        //[ClaimCheck("EXP_UPDATE_EXPENSE")]
        public async Task<IActionResult> UpdateExpense(Guid id, [FromBody] UpdateExpenseCommand updateExpenseCommand)
        {
            updateExpenseCommand.Id = id;
            var result = await _mediator.Send(updateExpenseCommand);

            if (result.Success)
            {
                var responseData = _expenseRepository.FindAsync(id);
                var userResult = _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id)).Result;
                var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                {
                    MasterExpenseId = updateExpenseCommand.MasterExpenseId.Value,
                    ExpenseId = updateExpenseCommand.Id,
                    ExpenseTypeName = responseData.Result.Name,
                    ActionType = "Activity",
                    Remarks = responseData.Result.Name + " Expense Updated By " + userResult.FirstName + " " + userResult.LastName,
                    Status = "Expense Updated By " + userResult.FirstName + " " + userResult.LastName,
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now,
                };
                var response = await _mediator.Send(addExpenseTrackingCommand);
            }
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

            if (result.Success)
            {
                var responseData = _expenseRepository.FindAsync(id);
                var userResult = _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id)).Result;
                var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                {
                    ExpenseId = updateExpenseStatusCommand.Id,
                    ExpenseTypeName = responseData.Result.Name,
                    MasterExpenseId = responseData.Result.MasterExpenseId,
                    ActionType = "Tracker",
                    Remarks = updateExpenseStatusCommand.Status == "APPROVED" ? updateExpenseStatusCommand.Status : updateExpenseStatusCommand.RejectReason,//responseData.Result.Name + " Expense Status Updated",
                    Status = "Expense Status Updated By " + userResult.FirstName + " " + userResult.LastName,
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now,
                };
                var response = await _mediator.Send(addExpenseTrackingCommand);
            }
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
            if (result.Success)
            {

                var responseData = _masterExpenseRepository.FindAsync(id);
                var userResult = _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id)).Result;
                var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                {
                    MasterExpenseId = id,
                    ExpenseTypeName = responseData.Result.Name,
                    ActionType = "Tracker",
                    Remarks = responseData.Result.Name + " Master Expense Status Updated By " + userResult.FirstName + " " + userResult.LastName,
                    Status = "Master Expense Status Updated By " + userResult.FirstName + " " + userResult.LastName,
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now,
                };
                var response = await _mediator.Send(addExpenseTrackingCommand);
            }
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Get Expenses 
        /// </summary>
        /// <param name="expenseResource"></param>
        /// <returns></returns>
        [HttpGet]
        //[ClaimCheck("EXP_VIEW_EXPENSE")]
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
                totalAmount = result.TotalAmount
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
                //totalCount = result[0].Expenses.Count,
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
        //[ClaimCheck("EXP_VIEW_EXPENSES")]
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
        //[ClaimCheck("EXP_VIEW_EXPENSES")]
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

            if (result.Success)
            {
                var responseData = _expenseRepository.FindAsync(id);
                var userResult = _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id)).Result;
                var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                {
                    MasterExpenseId = responseData.Result.MasterExpenseId,
                    ExpenseId = id,
                    ExpenseTypeName = responseData.Result.Name,
                    ActionType = "Activity",
                    Remarks = responseData.Result.Name + " Expense Deleted By " + userResult.FirstName + " " + userResult.LastName,
                    Status = "Expense Deleted By " + userResult.FirstName + " " + userResult.LastName,
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now,
                };
                var response = await _mediator.Send(addExpenseTrackingCommand);
            }

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

            if (result.Success)
            {
                var responseData = _masterExpenseRepository.FindAsync(id);
                var userResult = _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id)).Result;
                var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                {
                    MasterExpenseId = responseData.Result.Id,
                    ExpenseTypeName = responseData.Result.Name,
                    ActionType = "Activity",
                    Remarks = responseData.Result.Name + " Master Expense Deleted By " + userResult.FirstName + " " + userResult.LastName,
                    Status = "Master Expense Deleted By " + userResult.FirstName + " " + userResult.LastName,
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now,
                };
                var response = await _mediator.Send(addExpenseTrackingCommand);
            }
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
            var query = new GetTravelDocumentQuery { UserId = userid };
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

        /// <summary>
        /// Update Expense And Master Expense.
        /// </summary>
        /// <param name="updateExpenseAndMasterExpenseCommand"></param>
        /// <returns></returns>
        [HttpPut("UpdateExpenseAndMasterExpense")]
        //[ClaimCheck("EXP_MSTR_EXP_UPDATE_EXPENSE")]
        public async Task<IActionResult> UpdateExpenseAndMasterExpense([FromBody] UpdateExpenseAndMasterExpenseCommand updateExpenseAndMasterExpenseCommand)
        {
            var result = await _mediator.Send(updateExpenseAndMasterExpenseCommand);

            if (result.Success)
            {
                var userResult = _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id)).Result;
                if (updateExpenseAndMasterExpenseCommand.ExpenseId.HasValue)
                {
                    var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                    {
                        ExpenseId = updateExpenseAndMasterExpenseCommand.ExpenseId.Value,
                        ActionType = "Activity",
                        Remarks = "Expense REIMBURSED (Full/Partial/Rejected) By " + userResult.FirstName + " " + userResult.LastName,
                        Status = "Expense REIMBURSED (Full/Partial/Rejected) By " + userResult.FirstName + " " + userResult.LastName,
                        ActionBy = Guid.Parse(_userInfoToken.Id),
                        ActionDate = DateTime.Now,
                    };
                    var response = await _mediator.Send(addExpenseTrackingCommand);
                }

                if (updateExpenseAndMasterExpenseCommand.MasterExpenseId.HasValue)
                {
                    var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                    {
                        MasterExpenseId = updateExpenseAndMasterExpenseCommand.MasterExpenseId.Value,
                        ActionType = "Activity",
                        Remarks = "Expense REIMBURSED (Full/Partial/Rejected) By " + userResult.FirstName + " " + userResult.LastName,
                        Status = "Expense REIMBURSED (Full/Partial/Rejected) By " + userResult.FirstName + " " + userResult.LastName,
                        ActionBy = Guid.Parse(_userInfoToken.Id),
                        ActionDate = DateTime.Now,
                    };
                    var response = await _mediator.Send(addExpenseTrackingCommand);
                }
            }
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Get All Expense Tracking
        /// </summary>
        /// <param name="expenseTrackingResource"></param>
        /// <returns></returns>

        [HttpGet("GetExpenseTrackings")]
        public async Task<IActionResult> GetExpenseTrackings([FromQuery] ExpenseTrackingResource expenseTrackingResource)
        {
            var getAllExpenseTrackingQuery = new GetAllExpenseTrackingQuery
            {
                ExpenseTrackingResource = expenseTrackingResource
            };
            var result = await _mediator.Send(getAllExpenseTrackingQuery);

            var paginationMetadata = new
            {
                totalCount = result.TotalCount,
                pageSize = result.PageSize,
                skip = result.Skip,
                totalPages = result.TotalPages
            };
            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));
            return Ok(result);
        }


        /// <summary>
        /// Get All Travel Docs
        /// </summary>
        /// <param name="travelDocumentResource"></param>
        /// <returns></returns>

        [HttpPost("GetTravelDocuments")]
        public async Task<IActionResult> GetTravelDocuments([FromBody] TravelDocumentResource travelDocumentResource)
        {
            var getAllTravelDocumentQuery = new GetAllTravelDocumentQuery
            {
                TravelDocumentResource = travelDocumentResource
            };
            var result = await _mediator.Send(getAllTravelDocumentQuery);

            var paginationMetadata = new
            {
                totalCount = result.TotalCount,
                pageSize = result.PageSize,
                skip = result.Skip,
                totalPages = result.TotalPages
            };
            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));
            return Ok(result);
        }


        /// <summary>
        /// Add Travel Desk Expenses
        /// </summary>
        /// <param name="addTravelDeskExpenseCommand"></param>
        /// <returns></returns>
        [HttpPost("AddTravelDeskExpense")]
        //[ClaimCheck("EXP_ADD_EXPENSE")]
        public async Task<IActionResult> AddTravelDeskExpense([FromBody] AddTravelDeskExpenseCommand addTravelDeskExpenseCommand)
        {
            var result = await _mediator.Send(addTravelDeskExpenseCommand);
            if (result.Success)
            {
                var userResult = _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id)).Result;
                var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                {
                    ExpenseId = result.Data.Id,
                    //MasterExpenseId = result.Data.MasterExpenseId.Value,
                    ExpenseTypeName = addTravelDeskExpenseCommand.Name,
                    ActionType = "Activity",
                    Remarks = addTravelDeskExpenseCommand.Name + " Booking File Uploaded by Travel Desk - " + userResult.FirstName + " " + userResult.LastName,
                    Status = "Booking File Uploaded by Travel Desk " + userResult.FirstName + " " + userResult.LastName,
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now,
                };
                var response = await _mediator.Send(addExpenseTrackingCommand);
            }

            return ReturnFormattedResponse(result);
        }


        /// <summary>
        /// Update Travel Desk.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateTravelDeskExpenseCommand"></param>
        /// <returns></returns>
        [HttpPut("UpdateTravelDeskExpense/{id}")]
        //[ClaimCheck("EXP_UPDATE_EXPENSE")]
        public async Task<IActionResult> UpdateTravelDeskExpense(Guid id, [FromBody] UpdateTravelDeskExpenseCommand updateTravelDeskExpenseCommand)
        {
            updateTravelDeskExpenseCommand.Id = id;
            var result = await _mediator.Send(updateTravelDeskExpenseCommand);

            if (result.Success)
            {
                var userResult = _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id)).Result;
                var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                {
                    ExpenseId = updateTravelDeskExpenseCommand.Id,
                    ExpenseTypeName = updateTravelDeskExpenseCommand.Name,
                    ActionType = "Activity",
                    Remarks = updateTravelDeskExpenseCommand.Name + " Booking File Re-Uploaded by Travel Desk - " + userResult.FirstName + " " + userResult.LastName,
                    Status = "Booking File Re-Uploaded by Travel Desk - " + userResult.FirstName + " " + userResult.LastName,
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now,
                };
                var response = await _mediator.Send(addExpenseTrackingCommand);
            }
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Download Travel Desk Document
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/downloadTravelDeskFile")]
        public async Task<IActionResult> DownloadTravelDeskFile(Guid id)
        {
            var commnad = new DonwloadTravelDeskCommand
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
        /// Gets All Travel Desk 
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet("GetTravelDeskExpense/{id}")]
        //[ClaimCheck("EXP_VIEW_EXPENSES")]
        public async Task<IActionResult> GetTravelDeskExpense(Guid id)
        {
            var query = new GetAllTravelDeskQuery { Id = id };
            var result = await _mediator.Send(query);
            //return ReturnFormattedResponse(result);
            return Ok(result);
        }
    }
}
