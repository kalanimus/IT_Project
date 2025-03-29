using Core.Entities;
using Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Seeders
{
    public static class DataSeeder
    {
        public static void SeedData(AppDbContext context)
        {
            // Добавление ролей, если их нет
            if (!context.Roles.Any())
            {
                var roles = new List<ModelRole>{
                    new ModelRole { RoleName = "Администратор" },
                    new ModelRole { RoleName = "Модератор" },
                    new ModelRole { RoleName = "Студент" },
                    new ModelRole { RoleName = "Преподаватель" },
                    new ModelRole { RoleName = "Декан" },
                    new ModelRole { RoleName = "Ректор" }
                };
                context.Roles.AddRange(roles);
                context.SaveChanges();
            }

            // Добавление пользователей, если их нет
            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new ModelUser { FullName = "Администратор", RoleId = 1, Username = "admin" },
                    new ModelUser { FullName = "Модератор", RoleId = 2, Username = "moder" }
                );
                context.SaveChanges();
            }

            // Добавление разрешений, если их нет
            if (!context.Permissions.Any())
            {
                var permissions = new List<ModelPermission>
                {
                    new ModelPermission { PermissionName = "Получение уведомлений" },
                    new ModelPermission { PermissionName = "Просмотр рейтинга преподавателей" },
                    new ModelPermission { PermissionName = "Просмотр рейтинга «Самый отзывчивый студент»" },
                    new ModelPermission { PermissionName = "Покупка предметов персонализации" },
                    new ModelPermission { PermissionName = "Чтение отзывов" },
                    new ModelPermission { PermissionName = "Прохождение опросов" },
                    new ModelPermission { PermissionName = "Оставление отзывов" },
                    new ModelPermission { PermissionName = "Просмотр сводки результатов опросов по группам" },
                    new ModelPermission { PermissionName = "Возможность добавлять/редактировать кастомные опросы" },
                    new ModelPermission { PermissionName = "Просмотр статистики за прошлые года" },
                    new ModelPermission { PermissionName = "Формирование отчета" },
                    new ModelPermission { PermissionName = "Просмотр сводки результатов опросов у всех преподавателей своего факультета" },
                    new ModelPermission { PermissionName = "Просмотр статистики по всему вузу отдельно по каждому из факультетов" },
                    new ModelPermission { PermissionName = "Модерация оставленных отзывов, с последующим допуском/не допуском на сайт" },
                    new ModelPermission { PermissionName = "Модерация ответов в опросах (при необходимости)" },
                    new ModelPermission { PermissionName = "Редактирование внутреннего магазина (добавление/удаление/редактирование товаров)" },
                    new ModelPermission { PermissionName = "Добавление данных (учебный план, список студентов) на сайт для корректной работы" },
                    new ModelPermission { PermissionName = "Запуск парсеров для получения этих данных" },
                    new ModelPermission { PermissionName = "Редактирование ролей на сайте" },
                    new ModelPermission { PermissionName = "Возможность создания/редактирования/удаления профиля" },
                    new ModelPermission { PermissionName = "Просмотр всех профилей" },
                    new ModelPermission { PermissionName = "Редактирование опроса по умолчанию" }
                };

                context.Permissions.AddRange(permissions);
                context.SaveChanges();
            }

            // Добавление связей между ролями и разрешениями, если их нет
            if (!context.PermissionsForRoles.Any())
            {
                var rolePermissions = new List<ModelPermissionForRole>();

                // Права для "Все" (первые 5 прав) добавляются каждой роли
                var commonPermissionIds = new List<int> { 1, 2, 3, 4, 5 };

                // Связи для всех ролей (права для "Все")
                foreach (var roleId in context.Roles.Select(r => r.Id))
                {
                    foreach (var permissionId in commonPermissionIds)
                    {
                        rolePermissions.Add(new ModelPermissionForRole
                        {
                            RoleId = roleId,
                            PermissionId = permissionId
                        });
                    }
                }

                // Связи для конкретных ролей
                // Администратор (RoleId = 1)
                var adminPermissionIds = new List<int> { 17, 18, 19, 20, 21, 22 };
                foreach (var permissionId in adminPermissionIds)
                {
                    rolePermissions.Add(new ModelPermissionForRole
                    {
                        RoleId = 1, // Администратор
                        PermissionId = permissionId
                    });
                }

                // Модератор (RoleId = 2)
                var moderatorPermissionIds = new List<int> { 14, 15, 16 };
                foreach (var permissionId in moderatorPermissionIds)
                {
                    rolePermissions.Add(new ModelPermissionForRole
                    {
                        RoleId = 2, // Модератор
                        PermissionId = permissionId
                    });
                }

                // Студент (RoleId = 3)
                var studentPermissionIds = new List<int> { 6, 7 };
                foreach (var permissionId in studentPermissionIds)
                {
                    rolePermissions.Add(new ModelPermissionForRole
                    {
                        RoleId = 3, // Студент
                        PermissionId = permissionId
                    });
                }

                // Преподаватель (RoleId = 4)
                var teacherPermissionIds = new List<int> { 8, 9, 10, 11 };
                foreach (var permissionId in teacherPermissionIds)
                {
                    rolePermissions.Add(new ModelPermissionForRole
                    {
                        RoleId = 4, // Преподаватель
                        PermissionId = permissionId
                    });
                }

                // Декан (RoleId = 5)
                var deanPermissionIds = new List<int> { 12, 11 }; // 11 - Формирование отчета
                foreach (var permissionId in deanPermissionIds)
                {
                    rolePermissions.Add(new ModelPermissionForRole
                    {
                        RoleId = 5, // Декан
                        PermissionId = permissionId
                    });
                }

                // Ректор (RoleId = 6)
                var rectorPermissionIds = new List<int> { 13, 11 }; // 11 - Формирование отчета
                foreach (var permissionId in rectorPermissionIds)
                {
                    rolePermissions.Add(new ModelPermissionForRole
                    {
                        RoleId = 6, // Ректор
                        PermissionId = permissionId
                    });
                }

                context.PermissionsForRoles.AddRange(rolePermissions);
                context.SaveChanges();
            }
        }
    }
}