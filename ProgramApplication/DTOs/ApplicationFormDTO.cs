using Core.Entities;

namespace ProgramApplication.DTOs
{
    public class ApplicationFormDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool RequirePhone { get; set; }
        public bool RequireNationality { get; set; }
        public bool RequireCurrentResidence { get; set; }
        public bool RequireIdNumber { get; set; }
        public bool RequireDateOfBirth { get; set; }
        public bool RequireGender { get; set; }
        public List<QuestionDTO> CustomQuestions { get; set; }
    }

    public class EditApplicationFormDTO: EditApplicationFormQuestionDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool? RequirePhone { get; set; }
        public bool? RequireNationality { get; set; }
        public bool? RequireCurrentResidence { get; set; }
        public bool? RequireIdNumber { get; set; }
        public bool? RequireDateOfBirth { get; set; }
        public bool? RequireGender { get; set; }
    }

    public class EditApplicationFormQuestionDTO
    {
        public List<EditQuestionDTO> CustomQuestions { get; set; }
    }
    public class ApplicationFormSummaryVM
    {
        public string Id { get; set; }
        public string ProgramTitle { get; set; }
        public string ProgramDescription { get; set; }
    }

    public class ApplicationFormDetailedVM: ApplicationFormSummaryVM
    {
        public bool RequirePhone { get; set; }
        public bool RequireNationality { get; set; }
        public bool RequireCurrentResidence { get; set; }
        public bool RequireIdNumber { get; set; }
        public bool RequireDateOfBirth { get; set; }
        public bool RequireGender { get; set; }
        public ICollection<Question> Questions { get; set; }

    }
}
