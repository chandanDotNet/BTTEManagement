using BTTEM.Data.Dto;
using BTTEM.MediatR.Command;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.API.Controllers;
using POS.Data.Dto;
using POS.MediatR.Brand.Command;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BTTEM.API.Controllers.Vendor
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorController : BaseController
    {
        public IMediator _mediator { get; set; }
        public VendorController(IMediator mediator)
        {
            _mediator = mediator;   
        }
        /// <summary>
        /// Add Vendor
        /// </summary>
        /// <param name="addVendorCommand"></param>
        /// <returns></returns>
        [HttpPost(Name = "AddVendor")]
        [Produces("application/json", "application/xml", Type = typeof(VendorDto))]
        public async Task<IActionResult> AddVendor(AddVendorCommand addVendorCommand)
        {
            var result = await _mediator.Send(addVendorCommand);
            if (result.Success)
            {
                return ReturnFormattedResponse(result);
            }
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Update Vendor
        /// </summary>
        /// <param name="updateVendorCommand"></param>
        /// <returns></returns>
        [HttpPut(Name = "UpdateVendor")]
        [Produces("application/json", "application/xml", Type = typeof(VendorDto))]
        public async Task<IActionResult> UpdateVendor(UpdateVendorCommand updateVendorCommand)
        {
            var result = await _mediator.Send(updateVendorCommand);
            if (result.Success)
            {
                return ReturnFormattedResponse(result);
            }
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Delete Vendor
        /// </summary>
        /// <param name="deleteVendorCommand"></param>
        /// <returns></returns>
        [HttpDelete(Name = "DeleteVendor")]
        [Produces("application/json", "application/xml", Type = typeof(bool))]
        public async Task<IActionResult> DeleteVendor(DeleteVendorCommand deleteVendorCommand)
        {
            var result = await _mediator.Send(deleteVendorCommand);
            if (result.Success)
            {
                return ReturnFormattedResponse(result);
            }
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Get Vendor.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Vendor/{id}", Name = "GetVendor")]
        [Produces("application/json", "application/xml", Type = typeof(VendorDto))]
        public async Task<IActionResult> GetVendor(Guid id)
        {
            var getVendorCommand = new GetVendorCommand { Id = id };
            var result = await _mediator.Send(getVendorCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Get Vendors.
        /// </summary>
        /// <returns></returns>
        [HttpGet("Vendors")]
        [Produces("application/json", "application/xml", Type = typeof(List<VendorDto>))]
        public async Task<IActionResult> GetVendors()
        {
            var getAllVendorCommand = new GetAllVendorCommand { };
            var result = await _mediator.Send(getAllVendorCommand);
            return Ok(result);
        }

    }
}
