using AutoMapper;
using AutoMapper.QueryableExtensions;
using BTTEM.Data;
using BTTEM.Data.Dto;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Department.Commands;
using BTTEM.MediatR.Handlers;
using BTTEM.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Domain;
using POS.MediatR.Brand.Command;
using POS.MediatR.Currency.Commands;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Department.Handlers
{
    public class GetAllDepartmentCommandHandler : IRequestHandler<GetAllDepartmentCommand, List<DepartmentDto>>
    {

        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;


        public GetAllDepartmentCommandHandler(
           IDepartmentRepository departmentRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper,
           IUserRepository userRepository


          )
        {
            _departmentRepository = departmentRepository;
            _uow = uow;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<List<DepartmentDto>> Handle(GetAllDepartmentCommand request, CancellationToken cancellationToken)
        {
            List<DepartmentDto> result = new List<DepartmentDto>(new List<DepartmentDto>());
            if (!request.Id.HasValue || request.Id.Value == Guid.Empty)
            {
                //result = await _departmentRepository.All.OrderBy(c => c.DepartmentName).ProjectTo<DepartmentDto>(_mapper.ConfigurationProvider).ToListAsync();

                var query =
                        (from post in _departmentRepository.All
                         join meta in _userRepository.All on post.DepartmentHeadId equals meta.Id  //into t from rt in t.DefaultIfEmpty()
                         select new { Post = post, Meta = meta }).OrderBy(x => x.Post.DepartmentName); 



             result = await query
            .Select(cs => new DepartmentDto
            {
                Id = cs.Post.Id,
                DepartmentName = cs.Post.DepartmentName,
                DepartmentCode = cs.Post.DepartmentCode,
                DepartmentHeadId = cs.Post.DepartmentHeadId,
                IsActive = cs.Post.IsActive,
                DepartmentHeadName = cs.Meta.FirstName + " " + cs.Meta.LastName + "(" + cs.Meta.UserName + ")",
                DepartmentHeadCount = _userRepository.All.Where(c=>c.Department==cs.Post.Id).Count(),
            }).ToListAsync();

            }
            else
            {
                // result = await _departmentRepository.All.Where(c => c.Id == request.Id).OrderBy(c => c.DepartmentName).ProjectTo<DepartmentDto>(_mapper.ConfigurationProvider).ToListAsync();

                var query =
                        (from post in _departmentRepository.All
                         join meta in _userRepository.All on post.DepartmentHeadId equals meta.Id
                         select new { Post = post, Meta = meta }).Where(c => c.Post.Id == request.Id).OrderBy(x => x.Post.DepartmentName);



                result = await query
               .Select(cs => new DepartmentDto
               {
                   Id = cs.Post.Id,
                   DepartmentName = cs.Post.DepartmentName,
                   DepartmentCode = cs.Post.DepartmentCode,
                   DepartmentHeadId = cs.Post.DepartmentHeadId,
                   IsActive = cs.Post.IsActive,
                   DepartmentHeadName = cs.Meta.FirstName + " " + cs.Meta.LastName+"("+ cs.Meta.UserName + ")",
                   DepartmentHeadCount = _userRepository.All.Where(c => c.Department == cs.Post.Id).Count(),
               }).ToListAsync();

            }

            return result;
        }


    }
}
