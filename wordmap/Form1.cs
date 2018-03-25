using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using NHazm;
using WordCloudGen = WordCloud.WordCloud;



namespace wordmap
{
    public partial class Form1 : Form
    {
        //Preposition Data Table
        DataTable preposition = new DataTable();

        //verb Data Table
        DataTable verb = new DataTable();

        //sportsWords Data Table
        DataTable sportsWords = new DataTable();

        //politicswords Data Table
        DataTable politicsWords = new DataTable();

        //List to show wordmap
        List<string> wordslist = new List<string>();
        List<int> frequencylist = new List<int>();

        public Form1()
        {
            InitializeComponent();

            preposition.Columns.Add("preposition", typeof(string));
            verb.Columns.Add("verb", typeof(string));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //choose which tools are visible 
            button1.Visible = true;
            button2.Visible = true;
            button3.Visible = false;
            button4.Visible = false;
            label1.Visible = true;
            dataGridView1.Visible = false;
            dataGridView2.Visible = false;
            pictureBox1.Visible = false;

            //Load list of persian Preposition 
            string currentDirectory = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(currentDirectory, "preposition.txt");
            string[] prepositionLines = File.ReadAllLines(filePath);

            foreach (string line in prepositionLines)
            {
                DataRow dr = preposition.NewRow();
                dr["preposition"] = line;
                preposition.Rows.Add(dr);
            }

            //Load list of persian verbs
            filePath = Path.Combine(currentDirectory, "verb.txt");
            string[] verbLines = File.ReadAllLines(filePath);
        
            foreach (string line in verbLines)
            {
                DataRow dr = verb.NewRow();
                dr["verb"] = line;
                verb.Rows.Add(dr);
            }

            //Add word and count clolumn to sportsWords data table
            sportsWords.Columns.Add("word", typeof(string));
            sportsWords.Columns.Add("count", typeof(int));

            //Add word and count clolumn to politicsWords data table
            politicsWords.Columns.Add("word", typeof(string));
            politicsWords.Columns.Add("count", typeof(int));
        }


