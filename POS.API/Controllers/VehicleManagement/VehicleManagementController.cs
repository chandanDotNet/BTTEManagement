using BTTEM.Data.Dto;
using BTTEM.MediatR.Command;
using BTTEM.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.API.Controllers;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BTTEM.API.Controllers.VehicleManagement
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VehicleManagementController : BaseController
    {
        public IMediator _mediator { get; set; }

        public VehicleManagementController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get Vehicle Management.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("VehicleManagement/{id}", Name = "GetVehicleManagement")]
        [Produces("application/json", "application/xml", Type = typeof(VehicleManagementDto))]
        public async Task<IActionResult> GetMultiLevelApproval(Guid id)
        {
            var getVehicleManagementQuery = new GetVehicleManagementQuery { Id = id };
            var result = await _mediator.Send(getVehicleManagementQuery);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Get Vehicle Managements.
        /// </summary>
        /// <returns></returns>
        [HttpGet("VehicleManagements")]
        [Produces("application/json", "application/xml", Type = typeof(List<VehicleManagementDto>))]
        public async Task<IActionResult> VehicleManagements()
        {
            var getAllVehicleManagementQuery = new GetAllVehicleManagementQuery { };
            var result = await _mediator.Send(getAllVehicleManagementQuery);
            return Ok(result);
        }

        /// <summary>
        /// Create Vehicle Type.
        /// </summary>
        /// <param name="addVehicleManagementCommand"></param>
        /// <returns></returns>
        [HttpPost("VehicleType")]
        [Produces("application/json", "application/xml", Type = typeof(VehicleManagementDto))]
        public async Task<IActionResult> AddVehicleType(AddVehicleManagementCommand addVehicleManagementCommand)
        {
            var response = await _mediator.Send(addVehicleManagementCommand);
            if (!response.Success)
            {
                return ReturnFormattedResponse(response);
            }
            return CreatedAtAction("GetVehicleManagement", new { id = response.Data.Id }, response.Data);
        }

        /// <summary>
        /// Update Vehicle Type.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="updateVehicleManagementCommand"></param>
        /// <returns></returns>
        [HttpPut("VehicleType/{Id}")]
        [Produces("application/json", "application/xml", Type = typeof(VehicleManagementDto))]
        public async Task<IActionResult> UpdateVehicleType(Guid Id, UpdateVehicleManagementCommand updateVehicleManagementCommand)
        {
            updateVehicleManagementCommand.Id = Id;
            var result = await _mediator.Send(updateVehicleManagementCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Delete Vehicle Management.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("VehicleType/{Id}")]
        public async Task<IActionResult> DeleteVehicleType(Guid Id)
        {
            var deleteVehicleManagementCommand = new DeleteVehicleManagementCommand { Id = Id };
            var result = await _mediator.Send(deleteVehicleManagementCommand);
            return ReturnFormattedResponse(result);
        }
    }
}
