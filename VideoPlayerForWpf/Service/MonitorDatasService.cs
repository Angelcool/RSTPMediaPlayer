using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VideoPlayerForWpf.Models;

namespace VideoPlayerForWpf.Service
{
    public class MonitorDatasService
    {

        public IList<MonitorEntity> LoadMonitorDatas()
        {
            var path = Path.Combine(Environment.CurrentDirectory, "Resources", "navs.xml");

            if (!File.Exists(path))
            {
                return new List<MonitorEntity>();
            }

            return (from c in XDocument.Load(path).Descendants("item")
                    select new MonitorEntity
                    {
                        RowIndex = int.Parse(c.Attribute("num").Value),
                        Name = c.Attribute("name").Value,
                        Code = c.Attribute("code").Value,
                        Address = c.Attribute("area").Value
                    }).ToList();
        }
    }
}
