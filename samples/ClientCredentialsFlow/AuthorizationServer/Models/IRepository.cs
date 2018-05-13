using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions.Internal;

namespace AuthorizationServer.Models
{
    public interface IProfileRepository
    {
        Task<int> GetProfileIdAsync(string login, string password);
        Task CreateProfile(Profile profile);
        Task<bool> GetProfileAsync(string login, string password);
        Task EditeProfile(Profile profile);
    }
    public interface IProjectRepository
    {
        Task<int?> CreateProject(Project project);
        Task GoArhive(int id);
        Task EditProject(Project project);
        Task ReturnFromArchive(int id_project);
        Task<Project> GetOneAsync(int id_project);
        Task<List<Project>> GetAllAsync(int id_profile);
        Task<List<Project>> GetAllArhiveAsync(int id_profile);
        Task<List<Project>> Getprojectbound(int Id_Profile);
        Task GoBound(string name, int id_profile);
        Task<List<string>> GoBoundProfile(int id_project);
    }
    public interface ITaskRepository
    {
        Task<List<Tasks>> GetTasksAsync(int projectId, int id_task);
        Task CreateToProject(Tasks task);
        Task CreateToTask(Tasks task);
        Task EditTask(Tasks task);
        Task<bool> Complite(int id);
        Task DeliteTask(int id);
    }

    public class ProfileRepository : IProfileRepository
    {
        private EntityDbContext _entityDbContext;

        public ProfileRepository(EntityDbContext entityDbContext)
        {
            this._entityDbContext = entityDbContext;
        }

        public async Task<int> GetProfileIdAsync(string login, string password)
        {
            var q = await _entityDbContext.Profile
                .Where(c => c.Login == login && c.Password == password)
                .FirstOrDefaultAsync();
            return q.Id;
        } //Поиск id пользователя по логину и паролю.

        public async Task<bool> GetProfileAsync(string login, string password)
        {
            var q = await _entityDbContext.Profile
            .Where(c => c.Login == login && c.Password == password)
            .FirstOrDefaultAsync();

            if (q != null)
            {
                return true;
            }

            return false;
        } //Поиск пользователя по логину и паролю.

        public Task CreateProfile(Profile profile)
        {
            try
            {
                _entityDbContext.Profile.Add(profile);
                
                _entityDbContext.SaveChanges();

                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                var exception = new Exception(e.Message);
                throw;
            }
        } //Создание профиля, принемает экземпляр с логином и паролем.

        public Task EditeProfile(Profile profile)
        {
            string sql = "update Profile set Name = @Name where Id = @Id";
            _entityDbContext.Database.ExecuteSqlCommand(sql,
                new SqlParameter("@Name", profile.Name), new SqlParameter("@Id", profile.Id));

            _entityDbContext.SaveChanges();
            return Task.CompletedTask;
        } //Принемает экземпляр профиля и заменяет у существующего, ищет его по id.
    }

    public class ProjectRepository : IProjectRepository
    {
        private EntityDbContext _entityDbContext;

        public ProjectRepository(EntityDbContext entityDbContext)
        {
            this._entityDbContext = entityDbContext;
        }

