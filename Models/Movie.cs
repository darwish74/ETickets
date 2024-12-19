using ETickets.Data;
using Microsoft.AspNetCore.Mvc;
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
        [UniqueName]
        public string Name { get; set; }
        public string Image { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        [MaxLength(150)]
        [MinLength(20)]
        public string Description { get; set; }
        public int NoOfViews { get; set; }
        [Required]
        public string TrailerURl { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        [Display(Name="Category")]
        public int CategoryId { get; set; }
        [Required]
        [Display(Name = "Cinema")]
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
