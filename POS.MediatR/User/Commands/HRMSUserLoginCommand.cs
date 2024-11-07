using MediatR;
using POS.Data.Dto;
using POS.Helper;


namespace BTTEM.MediatR.CommandAndQuery
{
    public class HRMSUserLoginCommand : IRequest<ServiceResponse<UserAuthDto>>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
