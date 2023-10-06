using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;

namespace MobileApp.Services
{
    public class SQLiteDB
    {
        private string _dbPath { get; set; }
        private readonly SQLiteAsyncConnection _db;

        public SQLiteDB()
        {
            _dbPath = Path.Combine(Xamarin.Essentials.FileSystem.AppDataDirectory, "SQLITEDB");
            _db = new SQLiteAsyncConnection(_dbPath);
            _db.CreateTableAsync<Models.Assessment>();
            _db.CreateTableAsync<Models.Classes>();
            _db.CreateTableAsync<Models.Course>();
            _db.CreateTableAsync<Models.Instructor>();
            _db.CreateTableAsync<Models.Student>();
            _db.CreateTableAsync<Models.Term>();
            _db.CreateTableAsync<Models.User>();
            _db.CreateTableAsync<Models.Reminders>();
            if ( GetAllInstructors().Result.Count == 0)
            {
                BuildInstructor();
            }
            if( GetAllTerms().Result.Count == 0)
            {
                BuildTerm();
            }
            if(GetAllCourses().Result.Count == 0)
            {
                BuildCourse();
            }
            if (GetAllAssessments().Result.Count == 0)
            {
                BuildAssesments(); 
            }
            if (GetAllUsers().Result.Count == 0) 
            {
                BuildUser();
            }
            if (GetAllStudents().Result.Count == 0)
            {
                BuildStudent();
            }

        }

        
        #region Get All Rows Functions
        public Task<List<Models.Assessment>> GetAllAssessments()
        {
            return _db.Table<Models.Assessment>().ToListAsync();
        }

        public Task<List<Models.Classes>> GetAllClasses()
        {
            return _db.Table<Models.Classes>().ToListAsync();
        }

        public Task<List<Models.Course>> GetAllCourses()
        {
            return _db.Table<Models.Course>().ToListAsync();
        }

        public Task<List<Models.Instructor>> GetAllInstructors()
        {
            return _db.Table<Models.Instructor>().ToListAsync();
        }

        public Task<List<Models.Student>> GetAllStudents()
        {
            return _db.Table<Models.Student>().ToListAsync();
        }

        public Task<List<Models.Term>> GetAllTerms()
        {
            return _db.Table<Models.Term>().ToListAsync();
        }

        public Task<List<Models.User>> GetAllUsers()
        {
            return _db.Table<Models.User>().ToListAsync();
        }

        public Task<List<Models.Reminders>> GetAllReminder()
        {
            return _db.Table<Models.Reminders>().ToListAsync();
        }

        #endregion
        
        #region Insert Functions
        public Task<int> SaveAssessment(Models.Assessment assess)
        {
            return _db.InsertAsync(assess);
        }

        public Task<int> SaveClass(Models.Classes Class)
        {
            return _db.InsertAsync(Class);
        }

        public Task<int> SaveCourse(Models.Course course)
        {
            return _db.InsertAsync(course);
        }

        public Task<int> SaveInstructor(Models.Instructor instruct)
        {
            return _db.InsertAsync(instruct);
        }

        public Task<int> SaveStudent(Models.Student student)
        {
            return _db.InsertAsync(student);
        }

        public Task<int> SaveTerm(Models.Term term)
        {
            return _db.InsertAsync(term);
        }

        public Task<int> SaveUser(Models.User user)
        {
            return _db.InsertAsync(user);
        }

        public Task<int> SaveReminder(Models.Reminders reminder)
        {
            return _db.InsertAsync(reminder);
        }

        #endregion

        #region Delete Functions
        public Task<int> DeleteAssessment(Models.Assessment assess)
        {
            return _db.DeleteAsync(assess);
        }

        public Task<int> DeleteClasses(Models.Classes Class)
        {
            return _db.DeleteAsync(Class);
        }

        public Task<int> DeleteCourse(Models.Course course)
        {
            return _db.DeleteAsync(course);
        }

        public Task<int> DeleteInstructor(Models.Instructor instruct)
        {
            return _db.DeleteAsync(instruct);
        }

        public Task<int> DeleteStudent(Models.Student student)
        {
            return _db.DeleteAsync(student);
        }

        public Task<int> DeleteTerm(Models.Term term)
        {
            return _db.DeleteAsync(term);
        }

        public Task<int> DeleteUser(Models.User user)
        {
            return _db.DeleteAsync(user);
        }

        public Task<int> DeleteReminder(Models.Reminders reminder)
        {
            return _db.DeleteAsync(reminder);
        }

        #endregion

        #region Update Function
        public Task<int> UpdateAssessment(Models.Assessment assess)
        {
            return _db.UpdateAsync(assess);
        }

        public Task<int> UpdateClass(Models.Classes Class)
        {
            return _db.UpdateAsync(Class);
        }

        public Task<int> UpdateCourse(Models.Course course)
        {
            return _db.UpdateAsync(course);
        }

        public Task<int> UpdateInstructor(Models.Instructor instruct)
        {
            return _db.UpdateAsync(instruct);
        }

        public Task<int> UpdateStudent(Models.Student student)
        {
            return _db.UpdateAsync(student);
        }

        public Task<int> UpdateTerm(Models.Term term)
        {
            return _db.UpdateAsync(term);
        }

        public Task<int> UpdateUser(Models.User user)
        {
            return _db.UpdateAsync(user);
        }

        public Task<int> UpdateReminder(Models.Reminders reminder)
        {
            return _db.UpdateAsync(reminder);
        }

        #endregion

        #region BuildTableFunction

        private void BuildInstructor()
        {
            SaveInstructor(new Models.Instructor()
            {
                Name = "Thaddeus Henderson",
                Email = "then223@wgu.edu",
                Phone = "6205156883"
            });
        }

        private void BuildTerm()
        {
            SaveTerm(new Models.Term()
            {
                TermID = 1,
                Name = "Term 1",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(10),
            }) ;
        }

        private void BuildCourse() 
        {
            SaveCourse(new Models.Course()
            {
                ID = 1,
                Name = "Course 1",
                Description = "Course 1 of Term 1",
                TermID = 1,
                StartDate= DateTime.Now,
                EndDate= DateTime.Now.AddDays(10),
                InstructorID = 1,
                Credits = 3,
                Assess1ID= 1,
                Assess2ID= 2,
            });
        }

        private void BuildAssesments()
        {
            for(int i = 0; i < 2; i++)
            {
                if(i == 0)
                {
                    SaveAssessment(new Models.Assessment()
                    {
                        Name = "Assessment " + Convert.ToString(i),
                        Description = "Performance Assessment for Course 1",
                        IsPerformance = true,
                        CourseID = 1,
                        Status = 0,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(10),
                    });
                }
                if (i == 1)
                {
                    SaveAssessment(new Models.Assessment()
                    {
                        Name = "Assessment " + Convert.ToString(i),
                        Description = "Objective Assessment for Course 1",
                        IsPerformance = false,
                        CourseID = 1,
                        Status = 0,
                        StartDate = DateTime.Now.AddDays(10),
                        EndDate = DateTime.Now.AddDays(20),
                    });
                }
            }
        }

        private void BuildUser()
        {
            SaveUser(new Models.User()
            {
                Username = "User",
                Password = "Password",
                StudentID = 1,
            });
        }

        private void BuildStudent()
        {
            SaveStudent(new Models.Student()
            {
                Name = "Student",
                Phone = "111-111-1111",
                Email = "Student@wgu.edu",
                InstructorID = 1,
                CurrentTermID = 1,
                CurrentCourseID = 1
            });
        }

        #endregion
    }
}
