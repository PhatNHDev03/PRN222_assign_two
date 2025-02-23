namespace FUNewsManagementSystem.WebRazorPage.ViewModels
{
    public class SystemAccountViewModel
    {
        public short AccountId { get; set; }

        public string AccountName { get; set; }

        public string AccountEmail { get; set; }
        public string NewAccountEmail { get; set; }

        public int? AccountRole { get; set; }
    }
}
