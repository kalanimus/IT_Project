using Core.Entities;
using Core.Interfaces;

namespace Application.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IGroupStudentRepository _groupStudentRepository;
        private readonly IGroupTeacherRepository _groupTeacherRepository;
        private readonly IUserRepository _userRepository;

        public GroupService(
            IGroupRepository groupRepository,
            IGroupStudentRepository groupStudentRepository,
            IGroupTeacherRepository groupTeacherRepository,
            IUserRepository userRepository)
        {
            _groupStudentRepository = groupStudentRepository;
            _groupTeacherRepository = groupTeacherRepository;
            _groupRepository = groupRepository;
            _userRepository = userRepository;
        }

        public async Task<ModelGroup> GetByIdAsync(int id)
        {
            return await _groupRepository.GetByIdAsync(id);
        }

        public async Task<List<ModelGroup>> GetAllAsync()
        {
            return await _groupRepository.GetAllAsync();
        }

        public async Task AddAsync(ModelGroup group)
        {
            await _groupRepository.AddAsync(group);
        }

        public async Task UpdateAsync(ModelGroup group)
        {
            var existingGroup = await _groupRepository.GetByIdAsync(group.Id);
            if (existingGroup == null) throw new Exception("Group not found");

            // Обновляем поля существующей группы
            existingGroup.GroupName = group.GroupName;
            // Другие поля, если необходимо

            await _groupRepository.UpdateAsync(existingGroup);
        }

        public async Task DeleteAsync(int id)
        {
            var group = await _groupRepository.GetByIdAsync(id);
            if (group == null) throw new Exception("Group not found");

            await _groupRepository.DeleteAsync(group.Id);
        }

        public async Task AddStudentToGroupAsync(int groupId, int studentId)
        {
            var groupStudent = new ModelGroupStudent
            {
                GroupId = groupId,
                StudentId = studentId
            };
            await _groupStudentRepository.AddAsync(groupStudent);
        }

        public async Task AddTeacherToGroupAsync(ModelGroupTeacher groupTeacher)
        {
            await _groupTeacherRepository.AddAsync(groupTeacher);
        }

        public async Task<List<ModelGroup>> GetGroupByTeacherUserNameAsync(string userName)
        {
            var teacher = await _userRepository.GetByUsernameAsync(userName);
            var groupTeachers = await _groupTeacherRepository.GetGroupTeachersByIdAsync(teacher.Id);
            if (groupTeachers == null || !groupTeachers.Any())
            {
                return new List<ModelGroup>();
            }
            var groups = new List<ModelGroup>();
            foreach (var groupTeacher in groupTeachers)
            {
                var group = groupTeacher.Group;
                if (group == null)
                {
                    continue;
                }
                groups.Add(group);
            }
            return groups;
        }
    }
}