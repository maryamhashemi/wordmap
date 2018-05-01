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
using WordCloudGen = WordCloud.WordCloud;


namespace wordmap
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }            

        private void button1_Click_1(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
                
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "sportsNews.txt");
            string sportsText = File.ReadAllText(filePath);

            wordmap wm = new wordmap();
            List<string> prepVerbList = wm.GetprepVerbList();
            label2.Text = "loading preposition and verb files ...";

            sportsText = wm.normalize(sportsText);
            label2.Text = "normalizing text ...";

            Dictionary<string, int> sentences = wm.GetSentences(sportsText);
            label2.Text = "tokenizing sentences ...";

            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("Values", "ID");
            dataGridView1.Columns.Add("Key", "SENTENCE");

            foreach (KeyValuePair<string, int> item in sentences)
            {
                dataGridView1.Rows.Add(item.Value, item.Key);
            }
            label2.Text = "displaying  sentences in datagridview ...";

            string[] words = wm.tokenize(sportsText);
            label2.Text = "tokenizing text ...";

            Dictionary<string, int> wordCount = wm.CalcWordCount(words);
            label2.Text = "lemmatizing and counting words frequencies ...";

            wordCount = wm.deletePrepVerb(prepVerbList, wordCount);
            label2.Text = "deleting preposition and verb from words ...";

            var items = from pair in wordCount
                        orderby pair.Value descending
                        select pair;

            dataGridView2.Rows.Clear();
            dataGridView2.Columns.Clear();
            dataGridView2.Columns.Add("Key", "WORD");
            dataGridView2.Columns.Add("Values", "COUNT");
           
            foreach (var kvp in items)
            {
                dataGridView2.Rows.Add(kvp.Key, kvp.Value);
            }
            label2.Text = "displaying  wordCount in datagridview ...";

            pictureBox1.Image = wm.getImageWordmap(wordCount,pictureBox1.Width,pictureBox1.Height);
            label2.Text = "displaying wordmap ...";
            label2.Text = "done";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
 
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "politicsNews .txt");
            string politicsText = File.ReadAllText(filePath);

            wordmap wm = new wordmap();
            List<string> prepVerbList = wm.GetprepVerbList();
            label2.Text = "loading preposition and verb files ...";

            politicsText = wm.normalize(politicsText);
            label2.Text = "normalizing text ...";

            Dictionary<string, int> sentences = wm.GetSentences(politicsText);
            label2.Text = "tokenizing sentences ...";

            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("Values", "ID");
            dataGridView1.Columns.Add("Key", "SENTENCE");

            foreach (KeyValuePair<string, int> item in sentences)
            {
                dataGridView1.Rows.Add(item.Value, item.Key);
            }
            label2.Text = "displaying  sentences in datagridview ...";

            string[] words = wm.tokenize(politicsText);
            label2.Text = "tokenizing text ...";

            Dictionary<string, int> wordCount = wm.CalcWordCount(words);
            label2.Text = "lemmatizing and counting words frequencies ...";

            wordCount = wm.deletePrepVerb(prepVerbList, wordCount);
            label2.Text = "deleting preposition and verb from words ...";

            var items = from pair in wordCount
                        orderby pair.Value descending
                        select pair;

            dataGridView2.Rows.Clear();
            dataGridView2.Columns.Clear();
            dataGridView2.Columns.Add("Key", "WORD");
            dataGridView2.Columns.Add("Values", "COUNT");

            foreach (var kvp in items)
            {
                dataGridView2.Rows.Add(kvp.Key, kvp.Value);
            }
            label2.Text = "displaying  wordCount in datagridview ...";

            pictureBox1.Image = wm.getImageWordmap(wordCount, pictureBox1.Width, pictureBox1.Height);
            label2.Text = "displaying wordmap ...";
            label2.Text = "done";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;   
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "sportsNews.txt");
            string sportsText = File.ReadAllText(filePath);

            wordmap wm = new wordmap();
            List<string> prepVerbList = wm.GetprepVerbList();
            label2.Text = "loading preposition and verb files ...";

            sportsText = wm.normalize(sportsText);
            label2.Text = "normalizing text ...";

            string[] words = wm.tokenize(sportsText);
            label2.Text = "tokenizing text ...";

            Dictionary<string, int> SportswordCount = wm.CalcWordCount(words);
            label2.Text = "lemmatizing and counting words frequencies ...";

            SportswordCount = wm.deletePrepVerb(prepVerbList, SportswordCount);
            label2.Text = "deleting preposition and verb from words ...";

            filePath = Path.Combine(Directory.GetCurrentDirectory(), "politicsNews .txt");
            string politicsText = File.ReadAllText(filePath);

            politicsText = wm.normalize(politicsText);
            label2.Text = "normalizing text ...";

            words = wm.tokenize(politicsText);
            label2.Text = "tokenizing text ...";

            Dictionary<string, int> PoliticswordCount = wm.CalcWordCount(words);
            label2.Text = "lemmatizing and counting words frequencies ...";

            PoliticswordCount = wm.deletePrepVerb(prepVerbList, PoliticswordCount);
            label2.Text = "deleting preposition and verb from words ...";

            decimal totalsportsWordCount = (decimal)SportswordCount.Sum(x => x.Value);
            decimal totalpoliticsWordCount = (decimal)PoliticswordCount.Sum(x => x.Value);


            List<string> AllWord = new List<string>();
            List<string> sportsWordList = new List<string>();
            List<string> politicsWordList = new List<string>();
            sportsWordList = SportswordCount.Keys.ToList();
            politicsWordList = PoliticswordCount.Keys.ToList();
            AllWord = sportsWordList.Union(politicsWordList).ToList();


            DataTable Allword = new DataTable();
            Allword.Columns.Add("word", typeof(string));
            Allword.Columns.Add("SportsCount", typeof(decimal));
            Allword.Columns.Add("PoliticsCount", typeof(decimal));
            Allword.Columns.Add("Distance", typeof(decimal));

            foreach(string word  in AllWord)
            {
                decimal sportValue;
                decimal politicsValue;

                if (SportswordCount.ContainsKey(word))
                    sportValue = (decimal)SportswordCount[word];
                else
                    sportValue = 0;
                if (PoliticswordCount.ContainsKey(word))
                    politicsValue = (decimal)PoliticswordCount[word];
                else
                    politicsValue = 0;
                decimal distance = (sportValue / totalsportsWordCount) - (politicsValue / totalpoliticsWordCount);
                Allword.Rows.Add(word, sportValue, politicsValue, distance);
            }


            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            Allword.DefaultView.Sort = "Distance DESC";
            Allword = Allword.DefaultView.ToTable();
            dataGridView1.DataSource = Allword;

            List<string> wordslist = new List<string>();
            List<int> frequencylist = new List<int>();

            for (int i = 0; i < Allword.Rows.Count; i++)
            {
                wordslist.Add(Allword.Rows[i]["word"].ToString());
                frequencylist.Add((int)(Math.Abs(Convert.ToDecimal(Allword.Rows[i]["Distance"].ToString()) * 10000)));

            }

            //create word cloud generation
            var wc = new WordCloudGen(pictureBox1.Width, pictureBox1.Height);

            // display wordmap image of sports news
            Image newImage = wc.Draw(wordslist, frequencylist);
            pictureBox1.Image = newImage;
            label2.Text = "done";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            NaiveBayes NB = new NaiveBayes();
             NB.classifier();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            label2.Text = "converting txt file to vowpal wabbit format ...";

            vowpalWabbit VW = new vowpalWabbit();

            string c1path = @"C:\Users\maryam\Documents\Visual Studio 2015\Projects\wordmap\data\train\sportsTrain.txt";
            string c2path = @"C:\Users\maryam\Documents\Visual Studio 2015\Projects\wordmap\data\train\politicsTrain.txt";
            string vwfilepath = @"C:\Users\maryam\Documents\Visual Studio 2015\Projects\wordmap\trainVW.txt";
            VW.convertToVW(c1path,c2path,vwfilepath);

            c1path = @"C:\Users\maryam\Documents\Visual Studio 2015\Projects\wordmap\data\test\sportsTest.txt";
            c2path = @"C:\Users\maryam\Documents\Visual Studio 2015\Projects\wordmap\data\test\politicsTest.txt";
            vwfilepath = @"C:\Users\maryam\Documents\Visual Studio 2015\Projects\wordmap\testVW.txt";
            VW.convertToVW(c1path, c2path, vwfilepath);

            double[] test_labels = VW.getTestLabels(vwfilepath);

            string predictionPath = @"C:\Users\maryam\Documents\Visual Studio 2015\Projects\wordmap\predictions_1gram.txt";
            double[] prediction_labels = VW.getPredictionLabels(predictionPath);

            double precision = VW.calcPrecision(test_labels, prediction_labels,4);
            double recall = VW.calcRecall(test_labels, prediction_labels,4);

            Console.WriteLine($"Precision = {precision}");
            Console.WriteLine($"Recall = {recall}");

            label2.Text = "done";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label2.Refresh();
        }
    }
}
