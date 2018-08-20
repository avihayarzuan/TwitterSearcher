using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwittyWise
{
    /// <summary>
    /// An interface of Scout class.
    /// The reason for it is that each browser has a different features.
    /// So the code is extended to support different browsers.
    /// </summary>
    interface IScout
    {
        void Login(string url, string userName, string password);
        HashSet<string> GetTweets(string url, int tweetsNum);
        void Close();
    }
}
