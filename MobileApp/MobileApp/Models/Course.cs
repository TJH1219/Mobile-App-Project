using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace MobileApp.Models
{
    public class Course
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TermID { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartDate { get; set; }
        // 0 = plan to take 1 = in progress 2 = complete 3 = dropped
        public int Status { get; set; }
        public int InstructorID { get; set; }
        public int Credits { get; set; }
        public int Assess1ID { get; set; }
        public int Assess2ID { get; set; }
    }
}
