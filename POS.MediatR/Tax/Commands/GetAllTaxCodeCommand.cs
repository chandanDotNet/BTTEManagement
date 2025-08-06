using MediatR;
using System;
using System.Collections.Generic;
using POS.Data.Dto;

namespace POS.MediatR.Tax.Commands
{
    public class GetAllTaxCodeCommand : IRequest<List<TaxCodeDto>>
    {
    }
}
