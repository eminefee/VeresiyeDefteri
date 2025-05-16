namespace Business.Models
{
    public class SentEmail
    {
        public int Id { get; set; }

        public string ToEmail { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public DateTime SentAt { get; set; }

        public int? DebtRecordId { get; set; }  // Eğer borç kaydına bağlıysa

        public DebtRecord? DebtRecord { get; set; }
    }
}
