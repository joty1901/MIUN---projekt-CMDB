using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;

namespace interaktiva20_7.Models.DTO
{
    public class MovieDto
    {
        public string Title { get; set; }
        public string Year { get; set; }
        public string Released { get; set; }
        public string Runtime { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Writer { get; set; }
        public string Actors { get; set; }
        public string Plot { get; set; }
        public string Poster { get; set; } 
        public string ImdbID { get; set; }
        public List<Rating> Ratings { get; set; }
        public int numberOfLikes { get; set; }
        public int numberOfDislikes { get; set; }
    }
}
