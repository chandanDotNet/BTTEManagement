using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.MediatR.Handlers;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Handlers
{
    public class AddTravelDeskExpenseCommandHandler : IRequestHandler<AddTravelDeskExpenseCommand, ServiceResponse<TravelDeskExpenseDto>>
    {

        private readonly ITravelDeskExpenseRepository _travelDeskExpenseRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddTravelDeskExpenseCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;

        public AddTravelDeskExpenseCommandHandler(
            ITravelDeskExpenseRepository travelDeskExpenseRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<AddTravelDeskExpenseCommandHandler> logger,
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

        public async Task<ServiceResponse<TravelDeskExpenseDto>> Handle(AddTravelDeskExpenseCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<TravelDeskExpense>(request);

            if (!string.IsNullOrWhiteSpace(request.ReceiptName) && !string.IsNullOrWhiteSpace(request.DocumentData))
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
                        entity.ReceiptPath = path;
                    }
                    catch
                    {
                        _logger.LogError("Error while saving files", entity);
                    }
                }
            }

            _travelDeskExpenseRepository.Add(entity);

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving Expense");
                return ServiceResponse<TravelDeskExpenseDto>.Return500();
            }

            var industrydto = _mapper.Map<TravelDeskExpenseDto>(entity);
            return ServiceResponse<TravelDeskExpenseDto>.ReturnResultWith200(industrydto);
        }

    }
}
