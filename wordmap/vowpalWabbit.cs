using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using NHazm;
using VW;
using VW.Serializer.Attributes;

namespace wordmap
{
    class vowpalWabbit
    {
        public void train()
        {
           

        }
        public void convertToVW(string class1path, string class2path, string vwfilepath)
        {
            string[] c1 = File.ReadAllLines(class1path);
            string[] c2 = File.ReadAllLines(class2path);

            int count = 2 * Math.Min(c1.Count(), c2.Count());

            int c1Index = 0;
            int c2Index = 0;
            string[] contents = new string[count];

            for(int i = 0; i < count; i++)
            {
                int C = i % 2;
                if(C == 0)
                {
                    contents[i] = "1 | " + c1[c1Index];
                    c1Index++;
                }
                else
                {
                    contents[i] = "-1 | " + c2[c2Index];
                    c2Index++;
                }
            }

            File.WriteAllLines(vwfilepath, contents);
        }
        public double calcPrecision(double[] test_labels, double[] prediction_label, int boundary)
        {
            double precision;

            double tp = 0;
            double tn = 0;
            double fp = 0;
            double fn = 0;

            for (int i = 0; i < test_labels.Length; i++)
            {
                if (test_labels[i] == 1 && prediction_label[i] > boundary)
                    tp = tp + 1;
                else if (test_labels[i] == 1 && prediction_label[i] < (-1*boundary))
                    fn = fn + 1;
                else if (test_labels[i] != 1 && prediction_label[i] > boundary)
                    fp = fp + 1;
                else if (test_labels[i] != 1 && prediction_label[i] < (-1 * boundary))
                    tn = tn + 1;
            }
            precision = tp / (tp + fp);
            return precision;
        }
        public double calcRecall(double[] test_labels, double[] prediction_label,int boundary)
        {
            double recall;

            double tp = 0;
            double tn = 0;
            double fp = 0;
            double fn = 0;

            for (int i = 0; i < test_labels.Length; i++)
            {
                if (test_labels[i] == 1 && prediction_label[i] > boundary)
                    tp = tp + 1;
                else if (test_labels[i] == 1 && prediction_label[i] < (-1 * boundary))
                    fn = fn + 1;
                else if (test_labels[i] != 1 && prediction_label[i] > boundary)
                    fp = fp + 1;
                else if (test_labels[i] != 1 && prediction_label[i] < (-1 * boundary))
                    tn = tn + 1;
            }
            recall = tp / (tp + fn);
            return recall;
        }
        public double[] getTestLabels(string testPath)
        {
            string[] lines = File.ReadAllLines(testPath);
            double[] test_labels = new double[lines.Length];


            var myregex = new Regex(@"(1|-1)");
            int index = 0;

            foreach (string line in lines)
            {
                if (myregex.IsMatch(line))
                {
                    Match match = myregex.Match(line);
                    test_labels[index] = double.Parse(match.Value);
                    index++;
                }
            }

            return test_labels;
        }
        public double[] getPredictionLabels(string predictionPath)
        {
            string[] lines = File.ReadAllLines(predictionPath);
            double[] prediction_labels = new double[lines.Length];

            int index = 0;

            foreach(string line in lines)
            {
                prediction_labels[index] = double.Parse(line);
                index++;
            }

            return prediction_labels;
        }        
        public string normalize(string text)
        {
            Normalizer normalize = new Normalizer();
            return normalize.Run(text);
        }
    
        
    }
}
