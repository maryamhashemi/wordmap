using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace wordmap
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Load list of persian Preposition 
            string[] prepositionLines = System.IO.File.ReadAllLines(@"C:\Users\maryam\Desktop\preposition.txt");

            //Preposition Data Table
            DataTable preposition = new DataTable();
            preposition.Columns.Add("preposition", typeof(string));

            foreach (string line in prepositionLines)
            {
                DataRow dr = preposition.NewRow();
                dr["preposition"] = line;
                preposition.Rows.Add(dr);
            }
            //Display preposition in dataGrid 
            dataGridView1.DataSource = preposition;

            //Load politics News
            string[] politicsLines = System.IO.File.ReadAllLines(@"C:\Users\maryam\Desktop\politicsNews .txt");

            //politicswords Data Table
            DataTable politicsWords = new DataTable();
            politicsWords.Columns.Add("word", typeof(string));
            politicsWords.Columns.Add("count", typeof(int));

            bool isPreposition;
            foreach (string line in politicsLines)
            {
                string[] words = line.Split(new string[] { " ","!","\"","#","؛","،","-","$","%","&","'",
                                                           "(",")","+",".","/",":",";","<",">","=","?",
                                                           "[","]","{","}","»","«","1","2","3","4",
                                                            "5","6","7","8","9","0"}, StringSplitOptions.RemoveEmptyEntries);
                foreach (string word in words)
                {
                    //Omit preposition from politics word
                    isPreposition = false;
                    foreach (DataRow row in preposition.Rows)
                    {
                        if(row.Field<string>("preposition") == word)
                        {
                            isPreposition = true;
                            break;
                        }
                    }
                    if(isPreposition == false)
                    {
                        //Counting the identicl word in politics News
                        bool isrepeated = false;
                        for(int i=0; i<politicsWords.Rows.Count; i++)
                        {
                            if(word == politicsWords.Rows[i]["word"].ToString())
                            {
                                politicsWords.Rows[i]["count"] = int.Parse(politicsWords.Rows[i]["count"].ToString()) + 1;
                                isrepeated = true;
                                break;
                            }
                        }
                        if(isrepeated == false)
                        {
                            DataRow dr = politicsWords.NewRow();
                            dr["word"] = word;
                            dr["count"] = 1;
                            politicsWords.Rows.Add(dr);
                        } 
                    }  
                }
            }

            //sort the politics words by number of repeated
            politicsWords.DefaultView.Sort = "count desc";
            politicsWords = politicsWords.DefaultView.ToTable();

            //Display politicsWords in dataGrid 
            dataGridView2.DataSource = politicsWords;

            //Load sports News file
            string[] sportsLines = System.IO.File.ReadAllLines(@"C:\Users\maryam\Desktop\sportsNews.txt");

            //sportsWords Data Table
            DataTable sportsWords = new DataTable();
            sportsWords.Columns.Add("word", typeof(string));
            sportsWords.Columns.Add("count", typeof(int));

            foreach (string line in sportsLines)
            {
                string[] words = line.Split(new string[] { " ","!","\"","#","؛","،","-","$","%","&","'",
                                                           "(",")","+",".","/",":",";","<",">","=","?",
                                                           "[","]","{","}","»","«","1","2","3","4",
                                                            "5","6","7","8","9","0"}, StringSplitOptions.RemoveEmptyEntries);
                foreach (string word in words)
                {
                    //Omit preposition from politics word
                    isPreposition = false;
                    foreach (DataRow row in preposition.Rows)
                    {
                        if (row.Field<string>("preposition") == word)
                        {
                            isPreposition = true;
                            break;
                        }
                    }
                    if (isPreposition == false)
                    {
                        //Counting the identicl word in sports News
                        bool isrepeated = false;
                        for (int i = 0; i < sportsWords.Rows.Count; i++)
                        {
                            if (word == sportsWords.Rows[i]["word"].ToString())
                            {
                                sportsWords.Rows[i]["count"] = int.Parse(sportsWords.Rows[i]["count"].ToString()) + 1;
                                isrepeated = true;
                                break;
                            }
                        }
                        if (isrepeated == false)
                        {
                            DataRow dr = sportsWords.NewRow();
                            dr["word"] = word;
                            dr["count"] = 1;
                            sportsWords.Rows.Add(dr);
                        }
                    }
                }

            }
            //sort the sports words by number of repeated
            sportsWords.DefaultView.Sort = "count desc";
            sportsWords = sportsWords.DefaultView.ToTable();

            //Display sportsWords in dataGrid 
            dataGridView3.DataSource = sportsWords;

            
        }
    }
}
