using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using NSoup;
using System.IO;
using Newtonsoft;
using NSoup.Nodes;
using NSoup.Select;
using Newtonsoft.Json;

namespace BlogSpider
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("爬取开始");
            GetZhaojieRawBlog();
            Console.WriteLine("爬取完成");
            Console.ReadKey();

        }


        public static void GetZhaojieNewBlog()
        {
            //zhaojie 博客地址为 http://blog.zhaojie.me/
            //共31页

            List<BlogInfo> blogList = new List<BlogInfo>();

            for (int page = 1; page <= 31; page++)
            {

                //下载html
                string url = "http://blog.zhaojie.me/?page=" + page;
                WebClient web = new WebClient();
                byte[] buffer = web.DownloadData(url);
                string html = Encoding.UTF8.GetString(buffer);

                //解析html
                NSoup.Nodes.Document doc = NSoup.NSoupClient.Parse(html);
                Element rootEle = doc.GetElementById("wrapper");
                Elements firstEle = rootEle.GetElementsByClass("post");

                foreach (var item in firstEle)
                {
                    Element firstItem = item;
                    Elements firstItemChildren = firstItem.Children;

                    //取出第1个和第3个子节点
                    Element firstChild = firstItemChildren[0];
                    Element aChild = firstChild.Children.First();
                    string childTitle = aChild.Text();
                    string childHref = aChild.Attr("href");

                    Element thirdChild = firstItemChildren[2];
                    string intro = thirdChild.TextNodes[1].Text();


                    //存储相关信息
                    blogList.Add(new BlogInfo() { Title = childTitle, Link = childHref, BriefIntro = intro });


                }

                string content = JsonConvert.SerializeObject(blogList);
                string path = AppDomain.CurrentDomain.BaseDirectory + page + ".txt";
                File.AppendAllText(path, content, Encoding.UTF8);

            }

        }

        public static void GetZhaojieRawBlog()
        {
            //zhaojie 博客地址为 http://www.cnblogs.com/JeffreyZhao/
            //共16页

            List<BlogInfo> blogList = new List<BlogInfo>();

            for (int page = 1; page <= 16; page++)
            {
               
                //下载html
                string url = "http://www.cnblogs.com/JeffreyZhao/default.aspx?page=" + page;
                WebClient web = new WebClient();
                byte[] buffer = web.DownloadData(url);
                string html = Encoding.UTF8.GetString(buffer);

                //解析html
                NSoup.Nodes.Document doc = NSoup.NSoupClient.Parse(html);
                Element rootEle = doc.GetElementById("wrapper");
                Element firstEle = rootEle.Children.First();
                Elements postList = firstEle.GetElementsByClass("PostTitle");


                for (int i = 0; i < postList.Count; i++)
                {

                    var aChild = postList[i];
                    string childTitle = aChild.Text();
                    string childHref = aChild.Attr("href");
                    var introEle = aChild.Parent.SiblingElements[1].Children.FirstOrDefault();
                    string intro = introEle == null ? string.Empty : introEle.TextNodes[0].Attributes["text"];

                    //存储相关信息
                    blogList.Add(new BlogInfo() { Title = childTitle, Link = childHref, BriefIntro = intro });
                }



                string content = JsonConvert.SerializeObject(blogList);
                string path = AppDomain.CurrentDomain.BaseDirectory + "\\Raw\\" + page + ".txt";
                File.AppendAllText(path, content, Encoding.UTF8);

            }
        }
    }


    public class BlogInfo
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string BriefIntro { get; set; }
    }
}
