using System;
using System.Data;

//自定义命名空间
using DreamCube.Framework.DataAccess.Basic;
using DreamCube.Foundation.Basic.Cache;

namespace DreamCube.Framework.Utilities.DACommon
{
    public class SystemGlobalVal : GenericBasicTable<Model.SystemGlobalVal>
    {
        #region "字段"

        /// <summary>
        /// 数据库表名，一般派生类不要轻易改
        /// </summary>
        protected static String tableName = "SystemGlobalVal";

        /// <summary>
        /// 如果项目中存在多个数据库的时候，派生类可以配置此字段值，用于指定此表在哪个数据库中
        /// </summary>
        protected static String dbName = "";

        /// <summary>
        /// 全局的缓冲类
        /// </summary>
        private static DictionaryCachePool<String, String> cache = new DictionaryCachePool<String, String>();

        #endregion

        #region "受保护静态方法"

        /// <summary>
        /// 更新全局参数值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        protected static void UpdateVal(String key, String val)
        {
            String sql = "update {0} set value='{1}' where syskey='{2}'";
            Database db = DbManager.GetDBByName(dbName);
            Int32 result = db.ExecuteNonQuery(String.Format(sql, tableName, val, key));
            //如果更新数据库成功，则更新缓存中的值
            if (result > 0) cache[key] = val;
        }

        /// <summary>
        /// 根据键值获取全局配置的参数值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dbName">数据库名，默认获取第一个数据库</param>
        /// <returns></returns>
        protected static String GetVal(String key)
        {
            if (cache.ContainsKey(key))
                return cache[key];
            else
            {
                String value = "";
                DataTable table = BasicTable.GetEx(SystemGlobalVal.tableName,
                                                    dbName,
                                                    "*",
                                                    String.Format("syskey='{0}'", key),
                                                    "", -1);
                if (table != null && table.Rows.Count > 0)
                    value = Convert.ToString(table.Rows[0]["value"]);
                //把数据缓存起来
                cache.TryAdd(key, value);
                return value;
            }
        }

        #endregion

        #region "构造方法"

        public SystemGlobalVal() { }

        public SystemGlobalVal(Database db)
            : base(db)
        { }

        #endregion

        #region "公共静态方法"

        /// <summary>
        /// 保存到数据库中（如果存在相同的Key值，则替换其值，否则新增其值）
        /// </summary>
        /// <param name="db">数据库实例(不能为NULL)</param>
        /// <param name="sysGlobalVal">全局数据模型对象</param>
        /// <returns></returns>
        public static Boolean SaveToDB(Database db, Model.SystemGlobalVal sysGlobalVal)
        {
            //获取键值所匹配的数据库列名
            String columnName = DbColumnAttribute.GetPropertyMapperDBColumnName<Model.SystemGlobalVal>("SysKey");
            String whereStr = String.Format("{0}='{1}'", columnName, sysGlobalVal.SysKey);
            if (!BasicTable.ExistEx(tableName, whereStr, db))
                return GenericBasicTable<Model.SystemGlobalVal>.Add(sysGlobalVal, tableName, db);
            else return GenericBasicTable<Model.SystemGlobalVal>.UpdateEx(sysGlobalVal, tableName, db, whereStr);
        }

        #endregion

        #region "公共实例方法"

        /// <summary>
        /// 保存到数据库中（如果存在相同的Key值，则替换其值，否则新增其值）
        /// </summary>
        /// <param name="sysGlobalVal">全局数据模型对象</param>
        /// <returns></returns>
        public Boolean SaveToDB(Model.SystemGlobalVal sysGlobalVal)
        {
            return SaveToDB(DB == null ? DbManager.GetDBByName() : DB, sysGlobalVal);
        }

        #endregion

        #region "实例属性"

        public override String TableName
        {
            get { return SystemGlobalVal.tableName; }
        }

        #endregion
    }
}