        public async Task<int?> CreateProject(Project project)
        {
            try
            {
                string sql = "INSERT INTO Project(Title,Description,Id_Profile,Arhive) VALUES(@Title,@Description,@Id_Profile,@Arhive)";
                _entityDbContext.Database.ExecuteSqlCommand(sql,
                    new SqlParameter("@Title", project.Title), new SqlParameter("@Description", project.Description), new SqlParameter("@Id_Profile", project.Id_Profile), new SqlParameter("@Arhive", project.Arhive));

                var id = await _entityDbContext.Projects
                    .Where(c => c.Title == project.Title && c.Description == project.Description)
                    .FirstOrDefaultAsync();

                _entityDbContext.SaveChanges();

                //foreach (Project p in _entityDbContext.Projects)
                //{
                //    if (p.Title == project.Title && p.Description == project.Description)
                //    {
                //        foreach (Bound b in _entityDbContext.Bound)
                //        {
                //            Bound b1 = new Bound()
                //            {
                //                id_profile = project.Id_Profile,
                //                id_project = p.Id
                //            };
                //           _entityDbContext.Bound
                //               .Add(b1);
                //            break;
                //        }
                //    }
                //}
              
                return id.Id;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        } //Создание проекта по его экземпляру

        public Task GoArhive(int Id)
        {
            try
            {
                string sql = "update Project set Arhive = 'True' where Id = @Id";
                _entityDbContext.Database.ExecuteSqlCommand(sql,
                     new SqlParameter("@Id", Id));

                _entityDbContext.SaveChanges();
                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        } //Отправка существующего проекта в архив

        public Task EditProject(Project project)
        {
            try
            {
                string sql = "update Project set Title = @Title, Description = @Description where Id = @Id";
                _entityDbContext.Database.ExecuteSqlCommand(sql,
                    new SqlParameter("@Title", project.Title), new SqlParameter("@Description", project.Description), new SqlParameter("@Id", project.Id));

                _entityDbContext.SaveChanges();
                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        } //Редактирование проекта по его экземпляру, поиск по id, которое уже в экземпляре 

        public Task ReturnFromArchive(int Id)
        {
            try
            {
                string sql = "update Project set Arhive = 'False' where Id = @Id";
                _entityDbContext.Database.ExecuteSqlCommand(sql,
                    new SqlParameter("@Id", Id));

                _entityDbContext.SaveChanges();
                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        } //Возврат из архива 

        public async Task<Project> GetOneAsync(int Id)
        {
            try
            {
                Project customer = await _entityDbContext.Projects
                    .Where(c => c.Id == Id)
                    .FirstOrDefaultAsync();
                
                return customer;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        } //Вернуть все данные, но только одного проекта

        public async Task<List<Project>> GetAllAsync(int Id_Profile)
        {
            try
            {
                var q = await _entityDbContext.Projects
                    .Where(c => c.Arhive == "False" && c.Id_Profile == Id_Profile)
                    .ToListAsync();
                
                return q;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        } //Вернуть только названия и описания, но всех проектов 

        public async Task<List<Project>> Getprojectbound(int Id_Profile)
        {
            try
            {
                List<Project> query = new List<Project>();

                foreach (Bound b in _entityDbContext.Bound)
                {
                    if (b.id_profile == Id_Profile)
                    {
                        foreach (Project p in _entityDbContext.Projects)
                        {
                            if (p.Id == b.id_project)
                            {
                               query.Add(p);
                            }
                        }
                    }
                }

               return query;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        } //Вернуть проекты доступные редактированию пользователю

        public async Task<List<string>> GoBoundProfile(int id_project)
        {
            try
            {
                List<string> query = new List<string>();

                foreach (Bound i in _entityDbContext.Bound)
                {
                    if (i.id_project == id_project)
                    {
                        foreach (Profile p in _entityDbContext.Profile)
                        {
                            if (i.id_profile == p.Id)
                            {
                                query.Add(p.Name);
                            }
                        }
                    }
                }

                return query;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        } //Получение имён из связей проекта

        public Task GoBound(string name, int id_project)
        {
            try
            {
                foreach (Profile p in _entityDbContext.Profile)
                {
                    if (p.Name == name)
                    {
                        Bound b1 = new Bound()
                        {
                            id_profile = p.Id,
                            id_project = id_project
                        };
                         _entityDbContext.Bound
                            .Add(b1);
                    }
                }
                _entityDbContext.SaveChanges();

                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        } //Добавоение в проект пользователя

        public async Task<List<Project>> GetAllArhiveAsync(int Id_Profile)
        {
            try
            {
                var q = await _entityDbContext.Projects
                    .Where(c => c.Arhive == "True" && c.Id_Profile == Id_Profile)
                    .ToListAsync();

                return q;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        } //Вернуть только названия и описания, но всех проектов
    }

    public class TaskRepository : ITaskRepository
    {
        private EntityDbContext _entityDbContext;

        public TaskRepository(EntityDbContext entityDbContext)
        {
            this._entityDbContext = entityDbContext;
        }

        public async Task<List<Tasks>> GetTasksAsync(int projectId, int id_task)
        {
            try
            {
                List<Tasks> query = await _entityDbContext.Tasks
                    .Where(t => t.projectId == projectId && t.Id_Task == id_task)
                    .ToListAsync();
                return query;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        } //Вытащить подзадачи и корневые задачи.

        public Task CreateToProject(Tasks task)
        {
            try
            {
                string sql = "INSERT INTO Task(Title,Priority,Deadline,projectId,Id_Task) VALUES(@Title,@Priority,@Deadline,@projectId,@Id_Task)";
                _entityDbContext.Database.ExecuteSqlCommand(sql,
                    new SqlParameter("@Title", task.Title), new SqlParameter("@Priority", task.Priority), new SqlParameter("@Deadline", task.Deadline), new SqlParameter("@projectId", task.projectId), new SqlParameter("@Id_task", task.Id_Task));

                _entityDbContext.SaveChanges();
                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        } //Создание задачи к проету.

        public Task CreateToTask(Tasks task)
        {
            try
            {
                string sql = "INSERT INTO Task(Title,Priority,Deadline,Id_Task,projectId) VALUES(@Title,@Priority,@Deadline,@Id_Task,@projectId)";
                _entityDbContext.Database.ExecuteSqlCommand(sql,
                    new SqlParameter("@Title", task.Title), new SqlParameter("@Priority", task.Priority), new SqlParameter("@Deadline", task.Deadline), new SqlParameter("@Id_Task", task.Id_Task), new SqlParameter("@projectId", task.projectId));

                _entityDbContext.SaveChanges();
                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        } // Создание задачи к другой задачи.

        public async Task<bool> Complite(int id)
        {
            try
            {
                foreach (Tasks t in _entityDbContext.Tasks)
                {
                    if (t.Id == id)
                    {
                        foreach (Tasks t2 in _entityDbContext.Tasks)
                        {
                            if (t2.Id_Task == t.Id && t2.Complite == "no")
                            {
                                return false;
                            }
                        }
                    }
                }
                var q = await _entityDbContext.Tasks
                    .Where(c => c.Id == id && c.Complite == "no")
                    .FirstOrDefaultAsync();
                q.Complite = "yes";
                _entityDbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        } //Пометить задачу выполненой.

        public Task EditTask(Tasks task)
        {
            try
            {
                string sql = "update Task set Title = @Title, Priority = @Priority, Deadline = @Deadline where Id = @Id";
                _entityDbContext.Database.ExecuteSqlCommand(sql,
                    new SqlParameter("@Title", task.Title), new SqlParameter("@Priority", task.Priority), new SqlParameter("@Deadline", task.Deadline), new SqlParameter("@Id", task.Id));

                _entityDbContext.SaveChanges();
                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        } //Редактировать задачу, по id внутри экземпляра

        public Task DeliteTask(int id)
        {
            try
            {

                string sql = "DELETE FROM Task WHERE Id = @Id or Id_Task = @Id";
                _entityDbContext.Database.ExecuteSqlCommand(sql,
                    new SqlParameter("@Id", id));

                _entityDbContext.SaveChanges();
                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        } //Удалить задачу.
    }
}