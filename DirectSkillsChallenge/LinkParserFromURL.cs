﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DirectSkillsChallenge
{
    public class LinkParserFromURL
    {

        private string IncomingURL { get; set; }
        public LinkParserFromURL(string incomingURL)
        {
            this.IncomingURL = incomingURL;
        }

        public List<string> CaptureLinks()
        {
            List<string> links = new List<string>();

            using (HttpClient webClient = new HttpClient())
            {
                using (HttpResponseMessage webDoc = webClient.GetAsync(IncomingURL).Result)
                {
                    if (webDoc.StatusCode == HttpStatusCode.OK)
                    {
                        using (HttpContent webContent = webDoc.Content)
                        {
                            string webHtml = webContent.ReadAsStringAsync().Result;
                            //Console.WriteLine(webHtml);
                            foreach (Match m in Regex.Matches(webHtml, @"url"))
                            {
                                int firstBracket = webHtml.IndexOf("http://", m.Index);
                                int lastBracket = webHtml.IndexOf("}", m.Index);
                                //Console.WriteLine(string.Format("Link {0} at {1}", m.Value, m.Index));
                                //Console.WriteLine(webHtml.Substring(firstBracket, lastBracket - (firstBracket + 1)));
                                links.Add(webHtml.Substring(firstBracket, lastBracket - (firstBracket + 1)));
                            }
                        }
                    }
                }
            }
            return links;
        }
    }
}
