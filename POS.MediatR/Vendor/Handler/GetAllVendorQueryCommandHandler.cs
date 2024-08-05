using BTTEM.MediatR.Vendor.Command;
using BTTEM.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Vendor.Handler
{
    public class GetAllVendorQueryCommandHandler : IRequestHandler<GetAllVendorQueryCommand, VendorList>
    {
        private readonly IVendorRepository _vendorRepository;
        public GetAllVendorQueryCommandHandler(IVendorRepository vendorRepository)
        {
            _vendorRepository = vendorRepository;
        }

        public async Task<VendorList> Handle(GetAllVendorQueryCommand request, CancellationToken cancellationToken)
        {
            return await _vendorRepository.GetVendors(request.VendorResource);
        }
    }
}
