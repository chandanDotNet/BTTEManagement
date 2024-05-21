using BTTEM.MediatR.Department.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.API.Controllers;
using System.Threading.Tasks;
using System;
using BTTEM.MediatR.PoliciesTravel.Commands;
using BTTEM.Data.Resources;
using POS.API.Helpers;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.Data;
using BTTEM.Data.Dto.PoliciesTravel;
using BTTEM.MediatR.PoliciesTravel.Handlers;
using POS.MediatR.Brand.Command;
using System.Linq;

namespace BTTEM.API.Controllers.PoliciesTravel
{
    [Route("api/[controller]")]
    [ApiController]
    public class PoliciesTravelController : BaseController
    {

        readonly IMediator _mediator;

        public PoliciesTravelController(IMediator mediator)
        {
            _mediator = mediator;
        }


        /// <summary>
        /// Get All TravelMode
        /// </summary>

        /// <returns></returns>

        [HttpGet(Name = "GetTravelMode/{Id}")]
        public async Task<IActionResult> GetTravelMode(Guid Id)
        {
            var getAllTravelsModesQuery = new GetAllTravelsModesCommand
            {
                Id = Id
            };
            var result = await _mediator.Send(getAllTravelsModesQuery);

            //var paginationMetadata = new
            //{
            //    totalCount = result.TotalCount,
            //    pageSize = result.PageSize,
            //    skip = result.Skip,
            //    totalPages = result.TotalPages
            //};
            //Response.Headers.Add("X-Pagination",Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            return Ok(result);
        }


        /// <summary>
        /// Get All Conveyance
        /// </summary>

        /// <returns></returns>

        [HttpGet("GetConveyance")]
        public async Task<IActionResult> GetConveyance(Guid? Id)
        {
            var getAllConveyanceCommand = new GetAllConveyanceCommand
            {
                Id = Id
            };
            var result = await _mediator.Send(getAllConveyanceCommand);

            //var paginationMetadata = new
            //{
            //    totalCount = result.TotalCount,
            //    pageSize = result.PageSize,
            //    skip = result.Skip,
            //    totalPages = result.TotalPages
            //};
            //Response.Headers.Add("X-Pagination",Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            return Ok(result);
        }

        /// <summary>
        /// Get All GetAllPoliciesDetail
        /// </summary>

        /// <returns></returns>

