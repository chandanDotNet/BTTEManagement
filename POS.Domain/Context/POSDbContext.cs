using POS.Data;
using POS.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using BTTEM.Data;
using BTTEM.Data.Entities.Expense;
using BTTEM.Data.Entities;
using BTTEM.Data.Dto;

namespace POS.Domain
{
    public class POSDbContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public POSDbContext(DbContextOptions options) : base(options)
        {
        }
        public override DbSet<User> Users { get; set; }
        public override DbSet<Role> Roles { get; set; }
        public override DbSet<UserClaim> UserClaims { get; set; }
        public override DbSet<UserRole> UserRoles { get; set; }
        public override DbSet<UserLogin> UserLogins { get; set; }
        public override DbSet<RoleClaim> RoleClaims { get; set; }
        public override DbSet<UserToken> UserTokens { get; set; }
        public DbSet<Data.Action> Actions { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<NLog> NLog { get; set; }
        public DbSet<LoginAudit> LoginAudits { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<EmailSMTPSetting> EmailSMTPSettings { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SupplierAddress> SupplierAddresses { get; set; }
        public DbSet<ContactRequest> ContactRequests { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Testimonials> Testimonials { get; set; }
        public DbSet<NewsletterSubscriber> NewsletterSubscribers { get; set; }
        public DbSet<Reminder> Reminders { get; set; }
        public DbSet<ReminderNotification> ReminderNotifications { get; set; }
        public DbSet<ReminderUser> ReminderUsers { get; set; }
        public DbSet<ReminderScheduler> ReminderSchedulers { get; set; }
        public DbSet<HalfYearlyReminder> HalfYearlyReminders { get; set; }
        public DbSet<QuarterlyReminder> QuarterlyReminders { get; set; }
        public DbSet<DailyReminder> DailyReminders { get; set; }
        public DbSet<SendEmail> SendEmails { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }
        public DbSet<PurchaseOrderItemTax> PurchaseOrderItemTaxes { get; set; }
        public DbSet<SalesOrder> SalesOrders { get; set; }
        public DbSet<SalesOrderItem> SalesOrderItems { get; set; }
        public DbSet<SalesOrderItemTax> SalesOrderItemTaxes { get; set; }
        public DbSet<CompanyProfile> CompanyProfiles { get; set; }
        public DbSet<CompanyAccount> CompanyAccounts { get; set; }
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        public DbSet<ExpenseCategoryTax> ExpenseCategoryTaxes { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Tax> Taxes { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductTax> ProductTaxes { get; set; }
        public DbSet<Inquiry> Inquiries { get; set; }
        public DbSet<InquiryActivity> InquiryActivities { get; set; }
        public DbSet<InquiryAttachment> InquiryAttachments { get; set; }
        public DbSet<InquiryNote> InquiryNotes { get; set; }
        public DbSet<InquirySource> InquirySources { get; set; }
        public DbSet<InquiryProduct> InquiryProducts { get; set; }
        public DbSet<InquiryStatus> InquiryStatuses { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<InventoryHistory> InventoryHistories { get; set; }
        public DbSet<PurchaseOrderPayment> PurchaseOrderPayments { get; set; }
        public DbSet<SalesOrderPayment> SalesOrderPayments { get; set; }
        public DbSet<UnitConversation> UnitConversations { get; set; }
        public DbSet<WarehouseInventory> WarehouseInventories { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<EmpGrade> EmpGrades { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<TravelMode> TravelModes { get; set; }
        public DbSet<ClassOfTravel> ClassOfTravels { get; set; }
        public DbSet<Conveyance> Conveyances { get; set; }
        public DbSet<ConveyancesItem> ConveyancesItems { get; set; }
        public DbSet<PoliciesDetail> PoliciesDetails { get; set; }
        public DbSet<PoliciesLodgingFooding> PoliciesLodgingFoodings { get; set; }
        public DbSet<Purpose> Purposes { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<GroupTrip> GroupTrips { get; set; }
        public DbSet<TripItinerary> TripItinerarys { get; set; }
        public DbSet<ItineraryTicketBooking> ItineraryTicketBookings { get; set; }
        public DbSet<TripHotelBooking> TripHotelBookings { get; set; }
        public DbSet<MasterExpense> MasterExpenses { get; set; }
        public DbSet<GroupExpense> GroupExpenses { get; set; }

        public DbSet<MultiLevelApproval> MultiLevelApprovals { get; set; }
        public DbSet<VehicleManagement> VehicleManagements { get; set; }
        public DbSet<TravelDocument> TravelDocuments { get; set; }

        public DbSet<VehicleManagementRate> VehicleManagementRates { get; set; }
        public DbSet<PoliciesVehicleConveyance> PoliciesVehicleConveyances { get; set; }
        public DbSet<PoliciesSetting> PoliciesSettings { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<TripTracking> TripTrackings { get; set; }
        public DbSet<ExpenseTracking> ExpenseTrackings { get; set; }
        public DbSet<TravelDeskExpense> TravelDeskExpenses { get; set; }
        public DbSet<RequestCall> RequestCalls { get; set; }
        public DbSet<ContactSupport> ContactSupports { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<CompanyGST> CompanyGSTs { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<HelpSupport> HelpSupports { get; set; }
        public DbSet<ExpenseDocument> ExpenseDocuments { get; set; }
        public DbSet<LocalConveyanceExpense> LocalConveyanceExpenses { get; set; }
        public DbSet<LocalConveyanceExpenseDocument> LocalConveyanceExpenseDocuments { get; set; }
        public DbSet<CarBikeLogBookExpense> CarBikeLogBookExpenses { get; set; }
        public DbSet<CarBikeLogBookExpenseDocument> CarBikeLogBookExpenseDocuments { get; set; }
        public DbSet<CarBikeLogBookExpenseTollParkingDocument> CarBikeLogBookExpenseTollParkingDocuments { get; set; }
        public DbSet<CarBikeLogBookExpenseRefillingDocument> CarBikeLogBookExpenseRefillingDocuments { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<ExpenseDetail> ExpenseDetails { get; set; }
        public DbSet<ItineraryTicketBookingQuotation> ItineraryTicketBookingQuotations { get; set; }
        public DbSet<ItineraryHotelBookingQuotation> ItineraryHotelBookingQuotations { get; set; }
        public DbSet<CancelTripItineraryHotelUser> CancelTripItineraryHotelUsers { get; set; }
        public DbSet<AppVersionUpdate> AppVersions { get; set; }
        public DbSet<ApprovalLevelType> ApprovalLevelTypes { get; set; }
        public DbSet<ApprovalLevel> ApprovalLevels { get; set; }
        public DbSet<ApprovalLevelUser> ApprovalLevelUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(b =>
            {
                // Each User can have many UserClaims
                b.HasMany(e => e.UserClaims)
                    .WithOne(e => e.User)
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();

                // Each User can have many UserLogins
                b.HasMany(e => e.UserLogins)
                    .WithOne(e => e.User)
                    .HasForeignKey(ul => ul.UserId)
                    .IsRequired();

                // Each User can have many UserTokens
                b.HasMany(e => e.UserTokens)
                    .WithOne(e => e.User)
                    .HasForeignKey(ut => ut.UserId)
                    .IsRequired();

                // Each User can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            builder.Entity<Role>(b =>
            {
                // Each Role can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                // Each Role can have many associated RoleClaims
                b.HasMany(e => e.RoleClaims)
                    .WithOne(e => e.Role)
                    .HasForeignKey(rc => rc.RoleId)
                    .IsRequired();

                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(e => e.ModifiedByUser)
                    .WithMany()
                    .HasForeignKey(rc => rc.ModifiedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(e => e.DeletedByUser)
                    .WithMany()
                    .HasForeignKey(rc => rc.DeletedBy)
                    .OnDelete(DeleteBehavior.Restrict);

            });

            builder.Entity<ReminderUser>(b =>
            {
                b.HasKey(e => new { e.ReminderId, e.UserId });
                b.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(ur => ur.UserId)
                  .OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<Data.Action>(b =>
            {
                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Page>(b =>
            {
                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<EmailSMTPSetting>(b =>
            {
                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(e => e.ModifiedByUser)
                    .WithMany()
                    .HasForeignKey(rc => rc.ModifiedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(e => e.DeletedByUser)
                    .WithMany()
                    .HasForeignKey(rc => rc.DeletedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Customer>(b =>
            {
                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(e => e.ModifiedByUser)
                    .WithMany()
                    .HasForeignKey(rc => rc.ModifiedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(e => e.DeletedByUser)
                    .WithMany()
                    .HasForeignKey(rc => rc.DeletedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Supplier>(b =>
            {
                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(e => e.ModifiedByUser)
                    .WithMany()
                    .HasForeignKey(rc => rc.ModifiedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(e => e.DeletedByUser)
                    .WithMany()
                    .HasForeignKey(rc => rc.DeletedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(e => e.SupplierAddress)
                  .WithMany()
                  .HasForeignKey(rc => rc.SupplierAddressId)
                  .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(e => e.BillingAddress)
                  .WithMany()
                  .HasForeignKey(rc => rc.BillingAddressId)
                  .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(e => e.ShippingAddress)
                  .WithMany()
                  .HasForeignKey(rc => rc.ShippingAddressId)
                  .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Testimonials>(b =>
            {
                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(e => e.ModifiedByUser)
                    .WithMany()
                    .HasForeignKey(rc => rc.ModifiedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(e => e.DeletedByUser)
                    .WithMany()
                    .HasForeignKey(rc => rc.DeletedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<ProductCategory>(b =>
            {
                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<UnitConversation>(b =>
            {
                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);

            });

            builder.Entity<EmailTemplate>(b =>
            {
                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Reminder>(b =>
            {
                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Expense>(b =>
            {
               // b.HasKey(e => e.Id);    
                b.HasOne(e => e.ExpenseBy)
                    .WithMany()
                    .HasForeignKey(rc => rc.ExpenseById)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<MasterExpense>(b =>
            {

                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<ExpenseCategory>(b =>
            {
                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<ExpenseCategoryTax>(b =>
            {
                b.HasKey(c => new { c.ExpenseCategoryId, c.TaxId });
            });

            builder.Entity<ExpenseCategoryTax>(b =>
            {
                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<ProductTax>(b =>
            {
                b.HasKey(c => new { c.ProductId, c.TaxId });
            });

            builder.Entity<City>(b =>
            {
                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Inventory>(b =>
            {
                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Tax>(b =>
            {
                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            builder.Entity<Warehouse>(b =>
            {
                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Product>(b =>
            {
                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);

            });

            builder.Entity<ProductTax>(b =>
            {
                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<InquirySource>(b =>
            {
                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<InquiryStatus>(b =>
            {
                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<InquiryProduct>(b =>
            {
                b.HasKey(c => new { c.ProductId, c.InquiryId });
            });

            builder.Entity<InquiryActivity>(b =>
            {
                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<InquiryAttachment>(b =>
            {
                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<InquiryNote>(b =>
            {
                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Brand>(b =>
            {
                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<PurchaseOrderPayment>(b =>
            {
                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<PurchaseOrderItem>(b =>
            {
                b.HasOne(e => e.UnitConversation)
                    .WithMany()
                    .HasForeignKey(ur => ur.UnitId)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(e => e.Warehouse)
                    .WithMany()
                    .HasForeignKey(ur => ur.WarehouseId)
                    .OnDelete(DeleteBehavior.Restrict);
            });            

            builder.Entity<SalesOrderItem>(b =>
            {
                b.HasOne(e => e.UnitConversation)
                    .WithMany()
                    .HasForeignKey(ur => ur.UnitId)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(e => e.Warehouse)
                    .WithMany()
                    .HasForeignKey(ur => ur.WarehouseId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<SalesOrderPayment>(b =>
            {
                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Data.Page>(b =>
            {
                // Each User can have many UserClaims
                b.HasMany(e => e.Actions)
                    .WithOne(e => e.Page)
                    .HasForeignKey(uc => uc.PageId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();
            });


            builder.Entity<MultiLevelApproval>(b =>
            {
                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<VehicleManagement>(b =>
            {
                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<VehicleManagementRate>(b =>
            {
                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<TripTracking>(b =>
            {
                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<ExpenseTracking>(b =>
            {
                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<RequestCall>(b =>
            {
                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<ContactSupport>(b =>
            {
                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Notification>(b =>
            {
                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<CompanyGST>(b =>
            {
                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<State>(b =>
            {
                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Branch>(b =>
            {
                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<CompanyAccount>(b =>
            {
                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<HelpSupport>(b =>
            {
                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Vendor>(b =>
            {
                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            //builder.Entity<ItineraryTicketBookingQuotation>(b =>
            //{
            //    b.HasOne(e => e.CreatedByUser)
            //        .WithMany()
            //        .HasForeignKey(ur => ur.CreatedBy)
            //        .OnDelete(DeleteBehavior.Restrict);
            //});

            builder.Entity<ItineraryHotelBookingQuotation>(b =>
            {
                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<AppVersionUpdate>(b =>
            {
                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<ApprovalLevelType>(b =>
            {
                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<ApprovalLevel>(b =>
            {
                b.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(ur => ur.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<ApprovalLevelUser>(b =>
            {
                b.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(ur => ur.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(e => e.ApprovalLevel)
                    .WithMany()
                    .HasForeignKey(ur => ur.ApprovalLevelId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<User>().ToTable("Users");
            builder.Entity<Role>().ToTable("Roles");
            builder.Entity<RoleClaim>().ToTable("RoleClaims");
            builder.Entity<UserClaim>().ToTable("UserClaims");
            builder.Entity<UserLogin>().ToTable("UserLogins");
            builder.Entity<UserRole>().ToTable("UserRoles");
            builder.Entity<UserToken>().ToTable("UserTokens");
            builder.Entity<Grade>().ToTable("Grades");
            builder.Entity<EmpGrade>().ToTable("EmpGrades");
            builder.Entity<Department>().ToTable("Departments");
            builder.Entity<TravelMode>().ToTable("TravelModes");
            builder.Entity<ClassOfTravel>().ToTable("ClassOfTravels");
            builder.Entity<Conveyance>().ToTable("Conveyances");
            builder.Entity<ConveyancesItem>().ToTable("ConveyancesItems");
            builder.Entity<PoliciesDetail>().ToTable("PoliciesDetails");
            builder.Entity<PoliciesLodgingFooding>().ToTable("PoliciesLodgingFoodings");
            builder.Entity<Purpose>().ToTable("Purposes");
            builder.Entity<Trip>().ToTable("Trips");
            builder.Entity<GroupTrip>().ToTable("GroupTrips");
            builder.Entity<TripItinerary>().ToTable("TripItinerarys");
            builder.Entity<ItineraryTicketBooking>().ToTable("ItineraryTicketBookings");
            builder.Entity<ItineraryTicketBookingQuotation>().ToTable("ItineraryTicketBookingQuotations");
            builder.Entity<TripHotelBooking>().ToTable("TripHotelBookings");
            builder.Entity<MasterExpense>().ToTable("MasterExpenses");
            builder.Entity<GroupExpense>().ToTable("GroupExpenses");
            builder.Entity<TravelDocument>().ToTable("TravelDocuments");
            builder.Entity<PoliciesVehicleConveyance>().ToTable("PoliciesVehicleConveyances");
            builder.Entity<PoliciesSetting>().ToTable("PoliciesSettings");
            builder.Entity<Wallet>().ToTable("Wallets");
            builder.Entity<TravelDeskExpense>().ToTable("TravelDeskExpenses");
            builder.Entity<ExpenseDocument>().ToTable("ExpenseDocuments");
            builder.Entity<LocalConveyanceExpense>().ToTable("LocalConveyanceExpenses");
            builder.Entity<LocalConveyanceExpenseDocument>().ToTable("LocalConveyanceExpenseDocuments");
            builder.Entity<CarBikeLogBookExpense>().ToTable("CarBikeLogBookExpenses");
            builder.Entity<CarBikeLogBookExpenseDocument>().ToTable("CarBikeLogBookExpenseDocuments");
            builder.Entity<ExpenseDetail>().ToTable("ExpenseDetails");
            builder.Entity<CarBikeLogBookExpenseRefillingDocument>().ToTable("CarBikeLogBookExpenseRefillingDocuments");
            builder.Entity<CarBikeLogBookExpenseTollParkingDocument>().ToTable("CarBikeLogBookExpenseTollParkingDocuments");
            builder.Entity<CancelTripItineraryHotelUser>().ToTable("CancelTripItineraryHotelUsers");
            builder.DefalutMappingValue();
            builder.DefalutDeleteValueFilter();
        }
    }
}
