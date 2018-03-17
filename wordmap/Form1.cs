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

            bool isPreposition;
            foreach (string line in politicsLines)
            {
                string[] words = line.Split(new string[] { " ", ":", "/", ".", "؛", "،", "-" }, StringSplitOptions.RemoveEmptyEntries);
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
                        DataRow dr = politicsWords.NewRow();
                        dr["word"] = word;
                        politicsWords.Rows.Add(dr);
                    }  
                }
            }
            //Display politicsWords in dataGrid 
            dataGridView2.DataSource = politicsWords;

            //Load sports News file
            string[] sportsLines = System.IO.File.ReadAllLines(@"C:\Users\maryam\Desktop\sportsNews.txt");

            //sportsWords Data Table
            DataTable sportsWords = new DataTable();
            sportsWords.Columns.Add("word", typeof(string));


            foreach (string line in sportsLines)
            {
                string[] words = line.Split(new string[] { " ", ":", "/", ".", "؛", "،","-" }, StringSplitOptions.RemoveEmptyEntries);
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
                        DataRow dr = sportsWords.NewRow();
                        dr["word"] = word;
                        sportsWords.Rows.Add(dr);
                    }
                }

            }
            //Display sportsWords in dataGrid 
            dataGridView3.DataSource = sportsWords;

            
        }
    }
}
