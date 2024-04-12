using BTTEM.MediatR.PoliciesTravel.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.API.Controllers;
using System.Threading.Tasks;
using System;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.Data;

namespace BTTEM.API.Controllers.Trip
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripController :  BaseController
    {
        readonly IMediator _mediator;

        public TripController(IMediator mediator)
        {
            _mediator = mediator;
        }


        /// <summary>
        /// Get All Purpose
        /// </summary>

        /// <returns></returns>

        [HttpGet("GetAllPurpose")]
        public async Task<IActionResult> GetAllPurpose()
        {
            var getAllPurposeQuery = new GetAllPurposeQuery
            {
               
            };
            var result = await _mediator.Send(getAllPurposeQuery);

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
        ///  Create a Trip
        /// </summary>
        /// <param name="addTripCommand"></param>
        /// <returns></returns>
        [HttpPost]
        //[ClaimCheck("USR_ADD_USER")]
        [Produces("application/json", "application/xml", Type = typeof(TripDto))]
        public async Task<IActionResult> AddTripDetail(AddTripCommand addTripCommand)
        {
            var result = await _mediator.Send(addTripCommand);
            return ReturnFormattedResponse(result);
        }


        /// <summary>
        /// Get All Trips
        /// </summary>

        /// <returns></returns>

        [HttpGet(Name ="GetAllTrip")]
        public async Task<IActionResult> GetAllTrip(Guid? Id)
        {
            var getAllTripQuery = new GetAllTripQuery
            {
                Id = Id
            };
            var result = await _mediator.Send(getAllTripQuery);

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
    }
}
