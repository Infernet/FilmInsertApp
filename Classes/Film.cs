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
        public int RunTime{ get; set; }
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
            Released = default(DateTime);
            DVD = default(DateTime);
        }

        public void Reset()
        {
            Title = String.Empty;
            YearReleaseWorld = 0;
            Rated = String.Empty;
            Released = default(DateTime);
            RunTime = 0;
            Plot = String.Empty;
            RatingValue = 0.0;
            Metascore = 0;
            ImdbVotes = 0;
            BoxOffice = 0;
            DVD = default(DateTime);
            WebSite = String.Empty;
            Awards = String.Empty;
            Poster = String.Empty;
            Genre.Clear();
            Country.Clear();
            Production.Clear();
            Language.Clear();
            Person.Clear();
        }
    }
}
