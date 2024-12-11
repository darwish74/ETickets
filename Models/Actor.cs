namespace ETickets.Models
{
    public class Actor
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Bio { get; set; }
        public string profilePicture { get; set; }
        public string News { get; set; }
        public List<ActorMovie> ActorMovies { get; set;}
    }
}
