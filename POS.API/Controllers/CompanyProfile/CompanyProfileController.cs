using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using POS.API.Helpers;
using BTTEM.MediatR.CompanyProfile.Commands;
using POS.Data.Resources;
using BTTEM.Data.Resources;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.Data.Dto;
using System;
using System.IO;
using Microsoft.AspNetCore.StaticFiles;

namespace POS.API.Controllers.CompanyProfile
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompanyProfileController : BaseController
    {

        public IMediator _mediator { get; set; }

        /// <summary>
        /// Action
        /// </summary>
        /// <param name="mediator"></param>
        public CompanyProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get CompanyProfile
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Produces("application/json", "application/xml", Type = typeof(CompanyProfileDto))]
        public async Task<IActionResult> GetCompanyProfile()
        {
            var getCompanyProfileQuery = new GetCompanyProfileQuery { };
            var result = await _mediator.Send(getCompanyProfileQuery);
            return Ok(result);
        }
       
        /// <summary>
        /// Update Company Profile
        /// </summary>
        /// <param name="updateCompanyProfileCommand"></param>
        /// <returns></returns>
        [HttpPost]
        [ClaimCheck("SETT_UPDATE_COM_PROFILE")]
        [Produces("application/json", "application/xml", Type = typeof(CompanyProfileDto))]
        public async Task<IActionResult> UpdateCompanyProfile(UpdateCompanyProfileCommand updateCompanyProfileCommand)
        {
            var response = await _mediator.Send(updateCompanyProfileCommand);
            return ReturnFormattedResponse(response);
        }

        /// <summary>
        /// Update GST
        /// </summary>
        /// <param name="updateGSTCommand"></param>
        /// <returns></returns>
        [HttpPost("AddUpdateGST")]
        [Produces("application/json", "application/xml", Type = typeof(CompanyProfileDto))]
        public async Task<IActionResult> AddUpdateGST(UpdateGSTCommand updateGSTCommand)
        {
            var response = await _mediator.Send(updateGSTCommand);
            return ReturnFormattedResponse(response);
        }


        /// <summary>
        /// Get All Company Accounts
        /// </summary>
        /// <param name="companyAccountResource"></param>
        /// <returns></returns>

        [HttpGet("GetCompnayAccounts")]
        // [ClaimCheck("SETT_MANAGE_CITY")]
        public async Task<IActionResult> GetCompnayAccounts([FromQuery] CompanyAccountResource companyAccountResource)
        {
            var getAllCompnayAccountQuery = new GetAllCompanyAccountQuery
            {
                CompanyAccountResource = companyAccountResource
            };
            var result = await _mediator.Send(getAllCompnayAccountQuery);

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

        [HttpDelete("DeleteCompnayAccounts/{id}")]
        public async Task<IActionResult> DeleteCompnayAccounts(Guid id)
        {
            var deleteContactUsCommand = new DeleteCompanyCommand
            {
                Id = id
            };
            var result = await _mediator.Send(deleteContactUsCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Add Update State Wise GST
        /// </summary>
        /// <param name="addUpdateStateWiseGSTCommand"></param>
        /// <returns></returns>
        [HttpPost("AddUpdateStateWiseGST")]
        [Produces("application/json", "application/xml", Type = typeof(CompanyGSTDto))]
        public async Task<IActionResult> AddUpdateStateWiseGST(AddUpdateStateWiseGSTCommand addUpdateStateWiseGSTCommand)
        {
            var response = await _mediator.Send(addUpdateStateWiseGSTCommand);            
            return Ok(response);
            //return ReturnFormattedResponse(response);
        }

        /// <summary>
        /// Gets Company GST
        /// </summary>
        /// <param name="accountId">The identifier.</param>
        /// <returns></returns>
        [HttpGet("GetCompanyGST/{accountId}")]
        //[ClaimCheck("EXP_VIEW_EXPENSES")]
        public async Task<IActionResult> GetCompanyGST(Guid? accountId)
        {
            var query = new GetCompanyGSTQuery { CompanyAccountId = accountId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpDelete("DeleteCompanyGST/{id}")]
        public async Task<IActionResult> DeleteCompanyGST(Guid id)
        {
            var deleteContactUsCommand = new DeleteCompanyGSTCommand
            {
                Id = id
            };
            var result = await _mediator.Send(deleteContactUsCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Download Company AccountLogo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/downloadCompanyAccountLogo")]
        public async Task<IActionResult> DownloadFile(Guid id)
        {
            var commnad = new DonwloadCompanyAccountLogoCommand
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

    }
}
