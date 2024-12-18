using ETickets.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETickets.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        [MinLength(3)]
        [UnqiueName]
        public string Name { get; set; }
        [Required]  
        [RegularExpression("\\W+\\.(png|jpg)")]
        public string Image { get; set; }
        [Required]
        public double Price { get; set; }
        public string Description { get; set; }
        public int NoOfViews { get; set; }
        [Required]
        public string TrailerURl { get; set; }
        [Required]
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
