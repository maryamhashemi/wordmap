using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NHazm;
using System.Drawing;
using WordCloudGen = WordCloud.WordCloud;

namespace wordmap
{
    class wordmap
    {
        public List<string> GetprepVerbList()
        {
            List<string> prepVerbList = new List<string>();

            //Load list of persian Preposition 
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "preposition.txt");
            string[] prepositionLines = File.ReadAllLines(filePath);

            foreach (string line in prepositionLines)
            {
                prepVerbList.Add(line);
            }

            //Load list of persian verbs
            filePath = Path.Combine(Directory.GetCurrentDirectory(), "verb.txt");
            string[] verbLines = File.ReadAllLines(filePath);

            foreach (string line in verbLines)
            {
                prepVerbList.Add(line);
            }
            return prepVerbList;
        }

        public string normalize(string text)
        {
            Normalizer normalize = new Normalizer();
            return normalize.Run(text);
        }

        public string lemmatize(string word)
        {
            Lemmatizer lemmatizer = new Lemmatizer();
            return (word.Length >= 5 ? lemmatizer.Lemmatize(word) : word);
        }

        public string[] tokenize(string text)
        {
            char[] delimiters = new char[] {
              ',', '"', ')', '(', ';', '.', '\n', '\r', '\t',
              '>', '<', ':', '=', '\'', '[', ']',' ','!','#',
              '؛','،','-','$','%','&','\'', '+','/','?','؟',
              '{','}','»','«','1','2','3','4','5','6','7','8',
              '9','0','١','۲', '۳','۴','۵','۶','۷','۸','۹','٠'};

            return text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
        }
            
        public Dictionary<string,int> GetSentences(string text)
        {
            Dictionary<string, int> SentencesDic = new Dictionary<string, int>();
            int SentCount = 0;

            SentenceTokenizer senTokenizer = new SentenceTokenizer();

             string[] sentences = senTokenizer.Tokenize(text).ToArray();
 
            foreach (string sentence in sentences)
            {
                if (!SentencesDic.ContainsKey(sentence))
                {
                    SentencesDic.Add(sentence, SentCount++);
                }
            }

            return SentencesDic;       
        }

        public Dictionary<string, int> CalcWordCount(string[] words)
        {
            Dictionary<string, int> wordCount = new Dictionary<string, int>();
            Lemmatizer lemmatize = new Lemmatizer();

            foreach (string word in words)
            {
                string lemmWord = word;
                if (word.Length >= 5)
                    lemmWord = lemmatize.Lemmatize(word);

                if (wordCount.ContainsKey(lemmWord))
                    wordCount[lemmWord]++;
                else
                    wordCount.Add(lemmWord, 1);
            }

            return wordCount;
        }

        public Dictionary<string,int> deletePrepVerb(List<string> prepVerbList,Dictionary<string,int> wordCount)
        {
            Lemmatizer lemmatize = new Lemmatizer();
            foreach(string element in prepVerbList)
            {
                string lemElement = element;
                if (element.Length >= 5)
                    lemElement = lemmatize.Lemmatize(element);
                if (wordCount.ContainsKey(lemElement))
                    wordCount.Remove(lemElement);
            }

            return wordCount;
        }

        public Image getImageWordmap(Dictionary<string, int> wordCount,int width, int height)
        {
            //List to show wordmap
            List<string> wordslist = new List<string>();
            List<int> frequencylist = new List<int>();

            var items = from pair in wordCount
                        orderby pair.Value descending
                        select pair;

            //add sports words and frequency to the list           
            foreach (var kvp in items)
            {
                wordslist.Add(kvp.Key);
                frequencylist.Add(kvp.Value);
            }

            //create word cloud generation
            var wc = new WordCloudGen(width, height);

            // display wordmap image of sports news
            Image wordmap = wc.Draw(wordslist, frequencylist);

            return wordmap;
        }
    }
}
