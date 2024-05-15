using ProgramApplication.DTOs;
using AutoMapper;
using Core.Abstract;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ProgramApplication.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProgramApplicationController : ControllerBase
    {
        private readonly IApplicationFormRepository _applicationFormRepo;
        private readonly IMapper _mapper;
        public ProgramApplicationController(IApplicationFormRepository applicationFormRepo, IMapper mapper)
        {
            _applicationFormRepo = applicationFormRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("Programs/All")]
        [ProducesResponseType(typeof(List<ApplicationFormSummaryVM>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPrograms()
        {
            try
            {
                var programs = await _applicationFormRepo.GetAllItemsAsync();

                var result = _mapper.Map<List<ApplicationFormSummaryVM>>(programs);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpGet]
        [Route("Program/{Id}")]
        [ProducesResponseType(typeof(ApplicationFormDetailedVM), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProgram(Guid Id)
        {
            try
            {
                var program = await _applicationFormRepo.GetItemAsync(Id.ToString());
                var result = _mapper.Map<ApplicationFormDetailedVM>(program);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpGet]
        [Route("Program/CandidateApplications/{Id}")]
        [ProducesResponseType(typeof(List<CandidateApplicationVM>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProgramCandidateApplication(Guid Id)
        {
            try
            {
                var program = await _applicationFormRepo.GetItemAsync(Id.ToString());
                var result = _mapper.Map<List<CandidateApplicationVM>>(program.CandidateApplications);
                foreach(var item in result)
                {
                    item.Answers.ForEach(x =>
                    {
                        x.Question = program.Questions.Where(z => z.Id == x.QuestionId).Select(x => x.Text).FirstOrDefault();
                    });
                }
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        
        [HttpGet]
        [Route("Program/Question/Type")]
        [ProducesResponseType(typeof(List<NameValueVM>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetQuestionTypes()
        {
            try
            {
                List<NameValueVM> result = new();

                foreach (QuestionType item in Enum.GetValues(typeof(QuestionType)))
                {
                    NameValueVM typ = new();
                    typ.Name = item.ToString();
                    typ.Value = item.ToString();

                    result.Add(typ);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpPost]
        [Route("Program/Create")]
        public async Task<IActionResult> CreateProgram([FromBody] ApplicationFormDTO model)
        {
            try
            {
                var form = new ApplicationForm
                {
                    Id = Guid.NewGuid().ToString(),
                    RequireCurrentResidence = model.RequireCurrentResidence,
                    RequireDateOfBirth = model.RequireDateOfBirth,
                    RequireGender = model.RequireGender,
                    RequireIdNumber = model.RequireIdNumber,
                    RequireNationality = model.RequireNationality,
                    RequirePhone = model.RequirePhone,
                    ProgramDescription = model.Description,
                    ProgramTitle = model.Title,
                    Questions = new List<Question>(),
                    CandidateApplications = new List<CandidateApplication>()
                };

                foreach (var question in model.CustomQuestions)
                {
                    //QuestionType questionType;
                    bool isValidQuestionType = Enum.TryParse<QuestionType>(question.Type, true, out QuestionType questionType);
                    if (!isValidQuestionType)
                        return BadRequest(new { Message = $"Invalid Question type: {question.Type}" });

                    if ((questionType == QuestionType.MultipleChoice || questionType == QuestionType.Dropdown) && question.Choices.Count < 1)
                        return BadRequest(new { Message = $"Choice are required for {question.Type}" });

                    form.Questions.Add(new Question
                    {
                        Id = Guid.NewGuid().ToString(),
                        Choices = question.Choices,
                        MaxChoices = question.MaxChoices,
                        Text = question.Text,
                        Type = questionType
                    });
                }

                _applicationFormRepo.AddItemAsync(form);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpPut]
        [Route("Program/Edit/{Id}")]
        public async Task<IActionResult> EditProgram(Guid Id, [FromBody] EditApplicationFormDTO model)
        {
            try
            {
                var program = await _applicationFormRepo.GetItemAsync(Id.ToString());
                if (program == null)
                    return BadRequest(new { Message = "Program does not exist" });


                program.RequireCurrentResidence = model.RequireCurrentResidence.HasValue ? model.RequireCurrentResidence.Value : program.RequireCurrentResidence;
                program.RequireDateOfBirth = model.RequireDateOfBirth.HasValue ? model.RequireDateOfBirth.Value : program.RequireDateOfBirth;
                program.RequireGender = model.RequireGender.HasValue ? model.RequireGender.Value : program.RequireGender;
                program.RequireIdNumber = model.RequireIdNumber.HasValue ? model.RequireIdNumber.Value : program.RequireIdNumber;
                program.RequireNationality = model.RequireNationality.HasValue ? model.RequireNationality.Value : program.RequireNationality;
                program.RequirePhone = model.RequirePhone.HasValue ? model.RequirePhone.Value : program.RequirePhone;
                program.ProgramDescription = string.IsNullOrEmpty(model.Description) ? program.ProgramDescription : model.Description;
                program.ProgramTitle = string.IsNullOrEmpty(model.Title) ? program.ProgramTitle : model.Title;

                foreach (var question in model.CustomQuestions)
                {
                    bool isValidQuestionType = Enum.TryParse<QuestionType>(question.Type, true, out QuestionType questionType);
                    if (!isValidQuestionType)
                        return BadRequest(new { Message = $"Invalid Question type: {question.Type}" });

                    if ((questionType == QuestionType.MultipleChoice || questionType == QuestionType.Dropdown) && question.Choices.Count < 1)
                        return BadRequest(new { Message = $"Choice are required for {question.Type}" });

                    if (string.IsNullOrEmpty(question.Id))
                    {
                        program.Questions.Add(new Question
                        {
                            Id = Guid.NewGuid().ToString(),
                            Choices = question.Choices,
                            MaxChoices = question.MaxChoices,
                            Text = question.Text,
                            Type = questionType
                        });
                    }
                    else
                    {
                        var questionToUpdate = program.Questions.FirstOrDefault(x => x.Id == question.Id);
                        if (questionToUpdate != null)
                        {
                            questionToUpdate.Text = question.Text;
                            questionToUpdate.Choices = question.Choices;
                            questionToUpdate.Type = questionType;
                            questionToUpdate.MaxChoices = question.MaxChoices;
                        }
                    }
                }

                _applicationFormRepo.UpdateItemAsync(program.Id, program);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpPut]
        [Route("Program/Question/Edit/{Id}")]
        public async Task<IActionResult> EditProgramQuestion(Guid Id, [FromBody] EditApplicationFormQuestionDTO model)
        {
            try
            {
                var program = await _applicationFormRepo.GetItemAsync(Id.ToString());
                if (program == null)
                    return BadRequest(new { Message = "Program does not exist" });


                foreach (var question in model.CustomQuestions)
                {
                    bool isValidQuestionType = Enum.TryParse<QuestionType>(question.Type, true, out QuestionType questionType);
                    if (!isValidQuestionType)
                        return BadRequest(new { Message = $"Invalid Question type: {question.Type}" });

                    if ((questionType == QuestionType.MultipleChoice || questionType == QuestionType.Dropdown) && question.Choices.Count < 1)
                        return BadRequest(new { Message = $"Choice are required for {question.Type}" });

                    if (string.IsNullOrEmpty(question.Id))
                    {
                        program.Questions.Add(new Question
                        {
                            Id = Guid.NewGuid().ToString(),
                            Choices = question.Choices,
                            MaxChoices = question.MaxChoices,
                            Text = question.Text,
                            Type = questionType
                        });
                    }
                    else
                    {
                        var questionToUpdate = program.Questions.FirstOrDefault(x => x.Id == question.Id);
                        if (questionToUpdate != null)
                        {
                            questionToUpdate.Text = question.Text;
                            questionToUpdate.Choices = question.Choices;
                            questionToUpdate.Type = questionType;
                            questionToUpdate.MaxChoices = question.MaxChoices;
                        }
                    }
                }

                _applicationFormRepo.UpdateItemAsync(program.Id, program);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpPost]
        [Route("Program/Apply")]
        public async Task<IActionResult> ApplyForProgram([FromBody] CandidateApplicationDTO model)
        {
            try
            {
                var program = await _applicationFormRepo.GetItemAsync(model.ProgramId.ToString());
                if (program == null)
                    return BadRequest(new { Message = "Application Program not found" });

                if (program.RequireGender && string.IsNullOrEmpty(model.Gender))
                {
                    return BadRequest(new { Message = "Gender is Required" });
                }

                if (program.RequireCurrentResidence && string.IsNullOrEmpty(model.CurrentResidence))
                {
                    return BadRequest(new { Message = "Current Residence is Required" });
                }

                if (program.RequireDateOfBirth && !model.DateOfBirth.HasValue)
                {
                    return BadRequest(new { Message = "Date of Birth is Required" });
                }

                if (program.RequireIdNumber && string.IsNullOrEmpty(model.IdNumber))
                {
                    return BadRequest(new { Message = "ID number is Required" });
                }

                if (program.RequireNationality && string.IsNullOrEmpty(model.Nationality))
                {
                    return BadRequest(new { Message = "Nationality is Required" });
                }

                if (program.RequirePhone && string.IsNullOrEmpty(model.Phone))
                {
                    return BadRequest(new { Message = "Phone number is Required" });
                }

                CandidateApplication application = _mapper.Map<CandidateApplication>(model);

                program.CandidateApplications.Add(application);
                _applicationFormRepo.UpdateItemAsync(program.Id, program);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpDelete]
        [Route("Program/Delete/{Id}")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            try
            {
                var update = await _applicationFormRepo.GetItemAsync(Id.ToString());
                if (update == null)
                    return BadRequest(new { Message = "Application Program does not exist" });


                await _applicationFormRepo.DeleteItemAsync(Id.ToString());
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }


    }
}
