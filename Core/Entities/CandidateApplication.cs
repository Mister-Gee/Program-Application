namespace Core.Entities
{
    public class CandidateApplication: BaseEntity
    {
        public DateTime ApplicationDate { get; set; } = DateTime.Now;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? Nationality { get; set; }
        public string? CurrentResidence { get; set; }
        public string? IdNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public List<CandidateAnswer> Answers { get; set; }
    }

    public class CandidateApplicationVM
    {
        public string Id { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? Nationality { get; set; }
        public string? CurrentResidence { get; set; }
        public string? IdNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public List<CandidateAnswerVM>? Answers { get; set; }
    }
}
