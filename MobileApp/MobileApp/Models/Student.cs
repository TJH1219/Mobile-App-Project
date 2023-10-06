using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
namespace MobileApp.Models
{
    public class Student
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int InstructorID { get; set; }
        public int CurrentTermID { get; set; }
        public int CurrentCourseID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
