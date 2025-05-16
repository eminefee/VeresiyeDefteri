namespace Business.Configurations
{
    public class EmailFormViewModel
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public int? DebtRecordId { get; set; } // Borçla ilişkilendirme
    }
}
