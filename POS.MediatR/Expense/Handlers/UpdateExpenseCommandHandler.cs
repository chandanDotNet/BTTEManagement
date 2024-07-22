using AutoMapper;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using POS.Data;
using BTTEM.Repository;
using BTTEM.Data.Entities.Expense;
using Azure.Core;
using System.Net.Http.Headers;

namespace POS.MediatR.Handlers
{
    public class UpdateExpenseCommandHandler
        : IRequestHandler<UpdateExpenseCommand, ServiceResponse<bool>>
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateExpenseCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;
        private readonly IMasterExpenseRepository _masterExpenseRepository;
        private readonly IExpenseDocumentRepository _expenseDocumentRepository;

        public UpdateExpenseCommandHandler(
            IExpenseRepository expenseRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<UpdateExpenseCommandHandler> logger,
            IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper,
            IMasterExpenseRepository masterExpenseRepository,
            IExpenseDocumentRepository expenseDocumentRepository)
        {
            _expenseRepository = expenseRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
            _masterExpenseRepository = masterExpenseRepository;
            _expenseDocumentRepository = expenseDocumentRepository;
        }

        public async Task<ServiceResponse<bool>> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
        {
            decimal OldExpenseAmount = 0;
            var entityExist = await _expenseRepository.FindAsync(request.Id);
            if(entityExist != null)
            {
                request.Status = entityExist.Status;
                OldExpenseAmount = entityExist.Amount;
            }
            else
            {
                if(request.Amount>0)
                {
                    request.Status = "PENDING";
                }
                else
                {
                    request.Status = "APPROVED";
                }
                
            }
           

            if (request.MasterExpenseId == Guid.Empty || request.MasterExpenseId == null)
            {
                request.MasterExpenseId = entityExist.MasterExpenseId;
            }
            int index = 0;
            foreach (var item in request.ExpenseDocument)
            {
                var document = await _expenseDocumentRepository.FindAsync(item.Id);

                if (item.IsReceiptChange)
                {
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
                                request.ExpenseDocument[index].ReceiptPath = path;
                            }
                            catch
                            {
                                _logger.LogError("Error while saving files", document);
                            }


                        }

                    }
                    else
                    {
                        request.ExpenseDocument[index].ReceiptPath = null;
                        request.ExpenseDocument[index].ReceiptName = null;
                    }
                   // _expenseDocumentRepository.Update(document);
                }
                else
                {
                    if (document == null)
                    {
                        var entityExpenseDocument = _mapper.Map<ExpenseDocument>(item);
                        entityExpenseDocument.ExpenseId = item.ExpenseId;
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
                                    request.ExpenseDocument[index].ReceiptPath = path;
                                }
                                catch
                                {
                                    _logger.LogError("Error while saving files", entityExpenseDocument);
                                }


                            }

                        }
                        else
                        {
                            request.ExpenseDocument[index].ReceiptPath = null;
                            request.ExpenseDocument[index].ReceiptName = null;
                        }

                        if(item.FileDetails.Length>0)
                        {

                            try
                            {
                                var entityExpenseDocument1 = _mapper.Map<ExpenseDocument>(item);
                                entityExpenseDocument1.ExpenseId = item.ExpenseId;
                                entityExpenseDocument1.Id = Guid.NewGuid();

                                var files = item.FileDetails;
                                string contentRootPath = _webHostEnvironment.WebRootPath;
                                var pathToSave = Path.Combine(contentRootPath, _pathHelper.Attachments);

                                if (!Directory.Exists(pathToSave))
                                {
                                    Directory.CreateDirectory(pathToSave);
                                }

                                var extension = Path.GetExtension(item.FileDetails.Name);
                                var id = Guid.NewGuid();
                                var path = $"{id}.{extension}";
                                var documentPath = Path.Combine(pathToSave, path);


                                var fileName = ContentDispositionHeaderValue.Parse(files.ContentDisposition).FileName.Trim('"');
                                    var fullPath = Path.Combine(pathToSave, path);
                                    //var dbPath = Path.Combine(folderName, fileName); //you can add this path to a list and then return all dbPaths to the client if require
                                    using (var stream = new FileStream(fullPath, FileMode.Create))
                                    {
                                        files.CopyTo(stream);
                                    }
                                request.ExpenseDocument[index].ReceiptPath = path;

                            }
                            catch (Exception ex)
                            {
                                _logger.LogError("Error while saving files", ex.Message);
                            }

                        }
                       // _expenseDocumentRepository.Add(entityExpenseDocument);
                    }

                }
                index++;
            }

            if (entityExist == null)
            {
                _mapper.Map(request, entityExist);
                entityExist = _mapper.Map<Expense>(request);
                _expenseRepository.Add(entityExist);
            }
            else
            {
                _mapper.Map(request, entityExist);
                _expenseRepository.Update(entityExist);
            }

           
                //if (request.IsReceiptChange)
                //{
                //    if (!string.IsNullOrWhiteSpace(request.DocumentData)
                //        && !string.IsNullOrWhiteSpace(request.ReceiptName))
                //    {
                //        string contentRootPath = _webHostEnvironment.WebRootPath;
                //        var pathToSave = Path.Combine(contentRootPath, _pathHelper.Attachments);

                //        if (!Directory.Exists(pathToSave))
                //        {
                //            Directory.CreateDirectory(pathToSave);
                //        }

                //        var extension = Path.GetExtension(request.ReceiptName);
                //        var id = Guid.NewGuid();
                //        var path = $"{id}.{extension}";
                //        var documentPath = Path.Combine(pathToSave, path);
                //        string base64 = request.DocumentData.Split(',').LastOrDefault();
                //        if (!string.IsNullOrWhiteSpace(base64))
                //        {
                //            byte[] bytes = Convert.FromBase64String(base64);
                //            try
                //            {
                //                await File.WriteAllBytesAsync($"{documentPath}", bytes);
                //                entityExist.ReceiptPath = path;
                //            }
                //            catch
                //            {
                //                _logger.LogError("Error while saving files", entityExist);
                //            }
                //        }
                //    }
                //    else
                //    {
                //        entityExist.ReceiptPath = null;
                //        entityExist.ReceiptName = null;
                //    }
                //}


                //var entityMasterExist = await _masterExpenseRepository.FindAsync(entityExist.MasterExpenseId);
                //if (entityMasterExist != null)
                //{
                //    decimal UpdatedExpenseAmount = 0;
                //    //OldExpenseAmount = entityExist.Amount;
                //    decimal NowExpenseAmount = request.Amount;
                //    decimal TotalExpenseAmount = entityMasterExist.TotalAmount;
                //    UpdatedExpenseAmount = (TotalExpenseAmount - OldExpenseAmount);
                //    UpdatedExpenseAmount = UpdatedExpenseAmount + NowExpenseAmount;
                //    entityMasterExist.TotalAmount = UpdatedExpenseAmount;
                //    _masterExpenseRepository.Update(entityMasterExist);

                //}
                if (await _uow.SaveAsync() <= 0)
                {
                    _logger.LogError("Error while saving Expense.");
                    return ServiceResponse<bool>.Return500();
                }
                return ServiceResponse<bool>.ReturnSuccess();
            }
        }
    }
