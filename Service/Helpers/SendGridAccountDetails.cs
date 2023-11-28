namespace Service.Helpers
{
    public sealed class SendGridAccountDetails
    {
        public string ApiKey { get; set; }
        public string ConfirmAccountTemplate { get; set; }
        public string ResetPasswordTemplate { get; set; }
        public string CompanyApprovedTemplate { get; set; }
        public string CompanyDisApprovedTemplate { get; set; }
        public string TourApprovedTemplate { get; set; }
        public string TourDisApprovedTemplate { get; set; }
        public string TourCreatedCustmer { get; set; }
        public string TourCreatedAgentAndAdmin { get; set; }
    }
}
