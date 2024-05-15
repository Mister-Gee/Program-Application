namespace Core.Entities
{
    public class CandidateAnswer: BaseEntity
    {
        public string QuestionId { get; set; }
        public List<string> Answers { get; set; }
    }

    public class CandidateAnswerVM
    {
        public string Id { get; set; }
        public string QuestionId { get; set; }
        public string Question { get; set; } = "";
        public List<string> Answers { get; set; }
    }
}
