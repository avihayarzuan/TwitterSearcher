using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;


namespace TwittyWise
{
    class ChScout : IScout
    {
        IWebDriver driver;

        public ChScout()
        {
            this.driver = new ChromeDriver();
        }

        /// <summary>
        /// Logins the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="userName">User name.</param>
        /// <param name="password">The password.</param>
        public void Login(string url, string userName, string password)
        {
            driver.Url = url;
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Log in'])[2]/following::input[1]")).Click();
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Log in'])[2]/following::input[1]")).Clear();
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Log in'])[2]/following::input[1]")).SendKeys(userName);
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Log in'])[2]/following::input[2]")).Click();
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Log in'])[2]/following::input[2]")).Clear();
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Log in'])[2]/following::input[2]")).SendKeys(password);
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Log in'])[2]/following::button[1]")).Click();
        }

        /// <summary>
        /// Gets the last tweetsNum of tweets.
        /// </summary>
        /// <param name="url">The URL of the page</param>
        /// <param name="tweetsNum">The tweets amount</param>
        /// <returns>All the tweets in a HashSet</returns>
        public HashSet<string> GetTweets(string url, int tweetsNum)
        {
            HashSet<string> tweets = new HashSet<string>();
            // The browser opens the new URL
            driver.Url = url;
            IReadOnlyCollection<IWebElement> twElements;
            // twElements gets all the tweets from the current view of the website
            // Scrolling down is activated if needed.
            twElements = driver.FindElements(By.ClassName("TweetTextSize"));
            if (twElements.Count() < tweetsNum)
            {
                twElements = ScrollDown(tweetsNum);
            }
            // The following lines are removing all the redundant tweets/
            int count = 0;
            foreach (var t in twElements)
            {
                // From element to string
                tweets.Add(t.Text);
                count++;
                if (count > tweetsNum)
                {
                    break;
                }
            }
            return tweets;
        }

        /// <summary>
        /// Scrolls down the page until number of tweets are supplied
        /// </summary>
        /// <param name="tweetsNum">The tweets number.</param>
        /// <returns></returns>
        private IReadOnlyCollection<IWebElement> ScrollDown(int tweetsNum)
        {
            Actions act = new Actions(driver);
            int size = 0;
            while (size < tweetsNum)
            {
                for (int i = 0; i < 10; i++)
                {
                    act.SendKeys(Keys.PageDown).Build().Perform();
                }
                // need time for browser to load
                Thread.Sleep(200);
                size = driver.FindElements(By.ClassName("TweetTextSize")).Count();
            }
            return driver.FindElements(By.ClassName("TweetTextSize"));
        }

        /// <summary>
        /// Closes this driver.
        /// </summary>
        public void Close()
        {
            this.driver.Close();
        }

    }
}
