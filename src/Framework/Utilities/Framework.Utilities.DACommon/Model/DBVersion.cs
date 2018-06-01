using System;
using System.Data;

//自定义命名空间
using DreamCube.Framework.DataAccess.Basic;

namespace DreamCube.Framework.Utilities.DACommon.Model
{
    public class DBVersion
    {
        /// <summary>
        /// 版本的描述内容
        /// </summary>
        [DbColumn("des", DbType.String)]
        public String Des
        { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        [DbColumn("version", DbType.String)]
        public Single Version
        { get; set; }
    }
}
