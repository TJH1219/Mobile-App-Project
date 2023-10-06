using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace MobileApp.Models
{
    public class Reminders
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        // 0 = Course 1 = Term 2 = Assignment
        public int Type { get; set; }
        public int ReferenceID { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public DateTime TriggerDate { get; set; }
    }
}
