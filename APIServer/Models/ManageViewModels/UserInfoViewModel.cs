using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APIServer.Models.ManageViewModels
{
    public class UserInfoViewModel
    {
        public string Username { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string Role { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
