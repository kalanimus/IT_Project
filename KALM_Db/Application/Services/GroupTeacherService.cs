using Core.Entities;
using Core.Interfaces;

public class GroupTeacherService : IGroupTeacherService
{
    private readonly IGroupRepository _groupRepository;
    private readonly ISubjectRepository _subjectRepository;
    private readonly IUserRepository _userRepository;
    private readonly IGroupTeacherRepository _groupTeacherRepository;

    public GroupTeacherService(
        IGroupRepository groupRepository,
        ISubjectRepository subjectRepository,
        IUserRepository userRepository,
        IGroupTeacherRepository groupTeacherRepository)
    {
        _groupRepository = groupRepository;
        _subjectRepository = subjectRepository;
        _userRepository = userRepository;
        _groupTeacherRepository = groupTeacherRepository;
    }

    public async Task<ModelGroupTeacher> GetByDetailsAsync(string groupName, string subjectName, string teacherName)
    {
        var group = await _groupRepository.GetByGroupNameAsync(groupName);
        var subject = await _subjectRepository.GetByNameAsync(subjectName);
        var teacher = await _userRepository.GetByUsernameAsync(teacherName);

        if (group == null || subject == null || teacher == null)
        {
            throw new Exception("Не удалось найти одну из связанных записей (группа, предмет или преподаватель).");
        }

        return await _groupTeacherRepository.GetByIdsAsync(group.Id, teacher.Id, subject.Id);
    }
}