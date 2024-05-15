using ProgramApplication.DTOs;
using AutoMapper;
using Core.Entities;

namespace ProgramApplication.Mapper
{
    public class MapperProfiles: Profile
    {
        public MapperProfiles()
        {
            CreateMap<CandidateApplicationDTO, CandidateApplication>();
            CreateMap<CandidateAnswerDTO, CandidateAnswer>();
            CreateMap<CandidateAnswer, CandidateAnswerVM>();

            CreateMap<ApplicationForm, ApplicationFormSummaryVM>();
            CreateMap<ApplicationForm, ApplicationFormDetailedVM>();
            CreateMap<CandidateApplication, CandidateApplicationVM>();
        }
    }
}
