using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetSharp;
using System.Windows.Forms;

namespace TestProgram
{

    class SearchTweets
    {
        private const string con_key = "";
        private const string con_sec = "";
        private const string settingsFile = "settings.config";
        private TwitterSettings twitterSettings;
        private TwitterService service;
        private OAuthRequestToken requestToken;

        private bool loginOk;

        public bool LoginOk
        {
            get { return loginOk; }
        }

        public SearchTweets()
        {
            loginOk = false;

            //Service Initialization.
            this.service = new TwitterService(con_key, con_sec);
        }

        public void login()
        {
            //Check user data.
            if (System.IO.File.Exists(settingsFile))
            {
                System.Xml.Serialization.XmlSerializer serializer2 =
                    new System.Xml.Serialization.XmlSerializer(typeof(TwitterSettings));
                System.IO.FileStream fs2 =
                    new System.IO.FileStream(settingsFile, System.IO.FileMode.Open);
                this.twitterSettings =
                    (TwitterSettings)serializer2.Deserialize(fs2);
                fs2.Close();

                this.authProcess();
            }
        }

        public void startAuthSequence()
        {
            this.loginOk = false;
            // Step 1 - Retrieve an OAuth Request Token
            this.requestToken = service.GetRequestToken();

            // Step 2 - Redirect to the OAuth Authorization URL
            Uri uri = service.GetAuthorizationUri(requestToken);
            System.Diagnostics.Process.Start(uri.ToString());
        }

        public void verifierProcess(string verifier) 
        {
            OAuthAccessToken access = service.GetAccessToken(requestToken, verifier);

            if( !(access.Token=="?") &&
                !(access.TokenSecret=="?") )
            {
                //Create settings. 
                this.twitterSettings = new TwitterSettings(access.Token, access.TokenSecret);

                //Save user data.
                System.Xml.Serialization.XmlSerializer serializer1 =
                    new System.Xml.Serialization.XmlSerializer(typeof(TwitterSettings));
                System.IO.FileStream fs1 =
                    new System.IO.FileStream(settingsFile, System.IO.FileMode.Create);
                serializer1.Serialize(fs1, twitterSettings);
                fs1.Close();

                this.authProcess();
            }

        }

        public long getTweetOwnerId(long tweetId)
        {
            return service.GetTweet(new GetTweetOptions { Id = tweetId, TrimUser = true }).User.Id;
        }

        public IEnumerable<TwitterStatus> getRT(long id)
        {
            IEnumerable<TwitterStatus> statusArray
                = service.Retweets(new RetweetsOptions { Id = id, Count = 100, TrimUser = true });
            
            return statusArray;
        }

        public TwitterFriendship checkFriendship(string source, string target)
        {
            IAsyncResult res = 
                service.BeginGetFriendshipInfo(new GetFriendshipInfoOptions { SourceId = source, TargetId = target });
            TwitterFriendship friendship = service.EndGetFriendshipInfo(res);
            return friendship;
        }

        public List<long> getFollowerList(long id)
        {
            List<long> ids = new List<long>();

            IAsyncResult res =
                service.BeginListFollowerIdsOf(new ListFollowerIdsOfOptions { UserId = id, Count = 5000 });
            TwitterCursorList<long> list = service.EndListFollowerIdsOf(res);

            foreach(long user in list)
            {
                ids.Add(user);
            }

            while (list.NextCursor != 0)
            {
                res =
                    service.BeginListFollowerIdsOf(
                    new ListFollowerIdsOfOptions
                    {
                        UserId = id,
                        Count = 5000,
                        Cursor = list.NextCursor
                    });
                list = service.EndListFollowerIdsOf(res);

                foreach (long user in list)
                {
                    ids.Add(user);
                }
            }
            ids.Sort();
            return ids;
        }


        public List<long> getFriendsList(long id)
        {
            List<long> ids = new List<long>();

            IAsyncResult res =
                service.BeginListFriendIdsOf(new ListFriendIdsOfOptions { UserId = id, Count = 5000 });
            TwitterCursorList<long> list = service.EndListFollowerIdsOf(res);

            foreach (long user in list)
            {
                ids.Add(user);
            }

            while (list.NextCursor != 0)
            {
                res =
                    service.BeginListFollowerIdsOf(
                    new ListFollowerIdsOfOptions
                    {
                        UserId = id,
                        Count = 5000,
                        Cursor = list.NextCursor
                    });
                list = service.EndListFollowerIdsOf(res);

                foreach (long user in list)
                {
                    ids.Add(user);
                }
            }
            ids.Sort();
            return ids;
        }

        private void authProcess()
        {
            service.AuthenticateWith(twitterSettings.AccsToken, twitterSettings.AccsSec);
            this.loginOk = true;
        }
    }
}
