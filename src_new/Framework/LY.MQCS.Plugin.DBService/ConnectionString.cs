using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.MQCS.Plugin.DBService
{
    class ConnectionString
    {
        public String DataSource { get; protected set; }
        public String UserId { get; protected set; }
        public String Password { get; protected set; }

        public ConnectionString(String connectionStr)
        {
            Init(connectionStr);
        }

        public String ToOracle()
        {
            return $"data source={DataSource};user id={UserId};password={Password}";
        }

        /// <summary>
        /// server=MQCSBUS;data source=KFMQCS;user id=MQCSBUS;password=MQCSBUS
        /// </summary>
        /// <param name="connectionStr"></param>
        protected virtual void Init(String connectionStr)
        {
            if (String.IsNullOrEmpty(connectionStr)) return;
            var parts = connectionStr.Split(new String[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < parts.Length; i++)
            {
                var pair = parts[i].Split(new String[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                if (pair.Length != 2) continue;
                var pair1 = pair[0].Trim();
                var pair2 = pair[1].Trim();
                if (pair1.StartsWith("data", StringComparison.CurrentCultureIgnoreCase)
                    && pair1.EndsWith("source", StringComparison.CurrentCultureIgnoreCase))
                {
                    DataSource = pair2;
                }
                else if (pair1.StartsWith("user", StringComparison.CurrentCultureIgnoreCase)
                         && pair1.EndsWith("id", StringComparison.CurrentCultureIgnoreCase))
                {
                    UserId = pair2;
                }
                else if (pair1.StartsWith("password", StringComparison.CurrentCultureIgnoreCase))
                {
                    Password = pair2;
                }
            }
        }
    }
}
