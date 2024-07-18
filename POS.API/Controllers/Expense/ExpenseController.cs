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
        private readonly IExpenseCategoryRepository _expenseCategoryRepository;
        private readonly ITripRepository _tripRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSMTPSettingRepository _emailSMTPSettingRepository;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        public ExpenseController(
            IMediator mediator, UserInfoToken userInfoToken, IExpenseRepository expenseRepository,
            IMasterExpenseRepository masterExpenseRepository, IUserRepository userRepository, IExpenseCategoryRepository expenseCategoryRepository, ITripRepository tripRepository,
            IWebHostEnvironment webHostEnvironment,
            IEmailSMTPSettingRepository emailSMTPSettingRepository)
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
        /// Add Local Con Expenses
        /// </summary>
        /// <param name="addLocalConveyanceExpenseCommand"></param>
        /// <returns></returns>
        [HttpPost("AddLocalConveyanceExpense")]
        //[ClaimCheck("EXP_ADD_EXPENSE")]
        public async Task<IActionResult> AddLocalConveyanceExpense(List<AddLocalConveyanceExpenseCommand> addLocalConveyanceExpenseCommandList)
        {
            ResponseData responseData = new ResponseData();
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

            return ReturnFormattedResponse(result);

        }


        // <summary>
        /// Update Local Conveyance Expenses
        /// </summary>
        /// <param name="updateMasterExpenseCommand"></param>
        /// <returns></returns>
        [HttpPut("UpdateLocalConveyanceExpense")]
        //[ClaimCheck("EXP_ADD_EXPENSE")]
        public async Task<IActionResult> UpdateLocalConveyanceExpense(List<UpdateLocalConveyanceExpenseCommand> updateLocalConveyanceExpenseCommandList)
        {
            ResponseData responseData = new ResponseData();
            foreach (var item in updateLocalConveyanceExpenseCommandList)
            {
                UpdateLocalConveyanceExpenseCommand updateLocalConveyanceExpenseCommand = new UpdateLocalConveyanceExpenseCommand();
                updateLocalConveyanceExpenseCommand = item;
                var result = await _mediator.Send(updateLocalConveyanceExpenseCommand);
                if (result.Success)
                {
                    responseData.status = true;
                    responseData.StatusCode = 200;
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
            ResponseData responseData = new ResponseData();
            foreach (var item in addCarBikeLogBookExpenseCommandList)
            {
                AddCarBikeLogBookExpenseCommand addLocalConveyanceExpenseCommand = new AddCarBikeLogBookExpenseCommand();
                addLocalConveyanceExpenseCommand = item;
                var result = await _mediator.Send(addLocalConveyanceExpenseCommand);
                if (result.Success)
                {
                    responseData.status = true;
                    responseData.StatusCode = 200;
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
            ResponseData responseData = new ResponseData();
            foreach (var item in updateCarBikeLogBookExpenseCommandList)
            {
                UpdateCarBikeLogBookExpenseCommand updateCarBikeLogBookExpenseCommand = new UpdateCarBikeLogBookExpenseCommand();
                updateCarBikeLogBookExpenseCommand = item;
                var result = await _mediator.Send(updateCarBikeLogBookExpenseCommand);
                if (result.Success)
                {
                    responseData.status = true;
                    responseData.StatusCode = 200;
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
                    if(addExpenseCommand.Amount>0)
                    {
                        addExpenseCommand.Status = "PENDING";
                    }
                    else
                    {
                        addExpenseCommand.Status = "APPROVED";
                    }
                   
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
                                        updateExpenseStatusCommand.Status = "APPROVED";
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
                                    decimal PoliciesLodgingFooding = resultPoliciesLodgingFooding.MetroCitiesUptoAmount * Convert.ToDecimal(noOfDays);
                                    if (expenseAmount > PoliciesLodgingFooding)
                                    {
                                        IsDeviation = true;
                                    }
                                    else
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
                            //-- Lodging (Other City)
                            if (item.Id == new Guid("1AADD03D-90E1-4589-8B9D-6121049B490D"))
                            {
                                if (resultPoliciesLodgingFooding.OtherCities == true)
                                {
                                    decimal PoliciesLodgingFooding = resultPoliciesLodgingFooding.OtherCitiesUptoAmount * Convert.ToDecimal(noOfDays);
                                    if (expenseAmount > PoliciesLodgingFooding)
                                    {
                                        IsDeviation = true;
                                    }
                                    else
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
                                            }
                                            else
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
                                        else //Actuals
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
                                            }
                                            else
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
                                        else //Actuals
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
                            }
                            //--MISC /DA
                            if (item.Id == new Guid("ED69E9A0-2D54-4A91-A598-F79973B9FE99"))
                            {
                                decimal DA = 0;
                                if (resultPoliciesDetail.FirstOrDefault().DailyAllowance != null)
                                {
                                    DA = (decimal)resultPoliciesDetail.FirstOrDefault().DailyAllowance * Convert.ToDecimal(noOfDays);
                                }

                                if (expenseAmount > DA)
                                {
                                    IsDeviation = true;
                                }
                                else
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
                            //--Fooding Allowance
                            if (item.Id == new Guid("BB0BF3AA-1FD9-4F1C-9FDE-8498073C58A9"))
                            {
                                if (resultPoliciesLodgingFooding.IsBudget == true)
                                {
                                    decimal PoliciesFooding = 0;
                                    if (resultPoliciesLodgingFooding.BudgetAmount != null)
                                    {
                                        PoliciesFooding = resultPoliciesLodgingFooding.BudgetAmount * Convert.ToDecimal(noOfDays);
                                    }
                                    if (expenseAmount > PoliciesFooding)
                                    {
                                        IsDeviation = true;
                                    }
                                    else
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
                                else
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
                    }

                }
                //===============


                //**Email Start**
                if (addMasterExpenseCommand.Status == "APPLIED")
                {

                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Template", "AddExpense.html");
                    var defaultSmtp = await _emailSMTPSettingRepository.FindBy(c => c.IsDefault).FirstOrDefaultAsync();
                    var reportingHead = _userRepository.FindAsync(userResult.ReportingTo.Value).Result;

                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        string templateBody = sr.ReadToEnd();
                        templateBody = templateBody.Replace("{NAME}", string.Concat(userResult.FirstName, " ", userResult.LastName));
                        templateBody = templateBody.Replace("{DATETIME}", DateTime.Now.ToString("dddd, dd MMMM yyyy"));
                        templateBody = templateBody.Replace("{EXPENSE_AMOUNT}", Convert.ToString(addMasterExpenseCommand.TotalAmount));
                        EmailHelper.SendEmail(new SendEmailSpecification
                        {
                            Body = templateBody,
                            FromAddress = defaultSmtp.UserName,
                            Host = defaultSmtp.Host,
                            IsEnableSSL = defaultSmtp.IsEnableSSL,
                            Password = defaultSmtp.Password,
                            Port = defaultSmtp.Port,
                            Subject = "Expenses",
                            ToAddress = reportingHead.UserName,
                            CCAddress = userResult.UserName,
                            UserName = defaultSmtp.UserName
                        });
                    }
                }
                //**Email End**
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
            updateMasterExpenseCommand.NoOfBill = updateMasterExpenseCommand.ExpenseDetails.Where(a => a.Amount > 0).Count();
            updateMasterExpenseCommand.TotalAmount = updateMasterExpenseCommand.ExpenseDetails.Sum(a => a.Amount);
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
                                        updateExpenseStatusCommand.Status = "APPROVED";
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
                                    decimal PoliciesLodgingFooding = resultPoliciesLodgingFooding.MetroCitiesUptoAmount * Convert.ToDecimal(noOfDays);
                                    if (expenseAmount > PoliciesLodgingFooding)
                                    {
                                        IsDeviation = true;
                                        if (expenseList.Count > 0)
                                        {
                                            foreach (var expense in expenseList)
                                            {
                                                updateExpenseStatusCommand.Id = expense.Id;
                                                updateExpenseStatusCommand.Status = "PENDING";
                                                updateExpenseStatusCommand.PayableAmount = 0;
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
                                                updateExpenseStatusCommand.Status = "APPROVED";
                                                updateExpenseStatusCommand.PayableAmount = expense.Amount;
                                                var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                            }
                                        }
                                    }
                                }

                            }
                            //-- Lodging (Other City)
                            if (item.Id == new Guid("1AADD03D-90E1-4589-8B9D-6121049B490D"))
                            {
                                if (resultPoliciesLodgingFooding.OtherCities == true)
                                {
                                    decimal PoliciesLodgingFooding = resultPoliciesLodgingFooding.OtherCitiesUptoAmount * Convert.ToDecimal(noOfDays);
                                    if (expenseAmount > PoliciesLodgingFooding)
                                    {
                                        IsDeviation = true;
                                        if (expenseList.Count > 0)
                                        {
                                            foreach (var expense in expenseList)
                                            {
                                                updateExpenseStatusCommand.Id = expense.Id;
                                                updateExpenseStatusCommand.Status = "PENDING";
                                                updateExpenseStatusCommand.PayableAmount = 0;
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
                                                updateExpenseStatusCommand.Status = "APPROVED";
                                                updateExpenseStatusCommand.PayableAmount = expense.Amount;
                                                var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                            }
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
                                                        updateExpenseStatusCommand.Status = "PENDING";
                                                        updateExpenseStatusCommand.PayableAmount = 0;
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
                                                        updateExpenseStatusCommand.Status = "APPROVED";
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
                                                    updateExpenseStatusCommand.Status = "APPROVED";
                                                    updateExpenseStatusCommand.PayableAmount = expense.Amount;
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
                                                        updateExpenseStatusCommand.Status = "PENDING";
                                                        updateExpenseStatusCommand.PayableAmount = 0;
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
                                                        updateExpenseStatusCommand.Status = "APPROVED";
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
                                                    updateExpenseStatusCommand.Status = "APPROVED";
                                                    updateExpenseStatusCommand.PayableAmount = expense.Amount;
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
                                    DA = (decimal)resultPoliciesDetail.FirstOrDefault().DailyAllowance * Convert.ToDecimal(noOfDays);
                                }

                                if (expenseAmount > DA)
                                {
                                    IsDeviation = true;
                                    if (expenseList.Count > 0)
                                    {
                                        foreach (var expense in expenseList)
                                        {
                                            updateExpenseStatusCommand.Id = expense.Id;
                                            updateExpenseStatusCommand.Status = "PENDING";
                                            updateExpenseStatusCommand.PayableAmount = 0;
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
                                            updateExpenseStatusCommand.Status = "APPROVED";
                                            updateExpenseStatusCommand.PayableAmount = expense.Amount;
                                            var result1 = await _mediator.Send(updateExpenseStatusCommand);
                                        }
                                    }
                                }
                            }
                            //--Fooding Allowance
                            if (item.Id == new Guid("BB0BF3AA-1FD9-4F1C-9FDE-8498073C58A9"))
                            {
                                if (resultPoliciesLodgingFooding.IsBudget == true)
                                {
                                    decimal PoliciesFooding = 0;
                                    if (resultPoliciesLodgingFooding.BudgetAmount != null)
                                    {
                                        PoliciesFooding = resultPoliciesLodgingFooding.BudgetAmount * Convert.ToDecimal(noOfDays);
                                    }
                                    if (expenseAmount > PoliciesFooding)
                                    {
                                        IsDeviation = true;
                                        if (expenseList.Count > 0)
                                        {
                                            foreach (var expense in expenseList)
                                            {
                                                updateExpenseStatusCommand.Id = expense.Id;
                                                updateExpenseStatusCommand.Status = "PENDING";
                                                updateExpenseStatusCommand.PayableAmount = 0;
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
                                                updateExpenseStatusCommand.Status = "APPROVED";
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
                    //ActionType = "Tracker",
                    ActionType = "Activity",
                    Remarks = updateExpenseStatusCommand.Status == "APPROVED" ? updateExpenseStatusCommand.Status : updateExpenseStatusCommand.RejectReason,//responseData.Result.Name + " Expense Status Updated",
                    Status = "Expense Status Updated By " + userResult.FirstName + " " + userResult.LastName,
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now,
                };
                var response = await _mediator.Send(addExpenseTrackingCommand);
            }
            SyncMasterExpenseAmountCommand syncMasterExpenseAmountCommand = new SyncMasterExpenseAmountCommand();
            syncMasterExpenseAmountCommand.Id= id;
            var responseSync = await _mediator.Send(syncMasterExpenseAmountCommand);

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
                string StatusMessage = null, RemarksMessage = null;
                var responseData = _masterExpenseRepository.AllIncluding(u => u.CreatedByUser).Where(x => x.Id == id).FirstOrDefaultAsync();
                var userResult = _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id)).Result;
                if (updateMasterExpenseStatusCommand.Status == "ROLLBACK")
                {
                    StatusMessage = "Master Expense Rollback Updated By " + userResult.FirstName + " " + userResult.LastName;
                    RemarksMessage = responseData.Result.Name + " Master Expense Rollback Updated By " + userResult.FirstName + " " + userResult.LastName;
                }
                else
                {
                    StatusMessage = "Master Expense Status Updated By " + userResult.FirstName + " " + userResult.LastName;
                    RemarksMessage = responseData.Result.Name + " Master Expense Status Updated By " + userResult.FirstName + " " + userResult.LastName;
                }


                var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
                {
                    MasterExpenseId = id,
                    ExpenseTypeName = responseData.Result.Name,
                    //ActionType = "Tracker",
                    ActionType = "Activity",
                    Remarks = RemarksMessage,
                    Status = StatusMessage,
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now,
                };
                var response = await _mediator.Send(addExpenseTrackingCommand);


                //**Email Start**
                //var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Template", "ExpenseStatus.html");
                //var defaultSmtp = await _emailSMTPSettingRepository.FindBy(c => c.IsDefault).FirstOrDefaultAsync();
                //var reportingHead = _userRepository.FindAsync(userResult.ReportingTo.Value).Result;

                //using (StreamReader sr = new StreamReader(filePath))
                //{
                //    string templateBody = sr.ReadToEnd();
                //    templateBody = templateBody.Replace("{NAME}", string.Concat(userResult.FirstName, " ", userResult.LastName));
                //    templateBody = templateBody.Replace("{DATETIME}", DateTime.Now.ToString("dddd, dd MMMM yyyy"));
                //    templateBody = templateBody.Replace("{STATUS}", Convert.ToString(StatusMessage));
                //    //EmailHelper.SendEmail(new SendEmailSpecification
                //    //{
                //    //    Body = templateBody,
                //    //    FromAddress = defaultSmtp.UserName,
                //    //    Host = defaultSmtp.Host,
                //    //    IsEnableSSL = defaultSmtp.IsEnableSSL,
                //    //    Password = defaultSmtp.Password,
                //    //    Port = defaultSmtp.Port,
                //    //    Subject = "Expense Status",
                //    //    ToAddress = responseData.Result.CreatedByUser.UserName,
                //    //    CCAddress = userResult.UserName,
                //    //    UserName = defaultSmtp.UserName
                //    //});
                //}
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
                var responseData = _masterExpenseRepository.FindAsync(deleteExpenseByDateCommand.Id);
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
        /// Update  Travel Document
        /// </summary>
        /// <param name="updateTravelDocumentCommand"></param>
        /// <returns></returns>
        [HttpPost("UpdateTravelDocument")]
        //[ClaimCheck("EXP_ADD_EXPENSE")]
        public async Task<IActionResult> UpdateTravelDocument([FromBody] UpdateTravelDocumentCommand updateTravelDocumentCommand)
        {
            var result = await _mediator.Send(updateTravelDocumentCommand);
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


            //if (result.Success)
            //{
            //    var responseData = _masterExpenseRepository.FindAsync(id);
            //    var userResult = _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id)).Result;
            //    var addExpenseTrackingCommand = new AddExpenseTrackingCommand()
            //    {
            //        MasterExpenseId = responseData.Result.Id,
            //        ExpenseTypeName = responseData.Result.Name,
            //        ActionType = "Activity",
            //        Remarks = responseData.Result.Name + " Master Expense Deleted By " + userResult.FirstName + " " + userResult.LastName,
            //        Status = "Master Expense Deleted By " + userResult.FirstName + " " + userResult.LastName,
            //        ActionBy = Guid.Parse(_userInfoToken.Id),
            //        ActionDate = DateTime.Now,
            //    };
            //    var response = await _mediator.Send(addExpenseTrackingCommand);
            //}

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

                //**Email Start**
                if (updateExpenseAndMasterExpenseCommand.MasterExpenseId.HasValue)
                {
                    var responseData = _masterExpenseRepository.AllIncluding(u => u.CreatedByUser).Where(x => x.Id == updateExpenseAndMasterExpenseCommand.MasterExpenseId.Value).FirstOrDefaultAsync();
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Template", "ExpenseStatus.html");
                    var defaultSmtp = await _emailSMTPSettingRepository.FindBy(c => c.IsDefault).FirstOrDefaultAsync();
                    var reportingHead = _userRepository.FindAsync(userResult.ReportingTo.Value).Result;

                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        string templateBody = sr.ReadToEnd();
                        templateBody = templateBody.Replace("{NAME}", string.Concat(userResult.FirstName, " ", userResult.LastName));
                        templateBody = templateBody.Replace("{DATETIME}", DateTime.Now.ToString("dddd, dd MMMM yyyy"));
                        templateBody = templateBody.Replace("{STATUS}", Convert.ToString("Expense REIMBURSED"));
                        templateBody = templateBody.Replace("{AMOUNT}", Convert.ToString(responseData.Result.TotalAmount));
                        templateBody = templateBody.Replace("{APPROVAL_AMOUNT}", Convert.ToString(responseData.Result.PayableAmount));
                        templateBody = templateBody.Replace("{REIMBURSED_AMOUNT}", Convert.ToString(responseData.Result.ReimbursementAmount));
                        EmailHelper.SendEmail(new SendEmailSpecification
                        {
                            Body = templateBody,
                            FromAddress = defaultSmtp.UserName,
                            Host = defaultSmtp.Host,
                            IsEnableSSL = defaultSmtp.IsEnableSSL,
                            Password = defaultSmtp.Password,
                            Port = defaultSmtp.Port,
                            Subject = "Expense Reimburse Status",
                            ToAddress = responseData.Result.CreatedByUser.UserName,
                            CCAddress = userResult.UserName,
                            UserName = defaultSmtp.UserName
                        });
                    }
                }
               
            }
            return ReturnFormattedResponse(result);
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
                var ExpenseDetailsList = _expenseRepository.All.Where(a => a.MasterExpenseId == masterExpenseResourceGroupWise.MasterExpenseId).GroupBy(a => a.ExpenseDate).ToList();
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
                        item.AllowedAmount = resultPoliciesLodgingFooding.MetroCitiesUptoAmount * Convert.ToDecimal(noOfDays);
                    }
                    //-- Lodging (Other City)
                    if (item.ExpenseCategoryId == new Guid("1AADD03D-90E1-4589-8B9D-6121049B490D"))
                    {
                        item.AllowedAmount = resultPoliciesLodgingFooding.OtherCitiesUptoAmount * Convert.ToDecimal(noOfDays);
                    }
                    //--MISC /DA
                    if (item.ExpenseCategoryId == new Guid("ED69E9A0-2D54-4A91-A598-F79973B9FE99"))
                    {
                        item.AllowedAmount = resultPoliciesDetail.FirstOrDefault().DailyAllowance * Convert.ToDecimal(noOfDays);
                    }
                    //--Fooding Allowance
                    if (item.ExpenseCategoryId == new Guid("BB0BF3AA-1FD9-4F1C-9FDE-8498073C58A9"))
                    {
                        item.AllowedAmount = resultPoliciesLodgingFooding.BudgetAmount * Convert.ToDecimal(noOfDays);
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

                        }
                    }

                    //--Fare
                    if (item.ExpenseCategoryId == new Guid("DCAA05B6-5F1E-402F-835E-0704A3A1A455"))
                    {
                        item.AllowedAmount = expenseData.Sum(x => x.Amount);
                    }
                    //--Others
                    if (item.ExpenseCategoryId == new Guid("6C3EB31C-DF53-495A-B871-E2EB3CEF74D2"))
                    {
                        item.AllowedAmount = expenseData.Sum(x => x.Amount);
                    }
                    if (expenseData != null)
                    {
                        item.ExpenseAmount = expenseData.Sum(x => x.Amount);
                        if (item.ExpenseAmount >= item.AllowedAmount)
                        {
                            item.DeviationAmount = item.ExpenseAmount - item.AllowedAmount;
                        }
                        item.ExpenseDtos.AddRange(expenseData);
                    }
                }
            }

            responseData.MaseterExpense.NoOfPendingAction = result.FirstOrDefault().Expenses
            .Where(x => x.Amount > 0 && x.Status == null || x.Status == string.Empty || x.Status == "PENDING").Count();

            responseData.MaseterExpense.NoOfPendingReimbursementAction = result.FirstOrDefault().Expenses
            .Where(x => x.Amount > 0 && x.Status == "APPROVED" && x.AccountStatus == null || x.AccountStatus == string.Empty || x.AccountStatus == "PENDING").Count();

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
        /// Download Expense Files
        /// </summary>
        /// <param name="expenseId">The Expanse.</param>
        /// <returns></returns>
        [HttpGet("DownloadZipFile/{expenseId}")]
        public async Task<IActionResult> DownloadZipFile(Guid expenseId)
        {
            var zipQuery = new DownloadZipFileCommand { ExpenseId = expenseId };
            var result = await _mediator.Send(zipQuery);
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
    }
}


