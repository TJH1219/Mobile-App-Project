using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace MobileApp.Models
{
    public class Classes
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int TermID { get; set; }
        public int CourseID {get;set;}
        public int StudentID { get;set;}
        public DateTime StartDate { get;set;}
        public DateTime EndDate { get;set;}
    }
}
