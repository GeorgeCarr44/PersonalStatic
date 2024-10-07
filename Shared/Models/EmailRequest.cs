namespace PersonalStaticApp.Shared.Models
{
    public class EmailRequest
    {
        public string? To { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }

        public string? Subject { get; set; }
        public string? Body { get; set; }

    }
}
