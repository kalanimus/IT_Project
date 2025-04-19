using Core.Entities;
using Core.Interfaces;

namespace Application.Services;
public class ScheduleService : IScheduleService
{
  private readonly IScheduleParser _scheduleParser;
  private readonly IGroupTeacherRepository _groupTeacherRepository;
  private readonly IGroupRepository _groupRepository;
  private readonly IUserRepository _userRepository;
  private readonly ISubjectRepository _subjectRepository;
  private readonly IUsernameGeneratorService _usernameGeneratorService;

  public ScheduleService(
    IScheduleParser scheduleParser,
    IGroupTeacherRepository groupTeacherRepository,
    IGroupRepository groupRepository,
    IUserRepository userRepository,
    ISubjectRepository subjectRepository,
    IUsernameGeneratorService usernameGeneratorService
  ) 
  {
    _scheduleParser = scheduleParser;
    _groupTeacherRepository = groupTeacherRepository;
    _groupRepository = groupRepository;
    _userRepository = userRepository;
    _subjectRepository = subjectRepository;
    _usernameGeneratorService = usernameGeneratorService;
  }
  public async Task UploadScheduleAsync(Stream fileStream)
  {

    try
        {
            var parsedItems = await _scheduleParser.ParseScheduleAsync(fileStream);
            if (parsedItems == null || !parsedItems.Any())
            {
                throw new Exception($"Не удалось распарсить файл или файл пуст {parsedItems}");
            }

            foreach (var item in parsedItems)
            {
                // 1. Работа с группой
                var group = await _groupRepository.GetByGroupNameAsync(item.Group.Group_Name);
                if (group == null)
                {
                    group = new ModelGroup { Group_Name = item.Group.Group_Name };
                }

                // 2. Работа с преподавателем
                var teacher = await _userRepository.GetByFullNameAsync(item.Teacher.FullName);
                if (teacher == null)
                {
                    var username = _usernameGeneratorService.GenerateTeacherUsername(item.Teacher.FullName);
                    teacher = new ModelUser { FullName = item.Teacher.FullName, Username = username, RoleId = 4, Balance = 0 };
                }

                // 3. Работа с предметом
                var subject = await _subjectRepository.GetByNameAsync(item.Subject.SubjectName);
                if (subject == null)
                {
                    subject = new ModelSubject { SubjectName = item.Subject.SubjectName };
                }

                if (await _groupTeacherRepository.GetByIdsAsync(group.Id, teacher.Id,subject.Id) == null){
                  // 4. Создание связи
                  var scheduleItem = new ModelGroupTeacher
                  {
                      Group = group,
                      Teacher = teacher,
                      Subject = subject
                  };
                  await _groupTeacherRepository.AddAsync(scheduleItem);
                }
                
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
  }

}