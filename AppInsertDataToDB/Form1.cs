using AppInsertDataToDB.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppInsertDataToDB
{
    public partial class Form1 : Form
    {
        private string DBConnectionString = @"Data Source=DESKTOP-KMNRV0D\SQLEXPRESS;Initial Catalog=MovieDB;Persist Security Info=True;User ID=Test;Password=12345";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ExcelManipul excel = new ExcelManipul(@"Данные", Application.StartupPath + @"\Resources\", false);
            List<string> Data = new List<string>();
            Film film = new Film();
            for (int row = 2; row <= 101; row++)
            {

                for (int column = 2; column <= 23; column++)
                    Data.Add(excel.GetCell(column, row));

                //заполнение

                film.Title = Data[0];
                film.YearReleaseWorld = Int32.Parse(Data[1]);
                if (Data[2] != "NOT RATED")
                    film.Rated = Data[2];
                else
                    film.Rated = "UNRATED";
                film.Released = DateTime.Parse(Data[3]);
                var runtime = Data[4].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                film.RunTime = Int32.Parse(runtime[0]);
                film.Plot = Data[9];
                if (!String.IsNullOrEmpty(Data[12]))
                    film.Awards = Data[12];
                if (!String.IsNullOrEmpty(Data[13]))
                    film.Poster = Data[13];
                else
                    film.Poster = "Default";
                film.RatingValue = Double.Parse(Data[14].Remove(3).Replace('.', ','));
                if (Data[15] != "N/A")
                    film.Metascore = Int32.Parse(Data[15]);
                if (!String.IsNullOrEmpty(Data[16]))
                    film.ImdbRating = Double.Parse(Data[16].Replace('.', ','));
                if (!String.IsNullOrEmpty(Data[17]))
                    film.ImdbVotes = Int32.Parse(string.Join("", Data[17].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)));
                if (Data[18] != "N/A")
                    film.DVD = DateTime.Parse(Data[18]);
                if (Data[19] != "N/A")
                    film.BoxOffice = Int32.Parse(string.Join("", Data[19].Split(new char[] { ',', '$' }, StringSplitOptions.RemoveEmptyEntries)));
                if (Data[21] != "N/A")
                    film.WebSite = @Data[21];




                //жанр
                var splitdata = Data[5].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in splitdata)
                    film.Genre.Add(item.Trim(new char[] { ' ' }));
                //режисеры
                splitdata = Data[6].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in splitdata)
                {
                    film.Person.Add(new PersonAndRole() { Name = item.Trim(new char[] { ' ' }), Role = "Director" });
                }
                //писатели
                //
                //  проблема со строками наблюдается аномалия в виде двойной \"\" а так же нужно соединить разделенную строку внутри скобок
                splitdata = Data[7].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                Regex regexRole = new Regex(@"\([^)]+\)");
                foreach (var item in splitdata)
                {
                    string personName = item;
                    string role = "";
                    if (regexRole.IsMatch(item))
                    {
                        role = regexRole.Match(item).Value.Trim(new char[] { '(', ')' });
                        string roleFirstSymbol = role[0].ToString().ToUpper();
                        role = roleFirstSymbol + role.Remove(0, 1);
                        personName = regexRole.Replace(personName, string.Empty).Trim(new char[] { ' ' });
                    }
                    if (role != "")
                        film.Person.Add(new PersonAndRole() { Name = personName, Role = role });
                    else
                        film.Person.Add(new PersonAndRole() { Name = personName, Role = "Writer" });
                }
                //актеры
                splitdata = Data[8].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in splitdata)
                    film.Person.Add(new PersonAndRole() { Name = item.Trim(new char[] { ' ' }), Role = "Actors" });
                //язык
                splitdata = Data[10].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in splitdata)
                    film.Language.Add(item.Trim(new char[] { ' ' }));
                //страна
                splitdata = Data[11].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in splitdata)
                    film.Country.Add(item.Trim(new char[] { ' ' }));
                //издатель
                splitdata = Data[20].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in splitdata)
                    film.Production.Add(item.Trim(new char[] { ' ' }));



                using (SqlConnection connection = new SqlConnection(DBConnectionString))
                {
                    //0         2       3       4       9       13
                    //@Title @Rated @Released @RunTime @Plot @Poster 
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();
                    try
                    {
                        int FilmId;
                        //добавление фильма
                        SqlCommand insertCommand = connection.CreateCommand();
                        insertCommand.CommandType = CommandType.StoredProcedure;
                        insertCommand.Transaction = transaction;
                        insertCommand.CommandText = "InsertMovie";

                        //добаление обязательных параметров

                        //возвращаемое значение
                        SqlParameter paramResult = new SqlParameter("@result", SqlDbType.Int);
                        paramResult.Direction = ParameterDirection.Output;
                        insertCommand.Parameters.Add(paramResult);
                        //заголовок
                        SqlParameter paramTitle = new SqlParameter("@Title", SqlDbType.NVarChar, 50);
                        paramTitle.Value = film.Title;
                        insertCommand.Parameters.Add(paramTitle);
                        //имя рейтинга
                        SqlParameter paramRated = new SqlParameter("@Rated", SqlDbType.NVarChar, 30);
                        paramRated.Value = film.Rated;
                        insertCommand.Parameters.Add(paramRated);
                        //дата релиза
                        SqlParameter paramReleased = new SqlParameter("@Released", SqlDbType.DateTime);
                        paramReleased.Value = film.Released;
                        insertCommand.Parameters.Add(paramReleased);
                        //длительность
                        SqlParameter paramRunTime = new SqlParameter("@RunTime", SqlDbType.Int);
                        paramRunTime.Value = film.RunTime;
                        insertCommand.Parameters.Add(paramRunTime);
                        //описание
                        SqlParameter paramPlot = new SqlParameter("@Plot", SqlDbType.NVarChar, 500);
                        paramPlot.Value = film.Plot;
                        insertCommand.Parameters.Add(paramPlot);
                        //постер
                        SqlParameter paramPoster = new SqlParameter("@Poster", SqlDbType.NVarChar, 50);
                        paramPoster.Value = film.Poster + ".jpg";
                        insertCommand.Parameters.Add(paramPoster);
                        //опциональные параметры

                        //год выхода
                        if (film.YearReleaseWorld != 0)
                        {
                            SqlParameter paramYearFilm = new SqlParameter("@YearFilm", SqlDbType.Int);
                            paramYearFilm.Value = film.YearReleaseWorld;
                            insertCommand.Parameters.Add(paramYearFilm);
                        }
                        //награды
                        if (!String.IsNullOrEmpty(film.Awards))
                        {
                            SqlParameter paramAwards = new SqlParameter("@Awards", SqlDbType.NVarChar, 100);
                            paramAwards.Value = film.Awards;
                            insertCommand.Parameters.Add(paramAwards);
                        }
                        //рейтинг значение
                        if (film.RatingValue != 0.0)
                        {
                            SqlParameter paramRatinValue = new SqlParameter("@Rating", SqlDbType.Float);
                            paramRatinValue.Value = film.RatingValue;
                            insertCommand.Parameters.Add(paramRatinValue);
                        }
                        //метасумма
                        if (film.Metascore != 0)
                        {
                            SqlParameter paramMetaScore = new SqlParameter("@MetaScore", SqlDbType.Int);
                            paramMetaScore.Value = film.Metascore;
                            insertCommand.Parameters.Add(paramMetaScore);
                        }
                        //голоса
                        if (film.ImdbVotes != 0)
                        {
                            SqlParameter paramVotes = new SqlParameter("@Votes", SqlDbType.Int);
                            paramVotes.Value = film.ImdbVotes;
                            insertCommand.Parameters.Add(paramVotes);
                        }
                        //dvd
                        if (film.DVD != default(DateTime))
                        {
                            SqlParameter paramDVD = new SqlParameter("@DVD", SqlDbType.DateTime);
                            paramDVD.Value = film.DVD;
                            insertCommand.Parameters.Add(paramDVD);
                        }
                        //сборы
                        if (film.BoxOffice != 0)
                        {
                            SqlParameter paramBoxOffice = new SqlParameter("@BoxOffice", SqlDbType.Int);
                            paramBoxOffice.Value = film.BoxOffice;
                            insertCommand.Parameters.Add(paramBoxOffice);
                        }
                        //сайт
                        if (!String.IsNullOrEmpty(film.WebSite))
                        {
                            SqlParameter paramWebSite = new SqlParameter("@SiteURL", SqlDbType.NVarChar, 100);
                            paramWebSite.Value = film.WebSite;
                            insertCommand.Parameters.Add(paramWebSite);
                        }

                        insertCommand.ExecuteNonQuery();
                        FilmId = (int)insertCommand.Parameters["@result"].Value;

                        SqlParameter paramFilmId = new SqlParameter("@FilmId", SqlDbType.Int);
                        paramFilmId.Value = FilmId;
                        insertCommand.Parameters.Clear();
                        //образование связей и заполнение остальных таблиц
                        //добавление жанров
                        insertCommand.CommandText = "InsertMovieOnGenre";
                        foreach (string genre in film.Genre)
                        {
                            SqlParameter paramName = new SqlParameter("@GenreValue", SqlDbType.NVarChar, 30);
                            paramName.Value = genre;
                            insertCommand.Parameters.Add(paramFilmId);
                            insertCommand.Parameters.Add(paramName);
                            insertCommand.ExecuteNonQuery();
                            insertCommand.Parameters.Clear();
                        }
                        //добавление режисеров, актеров и писателей
                        insertCommand.Parameters.Clear();
                        insertCommand.CommandText = "InsertPersonInMovie";
                        foreach (PersonAndRole person in film.Person)
                        {
                            SqlParameter paramPersonName = new SqlParameter("@PersonName", SqlDbType.NVarChar, 60);
                            paramPersonName.Value = person.Name;
                            SqlParameter paramRoleName = new SqlParameter("@RoleName", SqlDbType.NVarChar, 30);
                            paramRoleName.Value = person.Role;
                            insertCommand.Parameters.Add(paramFilmId);
                            insertCommand.Parameters.Add(paramPersonName);
                            insertCommand.Parameters.Add(paramRoleName);
                            insertCommand.ExecuteNonQuery();
                            insertCommand.Parameters.Clear();
                        }
                        //языки
                        insertCommand.Parameters.Clear();
                        insertCommand.CommandText = "InsertMovieOnLanguage";
                        foreach (string lang in film.Language)
                        {
                            SqlParameter paramLangName = new SqlParameter("@LanguageValue", SqlDbType.NVarChar, 30);
                            paramLangName.Value = lang;
                            insertCommand.Parameters.Add(paramFilmId);
                            insertCommand.Parameters.Add(paramLangName);
                            insertCommand.ExecuteNonQuery();
                            insertCommand.Parameters.Clear();
                        }
                        //страна
                        insertCommand.CommandText = "InsertMovieOnCountry";
                        foreach (string countryName in film.Country)
                        {
                            SqlParameter paramCountryName = new SqlParameter("@CountryValue", SqlDbType.NVarChar, 30);
                            paramCountryName.Value = countryName;
                            insertCommand.Parameters.Add(paramFilmId);
                            insertCommand.Parameters.Add(paramCountryName);
                            insertCommand.ExecuteNonQuery();
                            insertCommand.Parameters.Clear();
                        }
                        //издательские компании
                        insertCommand.CommandText = "InsertMovieOnProduction";
                        foreach (string production in film.Production)
                        {
                            SqlParameter paramProductionName = new SqlParameter("@ProductionValue", SqlDbType.NVarChar, 30);
                            paramProductionName.Value = production;
                            insertCommand.Parameters.Add(paramFilmId);
                            insertCommand.Parameters.Add(paramProductionName);
                            insertCommand.ExecuteNonQuery();
                            insertCommand.Parameters.Clear();
                        }





                        transaction.Commit();
                        textBox1.Text += "Успех, добавлен фильм: " + film.Title + Environment.NewLine;
                    }
                    catch (Exception error)
                    {
                        textBox1.Text = error.Message;
                        transaction.Rollback();
                        textBox1.Text += "Провал при добавлении фильма: " + film.Title + Environment.NewLine;
                    }
                }
                
                Data.Clear();
                film.Reset();
            }
            excel.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            
        }
    }
}

/*
 
                        ////имя рейтинга
                    //SqlParameter RatedName = new SqlParameter();
                    //RatedName.ParameterName = "@RatedName";
                    //RatedName.SqlDbType = SqlDbType.NVarChar;
                    //RatedName.Size = 30;
                    //RatedName.Value = film.Rated;

                    //insertCommand.Parameters.Add(RatedName);
                    //insertCommand.Parameters.Add(res);

                    //int FilmId = (int)insertCommand.Parameters["@result"].Value;
 */
