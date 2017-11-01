using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DreamCube.Foundation.LogService
{
    /// <summary>
    /// The definition of the electronic log configuration item
    /// </summary>
    public static class XdcEJLogKeyItem
    {
        public const string m_strTransBuffer = "TransBuffer";
        public const string m_strFuncCmd = "FuncCmd";

        public const string m_strDeviceWarning = "DeviceWarning";
        public const string m_strStateFlow = "StateFlow";

        public const string m_strDeviceError = "DeviceError";
        public const string m_strMaintenance = "Maintenance";

        public const string m_strATMMode = "ATMMode";
        public const string m_strHostJournal = "HostJournal";

        public const string m_strDispenserDetail = "DispenserDetail";
        public const string m_strCardInsert = "CardInsert";

        public const string m_strCardCapture = "CardCapture";
        public const string m_strDispenseResult = "DispenseResult";

        public const string m_strATMBootup = "ATMBootup";
        public const string m_strHostDispenseData = "HostDispenseData";

        public const string m_strScreen = "Screen";
        public const string m_strOthers = "Others";
    }

    /// <summary>
    /// The definition of the printer data log configuration item
    /// </summary>
    public static class XdcPJLogKeyItem
    {
        public const string m_strATMBootup = "ATMBootup";
        public const string m_strATMMode = "ATMMode";

        public const string m_strDeviceError = "DeviceError";
        public const string m_strDeviceWarning = "DeviceWarning";

        public const string m_strMaintenance = "Maintenance";
        public const string m_strHostJournal = "HostJournal";

        public const string m_strCardCapture = "CardCapture";
        public const string m_strCardInsert = "CardInsert";

        public const string m_strDispenseResult = "DispenseResult";
        public const string m_strDispenseDetail = "DispenseDetail";

        public const string m_strHostDispenseData = "HostDispenseData";
        public const string m_strFuncCmd = "FuncCmd";

        public const string m_strGRGDepositLogFormat = "GRGDepositLogFormat";
        public const string m_strOthers = "Others";
    }

    /// <summary>
    /// Trace log key item 
    /// </summary>
    public static class XdcTraceLogKeyItem
    {
        public const string m_strScreenModule = "Screen";
        public const string m_strException = "Exception";
        public const string m_strMessageParser = "MessageParser";

        public const string m_strXDCBaseElement = "XDCBaseElement";
        public const string m_strFuncCmdModule = "FuncCmdModule";
        public const string m_strEmvModule = "EmvModule";

        public const string m_strAdaModule = "AdaModule";
        public const string m_strRklModule = "RklModule";
        public const string m_strCashAcceptorCashIn = "CashAcceptorCashIn";
        public const string m_strOthers = "Others";
    }

    /// <summary>
    /// The dictionary container for EJ log configuration
    /// </summary>
    public class XdcLogKeyDictionary
    {
        #region constructor
         /// <summary>
        /// Define the instance of the current object
        /// </summary>
        private static XdcLogKeyDictionary s_objXdcLogKeyDictionary = null;

        /// <summary>
        ///  static construct function
        /// </summary>
        static XdcLogKeyDictionary()
        {
            s_objXdcLogKeyDictionary = new XdcLogKeyDictionary();           
        }

        /// <summary>
        /// Get the instance of the XDC log key dictionary object
        /// </summary>
        /// <returns></returns>
        public static XdcLogKeyDictionary Instance
        {
            get
            {
                return s_objXdcLogKeyDictionary;
            }
        }
#endregion

        /// <summary>
        /// Write log by given configuration item defined in XDC configuration file
        /// </summary>
        /// <returns>Continue write log or not</returns>
        public bool bXdcWriteLogByGivenConfig(string argLogKeyName)
        {
            bool bReturn = true;

            if (argLogKeyName.Length <= 0)
            {
                return bReturn;
            }

            if (m_iProtocolType <= 0)
            {
                return bReturn;
            }

            if (m_xdclogKeyDictionary.Count <= 0)
            {
                return bReturn;
            }

            if (m_xdclogKeyDictionary.ContainsKey(argLogKeyName))
            {
                int iValue = m_xdclogKeyDictionary[argLogKeyName];

                if (iValue <= 0)
                {
                    bReturn = false;
                    return bReturn;
                }
            }

            return bReturn;
        }

        /// <summary>
        /// Load setting with EJ configuration items
        /// </summary>
        /// <param name="argDictEJLogItem">The dictionary container to set </param>
        public void LoadSetting(Dictionary<string, int> argDictEJLogItem) 
        {
            m_xdclogKeyDictionary.Clear();

            foreach (var item in argDictEJLogItem)
            {
                m_xdclogKeyDictionary.Add(item.Key, item.Value);
            }

            return;
        }

        /// <summary>
        /// Protocol type setting
        /// </summary>
        public int ProtocolType
        {
            get
            {
                return m_iProtocolType;
            }
            set
            {
                 m_iProtocolType = value;
            }
        }

#region field
        /// <summary>
        /// The definition of the log key dictionary container
        /// </summary>
        private  Dictionary<string, int> m_xdclogKeyDictionary = new Dictionary<string, int>();

        /// <summary>
        /// The definition of the protocol type
        /// 0 - ISO8583, other value - NDC/DC
        /// </summary>
        private int m_iProtocolType = 0;
#endregion
    } 
}
