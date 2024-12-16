using AutoMapper;
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
using System.Net.Http.Headers;

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
        private readonly IUserRepository _userRepository;
        private readonly UserInfoToken _userInfoToken;

        public AddExpenseCommandHandler(
            IExpenseRepository expenseRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<AddExpenseCommandHandler> logger,
            IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper,
            IExpenseDocumentRepository expenseDocumentRepository,
            IUserRepository userRepository,
           UserInfoToken userInfoToken)
        {
            _expenseRepository = expenseRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
            _expenseDocumentRepository = expenseDocumentRepository;
            _userRepository = userRepository;
            _userInfoToken = userInfoToken;
        }

        public async Task<ServiceResponse<ExpenseDto>> Handle(AddExpenseCommand request, CancellationToken cancellationToken)
        {
            var userDetails = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));

            var entity = _mapper.Map<Expense>(request);

            if (userDetails.IsDirector == true)
            {
                entity.Status = "APPROVED";
            }

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

            foreach (var item in request.ExpenseDocument)
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

                //===============================
                //if (item.FileDetails.Length > 0)
                //{

                //    try
                //    {
                //        var entityExpenseDocument1 = _mapper.Map<ExpenseDocument>(item);
                //        entityExpenseDocument1.ExpenseId = item.ExpenseId;
                //        entityExpenseDocument1.Id = Guid.NewGuid();

                //        var files = item.FileDetails;
                //        string contentRootPath = _webHostEnvironment.WebRootPath;
                //        var pathToSave = Path.Combine(contentRootPath, _pathHelper.Attachments);

                //        if (!Directory.Exists(pathToSave))
                //        {
                //            Directory.CreateDirectory(pathToSave);
                //        }

                //        var extension = Path.GetExtension(item.FileDetails.Name);
                //        var id = Guid.NewGuid();
                //        var path = $"{id}.{extension}";
                //        var documentPath = Path.Combine(pathToSave, path);


                //        var fileName = ContentDispositionHeaderValue.Parse(files.ContentDisposition).FileName.Trim('"');
                //        var fullPath = Path.Combine(pathToSave, path);
                //        //var dbPath = Path.Combine(folderName, fileName); //you can add this path to a list and then return all dbPaths to the client if require
                //        using (var stream = new FileStream(fullPath, FileMode.Create))
                //        {
                //            files.CopyTo(stream);
                //        }
                //        request.ExpenseDocument[index].ReceiptPath = path;

                //    }
                //    catch (Exception ex)
                //    {
                //        _logger.LogError("Error while saving files", ex.Message);
                //    }

                //}


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
