using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace TwittyWise
{
    class Program
    {

        static void Main(string[] args)
        {
            // UserName and password to login to twitter
            string userName = "";
            string password = "";
            // You should put your password and user name in the AppConfig
            // It should not go to git due to git ignore
            userName = ConfigurationManager.AppSettings["UserName"];
            password = ConfigurationManager.AppSettings["Password"];

            string loginUrl = "https://twitter.com/login";
            // URL of a twitter page to gain data from
            string trumpUrl = "https://twitter.com/realDonaldTrump";
            // Path to put output files in
            string path = @"C:\Users\Avihay Arzuan\Desktop\Celle\";
            // Number of tweets to take from the page
            int numOfTweets = 100;

            // Creates a Chrome Scout class.
            IScout scout = new ChScout();

            // Optional - log in to the page. Can run without logging
            scout.Login(loginUrl, userName, password);

            // Save all tweets in a Hashset
            HashSet<string> tweets;
            tweets = scout.GetTweets(trumpUrl, numOfTweets);
            scout.Close();

            Analyzer an = new Analyzer(path);
            an.HashTagList(tweets);
            an.MentionList(tweets);

            // Can put here words to exclude from statistics.
            string[] array = { "", "a", "the", "to"};
            List<string> ExcludedWords = new List<string>(array);
            an.MostUsedWords(tweets, 10, ExcludedWords);
        }

    }
}
