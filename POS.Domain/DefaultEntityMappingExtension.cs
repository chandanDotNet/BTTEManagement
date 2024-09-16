using POS.Data;
using POS.Data.Entities;
using Microsoft.EntityFrameworkCore;
using BTTEM.Data;
using BTTEM.Data.Dto;

namespace POS.Domain
{
    public static class DefaultEntityMappingExtension
    {
        public static void DefalutMappingValue(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Action>()
               .Property(b => b.ModifiedDate)
               .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Page>()
                .Property(b => b.ModifiedDate)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<User>()
                .Property(b => b.ModifiedDate)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Role>()
                .Property(b => b.ModifiedDate)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Country>()
              .Property(b => b.ModifiedDate)
              .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<City>()
              .Property(b => b.ModifiedDate)
              .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Supplier>()
              .Property(b => b.ModifiedDate)
              .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<ContactRequest>()
              .Property(b => b.ModifiedDate)
              .HasDefaultValueSql("GETUTCDATE()");


            modelBuilder.Entity<ProductCategory>()
                .Property(b => b.ModifiedDate)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Testimonials>()
                .Property(b => b.ModifiedDate)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<PurchaseOrder>()
                .Property(b => b.ModifiedDate)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<PurchaseOrderPayment>()
                .Property(b => b.ModifiedDate)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Expense>()
               .Property(b => b.ModifiedDate)
               .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<ExpenseCategory>()
               .Property(b => b.ModifiedDate)
               .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Grade>()
              .Property(b => b.ModifiedDate)
              .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Department>()
              .Property(b => b.ModifiedDate)
              .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<MasterExpense>()
              .Property(b => b.ModifiedDate)
              .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<MultiLevelApproval>()
             .Property(b => b.ModifiedDate)
             .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<VehicleManagement>()
            .Property(b => b.ModifiedDate)
            .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<VehicleManagementRate>()
          .Property(b => b.ModifiedDate)
          .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Trip>()
         .Property(b => b.ModifiedDate)
         .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<MasterExpense>()
            .Property(b => b.ModifiedDate)
            .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<TripTracking>()
            .Property(b => b.ModifiedDate)
            .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<ExpenseTracking>()
           .Property(b => b.ModifiedDate)
           .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<RequestCall>()
          .Property(b => b.ModifiedDate)
          .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<ContactSupport>()
        .Property(b => b.ModifiedDate)
        .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Notification>()
        .Property(b => b.ModifiedDate)
        .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<CompanyGST>()
             .Property(b => b.ModifiedDate)
             .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<State>()
     .Property(b => b.ModifiedDate)
     .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Branch>()
     .Property(b => b.ModifiedDate)
     .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<CompanyAccount>()
   .Property(b => b.ModifiedDate)
   .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<TravelDocument>()
  .Property(b => b.ModifiedDate)
  .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<HelpSupport>()
.Property(b => b.ModifiedDate)
.HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<ItineraryTicketBooking>()
.Property(b => b.ModifiedDate)
.HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<LocalConveyanceExpense>()
.Property(b => b.ModifiedDate)
.HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<CarBikeLogBookExpense>()
.Property(b => b.ModifiedDate)
.HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Vendor>()
.Property(b => b.ModifiedDate)
.HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<ItineraryTicketBookingQuotation>()
.Property(b => b.ModifiedDate)
.HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<ItineraryHotelBookingQuotation>()
.Property(b => b.ModifiedDate)
.HasDefaultValueSql("GETUTCDATE()");


            modelBuilder.Entity<AppVersionUpdate>()
            .Property(b => b.ModifiedDate)
            .HasDefaultValueSql("GETUTCDATE()");
        }

        public static void DefalutDeleteValueFilter(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
            .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<Role>()
            .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<Action>()
              .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<Page>()
             .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<EmailTemplate>()
                .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<EmailSMTPSetting>()
                .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<Country>()
             .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<City>()
             .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<Supplier>()
             .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<SupplierAddress>()
             .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<ContactRequest>()
                .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<Product>()
                .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<ProductCategory>()
                .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<Customer>()
               .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<Testimonials>()
                .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<Reminder>()
                .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<PurchaseOrder>()
                .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<PurchaseOrderPayment>()
                .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<SalesOrderPayment>()
              .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<Expense>()
                .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<ExpenseCategory>()
                .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<Warehouse>()
                .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<InquiryAttachment>()
               .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<SalesOrder>()
                .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<Tax>()
              .HasQueryFilter(p => !p.IsDeleted);


            modelBuilder.Entity<Brand>()
              .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<InquiryStatus>()
             .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<InquirySource>()
               .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<UnitConversation>()
               .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<Grade>()
              .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<Department>()
             .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<MasterExpense>()
            .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<MultiLevelApproval>()
            .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<VehicleManagement>()
           .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<VehicleManagementRate>()
          .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<Trip>()
         .HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<ItineraryTicketBooking>()

         .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<ItineraryTicketBookingQuotation>()
.HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<MasterExpense>()
               .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<TripTracking>()
             .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<ExpenseTracking>()
           .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<RequestCall>()
          .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<ContactSupport>()
          .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<Notification>()
          .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<CompanyGST>()
         .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<State>()
       .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<Branch>()
      .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<CompanyAccount>()
     .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<TravelDocument>()
    .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<HelpSupport>()
   .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<LocalConveyanceExpense>()
  .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<CarBikeLogBookExpense>()
 .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<TripHotelBooking>()
.HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<TripItinerary>()
.HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<Vendor>()
.HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<ItineraryHotelBookingQuotation>()
.HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<AppVersionUpdate>()
.HasQueryFilter(p => !p.IsDeleted);

        }
    }
}
