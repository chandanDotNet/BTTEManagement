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
using BTTEM.Data.Dto;
using System.Linq;
using BTTEM.MediatR.PoliciesTravel.Commands;
using POS.Data;
using BTTEM.Data;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using BTTEM.Data.Entities;
using BTTEM.MediatR.TravelDocument.Commands;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Hosting;
using System.IO.Compression;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json.Nodes;
using POS.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections;
using BTTEM.MediatR.ApprovalLevel.Command;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using BTTEM.MediatR.Trip.Commands;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Text;
using BTTEM.API.Models;
using BTTEM.API.Service;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace POS.API.Controllers.Expense
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ExpenseController'
    public class ExpenseController : BaseController
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ExpenseController'
    {
        private IMediator _mediator;
        private readonly UserInfoToken _userInfoToken;
        private readonly ITripItineraryRepository _tripItineraryRepository;
        private readonly IExpenseRepository _expenseRepository;
        private readonly IExpenseDocumentRepository _expenseDocumentRepository;
        private readonly ILocalConveyanceExpenseDocumentRepository _localConveyanceExpenseDocumentRepository;
        private readonly ILocalConveyanceExpenseRepository _localConveyanceExpenseRepository;
        private readonly ICarBikeLogBookExpenseRepository _carBikeLogBookExpenseRepository;
        private readonly ICarBikeLogBookExpenseDocumentRepository _carBikeLogBookExpenseDocumentRepository;
        private readonly IMasterExpenseRepository _masterExpenseRepository;
        private readonly IUserRepository _userRepository;
        private readonly IExpenseCategoryRepository _expenseCategoryRepository;
        private readonly ITripRepository _tripRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSMTPSettingRepository _emailSMTPSettingRepository;
        private IConfiguration _configuration;
        private readonly INotificationService _notificationService;
        private readonly ICompanyAccountRepository _companyAccountRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        public ExpenseController(
            IMediator mediator, UserInfoToken userInfoToken, IExpenseRepository expenseRepository,
            IMasterExpenseRepository masterExpenseRepository, IUserRepository userRepository, IExpenseCategoryRepository expenseCategoryRepository, ITripRepository tripRepository,
            IWebHostEnvironment webHostEnvironment,
            IEmailSMTPSettingRepository emailSMTPSettingRepository,
            IConfiguration configuration, ILocalConveyanceExpenseDocumentRepository localConveyanceExpenseDocumentRepository,
            ILocalConveyanceExpenseRepository localConveyanceExpenseRepository, ICarBikeLogBookExpenseRepository carBikeLogBookExpenseRepository,
            ICarBikeLogBookExpenseDocumentRepository carBikeLogBookExpenseDocumentRepository, IExpenseDocumentRepository expenseDocumentRepository, ITripItineraryRepository tripItineraryRepository, INotificationService notificationService, ICompanyAccountRepository companyAccountRepository)
        {
            _mediator = mediator;
            _userInfoToken = userInfoToken;
            _expenseRepository = expenseRepository;
            _masterExpenseRepository = masterExpenseRepository;
            _userRepository = userRepository;
            _expenseCategoryRepository = expenseCategoryRepository;
            _tripRepository = tripRepository;
            _webHostEnvironment = webHostEnvironment;
            _emailSMTPSettingRepository = emailSMTPSettingRepository;
            _configuration = configuration;
            _localConveyanceExpenseDocumentRepository = localConveyanceExpenseDocumentRepository;
            _localConveyanceExpenseRepository = localConveyanceExpenseRepository;
            _carBikeLogBookExpenseRepository = carBikeLogBookExpenseRepository;
            _carBikeLogBookExpenseDocumentRepository = carBikeLogBookExpenseDocumentRepository;
            _expenseDocumentRepository = expenseDocumentRepository;
            _tripItineraryRepository = tripItineraryRepository;
            _notificationService = notificationService;
            _companyAccountRepository = companyAccountRepository;
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
                var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                var masterExpense = await _masterExpenseRepository.FindAsync(result.Data.MasterExpenseId.Value);

                var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                {
                    ExpenseId = result.Data.Id,
                    MasterExpenseId = result.Data.MasterExpenseId.Value,
                    ExpenseTypeName = masterExpense.ExpenseType,
                    ActionType = "Activity",
                    Remarks = "Expense No. " + masterExpense.ExpenseNo + " created.",
                    Status = "Added",
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now,
                };
                var response = await _mediator.Send(addExpenseTrackingCommand);
            }

            return ReturnFormattedResponse(result);
        }


        /// <summary>
        /// Add Local Con Expenses
        /// </summary>
        /// <param name="addLocalConveyanceExpenseCommandList"></param>
        /// <returns></returns>
        [HttpPost("AddLocalConveyanceExpense")]
        //[ClaimCheck("EXP_ADD_EXPENSE")]
        public async Task<IActionResult> AddLocalConveyanceExpense(List<AddLocalConveyanceExpenseCommand> addLocalConveyanceExpenseCommandList)
        {
            BTTEM.Data.Entities.ResponseData responseData = new BTTEM.Data.Entities.ResponseData();
            foreach (var item in addLocalConveyanceExpenseCommandList)
            {
                AddLocalConveyanceExpenseCommand addLocalConveyanceExpenseCommand = new AddLocalConveyanceExpenseCommand();
                addLocalConveyanceExpenseCommand = item;
                var result = await _mediator.Send(addLocalConveyanceExpenseCommand);
                if (result.Success)
                {
                    responseData.status = true;
                    responseData.StatusCode = 200;
                }
            }

            if (responseData.status == true)
            {
                var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                var masterExpense = await _masterExpenseRepository.FindAsync(addLocalConveyanceExpenseCommandList.FirstOrDefault().MasterExpenseId);

                var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                {
                    ExpenseId = Guid.Empty,
                    MasterExpenseId = masterExpense.Id,
                    ExpenseTypeName = masterExpense.ExpenseType,
                    ActionType = "Activity",
                    Remarks = "Local conveyance expense added - Expense No. " + masterExpense.ExpenseNo,
                    Status = "Added",
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now,
                };
                var response = await _mediator.Send(addExpenseTrackingCommand);
            }

            responseData.message = "Data Updated Successfully";

            return Ok(responseData);

        }

        /// <summary>
        /// Add Local Con Expenses for App
        /// </summary>
        /// <param name="addLocalConveyanceExpenseForAppCommand"></param>
        /// <returns></returns>
        [HttpPost("AddLocalConveyanceExpenseForApp")]
        //[ClaimCheck("EXP_ADD_EXPENSE")]
        public async Task<IActionResult> AddLocalConveyanceExpenseForApp(AddLocalConveyanceExpenseForAppCommand addLocalConveyanceExpenseForAppCommand)
        {
            BTTEM.Data.Entities.ResponseData responseData = new BTTEM.Data.Entities.ResponseData();
            foreach (var item in addLocalConveyanceExpenseForAppCommand.addLocalConveyanceExpenseData)
            {
                AddLocalConveyanceExpenseCommand addLocalConveyanceExpenseCommand = new AddLocalConveyanceExpenseCommand();
                addLocalConveyanceExpenseCommand = item;
                var result = await _mediator.Send(addLocalConveyanceExpenseCommand);
                if (result.Success)
                {
                    responseData.status = true;
                    responseData.StatusCode = 200;
                }
            }


            if (responseData.status == true)
            {
                var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                var masterExpense = await _masterExpenseRepository.FindAsync(addLocalConveyanceExpenseForAppCommand.addLocalConveyanceExpenseData.FirstOrDefault().MasterExpenseId);

                var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                {
                    ExpenseId = Guid.Empty,
                    MasterExpenseId = masterExpense.Id,
                    ExpenseTypeName = masterExpense.ExpenseType,
                    ActionType = "Activity",
                    Remarks = "Local conveyance expense added - Expense No. " + masterExpense.ExpenseNo,
                    Status = "Added",
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now,
                };
                var response = await _mediator.Send(addExpenseTrackingCommand);
            }

            responseData.message = "Data Updated Successfully";

            return Ok(responseData);

        }

        /// <summary>
        /// Add Local Conveyance Expense Document
        /// </summary>
        /// <param name="addLocalConveyanceExpenseDocumentCommand"></param>
        /// <returns></returns>
        [HttpPost("AddLocalConveyanceExpenseDocument")]
        //[ClaimCheck("EXP_ADD_EXPENSE")]
        public async Task<IActionResult> AddLocalConveyanceExpenseDocument(AddLocalConveyanceExpenseDocumentCommand addLocalConveyanceExpenseDocumentCommand)
        {
            var result = await _mediator.Send(addLocalConveyanceExpenseDocumentCommand);

            if (result.Success)
            {

                var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                var localConveyanceExpenseDocs = await _localConveyanceExpenseDocumentRepository.FindAsync(addLocalConveyanceExpenseDocumentCommand.LocalConveyanceExpenseId);
                var localConveyanceExpense = await _localConveyanceExpenseRepository.FindAsync(localConveyanceExpenseDocs.LocalConveyanceExpenseId);
                var masterExpense = await _masterExpenseRepository.FindAsync(localConveyanceExpense.MasterExpenseId);

                var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                {
                    ExpenseId = Guid.Empty,
                    MasterExpenseId = masterExpense.Id,
                    ExpenseTypeName = masterExpense.ExpenseType,
                    ActionType = "Activity",
                    Remarks = "Expense document attached - Expense No. " + masterExpense.ExpenseNo,
                    Status = "Added",
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now,
                };
                var response = await _mediator.Send(addExpenseTrackingCommand);
            }

            return ReturnFormattedResponse(result);
        }


        /// <summary>
        /// Update Local Conveyance Expenses
        /// </summary>
        /// <param name="updateLocalConveyanceExpenseCommandList"></param>
        /// <returns></returns>
        [HttpPut("UpdateLocalConveyanceExpense")]
        //[ClaimCheck("EXP_ADD_EXPENSE")]
        public async Task<IActionResult> UpdateLocalConveyanceExpense(List<UpdateLocalConveyanceExpenseCommand> updateLocalConveyanceExpenseCommandList)
        {
            BTTEM.Data.Entities.ResponseData responseData = new BTTEM.Data.Entities.ResponseData();
            foreach (var item in updateLocalConveyanceExpenseCommandList)
            {
                UpdateLocalConveyanceExpenseCommand updateLocalConveyanceExpenseCommand = new UpdateLocalConveyanceExpenseCommand();
                updateLocalConveyanceExpenseCommand = item;
                var result = await _mediator.Send(updateLocalConveyanceExpenseCommand);
                if (result.Success)
                {
                    responseData.status = true;
                    responseData.StatusCode = 200;

                    var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                    var masterExpense = await _masterExpenseRepository.FindAsync(item.MasterExpenseId);

                    var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                    {
                        ExpenseId = Guid.Empty,
                        MasterExpenseId = masterExpense.Id,
                        ExpenseTypeName = masterExpense.ExpenseType,
                        ActionType = "Activity",
                        Remarks = "Local conveyance expense modified - Expense No. " + masterExpense.ExpenseNo,
                        Status = "Updated",
                        ActionBy = Guid.Parse(_userInfoToken.Id),
                        ActionDate = DateTime.Now,
                    };
                    var response = await _mediator.Send(addExpenseTrackingCommand);
                }
            }

            // var result = await _mediator.Send(updateLocalConveyanceExpenseCommand);

            responseData.message = "Data Updated Successfully";

            return Ok(responseData);

        }


        /// <summary>
        /// Update Expense Approval Level
        /// </summary>
        /// <param name="updateExpenseApprovalLevelCommand"></param>
        /// <returns></returns>
        [HttpPut("UpdateExpenseApprovalLevel")]
        //[ClaimCheck("EXP_ADD_EXPENSE")]
        public async Task<IActionResult> UpdateExpenseApprovalLevel(UpdateExpenseApprovalLevelCommand updateExpenseApprovalLevelCommand)
        {
            BTTEM.Data.Entities.ResponseData responseData = new BTTEM.Data.Entities.ResponseData();
            var result = await _mediator.Send(updateExpenseApprovalLevelCommand);
            if (result.Success)
            {
                responseData.status = result.Success;
                responseData.StatusCode = result.StatusCode;
                responseData.message = "Data Updated Successfully";

                var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                var masterExpense = await _masterExpenseRepository.FindAsync(updateExpenseApprovalLevelCommand.MasterExpenseId.Value);

                var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                {
                    ExpenseId = Guid.Empty,
                    MasterExpenseId = masterExpense.Id,
                    ExpenseTypeName = masterExpense.ExpenseType,
                    ActionType = "Activity",
                    Remarks = "Expense approval updated - Expense No. " + masterExpense.ExpenseNo,
                    Status = "Updated",
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now,
                };
                var response = await _mediator.Send(addExpenseTrackingCommand);

            }
            return Ok(responseData);

        }

        // <summary>

        /// Update Local Conveyance Expenses
        /// </summary>
        /// <param name="updateMasterExpenseCommand"></param>
        /// <returns></returns>
        [HttpPut("UpdateLocalConveyanceExpenseForApp")]
        //[ClaimCheck("EXP_ADD_EXPENSE")]
        public async Task<IActionResult> UpdateLocalConveyanceExpenseForApp(UpdateLocalConveyanceExpenseForAppCommand updateLocalConveyanceExpenseForAppCommand)
        {
            BTTEM.Data.Entities.ResponseData responseData = new BTTEM.Data.Entities.ResponseData();
            foreach (var item in updateLocalConveyanceExpenseForAppCommand.updateLocalConveyanceExpenseData)
            {
                UpdateLocalConveyanceExpenseCommand updateLocalConveyanceExpenseCommand = new UpdateLocalConveyanceExpenseCommand();
                updateLocalConveyanceExpenseCommand = item;
                var result = await _mediator.Send(updateLocalConveyanceExpenseCommand);
                if (result.Success)
                {
                    responseData.status = true;
                    responseData.StatusCode = 200;

                    var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                    var masterExpense = await _masterExpenseRepository.FindAsync(item.MasterExpenseId);

                    var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                    {
                        ExpenseId = Guid.Empty,
                        MasterExpenseId = masterExpense.Id,
                        ExpenseTypeName = masterExpense.ExpenseType,
                        ActionType = "Activity",
                        Remarks = "Local conveyance expense modified - Expense No. " + masterExpense.ExpenseNo,
                        Status = "Updated",
                        ActionBy = Guid.Parse(_userInfoToken.Id),
                        ActionDate = DateTime.Now,
                    };
                    var response = await _mediator.Send(addExpenseTrackingCommand);
                }
            }

            // var result = await _mediator.Send(updateLocalConveyanceExpenseCommand);

            responseData.message = "Data Updated Successfully";

            return Ok(responseData);

        }

        /// <summary>
        /// Deletes Local Conveyance Expense 
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete("DeleteLocalConveyanceExpense/{id}")]
        //[ClaimCheck("EXP_DELETE_EXPENSE")]
        public async Task<IActionResult> DeleteLocalConveyanceExpense(Guid id)
        {
            var command = new DeleteLocalConveyanceExpenseCommand() { Id = id };
            var result = await _mediator.Send(command);

            if (result.Success)
            {

                var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                var localConveyanceExpense = await _localConveyanceExpenseRepository.FindAsync(id);
                var masterExpense = await _masterExpenseRepository.FindAsync(localConveyanceExpense.MasterExpenseId);

                var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                {
                    ExpenseId = Guid.Empty,
                    MasterExpenseId = masterExpense.Id,
                    ExpenseTypeName = masterExpense.ExpenseType,
                    ActionType = "Activity",
                    Remarks = "Local conveyance expense deleted - Expense No. " + masterExpense.ExpenseNo,
                    Status = "Deleted",
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now,
                };
                var response = await _mediator.Send(addExpenseTrackingCommand);
            }

            return ReturnFormattedResponse(result);
        }


        /// <summary>
        /// Deletes Local Conveyance Expense Document 
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete("DeleteLocalConveyanceExpenseDocument/{id}")]
        //[ClaimCheck("EXP_DELETE_EXPENSE")]
        public async Task<IActionResult> DeleteLocalConveyanceExpenseDocument(Guid id)
        {
            var command = new DeleteLocalConveyanceExpenseDocumentCommand() { Id = id };
            var result = await _mediator.Send(command);

            if (result.Success)
            {
                var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                var localConveyanceExpenseDocs = await _localConveyanceExpenseDocumentRepository.FindAsync(id);
                var localConveyanceExpense = await _localConveyanceExpenseRepository.FindAsync(localConveyanceExpenseDocs.LocalConveyanceExpenseId);
                var masterExpense = await _masterExpenseRepository.FindAsync(localConveyanceExpense.MasterExpenseId);

                var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                {
                    ExpenseId = Guid.Empty,
                    MasterExpenseId = masterExpense.Id,
                    ExpenseTypeName = masterExpense.ExpenseType,
                    ActionType = "Activity",
                    Remarks = "Local conveyance expense documents deleted - Expense No. " + masterExpense.ExpenseNo,
                    Status = "Deleted",
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now,
                };
                var response = await _mediator.Send(addExpenseTrackingCommand);
            }

            return ReturnFormattedResponse(result);
        }



        /// <summary>
        /// Get All Local Conveyance Expenses 
        /// </summary>
        /// <param name="localConveyanceExpenseResource"></param>
        /// <returns></returns>
        [HttpGet("GetExpensesLocalConveyance")]
        public async Task<IActionResult> GetExpensesLocalConveyance([FromQuery] LocalConveyanceExpenseResource localConveyanceExpenseResource)
        {
            var getAllExpenseQuery = new GetAllLocalConveyanceExpenseQuery
            {
                ExpenseResource = localConveyanceExpenseResource
            };

            var result = await _mediator.Send(getAllExpenseQuery);

            var paginationMetadata = new
            {
                totalCount = result.TotalCount,
                pageSize = result.PageSize,
                skip = result.Skip,
                totalPages = result.TotalPages,

            };
            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));
            return Ok(result);
        }


        /// <summary>
        /// Add Car Bike LogBook Expense 
        /// </summary>
        /// <param name="addCarBikeLogBookExpenseCommandList"></param>
        /// <returns></returns>
        [HttpPost("AddCarBikeLogBookExpense")]
        //[ClaimCheck("EXP_ADD_EXPENSE")]
        public async Task<IActionResult> AddCarBikeLogBookExpense(List<AddCarBikeLogBookExpenseCommand> addCarBikeLogBookExpenseCommandList)
        {
            BTTEM.Data.Entities.ResponseData responseData = new BTTEM.Data.Entities.ResponseData();
            foreach (var item in addCarBikeLogBookExpenseCommandList)
            {
                AddCarBikeLogBookExpenseCommand addLocalConveyanceExpenseCommand = new AddCarBikeLogBookExpenseCommand();
                addLocalConveyanceExpenseCommand = item;
                var result = await _mediator.Send(addLocalConveyanceExpenseCommand);
                if (result.Success)
                {
                    responseData.status = true;
                    responseData.StatusCode = 200;

                    var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                    var masterExpense = await _masterExpenseRepository.FindAsync(addCarBikeLogBookExpenseCommandList.FirstOrDefault().MasterExpenseId);

                    var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                    {
                        ExpenseId = Guid.Empty,
                        MasterExpenseId = masterExpense.Id,
                        ExpenseTypeName = masterExpense.ExpenseType,
                        ActionType = "Activity",
                        Remarks = "Car Bike log book expense added - Expense No. " + masterExpense.ExpenseNo,
                        Status = "Added",
                        ActionBy = Guid.Parse(_userInfoToken.Id),
                        ActionDate = DateTime.Now,
                    };
                    var response = await _mediator.Send(addExpenseTrackingCommand);

                }
            }
            responseData.message = "Data Added Successfully";

            return Ok(responseData);
        }


        /// <summary>
        /// Add Car Bike LogBook Expense For App 
        /// </summary>
        /// <param name="addCarBikeLogBookExpenseCommandForApp"></param>
        /// <returns></returns>
        [HttpPost("AddCarBikeLogBookExpenseForApp")]
        //[ClaimCheck("EXP_ADD_EXPENSE")]
        public async Task<IActionResult> AddCarBikeLogBookExpenseForApp(AddCarBikeLogBookExpenseCommandForApp addCarBikeLogBookExpenseCommandForApp)
        {
            BTTEM.Data.Entities.ResponseData responseData = new BTTEM.Data.Entities.ResponseData();
            foreach (var item in addCarBikeLogBookExpenseCommandForApp.addCarBikeLogBookExpenseCommandList)
            {
                AddCarBikeLogBookExpenseCommand addLocalConveyanceExpenseCommand = new AddCarBikeLogBookExpenseCommand();
                addLocalConveyanceExpenseCommand = item;
                var result = await _mediator.Send(addLocalConveyanceExpenseCommand);
                if (result.Success)
                {
                    responseData.status = true;
                    responseData.StatusCode = 200;

                    var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                    var masterExpense = await _masterExpenseRepository.FindAsync(addCarBikeLogBookExpenseCommandForApp.addCarBikeLogBookExpenseCommandList.FirstOrDefault().MasterExpenseId);

                    var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                    {
                        ExpenseId = Guid.Empty,
                        MasterExpenseId = masterExpense.Id,
                        ExpenseTypeName = masterExpense.ExpenseType,
                        ActionType = "Activity",
                        Remarks = "Car Bike log book expense added - Expense No. " + masterExpense.ExpenseNo,
                        Status = "Added",
                        ActionBy = Guid.Parse(_userInfoToken.Id),
                        ActionDate = DateTime.Now,
                    };
                    var response = await _mediator.Send(addExpenseTrackingCommand);
                }
            }
            responseData.message = "Data Added Successfully";

            return Ok(responseData);
        }

        /// <summary>
        /// Add Car Bike LogBook Expense Document
        /// </summary>
        /// <param name="addCarBikeLogBookExpenseDocumentCommand"></param>
        /// <returns></returns>
        [HttpPost("AddCarBikeLogBookExpenseDocument")]
        //[ClaimCheck("EXP_ADD_EXPENSE")]
        public async Task<IActionResult> AddCarBikeLogBookExpenseDocument(AddCarBikeLogBookExpenseDocumentCommand addCarBikeLogBookExpenseDocumentCommand)
        {

            var result = await _mediator.Send(addCarBikeLogBookExpenseDocumentCommand);

            if (result.Success)
            {
                var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                var carBikeLogBookExpense = await _carBikeLogBookExpenseRepository.FindAsync(addCarBikeLogBookExpenseDocumentCommand.CarBikeLogBookExpenseId);
                var masterExpense = await _masterExpenseRepository.FindAsync(carBikeLogBookExpense.MasterExpenseId);

                var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                {
                    ExpenseId = Guid.Empty,
                    MasterExpenseId = masterExpense.Id,
                    ExpenseTypeName = masterExpense.ExpenseType,
                    ActionType = "Activity",
                    Remarks = "Car Bike log book expense document added - Expense No. " + masterExpense.ExpenseNo,
                    Status = "Added",
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now,
                };
                var response = await _mediator.Send(addExpenseTrackingCommand);
            }

            return ReturnFormattedResponse(result);

        }

        // <summary>

        /// Update CarBike LogBook Expense
        /// </summary>
        /// <param name="updateMasterExpenseCommand"></param>
        /// <returns></returns>
        [HttpPut("UpdateCarBikeLogBookExpense")]
        //[ClaimCheck("EXP_ADD_EXPENSE")]
        public async Task<IActionResult> UpdateCarBikeLogBookExpense(List<UpdateCarBikeLogBookExpenseCommand> updateCarBikeLogBookExpenseCommandList)
        {
            BTTEM.Data.Entities.ResponseData responseData = new BTTEM.Data.Entities.ResponseData();
            foreach (var item in updateCarBikeLogBookExpenseCommandList)
            {
                UpdateCarBikeLogBookExpenseCommand updateCarBikeLogBookExpenseCommand = new UpdateCarBikeLogBookExpenseCommand();
                updateCarBikeLogBookExpenseCommand = item;
                var result = await _mediator.Send(updateCarBikeLogBookExpenseCommand);
                if (result.Success)
                {
                    responseData.status = true;
                    responseData.StatusCode = 200;

                    var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                    var masterExpense = await _masterExpenseRepository.FindAsync(updateCarBikeLogBookExpenseCommandList.FirstOrDefault().MasterExpenseId);

                    var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                    {
                        ExpenseId = Guid.Empty,
                        MasterExpenseId = masterExpense.Id,
                        ExpenseTypeName = masterExpense.ExpenseType,
                        ActionType = "Activity",
                        Remarks = "Car Bike log book expense document modified - Expense No. " + masterExpense.ExpenseNo,
                        Status = "Updated",
                        ActionBy = Guid.Parse(_userInfoToken.Id),
                        ActionDate = DateTime.Now,
                    };
                    var response = await _mediator.Send(addExpenseTrackingCommand);
                }
            }
            //var result = await _mediator.Send(updateCarBikeLogBookExpenseCommand);
            responseData.message = "Data Updated Successfully";

            return Ok(responseData);
        }


        /// <summary>
        /// Update CarBike LogBook Expense For App
        /// </summary>
        /// <param name="updateCarBikeLogBookExpenseCommandForApp"></param>
        /// <returns></returns>
        [HttpPut("UpdateCarBikeLogBookExpenseForApp")]
        //[ClaimCheck("EXP_ADD_EXPENSE")]
        public async Task<IActionResult> UpdateCarBikeLogBookExpenseForApp(UpdateCarBikeLogBookExpenseCommandForApp updateCarBikeLogBookExpenseCommandForApp)
        {
            BTTEM.Data.Entities.ResponseData responseData = new BTTEM.Data.Entities.ResponseData();
            foreach (var item in updateCarBikeLogBookExpenseCommandForApp.updateCarBikeLogBookExpenseCommandList)
            {
                UpdateCarBikeLogBookExpenseCommand updateCarBikeLogBookExpenseCommand = new UpdateCarBikeLogBookExpenseCommand();
                updateCarBikeLogBookExpenseCommand = item;
                var result = await _mediator.Send(updateCarBikeLogBookExpenseCommand);
                if (result.Success)
                {
                    responseData.status = true;
                    responseData.StatusCode = 200;

                    var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                    var masterExpense = await _masterExpenseRepository.FindAsync(updateCarBikeLogBookExpenseCommandForApp.updateCarBikeLogBookExpenseCommandList.FirstOrDefault().MasterExpenseId);

                    var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                    {
                        ExpenseId = Guid.Empty,
                        MasterExpenseId = masterExpense.Id,
                        ExpenseTypeName = masterExpense.ExpenseType,
                        ActionType = "Activity",
                        Remarks = "Car Bike log book expense modified - Expense No. " + masterExpense.ExpenseNo,
                        Status = "Updated",
                        ActionBy = Guid.Parse(_userInfoToken.Id),
                        ActionDate = DateTime.Now,
                    };
                    var response = await _mediator.Send(addExpenseTrackingCommand);
                }

            }
            //var result = await _mediator.Send(updateCarBikeLogBookExpenseCommand);
            responseData.message = "Data Updated Successfully";

            return Ok(responseData);

        }

        /// <summary>
        /// Deletes Car Bike LogBook Expense 
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete("DeleteCarBikeLogBookExpense/{id}")]
        //[ClaimCheck("EXP_DELETE_EXPENSE")]
        public async Task<IActionResult> DeleteCarBikeLogBookExpense(Guid id)
        {
            var command = new DeleteCarBikeLogBookExpenseCommand() { Id = id };
            var result = await _mediator.Send(command);

            if (result.Success)
            {
                var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                var carBikeLogBookExpense = await _carBikeLogBookExpenseRepository.FindAsync(id);
                var masterExpense = await _masterExpenseRepository.FindAsync(carBikeLogBookExpense.MasterExpenseId);

                var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                {
                    ExpenseId = Guid.Empty,
                    MasterExpenseId = masterExpense.Id,
                    ExpenseTypeName = masterExpense.ExpenseType,
                    ActionType = "Activity",
                    Remarks = "Car Bike log book expense deleted - Expense No. " + masterExpense.ExpenseNo,
                    Status = "Deleted",
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now,
                };
                var response = await _mediator.Send(addExpenseTrackingCommand);
            }

            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Delete Car Bike Log Book Expense Document
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete("DeleteCarBikeLogBookExpenseDocument/{id}")]
        //[ClaimCheck("EXP_DELETE_EXPENSE")]
        public async Task<IActionResult> DeleteCarBikeLogBookExpenseDocument(Guid id)
        {
            var command = new DeleteCarBikeLogBookExpenseDocumentCommand() { Id = id };
            var result = await _mediator.Send(command);

            if (result.Success)
            {
                var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                var carBikeLogBookExpenseDocs = await _carBikeLogBookExpenseDocumentRepository.FindAsync(id);
                var carBikeLogBookExpense = await _carBikeLogBookExpenseRepository.FindAsync(carBikeLogBookExpenseDocs.CarBikeLogBookExpenseId);
                var masterExpense = await _masterExpenseRepository.FindAsync(carBikeLogBookExpense.MasterExpenseId);

                var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                {
                    ExpenseId = Guid.Empty,
                    MasterExpenseId = masterExpense.Id,
                    ExpenseTypeName = masterExpense.ExpenseType,
                    ActionType = "Activity",
                    Remarks = "Car Bike log book expense document deleted - Expense No. " + masterExpense.ExpenseNo,
                    Status = "Deleted",
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now,
                };
                var response = await _mediator.Send(addExpenseTrackingCommand);
            }

            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Get All Car Bike LogBook Expense 
        /// </summary>
        /// <param name="carBikeLogBookExpenseResource"></param>
        /// <returns></returns>
        [HttpGet("GetCarBikeLogBookExpense")]
        public async Task<IActionResult> GetCarBikeLogBookExpense([FromQuery] CarBikeLogBookExpenseResource carBikeLogBookExpenseResource)
        {
            var getAllExpenseQuery = new GetAllCarBikeLogBookExpenseQuery
            {
                ExpenseResource = carBikeLogBookExpenseResource
            };

            var result = await _mediator.Send(getAllExpenseQuery);

            var paginationMetadata = new
            {
                totalCount = result.TotalCount,
                pageSize = result.PageSize,
                skip = result.Skip,
                totalPages = result.TotalPages,

            };
            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));
            return Ok(result);
        }

        /// <summary>
        /// Add Master Expenses
        /// </summary>
        /// <param name="addMasterExpenseCommand"></param>
        /// <returns></returns>
        [HttpPost("AddExpenseWithDetails")]
        //[ClaimCheck("EXP_ADD_EXPENSE")]
        public async Task<IActionResult> AddMasterExpense([FromBody] AddMasterExpenseCommand addMasterExpenseCommand)
        {
            var userDetails = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));

            GetNewExpenseNumberCommand getNewExpenseNumber = new GetNewExpenseNumberCommand();
            string ExpenseNo = await _mediator.Send(getNewExpenseNumber);
            addMasterExpenseCommand.ExpenseNo = ExpenseNo;
            addMasterExpenseCommand.NoOfBill = addMasterExpenseCommand.ExpenseDetails.Where(a => a.Amount > 0).Count();
            addMasterExpenseCommand.TotalAmount = addMasterExpenseCommand.ExpenseDetails.Sum(a => a.Amount);
            if (addMasterExpenseCommand.TripId.HasValue)
            {
                //var exitExpense = _masterExpenseRepository.All.Where(a => a.TripId == addMasterExpenseCommand.TripId).FirstOrDefault(); 
                //if(exitExpense==null) 
                //{
                //    return ReturnFormattedResponse("sss");
                //}
                var AdvanceAmount = _tripRepository.All.Where(a => a.Id == addMasterExpenseCommand.TripId && a.RequestAdvanceMoneyStatus == "APPROVED").FirstOrDefault();
                if (AdvanceAmount != null)
                {
                    addMasterExpenseCommand.AdvanceMoney = AdvanceAmount.AdvanceMoney.Value;
                }
            }

            var result = await _mediator.Send(addMasterExpenseCommand);
            if (result.Success)
            {
                Guid id = result.Data.Id;
                var userResult = _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id)).Result;
                var addMasterExpenseTrackingCommand = new AddExpenseTrackingCommand()
                {
                    MasterExpenseId = id,
                    ExpenseTypeName = result.Data.ExpenseType,
                    ActionType = "Activity",
                    Remarks = "Expense No. " + result.Data.ExpenseNo + " created.",
                    Status = "Added",
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now,
                };

                var masterResponse = await _mediator.Send(addMasterExpenseTrackingCommand);

                foreach (var item in addMasterExpenseCommand.ExpenseDetails)
                {
                    AddExpenseCommand addExpenseCommand = new AddExpenseCommand();
                    item.ExpenseDetail.ForEach(c => c.MasterExpenseId = id);
                    //if (item.ExpenseDetail.Count>0)
                    //{
                    //    item.ExpenseDetail.ForEach(c => c.MasterExpenseId = id);

                    //}
                    addExpenseCommand = item;
                    addExpenseCommand.MasterExpenseId = id;
                    addExpenseCommand.TripId = result.Data.TripId;
                    if (userDetails.IsDirector)
                    {
                        addExpenseCommand.Status = "APPROVED";
                    }
                    else
                    {
                        if (addExpenseCommand.Amount > 0)
                        {
                            addExpenseCommand.Status = "PENDING";
                        }
                        else
                        {
                            addExpenseCommand.Status = "APPROVED";
                        }
                    }

                    var result2 = await _mediator.Send(addExpenseCommand);
                    result.Data.ExpenseId = result2.Data.Id;

                    //var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                    //{
                    //    MasterExpenseId = id,
                    //    ExpenseId = result.Data.ExpenseId,
                    //    ExpenseTypeName = addExpenseCommand.Name,
                    //    ActionType = "Activity",
                    //    Remarks = addExpenseCommand.Name + " Expense Added By " + userResult.FirstName + " " + userResult.LastName,
                    //    Status = "Expense Added By " + userResult.FirstName + " " + userResult.LastName,
                    //    ActionBy = Guid.Parse(_userInfoToken.Id),
                    //    ActionDate = DateTime.Now,
                    //};
                    //var response = await _mediator.Send(addExpenseTrackingCommand);
                }

                //============================Approved Trip
                int noOfDays = 1;
                if (addMasterExpenseCommand.ExpenseType == "Local Trip")
                {
                    if (addMasterExpenseCommand.ExpenseDetails.Count > 0)
                    {
                        var ExpenseDetailsList = addMasterExpenseCommand.ExpenseDetails.GroupBy(a => a.ExpenseDate).ToList();
                        //var FirstDate = ExpenseDetailsList.First().ExpenseDate;
                        //var LastDate = ExpenseDetailsList.Last().ExpenseDate;
                        //noOfDays = (int)(LastDate - FirstDate).TotalDays+1;
                        noOfDays = ExpenseDetailsList.Count();
                    }
                    else
                    {
                        noOfDays = 1;
                    }

                }
                else
                {
                    var tripDetails = _tripRepository.FindAsync(addMasterExpenseCommand.TripId.Value);
                    noOfDays = (int)(tripDetails.Result.TripEnds - tripDetails.Result.TripStarts).TotalDays + 1;
                }
                if (noOfDays > 0)
                {

                    var expenseCategory = _expenseCategoryRepository.All.ToList();
                    if (expenseCategory.Count > 0)
                    {
                        //===============================
                        var getUserGradeAndAccountCommand = new GetUserGradeAndAccountCommand
                        {
                            UserId = Guid.Parse(_userInfoToken.Id)//result.Data.CreatedByUser.Id,
                        };
                        var resultUser = await _mediator.Send(getUserGradeAndAccountCommand);
                        PoliciesDetailResource policiesDetailResourceQuery = new PoliciesDetailResource
                        {
                            CompanyAccountId = resultUser.CompanyAccountId,
                            GradeId = resultUser.GradeId,
                        };

                        //PoliciesDetail
                        var getAllPoliciesDetailCommand = new GetAllPoliciesDetailCommand
                        {
                            PoliciesDetailResource = policiesDetailResourceQuery
                        };
                        var resultPoliciesDetail = await _mediator.Send(getAllPoliciesDetailCommand);
                        if (resultPoliciesDetail == null)
                        {
                            return NotFound("Policies not mapped with user");
                        }

                        //Policies Lodging Fooding
                        var getAllPoliciesLodgingFoodingCommand = new GetAllPoliciesLodgingFoodingCommand
                        {
                            Id = resultPoliciesDetail.FirstOrDefault().Id
                        };
                        var resultPoliciesLodgingFooding = await _mediator.Send(getAllPoliciesLodgingFoodingCommand);

                        //Conveyance
                        var getAllConveyanceCommand = new GetAllConveyanceCommand
                        {
                            Id = resultPoliciesDetail.FirstOrDefault().Id
                        };
                        var resultConveyance = await _mediator.Send(getAllConveyanceCommand);

                        //PoliciesVehicleConveyance
                        var getAlllPoliciesVehicleConveyanceCommand = new GetAllPoliciesVehicleConveyanceCommand
                        {
                            Id = resultPoliciesDetail.FirstOrDefault().Id
                        };
                        var resultlPoliciesVehicleConveyance = await _mediator.Send(getAlllPoliciesVehicleConveyanceCommand);

                        //PoliciesSetting
                        var getAllPoliciesSettingCommand = new GetAllPoliciesSettingCommand
                        {
                            Id = resultPoliciesDetail.FirstOrDefault().Id
                        };
                        var resultPoliciesSetting = await _mediator.Send(getAllPoliciesSettingCommand);
                        //===============================

                        bool IsDeviation = false;
                        UpdateExpenseStatusCommand updateExpenseStatusCommand = new UpdateExpenseStatusCommand();
                        foreach (var item in expenseCategory)
                        {
                            var expenseAmount = _expenseRepository.All.Where(a => a.MasterExpenseId == masterResponse.Data.MasterExpenseId && a.ExpenseCategoryId == item.Id).Sum(a => a.Amount);
                            var expenseList = _expenseRepository.All.Where(a => a.MasterExpenseId == masterResponse.Data.MasterExpenseId && a.ExpenseCategoryId == item.Id).ToList();

                            //--Fare
                            if (item.Id == new Guid("DCAA05B6-5F1E-402F-835E-0704A3A1A455"))
                            {
                                if (expenseList.Count > 0)
                                {
                                    foreach (var expense in expenseList)
                                    {
                                        updateExpenseStatusCommand.Id = expense.Id;
                                        if (userDetails.IsDirector)
                                        {
                                            updateExpenseStatusCommand.Status = "APPROVED";
                                        }
                                        else
                                        {
                                            updateExpenseStatusCommand.Status = resultUser.CompanyAccountId == new Guid("D0CCEA5F-5393-4A34-9DF6-43A9F51F9F91") ? "PENDING" : "APPROVED";
                                            updateExpenseStatusCommand.DeviationAmount = expense.Amount;
                                        }

                                        updateExpenseStatusCommand.PayableAmount = expense.Amount;
                                        updateExpenseStatusCommand.Allowance = "Not Specified";
                                        var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                    }
                                }
                            }

                            //--Lodging (Metro City)
                            if (item.Id == new Guid("FBF965BD-A53E-4D97-978A-34C2007202E5"))
                            {
                                if (resultPoliciesLodgingFooding.IsMetroCities == true)
                                {
                                    int localNoOfDays = 1;
                                    var ExpenseDetailsList = addMasterExpenseCommand.ExpenseDetails.Where(a => a.Amount > 0 && a.ExpenseCategoryId == new Guid("FBF965BD-A53E-4D97-978A-34C2007202E5")).GroupBy(a => a.ExpenseDate).ToList();
                                    if (ExpenseDetailsList != null)
                                    {
                                        localNoOfDays = ExpenseDetailsList.Count();
                                    }

                                    // decimal PoliciesLodgingFooding = resultPoliciesLodgingFooding.MetroCitiesUptoAmount * Convert.ToDecimal(localNoOfDays);
                                    decimal PoliciesLodgingFooding = resultPoliciesLodgingFooding.MetroCitiesUptoAmount;
                                    //if (expenseAmount > PoliciesLodgingFooding)
                                    //{
                                    //    IsDeviation = true;
                                    //}
                                    //else
                                    //{
                                    if (expenseList.Count > 0)
                                    {
                                        foreach (var expense in expenseList)
                                        {
                                            updateExpenseStatusCommand.Id = expense.Id;

                                            if (userDetails.IsDirector)
                                            {
                                                updateExpenseStatusCommand.Status = "APPROVED";
                                                updateExpenseStatusCommand.PayableAmount = expense.Amount;
                                                updateExpenseStatusCommand.DeviationAmount = 0;

                                            }
                                            if (expense.Amount <= PoliciesLodgingFooding)
                                            {
                                                updateExpenseStatusCommand.Status = resultUser.CompanyAccountId == new Guid("D0CCEA5F-5393-4A34-9DF6-43A9F51F9F91") ? "PENDING" : "APPROVED";
                                                updateExpenseStatusCommand.PayableAmount = expense.Amount;
                                                updateExpenseStatusCommand.DeviationAmount = 0;

                                                //var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                            }

                                            if (expense.Amount > PoliciesLodgingFooding)
                                            {
                                                if (!userDetails.IsDirector)
                                                {
                                                    updateExpenseStatusCommand.DeviationAmount = expense.Amount - PoliciesLodgingFooding;
                                                }
                                            }

                                            updateExpenseStatusCommand.Allowance = Convert.ToString(PoliciesLodgingFooding);

                                            var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                        }
                                    }
                                    //}
                                }
                                //dddd
                                if (userDetails.IsDirector)
                                {
                                    if (expenseList.Count > 0)
                                    {
                                        foreach (var expense in expenseList)
                                        {
                                            updateExpenseStatusCommand.Id = expense.Id;
                                            updateExpenseStatusCommand.Status = "APPROVED";
                                            updateExpenseStatusCommand.PayableAmount = expense.Amount;
                                            updateExpenseStatusCommand.DeviationAmount = 0;
                                            updateExpenseStatusCommand.Allowance = "Director(Full)";
                                            var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                        }
                                    }
                                }
                            }
                            //-- Lodging (Other City)
                            if (item.Id == new Guid("1AADD03D-90E1-4589-8B9D-6121049B490D"))
                            {
                                if (resultPoliciesLodgingFooding.OtherCities == true)
                                {
                                    int localNoOfDays = 1;
                                    var ExpenseDetailsList = addMasterExpenseCommand.ExpenseDetails.Where(a => a.Amount > 0 && a.ExpenseCategoryId == new Guid("1AADD03D-90E1-4589-8B9D-6121049B490D")).GroupBy(a => a.ExpenseDate).ToList();
                                    if (ExpenseDetailsList != null)
                                    {
                                        localNoOfDays = ExpenseDetailsList.Count();
                                    }

                                    //decimal PoliciesLodgingFooding = resultPoliciesLodgingFooding.OtherCitiesUptoAmount * Convert.ToDecimal(localNoOfDays);
                                    decimal PoliciesLodgingFooding = resultPoliciesLodgingFooding.OtherCitiesUptoAmount;
                                    //if (expenseAmount > PoliciesLodgingFooding)
                                    //{
                                    //    IsDeviation = true;
                                    //}
                                    //else
                                    //{
                                    if (expenseList.Count > 0)
                                    {
                                        foreach (var expense in expenseList)
                                        {
                                            updateExpenseStatusCommand.Id = expense.Id;
                                            if (userDetails.IsDirector)
                                            {
                                                updateExpenseStatusCommand.Status = "APPROVED";
                                                updateExpenseStatusCommand.DeviationAmount = 0;
                                                updateExpenseStatusCommand.PayableAmount = expense.Amount;
                                            }
                                            if (expense.Amount <= PoliciesLodgingFooding)
                                            {
                                                updateExpenseStatusCommand.Status = resultUser.CompanyAccountId == new Guid("D0CCEA5F-5393-4A34-9DF6-43A9F51F9F91") ? "PENDING" : "APPROVED";
                                                updateExpenseStatusCommand.PayableAmount = expense.Amount;

                                                //var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                            }
                                            if (expense.Amount > PoliciesLodgingFooding)
                                            {
                                                if (!userDetails.IsDirector)
                                                {
                                                    updateExpenseStatusCommand.DeviationAmount = expense.Amount - PoliciesLodgingFooding;
                                                }
                                            }
                                            else
                                            {
                                                updateExpenseStatusCommand.DeviationAmount = 0;
                                            }
                                            updateExpenseStatusCommand.Allowance = Convert.ToString(PoliciesLodgingFooding);
                                            var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                        }
                                    }
                                    //}
                                }

                                //dddd
                                if (userDetails.IsDirector)
                                {
                                    if (expenseList.Count > 0)
                                    {
                                        foreach (var expense in expenseList)
                                        {
                                            updateExpenseStatusCommand.Id = expense.Id;
                                            updateExpenseStatusCommand.Status = "APPROVED";
                                            updateExpenseStatusCommand.PayableAmount = expense.Amount;
                                            updateExpenseStatusCommand.DeviationAmount = 0;
                                            updateExpenseStatusCommand.Allowance = "Director(Full)";
                                            var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                        }
                                    }
                                }
                            }

                            //--Conveyance (within a City)
                            if (item.Id == new Guid("B1977DB3-D909-4936-A5DA-41BF84638963"))
                            {
                                var Conveyance = resultConveyance.Where(a => a.Name == "Conveyance (within a city)");
                                if (Conveyance != null)
                                {
                                    var ConveyancesItemAll = Conveyance.Select(a => a.conveyancesItem).Where(b => b.Any(a => a.ConveyanceItemName == "Budget")).FirstOrDefault();
                                    var ConveyancesItem = ConveyancesItemAll.Where(a => a.ConveyanceItemName == "Budget");
                                    if (ConveyancesItem != null)
                                    {
                                        bool IsCheck = (bool)ConveyancesItem.FirstOrDefault().IsCheck;
                                        if (IsCheck == true)
                                        {
                                            decimal ConveyancesAmount = 0;
                                            if (ConveyancesItem.FirstOrDefault().Amount != null)
                                            {
                                                ConveyancesAmount = (decimal)(ConveyancesItem.FirstOrDefault().Amount) * Convert.ToDecimal(noOfDays);
                                            }
                                            if (expenseAmount > ConveyancesAmount)
                                            {
                                                IsDeviation = true;

                                                if (expenseList.Count > 0)
                                                {
                                                    foreach (var expense in expenseList)
                                                    {
                                                        updateExpenseStatusCommand.Id = expense.Id;
                                                        updateExpenseStatusCommand.DeviationAmount = expenseAmount - ConveyancesAmount;
                                                        updateExpenseStatusCommand.Allowance = Convert.ToString(ConveyancesAmount);
                                                        var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (expenseList.Count > 0)
                                                {
                                                    foreach (var expense in expenseList)
                                                    {
                                                        updateExpenseStatusCommand.Id = expense.Id;
                                                        if (userDetails.IsDirector)
                                                        {
                                                            updateExpenseStatusCommand.Status = "APPROVED";
                                                            updateExpenseStatusCommand.DeviationAmount = 0;
                                                            updateExpenseStatusCommand.Allowance = "Director(Full)";
                                                        }
                                                        else
                                                        {
                                                            updateExpenseStatusCommand.Status = resultUser.CompanyAccountId == new Guid("D0CCEA5F-5393-4A34-9DF6-43A9F51F9F91") ? "PENDING" : "APPROVED";
                                                            updateExpenseStatusCommand.DeviationAmount = expense.Amount;
                                                            updateExpenseStatusCommand.Allowance = "Not Specified";
                                                        }
                                                        updateExpenseStatusCommand.PayableAmount = expense.Amount;
                                                        updateExpenseStatusCommand.Allowance = Convert.ToString(ConveyancesAmount);
                                                        var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                                    }
                                                }
                                            }
                                        }
                                        else //Actuals
                                        {
                                            if (expenseList.Count > 0)
                                            {
                                                foreach (var expense in expenseList)
                                                {
                                                    updateExpenseStatusCommand.Id = expense.Id;
                                                    //updateExpenseStatusCommand.Status = resultUser.CompanyAccountId == new Guid("D0CCEA5F-5393-4A34-9DF6-43A9F51F9F91") ? "PENDING" : "APPROVED";
                                                    if (userDetails.IsDirector)
                                                    {
                                                        updateExpenseStatusCommand.Status = "APPROVED";
                                                        updateExpenseStatusCommand.PayableAmount = expense.Amount;
                                                        updateExpenseStatusCommand.DeviationAmount = 0;
                                                        updateExpenseStatusCommand.Allowance = "Director(Full)";
                                                    }
                                                    else
                                                    {
                                                        updateExpenseStatusCommand.Status = "PENDING";
                                                        updateExpenseStatusCommand.PayableAmount = 0;
                                                        updateExpenseStatusCommand.DeviationAmount = 0;
                                                        updateExpenseStatusCommand.Allowance = "Actual";
                                                    }

                                                    var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            //--Conveyance (city to outer area)
                            if (item.Id == new Guid("5278397A-C8DD-475A-A7A7-C05708B2BB06"))
                            {
                                var Conveyance = resultConveyance.Where(a => a.Name == "Conveyance (city to outer area)");
                                if (Conveyance != null)
                                {
                                    var ConveyancesItemAll = Conveyance.Select(a => a.conveyancesItem).Where(b => b.Any(a => a.ConveyanceItemName == "Budget")).FirstOrDefault();
                                    var ConveyancesItem = ConveyancesItemAll.Where(a => a.ConveyanceItemName == "Budget");
                                    if (ConveyancesItem != null)
                                    {
                                        bool IsCheck = (bool)ConveyancesItem.FirstOrDefault().IsCheck;
                                        if (IsCheck == true)
                                        {
                                            decimal ConveyancesAmount = 0;
                                            if (ConveyancesItem.FirstOrDefault().Amount != null)
                                            {
                                                ConveyancesAmount = (decimal)(ConveyancesItem.FirstOrDefault().Amount) * Convert.ToDecimal(noOfDays);
                                            }
                                            if (expenseAmount > ConveyancesAmount)
                                            {
                                                IsDeviation = true;
                                                if (expenseList.Count > 0)
                                                {
                                                    foreach (var expense in expenseList)
                                                    {
                                                        updateExpenseStatusCommand.Id = expense.Id;
                                                        updateExpenseStatusCommand.DeviationAmount = expenseAmount - ConveyancesAmount;
                                                        updateExpenseStatusCommand.Allowance = Convert.ToString(ConveyancesAmount);
                                                        var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (expenseList.Count > 0)
                                                {
                                                    foreach (var expense in expenseList)
                                                    {
                                                        updateExpenseStatusCommand.Id = expense.Id;
                                                        if (userDetails.IsDirector)
                                                        {
                                                            updateExpenseStatusCommand.Status = "APPROVED";
                                                            updateExpenseStatusCommand.DeviationAmount = 0;
                                                            updateExpenseStatusCommand.Allowance = "Director(Full)";
                                                        }
                                                        else
                                                        {
                                                            updateExpenseStatusCommand.Status = resultUser.CompanyAccountId == new Guid("D0CCEA5F-5393-4A34-9DF6-43A9F51F9F91") ? "PENDING" : "APPROVED";
                                                            updateExpenseStatusCommand.DeviationAmount = expenseAmount - ConveyancesAmount;
                                                            updateExpenseStatusCommand.Allowance = Convert.ToString(ConveyancesAmount);
                                                        }
                                                        updateExpenseStatusCommand.PayableAmount = expense.Amount;

                                                        var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                                    }
                                                }
                                            }
                                        }
                                        else //Actuals
                                        {
                                            if (expenseList.Count > 0)
                                            {
                                                foreach (var expense in expenseList)
                                                {
                                                    updateExpenseStatusCommand.Id = expense.Id;
                                                    //updateExpenseStatusCommand.Status = resultUser.CompanyAccountId == new Guid("D0CCEA5F-5393-4A34-9DF6-43A9F51F9F91") ? "PENDING" : "APPROVED";
                                                    if (userDetails.IsDirector)
                                                    {
                                                        updateExpenseStatusCommand.Status = "APPROVED";
                                                        updateExpenseStatusCommand.PayableAmount = expense.Amount;
                                                        updateExpenseStatusCommand.DeviationAmount = 0;
                                                        updateExpenseStatusCommand.Allowance = "Director(Full)";
                                                    }
                                                    else
                                                    {
                                                        updateExpenseStatusCommand.Status = "PENDING";
                                                        updateExpenseStatusCommand.PayableAmount = 0;
                                                        updateExpenseStatusCommand.DeviationAmount = 0;
                                                        updateExpenseStatusCommand.Allowance = "Actual";
                                                    }
                                                    var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            //--MISC /DA
                            if (item.Id == new Guid("ED69E9A0-2D54-4A91-A598-F79973B9FE99"))
                            {
                                decimal DA = 0;
                                if (resultPoliciesDetail.FirstOrDefault().DailyAllowance != null)
                                {
                                    //DA = (decimal)resultPoliciesDetail.FirstOrDefault().DailyAllowance * Convert.ToDecimal(noOfDays);
                                    DA = (decimal)resultPoliciesDetail.FirstOrDefault().DailyAllowance;
                                }

                                //if (expenseAmount > DA)
                                //{
                                //    IsDeviation = true;
                                //}
                                //else
                                //{
                                if (expenseList.Count > 0)
                                {
                                    foreach (var expense in expenseList)
                                    {
                                        if (expense.Amount <= DA)
                                        {
                                            updateExpenseStatusCommand.Id = expense.Id;
                                            if (userDetails.IsDirector)
                                            {
                                                updateExpenseStatusCommand.Status = "APPROVED";
                                                updateExpenseStatusCommand.DeviationAmount = 0;
                                                updateExpenseStatusCommand.Allowance = "Director(Full)";
                                            }
                                            else
                                            {
                                                updateExpenseStatusCommand.DeviationAmount = 0;
                                                updateExpenseStatusCommand.Status = resultUser.CompanyAccountId == new Guid("D0CCEA5F-5393-4A34-9DF6-43A9F51F9F91") ? "PENDING" : "APPROVED";

                                            }
                                            updateExpenseStatusCommand.PayableAmount = expense.Amount;
                                            var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                        }
                                        else
                                        {
                                            updateExpenseStatusCommand.Id = expense.Id;
                                            if (userDetails.IsDirector)
                                            {
                                                updateExpenseStatusCommand.Status = "APPROVED";
                                                updateExpenseStatusCommand.PayableAmount = expense.Amount;
                                                updateExpenseStatusCommand.DeviationAmount = 0;
                                                updateExpenseStatusCommand.Allowance = "Director(Full)";
                                            }
                                            else
                                            {
                                                updateExpenseStatusCommand.Status = "PENDING";
                                                updateExpenseStatusCommand.PayableAmount = 0;
                                                updateExpenseStatusCommand.DeviationAmount = expense.Amount - DA;
                                                updateExpenseStatusCommand.Allowance = Convert.ToString(DA);
                                            }
                                            var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                        }
                                    }
                                }
                                //}
                            }
                            //--Fooding Allowance
                            if (item.Id == new Guid("BB0BF3AA-1FD9-4F1C-9FDE-8498073C58A9"))
                            {
                                if (resultPoliciesLodgingFooding.IsBudget == true)
                                {
                                    bool check = false;
                                    decimal PoliciesFooding = 0;
                                    int localNoOfDays = 1;
                                    var ExpenseDetailsList = addMasterExpenseCommand.ExpenseDetails.Where(a => a.Amount > 0 && a.ExpenseCategoryId == new Guid("BB0BF3AA-1FD9-4F1C-9FDE-8498073C58A9")).GroupBy(a => a.ExpenseDate).ToList();
                                    if (ExpenseDetailsList != null)
                                    {
                                        localNoOfDays = ExpenseDetailsList.Count();
                                    }

                                    if (resultPoliciesLodgingFooding.BudgetAmount != null)
                                    {
                                        PoliciesFooding = resultPoliciesLodgingFooding.BudgetAmount * Convert.ToDecimal(localNoOfDays);
                                    }
                                    if (expenseAmount > PoliciesFooding)
                                    {
                                        IsDeviation = true;
                                        if (expenseList.Count > 0)
                                        {
                                            foreach (var expense in expenseList)
                                            {
                                                updateExpenseStatusCommand.Id = expense.Id;
                                                if (check == false)
                                                {
                                                    if (expense.Amount >  resultPoliciesLodgingFooding.BudgetAmount )
                                                    {
                                                        updateExpenseStatusCommand.DeviationAmount = expenseAmount - PoliciesFooding;
                                                        check = true;
                                                    }
                                                }
                                                updateExpenseStatusCommand.Allowance = Convert.ToString(PoliciesFooding);
                                                var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                                updateExpenseStatusCommand.DeviationAmount = 0;
                                            }
                                        }

                                    }
                                    else
                                    {
                                        if (expenseList.Count > 0)
                                        {
                                            check = false;
                                            foreach (var expense in expenseList)
                                            {
                                                updateExpenseStatusCommand.Id = expense.Id;
                                                if (userDetails.IsDirector)
                                                {
                                                    updateExpenseStatusCommand.Status = "APPROVED";
                                                    updateExpenseStatusCommand.DeviationAmount = 0;
                                                    updateExpenseStatusCommand.Allowance = "Director(Full)";
                                                }
                                                else
                                                {
                                                    updateExpenseStatusCommand.Status = resultUser.CompanyAccountId == new Guid("D0CCEA5F-5393-4A34-9DF6-43A9F51F9F91") ? "PENDING" : "APPROVED";
                                                    
                                                    if (check == false)
                                                    {
                                                        if (expense.Amount > resultPoliciesLodgingFooding.BudgetAmount)
                                                        {
                                                            updateExpenseStatusCommand.DeviationAmount = expenseAmount - PoliciesFooding;
                                                            check = true;
                                                        }
                                                    }
                                                    updateExpenseStatusCommand.Allowance = Convert.ToString(PoliciesFooding);
                                                }
                                                updateExpenseStatusCommand.PayableAmount = expense.Amount;
                                                var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                                updateExpenseStatusCommand.DeviationAmount = 0;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (expenseList.Count > 0)
                                    {
                                        foreach (var expense in expenseList)
                                        {
                                            updateExpenseStatusCommand.Id = expense.Id;
                                            if (userDetails.IsDirector)
                                            {
                                                updateExpenseStatusCommand.Status = "APPROVED";
                                                updateExpenseStatusCommand.DeviationAmount = 0;
                                                updateExpenseStatusCommand.Allowance = "Director(Full)";
                                            }
                                            else
                                            {
                                                updateExpenseStatusCommand.Status = resultUser.CompanyAccountId == new Guid("D0CCEA5F-5393-4A34-9DF6-43A9F51F9F91") ? "PENDING" : "APPROVED";
                                                updateExpenseStatusCommand.DeviationAmount = 0;
                                                updateExpenseStatusCommand.Allowance = "Actual";
                                            }
                                            updateExpenseStatusCommand.PayableAmount = expense.Amount;
                                            var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                        }
                                    }
                                }

                                //Is Director
                                if (userDetails.IsDirector)
                                {
                                    if (expenseList.Count > 0)
                                    {
                                        foreach (var expense in expenseList)
                                        {
                                            updateExpenseStatusCommand.Id = expense.Id;
                                            updateExpenseStatusCommand.Status = "APPROVED";
                                            updateExpenseStatusCommand.PayableAmount = expense.Amount;
                                            updateExpenseStatusCommand.DeviationAmount = 0;
                                            updateExpenseStatusCommand.Allowance = "Director(Full)";
                                            var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                        }
                                    }
                                }

                            }

                            //Others
                            if (item.Id == new Guid("6C3EB31C-DF53-495A-B871-E2EB3CEF74D2"))
                            {
                                if (userDetails.IsDirector)
                                {
                                    if (expenseList.Count > 0)
                                    {
                                        foreach (var expense in expenseList)
                                        {
                                            updateExpenseStatusCommand.Id = expense.Id;
                                            updateExpenseStatusCommand.Status = "APPROVED";
                                            updateExpenseStatusCommand.PayableAmount = expense.Amount;
                                            updateExpenseStatusCommand.DeviationAmount = 0;
                                            updateExpenseStatusCommand.Allowance = "Director(Full)";
                                            var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                        }
                                    }
                                }
                                else
                                {
                                    if (expenseList.Count > 0)
                                    {
                                        foreach (var expense in expenseList)
                                        {
                                            updateExpenseStatusCommand.Id = expense.Id;
                                            updateExpenseStatusCommand.DeviationAmount = expense.Amount;
                                            updateExpenseStatusCommand.Allowance = "Not Specified";
                                            var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                        }
                                    }

                                }

                            }
                        }
                    }

                }
                //===============

                //**Email Start**
                if (addMasterExpenseCommand.Status == "APPLIED")
                {
                    string email = this._configuration.GetSection("AppSettings")["Email"];
                    string expenseRedirectionURL = this._configuration.GetSection("ExpenseRedirection")["ExpenseRedirectionURL"];

                    if (email == "Yes")
                    {
                        var itinerary = await _tripItineraryRepository.All.Where(x => x.TripId == addMasterExpenseCommand.TripId).ToListAsync();

                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Template", itinerary.Count == 0 ? "Expense.html" : "ExpenseWithTrip.html");
                        var defaultSmtp = await _emailSMTPSettingRepository.FindBy(c => c.IsDefault).FirstOrDefaultAsync();
                        var reportingHead = _userRepository.FindAsync(userResult.ReportingTo.Value).Result;

                        using (StreamReader sr = new StreamReader(filePath))
                        {
                            string templateBody = sr.ReadToEnd();
                            templateBody = templateBody.Replace("{NAME}", string.Concat(userResult.FirstName, " ", userResult.LastName));
                            templateBody = templateBody.Replace("{DATETIME}", DateTime.Now.ToString("dddd, dd MMMM yyyy"));
                            templateBody = templateBody.Replace("{EXPENSE_NO}", Convert.ToString(addMasterExpenseCommand.ExpenseNo));
                            templateBody = templateBody.Replace("{EXPENSE_AMOUNT}", Convert.ToString(addMasterExpenseCommand.TotalAmount));
                            templateBody = templateBody.Replace("{EXPENSE_TYPE}", Convert.ToString(addMasterExpenseCommand.ExpenseType));
                            templateBody = templateBody.Replace("{NOOFBILL}", Convert.ToString(addMasterExpenseCommand.NoOfBill));
                            templateBody = templateBody.Replace("{GROUPEXPENSE}", Convert.ToString(addMasterExpenseCommand.IsGroupExpense == true ? "Yes" : "No"));
                            templateBody = templateBody.Replace("{NO_OF_PERSON}", Convert.ToString(addMasterExpenseCommand.NoOfPerson == null ? "1" : addMasterExpenseCommand.NoOfPerson));
                            templateBody = templateBody.Replace("{EXPENSE_STATUS}", Convert.ToString(addMasterExpenseCommand.Status));

                            templateBody = templateBody.Replace("{WEB_URL}", expenseRedirectionURL + result.Data.Id);
                            templateBody = templateBody.Replace("{APP_URL}", expenseRedirectionURL + result.Data.Id + "/" + result.Data.CreatedBy);


                            if (addMasterExpenseCommand.TripId != null && addMasterExpenseCommand.TripId != Guid.Empty)
                            {
                                templateBody = templateBody.Replace("{TOUR_DETAILS}", "Tour Details");
                                templateBody = templateBody.Replace("{MODE_OF_TRIP}", "Mode Of Trip :");
                                templateBody = templateBody.Replace("{MODE_OF_TRIP_VAL}", "Domestic");


                                var responseData = await _tripRepository.AllIncluding(c => c.CreatedByUser).Where(x => x.Id == addMasterExpenseCommand.TripId).FirstOrDefaultAsync();
                                string itineraryHtml = ItineraryHtml(itinerary, responseData.TripType);
                                templateBody = templateBody.Replace("{ITINERARY_HTML}", itineraryHtml);

                                var ca = await _companyAccountRepository.FindAsync(responseData.CompanyAccountId.Value);
                                templateBody = templateBody.Replace("{BILLING_COMPANY}", ca.AccountName);
                                templateBody = templateBody.Replace("{DEPARTMENT}", Convert.ToString(responseData.DepartmentName));
                                templateBody = templateBody.Replace("{TRIP_TYPE}", Convert.ToString(responseData.TripType));
                                templateBody = templateBody.Replace("{JOURNEY_PURPOSE}", Convert.ToString(responseData.PurposeFor));
                            }
                            else
                            {
                                templateBody = templateBody.Replace("{TOUR_DETAILS}", "");
                                templateBody = templateBody.Replace("{MODE_OF_TRIP}", "");
                                templateBody = templateBody.Replace("{MODE_OF_TRIP_VAL}", "");
                                templateBody = templateBody.Replace("{ITINERARY_HTML}", "");

                                var ca = await _companyAccountRepository.FindAsync(addMasterExpenseCommand.CompanyAccountId.Value);
                                templateBody = templateBody.Replace("{BILLING_COMPANY}", ca.AccountName);
                                templateBody = templateBody.Replace("{DEPARTMENT}", "");
                                templateBody = templateBody.Replace("{TRIP_TYPE}", "");
                                templateBody = templateBody.Replace("{JOURNEY_PURPOSE}", "");
                            }

                            EmailHelper.SendEmail(new SendEmailSpecification
                            {
                                Body = templateBody,
                                FromAddress = defaultSmtp.UserName,
                                Host = defaultSmtp.Host,
                                IsEnableSSL = defaultSmtp.IsEnableSSL,
                                Password = defaultSmtp.Password,
                                Port = defaultSmtp.Port,
                                Subject = "Expense - " + DateTime.Now.Date.ToString("dd-MM-yyyy"),
                                //ToAddress = userResult.UserName,
                                ToAddress = reportingHead.UserName,
                                CCAddress = string.IsNullOrEmpty(userResult.AlternateEmail) ?
                                userResult.UserName :
                                userResult.UserName + "," + userResult.AlternateEmail,
                                UserName = defaultSmtp.UserName
                            });
                        }
                    }
                    //**Email End**


                    //*** Start Push Notification User ***
                    MessageRequest userRequest = new MessageRequest()
                    {
                        Body = "Master Expense added - Expense No. " + result.Data.ExpenseNo,
                        CustomKey = "Expense",
                        DeviceToken = userResult.DeviceKey,
                        DeviceType = userResult.IsDeviceTypeAndroid,
                        Id = result.Data.Id.ToString(),
                        Title = "SFT Travel Desk",
                        UserId = userResult.Id.ToString()
                    };
                    var user = PushNotificationForExpense(userRequest);

                    //*** End Push Notification User ***

                    //*** Start Push Notification Reporting Head ***

                    var reporting = await _userRepository.FindAsync(userResult.ReportingTo.Value);
                    MessageRequest rmRequest = new MessageRequest()
                    {
                        Body = "Master Expense added - Expense No. " + result.Data.ExpenseNo,
                        CustomKey = "Expense",
                        DeviceToken = reporting.DeviceKey,
                        DeviceType = reporting.IsDeviceTypeAndroid,
                        Id = result.Data.Id.ToString(),
                        Title = "SFT Travel Desk",
                        UserId = userResult.Id.ToString()
                    };
                    var rmUser = PushNotificationForExpense(rmRequest);

                    //*** Start Push Notification Reporting Head ***
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
            var userDetails = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));

            updateMasterExpenseCommand.NoOfBill = updateMasterExpenseCommand.ExpenseDetails.Where(a => a.Amount > 0).Count();
            updateMasterExpenseCommand.TotalAmount = updateMasterExpenseCommand.ExpenseDetails.Sum(a => a.Amount);
            var result = await _mediator.Send(updateMasterExpenseCommand);
            if (result.Success)
            {
                var masterResponseData = await _masterExpenseRepository.FindAsync(updateMasterExpenseCommand.Id);
                var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                var addMasterExpenseTrackingCommand = new AddExpenseTrackingCommand()
                {
                    MasterExpenseId = updateMasterExpenseCommand.Id,
                    ExpenseTypeName = masterResponseData.Name,
                    ActionType = "Activity",
                    Remarks = masterResponseData.ExpenseNo + " modified",
                    Status = "Updated",
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now,
                };
                var masterResponse = await _mediator.Send(addMasterExpenseTrackingCommand);

                var command = new DeleteExpenseDetailCommand() { MasterExpenseId = updateMasterExpenseCommand.Id };
                var resultDel = await _mediator.Send(command);

                // Guid id = result.Data.Id; 
                foreach (var item in updateMasterExpenseCommand.ExpenseDetails)
                {

                    UpdateExpenseCommand updateExpenseCommand = new UpdateExpenseCommand();
                    if (item.Id == null || item.Id == Guid.Empty)
                    {
                        item.Id = Guid.NewGuid();
                    }
                    item.ExpenseDetail.ForEach(c => { c.MasterExpenseId = item.MasterExpenseId; c.ExpenseId = item.Id; c.Id = Guid.Empty; });
                    updateExpenseCommand = item;
                    var result2 = await _mediator.Send(updateExpenseCommand);

                    var responseData = await _expenseRepository.FindAsync(item.Id);

                    var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                    {
                        MasterExpenseId = updateMasterExpenseCommand.Id,
                        ExpenseId = item.Id,
                        ExpenseTypeName = masterResponseData.ExpenseType,
                        ActionType = "Activity",
                        Remarks = masterResponseData.ExpenseNo + " modified.",
                        Status = "Updated",
                        ActionBy = Guid.Parse(_userInfoToken.Id),
                        ActionDate = DateTime.Now,
                    };
                    var response = await _mediator.Send(addMasterExpenseTrackingCommand);
                }



                //============================Approved Trip
                int noOfDays = 1;
                if (updateMasterExpenseCommand.ExpenseType == "Local Trip")
                {
                    if (updateMasterExpenseCommand.ExpenseDetails.Count > 0)
                    {
                        var ExpenseDetailsList = updateMasterExpenseCommand.ExpenseDetails.GroupBy(a => a.ExpenseDate).ToList();
                        //var FirstDate = ExpenseDetailsList.First().ExpenseDate;
                        //var LastDate = ExpenseDetailsList.Last().ExpenseDate;
                        //noOfDays = (int)(LastDate - FirstDate).TotalDays+1;
                        noOfDays = ExpenseDetailsList.Count();
                    }
                    else
                    {
                        noOfDays = 1;
                    }

                }
                else
                {
                    var tripDetails = _tripRepository.FindAsync(updateMasterExpenseCommand.TripId.Value);
                    noOfDays = (int)(tripDetails.Result.TripEnds - tripDetails.Result.TripStarts).TotalDays + 1;
                }
                if (noOfDays > 0)
                {



                    var expenseCategory = _expenseCategoryRepository.All.ToList();
                    if (expenseCategory.Count > 0)
                    {
                        //===============================
                        var getUserGradeAndAccountCommand = new GetUserGradeAndAccountCommand
                        {
                            UserId = Guid.Parse(_userInfoToken.Id)//result.Data.CreatedByUser.Id,
                        };
                        var resultUser = await _mediator.Send(getUserGradeAndAccountCommand);
                        PoliciesDetailResource policiesDetailResourceQuery = new PoliciesDetailResource
                        {
                            CompanyAccountId = resultUser.CompanyAccountId,
                            GradeId = resultUser.GradeId,
                        };

                        //PoliciesDetail
                        var getAllPoliciesDetailCommand = new GetAllPoliciesDetailCommand
                        {
                            PoliciesDetailResource = policiesDetailResourceQuery
                        };
                        var resultPoliciesDetail = await _mediator.Send(getAllPoliciesDetailCommand);
                        if (resultPoliciesDetail == null)
                        {
                            return NotFound("Policies not mapped with user");
                        }

                        //Policies Lodging Fooding
                        var getAllPoliciesLodgingFoodingCommand = new GetAllPoliciesLodgingFoodingCommand
                        {
                            Id = resultPoliciesDetail.FirstOrDefault().Id
                        };
                        var resultPoliciesLodgingFooding = await _mediator.Send(getAllPoliciesLodgingFoodingCommand);

                        //Conveyance
                        var getAllConveyanceCommand = new GetAllConveyanceCommand
                        {
                            Id = resultPoliciesDetail.FirstOrDefault().Id
                        };
                        var resultConveyance = await _mediator.Send(getAllConveyanceCommand);

                        //PoliciesVehicleConveyance
                        var getAlllPoliciesVehicleConveyanceCommand = new GetAllPoliciesVehicleConveyanceCommand
                        {
                            Id = resultPoliciesDetail.FirstOrDefault().Id
                        };
                        var resultlPoliciesVehicleConveyance = await _mediator.Send(getAlllPoliciesVehicleConveyanceCommand);

                        //PoliciesSetting
                        var getAllPoliciesSettingCommand = new GetAllPoliciesSettingCommand
                        {
                            Id = resultPoliciesDetail.FirstOrDefault().Id
                        };
                        var resultPoliciesSetting = await _mediator.Send(getAllPoliciesSettingCommand);
                        //===============================

                        bool IsDeviation = false;
                        UpdateExpenseStatusCommand updateExpenseStatusCommand = new UpdateExpenseStatusCommand();
                        foreach (var item in expenseCategory)
                        {
                            var expenseAmount = _expenseRepository.All.Where(a => a.MasterExpenseId == masterResponse.Data.MasterExpenseId && a.ExpenseCategoryId == item.Id).Sum(a => a.Amount);
                            var expenseList = _expenseRepository.All.Where(a => a.MasterExpenseId == masterResponse.Data.MasterExpenseId && a.ExpenseCategoryId == item.Id).ToList();

                            //--Fare
                            if (item.Id == new Guid("DCAA05B6-5F1E-402F-835E-0704A3A1A455"))
                            {

                                if (expenseList.Count > 0)
                                {
                                    foreach (var expense in expenseList)
                                    {
                                        updateExpenseStatusCommand.Id = expense.Id;
                                        if (userDetails.IsDirector)
                                        {
                                            updateExpenseStatusCommand.Status = "APPROVED";
                                        }
                                        else
                                        {
                                            updateExpenseStatusCommand.Status = resultUser.CompanyAccountId == new Guid("D0CCEA5F-5393-4A34-9DF6-43A9F51F9F91") ? "PENDING" : "APPROVED";
                                        }
                                        updateExpenseStatusCommand.PayableAmount = expense.Amount;
                                        var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                    }
                                }

                            }
                            //--Lodging (Metro City)
                            if (item.Id == new Guid("FBF965BD-A53E-4D97-978A-34C2007202E5"))
                            {
                                if (resultPoliciesLodgingFooding.IsMetroCities == true)
                                {
                                    int localNoOfDays = 1;
                                    var ExpenseDetailsList = updateMasterExpenseCommand.ExpenseDetails.Where(a => a.Amount > 0 && a.ExpenseCategoryId == new Guid("FBF965BD-A53E-4D97-978A-34C2007202E5")).GroupBy(a => a.ExpenseDate).ToList();
                                    if (ExpenseDetailsList != null)
                                    {
                                        localNoOfDays = ExpenseDetailsList.Count();
                                    }
                                    // decimal PoliciesLodgingFooding = resultPoliciesLodgingFooding.MetroCitiesUptoAmount * Convert.ToDecimal(localNoOfDays);
                                    decimal PoliciesLodgingFooding = resultPoliciesLodgingFooding.MetroCitiesUptoAmount;
                                    //if (expenseAmount > PoliciesLodgingFooding)
                                    //{
                                    //    IsDeviation = true;
                                    //    if (expenseList.Count > 0)
                                    //    {
                                    //        foreach (var expense in expenseList)
                                    //        {
                                    //            updateExpenseStatusCommand.Id = expense.Id;
                                    //            updateExpenseStatusCommand.Status = "PENDING";
                                    //            updateExpenseStatusCommand.PayableAmount = 0;
                                    //            var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                    //        }
                                    //    }
                                    //}
                                    //else
                                    //{
                                    if (expenseList.Count > 0)
                                    {
                                        foreach (var expense in expenseList)
                                        {
                                            if (userDetails.IsDirector)
                                            {
                                                updateExpenseStatusCommand.Status = "APPROVED";
                                                updateExpenseStatusCommand.PayableAmount = expense.Amount;
                                            }
                                            if (expense.Amount <= PoliciesLodgingFooding)
                                            {
                                                updateExpenseStatusCommand.Id = expense.Id;
                                                updateExpenseStatusCommand.Status = resultUser.CompanyAccountId == new Guid("D0CCEA5F-5393-4A34-9DF6-43A9F51F9F91") ? "PENDING" : "APPROVED";
                                                updateExpenseStatusCommand.PayableAmount = expense.Amount;
                                                var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                            }
                                            else
                                            {
                                                updateExpenseStatusCommand.Id = expense.Id;
                                                updateExpenseStatusCommand.Status = "PENDING";
                                                updateExpenseStatusCommand.PayableAmount = 0;
                                                var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                            }

                                        }
                                    }
                                    //}
                                }

                                if (userDetails.IsDirector)
                                {
                                    if (expenseList.Count > 0)
                                    {
                                        foreach (var expense in expenseList)
                                        {
                                            updateExpenseStatusCommand.Id = expense.Id;
                                            updateExpenseStatusCommand.Status = "APPROVED";
                                            updateExpenseStatusCommand.PayableAmount = expense.Amount;
                                            var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                        }
                                    }
                                }

                            }
                            //-- Lodging (Other City)
                            if (item.Id == new Guid("1AADD03D-90E1-4589-8B9D-6121049B490D"))
                            {
                                if (resultPoliciesLodgingFooding.OtherCities == true)
                                {
                                    int localNoOfDays = 1;
                                    var ExpenseDetailsList = updateMasterExpenseCommand.ExpenseDetails.Where(a => a.Amount > 0 && a.ExpenseCategoryId == new Guid("1AADD03D-90E1-4589-8B9D-6121049B490D")).GroupBy(a => a.ExpenseDate).ToList();
                                    if (ExpenseDetailsList != null)
                                    {
                                        localNoOfDays = ExpenseDetailsList.Count();
                                    }
                                    //decimal PoliciesLodgingFooding = resultPoliciesLodgingFooding.OtherCitiesUptoAmount * Convert.ToDecimal(localNoOfDays);
                                    decimal PoliciesLodgingFooding = resultPoliciesLodgingFooding.OtherCitiesUptoAmount;
                                    //if (expenseAmount > PoliciesLodgingFooding)
                                    //{
                                    //    IsDeviation = true;
                                    //    if (expenseList.Count > 0)
                                    //    {
                                    //        foreach (var expense in expenseList)
                                    //        {
                                    //            updateExpenseStatusCommand.Id = expense.Id;
                                    //            updateExpenseStatusCommand.Status = "PENDING";
                                    //            updateExpenseStatusCommand.PayableAmount = 0;
                                    //            var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                    //        }
                                    //    }
                                    //}
                                    //else
                                    //{
                                    if (expenseList.Count > 0)
                                    {
                                        foreach (var expense in expenseList)
                                        {
                                            if (userDetails.IsDirector)
                                            {
                                                updateExpenseStatusCommand.Status = "APPROVED";
                                                updateExpenseStatusCommand.PayableAmount = expense.Amount;
                                            }
                                            if (expense.Amount <= PoliciesLodgingFooding)
                                            {
                                                updateExpenseStatusCommand.Id = expense.Id;
                                                updateExpenseStatusCommand.Status = resultUser.CompanyAccountId == new Guid("D0CCEA5F-5393-4A34-9DF6-43A9F51F9F91") ? "PENDING" : "APPROVED";
                                                updateExpenseStatusCommand.PayableAmount = expense.Amount;
                                                var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                            }
                                            else
                                            {
                                                updateExpenseStatusCommand.Id = expense.Id;
                                                updateExpenseStatusCommand.Status = "PENDING";
                                                updateExpenseStatusCommand.PayableAmount = 0;
                                                var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                            }
                                        }
                                    }
                                    //}
                                }

                                //dddd
                                if (userDetails.IsDirector)
                                {
                                    if (expenseList.Count > 0)
                                    {
                                        foreach (var expense in expenseList)
                                        {
                                            updateExpenseStatusCommand.Id = expense.Id;
                                            updateExpenseStatusCommand.Status = "APPROVED";
                                            updateExpenseStatusCommand.PayableAmount = expense.Amount;
                                            var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                        }
                                    }
                                }

                            }
                            //--Conveyance (within a City)
                            if (item.Id == new Guid("B1977DB3-D909-4936-A5DA-41BF84638963"))
                            {
                                var Conveyance = resultConveyance.Where(a => a.Name == "Conveyance (within a city)");
                                if (Conveyance != null)
                                {
                                    var ConveyancesItemAll = Conveyance.Select(a => a.conveyancesItem).Where(b => b.Any(a => a.ConveyanceItemName == "Budget")).FirstOrDefault();
                                    var ConveyancesItem = ConveyancesItemAll.Where(a => a.ConveyanceItemName == "Budget");
                                    if (ConveyancesItem != null)
                                    {
                                        bool IsCheck = (bool)ConveyancesItem.FirstOrDefault().IsCheck;
                                        if (IsCheck == true)
                                        {
                                            decimal ConveyancesAmount = 0;
                                            if (ConveyancesItem.FirstOrDefault().Amount != null)
                                            {
                                                ConveyancesAmount = (decimal)(ConveyancesItem.FirstOrDefault().Amount) * Convert.ToDecimal(noOfDays);
                                            }
                                            if (expenseAmount > ConveyancesAmount)
                                            {
                                                IsDeviation = true;
                                                if (expenseList.Count > 0)
                                                {
                                                    foreach (var expense in expenseList)
                                                    {
                                                        updateExpenseStatusCommand.Id = expense.Id;
                                                        if (userDetails.IsDirector)
                                                        {
                                                            updateExpenseStatusCommand.Status = "APPROVED";
                                                            updateExpenseStatusCommand.PayableAmount = expense.Amount;
                                                        }
                                                        else
                                                        {
                                                            updateExpenseStatusCommand.Status = "PENDING";
                                                            updateExpenseStatusCommand.PayableAmount = 0;
                                                        }

                                                        var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (expenseList.Count > 0)
                                                {
                                                    foreach (var expense in expenseList)
                                                    {
                                                        updateExpenseStatusCommand.Id = expense.Id;
                                                        if (userDetails.IsDirector)
                                                        {
                                                            updateExpenseStatusCommand.Status = "APPROVED";
                                                        }
                                                        else
                                                        {
                                                            updateExpenseStatusCommand.Status = resultUser.CompanyAccountId == new Guid("D0CCEA5F-5393-4A34-9DF6-43A9F51F9F91") ? "PENDING" : "APPROVED";
                                                        }
                                                        updateExpenseStatusCommand.PayableAmount = expense.Amount;
                                                        var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                                    }
                                                }
                                            }
                                        }
                                        else //Actuals
                                        {
                                            if (expenseList.Count > 0)
                                            {
                                                foreach (var expense in expenseList)
                                                {
                                                    updateExpenseStatusCommand.Id = expense.Id;
                                                    //updateExpenseStatusCommand.Status = "PENDING"; updateExpenseStatusCommand.Status = resultUser.CompanyAccountId == new Guid("D0CCEA5F-5393-4A34-9DF6-43A9F51F9F91") ? "PENDING" : "APPROVED";
                                                    if (userDetails.IsDirector)
                                                    {
                                                        updateExpenseStatusCommand.Status = "APPROVED";
                                                        updateExpenseStatusCommand.PayableAmount = expense.Amount;
                                                    }
                                                    else
                                                    {
                                                        updateExpenseStatusCommand.Status = "PENDING";
                                                        updateExpenseStatusCommand.PayableAmount = 0;
                                                    }

                                                    var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            //--Conveyance (city to outer area)
                            if (item.Id == new Guid("5278397A-C8DD-475A-A7A7-C05708B2BB06"))
                            {
                                var Conveyance = resultConveyance.Where(a => a.Name == "Conveyance (city to outer area)");
                                if (Conveyance != null)
                                {
                                    var ConveyancesItemAll = Conveyance.Select(a => a.conveyancesItem).Where(b => b.Any(a => a.ConveyanceItemName == "Budget")).FirstOrDefault();
                                    var ConveyancesItem = ConveyancesItemAll.Where(a => a.ConveyanceItemName == "Budget");
                                    if (ConveyancesItem != null)
                                    {
                                        bool IsCheck = (bool)ConveyancesItem.FirstOrDefault().IsCheck;
                                        if (IsCheck == true)
                                        {
                                            decimal ConveyancesAmount = 0;
                                            if (ConveyancesItem.FirstOrDefault().Amount != null)
                                            {
                                                ConveyancesAmount = (decimal)(ConveyancesItem.FirstOrDefault().Amount) * Convert.ToDecimal(noOfDays);
                                            }
                                            if (expenseAmount > ConveyancesAmount)
                                            {
                                                IsDeviation = true;
                                                if (expenseList.Count > 0)
                                                {
                                                    foreach (var expense in expenseList)
                                                    {
                                                        updateExpenseStatusCommand.Id = expense.Id;
                                                        if (userDetails.IsDirector)
                                                        {
                                                            updateExpenseStatusCommand.Status = "APPROVED";
                                                            updateExpenseStatusCommand.PayableAmount = expense.Amount;
                                                        }
                                                        else
                                                        {
                                                            updateExpenseStatusCommand.Status = "PENDING";
                                                            updateExpenseStatusCommand.PayableAmount = 0;
                                                        }

                                                        var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (expenseList.Count > 0)
                                                {
                                                    foreach (var expense in expenseList)
                                                    {
                                                        updateExpenseStatusCommand.Id = expense.Id;
                                                        if (userDetails.IsDirector)
                                                        {
                                                            updateExpenseStatusCommand.Status = "APPROVED";
                                                        }
                                                        else
                                                        {
                                                            updateExpenseStatusCommand.Status = resultUser.CompanyAccountId == new Guid("D0CCEA5F-5393-4A34-9DF6-43A9F51F9F91") ? "PENDING" : "APPROVED";
                                                        }
                                                        updateExpenseStatusCommand.PayableAmount = expense.Amount;
                                                        var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                                    }
                                                }
                                            }
                                        }
                                        else //Actuals 
                                        {
                                            if (expenseList.Count > 0)
                                            {
                                                foreach (var expense in expenseList)
                                                {
                                                    updateExpenseStatusCommand.Id = expense.Id;
                                                    //updateExpenseStatusCommand.Status = resultUser.CompanyAccountId == new Guid("D0CCEA5F-5393-4A34-9DF6-43A9F51F9F91") ? "PENDING" : "APPROVED";
                                                    if (userDetails.IsDirector)
                                                    {
                                                        updateExpenseStatusCommand.Status = "APPROVED";
                                                        updateExpenseStatusCommand.PayableAmount = expense.Amount;
                                                    }
                                                    else
                                                    {
                                                        updateExpenseStatusCommand.Status = "PENDING";
                                                        updateExpenseStatusCommand.PayableAmount = 0;
                                                    }

                                                    var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            //--MISC /DA
                            if (item.Id == new Guid("ED69E9A0-2D54-4A91-A598-F79973B9FE99"))
                            {
                                decimal DA = 0;
                                if (resultPoliciesDetail.FirstOrDefault().DailyAllowance != null)
                                {
                                    //DA = (decimal)resultPoliciesDetail.FirstOrDefault().DailyAllowance * Convert.ToDecimal(noOfDays);
                                    DA = (decimal)resultPoliciesDetail.FirstOrDefault().DailyAllowance;
                                }

                                //if (expenseAmount > DA)
                                //{
                                IsDeviation = true;
                                if (expenseList.Count > 0)
                                {
                                    foreach (var expense in expenseList)
                                    {
                                        if (expense.Amount <= DA)
                                        {
                                            updateExpenseStatusCommand.Id = expense.Id;
                                            if (userDetails.IsDirector)
                                            {
                                                updateExpenseStatusCommand.Status = "APPROVED";
                                            }
                                            else
                                            {
                                                updateExpenseStatusCommand.Status = resultUser.CompanyAccountId == new Guid("D0CCEA5F-5393-4A34-9DF6-43A9F51F9F91") ? "PENDING" : "APPROVED";
                                            }
                                            updateExpenseStatusCommand.PayableAmount = expense.Amount;
                                            var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                        }
                                        else
                                        {
                                            updateExpenseStatusCommand.Id = expense.Id;
                                            if (userDetails.IsDirector)
                                            {
                                                updateExpenseStatusCommand.Status = "APPROVED";
                                                updateExpenseStatusCommand.PayableAmount = expense.Amount;
                                            }
                                            else
                                            {
                                                updateExpenseStatusCommand.Status = "PENDING";
                                                updateExpenseStatusCommand.PayableAmount = 0;
                                            }

                                            var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                        }

                                    }
                                }
                                //}
                                //else
                                //{
                                //    if (expenseList.Count > 0)
                                //    {
                                //        foreach (var expense in expenseList)
                                //        {
                                //            updateExpenseStatusCommand.Id = expense.Id;
                                //            updateExpenseStatusCommand.Status = "APPROVED";
                                //            updateExpenseStatusCommand.PayableAmount = expense.Amount;
                                //            var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                //        }
                                //    }
                                //}
                            }
                            //--Fooding Allowance
                            if (item.Id == new Guid("BB0BF3AA-1FD9-4F1C-9FDE-8498073C58A9"))
                            {
                                if (resultPoliciesLodgingFooding.IsBudget == true)
                                {
                                    decimal PoliciesFooding = 0;
                                    int localNoOfDays = 1;
                                    var ExpenseDetailsList = updateMasterExpenseCommand.ExpenseDetails.Where(a => a.Amount > 0 && a.ExpenseCategoryId == new Guid("BB0BF3AA-1FD9-4F1C-9FDE-8498073C58A9")).GroupBy(a => a.ExpenseDate).ToList();
                                    if (ExpenseDetailsList != null)
                                    {
                                        localNoOfDays = ExpenseDetailsList.Count();
                                    }
                                    if (resultPoliciesLodgingFooding.BudgetAmount != null)
                                    {
                                        PoliciesFooding = resultPoliciesLodgingFooding.BudgetAmount * Convert.ToDecimal(localNoOfDays);
                                    }
                                    if (expenseAmount > PoliciesFooding)
                                    {
                                        IsDeviation = true;
                                        if (expenseList.Count > 0)
                                        {
                                            foreach (var expense in expenseList)
                                            {
                                                updateExpenseStatusCommand.Id = expense.Id;
                                                if (userDetails.IsDirector)
                                                {
                                                    updateExpenseStatusCommand.Status = "APPROVED";
                                                    updateExpenseStatusCommand.PayableAmount = expense.Amount;
                                                }
                                                else
                                                {
                                                    updateExpenseStatusCommand.Status = "PENDING";
                                                    updateExpenseStatusCommand.PayableAmount = 0;
                                                }

                                                var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (expenseList.Count > 0)
                                        {
                                            foreach (var expense in expenseList)
                                            {
                                                updateExpenseStatusCommand.Id = expense.Id;
                                                if (userDetails.IsDirector)
                                                {
                                                    updateExpenseStatusCommand.Status = "APPROVED";
                                                }
                                                else
                                                {
                                                    updateExpenseStatusCommand.Status = resultUser.CompanyAccountId == new Guid("D0CCEA5F-5393-4A34-9DF6-43A9F51F9F91") ? "PENDING" : "APPROVED";
                                                }
                                                updateExpenseStatusCommand.PayableAmount = expense.Amount;
                                                var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (expenseList.Count > 0)
                                    {
                                        foreach (var expense in expenseList)
                                        {
                                            updateExpenseStatusCommand.Id = expense.Id;
                                            if (userDetails.IsDirector)
                                            {
                                                updateExpenseStatusCommand.Status = "APPROVED";
                                            }
                                            else
                                            {
                                                updateExpenseStatusCommand.Status = resultUser.CompanyAccountId == new Guid("D0CCEA5F-5393-4A34-9DF6-43A9F51F9F91") ? "PENDING" : "APPROVED";
                                            }
                                            updateExpenseStatusCommand.PayableAmount = expense.Amount;
                                            var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                        }
                                    }
                                }

                                //Is Director
                                if (userDetails.IsDirector)
                                {
                                    if (expenseList.Count > 0)
                                    {
                                        foreach (var expense in expenseList)
                                        {
                                            updateExpenseStatusCommand.Id = expense.Id;
                                            updateExpenseStatusCommand.Status = "APPROVED";
                                            updateExpenseStatusCommand.PayableAmount = expense.Amount;
                                            var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                        }
                                    }
                                }
                            }
                            //Others
                            if (item.Id == new Guid("6C3EB31C-DF53-495A-B871-E2EB3CEF74D2"))
                            {
                                if (userDetails.IsDirector)
                                {
                                    if (expenseList.Count > 0)
                                    {
                                        foreach (var expense in expenseList)
                                        {
                                            updateExpenseStatusCommand.Id = expense.Id;
                                            updateExpenseStatusCommand.Status = "APPROVED";
                                            updateExpenseStatusCommand.PayableAmount = expense.Amount;
                                            var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                        }
                                    }
                                }

                            }
                        }

                        //===============
                        SyncMasterExpenseAmountCommand syncMasterExpenseAmountCommand = new SyncMasterExpenseAmountCommand();
                        syncMasterExpenseAmountCommand.Id = updateMasterExpenseCommand.ExpenseDetails.FirstOrDefault().Id;
                        var responseSync = await _mediator.Send(syncMasterExpenseAmountCommand);
                    }
                }
                //===============


                //var exp = _expenseRepository.FindBy(c => c.MasterExpenseId == updateMasterExpenseCommand.Id).ToList();
                //_expenseRepository.RemoveRange(exp);

                //foreach (var item in updateMasterExpenseCommand.ExpenseDetails)
                //{
                //    AddExpenseCommand addExpenseCommand = new AddExpenseCommand();
                //    addExpenseCommand = item;
                //    addExpenseCommand.MasterExpenseId = updateMasterExpenseCommand.Id;
                //    addExpenseCommand.TripId = updateMasterExpenseCommand.TripId;
                //    addExpenseCommand.Status = "PENDING";
                //    var result2 = await _mediator.Send(addExpenseCommand);
                //    //result.Data.ExpenseId = result2.Data.Id;

                //    //var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                //    //{
                //    //    MasterExpenseId = id,
                //    //    ExpenseId = result.Data.ExpenseId,
                //    //    ExpenseTypeName = addExpenseCommand.Name,
                //    //    ActionType = "Activity",
                //    //    Remarks = addExpenseCommand.Name + " Expense Added By " + userResult.FirstName + " " + userResult.LastName,
                //    //    Status = "Expense Added By " + userResult.FirstName + " " + userResult.LastName,
                //    //    ActionBy = Guid.Parse(_userInfoToken.Id),
                //    //    ActionDate = DateTime.Now,
                //    //};
                //    //var response = await _mediator.Send(addExpenseTrackingCommand);
                //}


                //**Email Start**
                string email = this._configuration.GetSection("AppSettings")["Email"];
                string expenseRedirectionURL = this._configuration.GetSection("ExpenseRedirection")["ExpenseRedirectionURL"];

                if (email == "Yes")
                {
                    if (updateMasterExpenseCommand.Status == "APPLIED")
                    {
                        var itinerary = await _tripItineraryRepository.All.Where(x => x.TripId == masterResponseData.TripId).ToListAsync();

                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Template", itinerary.Count == 0 ? "ExpenseStatus.html" : "ExpenseStatusWithTrip.html");
                        var defaultSmtp = await _emailSMTPSettingRepository.FindBy(c => c.IsDefault).FirstOrDefaultAsync();
                        var reportingHead = _userRepository.FindAsync(userResult.ReportingTo.Value).Result;

                        using (StreamReader sr = new StreamReader(filePath))
                        {
                            string templateBody = sr.ReadToEnd();
                            templateBody = templateBody.Replace("{NAME}", string.Concat(userResult.FirstName, " ", userResult.LastName));
                            templateBody = templateBody.Replace("{DATETIME}", DateTime.Now.ToString("dddd, dd MMMM yyyy"));
                            templateBody = templateBody.Replace("{EXPENSE_NO}", Convert.ToString(masterResponseData.ExpenseNo));
                            templateBody = templateBody.Replace("{EXPENSE_AMOUNT}", Convert.ToString(masterResponseData.TotalAmount));
                            templateBody = templateBody.Replace("{EXPENSE_TYPE}", Convert.ToString(masterResponseData.ExpenseType));
                            templateBody = templateBody.Replace("{NOOFBILL}", Convert.ToString(masterResponseData.NoOfBill));
                            templateBody = templateBody.Replace("{GROUPEXPENSE}", Convert.ToString(masterResponseData.IsGroupExpense == true ? "Yes" : "No"));
                            templateBody = templateBody.Replace("{NO_OF_PERSON}", Convert.ToString(masterResponseData.NoOfPerson == null ? "1" : masterResponseData.NoOfPerson));
                            templateBody = templateBody.Replace("{EXPENSE_STATUS}", Convert.ToString(updateMasterExpenseCommand.Status));

                            templateBody = templateBody.Replace("{WEB_URL}", expenseRedirectionURL + masterResponseData.Id);
                            templateBody = templateBody.Replace("{APP_URL}", expenseRedirectionURL + masterResponseData.Id + "/" + masterResponseData.CreatedBy);


                            if (masterResponseData.TripId != null && masterResponseData.TripId != Guid.Empty)
                            {
                                templateBody = templateBody.Replace("{TOUR_DETAILS}", "Tour Details");
                                templateBody = templateBody.Replace("{MODE_OF_TRIP}", "Mode Of Trip :");
                                templateBody = templateBody.Replace("{MODE_OF_TRIP_VAL}", "Domestic");


                                var responseData = await _tripRepository.AllIncluding(c => c.CreatedByUser).Where(x => x.Id == masterResponseData.TripId).FirstOrDefaultAsync();
                                string itineraryHtml = ItineraryHtml(itinerary, responseData.TripType);
                                templateBody = templateBody.Replace("{ITINERARY_HTML}", itineraryHtml);

                                var ca = await _companyAccountRepository.FindAsync(responseData.CompanyAccountId.Value);
                                templateBody = templateBody.Replace("{BILLING_COMPANY}", ca.AccountName);
                                templateBody = templateBody.Replace("{DEPARTMENT}", Convert.ToString(responseData.DepartmentName));
                                templateBody = templateBody.Replace("{TRIP_TYPE}", Convert.ToString(responseData.TripType));
                                templateBody = templateBody.Replace("{JOURNEY_PURPOSE}", Convert.ToString(responseData.PurposeFor));
                            }
                            else
                            {
                                templateBody = templateBody.Replace("{TOUR_DETAILS}", "");
                                templateBody = templateBody.Replace("{MODE_OF_TRIP}", "");
                                templateBody = templateBody.Replace("{MODE_OF_TRIP_VAL}", "");
                                templateBody = templateBody.Replace("{ITINERARY_HTML}", "");

                                var ca = await _companyAccountRepository.FindAsync(masterResponseData.CompanyAccountId.Value);
                                templateBody = templateBody.Replace("{BILLING_COMPANY}", ca.AccountName);
                                templateBody = templateBody.Replace("{DEPARTMENT}", "");
                                templateBody = templateBody.Replace("{TRIP_TYPE}", "");
                                templateBody = templateBody.Replace("{JOURNEY_PURPOSE}", "");
                            }

                            EmailHelper.SendEmail(new SendEmailSpecification
                            {
                                Body = templateBody,
                                FromAddress = defaultSmtp.UserName,
                                Host = defaultSmtp.Host,
                                IsEnableSSL = defaultSmtp.IsEnableSSL,
                                Password = defaultSmtp.Password,
                                Port = defaultSmtp.Port,
                                Subject = "Expense - " + DateTime.Now.Date.ToString("dd-MM-yyyy"),
                                //ToAddress = userResult.UserName,
                                ToAddress = reportingHead.UserName,
                                CCAddress = string.IsNullOrEmpty(userResult.AlternateEmail) ?
                                userResult.UserName :
                                userResult.UserName + "," + userResult.AlternateEmail,
                                UserName = defaultSmtp.UserName
                            });
                        }

                        //*** Start Push Notification User ***
                        MessageRequest userRequest = new MessageRequest()
                        {
                            Body = "Expense status " + updateMasterExpenseCommand.Status + " - Expense No. " + masterResponseData.ExpenseNo,
                            CustomKey = "Expense",
                            DeviceToken = userResult.DeviceKey,
                            DeviceType = userResult.IsDeviceTypeAndroid,
                            Id = masterResponseData.Id.ToString(),
                            Title = "SFT Travel Desk",
                            UserId = userResult.Id.ToString()
                        };

                        var user = PushNotificationForExpense(userRequest);

                        //*** Start Push Notification User ***
                        MessageRequest rmRequest = new MessageRequest()
                        {
                            Body = "Expense status " + updateMasterExpenseCommand.Status + " - Expense No. " + masterResponseData.ExpenseNo,
                            CustomKey = "Expense",
                            DeviceToken = reportingHead.DeviceKey,
                            DeviceType = reportingHead.IsDeviceTypeAndroid,
                            Id = masterResponseData.Id.ToString(),
                            Title = "SFT Travel Desk",
                            UserId = reportingHead.Id.ToString()
                        };

                        var rm = PushNotificationForExpense(rmRequest);
                    }
                }
                //**Email End**
            }
            return ReturnFormattedResponse(result);
        }


        /// <summary>
        /// Deletes Expense Document
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete("DeleteExpenseDocument/{id}")]
        //[ClaimCheck("EXP_DELETE_EXPENSE")]
        public async Task<IActionResult> DeleteExpenseDocument(Guid id)
        {
            var command = new DeleteExpenseDocumentCommand() { Id = id };
            var result = await _mediator.Send(command);

            if (result.Success)
            {
                var expenseDocument = await _expenseDocumentRepository.FindAsync(id);
                var _expenseData = await _expenseRepository.FindAsync(expenseDocument.ExpenseId.Value);
                var masterResponseData = await _masterExpenseRepository.FindAsync(_expenseData.MasterExpenseId);

                var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));

                var addMasterExpenseTrackingCommand = new AddExpenseTrackingCommand()
                {
                    MasterExpenseId = masterResponseData.Id,
                    ExpenseTypeName = masterResponseData.ExpenseType,
                    ActionType = "Activity",
                    Remarks = "Expense document deleted - " + masterResponseData.ExpenseNo,
                    Status = "Deleted",
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now,
                };
                var masterResponse = await _mediator.Send(addMasterExpenseTrackingCommand);
            }

            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Deletes Expense Details
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete("DeleteExpenseDetails")]
        //[ClaimCheck("EXP_DELETE_EXPENSE")]
        public async Task<IActionResult> DeleteExpenseDetails(DeleteExpenseDetailCommand command)
        {
            //var command = new DeleteExpenseDetailCommand() { Id = id };
            var result = await _mediator.Send(command);

            if (result.Success)
            {
                var masterResponseData = await _masterExpenseRepository.FindAsync(command.MasterExpenseId.Value);

                var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));

                var addMasterExpenseTrackingCommand = new AddExpenseTrackingCommand()
                {
                    MasterExpenseId = masterResponseData.Id,
                    ExpenseTypeName = masterResponseData.ExpenseType,
                    ActionType = "Activity",
                    Remarks = "Expense details deleted - " + masterResponseData.ExpenseNo,
                    Status = "Deleted",
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now,
                };
                var masterResponse = await _mediator.Send(addMasterExpenseTrackingCommand);
            }

            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Add Expense Details
        /// </summary>
        /// <param name="addExpenseDetailListCommand"></param>
        /// <returns></returns>
        [HttpPost("AddExpenseDetails")]
        //[ClaimCheck("EXP_ADD_EXPENSE")]
        public async Task<IActionResult> AddExpenseDetails(AddExpenseDetailListCommand addExpenseDetailListCommand)
        {
            BTTEM.Data.Entities.ResponseData responseData = new BTTEM.Data.Entities.ResponseData();
            foreach (var item in addExpenseDetailListCommand.AddExpenseDetailList)
            {
                AddExpenseDetailCommand addExpenseDetailCommand = new AddExpenseDetailCommand();
                addExpenseDetailCommand = item;
                var result = await _mediator.Send(addExpenseDetailCommand);
                if (result.Success)
                {
                    responseData.status = true;
                    responseData.StatusCode = 200;

                    var masterResponseData = await _masterExpenseRepository.FindAsync(addExpenseDetailListCommand.AddExpenseDetailList.FirstOrDefault().MasterExpenseId.Value);
                    var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));

                    var addMasterExpenseTrackingCommand = new AddExpenseTrackingCommand()
                    {
                        MasterExpenseId = masterResponseData.Id,
                        ExpenseTypeName = masterResponseData.ExpenseType,
                        ActionType = "Activity",
                        Remarks = "Expense details added - " + masterResponseData.ExpenseNo,
                        Status = "Added",
                        ActionBy = Guid.Parse(_userInfoToken.Id),
                        ActionDate = DateTime.Now,
                    };
                    var masterResponse = await _mediator.Send(addMasterExpenseTrackingCommand);
                }
            }
            //var result = await _mediator.Send(updateCarBikeLogBookExpenseCommand);
            responseData.message = "Data Updated Successfully";

            return Ok(responseData);

        }

        // <summary>

        /// Update Expense Details
        /// </summary>
        /// <param name="updateExpenseDetailCommandList"></param>
        /// <returns></returns>
        [HttpPut("UpdateExpenseDetails")]
        //[ClaimCheck("EXP_ADD_EXPENSE")]
        public async Task<IActionResult> UpdateExpenseDetails(UpdateExpenseDetailListCommand updateExpenseDetailCommandList)
        {
            BTTEM.Data.Entities.ResponseData responseData = new BTTEM.Data.Entities.ResponseData();

            var command = new DeleteExpenseDetailCommand() { ExpenseId = updateExpenseDetailCommandList.UpdateExpenseDetailList.FirstOrDefault().ExpenseId };
            var result1 = await _mediator.Send(command);

            foreach (var item in updateExpenseDetailCommandList.UpdateExpenseDetailList)
            {
                UpdateExpenseDetailCommand updateExpenseDetailCommand = new UpdateExpenseDetailCommand();
                updateExpenseDetailCommand = item;
                var result = await _mediator.Send(updateExpenseDetailCommand);
                if (result.Success)
                {
                    responseData.status = true;
                    responseData.StatusCode = 200;

                    var masterResponseData = await _masterExpenseRepository.FindAsync(updateExpenseDetailCommandList.UpdateExpenseDetailList.FirstOrDefault().MasterExpenseId.Value);
                    var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));

                    var addMasterExpenseTrackingCommand = new AddExpenseTrackingCommand()
                    {
                        MasterExpenseId = masterResponseData.Id,
                        ExpenseTypeName = masterResponseData.ExpenseType,
                        ActionType = "Activity",
                        Remarks = "Expense details modified - " + masterResponseData.ExpenseNo,
                        Status = "Updated",
                        ActionBy = Guid.Parse(_userInfoToken.Id),
                        ActionDate = DateTime.Now,
                    };
                    var masterResponse = await _mediator.Send(addMasterExpenseTrackingCommand);
                }
            }
            //var result = await _mediator.Send(updateCarBikeLogBookExpenseCommand);
            responseData.message = "Data Updated Successfully";

            return Ok(responseData);
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
                var responseData = await _expenseRepository.FindAsync(id);
                var masterExpense = await _masterExpenseRepository.FindAsync(updateExpenseCommand.MasterExpenseId.Value);
                var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                {
                    MasterExpenseId = updateExpenseCommand.MasterExpenseId.Value,
                    ExpenseId = updateExpenseCommand.Id,
                    ExpenseTypeName = responseData.Name,
                    ActionType = "Activity",
                    Remarks = "Expense modified - " + masterExpense.ExpenseNo,
                    Status = "Updated",
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
                var responseData = await _expenseRepository.FindAsync(id);
                var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                {
                    ExpenseId = updateExpenseStatusCommand.Id,
                    ExpenseTypeName = responseData.Name,
                    MasterExpenseId = responseData.MasterExpenseId,
                    ActionType = "Activity",
                    Remarks = "Expense " + updateExpenseStatusCommand.Status == "APPROVED" ? updateExpenseStatusCommand.Status : " for " + updateExpenseStatusCommand.RejectReason,//responseData.Result.Name + " Expense Status Updated",
                    Status = "Updated",
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now,
                };
                var response = await _mediator.Send(addExpenseTrackingCommand);
            }
            SyncMasterExpenseAmountCommand syncMasterExpenseAmountCommand = new SyncMasterExpenseAmountCommand();
            syncMasterExpenseAmountCommand.Id = id;
            var responseSync = await _mediator.Send(syncMasterExpenseAmountCommand);

            return ReturnFormattedResponse(result);
        }


        /// <summary>
        /// Update Expense All Status.
        /// </summary>       
        /// <param name="allupdateExpenseStatusCommand"></param>
        /// <returns></returns>
        [HttpPut("AllUpdateExpenseStatus")]
        //[ClaimCheck("EXP_UPDATE_EXPENSE")]
        public async Task<IActionResult> AllUpdateExpenseStatus(AllUpdateExpenseStatusCommand allupdateExpenseStatusCommand)
        {
            DashboardReportData dashboardReportData = new DashboardReportData();
            int Response = 0;
            foreach (var item in allupdateExpenseStatusCommand.updateExpenseStatus)
            {
                UpdateExpenseStatusCommand updateExpenseStatusCommand = new UpdateExpenseStatusCommand();
                updateExpenseStatusCommand = item;
                var result = await _mediator.Send(updateExpenseStatusCommand);
                if (result.Success)
                {
                    Response = 1;
                }
            }


            if (Response == 1)
            {
                var responseData = await _expenseRepository.FindAsync(allupdateExpenseStatusCommand.updateExpenseStatus.FirstOrDefault().Id);
                var masterExpense = await _masterExpenseRepository.FindAsync(responseData.MasterExpenseId);
                //var masterExpense = await _masterExpenseRepository.AllIncluding(u => u.CreatedByUser).Where(x => x.Id == responseData.MasterExpenseId).FirstOrDefaultAsync();
                var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                {
                    ExpenseId = Guid.Empty,
                    ExpenseTypeName = masterExpense.ExpenseType,
                    MasterExpenseId = masterExpense.Id,
                    ActionType = "Activity",
                    Remarks = "All expenses status updated - " + masterExpense.ExpenseNo,//responseData.Result.Name + " Expense Status Updated",
                    Status = "Updated",
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now,
                };
                var response = await _mediator.Send(addExpenseTrackingCommand);
            }

            //SyncMasterExpenseAmountCommand syncMasterExpenseAmountCommand = new SyncMasterExpenseAmountCommand();
            //syncMasterExpenseAmountCommand.Id = id;
            //var responseSync = await _mediator.Send(syncMasterExpenseAmountCommand);
            //return ReturnFormattedResponse(200);

            if (Response > 0)
            {
                dashboardReportData.status = true;
                dashboardReportData.StatusCode = 200;
                //dashboardReportData.Data = result;
            }
            else
            {
                dashboardReportData.status = false;
                dashboardReportData.StatusCode = 500;
                //dashboardReportData.Data = result;
            }
            return Ok(dashboardReportData);

        }


        /// <summary>
        /// Update Expense All Status For Director.
        /// </summary>       
        /// <param name="allUpdateExpenseStatusForDirectorCommand"></param>
        /// <returns></returns>
        [HttpPut("AllUpdateExpenseStatusForDirector")]
        //[ClaimCheck("EXP_UPDATE_EXPENSE")]
        public async Task<IActionResult> AllUpdateExpenseStatusForDirector(AllUpdateExpenseStatusForDirectorCommand allUpdateExpenseStatusForDirectorCommand)
        {
            DashboardReportData dashboardReportData = new DashboardReportData();
            int Response = 0;
            foreach (var mitem in allUpdateExpenseStatusForDirectorCommand.AllMasterExpense)
            {

                foreach (var item in mitem.ExpenseDetailsList)
                {
                    UpdateExpenseStatusCommand updateExpenseStatusCommand = new UpdateExpenseStatusCommand();
                    updateExpenseStatusCommand = item;
                    var result = await _mediator.Send(updateExpenseStatusCommand);
                    if (result.Success)
                    {
                        Response = 1;
                    }
                }

                ////=======================
                UpdateMasterExpenseStatusCommand updateMasterExpenseStatusCommand = new UpdateMasterExpenseStatusCommand();
                updateMasterExpenseStatusCommand.Id = mitem.MasterExpenseId;
                updateMasterExpenseStatusCommand.ApprovalStage = "APPROVED";
                var result1 = await _mediator.Send(updateMasterExpenseStatusCommand);
            }


            //if (Response == 1)
            //{
            //    var responseData = await _expenseRepository.FindAsync(allupdateExpenseStatusCommand.updateExpenseStatus.FirstOrDefault().Id);
            //    var masterExpense = await _masterExpenseRepository.FindAsync(responseData.MasterExpenseId);
            //    var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
            //    var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
            //    {
            //        ExpenseId = Guid.Empty,
            //        ExpenseTypeName = masterExpense.ExpenseType,
            //        MasterExpenseId = masterExpense.Id,
            //        ActionType = "Activity",
            //        Remarks = "All expenses have been successfully updated - " + masterExpense.ExpenseNo,//responseData.Result.Name + " Expense Status Updated",
            //        Status = "Updated",
            //        ActionBy = Guid.Parse(_userInfoToken.Id),
            //        ActionDate = DateTime.Now,
            //    };
            //    var response = await _mediator.Send(addExpenseTrackingCommand);
            //}

            //SyncMasterExpenseAmountCommand syncMasterExpenseAmountCommand = new SyncMasterExpenseAmountCommand();
            //syncMasterExpenseAmountCommand.Id = id;
            //var responseSync = await _mediator.Send(syncMasterExpenseAmountCommand);
            //return ReturnFormattedResponse(200);

            if (Response > 0)
            {
                dashboardReportData.status = true;
                dashboardReportData.StatusCode = 200;
                dashboardReportData.message = "Approved Successfully";
                //dashboardReportData.Data = result;
            }
            else
            {
                dashboardReportData.status = false;
                dashboardReportData.StatusCode = 500;
                //dashboardReportData.Data = result;
            }
            return Ok(dashboardReportData);

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
                string StatusMessage = null, RemarksMessage = null;
                var responseData = await _masterExpenseRepository.AllIncluding(u => u.CreatedByUser).Where(x => x.Id == id).FirstOrDefaultAsync();
                var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));

                if (updateMasterExpenseStatusCommand.Status == "ROLLBACK")
                {
                    StatusMessage = "Rolled-back";
                    RemarksMessage = "Master expense Rolled-back - " + responseData.ExpenseNo;
                }
                else
                {
                    StatusMessage = "Updated";
                    RemarksMessage = "Master expense updated - " + responseData.ExpenseNo;
                }


                var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                {
                    MasterExpenseId = id,
                    ExpenseTypeName = responseData.Name,
                    //ActionType = "Tracker",
                    ActionType = "Activity",
                    Remarks = RemarksMessage,
                    Status = StatusMessage,
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now,
                };
                var response = await _mediator.Send(addExpenseTrackingCommand);


                //**Email Start**
                string email = this._configuration.GetSection("AppSettings")["Email"];
                string expenseRedirectionURL = this._configuration.GetSection("ExpenseRedirection")["ExpenseRedirectionURL"];
                if (email == "Yes")
                {
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Template", "ExpenseStatus.html");
                    var defaultSmtp = await _emailSMTPSettingRepository.FindBy(c => c.IsDefault).FirstOrDefaultAsync();
                    var reportingHead = _userRepository.FindAsync(userResult.ReportingTo.Value).Result;

                    //var accounts = _userRepository.AllIncluding(r => r.UserRoles).Where(r => r.UserRoles.FirstOrDefault().RoleId == Guid.Parse("241772CB-C907-4961-88CB-A0BF8004BBB2")).ToList();
                    //accounts = accounts.Where(x => x.AccountTeam == responseData.AccountTeam).ToList();                   

                    //var accounts = _userRepository.AllIncluding(r => r.UserRoles).Where(u => u.AccountTeamActionFor == responseData.AccountTeam).ToList();

                    // .Where(u => u.AccountTeamActionFor == responseData.AccountTeam).ToList();
                    //&& u.UserRoles.FirstOrDefault(r => r.RoleId == Guid.Parse("241772CB-C907-4961-88CB-A0BF8004BBB2")));

                    //var accountMail = string.Join(",", accounts.Select(x => x.UserName));

                    var accounts = this._configuration.GetSection(responseData.AccountTeam)["UserEmail"];

                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        string templateBody = sr.ReadToEnd();
                        templateBody = templateBody.Replace("{NAME}", string.Concat(responseData.CreatedByUser.FirstName, " ", responseData.CreatedByUser.LastName));
                        templateBody = templateBody.Replace("{DATETIME}", DateTime.Now.ToString("dddd, dd MMMM yyyy"));
                        templateBody = templateBody.Replace("{EXPENSE_NO}", responseData.ExpenseNo);
                        templateBody = templateBody.Replace("{EXPENSE_AMOUNT}", Convert.ToString(responseData.TotalAmount));
                        templateBody = templateBody.Replace("{EXPENSE_TYPE}", Convert.ToString(responseData.ExpenseType));
                        templateBody = templateBody.Replace("{NOOFBILL}", Convert.ToString(responseData.NoOfBill));
                        templateBody = templateBody.Replace("{GROUPEXPENSE}", Convert.ToString(responseData.IsGroupExpense == true ? "Yes" : "No"));
                        templateBody = templateBody.Replace("{NO_OF_PERSON}", Convert.ToString(responseData.NoOfPerson == null ? "1" : responseData.NoOfPerson));
                        templateBody = templateBody.Replace("{EXPENSE_STATUS}", Convert.ToString(responseData.ApprovalStage));

                        templateBody = templateBody.Replace("{WEB_URL}", expenseRedirectionURL + responseData.Id);
                        templateBody = templateBody.Replace("{APP_URL}", expenseRedirectionURL + responseData.Id + "/" + responseData.CreatedBy);

                        if (responseData.TripId != null && responseData.TripId != Guid.Empty)
                        {
                            templateBody = templateBody.Replace("{TOUR_DETAILS}", "Tour Details");
                            templateBody = templateBody.Replace("{MODE_OF_TRIP}", "Mode Of Trip :");
                            templateBody = templateBody.Replace("{MODE_OF_TRIP_VAL}", "Domestic");

                            var itinerary = await _tripItineraryRepository.All.Where(x => x.TripId == responseData.TripId).ToListAsync();
                            var tripData = await _tripRepository.AllIncluding(c => c.CreatedByUser).Where(x => x.Id == responseData.TripId).FirstOrDefaultAsync();
                            string itineraryHtml = ItineraryHtml(itinerary, tripData.TripType);
                            templateBody = templateBody.Replace("{ITINERARY_HTML}", itineraryHtml);

                            var ca = await _companyAccountRepository.FindAsync(tripData.CompanyAccountId.Value);
                            templateBody = templateBody.Replace("{BILLING_COMPANY}", ca.AccountName);
                            templateBody = templateBody.Replace("{DEPARTMENT}", Convert.ToString(tripData.DepartmentName));
                            templateBody = templateBody.Replace("{TRIP_TYPE}", Convert.ToString(tripData.TripType));
                            templateBody = templateBody.Replace("{JOURNEY_PURPOSE}", Convert.ToString(tripData.PurposeFor));
                        }
                        else
                        {
                            templateBody = templateBody.Replace("{TOUR_DETAILS}", "");
                            templateBody = templateBody.Replace("{MODE_OF_TRIP}", "");
                            templateBody = templateBody.Replace("{MODE_OF_TRIP_VAL}", "");
                            templateBody = templateBody.Replace("{ITINERARY_HTML}", "");

                            var ca = await _companyAccountRepository.FindAsync(responseData.CompanyAccountId.Value);
                            templateBody = templateBody.Replace("{BILLING_COMPANY}", ca.AccountName);
                            templateBody = templateBody.Replace("{DEPARTMENT}", "");
                            templateBody = templateBody.Replace("{TRIP_TYPE}", "");
                            templateBody = templateBody.Replace("{JOURNEY_PURPOSE}", "");
                        }

                        EmailHelper.SendEmail(new SendEmailSpecification
                        {
                            Body = templateBody,
                            FromAddress = defaultSmtp.UserName,
                            Host = defaultSmtp.Host,
                            IsEnableSSL = defaultSmtp.IsEnableSSL,
                            Password = defaultSmtp.Password,
                            Port = defaultSmtp.Port,
                            Subject = "Expense Status",
                            ToAddress = responseData.CreatedByUser.UserName,
                            CCAddress = string.IsNullOrEmpty(userResult.AlternateEmail) ?
                            //userResult.UserName :
                            //userResult.UserName + "," + userResult.AlternateEmail,
                            userResult.UserName + "," + accounts :
                            userResult.UserName + "," + accounts + "," + userResult.AlternateEmail,
                            UserName = defaultSmtp.UserName
                        });
                    }

                    MessageRequest userRequest = new MessageRequest()
                    {
                        Body = "Expense status " + responseData.ApprovalStage + " - Expense No. " + responseData.ExpenseNo,
                        CustomKey = "Expense",
                        DeviceToken = userResult.DeviceKey,
                        DeviceType = userResult.IsDeviceTypeAndroid,
                        Id = responseData.CreatedByUser.Id.ToString(),
                        Title = "SFT Travel Desk",
                        UserId = userResult.Id.ToString()
                    };

                    var user = PushNotificationForExpense(userRequest);
                }
                //**Email End**
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
                var responseData = await _expenseRepository.FindAsync(id);
                var masterExpense = await _masterExpenseRepository.FindAsync(responseData.MasterExpenseId);
                var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                {
                    MasterExpenseId = responseData.MasterExpenseId,
                    ExpenseId = id,
                    ExpenseTypeName = masterExpense.ExpenseType,
                    ActionType = "Activity",
                    Remarks = "Expense deleted - " + masterExpense.ExpenseNo,
                    Status = "Deleted",
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
                var responseData = await _masterExpenseRepository.FindAsync(id);
                var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                {
                    MasterExpenseId = responseData.Id,
                    ExpenseTypeName = responseData.ExpenseType,
                    ActionType = "Activity",
                    Remarks = "Master Expense deleted - " + responseData.ExpenseNo,
                    Status = "Deleted",
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
        /// <returns></returns>
        [HttpDelete("DeleteExpenseByDate")]
        //[ClaimCheck("EXP_DELETE_EXPENSE")]
        public async Task<IActionResult> DeleteExpenseByDate(DeleteExpenseByDateCommand deleteExpenseByDateCommand)
        {

            var result = await _mediator.Send(deleteExpenseByDateCommand);

            if (result.Success)
            {
                var responseData = await _masterExpenseRepository.FindAsync(deleteExpenseByDateCommand.Id);
                var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                {
                    MasterExpenseId = responseData.Id,
                    ExpenseTypeName = responseData.Name,
                    ActionType = "Activity",
                    Remarks = "Master expense deleted - " + responseData.ExpenseNo,
                    Status = "Deleted",
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

            if (result.Success)
            {
                var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                {
                    MasterExpenseId = Guid.Empty,
                    ExpenseTypeName = string.Empty,
                    ActionType = "Activity",
                    Remarks = "Travel document uploaded",
                    Status = "Added",
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now,
                };
                var response = await _mediator.Send(addExpenseTrackingCommand);
            }

            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Update  Travel Document
        /// </summary>
        /// <param name="updateTravelDocumentCommand"></param>
        /// <returns></returns>
        [HttpPost("UpdateTravelDocument")]
        //[ClaimCheck("EXP_ADD_EXPENSE")]
        public async Task<IActionResult> UpdateTravelDocument([FromBody] UpdateTravelDocumentCommand updateTravelDocumentCommand)
        {
            var result = await _mediator.Send(updateTravelDocumentCommand);
            if (result.Success)
            {
                var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                {
                    MasterExpenseId = Guid.Empty,
                    ExpenseTypeName = string.Empty,
                    ActionType = "Activity",
                    Remarks = "Travel documents updated",
                    Status = "Updated",
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now,
                };
                var response = await _mediator.Send(addExpenseTrackingCommand);
            }
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Update Status Travel Document
        /// </summary>
        /// <param name="updateTravelDocumentStatusCommand"></param>   
        /// <returns></returns>
        [HttpPost("UpdateTravelDocumentStatus")]
        //[ClaimCheck("EXP_ADD_EXPENSE")]
        public async Task<IActionResult> UpdateTravelDocumentStatus([FromBody] UpdateTravelDocumentStatusCommand updateTravelDocumentStatusCommand)
        {
            var result = await _mediator.Send(updateTravelDocumentStatusCommand);
            if (result.Success)
            {
                var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                {
                    MasterExpenseId = Guid.Empty,
                    ExpenseTypeName = string.Empty,
                    ActionType = "Activity",
                    Remarks = "Travel documents updated",
                    Status = "Updated",
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now,
                };
                var response = await _mediator.Send(addExpenseTrackingCommand);
            }
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Deletes the Travel Document
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete("DeleteTravelDocument/{id}")]
        //[ClaimCheck("EXP_DELETE_EXPENSE")]
        public async Task<IActionResult> DeleteTravelDocument(Guid id)
        {
            var command = new DeleteTravelDocumentCommand() { Id = id };
            var result = await _mediator.Send(command);

            if (result.Success)
            {
                var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                {
                    MasterExpenseId = Guid.Empty,
                    ExpenseTypeName = string.Empty,
                    ActionType = "Activity",
                    Remarks = "Travel documents deleted",
                    Status = "Deleted",
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now,
                };
                var response = await _mediator.Send(addExpenseTrackingCommand);
            }

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
            if (result.Success)
            {
                var masterExpense = await _masterExpenseRepository.FindAsync(addWalletCommand.MasterExpenseId.Value);
                var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                {
                    MasterExpenseId = masterExpense.Id,
                    ExpenseTypeName = masterExpense.ExpenseType,
                    ActionType = "Activity",
                    Remarks = "Amount added in the wallet - " + masterExpense.ExpenseNo,
                    Status = "Added",
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now,
                };
                var response = await _mediator.Send(addExpenseTrackingCommand);
            }
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
                //var masterExpense = await _masterExpenseRepository.FindAsync(updateExpenseAndMasterExpenseCommand.MasterExpenseId.Value);
                var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                //if (updateExpenseAndMasterExpenseCommand.ExpenseId.HasValue)
                //{
                //    var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                //    {
                //        ExpenseId = updateExpenseAndMasterExpenseCommand.ExpenseId.Value,
                //        ActionType = "Activity",
                //        Remarks = "Expense REIMBURSED (Full/Partial/Rejected) - " + masterExpense.ExpenseNo,
                //        Status = "Expense REIMBURSED (Full/Partial/Rejected) - " + masterExpense.ExpenseNo,
                //        ActionBy = Guid.Parse(_userInfoToken.Id),
                //        ActionDate = DateTime.Now,
                //    };
                //    var response = await _mediator.Send(addExpenseTrackingCommand);
                //}

                if (updateExpenseAndMasterExpenseCommand.MasterExpenseId.HasValue)
                {
                    var masterExpense = await _masterExpenseRepository.FindAsync(updateExpenseAndMasterExpenseCommand.MasterExpenseId.Value);
                    var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                    {
                        MasterExpenseId = updateExpenseAndMasterExpenseCommand.MasterExpenseId.Value,
                        ActionType = "Activity",
                        Remarks = "Expense REIMBURSED (Full/Partial/Rejected)  - " + masterExpense.ExpenseNo,
                        Status = "Expense REIMBURSED (Full/Partial/Rejected)  - " + masterExpense.ExpenseNo,
                        ActionBy = Guid.Parse(_userInfoToken.Id),
                        ActionDate = DateTime.Now,
                    };
                    var response = await _mediator.Send(addExpenseTrackingCommand);
                }

                //**Email Start**
                if (updateExpenseAndMasterExpenseCommand.MasterExpenseId.HasValue)
                {
                    string email = this._configuration.GetSection("AppSettings")["Email"];
                    string expenseRedirectionURL = this._configuration.GetSection("ExpenseRedirection")["ExpenseRedirectionURL"];
                    if (email == "Yes")
                    {
                        var responseData = await _masterExpenseRepository.AllIncluding(u => u.CreatedByUser).Where(x => x.Id == updateExpenseAndMasterExpenseCommand.MasterExpenseId.Value).FirstOrDefaultAsync();

                        var itinerary = await _tripItineraryRepository.All.Where(x => x.TripId == responseData.TripId).ToListAsync();

                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Template", itinerary.Count == 0 ? "ExpenseReimburse.html" : "ExpenseReimburseWithTrip.html");
                        var defaultSmtp = await _emailSMTPSettingRepository.FindBy(c => c.IsDefault).FirstOrDefaultAsync();
                        var reportingHead = _userRepository.FindAsync(userResult.ReportingTo.Value).Result;

                        var accounts = _userRepository.AllIncluding(r => r.UserRoles).Where(u => u.AccountTeamActionFor == responseData.AccountTeam).ToList();
                        //&& u.UserRoles.FirstOrDefault(r => r.RoleId == Guid.Parse("241772CB-C907-4961-88CB-A0BF8004BBB2")));
                        var accountMail = string.Join(",", accounts.Select(x => x.UserName));

                        using (StreamReader sr = new StreamReader(filePath))
                        {
                            string templateBody = sr.ReadToEnd();
                            templateBody = templateBody.Replace("{NAME}", string.Concat(responseData.CreatedByUser.FirstName, " ", responseData.CreatedByUser.LastName));
                            templateBody = templateBody.Replace("{DATETIME}", DateTime.Now.ToString("dddd, dd MMMM yyyy"));
                            templateBody = templateBody.Replace("{EXPENSE_NO}", responseData.ExpenseNo);
                            templateBody = templateBody.Replace("{REIMBURMENT_STATUS}", Convert.ToString(responseData.ReimbursementStatus));
                            templateBody = templateBody.Replace("{TOTAL_AMOUNT}", Convert.ToString(responseData.TotalAmount));
                            templateBody = templateBody.Replace("{PAYABLE_AMOUNT}", Convert.ToString(responseData.PayableAmount));
                            templateBody = templateBody.Replace("{REIMBURSED_AMOUNT}", Convert.ToString(responseData.ReimbursementAmount));
                            templateBody = templateBody.Replace("{EXPENSE_TYPE}", Convert.ToString(responseData.ExpenseType));
                            templateBody = templateBody.Replace("{NOOFBILL}", Convert.ToString(responseData.NoOfBill));

                            templateBody = templateBody.Replace("{WEB_URL}", expenseRedirectionURL + responseData.Id);
                            templateBody = templateBody.Replace("{APP_URL}", expenseRedirectionURL + responseData.Id + "/" + responseData.CreatedBy);


                            if (responseData.TripId != null && responseData.TripId != Guid.Empty)
                            {
                                templateBody = templateBody.Replace("{TOUR_DETAILS}", "Tour Details");
                                templateBody = templateBody.Replace("{MODE_OF_TRIP}", "Mode Of Trip :");
                                templateBody = templateBody.Replace("{MODE_OF_TRIP_VAL}", "Domestic");


                                var tripData = await _tripRepository.AllIncluding(c => c.CreatedByUser).Where(x => x.Id == responseData.TripId).FirstOrDefaultAsync();
                                string itineraryHtml = ItineraryHtml(itinerary, tripData.TripType);
                                templateBody = templateBody.Replace("{ITINERARY_HTML}", itineraryHtml);

                                var ca = await _companyAccountRepository.FindAsync(tripData.CompanyAccountId.Value);
                                templateBody = templateBody.Replace("{BILLING_COMPANY}", ca.AccountName);
                                templateBody = templateBody.Replace("{DEPARTMENT}", Convert.ToString(tripData.DepartmentName));
                                templateBody = templateBody.Replace("{TRIP_TYPE}", Convert.ToString(tripData.TripType));
                                templateBody = templateBody.Replace("{JOURNEY_PURPOSE}", Convert.ToString(tripData.PurposeFor));
                            }
                            else
                            {
                                templateBody = templateBody.Replace("{TOUR_DETAILS}", "");
                                templateBody = templateBody.Replace("{MODE_OF_TRIP}", "");
                                templateBody = templateBody.Replace("{MODE_OF_TRIP_VAL}", "");
                                templateBody = templateBody.Replace("{ITINERARY_HTML}", "");

                                var ca = await _companyAccountRepository.FindAsync(responseData.CompanyAccountId.Value);
                                templateBody = templateBody.Replace("{BILLING_COMPANY}", ca.AccountName);
                                templateBody = templateBody.Replace("{DEPARTMENT}", "");
                                templateBody = templateBody.Replace("{TRIP_TYPE}", "");
                                templateBody = templateBody.Replace("{JOURNEY_PURPOSE}", "");
                            }


                            EmailHelper.SendEmail(new SendEmailSpecification
                            {
                                Body = templateBody,
                                FromAddress = defaultSmtp.UserName,
                                Host = defaultSmtp.Host,
                                IsEnableSSL = defaultSmtp.IsEnableSSL,
                                Password = defaultSmtp.Password,
                                Port = defaultSmtp.Port,
                                Subject = "Expense Reimbursed Status",
                                ToAddress = responseData.CreatedByUser.UserName,
                                CCAddress = string.IsNullOrEmpty(userResult.AlternateEmail) ?
                                userResult.UserName + "," + accountMail :
                                userResult.UserName + "," + userResult.AlternateEmail,
                                UserName = defaultSmtp.UserName
                            });
                        }

                        MessageRequest userRequest = new MessageRequest()
                        {
                            Body = "Expense Reimbursed status " + responseData.ReimbursementStatus + " - Expense No. " + responseData.ExpenseNo,
                            CustomKey = "Expense",
                            DeviceToken = userResult.DeviceKey,
                            DeviceType = userResult.IsDeviceTypeAndroid,
                            Id = responseData.CreatedByUser.Id.ToString(),
                            Title = "SFT Travel Desk",
                            UserId = userResult.Id.ToString()
                        };

                        var user = PushNotificationForExpense(userRequest);
                    }
                }
                //**Email End**


            }
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Update Expense And Master Expense.
        /// </summary>
        /// <param name="updateAllExpenseAndMasterExpenseApprovalLevelCommand"></param>
        /// <returns></returns>
        [HttpPut("UpdateAllExpenseAndMasterExpenseApprovalLevel")]
        //[ClaimCheck("EXP_MSTR_EXP_UPDATE_EXPENSE")]
        public async Task<IActionResult> UpdateAllExpenseAndMasterExpenseApprovalLevel([FromBody] UpdateAllExpenseAndMasterExpenseApprovalLevelCommand updateAllExpenseAndMasterExpenseApprovalLevelCommand)
        {
            var userResult = _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id)).Result;

            int approvalcheck = updateAllExpenseAndMasterExpenseApprovalLevelCommand.
                AllExpenseAndMasterExpenseApprovalLevelCommand.Count(x => x.AccountStatus == "APPROVED");

            string approvalCheck = approvalcheck > 0 ? "APPROVED" : "REJECTED";

            decimal reimbursementAmount = updateAllExpenseAndMasterExpenseApprovalLevelCommand.
                AllExpenseAndMasterExpenseApprovalLevelCommand.Sum(x => x.ReimbursementAmount);

            BTTEM.Data.Entities.ResponseData response = new BTTEM.Data.Entities.ResponseData();
            int Response = 0;
            UpdateExpenseAndMasterExpenseApprovalLevelCommand updateExpenseAndMasterExpenseApprovalLevel = new UpdateExpenseAndMasterExpenseApprovalLevelCommand();
            foreach (var expenseAndMasterExpenseApprovalLevel in updateAllExpenseAndMasterExpenseApprovalLevelCommand.AllExpenseAndMasterExpenseApprovalLevelCommand)
            {
                updateExpenseAndMasterExpenseApprovalLevel = expenseAndMasterExpenseApprovalLevel;
                updateExpenseAndMasterExpenseApprovalLevel.LevelReimbursementAmount = reimbursementAmount;
                updateExpenseAndMasterExpenseApprovalLevel.checkApproval = approvalCheck;
                var result = await _mediator.Send(updateExpenseAndMasterExpenseApprovalLevel);
                if (result.Success)
                {
                    Response = 1;
                }
            }
            if (Response > 0)
            {
                response.status = true;
                response.StatusCode = 200;
                //dashboardReportData.Data = result;

                //string email = this._configuration.GetSection("AppSettings")["Email"];
                //if (email == "Yes")
                //{
                //    var responseData = await _masterExpenseRepository.AllIncluding(u => u.CreatedByUser).Where(x => x.Id == updateAllExpenseAndMasterExpenseApprovalLevelCommand.AllExpenseAndMasterExpenseApprovalLevelCommand.FirstOrDefault().MasterExpenseId).FirstOrDefaultAsync();
                //    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Template", "ExpenseReimburse.html");
                //    var defaultSmtp = await _emailSMTPSettingRepository.FindBy(c => c.IsDefault).FirstOrDefaultAsync();
                //    var reportingHead = _userRepository.FindAsync(userResult.ReportingTo.Value).Result;

                //    using (StreamReader sr = new StreamReader(filePath))
                //    {
                //        string templateBody = sr.ReadToEnd();
                //        templateBody = templateBody.Replace("{NAME}", string.Concat(userResult.FirstName, " ", userResult.LastName));
                //        templateBody = templateBody.Replace("{DATETIME}", DateTime.Now.ToString("dddd, dd MMMM yyyy"));
                //        templateBody = templateBody.Replace("{EXPENSE_NO}", responseData.ExpenseNo);
                //        templateBody = templateBody.Replace("{REIMBURMENT_STATUS}", Convert.ToString(responseData.ReimbursementStatus));
                //        templateBody = templateBody.Replace("{TOTAL_AMOUNT}", Convert.ToString(responseData.TotalAmount));
                //        templateBody = templateBody.Replace("{PAYABLE_AMOUNT}", Convert.ToString(responseData.PayableAmount));
                //        templateBody = templateBody.Replace("{REIMBURSED_AMOUNT}", Convert.ToString(responseData.ReimbursementAmount));
                //        EmailHelper.SendEmail(new SendEmailSpecification
                //        {
                //            Body = templateBody,
                //            FromAddress = defaultSmtp.UserName,
                //            Host = defaultSmtp.Host,
                //            IsEnableSSL = defaultSmtp.IsEnableSSL,
                //            Password = defaultSmtp.Password,
                //            Port = defaultSmtp.Port,
                //            Subject = "Expense Reimbursed Status",
                //            ToAddress = responseData.CreatedByUser.UserName,
                //            CCAddress = userResult.UserName,
                //            UserName = defaultSmtp.UserName
                //        });
                //    }
                //}

                //**Email End**



                //**Email Start**
                if (userResult.CompanyAccountId == new Guid("d0ccea5f-5393-4a34-9df6-43a9f51f9f91"))
                {
                    string email = this._configuration.GetSection("AppSettings")["Email"];
                    string expenseRedirectionURL = this._configuration.GetSection("ExpenseRedirection")["ExpenseRedirectionURL"];

                    if (email == "Yes")
                    {
                        var responseData = await _masterExpenseRepository.AllIncluding(u => u.CreatedByUser).Where(x => x.Id ==
                        updateAllExpenseAndMasterExpenseApprovalLevelCommand.AllExpenseAndMasterExpenseApprovalLevelCommand.FirstOrDefault()
                        .MasterExpenseId).FirstOrDefaultAsync();
                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Template", "ExpenseStatus.html");
                        var defaultSmtp = await _emailSMTPSettingRepository.FindBy(c => c.IsDefault).FirstOrDefaultAsync();
                        var reportingHead = _userRepository.FindAsync(userResult.ReportingTo.Value).Result;

                        string accountsEmail = string.Empty;
                        accountsEmail = responseData.AccountsApprovalStage.Value == 3 ? "nandan.mandal@shyaminfra.com" :
                            responseData.AccountsApprovalStage.Value == 2 ? "rajesh.yadav@shyaminfra.com" :
                            responseData.AccountsApprovalStage.Value == 1 ? "nandan.mandal@shyaminfra.com" : "";

                        string ccMail = string.Empty;

                        if (string.IsNullOrEmpty(accountsEmail))
                        {
                            if (string.IsNullOrEmpty(userResult.AlternateEmail))
                            {
                                ccMail = userResult.UserName;
                            }
                            else
                            {
                                ccMail = userResult.UserName + "," + userResult.AlternateEmail;
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(userResult.AlternateEmail))
                            {
                                ccMail = userResult.UserName + "," + accountsEmail;
                            }
                            else
                            {
                                ccMail = userResult.UserName + "," + userResult.AlternateEmail + "," + accountsEmail;
                            }
                        }

                        using (StreamReader sr = new StreamReader(filePath))
                        {
                            string templateBody = sr.ReadToEnd();
                            templateBody = templateBody.Replace("{NAME}", string.Concat(responseData.CreatedByUser.FirstName, " ", responseData.CreatedByUser.LastName));
                            templateBody = templateBody.Replace("{DATETIME}", DateTime.Now.ToString("dddd, dd MMMM yyyy"));
                            templateBody = templateBody.Replace("{EXPENSE_NO}", responseData.ExpenseNo);
                            templateBody = templateBody.Replace("{EXPENSE_AMOUNT}", Convert.ToString(responseData.TotalAmount));
                            templateBody = templateBody.Replace("{EXPENSE_TYPE}", Convert.ToString(responseData.ExpenseType));
                            templateBody = templateBody.Replace("{NOOFBILL}", Convert.ToString(responseData.NoOfBill));
                            templateBody = templateBody.Replace("{GROUPEXPENSE}", Convert.ToString(responseData.IsGroupExpense == true ? "Yes" : "No"));
                            templateBody = templateBody.Replace("{NO_OF_PERSON}", Convert.ToString(responseData.NoOfPerson == null ? "1" : responseData.NoOfPerson));
                            templateBody = templateBody.Replace("{EXPENSE_STATUS}", Convert.ToString(responseData.ApprovalStage));

                            templateBody = templateBody.Replace("{WEB_URL}", expenseRedirectionURL + responseData.Id);
                            templateBody = templateBody.Replace("{APP_URL}", expenseRedirectionURL + responseData.Id + "/" + responseData.CreatedBy);


                            if (responseData.TripId != null && responseData.TripId != Guid.Empty)
                            {
                                templateBody = templateBody.Replace("{TOUR_DETAILS}", "Tour Details");
                                templateBody = templateBody.Replace("{MODE_OF_TRIP}", "Mode Of Trip :");
                                templateBody = templateBody.Replace("{MODE_OF_TRIP_VAL}", "Domestic");

                                var itinerary = await _tripItineraryRepository.All.Where(x => x.TripId == responseData.TripId).ToListAsync();
                                var tripData = await _tripRepository.AllIncluding(c => c.CreatedByUser).Where(x => x.Id == responseData.TripId).FirstOrDefaultAsync();
                                string itineraryHtml = ItineraryHtml(itinerary, tripData.TripType);
                                templateBody = templateBody.Replace("{ITINERARY_HTML}", itineraryHtml);

                                var ca = await _companyAccountRepository.FindAsync(tripData.CompanyAccountId.Value);
                                templateBody = templateBody.Replace("{BILLING_COMPANY}", ca.AccountName);
                                templateBody = templateBody.Replace("{DEPARTMENT}", Convert.ToString(tripData.DepartmentName));
                                templateBody = templateBody.Replace("{TRIP_TYPE}", Convert.ToString(tripData.TripType));
                                templateBody = templateBody.Replace("{JOURNEY_PURPOSE}", Convert.ToString(tripData.PurposeFor));
                            }
                            else
                            {
                                templateBody = templateBody.Replace("{TOUR_DETAILS}", "");
                                templateBody = templateBody.Replace("{MODE_OF_TRIP}", "");
                                templateBody = templateBody.Replace("{MODE_OF_TRIP_VAL}", "");
                                templateBody = templateBody.Replace("{ITINERARY_HTML}", "");

                                var ca = await _companyAccountRepository.FindAsync(responseData.CompanyAccountId.Value);
                                templateBody = templateBody.Replace("{BILLING_COMPANY}", ca.AccountName);
                                templateBody = templateBody.Replace("{DEPARTMENT}", "");
                                templateBody = templateBody.Replace("{TRIP_TYPE}", "");
                                templateBody = templateBody.Replace("{JOURNEY_PURPOSE}", "");
                            }

                            EmailHelper.SendEmail(new SendEmailSpecification
                            {
                                Body = templateBody,
                                FromAddress = defaultSmtp.UserName,
                                Host = defaultSmtp.Host,
                                IsEnableSSL = defaultSmtp.IsEnableSSL,
                                Password = defaultSmtp.Password,
                                Port = defaultSmtp.Port,
                                Subject = "Expense Status",
                                ToAddress = responseData.CreatedByUser.UserName,
                                //CCAddress = string.IsNullOrEmpty(userResult.AlternateEmail) ?
                                //    userResult.UserName :
                                //    userResult.UserName + "," + userResult.AlternateEmail,
                                CCAddress = ccMail,
                                UserName = defaultSmtp.UserName
                            });
                        }


                        MessageRequest userRequest = new MessageRequest()
                        {
                            Body = "Expense Reimbursed status " + responseData.ApprovalStage + " - Expense No. " + responseData.ExpenseNo,
                            CustomKey = "Expense",
                            DeviceToken = userResult.DeviceKey,
                            DeviceType = userResult.IsDeviceTypeAndroid,
                            Id = responseData.CreatedByUser.Id.ToString(),
                            Title = "SFT Travel Desk",
                            UserId = userResult.Id.ToString()
                        };

                        var user = PushNotificationForExpense(userRequest);
                    }
                }
                //**Email End**

            }
            else
            {
                response.status = false;
                response.StatusCode = 500;
                //dashboardReportData.Data = result;
            }
            return Ok(response);
            //return ReturnFormattedResponse(result);
        }



        /// <summary>
        /// All  Update Expense And Master Expense.
        /// </summary>
        /// <param name="allAccountUpdateExpenseAndMasterExpenseCommand"></param>
        /// <returns></returns>
        [HttpPut("AllAccountUpdateExpenseAndMasterExpense")]
        //[ClaimCheck("EXP_MSTR_EXP_UPDATE_EXPENSE")]
        public async Task<IActionResult> AllAccountUpdateExpenseAndMasterExpense(AllAccountUpdateExpenseAndMasterExpenseCommand allAccountUpdateExpenseAndMasterExpenseCommand)
        {
            DashboardReportData dashboardReportData = new DashboardReportData();
            int Response = 0;
            foreach (var item in allAccountUpdateExpenseAndMasterExpenseCommand.AllUpdateExpenseAndMasterExpense)
            {
                UpdateExpenseAndMasterExpenseCommand updateExpenseAndMasterExpenseCommand = new UpdateExpenseAndMasterExpenseCommand();
                updateExpenseAndMasterExpenseCommand = item;
                var result = await _mediator.Send(updateExpenseAndMasterExpenseCommand);
                if (result.Success)
                {
                    Response = 1;
                }
            }

            if (Response > 0)
            {
                dashboardReportData.status = true;
                dashboardReportData.StatusCode = 200;
                //dashboardReportData.Data = result;
            }
            else
            {
                dashboardReportData.status = false;
                dashboardReportData.StatusCode = 500;
                //dashboardReportData.Data = result;
            }
            return Ok(dashboardReportData);

            //return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Add Expense Tracking
        /// </summary>
        /// <param name="addExpenseTrackingCommand"></param>
        /// <returns></returns>
        [HttpPost("AddExpenseTracking")]
        [Produces("application/json", "application/xml", Type = typeof(ExpenseTrackingDto))]
        public async Task<IActionResult> AddExpenseTracking([FromBody] AddExpenseTrackingCommand addExpenseTrackingCommand)
        {
            var result = await _mediator.Send(addExpenseTrackingCommand);
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
                //var userResult = _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id)).Result;
                //var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                //{
                //    ExpenseId = result.Data.Id,
                //    //MasterExpenseId = result.Data.MasterExpenseId.Value,
                //    ExpenseTypeName = addTravelDeskExpenseCommand.Name,
                //    ActionType = "Activity",
                //    Remarks = addTravelDeskExpenseCommand.Name + " Booking File Uploaded by Travel Desk - " + userResult.FirstName + " " + userResult.LastName,
                //    Status = "Booking File Uploaded by Travel Desk " + userResult.FirstName + " " + userResult.LastName,
                //    ActionBy = Guid.Parse(_userInfoToken.Id),
                //    ActionDate = DateTime.Now,
                //};
                //var response = await _mediator.Send(addExpenseTrackingCommand);
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
                //var userResult = _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id)).Result;
                //var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                //{
                //    ExpenseId = updateTravelDeskExpenseCommand.Id,
                //    ExpenseTypeName = updateTravelDeskExpenseCommand.Name,
                //    ActionType = "Activity",
                //    Remarks = updateTravelDeskExpenseCommand.Name + " Booking File Re-Uploaded by Travel Desk - " + userResult.FirstName + " " + userResult.LastName,
                //    Status = "Booking File Re-Uploaded by Travel Desk - " + userResult.FirstName + " " + userResult.LastName,
                //    ActionBy = Guid.Parse(_userInfoToken.Id),
                //    ActionDate = DateTime.Now,
                //};
                //var response = await _mediator.Send(addExpenseTrackingCommand);
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


        /// <summary>
        /// Get All Master Expenses Group 
        /// </summary>
        /// <param name="masterExpenseResourceGroupWise"></param>
        /// <returns></returns>
        [HttpGet("GetAllExpensesDetailsListGroupWise")]
        //[ClaimCheck("EXP_VIEW_EXPENSES")]
        public async Task<IActionResult> GetAllExpensesDetailsListGroupWise([FromQuery] ExpenseResource masterExpenseResourceGroupWise)
        {
            var masterExpensesDetails = _masterExpenseRepository.All.Where(a => a.Id == masterExpenseResourceGroupWise.MasterExpenseId).FirstOrDefault();
            if (masterExpensesDetails != null)
            {
                var UserId = masterExpensesDetails.CreatedBy;
            }
            var getUserGradeAndAccountCommand = new GetUserGradeAndAccountCommand
            {
                UserId = masterExpensesDetails.CreatedBy //result.Data.CreatedByUser.Id, 
            };
            var resultUser = await _mediator.Send(getUserGradeAndAccountCommand);
            PoliciesDetailResource policiesDetailResourceQuery = new PoliciesDetailResource
            {
                CompanyAccountId = resultUser.CompanyAccountId,
                GradeId = resultUser.GradeId,
            };

            //PoliciesDetail
            var getAllPoliciesDetailCommand = new GetAllPoliciesDetailCommand
            {
                PoliciesDetailResource = policiesDetailResourceQuery
            };
            var resultPoliciesDetail = await _mediator.Send(getAllPoliciesDetailCommand);
            if (resultPoliciesDetail.Count == 0)
            {
                return NotFound("Policies not mapped with user.");
            }

            //Policies Lodging Fooding
            var getAllPoliciesLodgingFoodingCommand = new GetAllPoliciesLodgingFoodingCommand
            {
                Id = resultPoliciesDetail.FirstOrDefault().Id
            };
            var resultPoliciesLodgingFooding = await _mediator.Send(getAllPoliciesLodgingFoodingCommand);

            //Conveyance
            var getAllConveyanceCommand = new GetAllConveyanceCommand
            {
                Id = resultPoliciesDetail.FirstOrDefault().Id
            };
            var resultConveyance = await _mediator.Send(getAllConveyanceCommand);

            //PoliciesVehicleConveyance
            var getAlllPoliciesVehicleConveyanceCommand = new GetAllPoliciesVehicleConveyanceCommand
            {
                Id = resultPoliciesDetail.FirstOrDefault().Id
            };
            var resultlPoliciesVehicleConveyance = await _mediator.Send(getAlllPoliciesVehicleConveyanceCommand);

            //PoliciesSetting
            var getAllPoliciesSettingCommand = new GetAllPoliciesSettingCommand
            {
                Id = resultPoliciesDetail.FirstOrDefault().Id
            };
            var resultPoliciesSetting = await _mediator.Send(getAllPoliciesSettingCommand);

            var getAllMasterExpenseQuery = new GetAllMasterExpenseQuery
            {
                ExpenseResource = masterExpenseResourceGroupWise
            };

            var result = await _mediator.Send(getAllMasterExpenseQuery);

            if (result.Count() == 0)
            {
                ExpenseResponseData returnObject = new ExpenseResponseData();
                return Ok(returnObject);
            }

            var expenseCategory = _expenseCategoryRepository.All.ToList();

            ExpenseResponseData responseData = new ExpenseResponseData();
            responseData.MaseterExpense = result.FirstOrDefault();

            int noOfDays = 1;
            if (masterExpensesDetails.ExpenseType == "Local Trip")
            {
                var ExpenseDetailsList = _expenseRepository.All.Where(a => a.MasterExpenseId == masterExpenseResourceGroupWise.MasterExpenseId && a.Amount > 0).GroupBy(a => a.ExpenseDate).ToList();
                if (ExpenseDetailsList.Count > 0)
                {
                    //var FirstDate = ExpenseDetailsList.First().ExpenseDate;
                    //var LastDate = ExpenseDetailsList.Last().ExpenseDate;
                    //noOfDays = (int)(LastDate - FirstDate).TotalDays + 1;
                    noOfDays = ExpenseDetailsList.Count();
                }
            }
            else
            {
                var tripDetails = _tripRepository.FindAsync(responseData.MaseterExpense.TripId.Value);
                noOfDays = (int)(tripDetails.Result.TripEnds - tripDetails.Result.TripStarts).TotalDays + 1;
            }


            foreach (var item in expenseCategory)
            {
                responseData.ExpenseCategories.Add(new ExpenseCategoryData()
                {
                    ExpenseCategoryId = item.Id,
                    ExpenseCategoryName = item.Name,
                    //ExpenseDtos = new List<ExpenseDto>() { item },
                });
            }

            foreach (var item in responseData.ExpenseCategories)
            {
                foreach (var expense in result)
                {
                    var expenseData = expense.Expenses.Where(x => x.ExpenseCategoryId == item.ExpenseCategoryId && x.Amount > 0);
                    //--Lodging (Metro City)
                    if (item.ExpenseCategoryId == new Guid("FBF965BD-A53E-4D97-978A-34C2007202E5"))
                    {
                        int localNoOfDays = 1;
                        var ExpenseDetailsList = _expenseRepository.All.Where(a => a.Amount > 0 && a.ExpenseCategoryId == new Guid("FBF965BD-A53E-4D97-978A-34C2007202E5") && a.MasterExpenseId == masterExpenseResourceGroupWise.MasterExpenseId).GroupBy(a => a.ExpenseDate).ToList();
                        if (ExpenseDetailsList != null)
                        {
                            localNoOfDays = ExpenseDetailsList.Count();
                        }
                        item.AllowedAmount = resultPoliciesLodgingFooding.MetroCitiesUptoAmount * Convert.ToDecimal(localNoOfDays);
                        item.ExpenseAmount = expenseData.Sum(x => x.Amount);
                        decimal DeviationAmount = 0;
                        foreach (var itm in expenseData)
                        {
                            decimal LoclDeviationAmount = 0;
                            if (itm.Amount > resultPoliciesLodgingFooding.MetroCitiesUptoAmount)
                            {
                                LoclDeviationAmount = (itm.Amount - resultPoliciesLodgingFooding.MetroCitiesUptoAmount);
                                DeviationAmount = DeviationAmount + LoclDeviationAmount;
                            }
                        }
                        item.DeviationAmount = DeviationAmount;
                    }
                    //-- Lodging (Other City)
                    if (item.ExpenseCategoryId == new Guid("1AADD03D-90E1-4589-8B9D-6121049B490D"))
                    {
                        int localNoOfDays = 1;
                        var ExpenseDetailsList = _expenseRepository.All.Where(a => a.Amount > 0 && a.ExpenseCategoryId == new Guid("1AADD03D-90E1-4589-8B9D-6121049B490D") && a.MasterExpenseId == masterExpenseResourceGroupWise.MasterExpenseId).GroupBy(a => a.ExpenseDate).ToList();
                        if (ExpenseDetailsList != null)
                        {
                            localNoOfDays = ExpenseDetailsList.Count();
                        }
                        item.AllowedAmount = resultPoliciesLodgingFooding.OtherCitiesUptoAmount * Convert.ToDecimal(localNoOfDays);
                        item.ExpenseAmount = expenseData.Sum(x => x.Amount);
                        decimal DeviationAmount = 0;
                        foreach (var itm in expenseData)
                        {
                            decimal LoclDeviationAmount = 0;
                            if (itm.Amount > resultPoliciesLodgingFooding.OtherCitiesUptoAmount)
                            {
                                LoclDeviationAmount = (itm.Amount - resultPoliciesLodgingFooding.OtherCitiesUptoAmount);
                                DeviationAmount = DeviationAmount + LoclDeviationAmount;
                            }
                        }
                        item.DeviationAmount = DeviationAmount;
                    }
                    //--MISC /DA
                    if (item.ExpenseCategoryId == new Guid("ED69E9A0-2D54-4A91-A598-F79973B9FE99"))
                    {
                        item.AllowedAmount = resultPoliciesDetail.FirstOrDefault().DailyAllowance * Convert.ToDecimal(noOfDays);
                        item.ExpenseAmount = expenseData.Sum(x => x.Amount);
                        decimal DeviationAmount = 0;
                        foreach (var itm in expenseData)
                        {
                            decimal LoclDeviationAmount = 0;
                            if (itm.Amount > resultPoliciesDetail.FirstOrDefault().DailyAllowance)
                            {
                                LoclDeviationAmount = (itm.Amount - resultPoliciesDetail.FirstOrDefault().DailyAllowance);
                                DeviationAmount = DeviationAmount + LoclDeviationAmount;
                            }
                        }
                        item.DeviationAmount = DeviationAmount;
                    }
                    //--Fooding Allowance
                    if (item.ExpenseCategoryId == new Guid("BB0BF3AA-1FD9-4F1C-9FDE-8498073C58A9"))
                    {
                        int localNoOfDays = 1;
                        var ExpenseDetailsList = _expenseRepository.All.Where(a => a.Amount > 0 && a.ExpenseCategoryId == new Guid("BB0BF3AA-1FD9-4F1C-9FDE-8498073C58A9") && a.MasterExpenseId == masterExpenseResourceGroupWise.MasterExpenseId).GroupBy(a => a.ExpenseDate).ToList();
                        if (ExpenseDetailsList != null)
                        {
                            localNoOfDays = ExpenseDetailsList.Count();
                        }
                        item.AllowedAmount = resultPoliciesLodgingFooding.BudgetAmount * Convert.ToDecimal(localNoOfDays);
                        item.ExpenseAmount = expenseData.Sum(x => x.Amount);

                        if (item.ExpenseAmount >= item.AllowedAmount)
                        {
                            item.FoodingAllowance = resultPoliciesLodgingFooding.BudgetAmount;
                        }
                    }
                    //--Conveyance (within a City)
                    if (item.ExpenseCategoryId == new Guid("B1977DB3-D909-4936-A5DA-41BF84638963"))
                    {
                        var Conveyance = resultConveyance.Where(a => a.Name == "Conveyance (within a city)");
                        if (Conveyance != null)
                        {
                            var ConveyancesItemAll = Conveyance.Select(a => a.conveyancesItem).Where(b => b.Any(a => a.ConveyanceItemName == "Budget")).FirstOrDefault();
                            var ConveyancesItem = ConveyancesItemAll.Where(a => a.ConveyanceItemName == "Budget");
                            if (ConveyancesItem != null)
                            {
                                bool IsCheck = (bool)ConveyancesItem.FirstOrDefault().IsCheck;
                                if (IsCheck == true)
                                {
                                    if (ConveyancesItem.FirstOrDefault().Amount != null)
                                    {
                                        item.AllowedAmount = (decimal)(ConveyancesItem.FirstOrDefault().Amount * Convert.ToDecimal(noOfDays));
                                    }
                                }
                                else
                                {
                                    item.AllowedAmount = expenseData.Sum(x => x.Amount);
                                }

                                item.ExpenseAmount = expenseData.Sum(x => x.Amount);
                                if (item.ExpenseAmount >= item.AllowedAmount)
                                {
                                    item.DeviationAmount = item.ExpenseAmount - item.AllowedAmount;
                                }
                            }
                        }
                    }
                    //--Conveyance (city to outer area)
                    if (item.ExpenseCategoryId == new Guid("5278397A-C8DD-475A-A7A7-C05708B2BB06"))
                    {
                        var Conveyance = resultConveyance.Where(a => a.Name == "Conveyance (city to outer area)");
                        if (Conveyance != null)
                        {
                            var ConveyancesItemAll = Conveyance.Select(a => a.conveyancesItem).Where(b => b.Any(a => a.ConveyanceItemName == "Budget")).FirstOrDefault();
                            var ConveyancesItem = ConveyancesItemAll.Where(a => a.ConveyanceItemName == "Budget");
                            if (ConveyancesItem != null)
                            {
                                bool IsCheck = (bool)ConveyancesItem.FirstOrDefault().IsCheck;
                                if (IsCheck == true)
                                {
                                    if (ConveyancesItem.FirstOrDefault().Amount != null)
                                    {
                                        item.AllowedAmount = ConveyancesItem.FirstOrDefault().Amount.Value * Convert.ToDecimal(noOfDays);
                                    }
                                }
                                else
                                {
                                    item.AllowedAmount = expenseData.Sum(x => x.Amount);
                                }
                            }

                            item.ExpenseAmount = expenseData.Sum(x => x.Amount);
                            if (item.ExpenseAmount >= item.AllowedAmount)
                            {
                                item.DeviationAmount = item.ExpenseAmount - item.AllowedAmount;
                            }

                        }
                    }

                    //--Fare
                    if (item.ExpenseCategoryId == new Guid("DCAA05B6-5F1E-402F-835E-0704A3A1A455"))
                    {
                        item.AllowedAmount = expenseData.Sum(x => x.Amount);
                        item.ExpenseAmount = expenseData.Sum(x => x.Amount);
                        if (item.ExpenseAmount >= item.AllowedAmount)
                        {
                            item.DeviationAmount = item.ExpenseAmount - item.AllowedAmount;
                        }
                    }
                    //--Others
                    if (item.ExpenseCategoryId == new Guid("6C3EB31C-DF53-495A-B871-E2EB3CEF74D2"))
                    {
                        item.AllowedAmount = expenseData.Sum(x => x.Amount);
                        item.ExpenseAmount = expenseData.Sum(x => x.Amount);
                        if (item.ExpenseAmount >= item.AllowedAmount)
                        {
                            item.DeviationAmount = item.ExpenseAmount - item.AllowedAmount;
                        }
                    }
                    if (expenseData != null)
                    {
                        //decimal DeviationAmount = 0;
                        //item.ExpenseAmount = expenseData.Sum(x => x.Amount);
                        //if (item.ExpenseAmount >= item.AllowedAmount)
                        //{
                        //    item.DeviationAmount = item.ExpenseAmount - item.AllowedAmount;
                        //}
                        item.ExpenseDtos.AddRange(expenseData);
                        //foreach (var itm in expenseData)
                        //{
                        //    decimal LoclDeviationAmount = 0;
                        //    if (itm.Amount > resultPoliciesLodgingFooding.MetroCitiesUptoAmount)
                        //    {
                        //        LoclDeviationAmount = (itm.Amount - resultPoliciesLodgingFooding.MetroCitiesUptoAmount);
                        //        DeviationAmount = DeviationAmount + LoclDeviationAmount;
                        //    }

                        //}
                        //item.DeviationAmount = DeviationAmount;
                    }
                }
            }

            responseData.MaseterExpense.NoOfPendingAction = result.FirstOrDefault().Expenses
            .Where(x => x.Amount > 0 && x.Status == "PENDING" || x.Status == null || x.Status == string.Empty).Count();

            responseData.MaseterExpense.NoOfPendingReimbursementAction = result.FirstOrDefault().Expenses
            .Where(x => x.Amount > 0 && x.Status == "APPROVED" && x.AccountStatus == null || x.AccountStatus == string.Empty || x.AccountStatus == "PENDING").Count();

            responseData.ExpenseCategories.ForEach(item =>
            {
                item.ExpenseDtos.ForEach(exp =>
                {
                    exp.LodingMetroCity = resultPoliciesLodgingFooding.MetroCitiesUptoAmount;
                    if (exp.ExpenseCategoryId == new Guid("FBF965BD-A53E-4D97-978A-34C2007202E5"))
                    {
                        exp.Deviation = exp.Amount > resultPoliciesLodgingFooding.MetroCitiesUptoAmount ?
                        exp.Amount - resultPoliciesLodgingFooding.MetroCitiesUptoAmount : 0;
                    }

                    exp.LodingOtherCity = resultPoliciesLodgingFooding.OtherCitiesUptoAmount;
                    if (exp.ExpenseCategoryId == new Guid("1AADD03D-90E1-4589-8B9D-6121049B490D"))
                    {
                        exp.Deviation = exp.Amount > resultPoliciesLodgingFooding.OtherCitiesUptoAmount ?
                        exp.Amount - resultPoliciesLodgingFooding.OtherCitiesUptoAmount : 0;
                    }

                    exp.MiscDA = resultPoliciesDetail.FirstOrDefault().DailyAllowance;
                    if (exp.ExpenseCategoryId == new Guid("ED69E9A0-2D54-4A91-A598-F79973B9FE99"))
                    {
                        exp.Deviation = exp.Amount > resultPoliciesDetail.FirstOrDefault().DailyAllowance ?
                        exp.Amount - resultPoliciesDetail.FirstOrDefault().DailyAllowance : 0;
                    }

                    exp.FoodingAllowance = resultPoliciesLodgingFooding.BudgetAmount;
                    if (exp.ExpenseCategoryId == new Guid("BB0BF3AA-1FD9-4F1C-9FDE-8498073C58A9"))
                    {
                        exp.Deviation = exp.Amount > resultPoliciesLodgingFooding.BudgetAmount ?
                        exp.Amount - resultPoliciesLodgingFooding.BudgetAmount : 0;
                    }

                    if (exp.ExpenseCategoryId == new Guid("B1977DB3-D909-4936-A5DA-41BF84638963"))
                    {
                        var Conveyance = resultConveyance.Where(a => a.Name == "Conveyance (within a city)");

                        if (Conveyance != null)
                        {
                            var ConveyancesItemAll = Conveyance.Select(a => a.conveyancesItem).Where(b => b.Any(a => a.ConveyanceItemName == "Budget")).FirstOrDefault();
                            var ConveyancesItem = ConveyancesItemAll.Where(a => a.ConveyanceItemName == "Budget");
                            if (ConveyancesItem != null)
                            {
                                bool IsCheck = (bool)ConveyancesItem.FirstOrDefault().IsCheck;
                                if (IsCheck == true)
                                {
                                    if (ConveyancesItem.FirstOrDefault().Amount != null)
                                    {
                                        exp.ConveyanceWithinCity = (decimal)(ConveyancesItem.FirstOrDefault().Amount);


                                        exp.Deviation = exp.Amount > ConveyancesItem.FirstOrDefault().Amount.Value ?
                                        exp.Amount - ConveyancesItem.FirstOrDefault().Amount.Value : 0;
                                    }
                                }
                            }
                        }
                    }

                    if (exp.ExpenseCategoryId == new Guid("5278397A-C8DD-475A-A7A7-C05708B2BB06"))
                    {
                        var ConveyanceCityOuterArea = resultConveyance.Where(a => a.Name == "Conveyance (city to outer area)");
                        if (ConveyanceCityOuterArea != null)
                        {
                            var ConveyancesItemAll = ConveyanceCityOuterArea.Select(a => a.conveyancesItem).Where(b => b.Any(a => a.ConveyanceItemName == "Budget")).FirstOrDefault();
                            var ConveyancesItem = ConveyancesItemAll.Where(a => a.ConveyanceItemName == "Budget");
                            if (ConveyancesItem != null)
                            {
                                bool IsCheck = (bool)ConveyancesItem.FirstOrDefault().IsCheck;
                                if (IsCheck == true)
                                {
                                    if (ConveyancesItem.FirstOrDefault().Amount != null)
                                    {
                                        exp.ConveyanceCityOuterArea = (decimal)(ConveyancesItem.FirstOrDefault().Amount);

                                        exp.Deviation = exp.Amount > ConveyancesItem.FirstOrDefault().Amount.Value ?
                                        exp.Amount - ConveyancesItem.FirstOrDefault().Amount.Value : 0;
                                    }
                                }
                            }
                        }
                    }
                });
            });

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
            return Ok(responseData);
        }


        /// <summary>
        /// Get All Master Expenses Group Date Wise
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("GetExpensesDetailsReportDateWise/{id}")]
        //[ClaimCheck("EXP_VIEW_EXPENSES")]
        public async Task<IActionResult> GetAllExpensesDetailsListDateWise(Guid id)
        {
            var ReportQuery = new GetAllExpenseDateWiseReportQuery { MasterExpenseId = id };
            var result = await _mediator.Send(ReportQuery);

            //var masterExpensesDetails = _expenseRepository.All.Where(a => a.MasterExpenseId == id).ToList();


            //var result = masterExpensesDetails.GroupBy(a => a.ExpenseDate).SelectMany(a => a).ToList();

            return Ok(result);

        }

        /// <summary>
        /// Get All Report Data
        /// </summary>       
        /// <returns></returns>
        [HttpPost("GetAllReportUserWise")]
        //[ClaimCheck("EXP_VIEW_EXPENSES")]
        public async Task<IActionResult> GetAllReportUserWise(GetAllReportQuery getAllReportQuery)
        {

            var result = await _mediator.Send(getAllReportQuery);

            return Ok(result);

        }


        /// <summary>
        /// Gets All Expense By Trip  
        /// </summary>
        /// <param name="id">The trip identifier.</param>
        /// <returns></returns>
        [HttpGet("GetExistingExpenseByTrip/{id}")]
        //[ClaimCheck("EXP_VIEW_EXPENSES")]
        public async Task<IActionResult> GetExistingExpenseByTrip(Guid id)
        {
            ExistingExpenseByTripData existingExpenseByTripData = new ExistingExpenseByTripData();
            var expense = _masterExpenseRepository.All.Where(a => a.TripId == id).FirstOrDefault();
            if (expense != null)
            {
                existingExpenseByTripData.status = true;
                existingExpenseByTripData.StatusCode = 200;
                existingExpenseByTripData.Data = expense;
            }
            else
            {
                existingExpenseByTripData.status = false;
                existingExpenseByTripData.StatusCode = 500;

            }
            return Ok(existingExpenseByTripData);
        }

        /// <summary>
        /// Gets All Expense By Trip  
        /// </summary>
        /// <param name="id">The trip identifier.</param>
        /// <returns></returns>
        [HttpGet("GetExistingExpenseByTripApp/{id}")]
        //[ClaimCheck("EXP_VIEW_EXPENSES")]
        public async Task<IActionResult> GetExistingExpenseByTripApp(Guid id)
        {
            ExistingExpenseByTripData existingExpenseByTripData = new ExistingExpenseByTripData();
            var expense = _masterExpenseRepository.All.Where(a => a.TripId == id).FirstOrDefault();
            if (expense != null)
            {
                existingExpenseByTripData.status = true;
                existingExpenseByTripData.StatusCode = 200;
                existingExpenseByTripData.IsExpenseExist = true;
                existingExpenseByTripData.Data = expense;
            }
            else
            {
                existingExpenseByTripData.status = true;
                existingExpenseByTripData.StatusCode = 200;
                existingExpenseByTripData.IsExpenseExist = false;
            }
            return Ok(existingExpenseByTripData);
        }

        /// <summary>
        /// Download Expense Files
        /// </summary>
        /// <param name="expenseId">The Expanse.</param>
        /// <returns></returns>
        [HttpGet("DownloadZipFile/{expenseId}")]
        public async Task<IActionResult> DownloadZipFile(Guid expenseId)
        {
            var zipQuery = new DownloadZipFileCommand { ExpenseId = expenseId };
            var result = await _mediator.Send(zipQuery);
            bool exists = result.Any(x => string.IsNullOrEmpty(x.ReceiptPath));
            if (exists)
            {
                using (var memoryStream = new MemoryStream())
                {
                    using (var zipArcheive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        foreach (var file in result)
                        {
                            var files = Directory.GetFiles(file.ReceiptPath.Substring(0, file.ReceiptPath.LastIndexOf('\\')));

                            if (files.Length == 0)
                                return NotFound("No files found to download.");

                            var fileInfo = new System.IO.FileInfo(file.ReceiptPath);
                            var entry = zipArcheive.CreateEntry(fileInfo.Name);
                            using (var entryStream = entry.Open())
                            using (var fileStream = new FileStream(file.ReceiptPath, FileMode.Open, FileAccess.Read))
                            {
                                fileStream.CopyTo(entryStream);
                            }
                        }
                    }
                    string fileName = DateTime.Now.Year.ToString() + "" +
                                                  DateTime.Now.Month.ToString() + "" +
                                                  DateTime.Now.Day.ToString() + "" +
                                                  DateTime.Now.Hour.ToString() + "" +
                                                  DateTime.Now.Minute.ToString() + "" +
                                                  DateTime.Now.Second.ToString();

                    memoryStream.Seek(0, SeekOrigin.Begin);

                    //For Mobile App
                    var pathToSave = result[0].ReceiptPath.Substring(0, result[0].ReceiptPath.LastIndexOf('\\'));
                    System.IO.File.WriteAllBytes(Path.Combine(pathToSave, "ExpenseDocs_" + fileName + ".zip"), memoryStream.ToArray());
                    var filepath = Path.Combine("Attachments", "ExpenseDocs_" + fileName + ".zip");
                    var jsonData = new { Download = filepath };
                    return Ok(jsonData);
                    //
                    //return File(memoryStream.ToArray(), "application/zip", "ExpenseDocs_" + fileName + ".zip");
                }
            }

            return NotFound("No files found to download.");
        }

        /// <summary>
        /// Download All Expense  Files
        /// </summary>
        /// <param name="masterExpenseId">The All Expense identifier.</param>
        /// <returns></returns>
        [HttpGet("DownloadAllExpenseZipFile/{masterExpenseId}")]
        public async Task<IActionResult> DownloadAllExpenseZipFile(Guid masterExpenseId)
        {
            var allZipQuery = new DownloadAllExpenseZipFileCommand { MasterExpenseId = masterExpenseId };
            var result = await _mediator.Send(allZipQuery);

            if (result.Count > 0)
            {
                //return Ok(result);
                using (var memoryStream = new MemoryStream())
                {
                    using (var zipArcheive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        foreach (var file in result)
                        {
                            var files = Directory.GetFiles(file.Substring(0, file.LastIndexOf('\\')));

                            if (files.Length == 0)
                                return NotFound("No files found to download.");

                            var fileInfo = new System.IO.FileInfo(file);
                            var entry = zipArcheive.CreateEntry(fileInfo.Name);
                            using (var entryStream = entry.Open())
                            using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
                            {
                                fileStream.CopyTo(entryStream);
                            }
                        }
                    }

                    string fileName = DateTime.Now.Year.ToString() + "" +
                                                  DateTime.Now.Month.ToString() + "" +
                                                  DateTime.Now.Day.ToString() + "" +
                                                  DateTime.Now.Hour.ToString() + "" +
                                                  DateTime.Now.Minute.ToString() + "" +
                                                  DateTime.Now.Second.ToString();

                    memoryStream.Seek(0, SeekOrigin.Begin);

                    //For Mobile App
                    var pathToSave = result[0].Substring(0, result[0].LastIndexOf('\\'));
                    System.IO.File.WriteAllBytes(Path.Combine(pathToSave, "AllExpenseDocs_" + fileName + ".zip"), memoryStream.ToArray());
                    var filepath = Path.Combine("Attachments", "AllExpenseDocs_" + fileName + ".zip");
                    var jsonData = new { Download = filepath };
                    return Ok(jsonData);
                }
            }

            return NotFound("No files found to download.");
        }

        /// <summary>
        /// Deletes Expense Misc
        /// </summary>
        /// <param name="masterExpenseId">The All Expense identifier.</param>
        /// <returns></returns>
        [HttpDelete("DeleteExpenseMisc/{masterExpenseId}")]
        public async Task<IActionResult> DeleteExpenseMisc(Guid masterExpenseId)
        {
            var command = new DeleteExpenseMiscCommand() { MasterExpenseId = masterExpenseId };
            var result = await _mediator.Send(command);
            return ReturnFormattedResponse(result);
        }


        /// <summary>
        /// Change Expense Account Team.
        /// </summary>      
        /// <param name="changeExpenseAccountTeamCommand"></param>
        /// <returns></returns>
        [HttpPut("ChangeExpenseAccountTeam")]
        //[ClaimCheck("EXP_UPDATE_EXPENSE")]
        public async Task<IActionResult> ChangeExpenseAccountTeam([FromBody] ChangeExpenseAccountTeamCommand changeExpenseAccountTeamCommand)
        {
            BTTEM.Data.Entities.ResponseData response = new BTTEM.Data.Entities.ResponseData();
            int Response = 0;
            var result = await _mediator.Send(changeExpenseAccountTeamCommand);
            if (result.Success)
            {
                Response = 1;

            }
            if (Response > 0)
            {
                response.status = true;
                response.StatusCode = 200;
                response.message = "Success";
            }
            else
            {
                response.status = false;
                response.StatusCode = 500;
                response.message = "Error";
            }
            return Ok(response);

            //return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Add Approval Level Type.
        /// </summary>      
        /// <param name="addApprovalLevelTypeCommand"></param>
        /// <returns></returns>
        [HttpPost("AddApprovalTypeLevel")]
        public async Task<IActionResult> AddApprovalLevelType([FromBody] AddApprovalLevelTypeCommand addApprovalLevelTypeCommand)
        {
            var result = await _mediator.Send(addApprovalLevelTypeCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Update Approval Level Type.
        /// </summary>      
        /// <param name="updateApprovalLevelTypeCommand"></param>
        /// <returns></returns>
        [HttpPut("UpdateApprovalLevelType")]
        public async Task<IActionResult> UpdateApprovalLevelType([FromBody] UpdateApprovalLevelTypeCommand updateApprovalLevelTypeCommand)
        {
            var result = await _mediator.Send(updateApprovalLevelTypeCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Get All Approval Level Type
        /// </summary>
        /// <param name="approvalLevelTypeResource"></param>
        /// <returns></returns>

        [HttpGet("GetApprovalLevelTypes")]
        public async Task<IActionResult> GetApprovalLevelTypes([FromQuery] ApprovalLevelTypeResource approvalLevelTypeResource)
        {
            var getAllApprovalLevelTypeQuery = new GetAllApprovalLevelTypeQuery
            {
                ApprovalLevelTypeResource = approvalLevelTypeResource
            };
            var result = await _mediator.Send(getAllApprovalLevelTypeQuery);

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
        /// Add Approval Level.
        /// </summary>      
        /// <param name="addApprovalLevelCommand"></param>
        /// <returns></returns>
        [HttpPost("AddApprovalLevel")]
        public async Task<IActionResult> AddApprovalLevel([FromBody] AddApprovalLevelCommand addApprovalLevelCommand)
        {
            var result = await _mediator.Send(addApprovalLevelCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Update Approval Level.
        /// </summary>      
        /// <param name="updateApprovalLevelCommand"></param>
        /// <returns></returns>
        [HttpPut("UpdateApprovalLevel")]
        public async Task<IActionResult> UpdateApprovalLevel([FromBody] UpdateApprovalLevelCommand updateApprovalLevelCommand)
        {
            var result = await _mediator.Send(updateApprovalLevelCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Get All Approval Level
        /// </summary>
        /// <param name="approvalLevelResource"></param>
        /// <returns></returns>

        [HttpGet("GetApprovalLevels")]
        public async Task<IActionResult> GetApprovalLevels([FromQuery] ApprovalLevelResource approvalLevelResource)
        {
            var getAllApprovalLevelQuery = new GetAllApprovalLevelQuery
            {
                ApprovalLevelResource = approvalLevelResource
            };
            var result = await _mediator.Send(getAllApprovalLevelQuery);

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
        /// Send to SAP.
        /// </summary>      
        /// <param name="sapCommand"></param>
        /// <returns></returns>
        [HttpPost("SendToSAP")]
        public async Task<IActionResult> SendToSAP([FromBody] SapCommand sapCommand)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://ssilsaprtr.shyamsteel.com:44340/RESTAdapter/TravelExpense");
            request.Headers.Add("Authorization", "Basic UE9FSU5WT0lDRTphc2Rmams4bEAqMjQ=");
            request.Headers.Add("Cookie", "saplb_*=(J2EE2526620)2526650");

            var payload = JsonConvert.SerializeObject(new { Record = sapCommand.Record });
            request.Content = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadAsStringAsync();

            var resultObject = System.Text.Json.JsonSerializer.Deserialize<RootObject>(responseData);

            int journeyNumber = resultObject?.Result?.JourneyNumber ?? 0;

            AddSapCommand addSapCommand = new AddSapCommand()
            {
                Id = Guid.NewGuid(),
                SapData = Newtonsoft.Json.JsonConvert.SerializeObject(sapCommand.Record),
                Status = "Parked",
                DocumentNumber = resultObject?.Result?.JourneyNumber ?? 0
            };

            var result = await _mediator.Send(addSapCommand);
            return ReturnFormattedResponse(result);
        }
        public class RootObject
        {
            public ResultData Result { get; set; }
        }
        public class ResultData
        {
            public int JourneyNumber { get; set; }
            public string Message { get; set; }
        }

        [NonAction]
        public string ItineraryHtml(List<TripItinerary> tripItineraries, string TripType)
        {
            string baseUrl = this._configuration.GetSection("Url")["BaseUrl"];
            StringBuilder sb = new StringBuilder();
            foreach (var item in tripItineraries)
            {
                sb.Append("<table class='journeyTableTop' style = 'background-color:#fff; box-shadow: -1px 4px 3px 2px #0000ff1a; margin-bottom:5px;'>");
                sb.Append("<tr>");
                sb.Append("<td>");
                sb.Append("<div class='Journey startJourny'>");
                sb.Append("<p>Start Journey</p>");
                sb.Append("<h5>" + item.DepartureCityName + "</h5>");
                sb.Append("<h6><span>" + item.DepartureDate + "</span></h6>");
                sb.Append("</div>");
                sb.Append("</td>");
                sb.Append("<td>");
                sb.Append("<div class='journeyDetail'>");
                sb.Append("<p>" + TripType + "</p>");
                sb.Append("<div class='journeyImage'>");
                if (item.TripBy == "Bus")
                {
                    sb.Append("<img src = '" + baseUrl + " images/busImg.png' class='busImg' alt='' style='max-width: 27px; margin: 0 auto !important; display: block;'> ");
                }
                if (item.TripBy == "Flight")
                {
                    sb.Append("<img src = '" + baseUrl + "images/flightImg.png' class='busImg' alt='' style='display:block; margin:0 auto;' >");
                }
                if (item.TripBy == "Train")
                {
                    sb.Append("<img src = '" + baseUrl + "images/trainImg2.png' class='busImg' alt='' style='display:block; margin:0 auto;' >");
                }
                if (item.TripBy == "Car")
                {
                    sb.Append("<img src = '" + baseUrl + "images/carImg.png' class='busImg' alt='' style='display:block; margin:0 auto;' >");
                }
                sb.Append("<img src = '" + baseUrl + "images/lines.png' class='lines' alt='' style='display:block; margin:0 auto;' >");
                sb.Append("</div>");
                if (item.TripBy == "Bus")
                {
                    sb.Append("<p>Bus Booking</p>");
                }
                if (item.TripBy == "Flight")
                {
                    sb.Append("<p>Flight Booking</p>");
                }
                if (item.TripBy == "Train")
                {
                    sb.Append("<p>Train Booking</p>");
                }
                if (item.TripBy == "Car")
                {
                    sb.Append("<p>Car Booking</p>");
                }
                sb.Append("</div>");
                sb.Append("</td>");
                sb.Append("<td>");
                sb.Append("<div class='Journey endtJourny'>");
                sb.Append("<p>End Journey</p>");
                sb.Append("<h5>" + item.ArrivalCityName + "</h5>");
                sb.Append("<h6><span>" + item.DepartureDate + "</span></h6>");
                sb.Append("</div>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</table>");
            }
            return sb.ToString();
        }


        [NonAction]
        public async Task<ResponseModel> PushNotificationForExpense(MessageRequest request)
        {
            NotificationModel message = null;
            if (!string.IsNullOrEmpty(request.DeviceToken) && request.DeviceType == false)
            {
                message = new NotificationModel()
                {
                    Message = new BTTEM.API.Models.Message
                    {
                        Token = request.DeviceToken,

                        Notification = new BTTEM.API.Models.Notification()
                        {
                            Title = request.Title,
                            Body = request.Body
                        },

                        Data = new BTTEM.API.Models.Data
                        {
                            NotificationTitle = request.Title,
                            NotificationBody = request.Body,
                            UserId = request.UserId,
                            Screen = "Profile",
                            CustomKey = request.CustomKey,
                            Id = request.Id
                        },

                        Android = new Android()
                        {
                            Priority = "high"
                        }
                    }
                };
            }
            else if (!string.IsNullOrEmpty(request.DeviceToken) && request.DeviceType == true)
            {
                message = new NotificationModel()
                {
                    Message = new BTTEM.API.Models.Message
                    {
                        Token = request.DeviceToken,

                        Data = new BTTEM.API.Models.Data
                        {
                            NotificationTitle = request.Title,
                            NotificationBody = request.Body,
                            UserId = request.UserId,
                            Screen = "Profile",
                            CustomKey = request.CustomKey,
                            Id = request.Id
                        },

                        Android = new Android()
                        {
                            Priority = "high"
                        }
                    }
                };
            }

            var resultNotification = await _notificationService.SendNotification(message);

            if (resultNotification.IsSuccess)
            {
                return resultNotification;
            }
            else
            {
                return resultNotification;
            }
        }


        /// <summary>
        /// Get All Expenses Group Category Wise
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("GetAllExpenseCategoryWise/{id}")]
        //[ClaimCheck("EXP_VIEW_EXPENSES")]
        public async Task<IActionResult> GetAllExpensesDetailsListCategoryWise(Guid id)
        {
            var ReportQuery = new GetAllExpenseCategoryWiseQuery { Id = id }; //AllExpenseCategoryWise
            var result = await _mediator.Send(ReportQuery);            

            return Ok(result);

        }
    }
}


