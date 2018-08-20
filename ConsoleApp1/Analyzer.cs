using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TwittyWise
{
    class Analyzer
    {
        readonly string path;

        public Analyzer(string path)
        {
            this.path = path;
        }

        public void HashTagList(HashSet<string> set)
        {
            SaveList("HashtagList", set, @"(?<=#)\w+");
        }

        public void MentionList(HashSet<string> set)
        {
            SaveList("MentionList", set, @"(?<=@)\w+");
        }

        /// <summary>
        /// Saves the list in file.
        /// </summary>
        /// <param name="fileName">Name of the file to create.</param>
        /// <param name="set">The set of tweets</param>
        /// <param name="reg">The regex to search for</param>
        private void SaveList(string fileName, HashSet<string> set, string reg)
        {
            string[] arr = { fileName + ":" };
            List<string> list = new List<string>(arr);
            foreach (var s in set)
            {
                var regex = new Regex(reg);
                var matches = regex.Matches(s);
                foreach (Match m in matches)
                {
                    list.Add(m.Value);
                }
            }
            System.IO.File.WriteAllLines(path + fileName + ".txt", list);

            //Optional prints the data to screen:
            //list.ForEach(Console.WriteLine);
        }

        /// <summary>
        /// The method gives the Most used words in the hashset.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <param name="wordNum">The word number of most used words.</param>
        /// <param name="ExcludedWords">The excluded words list.</param>
        public void MostUsedWords(HashSet<string> set, int wordNum, List<string> ExcludedWords)
        {
            // Using a dictionary to count each word- the value is the amount of occurences.
            Dictionary<string, int> dic = new Dictionary<string, int>();
            string AllText = string.Join(" ", set.ToArray());
            var fixedInput = Regex.Replace(AllText, "[^a-zA-Z0-9% ._]", string.Empty);
            var splitted = fixedInput.Split(' ');

            foreach (string s in splitted)
            {
                if (dic.Keys.Contains(s))
                {
                    dic[s] = dic[s] + 1;
                }
                else
                {
                    dic.Add(s, 1);
                }
            }
            // Check that the amount of required words is less than the dictinary size.
            if (dic.Count() < wordNum )
            {
                wordNum = dic.Count();
            }
            // Remove's from dictionary all words appeared in the Excluded list
            foreach(string s in ExcludedWords)
            {
                if(dic.Keys.Contains(s))
                {
                    dic.Remove(s);
                }
            }

            List<string> list = new List<string>();
            string line  = "Most Used Words:";
            list.Add(line);
            for (int i = 0; i < wordNum; i++)
            {
                var word = dic.OrderByDescending(x => x.Value).FirstOrDefault().Key;
                list.Add((i+1).ToString() + ". " + word + ", " + dic[word] + " Occurrences");
                dic.Remove(word);
            }

            System.IO.File.WriteAllLines(path + "MostUsedWords.txt", list);
            //Optional prints the data to screen:
            //list.ForEach(Console.WriteLine);
        }
    }
}
