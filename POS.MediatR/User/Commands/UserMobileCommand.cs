using MediatR;
using System;
using POS.Helper;

namespace POS.MediatR.CommandAndQuery
{
    public class UserMobileCommand : IRequest<ServiceResponse<bool>>
    {
        public Guid Id { get; set; }
    }
}
