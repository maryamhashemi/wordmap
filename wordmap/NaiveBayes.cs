using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NHazm;

namespace wordmap
{
    class NaiveBayes
    {
        public void classifier()
        {
            string sportsText = File.ReadAllText(@"C:\Users\maryam\Documents\Visual Studio 2015\Projects\wordmap\data\train\sportsTrain.txt");
            string politicsText = File.ReadAllText(@"C:\Users\maryam\Documents\Visual Studio 2015\Projects\wordmap\data\train\politicsTrain.txt");

            sportsText = normalize(sportsText);
            politicsText = normalize(politicsText);

            HashSet<string> ALLWords = uniqeTokens(sportsText, politicsText);

            Dictionary<string, double> sportsWordLogProbs = CalulateWordLogProbs(sportsText, ALLWords);
            Dictionary<string, double> politicsWordLogProbs = CalulateWordLogProbs(politicsText, ALLWords);

            double sportsLogProb = Math.Log(0.5);
            double politicsLogProb = Math.Log(0.5);
         
            string testFiles = @"C:\Users\maryam\Documents\Visual Studio 2015\Projects\wordmap\data\test\*.txt";
          
            foreach(var testFile in Directory.GetFiles(Path.GetDirectoryName(testFiles),Path.GetFileName(testFiles)))
            {
                Dictionary<string, double> feature = new Dictionary<string, double>();

                double sportsLogProbSum = sportsLogProb;
                double politicsLogProbSum = politicsLogProb;

                foreach (string word in tokenize(File.ReadAllText(testFile)))
                {
                    double wordSportsLogProb;
                    double wordPoliticsLogProb;
                    if (sportsWordLogProbs.TryGetValue(word, out wordSportsLogProb))
                    {
                        sportsLogProbSum += wordSportsLogProb;
                    }
                    else
                    {
                        wordSportsLogProb = sportsWordLogProbs["unknown"];
                        sportsLogProbSum += wordSportsLogProb;
                    }

                    if (politicsWordLogProbs.TryGetValue(word, out wordPoliticsLogProb))
                    {
                        politicsLogProbSum += wordPoliticsLogProb;
                    }
                    else
                    {
                        wordPoliticsLogProb = politicsWordLogProbs["unknown"];
                        politicsLogProbSum += wordPoliticsLogProb;
                    }
                    feature[word] = Math.Abs(wordSportsLogProb - wordPoliticsLogProb);
                }

                System.Diagnostics.Debug.WriteLine($"{Path.GetFileName(testFile)} is class {(sportsLogProbSum > politicsLogProbSum ? 1 : 2)}");
                System.Diagnostics.Debug.WriteLine($"\t Class1:{sportsLogProbSum} Class2:{politicsLogProbSum}");
                var topFeature = feature.OrderByDescending(x => x.Value).Take(5).Select(x => x.Key);
                System.Diagnostics.Debug.WriteLine($"\t TopFeature: {string.Join(" ", topFeature)}");
            }    
        }
        public string normalize(string text)
        {
            Normalizer normalizer = new Normalizer();
            return normalizer.Run(text);
        }
        public string[] tokenize(string text)
        {
            char[] delimiters = new char[] {
              ',', '"', ')', '(', ';', '.', '\n', '\r', '\t',
              '>', '<', ':', '=', '\'', '[', ']',' ','!','#',
              '؛','،','-','$','%','&','\'', '+','/','?','؟',
              '{','}','»','«','1','2','3','4','5','6','7','8',
              '9','0','١','۲', '۳','۴','۵','۶','۷','۸','۹','٠'};

            return normalize(text).Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
        }
        public HashSet<string> uniqeTokens(string textClass1 , string textClass2)
        {
            string[] wordsClass1 = tokenize(textClass1);
            string[] wordsClass2 = tokenize(textClass2 + " unknown");

            var Allwords = wordsClass1.Union(wordsClass2);
            return new HashSet<string>(Allwords);
        }
        public Dictionary<string,double> CalulateWordLogProbs(string textClass, HashSet<string> ALLWords)
        {
            Dictionary<string, int> wordCount = new Dictionary<string, int>();

            string[] wordClass = tokenize(textClass+ " unknown");

            foreach(string word in wordClass)
            {
                if (wordCount.ContainsKey(word))
                    wordCount[word]++;
                else
                    if(word == "unknown")
                        wordCount.Add(word, 1);
                    else 
                        wordCount.Add(word, 1 + 1); // +1 smoothing
            }

            int totalWordCount = wordCount.Sum(x => x.Value);
            Dictionary<string, double> wordLogProbs = new Dictionary<string, double>();

            foreach (var word in wordCount)
            {
                wordLogProbs[word.Key] = Math.Log((double)(word.Value) / (totalWordCount + ALLWords.Count));
            }
            return wordLogProbs;
        }
    }
}
