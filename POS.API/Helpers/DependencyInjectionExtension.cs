using POS.Common.UnitOfWork;
using POS.Repository;
using Microsoft.Extensions.DependencyInjection;
using BTTEM.Repository;


namespace POS.API.Helpers
{
    public static class DependencyInjectionExtension
    {
        public static void AddDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            services.AddScoped<IPropertyMappingService, PropertyMappingService>();
            services.AddScoped<IPageRepository, PageRepository>();
            services.AddScoped<IActionRepository, ActionRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IUserClaimRepository, UserClaimRepository>();
            services.AddScoped<IRoleClaimRepository, RoleClaimRepository>();
            services.AddScoped<ILoginAuditRepository, LoginAuditRepository>();
            services.AddScoped<INLogRepository, NLogRepository>();
            services.AddScoped<IEmailTemplateRepository, EmailTemplateRepository>();
            services.AddScoped<IEmailSMTPSettingRepository, EmailSMTPSettingRepository>();
            services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<IContactUsRepository, ContactUsRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<INewsletterSubscriberRepository, NewsletterSubscriberRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<ITestimonialsRepository, TestimonialsRepository>();
            services.AddScoped<IReminderNotificationRepository, ReminderNotificationRepository>();
            services.AddScoped<IReminderRepository, ReminderRepository>();
            services.AddScoped<IReminderUserRepository, ReminderUserRepository>();
            services.AddScoped<IReminderSchedulerRepository, ReminderSchedulerRepository>();
            services.AddScoped<IDailyReminderRepository, DailyReminderRepository>();
            services.AddScoped<IQuarterlyReminderRepository, QuarterlyReminderRepository>();
            services.AddScoped<IHalfYearlyReminderRepository, HalfYearlyReminderRepository>();
            services.AddScoped<ISendEmailRepository, SendEmailRepository>();
            // PO
            services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
            services.AddScoped<IPurchaseOrderItemRepository, PurchaseOrderItemRepository>();
            services.AddScoped<IPurchaseOrderPaymentRepository, PurchaseOrderPaymentRepository>();
            //SO
            services.AddScoped<ISalesOrderRepository, SalesOrderRepository>();
            services.AddScoped<ISalesOrderItemRepository, SalesOrderItemRepository>();
            services.AddScoped<ISalesOrderItemTaxRepository, SalesOrderItemTaxRepository>();
            services.AddScoped<ISalesOrderPaymentRepository, SalesOrderPaymentRepository>();

            services.AddScoped<ICompanyProfileRepository, CompanyProfileRepository>();
            services.AddScoped<IExpenseCategoryRepository, ExpenseCategoryRepository>();
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            //services.AddScoped<IMasterExpenseRepository, MasterExpenseRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductTaxRepository, ProductTaxRepository>();
            services.AddScoped<ITaxRepository, TaxRepository>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<IWarehouseRepository, WarehouseRepository>();
            services.AddScoped<IInquiryRepository, InquiryRepository>();
            services.AddScoped<IInquiryProductRepository, InquiryProductRepository>();
            services.AddScoped<IInquiryStatusRepository, InquiryStatusRepository>();
            services.AddScoped<IInquiryNoteRepository, InquiryNoteRepository>();
            services.AddScoped<IInquiryAttachmentRepository, InquiryAttachmentRepository>();
            services.AddScoped<IInquiryActivityRepository, InquiryActivityRepository>();
            services.AddScoped<IInquirySourceRepository, InquirySourceRepository>();

            services.AddScoped<IBrandRepository, BrandRepository>();

            services.AddScoped<IInventoryRepository, InventoryRepository>();
            services.AddScoped<IInventoryHistoryRepository, InventoryHistoryRepository>();

            services.AddScoped<IInventoryHistoryRepository, InventoryHistoryRepository>();
            services.AddScoped<IUnitConversationRepository, UnitConversationRepository>();
            services.AddScoped<IWarehouseInventoryRepository, WarehouseInventoryRepository>();

            services.AddScoped<IGradeRepository, GradeRepository>();
            services.AddScoped<IEmpGradeRepository, EmpGradeRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<ITravelsModeRepository, TravelsModeRepository>();
            services.AddScoped<IConveyanceRepository, ConveyanceRepository>();
            services.AddScoped<IPoliciesDetailRepository, PoliciesDetailRepository>();
            services.AddScoped<IPoliciesLodgingFoodingRepository, PoliciesLodgingFoodingRepository>();
            services.AddScoped<IPurposeRepository, PurposeRepository>();
            services.AddScoped<ITripRepository, TripRepository>();
            services.AddScoped<IGroupTripRepository, GroupTripRepository>();
            services.AddScoped<ITripItineraryRepository, TripItineraryRepository>();
            services.AddScoped<ITripHotelBookingRepository, TripHotelBookingRepository>();
            services.AddScoped<IMasterExpenseRepository, MasterExpenseRepository>();
            services.AddScoped<IGroupExpenseRepository, GroupExpenseRepository>();
            services.AddScoped<IMultiLevelApprovalRepository, MultiLevelApprovalRepository>();
            services.AddScoped<IVehicleManagementRepository, VehicleManagementRepository>();
            services.AddScoped<ITravelDocumentRepository, TravelDocumentRepository>();
            services.AddScoped<IVehicleManagementRateRepository, VehicleManagementRateRepository>();
            services.AddScoped<ICompanyAccountRepository, CompanyAccountRepository>();
            services.AddScoped<IPoliciesVehicleConveyanceRepository, PoliciesVehicleConveyanceRepository>();
            services.AddScoped<IPoliciesSettingRepository, PoliciesSettingRepository>();
            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<ITripTrackingRepository, TripTrackingRepository>();
            services.AddScoped<IExpenseTrackingRepository, ExpenseTrackingRepository>();
            services.AddScoped<ITravelDeskExpenseRepository, TravelDeskExpenseRepository>();
            services.AddScoped<IRequestCallRepository, RequestCallRepository>();
            services.AddScoped<IContactSupportRepository, ContactSupportRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<ICompanyGSTRepository, CompanyGSTRepository>();
            services.AddScoped<IStateRepository, StateRepository>();
            services.AddScoped<IBranchRepository, BranchRepository>();
            services.AddScoped<IHelpSupportRepository, HelpSupportRepository>();
            services.AddScoped<IExpenseDocumentRepository, ExpenseDocumentRepository>();
            services.AddScoped<IItineraryTicketBookingRepository, ItineraryTicketBookingRepository>();
            services.AddScoped<ILocalConveyanceExpenseRepository, LocalConveyanceExpenseRepository>();
            services.AddScoped<ILocalConveyanceExpenseDocumentRepository, LocalConveyanceExpenseDocumentRepository>();
            services.AddScoped<ICarBikeLogBookExpenseRepository, CarBikeLogBookExpenseRepository>();
            services.AddScoped<ICarBikeLogBookExpenseDocumentRepository, CarBikeLogBookExpenseDocumentRepository>();
        }
    }
}
