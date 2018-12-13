using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppInsertDataToDB.Classes
{
    class Film
    {
        public string Title{ get; set; }
        public int YearReleaseWorld{ get; set; }
        public string Rated{ get; set; }
        public DateTime Released{ get; set; }
        public string RunTime{ get; set; }
        public string Plot{ get; set; }
        public double RatingValue{ get; set; }
        public int Metascore{ get; set; }
        public double ImdbRating{ get; set; }
        public int ImdbVotes{ get; set; }
        public int BoxOffice{ get; set; }
        public DateTime DVD{ get; set; }
        public string WebSite{ get; set; }
        public string Awards { get; set; }
        public string Poster { get; set; }

        public List<string> Genre { get; set; }
        public List<string> Country { get; set; }
        public List<string> Production { get; set; }
        public List<string> Language { get; set; }
        public List<PersonAndRole> Person { get; set; }

        public Film()
        {
            Genre = new List<string>();
            Country = new List<string>();
            Production = new List<string>();
            Language = new List<string>();
            Person = new List<PersonAndRole>();
        }
    }
}
