using BTTEM.Data.Resources;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Grade.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.API.Controllers;
using POS.API.Helpers;
using POS.Data.Resources;
using POS.MediatR.City.Commands;
using POS.MediatR.CommandAndQuery;
using System;
using System.Threading.Tasks;

namespace BTTEM.API.Controllers.Grade
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradeController : BaseController
    {
        readonly IMediator _mediator;

        public GradeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get All Grades
        /// </summary>
        /// <param name="gradeResource"></param>
        /// <returns></returns>

        [HttpGet(Name = "GetGrades")]       
        public async Task<IActionResult> GetGrades([FromQuery] GradeResource gradeResource)
        {
            var getAllGradeQuery = new GetAllGradeQuery
            {
                GradeResource = gradeResource
            };
            var result = await _mediator.Send(getAllGradeQuery);

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
        /// Add Grade
        /// </summary>
        /// <param name="addGradeCommand"></param>
        /// <returns></returns>
        [HttpPost]       
        public async Task<IActionResult> AddGrade([FromBody] AddGradeCommand addGradeCommand)
        {
            var result = await _mediator.Send(addGradeCommand);
            return ReturnFormattedResponse(result);
        }


        /// <summary>
        /// Update Grade By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateGradeCommand"></param>
        /// <returns></returns>
        [HttpPut("{id}")]        
        public async Task<IActionResult> UpdateGrade(Guid id, [FromBody] UpdateGradeCommand updateGradeCommand)
        {
            updateGradeCommand.Id = id;
            var result = await _mediator.Send(updateGradeCommand);
            return ReturnFormattedResponse(result);
        }


        /// <summary>
        /// Delete Grade By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]        
        public async Task<IActionResult> DeleteGrade(Guid id)
        {
            var deleteCityCommand = new DeleteGradeCommand { Id = id };
            var result = await _mediator.Send(deleteCityCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Get All Employee Grades
        /// </summary>
        /// <param name="empGradeResource"></param>
        /// <returns></returns>

        [HttpGet("GetEmployeeGrades")]
        public async Task<IActionResult> GetEmployeeGrades([FromQuery] EmpGradeResource empGradeResource)
        {
            var getAllEmpGradeQuery = new GetAllEmpGradeQuery
            {
                EmpGradeResource = empGradeResource
            };
            var result = await _mediator.Send(getAllEmpGradeQuery);

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
        /// Add Employee Grade
        /// </summary>
        /// <param name="addEmpGradeCommand"></param>
        /// <returns></returns>
        [HttpPost("AddEmployeeGrade")]
        public async Task<IActionResult> AddEmployeeGrade([FromBody] AddEmpGradeCommand addEmpGradeCommand)
        {
            var result = await _mediator.Send(addEmpGradeCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Update Employee Grade By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateEmpGradeCommand"></param>
        /// <returns></returns>
        [HttpPut("UpdateEmployeeGrade/{id}")]
        public async Task<IActionResult> UpdateEmployeeGrade(Guid id, [FromBody] UpdateEmpGradeCommand updateEmpGradeCommand)
        {
            updateEmpGradeCommand.Id = id;
            var result = await _mediator.Send(updateEmpGradeCommand);
            return ReturnFormattedResponse(result);
        }


        /// <summary>
        /// Delete Employee Grade By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteEmployeeGrade/{id}")]
        public async Task<IActionResult> DeleteEmployeeGrade(Guid id)
        {
            var deleteEmpGradeommand = new DeleteEmpGradeCommand { Id = id };
            var result = await _mediator.Send(deleteEmpGradeommand);
            return ReturnFormattedResponse(result);
        }

    }
}
