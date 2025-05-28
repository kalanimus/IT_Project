using Core.Entities;
using Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        private readonly IStudentsParser _studentsParser;
        private readonly IGroupStudentRepository _groupStudentRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IUsernameGeneratorService _usernameGeneratorService;
        private readonly IPasswordHasher _hasher;


        public UserService(
            IUserRepository userRepository,
            IStudentsParser studentsParser,
            IGroupStudentRepository groupStudentRepository,
            IGroupRepository groupRepository,
            IUsernameGeneratorService usernameGeneratorService,
            IPasswordHasher hasher)
        {
            _userRepository = userRepository;
            _studentsParser = studentsParser;
            _groupStudentRepository = groupStudentRepository;
            _groupRepository = groupRepository;
            _usernameGeneratorService = usernameGeneratorService;
            _hasher = hasher;
        }

        public async Task<ModelUser> GetByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<ModelUser> GetByUsernameAsync(string username)
        {
            return await _userRepository.GetByUsernameAsync(username);
        }

        public async Task<List<ModelUser>> GetAllAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task AddAsync(ModelUser user)
        {
            user.PasswordHash = _hasher.Hash(user.PasswordHash);
            await _userRepository.AddAsync(user);
        }

        public async Task UpdateAsync(ModelUser user)
        {
            var existingUser = await _userRepository.GetByIdAsync(user.Id);
            if (existingUser == null) throw new Exception("User not found");

            existingUser.Email = user.Email != null? user.Email : existingUser.Email;
            existingUser.PasswordHash =user.PasswordHash != null? _hasher.Hash(user.PasswordHash) : existingUser.PasswordHash;

            await _userRepository.UpdateAsync(existingUser);
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) throw new Exception("User not found");

            await _userRepository.DeleteAsync(user.Id);
        }

        public async Task UploadStudentsAsync(Stream fileStream)
        {
            try
            {
                Console.WriteLine(fileStream.ToString());
                var parsedItems = await _studentsParser.ParseStudentsAsync(fileStream);
                if (parsedItems == null || !parsedItems.Any())
                {
                    throw new Exception($"Не удалось распарсить файл или файл пуст {parsedItems}");
                }

                foreach (var item in parsedItems)
                {
                    // 1. Работа с группой
                    var group = await _groupRepository.GetByGroupNameAsync(item.Group.GroupName);
                    if (group == null)
                    {
                        group = new ModelGroup { GroupName = item.Group.GroupName };
                    }

                    // 2. Работа с преподавателем
                    var student = await _userRepository.GetByFullNameAsync(item.Student.FullName);
                    if (student == null)
                    {
                        string Username = _usernameGeneratorService.GenerateStudentUsername(item.Student.FullName, item.Student.Username.ToLower());
                        student = new ModelUser
                        {
                            FullName = item.Student.FullName,
                            Username = Username,
                            RoleId = 3,
                            Balance = 0,
                            Email = Username + "@student.bmstu.ru",
                            IsConfirmed = false
                        };
                    }

                    if (await _groupStudentRepository.GetByIdsAsync(group.Id, student.Id) == null)
                    {
                        // 4. Создание связи
                        var studentItem = new ModelGroupStudent
                        {
                            Group = group,
                            Student = student
                        };
                        await _groupStudentRepository.AddAsync(studentItem);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<(List<ModelUser> Teachers, int Total)> GetPagedTeachersAsync(int page, int pageSize)
        {
            var query = await _userRepository.GetTeachersAsync();
            var total = query.Count();

            var teachers = query
                .OrderBy(u => u.FullName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return (teachers, total);
        }

        public async Task<List<ModelUser>> GetTopRatedTeachersAsync(int count)
        {
            return await _userRepository.GetTopRatedTeachersAsync(count);
        }

        public async Task<ModelUser> GetMostActiveStudentAsync()
        {
            return await _userRepository.GetMostActiveStudentAsync();
        }
    }
}