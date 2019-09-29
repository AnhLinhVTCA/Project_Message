using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_Email_MVC.Models
{
    public class Users
    {
        public int Id { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Username { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Password { get; set; }
        public Users(){}
        public Users(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }
    }
}