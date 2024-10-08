﻿using Microsoft.Extensions.Configuration;
using System.IO;

namespace POS.Helper
{
    public class PathHelper
    {
        public IConfiguration _configuration;

        public PathHelper(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public string UserProfilePath
        {
            get
            {
                return _configuration["UserProfilePath"];
            }
        }

        public string BrandImagePath
        {
            get
            {
                return _configuration["ImagePathSettings:BrandImages"];
            }
        }
        public string PolicyDocumentPath
        {
            get
            {
                return _configuration["ImagePathSettings:PolicyDocument"];
            }
        }

        public string TollParkingDocumnentPath
        {
            get
            {
                return _configuration["ImagePathSettings:TollParkingDocument"];
            }
        }

        public string RefillingDocumnentPath
        {
            get
            {
                return _configuration["ImagePathSettings:RefillingDocumnent"];
            }
        }



        public string ProductImagePath
        {
            get
            {
                return _configuration["ImagePathSettings:ProductImages"];
            }
        }

        public string ProductThumbnailImagePath
        {
            get
            {
                return Path.Combine(ProductImagePath, "Thumbnail");
            }
        }

        public string NoImageFound
        {
            get
            {
                return _configuration["ImagePathSettings:NoImageFound"];
            }
        }

        public string CompanyLogo
        {
            get
            {
                return _configuration["ImagePathSettings:CompanyLogo"];
            }
        }

        public string SupplierImagePath
        {
            get
            {
                return _configuration["ImagePathSettings:SupplierImages"];
            }
        }

        public string ArticleBannerImagePath
        {
            get
            {
                return _configuration["ImagePathSettings:ArticleBannerImagePath"];
            }
        }

        public string CustomerImagePath
        {
            get
            {
                return _configuration["ImagePathSettings:CustomerImages"];
            }
        }

        public string TestimonialsImagePath
        {
            get
            {
                return _configuration["ImagePathSettings:TestimonialsImagePath"];
            }
        }

        public string Attachments
        {
            get
            {
                return _configuration["ImagePathSettings:Attachments"];
            }
        }
        public string UserManualDoc
        {
            get
            {
                return _configuration["ImagePathSettings:UserManualDoc"];
            }
        }
        public string TravelDocument
        {
            get
            {
                return _configuration["ImagePathSettings:TravelDocument"];
            }
        }
        public string TravelDeskAttachments
        {
            get
            {
                return _configuration["ImagePathSettings:TravelDeskAttachments"];
            }
        }

        public string ItineraryTicketBookingQuotationAttachments
        {
            get
            {
                return _configuration["ImagePathSettings:ItineraryTicketBookingQuotationAttachments"];
            }
        }

        public string SiteMapPath
        {
            get
            {
                return _configuration["SiteMapPath"];
            }
        }

        public string DocumentPath
        {
            get
            {
                return _configuration["DocumentPath"];
            }
        }
        public string AesEncryptionKey
        {
            get
            {
                return _configuration["AesEncryptionKey"];
            }
        }

        public string ReminderFromEmail
        {
            get
            {
                return _configuration["ReminderFromEmail"];
            }
        }

        public string TrackingDocument
        {
            get
            {
                return _configuration["ImagePathSettings:TrackingDocument"];
            }
        }

        public string RequestCallDocument
        {
            get
            {
                return _configuration["ImagePathSettings:RequestCallDocument"];
            }
        }

        public string ContactSupportDocument
        {
            get
            {
                return _configuration["ImagePathSettings:ContactSupportDocument"];
            }
        }

        public string connectionStrings
        {
            get
            {
                return _configuration["connectionStrings:DbConnectionString"];
            }
        }
    }
}
