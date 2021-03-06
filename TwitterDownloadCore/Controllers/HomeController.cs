﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TwitterDownloadCore.Helper;
using TwitterDownloadCore.Models;

namespace TwitterDownloadCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult zh()
        {
            return View("zh");
        }

        public async Task<List<VideoResultViewModel>> TryMore(string url, int counter)
        {
            if (counter == 3) return new List<VideoResultViewModel>();
            List<VideoResultViewModel> viewModel = new List<VideoResultViewModel>();
            using (HttpClient client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("URL", url) });
                client.BaseAddress = new Uri("https://cors-anywhere.herokuapp.com/");
                var result = await client.PostAsync("https://twdown.net/download.php", content);
                string resultContent = await result.Content.ReadAsStringAsync();
                //_logger.LogInformation(resultContent);
                viewModel = StaticHttp.ParserFromTwdown(resultContent);
                if (viewModel.Count == 0)
                {
                    
                    _logger.LogError("Result not Found Error: {1} : {0}", resultContent, DateTime.Now);
                    System.Threading.Thread.Sleep(2000);
                    return TryMore(url, counter++).Result;
                }
            }
            return viewModel;
        }

        //[HttpPost]
        //public async Task<IActionResult> Download(string url)
        //{
        //    List<VideoResultViewModel> viewModel = new List<VideoResultViewModel>();
        //    try
        //    {
        //        var content = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("URL", url) });

        //        using (HttpClient client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri("https://cors-anywhere.herokuapp.com/");
        //            var result = await client.PostAsync("https://twdown.net/download.php", content);
        //            string resultContent = await result.Content.ReadAsStringAsync();
        //            //_logger.LogInformation(resultContent);
        //            viewModel = StaticHttp.ParserFromTwdown(resultContent);
        //            if (viewModel.Count == 0)
        //            {
        //                _logger.LogError("Result not Found Error: {1} : {0}", resultContent, DateTime.Now);
        //                System.Threading.Thread.Sleep(2000);
        //                viewModel = TryMore(url, 0).Result;

        //            }
        //        }


        //        return View("Download", viewModel);
        //    }
        //    catch (Exception)
        //    {
        //        _logger.LogError("Http Error: {1} : {0}", url, DateTime.Now);
        //        return View("Error");
        //    }
        //}


        [HttpGet]
        public async Task<IActionResult> Download(string id)
        {
            List<VideoResultViewModel> viewModel = new List<VideoResultViewModel>();

            var base64EncodedBytes = System.Convert.FromBase64String(id);

            string url = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            try
            {
                var content = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("URL", url) });

                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://cors-anywhere.herokuapp.com/");
                    var result = await client.PostAsync("https://twdown.net/download.php", content);
                    string resultContent = await result.Content.ReadAsStringAsync();
                    //_logger.LogInformation(resultContent);
                    viewModel = StaticHttp.ParserFromTwdown(resultContent);
                    if (viewModel.Count == 0)
                    {
                        _logger.LogError("Result not Found Error: {1} : {0}", resultContent, DateTime.Now);
                        System.Threading.Thread.Sleep(2000);
                        viewModel = TryMore(url, 0).Result;

                    }
                }


                return View("Download", viewModel);
            }
            catch (Exception)
            {
                _logger.LogError("Http Error: {1} : {0}", url, DateTime.Now);
                return View("Error");
            }
        }


        [HttpGet]
        public async Task<IActionResult> zhDownload(string id)
        {
            List<VideoResultViewModel> viewModel = new List<VideoResultViewModel>();
            var base64EncodedBytes = System.Convert.FromBase64String(id);

            string url = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            try
            {
                var content = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("URL", url) });

                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://cors-anywhere.herokuapp.com/");
                    var result = await client.PostAsync("https://twdown.net/download.php", content);
                    string resultContent = await result.Content.ReadAsStringAsync();
                    //_logger.LogInformation(resultContent);
                    viewModel = StaticHttp.ParserFromTwdown(resultContent);
                    if (viewModel.Count == 0)
                    {
                        _logger.LogError("Result not Found Error: {1} : {0}", resultContent, DateTime.Now);
                        System.Threading.Thread.Sleep(2000);
                        viewModel = TryMore(url, 0).Result;
                    }
                        
                }


                return View("zhDownload", viewModel);
            }
            catch (Exception)
            {
                _logger.LogError("Http Error: {1} : {0}", url, DateTime.Now);
                return View("Error");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        [Route("sitemap.xml")]
        public IActionResult SitemapXml()
        {
            String xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";

            xml += "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">";
            xml += "<url>";
            xml += "<loc>http://www.twittervideodownloads.com</loc>";
            xml += "<lastmod>" + DateTime.Now.ToString("yyyy-MM-dd") + "</lastmod>";
            xml += "</url>";
            xml += "<url>";
            xml += "<loc>http://www.twittervideodownloads.com/zh</loc>";
            xml += "<lastmod>" + DateTime.Now.ToString("yyyy-MM-dd") + "</lastmod>";
            xml += "</url>";
            xml += "</urlset>";

            return Content(xml, "text/xml");

        }
    }
}
