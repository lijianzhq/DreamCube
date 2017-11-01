/********************************************************************
	FileName:   ConsoleLog
    purpose:	

	author:		huang wei
	created:	2013/01/24

    revised history:
	2013/01/24  

================================================================
    Copyright (C) 2013, Grgbanking CO,. Ltd. All rights reserved.
================================================================
********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace DreamCube.Foundation.LogService
{
    public static class Win32Api
    {
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();
    }

    public class ConsoleLog
    {
#region constructor
        static ConsoleLog()
        {
            Win32Api.AllocConsole();
            s_logBuilder = new StringBuilder(32);
            s_logLocker = new object();
        }
#endregion

#region method
        public static void LogXDCCommInfo( byte[] arrBuffer,
                                           int    argSize,
                                           int    argHeadSize,
                                           bool   argIsSend )
        {
            Debug.Assert(null != arrBuffer && argSize >= argHeadSize);            
            lock ( s_logLocker )
            {
                DateTime nowTime = DateTime.Now;
                string time = string.Format(s_XDCCommDateTimeFormat, nowTime.Hour, nowTime.Minute, nowTime.Second, nowTime.Millisecond);
                s_logBuilder.AppendFormat(s_packageHeaderFormat,
                                           time,
                                           argIsSend ? s_sendSymbol : s_recvSymbol,
                                           argSize);
                if ( argHeadSize > 0 )
                {
                    s_logBuilder.Append("Head ");
                    for (int i = 0; i < argHeadSize; ++i)
                    {
                        s_logBuilder.AppendFormat("[0x{0:x2}] ", arrBuffer[i]);
                    }
                    s_logBuilder.Append(":\r\n");
                }
                else
                {
                    s_logBuilder.Append("Head :\r\n");
                }
                //log msg
                int total = argSize - argHeadSize;
                if ( total > 0 )
                {
                    s_logBuilder.Append(' ', time.Length + 1);
                    char temp = ' ';
                    for (int i = argHeadSize; i < argSize; ++i)
                    {
                        temp = Convert.ToChar(arrBuffer[i]);
                        if ( Char.IsLetterOrDigit(temp) )
                        {
                            s_logBuilder.AppendFormat("{0}", temp);
                        }
                        else
                        {
                            s_logBuilder.Append( Convert.ToChar( temp ) );
                        }
                        //else if ( Char.IsWhiteSpace( temp ) )
                        //{
                        //    s_logBuilder.Append( temp );
                        //}
                        //else
                        //{
                        //    s_logBuilder.AppendFormat("[0x{0:x2}]", arrBuffer[i]);
                        //}
                    }
                }

                LogMsg(s_logBuilder.ToString());
                s_logBuilder.Clear();
            }
        }

        public static void LogMsg( string argMsg )
        {
            Debug.Assert(!string.IsNullOrEmpty(argMsg));
            Console.WriteLine(argMsg);
        }

        public static void LogMsgFormat( string argFormat,
                                         params object[] argParams )
        {
            Debug.Assert(!string.IsNullOrEmpty(argFormat));
            LogMsg(string.Format(argFormat, argParams));
        }
#endregion

#region field
        public const string s_sendSymbol = "Send:";

        public const string s_recvSymbol = "Receive:";

        public const string s_XDCCommDateTimeFormat = "{0:D2}:{1:D2}:{2:D3}.{3}";

        public const string s_packageHeaderFormat = "{0} {1} Package Len={2},";

        public static StringBuilder s_logBuilder = null;

        private static object s_logLocker = null;
#endregion
    }
}
