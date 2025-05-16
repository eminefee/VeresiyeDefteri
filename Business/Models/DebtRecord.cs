namespace Business.Models
{
    public class DebtRecord
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public float Amount { get; set; }
        public string Email { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}
