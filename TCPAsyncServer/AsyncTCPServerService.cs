using log4net;
using Logger;
using System;
using System.Net;
using Newtonsoft.Json;
using TCPAsyncServer.model;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace TCPAsyncServer
{
    public class AsyncTCPServerService
    {
        private static ILog logger = LogHelper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        AsyncTcpServer server;

        /// <summary>
        /// 客户端连接状态
        /// </summary>
        public bool isConnector;

        public delegate void hasdisConnectEventHandler();
        //public delegate void hasConnectEventHandler();
        public delegate void callbackEventHandler(string groupId);
        /// <summary>
        /// 开始播放
        /// </summary>
        /// <param name="messageModel"></param>
        public delegate void startPlayVideoEventHandler(MessageModel messageModel);
        /// <summary>
        /// 停止播放
        /// </summary>
        /// <param name="memberNo"></param>
        public delegate void stopPlayVideoEventHandler(string memberNo);
        public delegate void collectStatEventHandler(string memberNo,bool stat);//收藏状态

        public delegate void killMyselfEventHandler();
        public delegate void queryMemberEventHandler(string memberNo,string  data,string memberMessageType);//查询终端是否存在
        public delegate void showMainFormEventHandler(MessageModel messageModel);//显示窗体
        public delegate void showPromptEventHandler(string prompt);//显示提示信息


        //public event hasConnectEventHandler hasConnectEvent;
        public event hasdisConnectEventHandler hasdisConnectEvent;
        /// <summary>
        /// //测试专用
        /// </summary>
        public event callbackEventHandler callbackEvent; 
        /// <summary>
        /// //PC端通知开始播放
        /// </summary>
        public event startPlayVideoEventHandler startPlayVideoEvent; 
        /// <summary>
        /// //PC端通知结束播放
        /// </summary>
        public event stopPlayVideoEventHandler stopPlayVideoEvent;   
        /// <summary>
        /// //收藏状态
        /// </summary>
        public event collectStatEventHandler collectStatEvent;
        /// <summary>
        /// pc端通知关闭程序
        /// </summary>
        public event killMyselfEventHandler killMyselfEvent;
        /// <summary>
        /// 查询终端是否存在
        /// </summary>
        public event queryMemberEventHandler queryMemberEvent;
        /// <summary>
        /// 显示主窗体
        /// </summary>
        public event showMainFormEventHandler showMainFormEvent;
        /// <summary>
        /// 显示提示信息
        /// </summary>
        public event showPromptEventHandler showPromptEvent;
        /// <summary>
        /// 启动TCPAsyncServer
        /// </summary>
        public void serverStart(int port)
        {
            var ipAddress = ConfigurationManager.AppSettings["Address"];
            IPAddress ip = IPAddress.Parse(ipAddress);
            server = new AsyncTcpServer(ip, port);
            server.ClientConnected += new EventHandler<TcpClientConnectedEventArgs>(server_ClientConnected);
            server.ClientDisconnected += new EventHandler<TcpClientDisconnectedEventArgs>(server_ClientDisconnected);
            server.PlaintextReceived += new EventHandler<TcpDatagramReceivedEventArgs<string>>(server_PlaintextReceived);
            server.Start();
            logger.Info("TCP服务已启动");
        }
        public void serverDispose()
        {
            notifyClose();
            server.Dispose();
        }
        /// <summary>
        /// 转发给信令voiceReader的返回结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void server_PlaintextReceived(object sender, TcpDatagramReceivedEventArgs<string> e)
        {
            string vRes = e.Datagram;

            logger.Info("TCP收到消息：" + vRes);

            dislist.Clear();
            disInfo(vRes);
            
            try
            {
                foreach (string message in dislist)
                {

                    MessageModel messageModel = JsonConvert.DeserializeObject<MessageModel>(message);
                    if (messageModel.videoMessageType == "start") //开始播放
                    {
                        startPlayVideoEvent.Invoke(messageModel);
                    }
                    if (messageModel.videoMessageType == "videomsg") //开始播放
                    {
                        startPlayVideoEvent.Invoke(messageModel);
                    }
                    if (messageModel.videoMessageType == "stop")
                    {
                        stopPlayVideoEvent.Invoke(messageModel.memberNo); //结束播放
                    }
                    if (messageModel.videoMessageType == "collectStat")   //收藏状态
                    {
                        collectStatEvent.Invoke(messageModel.memberNo, messageModel.collectStat);
                    }
                    if (messageModel.videoMessageType == "kill")
                    {
                        killMyselfEvent.Invoke();
                    }
                    if (messageModel.videoMessageType == "queryMember")   //查询终端是否存在
                    {
                        queryMemberEvent.Invoke(messageModel.memberNo,messageModel.data,messageModel.memberMessageType);
                    }
                    if (messageModel.videoMessageType == "show")   //显示窗体
                    {
                        showMainFormEvent.Invoke(messageModel);
                    }
                    if (messageModel.videoMessageType == "prompt")   //显示窗体
                    {
                        showPromptEvent.Invoke(messageModel.prompt);
                        //todo 弹出消息提示
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Debug("数据处理异常");
                logger.Error(ex);
                //sendMessage("数据有误");
            }
            

        }
        List<object> dislist = new List<object>();
        private void disInfo(string vRes)
        {
            if (vRes.Contains("}{"))
            {
                var ret = vRes.IndexOf("}{");
                dislist.Add(vRes.Substring(0, ret + 1));
                var s2 = vRes.Substring(ret + 1);
                if (s2.Contains("}{"))
                {
                    disInfo(s2);
                }
                else
                {
                    dislist.Add(s2);
                }

            }
            else
            {
                dislist.Add(vRes);
            }
        }

        private void server_ClientDisconnected(object sender, TcpClientDisconnectedEventArgs e)
        {
            logger.Info("客户端断开！");
            isConnector = false;

            hasdisConnectEvent.Invoke();
        }

        private void server_ClientConnected(object sender, TcpClientConnectedEventArgs e)
        {
            isConnector = true;
            logger.Info("客户端连入");
        }

        /// <summary>
        /// 通知关闭
        /// </summary>
        private void notifyClose() {
            Dictionary<string, object> parameter = new Dictionary<string, object>();
            parameter.Add("no", "[]");
            parameter.Add("type", "stop");
            string content = JsonConvert.SerializeObject(parameter);

            sendMessage(content);//关闭所有
        }

        /// <summary>
        /// 主动挂断
        /// </summary>
        /// <param name="memberNo"></param>
        public void notifyHangupVideo(string memberNo, bool isCamera)
        {
            Dictionary<string, object> parameter = new Dictionary<string, object>();
            parameter.Add("no", memberNo);
            parameter.Add("type", "stop");
            parameter.Add("isCamera", isCamera);
            string content = JsonConvert.SerializeObject(parameter);
            sendMessage(content);
        }

        public void notifyHangupVideo(VideoPlayModel videoData)
        {
            Dictionary<string, object> parameter = new Dictionary<string, object>();
            parameter.Add("memberNo", videoData.liveMemberNo);
            parameter.Add("videoMessageType", "stop");
            parameter.Add("videoData",videoData);
            string content = JsonConvert.SerializeObject(parameter);
            sendMessage(content);
        }

        /// <summary>
        /// 包含子节点时
        /// </summary>
        /// <param name="memberNo"></param>
        /// <param name="isCamera"></param>
        /// <param name="subordinateLiveMessage"></param>
        public void notifyHangupVideoChild(string memberNo, bool isCamera, WatchSubordinateLiveMessageBean subordinateLiveMessage) 
        {
            Dictionary<string, object> parameter = new Dictionary<string, object>();
            parameter.Add("no", memberNo);
            parameter.Add("type", "stopchild");
            parameter.Add("isCamera", isCamera);
            parameter.Add("watchSubordinateLiveMessageBean", subordinateLiveMessage);
            
            string content = JsonConvert.SerializeObject(parameter);
            sendMessage(content);
        }

        /// <summary>
        /// 收藏
        /// </summary>
        /// <param name="memberNo"></param>
        public void notifyCollect(string memberNo, int isCamera,string collectName)
        {
            Dictionary<string, object> parameter = new Dictionary<string, object>();
            parameter.Add("no", memberNo);
            parameter.Add("type", "collect");
            parameter.Add("isCamera", isCamera);
            parameter.Add("collectName", collectName);
            string content = JsonConvert.SerializeObject(parameter);
            sendMessage(content);
        }
        /// <summary>
        /// 取消收藏
        /// </summary>
        /// <param name="memberNo"></param>
        public void notifyUnCollect(string memberNo, int isCamera)
        {
            Dictionary<string, object> parameter = new Dictionary<string, object>();
            parameter.Add("no", memberNo);
            parameter.Add("type", "unCollect");
            parameter.Add("isCamera", isCamera);
            string content = JsonConvert.SerializeObject(parameter);
            sendMessage(content);
        }
        /// <summary>
        /// 推送
        /// </summary>
        /// <param name="memberNo"></param>
        public void notifyPush(string memberNo, bool isCamera, string rtsp, string callId, string streamType, WatchSubordinateLiveMessageBean subordinateLiveMessageBean)
        {
            Dictionary<string, object> parameter = new Dictionary<string, object>();
            parameter.Add("memberNo", memberNo);
            parameter.Add("videoMessageType", "openPush");
            Dictionary<string, object> videoData = new Dictionary<string, object>();
            videoData.Add("isCamera", 0);
            videoData.Add("callId", callId);
            videoData.Add("streamType", streamType);
            parameter.Add("videoData", videoData);
            if (isCamera)
            {
                parameter.Add("rtspUrl",rtsp);
                parameter.Add("callId", callId);
            }
            else
            {
                parameter.Add("watchSubordinateLiveMessageBean", subordinateLiveMessageBean);
            }
            string content = JsonConvert.SerializeObject(parameter);
            sendMessage(content);
        }

        //分屏推送
        public void notifyPush(VideoPlayModel videoData)
        {
            if (videoData == null)
            {
                return;
            }

            Dictionary<string, object> parameter = new Dictionary<string, object>();
            parameter.Add("memberNo", videoData.liveMemberNo);
            parameter.Add("uniqueNo", videoData.liveMemberUniqueNo);
            parameter.Add("videoMessageType", "openPush");
            parameter.Add("videoData", videoData);

            string content = JsonConvert.SerializeObject(parameter);
            sendMessage(content);
        }

        //推送到分屏
        public void notifyPush2Split(VideoPlayModel videoData)
        {
            if (videoData == null)
            {
                return;
            }

            Dictionary<string, object> parameter = new Dictionary<string, object>();
            parameter.Add("memberNo", videoData.liveMemberNo);
            parameter.Add("uniqueNo", videoData.liveMemberUniqueNo);
            parameter.Add("videoMessageType", "push2Split");
            parameter.Add("videoData", videoData);

            string content = JsonConvert.SerializeObject(parameter);
            sendMessage(content);
        }

        //主窗体推送
        public void notifyPush(List<VideoPlayModel> videoPlays)
        {
            if (videoPlays == null || videoPlays.Count == 0) {
                return;
            }

            VideoPlayModel videoData = videoPlays[0];

            Dictionary<string, object> parameter = new Dictionary<string, object>();
            parameter.Add("memberNo", videoData.liveMemberNo);
            parameter.Add("uniqueNo", videoData.liveMemberUniqueNo);
            parameter.Add("videoMessageType", "pushMany");
            parameter.Add("videoDataList", videoPlays);

            string content = JsonConvert.SerializeObject(parameter);
            sendMessage(content);
        }

        //选中
        public void notifySelect(MessageModel message) {
            string content = JsonConvert.SerializeObject(message);
            sendMessage(content);
        }

        public void notifyCameraSwich(string memberNo)
        {
            Dictionary<string, string> parameter = new Dictionary<string, string>();
            parameter.Add("type", "cameraSwitch");
            parameter.Add("memberNo", memberNo);
            string content = JsonConvert.SerializeObject(parameter);
            sendMessage(content);
        }

        /// <summary>
        /// 返回播放状态
        /// </summary>
        /// <param name="memberNo"></param>
        public void notifyPlayResult(string data, string memberMessageType, bool videoWindow)
        {
            Dictionary<string, object> parameter = new Dictionary<string, object>();
            parameter.Add("type", "allowResult");
            parameter.Add("data", data);
            parameter.Add("memberMessageType", memberMessageType);
            parameter.Add("isExist", videoWindow);

            string content = JsonConvert.SerializeObject(parameter);
            sendMessage(content);
        }

        public void notifyErrMsg(string memberNo, string msg)
        {
            Dictionary<string, string> parameter = new Dictionary<string, string>();
            parameter.Add("type", "errorMsg");
            parameter.Add("memberNo", memberNo);
            parameter.Add("errorMsg", msg);
            string content = JsonConvert.SerializeObject(parameter);
            sendMessage(content);
        }

        /// <summary>
        /// 给所客户端发送通知
        /// </summary>
        /// <param name="parameter"></param>
        public void sendMessage(string parameter)
        {
            if (!isConnector) {
                logger.Warn("客户端已经断开连接！！！");
                return;
            }

            logger.Info("发送给PC———>:" + parameter);
            server.SendAll(parameter);
        }
    }
}
