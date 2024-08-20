using AutoMapper;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BTTEM.Repository;
using BTTEM.Data;

namespace POS.MediatR.Handlers
{
    public class UpdateExpenseCategoryCommandHandler
        : IRequestHandler<UpdateExpenseCategoryCommand, ServiceResponse<bool>>
    {
        private readonly IExpenseCategoryRepository _expenseCategoryRepository;
        private readonly IExpenseCategoryTaxRepository _expenseCategoryTaxRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<UpdateExpenseCategoryCommandHandler> _logger;
        private readonly IMapper _mapper;
        public UpdateExpenseCategoryCommandHandler(
           IExpenseCategoryRepository expenseCategoryRepository,
            IUnitOfWork<POSDbContext> uow,
            ILogger<UpdateExpenseCategoryCommandHandler> logger,
            IMapper mapper, IExpenseCategoryTaxRepository expenseCategoryTaxRepository

            )
        {
            _expenseCategoryRepository = expenseCategoryRepository;
            _uow = uow;
            _logger = logger;
            _mapper = mapper;
            _expenseCategoryTaxRepository = expenseCategoryTaxRepository;
        }

        public async Task<ServiceResponse<bool>> Handle(UpdateExpenseCategoryCommand request, CancellationToken cancellationToken)
        {
            var existingEntity = await _expenseCategoryRepository.FindBy(c => c.Name == request.Name && c.Id != request.Id).FirstOrDefaultAsync();
            if (existingEntity != null)
            {
                _logger.LogError("Expense Category already Exists.");
                return ServiceResponse<bool>.Return409("Expense Category already Exists.");
            }
            existingEntity= await _expenseCategoryRepository.FindAsync(request.Id);

            if (existingEntity == null)
            {
                _logger.LogError("Expense Category does not Exists.");
                return ServiceResponse<bool>.Return409("Expense Category does not Exists.");
            }

            var expenseCategoryTaxes = _expenseCategoryTaxRepository.All.Where(c => c.ExpenseCategoryId == request.Id).ToList();
            if (request.ExpenseCategoryTaxes != null)
            {
                var expenseCategoryToAdd = request.ExpenseCategoryTaxes.Where(c => !expenseCategoryTaxes.Select(c => c.TaxId).Contains(c.TaxId)).ToList();
                _expenseCategoryTaxRepository.AddRange(_mapper.Map<List<ExpenseCategoryTax>>(expenseCategoryToAdd));
                var expenseCategoryTaxToDelete = expenseCategoryTaxes.Where(c => !request.ExpenseCategoryTaxes.Select(cs => cs.TaxId).Contains(c.TaxId)).ToList();
                _expenseCategoryTaxRepository.RemoveRange(expenseCategoryTaxToDelete);
            }

            existingEntity.Name = request.Name;
            existingEntity.Description = request.Description;
            existingEntity.IsActive = request.IsActive;
            existingEntity.CGST = request.CGST;
            existingEntity.SGST = request.SGST;
            existingEntity.IGST = request.IGST;
            existingEntity.SSECode= request.SSECode;
            _expenseCategoryRepository.Update(existingEntity);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving Expense Category");
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
