using BTTEM.Data.Resources;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Grade.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using BTTEM.MediatR.Department.Commands;
using POS.API.Controllers;

namespace BTTEM.API.Controllers.Department
{
    [Route("api/[controller]")]
    [ApiController]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'DepartmentController'
    public class DepartmentController : BaseController
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'DepartmentController'
    {

        readonly IMediator _mediator;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'DepartmentController.DepartmentController(IMediator)'
        public DepartmentController(IMediator mediator)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'DepartmentController.DepartmentController(IMediator)'
        {
            _mediator = mediator;
        }



        /// <summary>
        /// Get All Departments
        /// </summary>

        /// <returns></returns>

        [HttpGet(Name = "GetDepartments/{Id}")]
        public async Task<IActionResult> GetDepartments(Guid Id )
        {
            var getAllDepartmentQuery = new GetAllDepartmentCommand
            {
                Id=Id
            };
            var result = await _mediator.Send(getAllDepartmentQuery);

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
        /// Add Departments
        /// </summary>
        /// <param name="addDepartmentCommand"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddDepartments([FromBody] AddDepartmentCommand addDepartmentCommand)
        {
            var result = await _mediator.Send(addDepartmentCommand);
            return ReturnFormattedResponse(result);
        }


        /// <summary>
        /// Update Department By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateDepartmentCommand"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGrade(Guid id, [FromBody] UpdateDepartmentCommand updateDepartmentCommand)
        {
            updateDepartmentCommand.Id = id;
            var result = await _mediator.Send(updateDepartmentCommand);
            return ReturnFormattedResponse(result);
        }


        /// <summary>
        /// Delete Department By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(Guid id)
        {
            var deleteDepartmentCommand = new DeleteDepartmentCommand { Id = id };
            var result = await _mediator.Send(deleteDepartmentCommand);
            return ReturnFormattedResponse(result);
        }       
    }
}
