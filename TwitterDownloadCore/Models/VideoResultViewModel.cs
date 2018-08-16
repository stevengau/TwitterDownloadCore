using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwitterDownloadCore.Models
{
    public class VideoResultViewModel
    {
        string resolution;
        string hrefAttribute;
        public string HrefAttribute
        {
            get { return hrefAttribute; }
            set
            {
                hrefAttribute = value.ToString().Replace(@"href=""https://", "");
                if (hrefAttribute.ToUpper().Contains(".MP4"))
                    Quality = "MP4";
                if (hrefAttribute.ToUpper().Contains(".MP3"))
                    Quality = "MP3";
            }
        }
        public string Resolution
        {
            get { return resolution; }
            set
            {
                resolution = value.ToString().Replace("<td>", "").Replace("</td>", "");
            }
        }
        public string Quality { get; set; }
    }
}
