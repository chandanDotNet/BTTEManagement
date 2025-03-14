﻿using AutoMapper;
using BTTEM.API.Helpers.Mapping;

namespace POS.API.Helpers.Mapping
{
    public static class MapperConfig
    {
        public static IMapper GetMapperConfigs()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ActionProfile());
                mc.AddProfile(new PageProfile());
                mc.AddProfile(new RoleProfile());
                mc.AddProfile(new UserProfile());
                mc.AddProfile(new NLogProfile());
                mc.AddProfile(new EmailTemplateProfile());
                mc.AddProfile(new EmailProfile());
                mc.AddProfile(new CountryProfile());
                mc.AddProfile(new CustomerProfile());
                mc.AddProfile(new TestimonialsProfile());
                mc.AddProfile(new NewsletterSubscriberProfile());
                mc.AddProfile(new CityProfile());
                mc.AddProfile(new SupplierProfile());
                mc.AddProfile(new ContactUsMapping());

                mc.AddProfile(new ReminderProfile());
                mc.AddProfile(new PurchaseOrderProfile());
                mc.AddProfile(new SalesOrderProfile());

                mc.AddProfile(new CompanyProfileProfile());
                mc.AddProfile(new ExpenseProfile());
                mc.AddProfile(new CurrencyProfile());
                mc.AddProfile(new UnitProfile());
                mc.AddProfile(new TaxProfile());
                mc.AddProfile(new WarehouseProfile());


                mc.AddProfile(new InquiryNoteProfile());
                mc.AddProfile(new InquiryActivityProfile());
                mc.AddProfile(new InquiryAttachmentProfile());
                mc.AddProfile(new InquiryProfile());
                mc.AddProfile(new InquiryStatusProfile());
                mc.AddProfile(new InquirySourceProfile());

                mc.AddProfile(new ProductCategoryProfile());
                mc.AddProfile(new ProductProfile());

                mc.AddProfile(new BrandProfile());

                mc.AddProfile(new InventoryProfle());
                mc.AddProfile(new GradeProfile());
                mc.AddProfile(new EmpGradeProfile());
                mc.AddProfile(new DepartmentProfile());
                mc.AddProfile(new TravelModeProfile());
                mc.AddProfile(new ConveyanceProfile());
                mc.AddProfile(new PoliciesDetailProfile());
                mc.AddProfile(new PoliciesLodgingFoodingProfile());
                mc.AddProfile(new PurposeProfile());
                mc.AddProfile(new TripProfile());
                mc.AddProfile(new MultiLevelApprovalProfile());
                mc.AddProfile(new TripItineraryProfile());
                mc.AddProfile(new ItineraryTicketBookingProfile());
                mc.AddProfile(new TripHotelBookingProfile());
                mc.AddProfile(new VehicleManagementProfile());
                mc.AddProfile(new TravelDocumentProfile());
                mc.AddProfile(new VehicleManagementRateProfile());
                mc.AddProfile(new PoliciesVehicleConveyanceProfile());
                mc.AddProfile(new PoliciesSettingProfile());
                mc.AddProfile(new WalletProfile());
                mc.AddProfile(new TripTrackingProfile());
                mc.AddProfile(new ExpenseTrackingProfile());
                mc.AddProfile(new TravelDeskProfile());
                mc.AddProfile(new RequestCallProfile());
                mc.AddProfile(new ContactSupportProfile());
                mc.AddProfile(new NotificationProfile());
                mc.AddProfile(new CompanyGSTProfile());
                mc.AddProfile(new StateProfile());
                mc.AddProfile(new BranchProfile());
                mc.AddProfile(new HelpSupportProfile());
                mc.AddProfile(new LocalConveyanceExpenseProfile());
                mc.AddProfile(new CarBikeLogBookExpenseProfile());
                mc.AddProfile(new VendorProfile());
                mc.AddProfile(new ItineraryTicketBookingQuotationProfile());
                mc.AddProfile(new ItineraryHotelBookingQuotationProfile());
                mc.AddProfile(new CancelTripItineraryHotelUserProfile());
                mc.AddProfile(new AppVersionUpdateProfile());
                mc.AddProfile(new ApprovalLevelTypeProfile());
                mc.AddProfile(new ApprovalLevelProfile());
            });
            return mappingConfig.CreateMapper();
        }
    }
}
