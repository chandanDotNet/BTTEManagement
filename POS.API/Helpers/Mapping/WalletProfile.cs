using AutoMapper;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;
using POS.Data.Dto;
using POS.Data;
using POS.MediatR.CommandAndQuery;

namespace BTTEM.API.Helpers.Mapping
{
    public class WalletProfile :Profile
    {

        public WalletProfile()
        {
            CreateMap<Wallet, WalletDto>().ReverseMap();           
            CreateMap<AddWalletCommand, Wallet>();
          
        }
    }
}
