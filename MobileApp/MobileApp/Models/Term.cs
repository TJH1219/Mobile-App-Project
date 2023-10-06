using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace MobileApp.Models
{
    public class Term
    {
        [PrimaryKey, AutoIncrement]
        public int TermID { get; set; }
        public string Name { get; set; }
        public int StudentID {get;set;}
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
