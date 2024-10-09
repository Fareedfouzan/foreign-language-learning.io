namespace ConversationApp.Framework
{
    //using imovo.IBB.Framework.JsonObjects;
    //using Microsoft.Extensions.Configuration;
    public class AppSettings
    {
        //private IConfiguration config;

        //public AppSettings(IConfiguration config)
        //{
        //    this.config = config;
        //}

        //public string IBBConnectionString => this.config.GetConnectionString("IBBDatabase");
        //public string SVPConnectionString => this.config.GetConnectionString("SVPDatabase");

        public string? AuthBypassUsers { get; set; }
        public string? SafeNetHostName { get; set; }
        public string? SafeNetSharedKey { get; set; }

        public string? TwoFactorAuthCampaign { get; set; }
        public string? TwoFactorAuthRedemptionDevice { get; set; }
        public string? ClxBaseUrl { get; set; }
        public string? UploadAdminEmails { get; set; }
        public string? AdminOrgs { get; set; }
        public string? SendGridKey { get; set; }
        public string? IINCardPrinter { get; set; }
        //public List<VoucherEmailTemplateDetail> VoucherEmailTemplateDetails { get; set; }
        public string? PrinterFilesPath { get; set; }
        public int VoucherDeliverBatchSize { get; set; }

        public int VoucherEmailBatchSize { get; set; }
        public int CreatePendingVoucherRunMinutes { get; set; }
        public string? GenerateVoucherEndpoint { get; set; }
        public string? GenerateVoucherEndpointXApiKeyValue { get; set; }
        public string? GetCustomerDetailsEndpointXApiKeyValue { get; set; }
        public string? CoreDomain { get; set; }
        public string? CoreRelativeRedeemUrl { get; set; }
        public string? CoreRelativeCreateVerificationVoucherUrl { get; set; }
        public string? CoreRelativeVoucherDetailUrl { get; set; }
        public int ConnectionTimeoutOverride { get; set; }
        public string? EmailTemplateDir { get; set; }
    }
}