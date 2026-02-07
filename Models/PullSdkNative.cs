using System;
using System.Runtime.InteropServices;

namespace PullSDKDataListener.UI.Models
{
    public static class PullSdkNative
    {
        private const string Dll = "plcommpro.dll";

        [DllImport(Dll, EntryPoint = "Connect")]
        public static extern IntPtr Connect(string parameters);

        [DllImport(Dll, EntryPoint = "Disconnect")]
        public static extern void Disconnect(IntPtr h);

        [DllImport(Dll, EntryPoint = "PullLastError")]
        public static extern int PullLastError();

        [DllImport(Dll, EntryPoint = "GetRTLog")]
        public static extern int GetRTLog(IntPtr h, ref byte buffer, int buffersize);

        [DllImport(Dll, EntryPoint = "SetDeviceData")]
        public static extern int SetDeviceData(IntPtr h, string tablename, string data, string options);

        [DllImport(Dll, EntryPoint = "DeleteDeviceData")]
        public static extern int DeleteDeviceData(IntPtr h, string tablename, string data, string options);

        [DllImport(Dll, EntryPoint = "GetDeviceData")]
        public static extern int GetDeviceData(IntPtr h, ref byte buffer, int buffersize, string tablename, string fieldnames, string filter, string options);
    }
}
