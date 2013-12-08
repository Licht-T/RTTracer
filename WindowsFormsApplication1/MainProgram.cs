using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TweetSharp;

namespace TestProgram
{
    class MainProgram
    {

        private const int WAITTIME = 1000;

        private MainWindow mainWindow;
        private LoginForm loginForm;

        private SearchTweets searchTweets;

        public MainProgram(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            this.searchTweets = new SearchTweets();
            loginForm = new LoginForm(finalizeAuth);
        }

        public void loginCheck()
        {
            this.searchTweets.login();

            if (searchTweets.LoginOk)
            {
                initialize();
            }
            else
            {
                startAuth();
            }
        }

        public void startAuth()
        {
            this.searchTweets.startAuthSequence();
            openVerifierWindow();
        }

        public void finalizeAuth(string code)
        {
            this.searchTweets.verifierProcess(code);
            if(searchTweets.LoginOk)
            {
                initialize();
                loginForm.Close();
            }
            else
            {
                MessageBox.Show("Invalid code.",
                                "エラー",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        public void initialize()
        {
            Console.WriteLine("Initialized!");
        }

        public void openVerifierWindow()
        {
            loginForm.ShowDialog(mainWindow);
        }

        public void createGraph(long tweetId)
        {
            GraphCreator gc = new GraphCreator(createGraphData(tweetId));
        }

        public GraphData createGraphData(long tweetId)
        {
            Dictionary<long, List<long>> dict = new Dictionary<long, List<long>>();

            long ownerId = searchTweets.getTweetOwnerId(tweetId);
            dict[ownerId] = searchTweets.getFollowerList(ownerId);
            System.Threading.Thread.Sleep(WAITTIME);

            IEnumerable<TwitterStatus> rt_ids = searchTweets.getRT(tweetId);
            foreach (TwitterStatus status in rt_ids)
            {
                dict[status.User.Id] = searchTweets.getFollowerList(status.User.Id);
                System.Threading.Thread.Sleep(WAITTIME);
            }

            GraphData graph;
            graph = new GraphData(ownerId, new Dictionary<long, List<long>>());

            foreach (long id in dict.Keys)
            {
                foreach (long follower in dict[id])
                {
                    if (dict.ContainsKey(follower))
                    {
                        if (!graph.Reatonships.ContainsKey(follower))
                        {
                            graph.Reatonships[follower] = new List<long>();
                        }
                        graph.Reatonships[follower].Add(id);
                    }
                }
            }
            return graph;
        }
    }
}
