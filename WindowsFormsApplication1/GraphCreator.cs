using GraphVizWrapper;
using GraphVizWrapper.Commands;
using GraphVizWrapper.Queries;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProgram
{
    class GraphCreator
    {
        private GraphData graphData;

        public GraphCreator(GraphData graphData)
        {
            this.graphData = graphData;
            drawGraph(this.graphData);
        }

        private void drawGraph(GraphData graphData)
        {
            string dot = createDot(graphData);

            Console.WriteLine(dot);

            var getStartProcessQuery = new GetStartProcessQuery();
            var getProcessStartInfoQuery = new GetProcessStartInfoQuery();
            var registerLayoutPluginCommand = 
                new RegisterLayoutPluginCommand(getProcessStartInfoQuery, getStartProcessQuery);
            var wrapper = new GraphGeneration(getStartProcessQuery, 
                                              getProcessStartInfoQuery, 
                                              registerLayoutPluginCommand);

            byte[] output 
                = wrapper.GenerateGraph(dot, 
                                        Enums.GraphReturnType.Png);

            ImageConverter imgconv = new ImageConverter();
            Image img = (Image)imgconv.ConvertFrom(output);
            GraphViewer v = new GraphViewer(graphData.OwnerId.ToString(),new Bitmap(img));

            v.Show();
        }

        private string createDot(GraphData graphData)
        {
            StringBuilder sb = new StringBuilder();
            using(StringWriter w = new StringWriter(sb))
            {
                Dictionary<long, List<long>> dict = graphData.Reatonships;
                w.WriteLine("digraph sample {");
                w.WriteLine("node [style=filled  colorscheme=piyg9];");
                w.WriteLine("\"{0}\"[color=1];", graphData.OwnerId);
                foreach (long id in dict.Keys)
                {
                    foreach (long follower in dict[id])
                    {
                        if (dict.ContainsKey(follower))
                        {
                            w.WriteLine("{0} -> {1};", follower, id);
                        }
                    }
                }
                w.WriteLine("}");
            }
            return sb.ToString();
        }
    }
}
