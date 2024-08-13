using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS.Data.Resources;
using POS.Repository;
using POS.API.Helpers;
using BTTEM.MediatR.User.Commands;
using BTTEM.MediatR.Commands;
using System.Linq.Dynamic.Core;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authentication;
using System.Text.Json;
using Newtonsoft.Json;
using BTTEM.Data.Entities;
using System.Security.Cryptography.X509Certificates;
using BTTEM.Repository;
using BTTEM.MediatR.Department.Commands;
using BTTEM.MediatR.Grade.Commands;
using BTTEM.MediatR.CompanyProfile.Commands;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using BTTEM.MediatR;

namespace POS.API.Controllers
{
    /// <summary>
    /// User
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : BaseController
    {
        public IMediator _mediator { get; set; }
        public readonly UserInfoToken _userInfo;
        public readonly IUserRepository _userRepository;
        public readonly IDepartmentRepository _departmentRepository;
        public readonly IPoliciesDetailRepository _policiesDetailRepository;
        public readonly IGradeRepository _gradeRepository;
        public readonly ICompanyAccountRepository _companyAccountRepository;
        public readonly IPoliciesVehicleConveyanceRepository _policiesVehicleConveyanceRepository;
        private readonly IMapper _mapper;
        /// <summary>
        /// User
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="userInfo"></param>
        public UserController(
            IMediator mediator,
            UserInfoToken userInfo, IUserRepository userRepository,
            IDepartmentRepository departmentRepository, IGradeRepository gradeRepository,
             ICompanyAccountRepository companyAccountRepository,
             IMapper mapper,
             IPoliciesVehicleConveyanceRepository policiesVehicleConveyanceRepository,
             IPoliciesDetailRepository policiesDetailRepository
            )
        {
            _mediator = mediator;
            _userInfo = userInfo;
            _userRepository = userRepository;
            _departmentRepository = departmentRepository;
            _gradeRepository = gradeRepository;
            _companyAccountRepository = companyAccountRepository;
            _mapper = mapper;
            _policiesVehicleConveyanceRepository = policiesVehicleConveyanceRepository;
            _policiesDetailRepository = policiesDetailRepository;
        }
        /// <summary>
        ///  Create a User
        /// </summary>
        /// <param name="addUserCommand"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        //[ClaimCheck("USR_ADD_USER")]
        [Produces("application/json", "application/xml", Type = typeof(UserDto))]
        public async Task<IActionResult> AddUser(AddUserCommand addUserCommand)
        {
            var result = await _mediator.Send(addUserCommand);
            if (!result.Success)
            {
                return ReturnFormattedResponse(result);
            }
            return CreatedAtAction("GetUser", new { id = result.Data.Id }, result.Data);
        }


        /// <summary>
        /// Get All Users
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllUsers")]
        [ClaimCheck("USR_VIEW_USERS")]
        [Produces("application/json", "application/xml", Type = typeof(List<UserDto>))]
        public async Task<IActionResult> GetAllUsers()
        {
            var getAllUserQuery = new GetAllUserQuery { };
            var result = await _mediator.Send(getAllUserQuery);
            return Ok(result);
        }

        /// <summary>
        /// Get User By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetUser")]
        [ClaimCheck("USR_VIEW_USERS")]
        [Produces("application/json", "application/xml", Type = typeof(UserDto))]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var getUserQuery = new GetUserQuery { Id = id };
            var result = await _mediator.Send(getUserQuery);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Get Use Notification Coung.
        /// </summary>
        /// <returns></returns>
        [HttpGet("notification/count")]
        [Produces("application/json", "application/xml", Type = typeof(int))]
        public async Task<IActionResult> GetUserNotificationCount()
        {
            var getUserNotificationCountQuery = new GetUserNotificationCountQuery { };
            var result = await _mediator.Send(getUserNotificationCountQuery);
            return Ok(result);
        }

        /// <summary>
        /// Get Users
        /// </summary>
        /// <param name="userResource"></param> 
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("GetUsers")]
        //[ClaimCheck("USR_VIEW_USERS")]
        [Produces("application/json", "application/xml", Type = typeof(UserList))]
        public async Task<IActionResult> GetUsers([FromQuery] UserResource userResource)
        {
            var getAllLoginAuditQuery = new GetUsersQuery
            {
                UserResource = userResource
            };
            var result = await _mediator.Send(getAllLoginAuditQuery);

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
        /// Get Recently Registered Users 
        /// </summary>
        /// <returns></returns> 
        [HttpGet("GetRecentlyRegisteredUsers")]
        [ClaimCheck("USR_VIEW_USERS")]
        [Produces("application/json", "application/xml", Type = typeof(List<UserDto>))]
        public async Task<IActionResult> GetRecentlyRegisteredUsers()
        {
            var getRecentlyRegisteredUserQuery = new GetRecentlyRegisteredUserQuery { };
            var result = await _mediator.Send(getRecentlyRegisteredUserQuery);
            return Ok(result);
        }


        /// <summary>
        /// Update User By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateUserCommand"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        // [ClaimCheck("USR_UPDATE_USER")]
        [Produces("application/json", "application/xml", Type = typeof(UserDto))]
        public async Task<IActionResult> UpdateUser(Guid id, UpdateUserCommand updateUserCommand)
        {
            updateUserCommand.Id = id;
            var result = await _mediator.Send(updateUserCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateUserProfileCommand"></param>
        /// <returns></returns>
        [HttpPut("profile")]
        //[ClaimCheck("USR_UPDATE_USER")]
        [Produces("application/json", "application/xml", Type = typeof(UserDto))]
        public async Task<IActionResult> UpdateUserProfile(UpdateUserProfileCommand updateUserProfileCommand)
        {
            var result = await _mediator.Send(updateUserProfileCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Update Profile photo
        /// </summary>
        /// <returns></returns>
        [HttpPost("UpdateUserProfilePhoto"), DisableRequestSizeLimit]
        // [ClaimCheck("USR_UPDATE_USER")]
        [Produces("application/json", "application/xml", Type = typeof(UserDto))]
        public async Task<IActionResult> UpdateUserProfilePhoto()
        {
            var updateUserProfilePhotoCommand = new UpdateUserProfilePhotoCommand()
            {
                FormFile = Request.Form.Files,
            };
            var result = await _mediator.Send(updateUserProfilePhotoCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Delete User By Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("{Id}")]
        [ClaimCheck("USR_DELETE_USER")]
        public async Task<IActionResult> DeleteUser(Guid Id)
        {
            var deleteUserCommand = new DeleteUserCommand { Id = Id };
            var result = await _mediator.Send(deleteUserCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// User Change Password
        /// </summary>
        /// <param name="resetPasswordCommand"></param>
        /// <returns></returns>
        [HttpPost("changepassword")]
        [ClaimCheck("USR_RESET_PWD")]
        public async Task<IActionResult> ChangePassword(ChangePasswordCommand resetPasswordCommand)
        {
            var result = await _mediator.Send(resetPasswordCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Reset Resetpassword
        /// </summary>
        /// <param name="newPasswordCommand"></param>
        /// <returns></returns>
        [HttpPost("resetpassword")]
        [ClaimCheck("USR_RESET_PWD")]
        public async Task<IActionResult> ResetPassword(ResetPasswordCommand newPasswordCommand)
        {
            var result = await _mediator.Send(newPasswordCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Get User Profile
        /// </summary>
        /// <returns></returns>
        [HttpGet("profile")]
        [ClaimCheck("USR_VIEW_USERS")]
        public async Task<IActionResult> GetProfile()
        {
            var getUserQuery = new GetUserQuery
            {
                Id = Guid.Parse(_userInfo.Id)
            };
            var result = await _mediator.Send(getUserQuery);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Forget password OTP
        /// </summary>
        /// <param name="forgetPasswordOTPCommand"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("forgetpasswordotp")]
        public async Task<IActionResult> ForgetPasswordOTP(ForgetPasswordOTPCommand forgetPasswordOTPCommand)
        {
            var result = await _mediator.Send(forgetPasswordOTPCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// OTP Verification
        /// </summary>
        /// <param name="otpVerficationCommand"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("otpVerification")]
        public async Task<IActionResult> OTPVerification(OTPVerificationCommand otpVerficationCommand)
        {
            var result = await _mediator.Send(otpVerficationCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Forget password
        /// </summary>
        /// <param name="forgetPasswordCommand"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("forgetpassword")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordCommand forgetPasswordCommand)
        {
            var result = await _mediator.Send(forgetPasswordCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Assigned Employee Count
        /// </summary>        
        /// <returns></returns>        
        [HttpGet("assignedEmployeeCount")]
        public async Task<IActionResult> AssignedEmployeeCount()
        {
            //var count = _userRepository.All.Where(u => u.ReportingTo.Value == Guid.Parse(_userInfo.Id)).Count();
            var count = _userRepository.All.Where(u => u.ReportingTo.Value == Guid.Parse(_userInfo.Id)).Count();
            return Ok(count);
            //return ReturnFormattedResponse();
        }

        /// <summary>
        /// Department Sync
        /// </summary>        
        /// <returns></returns>    
        [AllowAnonymous]
        [HttpGet("SyncDepartment")]
        public async Task<IActionResult> DepartmentSync()
        {
            var requestUri = "https://shyamsteel.tech:8002/tour_and_travels_all_user_list/?all_data=true";
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get,
                requestUri);
            request.Headers.Add("api-key", "3d4da1b5-0124-48fd-bba3-257e309333db");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ShyamSteel>(responseData);

            var department = result.all_users.Select(dept => new { dept.department_name, dept.department }).Distinct().ToList();
            AddDepartmentListCommand dptList = new AddDepartmentListCommand();
            department.ForEach(dept =>
            {
                dptList.DepartmentList.Add(new BTTEM.Data.Dto.DepartmentDto()
                {
                    DepartmentName = dept.department_name,
                    DepartmentCode = dept.department.ToString()
                });
            });

            var dptResult = await _mediator.Send(dptList);
            return Ok(dptList);
        }

        /// <summary>
        /// Grade Sync
        /// </summary>        
        /// <returns></returns>    
        [AllowAnonymous]
        [HttpGet("SyncGrade")]
        public async Task<IActionResult> GradeSync()
        {
            var requestUri = "https://shyamsteel.tech:8002/tour_and_travels_all_user_list/?all_data=true";
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get,
                requestUri);
            request.Headers.Add("api-key", "3d4da1b5-0124-48fd-bba3-257e309333db");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ShyamSteel>(responseData);

            var grade = result.all_users.Select(grd => new { grd.grade, grd.employee_grade }).Distinct().ToList();

            AddGradeListCommand grdList = new AddGradeListCommand();
            grade.ForEach(grd =>
            {
                if (grd.grade != null)
                {
                    grdList.GradeList.Add(new BTTEM.Data.GradeDto()
                    {
                        GradeName = grd.grade.ToString(),
                        GradeCode = grd.employee_grade.ToString(),
                    });
                }
            });
            var grdResult = await _mediator.Send(grdList);
            return Ok(grdList);
        }

        /// <summary>
        /// Grade Sync
        /// </summary>        
        /// <returns></returns>    
        [AllowAnonymous]
        [HttpGet("SyncCompany")]
        public async Task<IActionResult> CompanySync()
        {
            var requestUri = "https://shyamsteel.tech:8002/tour_and_travels_all_user_list/?all_data=true";
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get,
                requestUri);
            request.Headers.Add("api-key", "3d4da1b5-0124-48fd-bba3-257e309333db");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ShyamSteel>(responseData);

            var company = result.all_users.Select(c => new { c.company_name, c.company }).Distinct().ToList();

            AddCompanyAccountsCommand companyAccounts = new AddCompanyAccountsCommand();
            company.ForEach(item =>
            {
                companyAccounts.CompanyAccountsList.Add(new BTTEM.Data.Dto.CompanyAccountDto()
                {
                    AccountName = item.company_name,
                    AccountCode = item.company.ToString(),
                });
            });
            var grdResult = await _mediator.Send(companyAccounts);
            return Ok(companyAccounts);
        }

        /// <summary>
        /// Get Employees Details
        /// </summary>        
        /// <returns></returns>    
        [AllowAnonymous]
        [HttpGet("SyncEmployee")]
        public async Task<IActionResult> EmployeeSync(string companyName)
        {
            var requestUri = "https://shyamsteel.tech:8002/tour_and_travels_all_user_list/?all_data=true";
            var client = new HttpClient();
            if (!string.IsNullOrEmpty(companyName))
            {
                requestUri = requestUri + "&company_name=" + companyName;
            }

            var request = new HttpRequestMessage(HttpMethod.Get,
                requestUri);
            request.Headers.Add("api-key", "3d4da1b5-0124-48fd-bba3-257e309333db");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ShyamSteel>(responseData);

            if (result.all_users.Count > 0)
            {
                List<AddUserCommand> addUserCommand = new List<AddUserCommand>();
                result.all_users.ForEach(item =>
                {
                    //var name = item.employee_name.Split(" ");
                    addUserCommand.Add(new AddUserCommand()
                    {
                        AadhaarNo = item.aadhar_no,
                        Designation = item.designation_name,
                        BankName = item.bank_name,
                        SapCode = item.sap_personnel_no,
                        UserName = !string.IsNullOrEmpty(item.official_email_id) ? item.official_email_id : item.employee_code + "@sft.com",
                        Email = !string.IsNullOrEmpty(item.official_email_id) ? item.official_email_id : item.employee_code + "@sft.com",
                        ReportingToName = item.reporting_head_name,
                        PhoneNumber = item.official_contact_no,
                        PanNo = item.pan_no,
                        DateOfBirth = item.dob != null ? Convert.ToDateTime(item.dob) : null,
                        DateOfJoining = item.date_of_joining,
                        EmployeeCode = item.employee_code,
                        Address = item.address,
                        IsActive = item.is_active,
                        IFSC = item.ifsc_code,
                        AccountNumber = item.bank_account,
                        FirstName = item.employee_name,
                        LastName = item.employee_name,
                        Password = "sft@123",
                        //Department = item.department.Value != null ? _departmentRepository.All.Where(d => d.DepartmentCode == item.department.ToString()).FirstOrDefault().Id : Guid.Empty,
                        //GradeId = item.employee_grade.Value != null ? _gradeRepository.All.Where(d => d.GradeCode == item.employee_grade.ToString()).FirstOrDefault().Id : Guid.Empty,
                        BranchName = item.branch_name,
                        CompanyAccountId = _companyAccountRepository.All.Where(d => d.AccountName == item.company_name).FirstOrDefault().Id,
                        HrmsId = item.id,
                        HrmsReportingHeadCode = item.reporting_head,
                        HrmsUser = item.user,
                        HrmsDepartmentId = item.department,
                        HrmsGradeId = item.employee_grade

                    });
                });

                foreach (var item in addUserCommand)
                {
                    var userResult = await _mediator.Send(item);
                }
            }
            return Ok(result);


            //var designation = result.all_users.DistinctBy(d => d.designation_name).ToList();
            //var department = result.all_users.Select(dept => dept.department_name).Distinct().ToList();
            //AddDepartmentListCommand dptList = new AddDepartmentListCommand();
            //department.ForEach(dept =>
            //{
            //    dptList.DepartmentList.Add(new BTTEM.Data.Dto.DepartmentDto()
            //    {
            //        DepartmentName = dept
            //    });
            //});
            //var dptResult = await _mediator.Send(dptList);
            //var grade = result.all_users.Select(grd => grd.grade).Distinct();
            //var company = result.all_users.Select(c => c.company_name).Distinct();
        }


        /// <summary>
        /// Update Employee
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPut("UpdateEmployee")]
        [Produces("application/json", "application/xml", Type = typeof(UserDto))]
        public async Task<IActionResult> UpdateEmployee()
        {
            UpdateUserCommand updateUserCommand = new UpdateUserCommand();
            var entity = await _userRepository.All.ToListAsync();
            foreach (var item in entity)
            {
                updateUserCommand = _mapper.Map<UpdateUserCommand>(item);
                updateUserCommand.Id = item.Id;
                updateUserCommand.FirstName = "Sync";
                updateUserCommand.UserRoles.Add(new UserRoleDto
                {
                    RoleId = new Guid("E1BD3DCE-EECF-468D-B930-1875BD59D1F4"),
                    UserId = item.Id,
                });
                //updateUserCommand.ReportingTo = _userRepository.All.Where(x => x.ReportingToName == item.ReportingToName).FirstOrDefault().Id;
                var result = await _mediator.Send(updateUserCommand);
            }
            return Ok();
        }

        /// <summary>
        /// Get User Info Details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetUserInfoDetails/{id}")]
        public async Task<IActionResult> GetUserInfoDetails(Guid id)
        {
            var ReportQuery = new GetUserInfoDetailsQuery { UserId = id };
            var result = await _mediator.Send(ReportQuery);

            return Ok(result);
        }

        /// <summary>
        /// Delete User By Email
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>       
        [HttpDelete("DeleteUserByEmail/{Email}")]
        public async Task<IActionResult> DeleteUserByEmail(string Email)
        {
            var deleteUserCommandByEmail = new DeleteUserCommandByEmail { Email = Email };
            var result = await _mediator.Send(deleteUserCommandByEmail);

            UserDeleteResponse response = new UserDeleteResponse();
            if (result.Success)
            {
                response.status = true;
                response.StatusCode = result.StatusCode;
                response.message = "User Deleted Successfully";
            }
            else
            {
                response.status = false;
                response.StatusCode = result.StatusCode;
                response.message = "User not exists";
            }
            return Ok(response);
            //return ReturnFormattedResponse(result);
        }


        /// <summary>
        /// Get Policy Conveyance
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="FuelType"></param>
        /// <param name="VehicleType"></param>
        /// <returns></returns>       
        [HttpGet("GetPolicyConveyanceRates/{UserId},{VehicleType},{FuelType}")]
        public async Task<IActionResult> GetPolicyConveyanceRates(Guid UserId, string VehicleType, string FuelType)
        {
            VehicleType = VehicleType == "Car" ? "4 Wheeler" : VehicleType == "Bike" ? "Two Wheeler" : "";

            var userDetails = await _userRepository.FindAsync(UserId);

            if (userDetails != null)
            {
                var policyDetails = await _policiesDetailRepository.All.
                FirstOrDefaultAsync(x => x.CompanyAccountId == userDetails.CompanyAccountId && x.GradeId == userDetails.GradeId);

                var conveyanceRates =
                await _policiesVehicleConveyanceRepository.AllIncluding(v => v.VehicleManagement)
                .Where(x => x.PoliciesDetailId == policyDetails.Id &&
                 x.VehicleManagement.FuelType == FuelType && x.VehicleManagement.Name == VehicleType).ToListAsync();

                return Ok(conveyanceRates);
            }
            //if (result.Success)
            //{
            //    response.status = true;
            //    response.StatusCode = result.StatusCode;
            //    response.message = "User Deleted Successfully";
            //}
            //else
            //{
            //    response.status = false;
            //    response.StatusCode = result.StatusCode;
            //    response.message = "User not exists";
            //}
            //return Ok(response);

            return BadRequest();
        }
    }
}
