using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Milestone_CST350.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(20, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(20, MinimumLength = 2)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Sex is required.")]
        [StringLength(20, MinimumLength = 2)]
        public string Sex { get; set; }

        [Required(ErrorMessage = "Age is required.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "State is required.")]
        [StringLength(20, MinimumLength = 2)]
        public string State { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [StringLength(50, MinimumLength = 2)]
        public string Email { get; set; }

        [Required(ErrorMessage = "User Name is required.")]
        [StringLength(50, MinimumLength = 2)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(50, MinimumLength = 2)]
        public string Password { get; set; }


        public User(int id, string firstName, string lastName, string sex, int age, string state, string email, string userName, string password)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            Sex = sex;
            State = state;
            Email = email;
            UserName = userName;
            Password = password;
        }

        public User() { }
    }
}
