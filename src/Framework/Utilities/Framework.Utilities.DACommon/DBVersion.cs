using System;
using System.Data;

//自定义命名空间
using DreamCube.Framework.DataAccess.Basic;
using DreamCube.Foundation.Basic.Cache;

namespace DreamCube.Framework.Utilities.DACommon
{
    public class DBVersion : GenericBasicTable<Model.DBVersion>
    {
        #region "字段"

        /// <summary>
        /// 数据库表名，一般派生类不要轻易改
        /// </summary>
        protected static String tableName = "DBVersion";

        /// <summary>
        /// 如果项目中存在多个数据库的时候，派生类可以配置此字段值，用于指定此表在哪个数据库中
        /// </summary>
        protected static String dbName = "";

        #endregion

        #region "属性"

        public override String TableName
        {
            get { return tableName; }
        }

        #endregion
    }
}