        private void button1_Click(object sender, EventArgs e)
        {
            //choose which tools are visible 
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = true;
            button4.Visible = true;
            label1.Visible = false;
            dataGridView1.Visible = true;
            dataGridView2.Visible = true;
            pictureBox1.Visible = false;
            Cursor.Current = Cursors.WaitCursor;

            //Load sports News file
            string currentDirectory = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(currentDirectory, "sportsNews.txt");
            string[] sportsLines = File.ReadAllLines(@"C:\Users\maryam\Desktop\sportsNews.txt");
                          
            //whether the word is preposition or not
            bool isPreposition;

            //whether the word is verb or not
            bool isVerb;

            //split senteneces of sports News text
            SentenceTokenizer senTokenizer = new SentenceTokenizer();

            //normalize the sentences
            Normalizer normalize = new Normalizer();

            //clear sportsWord data table
            sportsWords.Clear();

            //sportsSentences data table
            DataTable sportsSentences = new DataTable();
            sportsSentences.Clear();
            sportsSentences.Columns.Add("Sentence", typeof(string));
            sportsSentences.Columns.Add("NormalizeSentence", typeof(string));

            //split senteneces of sports News text and add to sportsSentences data table
            foreach (string line in sportsLines)
            {
                string[] sentences;
                sentences = senTokenizer.Tokenize(line).ToArray();
                foreach (string sentence in sentences)
                {
                    DataRow dr = sportsSentences.NewRow();
                    dr["Sentence"] = sentence;
                    dr["NormalizeSentence"] = normalize.Run(sentence);
                    sportsSentences.Rows.Add(dr);
                }
            }

            //Display sentences in dataGridView1
            dataGridView1.DataSource = sportsSentences;


            //split the words of sports News text and add to sportsWords data table
            foreach (string line in sportsLines)
            {
                //split each words of each lines
                string[] words = line.Split(new string[] { " ","!","\"","#","؛","،","-","$","%","&","'",
                                                           "(",")","+",".","/",":",";","<",">","=","?",
                                                           "[","]","{","}","»","«","1","2","3","4",
                                                            "5","6","7","8","9","0","١","۲", "۳","۴",
                                                            "۵","۶","۷","۸","۹","٠"}, StringSplitOptions.RemoveEmptyEntries);
                foreach (string word in words)
                {
                    //Omit preposition from sports word
                    isPreposition = false;
                    foreach (DataRow row in preposition.Rows)
                    {
                        if (row.Field<string>("preposition") == word)
                        {
                            isPreposition = true;
                            break;
                        }
                    }

                    //Omit verb from sports word
                    isVerb = false;
                   /* if (isPreposition == false)
                    {
                        //Omit verb from politics word
                        foreach (DataRow row in verb.Rows)
                        {
                            if (row.Field<string>("verb") == word)
                            {
                                isVerb = true;
                                break;
                            }
                        }
                    }*/
                    if (isPreposition == false && isVerb == false)
                    {
                        //Counting the uniqe word in sports News
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

            //Display sportsWords in dataGridView2 
            dataGridView2.DataSource = sportsWords;                 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //choose which tools are visible 
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = true;
            button4.Visible = true;
            label1.Visible = false;
            dataGridView1.Visible = true;
            dataGridView2.Visible = true;
            pictureBox1.Visible = false;
            Cursor.Current = Cursors.WaitCursor;

            //Load politics News
            string currentDirectory = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(currentDirectory, "politicsNews .txt");
            string[] politicsLines = File.ReadAllLines(@"C:\Users\maryam\Desktop\politicsNews .txt");           

            //whether the word is preposition or not
            bool isPreposition;

            //whether the word is verb or not
            bool isVerb;
          
            //split senteneces of politics News text
            SentenceTokenizer senTokenizer = new SentenceTokenizer();

            //normalize the sentences
            Normalizer normalize = new Normalizer();

            //clear politicsWords data table
            politicsWords.Clear();

            //politicsSentences
            DataTable politicsSentences = new DataTable();
            politicsSentences.Clear();
            politicsSentences.Columns.Add("Sentence", typeof(string));
            politicsSentences.Columns.Add("NormalizeSentence", typeof(string));

            //split senteneces of politics News text and add to politicsSentences data table
            foreach (string line in politicsLines)
            {
                string[] sentences;
                sentences = senTokenizer.Tokenize(line).ToArray();
                foreach (string sentence in sentences)
                {
                    DataRow dr = politicsSentences.NewRow();
                    dr["Sentence"] = sentence;
                    dr["NormalizeSentence"] = normalize.Run(sentence);
                    politicsSentences.Rows.Add(dr);
                }
            }

            //Display sentences in dataGridView1
            dataGridView1.DataSource = politicsSentences;

            //split the words of politics News text and add to politicsWords data table
            foreach (string line in politicsLines)
            {              
                //split each words of each lines
                string[] words = line.Split(new string[] { " ","!","\"","#","؛","،","-","$","%","&","'",
                                                           "(",")","+",".","/",":",";","<",">","=","?",
                                                           "[","]","{","}","»","«","1","2","3","4",
                                                            "5","6","7","8","9","0","١","۲", "۳","۴",
                                                            "۵","۶","۷","۸","۹","٠"}, StringSplitOptions.RemoveEmptyEntries);

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

                    isVerb = false;
                    if (isPreposition == false)
                    {
                        //Omit verb from politics word
                        foreach (DataRow row in verb.Rows)
                        {
                            if (row.Field<string>("verb") == word)
                            {
                                isVerb = true;
                                break;
                            }
                        }
                    }
                    if (isPreposition == false && isVerb == false)
                    {
                        //Counting the uniqe word in politics News
                        bool isrepeated = false;
                        for (int i = 0; i < politicsWords.Rows.Count; i++)
                        {
                            if (word == politicsWords.Rows[i]["word"].ToString())
                            {
                                politicsWords.Rows[i]["count"] = int.Parse(politicsWords.Rows[i]["count"].ToString()) + 1;
                                isrepeated = true;
                                break;
                            }
                        }
                        if (isrepeated == false)
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

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //choose which tools are visible 
            button1.Visible = true;
            button2.Visible = true;
            button3.Visible = false;
            button4.Visible = false;
            label1.Visible = true;
            dataGridView1.Visible = false;
            dataGridView2.Visible = false;
            pictureBox1.Visible = false;
            Cursor.Current = Cursors.WaitCursor;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //choose which tools are visible 
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = true;
            button4.Visible = false;
            label1.Visible = false;
            dataGridView1.Visible = false;
            dataGridView2.Visible = false;            
            Cursor.Current = Cursors.WaitCursor;

            //clear wordslist
            wordslist.Clear();

            //clear frequencylist
            frequencylist.Clear();

            //add sports words and frequency to the list
            if (sportsWords.Rows.Count > 0)
            {
                for (int i = 0; i < sportsWords.Rows.Count; i++)
                {
                    wordslist.Add(sportsWords.Rows[i]["word"].ToString());
                    frequencylist.Add(int.Parse(sportsWords.Rows[i]["count"].ToString()));
                }
            }
            else
            {
                for (int i = 0; i < politicsWords.Rows.Count; i++)
                {
                    wordslist.Add(politicsWords.Rows[i]["word"].ToString());
                    frequencylist.Add(int.Parse(politicsWords.Rows[i]["count"].ToString()));
                }
            }

            //create word cloud generation
            var wc = new WordCloudGen(pictureBox1.Width,pictureBox1.Height);

            // display wordmap image of sports news
            Image newImage;
            newImage = wc.Draw(wordslist, frequencylist);

            pictureBox1.Image = newImage;
            pictureBox1.Visible = true;            
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
        }       
    }
}
