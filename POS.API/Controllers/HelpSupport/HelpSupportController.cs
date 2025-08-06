using BTTEM.Data.Resources;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.HelpSupport.Command;
using MediatR;
using BTTEM.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.API.Controllers;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using BTTEM.Data.Dto;
using System;
using BTTEM.Data.Entities;

namespace BTTEM.API.Controllers.HelpSupport
{
    [Route("api/[controller]")]
    [ApiController]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'HelpSupportController'
    public class HelpSupportController : BaseController
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'HelpSupportController'
    {
        readonly IMediator _mediater;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'HelpSupportController.HelpSupportController(IMediator)'
        public HelpSupportController(IMediator mediator)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'HelpSupportController.HelpSupportController(IMediator)'
        {
            _mediater = mediator;
        }

        
#pragma warning disable CS1570 // XML comment has badly formed XML -- 'Whitespace is not allowed at this location.'
/// <summary>
        /// Get Help & Support
        /// </summary>
        /// <param name="helpSupportResource"></param>
        /// <returns></returns>
        [HttpGet]
#pragma warning restore CS1570 // XML comment has badly formed XML -- 'Whitespace is not allowed at this location.'
        public async Task<IActionResult> GetHelpSupport([FromQuery] HelpSupportResource helpSupportResource)
        {
            var getAllHelpSupportQuery = new GetAllHelpSupportQuery()
            {
                HelpSupportResource = helpSupportResource
            };
            var result = await _mediater.Send(getAllHelpSupportQuery);

            var paginationMetaData = new
            {
                totalCount = result.Count,
                pageSize = result.PageSize,
                skip = result.Skip,
                totalPages = result.TotalPages,
            };
            Response.Headers.Add("X-Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetaData));
            HelpSupportResponse response = new HelpSupportResponse();
            response.Description = result.FirstOrDefault().Description;
            result.ForEach(responseItem =>
            {
                response.helpSupportQueries.Add(new HelpSupportQuery
                {
                    Id= responseItem.Id,
                    Answer= responseItem.Answer,
                    Question= responseItem.Question,
                    Title = responseItem.Title,
                });
            });
            return Ok(response);
        }       
    }
}
