using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace MobileApp.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int StudentID { get; set; }
        public string Username { get;set; }
        public string Password { get;set; }
    }
}
