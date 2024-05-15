namespace Core.Entities
{
    public class ApplicationForm: BaseEntity
    {
        public string ProgramTitle { get; set; }
        public string ProgramDescription { get; set; }
        public bool RequirePhone { get; set; }
        public bool RequireNationality { get; set; }
        public bool RequireCurrentResidence { get; set; }
        public bool RequireIdNumber { get; set; }
        public bool RequireDateOfBirth { get; set; }
        public bool RequireGender { get; set; }
        public List<Question> Questions { get; set; }
        public List<CandidateApplication> CandidateApplications { get; set; }

    }
}
