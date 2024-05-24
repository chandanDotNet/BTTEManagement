using AutoMapper;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Handlers;
using BTTEM.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Handlers
{
    public class UpdateTravelDeskExpenseCommandHandler : IRequestHandler<UpdateTravelDeskExpenseCommand, ServiceResponse<bool>>
    {

        private readonly ITravelDeskExpenseRepository _travelDeskExpenseRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateTravelDeskExpenseCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;

        public UpdateTravelDeskExpenseCommandHandler(
            ITravelDeskExpenseRepository travelDeskExpenseRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<UpdateTravelDeskExpenseCommandHandler> logger,
            IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper)
        {
            _travelDeskExpenseRepository = travelDeskExpenseRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
        }


        public async Task<ServiceResponse<bool>> Handle(UpdateTravelDeskExpenseCommand request, CancellationToken cancellationToken)
        {
            
            var entityExist = await _travelDeskExpenseRepository.FindAsync(request.Id);           

            if (entityExist == null)
            {
                _mapper.Map(request, entityExist);
                entityExist = _mapper.Map<TravelDeskExpense>(request);
                _travelDeskExpenseRepository.Add(entityExist);
            }
            else
            {
                _mapper.Map(request, entityExist);
                _travelDeskExpenseRepository.Update(entityExist);
            }

            if (request.IsReceiptChange)
            {
                if (!string.IsNullOrWhiteSpace(request.DocumentData)
                    && !string.IsNullOrWhiteSpace(request.ReceiptName))
                {
                    string contentRootPath = _webHostEnvironment.WebRootPath;
                    var pathToSave = Path.Combine(contentRootPath, _pathHelper.TravelDeskAttachments);

                    if (!Directory.Exists(pathToSave))
                    {
                        Directory.CreateDirectory(pathToSave);
                    }

                    var extension = Path.GetExtension(request.ReceiptName); 
                    var id = Guid.NewGuid();
                    var path = $"{id}.{extension}";
                    var documentPath = Path.Combine(pathToSave, path);
                    string base64 = request.DocumentData.Split(',').LastOrDefault();
                    if (!string.IsNullOrWhiteSpace(base64))
                    {
                        byte[] bytes = Convert.FromBase64String(base64);
                        try
                        {
                            await File.WriteAllBytesAsync($"{documentPath}", bytes);
                            entityExist.ReceiptPath = path;
                        }
                        catch
                        {
                            _logger.LogError("Error while saving files", entityExist);
                        }
                    }
                }
                else
                {
                    entityExist.ReceiptPath = null;
                    entityExist.ReceiptName = null;
                }
            }
           
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving Expense.");
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnSuccess();
        }

    }
}
