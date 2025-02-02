﻿using Microsoft.AspNetCore.Identity;

namespace ETickets.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Photo { get; set; }
        public string ImageUrl { get; internal set; }
    }
}
