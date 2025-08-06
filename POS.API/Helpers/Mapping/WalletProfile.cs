using AutoMapper;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;
using POS.Data.Dto;
using POS.Data;
using POS.MediatR.CommandAndQuery;

namespace BTTEM.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'WalletProfile'
    public class WalletProfile :Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'WalletProfile'
    {

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'WalletProfile.WalletProfile()'
        public WalletProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'WalletProfile.WalletProfile()'
        {
            CreateMap<Wallet, WalletDto>().ReverseMap();           
            CreateMap<AddWalletCommand, Wallet>();
          
        }
    }
}
