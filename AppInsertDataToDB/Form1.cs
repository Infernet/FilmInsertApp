using AppInsertDataToDB.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ExcelManipul excel = new ExcelManipul(@"Данные", Application.StartupPath + @"\Resources\", false);
            //try
            //{
                for (int row = 2; row <= 101; row++)
                {
                    List<string> Data = new List<string>();
                    for (int column = 2; column <= 23; column++)
                        Data.Add(excel.GetCell(column, row));
                    
                    //заполнение
                    Film film = new Film();
                    film.Title = Data[0];
                    film.YearReleaseWorld = Int32.Parse(Data[1]);
                    if (Data[2] != "UNRATED")
                        film.Rated = Data[2];
                    film.Released = DateTime.Parse(Data[3]);
                    film.RunTime = Data[4];
                    film.Plot = Data[9];
                    film.Awards = Data[12];
                    film.Poster = Data[13];
                    film.RatingValue = Double.Parse(Data[14].Remove(3).Replace('.', ','));
                    if (Data[15] != "N/A")
                        film.Metascore = Int32.Parse(Data[15]);
                    film.ImdbRating = Double.Parse(Data[16].Replace('.', ','));
                    film.ImdbVotes = Int32.Parse(string.Join("", Data[17].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)));
                    if (Data[18] != "N/A")
                        film.DVD = DateTime.Parse(Data[18]);
                    if (Data[19] != "N/A")
                        film.BoxOffice = Int32.Parse(string.Join("", Data[19].Split(new char[] { ',', '$' }, StringSplitOptions.RemoveEmptyEntries)));
                    if (Data[21] != "N/A")
                        film.WebSite = Data[21];




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
                        if(regexRole.IsMatch(item))
                        {
                            role = regexRole.Match(item).Value.Trim(new char[] { '(', ')' });
                            personName = regexRole.Replace(personName, string.Empty).Trim(new char[] { ' ' });
                        }
                        if (role!="")
                            film.Person.Add(new PersonAndRole() { Name = personName, Role = role });
                        else
                            film.Person.Add(new PersonAndRole() { Name = personName, Role = "Writer" });
                    }
                    //актеры
                    splitdata = Data[8].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in splitdata)
                        film.Person.Add(new PersonAndRole() { Name =item.Trim(new char[] { ' ' }), Role = "Actors" });
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





                    Data.Clear();
                }
            //}
            //catch (Exception error)
            //{
            //    excel.Close();
           //}




            //var str2 = str[7].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //var str3 = str2[0].Split(new char[] { ' ', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
            //textBox1.Text = str3.ToString();
        }

    }
}



//бекап
/*
 using (StreamReader reader = new StreamReader(Application.StartupPath + @"\Resources\Данные.csv"))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    //считывание строки
                    string line = reader.ReadLine();
                    //получение колонок
                    var Columns = line.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    int k = 0;
                    if (Columns[0] == "The Shawshank Redemption")
                        k = 5;
                    //заполнение
                    Film film = new Film();
                    film.Title = Columns[0];
                    film.YearReleaseWorld = Int32.Parse(Columns[1]);
                    if (Columns[2] != "UNRATED")
                        film.Rated = Columns[2];
                    film.Released = DateTime.Parse(Columns[3]);
                    film.RunTime = Columns[4];
                    film.Plot = Columns[9];
                    film.Awards = Columns[12];
                    film.Poster = Columns[13];
                    film.RatingValue = Double.Parse(Columns[14].Remove(3).Replace('.', ','));
                    if (Columns[15] != "N/A")
                        film.Metascore = Int32.Parse(Columns[15]);
                    film.ImdbRating = Double.Parse(Columns[16].Replace('.', ','));
                    film.ImdbVotes = Int32.Parse(string.Join("", Columns[17].Split(new char[] {',' },StringSplitOptions.RemoveEmptyEntries)) );
                    if (Columns[18] != "N/A")
                        film.DVD = DateTime.Parse(Columns[18]);
                    if (Columns[19] != "N/A")
                        film.BoxOffice = Int32.Parse(string.Join("", Columns[19].Split(new char[] { ',', '$' }, StringSplitOptions.RemoveEmptyEntries)));
                    if (Columns[21] != "N/A")
                        film.WebSite = Columns[21];

                    //жанр
                    var splitdata = Columns[5].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in splitdata)
                        film.Genre.Add(item.Trim(new char[] {' '}));
                    //режисеры
                    splitdata = Columns[6].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in splitdata)
                    {
                        var person = item.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        film.Person.Add(new PersonAndRole() { FName = person[0].Trim(new char[] { ' ' }), LName = person[1].Trim(new char[] { ' ' }), Role = "Director" });
                    }
                    //писатели
                    //
                    //  проблема со строками наблюдается аномалия в виде двойной \"\" а так же нужно соединить разделенную строку внутри скобок
                    splitdata = Columns[7].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in splitdata)
                    {

                        var person = item.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        string role = "";
                        if (person.Length>2)
                            for (int i = 2; i < person.Length; i++)
                            {
                                role += person[i] + " ";
                            }

                        if (person.Length == 3)
                            film.Person.Add(new PersonAndRole() { FName = person[0].Trim(new char[] { ' ' }), LName = person[1].Trim(new char[] { ' ' }), Role = role.Trim(new char[] { '(', ')' }) });
                        else
                            film.Person.Add(new PersonAndRole() { FName = person[0].Trim(new char[] { ' ' }), LName = person[1].Trim(new char[] { ' ' }), Role = "Writer" });
                    }
                    //актеры
                    splitdata = Columns[8].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in splitdata)
                    {
                        var person = item.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        film.Person.Add(new PersonAndRole() { FName = person[0].Trim(new char[] { ' ' }), LName = person[1].Trim(new char[] { ' ' }), Role = "Actors" });
                    }
                    //язык
                    splitdata = Columns[10].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in splitdata)
                        film.Language.Add(item.Trim(new char[] { ' ' }));
                    //страна
                    splitdata = Columns[11].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in splitdata)
                        film.Country.Add(item.Trim(new char[] { ' ' }));
                    //издатель
                    splitdata = Columns[20].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in splitdata)
                        film.Production.Add(item.Trim(new char[] { ' ' }));
                }
                
                //var str2 = str[7].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                //var str3 = str2[0].Split(new char[] { ' ', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
                //textBox1.Text = str3.ToString();
*/
