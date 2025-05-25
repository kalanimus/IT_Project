using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Application.DTOs;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Text.Json.Serialization;

namespace Application.Mappings
{
  public class MappingProfile : Profile
  {
    public MappingProfile()
    {
      CreateMap<ModelUser, UserDto>()
        .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
        .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName));
      CreateMap<UserDto, ModelUser>()
        .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
        .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));

      CreateMap<ModelSubject, SubjectDto>();
      CreateMap<SubjectDto, ModelSubject>();

      CreateMap<ModelRole, RoleDto>();
      CreateMap<RoleDto, ModelRole>();

      CreateMap<ModelGroupTeacher, GroupTeacherDto>().ReverseMap();

      CreateMap<ModelPermission, PermissionDto>();
      CreateMap<PermissionDto, ModelPermission>();

      CreateMap<ModelGroup, GroupDto>();
      CreateMap<GroupDto, ModelGroup>();

      CreateMap<ModelSurveyAnalytics, SurveyAnalyticsDto>();
      CreateMap<SurveyAnalyticsDto, ModelSurveyAnalytics>();

      CreateMap<ModelAnswerParam, QuestionAnswerDto>();
      CreateMap<QuestionAnswerDto, ModelAnswerParam>();

      CreateMap<ModelAnswerParam, QuestionAnalyticsDto>();
      CreateMap<ModelSurveyAnalytics, SurveyAnalyticsDto>();

      CreateMap<ModelUser, TeacherDto>().ReverseMap();

      CreateMap<ModelReview, ReviewDto>()
      .ForMember(dest => dest.AuthorFullName, opt => opt.MapFrom(src => src.Author.FullName))
      .ForMember(dest => dest.TeacherFullName, opt => opt.MapFrom(src => src.Teacher.FullName));
      CreateMap<ReviewDto, ModelReview>()
      .ForPath(dest => dest.Author.FullName, opt => opt.MapFrom(src => src.AuthorFullName))
      .ForPath(dest => dest.Teacher.FullName, opt => opt.MapFrom(src => src.TeacherFullName));


      CreateMap<ModelSurvey, SurveyDto>()
      .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Teacher.Teacher.FullName))
      .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.Teacher.Subject.SubjectName))
      .ForMember(dest => dest.Group, opt => opt.MapFrom(src => src.Teacher.Group.GroupName))
      .ForMember(dest => dest.QuestionsJson,
        opt => opt.MapFrom(src =>
        JsonSerializer.Deserialize<List<ModelSurveyPart>>(src.QuestionsJson, new JsonSerializerOptions
        {
          Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
          // Converters = { new JsonStringEnumConverter() } // Конвертер для всех enum в модели
        })
          .Select(part => new SurveyPartDto
          {
            Title = part.Title.ToString(),
            Questions = part.Questions.Select(q => new QuestionDto
            {
              Type = Enum.GetName(typeof(QuestionType), q.Type).ToString(),
              Text = q.Text.ToString(),
              Options = q.Options
            }).ToList()
          }).ToList()

        ));
      CreateMap<SurveyDto, ModelSurvey>()
        .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
        .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
        .ForMember(dest => dest.IsStandart, opt => opt.MapFrom(src => src.IsStandart))
        .ForMember(dest => dest.QuestionsJson,
          opt => opt.MapFrom(src =>
            JsonSerializer.Serialize(src.QuestionsJson, new JsonSerializerOptions
            {
              Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)
            })))
        .ForMember(dest => dest.Teacher, opt => opt.Ignore());

      CreateMap<SurveyAnswerDto, ModelSurveyAnswer>()
        .ForMember(dest => dest.AnswerJson,
          opt => opt.MapFrom(src =>
            JsonSerializer.Serialize(src.Answers, new JsonSerializerOptions
            {
              Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)
            })))
        .ReverseMap()
        .ForMember(dest => dest.Answers,
          opt => opt.MapFrom(src =>
            JsonSerializer.Deserialize<List<QuestionAnswerDto>>(src.AnswerJson, new JsonSerializerOptions
            {
              Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)
            })));

    }
  }
}