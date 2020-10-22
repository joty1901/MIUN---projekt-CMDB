using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace interaktiva20_7.Models.DTO
{
    public class MovieDto
    {
        public string Title { get; set; }
        public string Year { get; set; }
        public string ImdbID { get; set; }
        public string Type { get; set; }
        public string Poster { get; set; }
        public int numberOfLikes { get; set; }
        public int numberOfDislikes { get; set; }


    }

}
