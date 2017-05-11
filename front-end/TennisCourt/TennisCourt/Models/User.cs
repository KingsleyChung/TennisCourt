﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisCourt.Models
{
    class User
    {
        public string Username { get; set; }
        public string StudentID { get; set; }
        public string Password { get; set; }
        public string Mode { get; set; }

        public User(string username, string studentID, string password, string mode)
        {
            Username = username;
            StudentID = studentID;
            Password = password;
            Mode = mode;
        }
    }
}
