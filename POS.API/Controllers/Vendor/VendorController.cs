using BTTEM.Data.Dto;
using BTTEM.Data.Resources;
using BTTEM.MediatR.Branch.Command;
using BTTEM.MediatR.Command;
using BTTEM.MediatR.Vendor.Command;
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
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'VendorController'
    public class VendorController : BaseController
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'VendorController'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'VendorController._mediator'
        public IMediator _mediator { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'VendorController._mediator'
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'VendorController.VendorController(IMediator)'
        public VendorController(IMediator mediator)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'VendorController.VendorController(IMediator)'
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
        [HttpGet("GetAllVendors")]
        [Produces("application/json", "application/xml", Type = typeof(List<VendorDto>))]
        public async Task<IActionResult> GetAllVendors()
        {
            var getAllVendorCommand = new GetAllVendorCommand { };
            var result = await _mediator.Send(getAllVendorCommand);
            return Ok(result);
        }

        [HttpGet("GetVendors")]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'VendorController.GetVendors(VendorResource)'
        public async Task<IActionResult> GetVendors([FromQuery] VendorResource vendorResource)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'VendorController.GetVendors(VendorResource)'
        {
            var getAllVendorQueryCommand = new GetAllVendorQueryCommand()
            {
                VendorResource = vendorResource
            };
            var result = await _mediator.Send(getAllVendorQueryCommand);
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
    }
}
