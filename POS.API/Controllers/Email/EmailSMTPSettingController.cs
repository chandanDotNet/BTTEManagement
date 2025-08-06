using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using Microsoft.AspNetCore.Authorization;
using POS.API.Helpers;

namespace POS.API.Controllers.Email
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'EmailSMTPSettingController'
    public class EmailSMTPSettingController : BaseController
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'EmailSMTPSettingController'
    {
        IMediator _mediator;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'EmailSMTPSettingController.EmailSMTPSettingController(IMediator)'
        public EmailSMTPSettingController(IMediator mediator)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'EmailSMTPSettingController.EmailSMTPSettingController(IMediator)'
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Create an Email SMTP Configuration.
        /// </summary>
        /// <param name="addEmailSMTPSettingCommand"></param>
        /// <returns></returns>
        [HttpPost]
        //[ClaimCheck("EMAIL_MANAGE_EMAIL_SMTP_SETTINS")]
        [Produces("application/json", "application/xml", Type = typeof(EmailSMTPSettingDto))]
        public async Task<IActionResult> AddEmailSMTPSetting(AddEmailSMTPSettingCommand addEmailSMTPSettingCommand)
        {
            var result = await _mediator.Send(addEmailSMTPSettingCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Get Email SMTP Configuration.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ClaimCheck("EMAIL_MANAGE_EMAIL_SMTP_SETTINS")]
        [Produces("application/json", "application/xml", Type = typeof(EmailSMTPSettingDto))]
        public async Task<IActionResult> GetEmailSMTPSetting(Guid id)
        {
            var query = new GetEmailSMTPSettingQuery() { Id = id };
            var result = await _mediator.Send(query);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Get Email SMTP Configuration list.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ClaimCheck("EMAIL_MANAGE_EMAIL_SMTP_SETTINS")]
        [Produces("application/json", "application/xml", Type = typeof(List<EmailSMTPSettingDto>))]
        public async Task<IActionResult> GetEmailSMTPSettings()
        {
            var query = new GetEmailSMTPSettingsQuery() { };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        
#pragma warning disable CS1572 // XML comment has a param tag for 'addEmailSMTPSettingCommand', but there is no parameter by that name
/// <summary>
        /// Update an Email SMTP Configuration.
        /// </summary>
        /// <param name="addEmailSMTPSettingCommand"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
#pragma warning restore CS1572 // XML comment has a param tag for 'addEmailSMTPSettingCommand', but there is no parameter by that name
        [ClaimCheck("EMAIL_MANAGE_EMAIL_SMTP_SETTINS")]
        [Produces("application/json", "application/xml", Type = typeof(EmailSMTPSettingDto))]
#pragma warning disable CS1573 // Parameter 'updateEmailSMTPSettingCommand' has no matching param tag in the XML comment for 'EmailSMTPSettingController.UpdateEmailSMTPSetting(Guid, UpdateEmailSMTPSettingCommand)' (but other parameters do)
#pragma warning disable CS1573 // Parameter 'id' has no matching param tag in the XML comment for 'EmailSMTPSettingController.UpdateEmailSMTPSetting(Guid, UpdateEmailSMTPSettingCommand)' (but other parameters do)
        public async Task<IActionResult> UpdateEmailSMTPSetting(Guid id, UpdateEmailSMTPSettingCommand updateEmailSMTPSettingCommand)
#pragma warning restore CS1573 // Parameter 'id' has no matching param tag in the XML comment for 'EmailSMTPSettingController.UpdateEmailSMTPSetting(Guid, UpdateEmailSMTPSettingCommand)' (but other parameters do)
#pragma warning restore CS1573 // Parameter 'updateEmailSMTPSettingCommand' has no matching param tag in the XML comment for 'EmailSMTPSettingController.UpdateEmailSMTPSetting(Guid, UpdateEmailSMTPSettingCommand)' (but other parameters do)
        {
            updateEmailSMTPSettingCommand.Id = id;
            var result = await _mediator.Send(updateEmailSMTPSettingCommand);
            return ReturnFormattedResponse(result);
        }

        
#pragma warning disable CS1572 // XML comment has a param tag for 'addEmailSMTPSettingCommand', but there is no parameter by that name
/// <summary>
        /// Delete an Email SMTP Configuration.
        /// </summary>
        /// <param name="addEmailSMTPSettingCommand"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
#pragma warning restore CS1572 // XML comment has a param tag for 'addEmailSMTPSettingCommand', but there is no parameter by that name
        [ClaimCheck("EMAIL_MANAGE_EMAIL_SMTP_SETTINS")]
        [Produces("application/json", "application/xml", Type = typeof(EmailSMTPSettingDto))]
#pragma warning disable CS1573 // Parameter 'id' has no matching param tag in the XML comment for 'EmailSMTPSettingController.DeleteEmailSMTPSetting(Guid)' (but other parameters do)
        public async Task<IActionResult> DeleteEmailSMTPSetting(Guid id)
#pragma warning restore CS1573 // Parameter 'id' has no matching param tag in the XML comment for 'EmailSMTPSettingController.DeleteEmailSMTPSetting(Guid)' (but other parameters do)
    {
            var deleteEmailSMTPSettingCommand = new DeleteEmailSMTPSettingCommand() { Id = id };
            var result = await _mediator.Send(deleteEmailSMTPSettingCommand);
            return ReturnFormattedResponse(result);
        }
    }
}
