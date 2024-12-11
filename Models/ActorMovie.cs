﻿using System.Runtime.InteropServices;

namespace ETickets.Models
{
    public class ActorMovie
    {
        public int ActorId { get; set; }  
        public int MovieId { get; set; }
        public Actor Actor { get; set; }
        public Movie Movie { get; set; }
    }
}
