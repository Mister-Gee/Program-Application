namespace Core.Entities
{
    public class Question: BaseEntity
    {
        public QuestionType Type { get; set; }
        public string Text { get; set; }
        public int? MaxChoices { get; set; }
        public List<QuestionChoice>? Choices { get; set; }

    }

    public class QuestionDTO
    {
        public string Type { get; set; }
        public string Text { get; set; }
        public int? MaxChoices { get; set; }
        public List<QuestionChoice>? Choices { get; set; }
    }

    public class EditQuestionDTO
    {
        public string? Id { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public int? MaxChoices { get; set; }
        public List<QuestionChoice>? Choices { get; set; }
    }

    public class QuestionChoice
    {
        public int Position { get; set; }
        public string Value { get; set; }
    }


    public enum QuestionType
    {
        Paragraph,
        YesNo,
        Dropdown,
        MultipleChoice,
        Date,
        Number
    }
}
