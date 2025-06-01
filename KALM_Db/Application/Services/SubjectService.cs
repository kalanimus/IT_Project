using Core.Entities;
using Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGroupTeacherRepository _groupTeacherRepository;

        public SubjectService(
            ISubjectRepository subjectRepository,
            IUserRepository userRepository,
            IGroupTeacherRepository groupTeacherRepository)
        {
            _subjectRepository = subjectRepository;
            _userRepository = userRepository;
            _groupTeacherRepository = groupTeacherRepository;
        }

        public async Task<ModelSubject> GetByIdAsync(int id)
        {
            return await _subjectRepository.GetByIdAsync(id);
        }

        public async Task<List<ModelSubject>> GetAllAsync()
        {
            return await _subjectRepository.GetAllAsync();
        }

        public async Task AddAsync(ModelSubject subject)
        {
            await _subjectRepository.AddAsync(subject);
        }

        public async Task UpdateAsync(ModelSubject subject)
        {
            var existingSubject = await _subjectRepository.GetByIdAsync(subject.Id);
            if (existingSubject == null) throw new Exception("Subject not found");

            // Обновляем поля существующего предмета
            existingSubject.SubjectName = subject.SubjectName;
            // Другие поля, если необходимо

            await _subjectRepository.UpdateAsync(existingSubject);
        }

        public async Task DeleteAsync(int id)
        {
            var subject = await _subjectRepository.GetByIdAsync(id);
            if (subject == null) throw new Exception("Subject not found");

            await _subjectRepository.DeleteAsync(subject.Id);
        }
        public async Task<List<ModelSubject>> GetSubjectsByTeacherUserNameAsync(string username)
        {
            var teacher = await _userRepository.GetByUsernameAsync(username);
            var groupTeachers = await _groupTeacherRepository.GetGroupTeachersByIdAsync(teacher.Id);
            if (groupTeachers == null || !groupTeachers.Any())
            {
                return new List<ModelSubject>();
            }
            var subjects = new List<ModelSubject>();
            foreach (var groupTeacher in groupTeachers)
            {
                var subject = groupTeacher.Subject;
                if (subject == null)
                {
                    continue;
                }
                subjects.Add(subject);
            }
            return subjects;
        }
    }
}