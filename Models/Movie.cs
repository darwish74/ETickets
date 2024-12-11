using ETickets.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETickets.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string TrailerURl { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CategoryId { get; set; }
        public int CinemaId { get; set; }
        public Category category { get; set; }
        public Cinema cinema { get; set; }
        public List<ActorMovie> ActorMovies { get; set; }
        [NotMapped]
        public MovieStatus MovieStatus
        {
            get
            {
                if (DateTime.Now < StartDate)
                    return MovieStatus.Upcoming;
                else if (DateTime.Now > EndDate)
                    return MovieStatus.Expired;
                else
                    return MovieStatus.Available;
            }
        }
    }
}
