using BTTEM.MediatR.CommandAndQuery;
using BTTEM.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Handlers
{
    public class DonwloadCompanyAccountLogoCommandHandler : IRequestHandler<DonwloadCompanyAccountLogoCommand, string>
    {

        private readonly ICompanyAccountRepository _companyAccountRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;
        public DonwloadCompanyAccountLogoCommandHandler(
            ICompanyAccountRepository companyAccountRepository,
            IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper)
        {
            _companyAccountRepository = companyAccountRepository;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
        }

        public async Task<string> Handle(DonwloadCompanyAccountLogoCommand request, CancellationToken cancellationToken)
        {
            var expense = await _companyAccountRepository.FindAsync(request.Id);
            if (expense == null)
            {
                return "";
            }
            string contentRootPath = _webHostEnvironment.WebRootPath;
            return Path.Combine(contentRootPath, _pathHelper.CompanyLogo, expense.ReceiptPath);
        }
    }
}
