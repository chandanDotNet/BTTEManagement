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

        [HttpGet("GetAllPoliciesDetail")]
        //[Produces("application/json", "application/xml", Type = typeof(PoliciesDetailDto))]
        public async Task<IActionResult> GetAllPoliciesDetail()
        {
            var getAllRoleQuery = new PoliciesDetailResource { };
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
    }
}
