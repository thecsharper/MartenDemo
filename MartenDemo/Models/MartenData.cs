namespace MartenDemo.Models
{
    public record MartenData
    {
        public DateTime Date { get; set; }

        public Guid Id { get; set; }

        public string? Text { get; set; }    
    }
}