        [HttpPost("GetAllPoliciesDetail")]
        //[Produces("application/json", "application/xml", Type = typeof(PoliciesDetailDto))]
        public async Task<IActionResult> GetAllPoliciesDetail(PoliciesDetailResource getAllRoleQuery)
        {
            //var getAllRoleQuery = new PoliciesDetailResource { };
            var getAllPoliciesDetailCommand = new GetAllPoliciesDetailCommand
            {
                PoliciesDetailResource = getAllRoleQuery
            };
            var result = await _mediator.Send(getAllPoliciesDetailCommand);

            var paginationMetadata = new
            {
                totalCount = result.TotalCount,
                pageSize = result.PageSize,
                skip = result.Skip,
                totalPages = result.TotalPages
            };
            Response.Headers.Add("X-Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            return Ok(result);
        }


        /// <summary>
        ///  Create a PoliciesDetail
        /// </summary>
        /// <param name="addPoliciesDetailCommand"></param>
        /// <returns></returns>
        [HttpPost]
        //[ClaimCheck("USR_ADD_USER")]
        [Produces("application/json", "application/xml", Type = typeof(PoliciesDetailDto))]
        public async Task<IActionResult> AddPoliciesDetail(AddPoliciesDetailCommand addPoliciesDetailCommand)
        {
            var result = await _mediator.Send(addPoliciesDetailCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Delete Policies Detail.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("PoliciesDetail/{Id}")]
        public async Task<IActionResult> DeletePoliciesDetail(Guid Id)
        {
            var deletePoliciesDetailCommand = new DeletePoliciesDetailCommand { Id = Id };
            var result = await _mediator.Send(deletePoliciesDetailCommand);
            return ReturnFormattedResponse(result);
        }


        /// <summary>
        ///  Create a Travel Mode
        /// </summary>
        /// <param name="addTravelModeCommand"></param>
        /// <returns></returns>
        [HttpPost("AddTravelMode")]
        //[ClaimCheck("USR_ADD_USER")]
        [Produces("application/json", "application/xml", Type = typeof(TravelModeDto))]
        public async Task<IActionResult> AddTravelMode(AddTravelModeCommand addTravelModeCommand)
        {
            var result = await _mediator.Send(addTravelModeCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        ///  Update a Travel Mode
        /// </summary>
        /// <param name="updateTravelModeCommand"></param>
        /// <returns></returns>
        [HttpPut("UpdateTravelMode")]
        //[ClaimCheck("USR_ADD_USER")]
        [Produces("application/json", "application/xml", Type = typeof(TravelModeDto))]
        public async Task<IActionResult> UpdateTravelMode(UpdateTravelModeCommand updateTravelModeCommand)
        {
            var result = await _mediator.Send(updateTravelModeCommand);
            return ReturnFormattedResponse(result);
        }


        /// <summary>
        ///  Create Policies Lodging Fooding
        /// </summary>
        /// <param name="addPoliciesLodgingFoodingCommand"></param>
        /// <returns></returns>
        [HttpPost("AddPoliciesLodgingFooding")]
        //[ClaimCheck("USR_ADD_USER")]
        [Produces("application/json", "application/xml", Type = typeof(PoliciesLodgingFoodingDto))]
        public async Task<IActionResult> AddPoliciesLodgingFooding(AddPoliciesLodgingFoodingCommand addPoliciesLodgingFoodingCommand)
        {
            var result = await _mediator.Send(addPoliciesLodgingFoodingCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        ///  Update Policies Lodging Fooding
        /// </summary>
        /// <param name="updatePoliciesLodgingFoodingCommand"></param>
        /// <returns></returns>
        [HttpPut("UpdatePoliciesLodgingFooding")]
        //[ClaimCheck("USR_ADD_USER")]
        [Produces("application/json", "application/xml", Type = typeof(PoliciesLodgingFoodingDto))]
        public async Task<IActionResult> UpdatePoliciesLodgingFooding(UpdatePoliciesLodgingFoodingCommand updatePoliciesLodgingFoodingCommand)
        {
            var result = await _mediator.Send(updatePoliciesLodgingFoodingCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Get All Policies Lodging Fooding
        /// </summary>

        /// <returns></returns>

        [HttpGet("GetPoliciesLodgingFooding")]
        public async Task<IActionResult> GetPoliciesLodgingFooding(Guid? Id)
        {
            var getAllPoliciesLodgingFoodingCommand = new GetAllPoliciesLodgingFoodingCommand
            {
                Id = Id
            };
            var result = await _mediator.Send(getAllPoliciesLodgingFoodingCommand);           

            return Ok(result);
        }

        /// <summary>
        /// Get All Policies Lodging Fooding Allowance By User 
        /// </summary>

        /// <returns></returns>

        [HttpGet("GetPoliciesLodgingFoodingAllowanceByUser")]
        public async Task<IActionResult> GetPoliciesLodgingFoodingAllowanceByUser(Guid? Id)
        {
           
            Guid? CompanyAccountId = null;
            Guid? GradeId = null;
            PoliciesDetailResource policiesDetailResourceQuery = new PoliciesDetailResource
            {
                CompanyAccountId = CompanyAccountId,
                GradeId=GradeId,
            };
            var getAllPoliciesDetailCommand = new GetAllPoliciesDetailCommand
            {
                PoliciesDetailResource = policiesDetailResourceQuery
            };
            var resultPoliciesDetail = await _mediator.Send(getAllPoliciesDetailCommand);

            var getAllPoliciesLodgingFoodingCommand = new GetAllPoliciesLodgingFoodingCommand
            {
                Id = resultPoliciesDetail.FirstOrDefault().Id
            };
            var result = await _mediator.Send(getAllPoliciesLodgingFoodingCommand);

            return Ok(result);
        }

        /// <summary>
        /// Get All Conveyance AllowanceByUser
        /// </summary>

        /// <returns></returns>

        [HttpGet("GetConveyanceAllowanceByUser")]
        public async Task<IActionResult> GetConveyanceAllowanceByUser(Guid? UserId)
        {
            Guid? CompanyAccountId = null;
            Guid? GradeId = null;
            PoliciesDetailResource policiesDetailResourceQuery = new PoliciesDetailResource
            {
                CompanyAccountId = CompanyAccountId,
                GradeId = GradeId,
            };
            var getAllPoliciesDetailCommand = new GetAllPoliciesDetailCommand
            {
                PoliciesDetailResource = policiesDetailResourceQuery
            };
            var resultPoliciesDetail = await _mediator.Send(getAllPoliciesDetailCommand);

            var getAllConveyanceCommand = new GetAllConveyanceCommand
            {
                Id = resultPoliciesDetail.FirstOrDefault().Id
            };
            var result = await _mediator.Send(getAllConveyanceCommand);
            

            return Ok(result);
        }

        /// <summary>
        /// Get All Policies Vehicle Conveyance AllowanceByUser
        /// </summary>

        /// <returns></returns>

        [HttpGet("GetPoliciesVehicleConveyanceAllowanceByUser")]
        public async Task<IActionResult> GetPoliciesVehicleConveyanceAllowanceByUser(Guid UserId)
        {
            Guid? CompanyAccountId = null;
            Guid? GradeId = null;
            PoliciesDetailResource policiesDetailResourceQuery = new PoliciesDetailResource
            {
                CompanyAccountId = CompanyAccountId,
                GradeId = GradeId,
            };
            var getAllPoliciesDetailCommand = new GetAllPoliciesDetailCommand
            {
                PoliciesDetailResource = policiesDetailResourceQuery
            };
            var resultPoliciesDetail = await _mediator.Send(getAllPoliciesDetailCommand);

            var getAllConveyanceCommand = new GetAllPoliciesVehicleConveyanceCommand
            {
                Id = resultPoliciesDetail.FirstOrDefault().Id
            };
            var result = await _mediator.Send(getAllConveyanceCommand);


            return Ok(result);
        }

        /// <summary>
        /// Get All Policies MISC AllowanceByUser
        /// </summary>

        /// <returns></returns>

        [HttpGet("GetPoliciesMISCAllowanceByUser")]
        public async Task<IActionResult> GetPoliciesMISCAllowanceByUser(Guid UserId)
        {
            Guid? CompanyAccountId = null;
            Guid? GradeId = null;
            PoliciesDetailResource policiesDetailResourceQuery = new PoliciesDetailResource
            {
                CompanyAccountId = CompanyAccountId,
                GradeId = GradeId,
            };
            var getAllPoliciesDetailCommand = new GetAllPoliciesDetailCommand
            {
                PoliciesDetailResource = policiesDetailResourceQuery
            };
            var result = await _mediator.Send(getAllPoliciesDetailCommand);           


            return Ok(result);
        }





        /// <summary>
        ///  Update a PoliciesDetail
        /// </summary>
        /// <param name="UpdatePoliciesDetailCommand"></param>
        /// <returns></returns>
        [HttpPut]
        //[ClaimCheck("USR_ADD_USER")]
        [Produces("application/json", "application/xml", Type = typeof(PoliciesDetailDto))]
        public async Task<IActionResult> UpdatePoliciesDetail(UpdatePoliciesDetailCommand updatePoliciesDetailCommand)
        {
            var result = await _mediator.Send(updatePoliciesDetailCommand);
            return ReturnFormattedResponse(result);
        }


        /// <summary>
        ///  Create a Conveyance 
        /// </summary>
        /// <param name="addConveyanceCommand"></param>
        /// <returns></returns>
        [HttpPost("AddConveyance")]
        //[ClaimCheck("USR_ADD_USER")]
        [Produces("application/json", "application/xml", Type = typeof(ConveyanceDto))]
        public async Task<IActionResult> AddConveyance(AddConveyanceCommand addConveyanceCommand)
        {
            var result = await _mediator.Send(addConveyanceCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        ///  Update a Conveyance 
        /// </summary>
        /// <param name="updateConveyanceCommand"></param>
        /// <returns></returns>
        [HttpPut("UpdateConveyance")]
        //[ClaimCheck("USR_ADD_USER")]
        [Produces("application/json", "application/xml", Type = typeof(ConveyanceDto))]
        public async Task<IActionResult> UpdateConveyance(UpdateConveyanceCommand updateConveyanceCommand)
        {
            var result = await _mediator.Send(updateConveyanceCommand);
            return ReturnFormattedResponse(result);
        }


        /// <summary>
        ///  Create a PoliciesVehicleConveyance 
        /// </summary>
        /// <param name="addPoliciesVehicleConveyanceCommand"></param>
        /// <returns></returns>
        [HttpPost("AddPoliciesVehicleConveyance")]
        //[ClaimCheck("USR_ADD_USER")]
        [Produces("application/json", "application/xml", Type = typeof(PoliciesVehicleConveyanceDto))]
        public async Task<IActionResult> AddPoliciesVehicleConveyance(AddPoliciesVehicleConveyanceCommand addPoliciesVehicleConveyanceCommand)
        {
            var result = await _mediator.Send(addPoliciesVehicleConveyanceCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        ///  Update a PoliciesVehicleConveyance
        /// </summary>
        /// <param name="updatePoliciesVehicleConveyanceCommand"></param>
        /// <returns></returns>
        [HttpPut("UpdatePoliciesVehicleConveyance")]
        //[ClaimCheck("USR_ADD_USER")]
        [Produces("application/json", "application/xml", Type = typeof(PoliciesVehicleConveyanceDto))]
        public async Task<IActionResult> UpdatePoliciesVehicleConveyance(UpdatePoliciesVehicleConveyanceCommand updatePoliciesVehicleConveyanceCommand)
        {
            var result = await _mediator.Send(updatePoliciesVehicleConveyanceCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Get All Policies Vehicle Conveyance
        /// </summary>

        /// <returns></returns>

        [HttpGet("GetPoliciesVehicleConveyance")]
        public async Task<IActionResult> GetPoliciesVehicleConveyance(Guid Id)
        {
            var getAllConveyanceCommand = new GetAllPoliciesVehicleConveyanceCommand
            {
                Id = Id
            };
            var result = await _mediator.Send(getAllConveyanceCommand);
          

            return Ok(result);
        }


        /// <summary>
        ///  Create a Policies Setting 
        /// </summary>
        /// <param name="addPoliciesSettingCommand"></param>
        /// <returns></returns>
        [HttpPost("AddPoliciesSetting")]
        //[ClaimCheck("USR_ADD_USER")]
        [Produces("application/json", "application/xml", Type = typeof(PoliciesSettingDto))]
        public async Task<IActionResult> AddPoliciesSetting(AddPoliciesSettingCommand addPoliciesSettingCommand)
        {
            var result = await _mediator.Send(addPoliciesSettingCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        ///  Update a Policies Setting 
        /// </summary>
        /// <param name="updatePoliciesSettingCommand"></param>
        /// <returns></returns>
        [HttpPut("UpdatePoliciesSetting")]
        //[ClaimCheck("USR_ADD_USER")]
        [Produces("application/json", "application/xml", Type = typeof(PoliciesSettingDto))]
        public async Task<IActionResult> UpdatePoliciesSetting(UpdatePoliciesSettingCommand updatePoliciesSettingCommand)
        {
            var result = await _mediator.Send(updatePoliciesSettingCommand);
            return ReturnFormattedResponse(result);
        }


        /// <summary>
        /// Get All Policies Setting
        /// </summary>

        /// <returns></returns>

        [HttpGet("GetPoliciesSetting")]
        public async Task<IActionResult> GetPoliciesSetting(Guid Id)
        {
            var getAllPoliciesSettingCommand = new GetAllPoliciesSettingCommand
            {
                Id = Id
            };
            var result = await _mediator.Send(getAllPoliciesSettingCommand);


            return Ok(result);
        }


    }
}
