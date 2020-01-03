using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoPlayerForWpf.Models;

namespace VideoPlayerForWpf.Service
{

    /// <summary>
    /// 视频列表服务
    /// </summary>
    public class VideoListService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public RootVideo GetVideoList()
        {
            try
            {
                var result = HttpClient.PostAsync("video/getVideoLiveList", string.Empty);
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<RootVideo>(result.Html);
                }

                return null;
            }
            catch (Exception ex)
            {
                App.logger.Error("获取视频列表失败", ex);
                return null;
            }
        }
    }
}
