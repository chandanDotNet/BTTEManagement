﻿using AutoMapper;
using BTTEM.Data;
using BTTEM.MediatR.Trip.Commands;
using BTTEM.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Trip.Handlers
{
    public class UpdateItineraryTicketBookingQuotationCommandHandler : IRequestHandler<UpdateItineraryTicketBookingQuotationCommand, ServiceResponse<bool>>
    {
        private readonly IItineraryTicketBookingQuotationRepository _itineraryTicketBookingQuotationRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<UpdateItineraryTicketBookingQuotationCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;
        public UpdateItineraryTicketBookingQuotationCommandHandler(
            IItineraryTicketBookingQuotationRepository itineraryTicketBookingQuotationRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<UpdateItineraryTicketBookingQuotationCommandHandler> logger,
            IWebHostEnvironment webHostEnvironment, PathHelper pathHelper)
        {
            _itineraryTicketBookingQuotationRepository = itineraryTicketBookingQuotationRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
        }

        public async Task<ServiceResponse<bool>> Handle(UpdateItineraryTicketBookingQuotationCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _itineraryTicketBookingQuotationRepository
                .All.Where(x => request.ItineraryTicketBookingQuotationList.Select(q => q.Id).Contains(x.Id))
                .ToListAsync();

            if (entityExist == null || entityExist.Count == 0)
            {
                _logger.LogError("Quotation does not exists");
                return ServiceResponse<bool>.Return404("Quotation does not exists");
            }

            entityExist.ForEach(item =>
            {
                if (!string.IsNullOrEmpty(request.ItineraryTicketBookingQuotationList.FirstOrDefault(x => x.Id == item.Id).QuotationName))
                {
                    item.QuotationName = request.ItineraryTicketBookingQuotationList.FirstOrDefault(x => x.Id == item.Id).QuotationName;
                }
                if (!string.IsNullOrEmpty(item.TravelDeskNotes = request.ItineraryTicketBookingQuotationList.FirstOrDefault(x => x.Id == item.Id).TravelDeskNotes))
                {
                    item.TravelDeskNotes = request.ItineraryTicketBookingQuotationList.FirstOrDefault(x => x.Id == item.Id).TravelDeskNotes;
                }
                if (!string.IsNullOrEmpty(request.ItineraryTicketBookingQuotationList.FirstOrDefault(x => x.Id == item.Id).RMNotes))
                {
                    item.RMNotes = request.ItineraryTicketBookingQuotationList.FirstOrDefault(x => x.Id == item.Id).RMNotes;
                }
            });

            int i = 0;
            request.ItineraryTicketBookingQuotationList.ForEach(async item =>
            {
                if (!string.IsNullOrWhiteSpace(item.QuotationName) && !string.IsNullOrWhiteSpace(item.QuotationPath))
                {
                    string contentRootPath = _webHostEnvironment.WebRootPath;
                    var pathToSave = Path.Combine(contentRootPath, _pathHelper.ItineraryTicketBookingQuotationAttachments);

                    if (!Directory.Exists(pathToSave))
                    {
                        Directory.CreateDirectory(pathToSave);
                    }

                    var extension = Path.GetExtension(item.QuotationName);
                    var id = Guid.NewGuid();
                    var path = $"{id}.{extension}";
                    var documentPath = Path.Combine(pathToSave, path);
                    string base64 = item.QuotationPath.Split(',').LastOrDefault();
                    if (!string.IsNullOrWhiteSpace(base64))
                    {
                        byte[] bytes = Convert.FromBase64String(base64);
                        try
                        {
                            await File.WriteAllBytesAsync($"{documentPath}", bytes);
                            entityExist[i].QuotationPath = path;
                            i++;
                        }
                        catch
                        {
                            _logger.LogError("Error while saving files", entityExist);
                        }
                    }
                }
            });

            _itineraryTicketBookingQuotationRepository.UpdateRange(entityExist);

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error While saving Trip Itinerary.");
                return ServiceResponse<bool>.Return500();
            }

            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
