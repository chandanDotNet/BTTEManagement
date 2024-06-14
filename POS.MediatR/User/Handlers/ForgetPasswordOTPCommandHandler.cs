using BTTEM.MediatR.User.Commands;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.User.Handlers
{
    public class ForgetPasswordOTPCommandHandler : IRequestHandler<ForgetPasswordOTPCommand, ServiceResponse<string>>
    {
        private readonly IEmailSMTPSettingRepository _emailSMTPSettingRepository;
        private readonly UserManager<POS.Data.User> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<ForgetPasswordOTPCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly POS.Helper.PathHelper _pathHelper;
        public ForgetPasswordOTPCommandHandler(IUserRepository userRepository, IUnitOfWork<POSDbContext> uow,
            ILogger<ForgetPasswordOTPCommandHandler> logger,
            UserManager<POS.Data.User> userManager, IEmailSMTPSettingRepository emailSMTPSettingRepository,
            IWebHostEnvironment webHostEnvironment, POS.Helper.PathHelper pathHelper)
        {
            _userRepository = userRepository;
            _uow = uow;
            _logger = logger;
            _userManager = userManager;
            _emailSMTPSettingRepository = emailSMTPSettingRepository;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
        }

        public async Task<ServiceResponse<string>> Handle(ForgetPasswordOTPCommand request, CancellationToken cancellationToken)
        {
            Random rnd = new Random();
            int otp = rnd.Next(1000, 9999);
            //var user = await _userRepository.All.Where(u => u.UserName == request.UserName).FirstOrDefaultAsync();
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                _logger.LogError("Invalid OTP.");
                return ServiceResponse<string>.ReturnFailed(404, "User not found.");
            }
            user.OTP = otp.ToString();
            _userRepository.Update(user);
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<string>.Return500();
            }

            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Template", "OtpTemplate.html");

            var defaultSmtp = await _emailSMTPSettingRepository.FindBy(c => c.IsDefault).FirstOrDefaultAsync();
            using (StreamReader sr = new StreamReader(filePath))
            {
                string templateBody = sr.ReadToEnd();
                templateBody = templateBody.Replace("{NAME}", string.Concat(user.FirstName, " ", user.LastName));
                templateBody = templateBody.Replace("{DATETIME}", DateTime.Now.ToString("dddd, dd MMMM yyyy"));
                templateBody = templateBody.Replace("{OTP}", otp.ToString());
                EmailHelper.SendEmail(new SendEmailSpecification
                {
                    Body = templateBody,
                    FromAddress = defaultSmtp.UserName,
                    Host = defaultSmtp.Host,
                    IsEnableSSL = defaultSmtp.IsEnableSSL,
                    Password = defaultSmtp.Password,
                    Port = defaultSmtp.Port,
                    Subject = "OTP",
                    ToAddress = request.UserName,
                    UserName = defaultSmtp.UserName
                });
            }
            return ServiceResponse<string>.ReturnResultWith200(otp.ToString());
        }
    }
}
