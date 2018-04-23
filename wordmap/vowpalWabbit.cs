using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NHazm;

namespace wordmap
{
    class vowpalWabbit
    {

        public void convertToVW()
        {
            string[] c1 = File.ReadAllLines(@"C:\Users\maryam\Documents\Visual Studio 2015\Projects\wordmap\data\train\sportsTrain.txt");
            string[] c2 = File.ReadAllLines(@"C:\Users\maryam\Documents\Visual Studio 2015\Projects\wordmap\data\train\politicsTrain.txt");
            string path = @"C:\Users\maryam\Documents\Visual Studio 2015\Projects\wordmap\inputVW.txt";


            int count = 2 * Math.Min(c1.Count(), c2.Count());
            Console.WriteLine(c2.Count());
            Console.WriteLine(c1.Count());
            Console.WriteLine(count);

            int c1Index = 0;
            int c2Index = 0;
            string[] contents = new string[count];

            for(int i = 0; i < count; i++)
            {
                int C = i % 2;
                if(C == 0)
                {
                    contents[i] = C.ToString() + " | " + c1[c1Index];
                    c1Index++;
                }
                else
                {
                    contents[i] = C.ToString() + " | " + c2[c2Index];
                    c2Index++;
                }
            }

            File.WriteAllLines(path, contents);
        }

        public string normalize(string text)
        {
            Normalizer normalize = new Normalizer();
            return normalize.Run(text);
        }

    }
}
