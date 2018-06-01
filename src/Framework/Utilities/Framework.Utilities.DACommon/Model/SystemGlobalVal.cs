using System;
using System.Data;

//自定义命名空间
using DreamCube.Framework.DataAccess.Basic;

namespace DreamCube.Framework.Utilities.DACommon.Model
{
    public class SystemGlobalVal
    {
        [DbColumn("id", DbType.Int32, IsPrimaryKey = true, IsIdentity = true)]
        public Int32 ID
        { get; set; }

        /// <summary>
        /// 参数的Key值
        /// </summary>
        [DbColumn("syskey", DbType.String)]
        public String SysKey
        { get; set; }

        /// <summary>
        /// 参数的值
        /// </summary>
        [DbColumn("value", DbType.String)]
        public String Value
        { get; set; }
    }
}
