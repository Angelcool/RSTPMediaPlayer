using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using videoPlayer.eunm;

namespace videoPlayer.Util
{
    class SysInfo
    {
        [DllImport("kernel32")]
        public static extern void GlobalMemoryStatus(ref MEMORY_INFO meminfo);
        //定义内存的信息结构  
        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORY_INFO
        {
            public uint dwLength;
            public uint dwMemoryLoad;
            public uint dwTotalPhys;
            public uint dwAvailPhys;
            public uint dwTotalPageFile;
            public uint dwAvailPageFile;
            public uint dwTotalVirtual;
            public uint dwAvailVirtual;
        }

        private static Random ran = new Random();

        public static float getMemoryStatus()
        {
            MEMORY_INFO MemInfo;
            MemInfo = new MEMORY_INFO();
            GlobalMemoryStatus(ref MemInfo);
            return MemInfo.dwMemoryLoad;
        }
        ///获取cpu使用率
        public static float ReGetCpuInfo() 
        {
            float cpu = 0;
            try
            {
                PerformanceCounter cpucounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                
                while (cpu == 0)
                {
                    cpu = cpucounter.NextValue();
                    Thread.Sleep(500);
                }
            }
            catch
            {}

            return cpu;
        }

        public static void GetNetUsage(out decimal netUsageret)
        {
            NetworkInterface[] nicArr = NetworkInterface.GetAllNetworkInterfaces();
            double lblBytesReceived = 0;
            double lblUpload = 0;
            netUsageret = 0;
            for (int i = 0; i < nicArr.Length; i++)
            {
                NetworkInterface nic = nicArr[i];
                if (nic.OperationalStatus != OperationalStatus.Up) continue;

                IPv4InterfaceStatistics interfaceStats = nic.GetIPv4Statistics();

                decimal upload = (decimal)(interfaceStats.BytesSent - lblUpload) / 1024;
                decimal dowload = (decimal)(interfaceStats.BytesReceived - lblBytesReceived) / 1024;
                lblBytesReceived = interfaceStats.BytesReceived;
                lblUpload = interfaceStats.BytesSent;
                var netCSpeed = nic.Speed;
                decimal netUsage = (dowload + upload) * 1024 / (netCSpeed / 8) * 100;

                Thread.Sleep(1000);

                interfaceStats = nic.GetIPv4Statistics();
                upload = (decimal)(interfaceStats.BytesSent - lblUpload) / 1024;
                dowload = (decimal)(interfaceStats.BytesReceived - lblBytesReceived) / 1024;
                lblBytesReceived = interfaceStats.BytesReceived;
                lblUpload = interfaceStats.BytesSent;
                netCSpeed = nic.Speed;
                netUsage = (dowload + upload) * 1024 / (netCSpeed / 8) * 100;
                if (netUsage > 0 && netUsage <= 100)
                {
                    netUsageret = netUsage;
                    break;
                }
                //Console.WriteLine("test1=======网卡速率=====" + netCSpeed);
                //Console.WriteLine("test1======下载======" + dowload);
                //Console.WriteLine("test1======上传======" + upload);
                //Console.WriteLine("test1=======使用率=====" + netUsage);
            }
            
        }
        

        /// <summary>
        /// 查询端口是否被占用
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool PortInUse(int port)
        {
            bool inUse = false;

            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();

            foreach (IPEndPoint endPoint in ipEndPoints)
            {
                if (endPoint.Port == port)
                {
                    inUse = true;
                    break;
                }
            }
            return inUse;
        }

        public static int getIdlePort(int port) {
            //避免同时启动程序启用相同的端口
            port += ran.Next(1, 100);
            if (!PortInUse(port))
            {
                return port;
            }
            else {
                return getIdlePort(port);
            }
        }

        public static int getPortByPattern(WinPatternEnums winPattern)
        {
            //避免同时启动程序启用相同的端口
            if (WinPatternEnums.PREVIEW.Equals(winPattern))
            {
                return 2333;
            }
            else if (WinPatternEnums.SPLIT.Equals(winPattern))
            {
                return 2334;
            }
            else if (WinPatternEnums.CRUISE.Equals(winPattern))
            {
                return 2335;
            }
            else if (WinPatternEnums.CALL.Equals(winPattern))
            {
                return 2336;
            }
            else if (WinPatternEnums.PREVIEW_VERTICAL.Equals(winPattern))
            {
                return 2337;
            }
            else
            {
                return 0;
            }

        }
    }
}
