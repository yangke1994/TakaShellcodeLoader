﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace TakaLoader
{
    // 委托的定义
    public class DELEGATES
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr VirtualAllocRx(
            UInt32 lpStartAddr,
            UInt32 size,
            UInt32 flAllocationType,
            UInt32 flProtect
        );

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr CreateThreadRx(
            UInt32 lpThreadAttributes,
            UInt32 dwStackSize,
            IntPtr lpStartAddress,
            IntPtr param,
            UInt32 dwCreationFlags,
            ref UInt32 lpThreadId
        );

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate UInt32 WaitForSingleObjectRx(IntPtr hHandle, UInt32 dwMilliseconds);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool CheckRemoteDebuggerPresentRx(IntPtr hProcess, ref bool isDebuggerPresent);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr GetModuleHandleRx(string lpModuleName);
    }
}
