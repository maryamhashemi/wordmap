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
using System.Text.RegularExpressions;



namespace wordmap
{
    public partial class Form1 : Form
    {
        //split senteneces of text
        public SentenceTokenizer senTokenizer = new SentenceTokenizer();

        //normalize the sentences
        public Normalizer normalize = new Normalizer();

        //lemmatize the word
        public Lemmatizer lemmatizer = new Lemmatizer();

        public Form1()
        {
            InitializeComponent();
        }            

        private void button1_Click_1(object sender, EventArgs e)
        {
            List<string> prepVerbList = new List<string>();

            //Load list of persian Preposition 
            string currentDirectory = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(currentDirectory, "preposition.txt");
            string[] prepositionLines = File.ReadAllLines(filePath);

            foreach (string line in prepositionLines)
            {
                prepVerbList.Add(line);
            }

            //Load list of persian verbs
            filePath = Path.Combine(currentDirectory, "verb.txt");
            string[] verbLines = File.ReadAllLines(filePath);

            foreach (string line in verbLines)
            {
                prepVerbList.Add(line);
            }

            //Load sports News file
            currentDirectory = Directory.GetCurrentDirectory();
            filePath = Path.Combine(currentDirectory, "sportsNews.txt");
            string[] sportsLines = File.ReadAllLines(filePath);

            Dictionary<string, int> sportsSenDic = new Dictionary<string, int>();
            int sportsSenCount = 0;

            //split senteneces of sports News text and add to sportsSentences data table
            foreach (string line in sportsLines)
            {
                string[] sentences;
                sentences = senTokenizer.Tokenize(line).ToArray();

                foreach (string sentence in sentences)
                {
                    if (!sportsSenDic.ContainsKey(normalize.Run(sentence)))
                    {
                        sportsSenDic.Add(normalize.Run(sentence), sportsSenCount++);
                    }
                }
            }

            char[] delimiters = new char[] {
              ',', '"', ')', '(', ';', '.', '\n', '\r', '\t',
              '>', '<', ':', '=', '\'', '[', ']',' ','!','#',
              '؛','،','-','$','%','&','\'', '+','/','?','؟',
              '{','}','»','«','1','2','3','4','5','6','7','8',
              '9','0','١','۲', '۳','۴','۵','۶','۷','۸','۹','٠'};

            Dictionary<string, int> sportsWord = new Dictionary<string, int>();

            //split the words of sports News text and add to sportsWords data table
            foreach (string line in sportsLines)
            {
                //split each words of each lines
                string[] words = normalize.Run(line).Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                foreach (string word in words)
                {
                    string lemWord =word;
                    if(word.Length > 5)
                        lemWord= lemmatizer.Lemmatize(word);

                    if (sportsWord.ContainsKey(lemWord))
                        sportsWord[lemWord] += 1;
                    else
                        sportsWord.Add(lemWord, 1);
                }
            }

            //delete preposition and verb from sports words
            foreach(string element in prepVerbList)
            {
                string lemElement = element;
                if (element.Length > 5)
                     lemElement = lemmatizer.Lemmatize(element);

                if (sportsWord.ContainsKey(lemElement))
                    sportsWord.Remove(lemElement);
            }

            //List to show wordmap
            List<string> wordslist = new List<string>();
            List<int> frequencylist = new List<int>();

            var items = from pair in sportsWord
                        orderby pair.Value descending
                        select pair;

            //add sports words and frequency to the list           
            foreach (var kvp in items)
            {
                    wordslist.Add(kvp.Key);
                    frequencylist.Add(kvp.Value);
            }

            //create word cloud generation
            var wc = new WordCloudGen(pictureBox1.Width, pictureBox1.Height);

            // display wordmap image of sports news
            Image newImage = wc.Draw(wordslist, frequencylist);

            pictureBox1.Image = newImage;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<string> prepVerbList = new List<string>();

            //Load list of persian Preposition 
            string currentDirectory = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(currentDirectory, "preposition.txt");
            string[] prepositionLines = File.ReadAllLines(filePath);

            foreach (string line in prepositionLines)
            {
                prepVerbList.Add(line);
            }

            //Load list of persian verbs
            filePath = Path.Combine(currentDirectory, "verb.txt");
            string[] verbLines = File.ReadAllLines(filePath);

            foreach (string line in verbLines)
            {
                prepVerbList.Add(line);
            }

            //Load politics News file
            currentDirectory = Directory.GetCurrentDirectory();
            filePath = Path.Combine(currentDirectory, "politicsNews .txt");
            string[] politicsLines = File.ReadAllLines(filePath);

            Dictionary<string, int> politicsSenDic = new Dictionary<string, int>();
            int politicsSenCount = 0;

            //split senteneces of politics News text and add to politicsSenDic dictionary
            foreach (string line in politicsLines)
            {
                string[] sentences;
                sentences = senTokenizer.Tokenize(line).ToArray();

                foreach (string sentence in sentences)
                {
                    if (!politicsSenDic.ContainsKey(normalize.Run(sentence)))
                    {
                        politicsSenDic.Add(normalize.Run(sentence), politicsSenCount++);
                    }
                }
            }

            char[] delimiters = new char[] {
              ',', '"', ')', '(', ';', '.', '\n', '\r', '\t',
              '>', '<', ':', '=', '\'', '[', ']',' ','!','#',
              '؛','،','-','$','%','&','\'', '+','/','?','؟',
              '{','}','»','«','1','2','3','4','5','6','7','8',
              '9','0','١','۲', '۳','۴','۵','۶','۷','۸','۹','٠'};

            Dictionary<string, int> politicsWord = new Dictionary<string, int>();

            //split the words of politics News text and add to sportsWord dictionary
            foreach (string line in politicsLines)
            {
                //split each words of each lines
                string[] words = normalize.Run(line).Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                foreach (string word in words)
                {
                    string lemWord = word;
                    if (word.Length > 5)
                        lemWord = lemmatizer.Lemmatize(word);

                    if (politicsWord.ContainsKey(lemWord))
                        politicsWord[lemWord] += 1;
                    else
                        politicsWord.Add(lemWord, 1);
                }
            }

            //delete preposition and verb from politics words
            foreach (string element in prepVerbList)
            {
                string lemElement = element;
                if (element.Length > 5)
                    lemElement = lemmatizer.Lemmatize(element);

                if (politicsWord.ContainsKey(lemElement))
                    politicsWord.Remove(lemElement);
            }

            //List to show wordmap
            List<string> wordslist = new List<string>();
            List<int> frequencylist = new List<int>();

            var items = from pair in politicsWord
                        orderby pair.Value descending
                        select pair;

            //add sports words and frequency to the list           
            foreach (var kvp in items)
            {
                wordslist.Add(kvp.Key);
                frequencylist.Add(kvp.Value);
            }

            //create word cloud generation
            var wc = new WordCloudGen(pictureBox1.Width, pictureBox1.Height);

            // display wordmap image of sports news
            Image newImage = wc.Draw(wordslist, frequencylist);

            pictureBox1.Image = newImage;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

    }
}
