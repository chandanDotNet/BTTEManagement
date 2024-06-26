﻿using AutoMapper;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.Repository;
using BTTEM.Data.Entities.Expense;

namespace POS.MediatR.Handlers
{
    public class AddExpenseCommandHandler
        : IRequestHandler<AddExpenseCommand, ServiceResponse<ExpenseDto>>
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddExpenseCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;
        private readonly IExpenseDocumentRepository _expenseDocumentRepository;

        public AddExpenseCommandHandler(
            IExpenseRepository expenseRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<AddExpenseCommandHandler> logger,
            IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper,
            IExpenseDocumentRepository expenseDocumentRepository)
        {
            _expenseRepository = expenseRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
            _expenseDocumentRepository = expenseDocumentRepository;
        }

        public async Task<ServiceResponse<ExpenseDto>> Handle(AddExpenseCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Expense>(request);


            //if (!string.IsNullOrWhiteSpace(request.ReceiptName) && !string.IsNullOrWhiteSpace(request.DocumentData))
            //{
            //    string contentRootPath = _webHostEnvironment.WebRootPath;
            //    var pathToSave = Path.Combine(contentRootPath, _pathHelper.Attachments);

            //    if (!Directory.Exists(pathToSave))
            //    {
            //        Directory.CreateDirectory(pathToSave);
            //    }

            //    var extension = Path.GetExtension(request.ReceiptName); 
            //    var id = Guid.NewGuid();
            //    var path = $"{id}.{extension}";
            //    var documentPath = Path.Combine(pathToSave, path);
            //    string base64 = request.DocumentData.Split(',').LastOrDefault();
            //    if (!string.IsNullOrWhiteSpace(base64))
            //    {
            //        byte[] bytes = Convert.FromBase64String(base64);
            //        try
            //        {
            //            await File.WriteAllBytesAsync($"{documentPath}", bytes);
            //            entity.ReceiptPath = path;
            //        }
            //        catch
            //        {
            //            _logger.LogError("Error while saving files", entity);
            //        }
            //    }
            //}
            int index = 0;

            foreach (var item in entity.ExpenseDocument)
            {
                var entityExpenseDocument = _mapper.Map<ExpenseDocument>(item);
                entityExpenseDocument.ExpenseId = entity.Id;
                entityExpenseDocument.Id = Guid.NewGuid();

                if (!string.IsNullOrWhiteSpace(item.ReceiptName) && !string.IsNullOrWhiteSpace(item.ReceiptPath))
                {
                    string contentRootPath = _webHostEnvironment.WebRootPath;
                    var pathToSave = Path.Combine(contentRootPath, _pathHelper.Attachments);

                    if (!Directory.Exists(pathToSave))
                    {
                        Directory.CreateDirectory(pathToSave);
                    }

                    var extension = Path.GetExtension(item.ReceiptName);
                    var id = Guid.NewGuid();
                    var path = $"{id}.{extension}";
                    var documentPath = Path.Combine(pathToSave, path);
                    string base64 = item.ReceiptPath.Split(',').LastOrDefault();
                    if (!string.IsNullOrWhiteSpace(base64))
                    {
                        byte[] bytes = Convert.FromBase64String(base64);
                        try
                        {
                            await File.WriteAllBytesAsync($"{documentPath}", bytes);
                            entity.ExpenseDocument[index].ReceiptPath = path;
                        }
                        catch
                        {
                            _logger.LogError("Error while saving files", entity);
                        }
                    }
                }
                index++;

                //_expenseDocumentRepository.Add(entityExpenseDocument);
            }

            _expenseRepository.Add(entity);

            //if (await _uow.SaveAsync() <= 0)
            //{
            //    _logger.LogError("Error while saving Expense");
            //    return ServiceResponse<ExpenseDto>.Return500();
            //}

            //================File Upload

            //foreach (var item in request.ExpenseDocument)
            //{
            //    var entityExpenseDocument = _mapper.Map<ExpenseDocument>(item);               
            //    entityExpenseDocument.ExpenseId = entity.Id;
            //    entityExpenseDocument.Id = Guid.NewGuid();

            //    if (!string.IsNullOrWhiteSpace(item.ReceiptName) && !string.IsNullOrWhiteSpace(item.ReceiptPath))
            //    {
            //        string contentRootPath = _webHostEnvironment.WebRootPath;
            //        var pathToSave = Path.Combine(contentRootPath, _pathHelper.Attachments);

            //        if (!Directory.Exists(pathToSave))
            //        {
            //            Directory.CreateDirectory(pathToSave);
            //        }

            //        var extension = Path.GetExtension(item.ReceiptName);
            //        var id = Guid.NewGuid();
            //        var path = $"{id}.{extension}";
            //        var documentPath = Path.Combine(pathToSave, path);
            //        string base64 = item.ReceiptPath.Split(',').LastOrDefault();
            //        if (!string.IsNullOrWhiteSpace(base64))
            //        {
            //            byte[] bytes = Convert.FromBase64String(base64);
            //            try
            //            {
            //                await File.WriteAllBytesAsync($"{documentPath}", bytes);
            //                entityExpenseDocument.ReceiptPath = path;
            //            }
            //            catch
            //            {
            //                _logger.LogError("Error while saving files", entityExpenseDocument);
            //            }
            //        }
            //    }

            //    _expenseDocumentRepository.Add(entityExpenseDocument);
            //}

            if (await _uow.SaveAsync() <= 0)
           {
                _logger.LogError("Error while saving Expense");
                return ServiceResponse<ExpenseDto>.Return500();
            }

            //===============================
            var industrydto = _mapper.Map<ExpenseDto>(entity);
            return ServiceResponse<ExpenseDto>.ReturnResultWith200(industrydto);
        }
    }
}
