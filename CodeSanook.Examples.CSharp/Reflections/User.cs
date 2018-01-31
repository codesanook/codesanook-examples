﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CodeSanook.Examples.CSharp.Reflections
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }
    }
}
