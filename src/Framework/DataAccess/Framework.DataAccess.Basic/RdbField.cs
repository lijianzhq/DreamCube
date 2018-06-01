using System;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.OleDb;
using System.Collections;
using System.IO;

using DreamCube.Foundation.Basic.Utility;

namespace DreamCube.Framework.DataAccess.Basic
{
    /// <summary> 
    /// 字段类 
    /// </summary> 
    public class RdbField
    {
        #region "字段"

        private DataRow m_Row = null;
        private DataColumn m_Column = null;
        private RdbRecord m_ParentRecord = null;

        #endregion

        #region "公共属性"

        /// <summary> 
        /// 字段值 
        /// </summary> 
        public object Value
        {
            get
            {
                return this.GetValue();
            }
            set
            {
                this.SetValue(value);
            }
        }

        /// <summary>
        /// 功能: 安全型字段值
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public object SafeValue
        {
            get
            {
                object vValue = this.GetValue();
                Type eType = vValue.GetType();

                if (this.IsNull)
                {
                    if (this.DataType.Equals(typeof(decimal)) || this.DataType.Equals(typeof(int)) || this.DataType.Equals(typeof(double)) || this.DataType.Equals(typeof(float)))
                    {
                        return 0;
                    }
                    if (this.DataType.Equals(typeof(string)) || this.DataType.Equals(typeof(System.DateTime)))
                    {
                        return "";
                    }
                    return null;
                }
                else
                {
                    return vValue;
                }
            }
            set
            {
                this.Value = value;
            }
        }

        /// <summary> 
        /// 字段名称 
        /// </summary> 
        public string Name
        {
            get
            {
                return SchemaColumn.Caption;
            }
        }

        /// <summary> 
        /// 最大长度 
        /// </summary> 
        public int MaxLength
        {
            get
            {
                return SchemaColumn.MaxLength;
            }
        }

        /// <summary> 
        /// 数据类型
        /// </summary> 
        public Type DataType
        {
            get
            {
                return SchemaColumn.DataType;
            }
        }

        /// <summary> 
        /// 列的默认值
        /// </summary>
        public object DefaultValue
        {
            get
            {
                return SchemaColumn.DefaultValue;
            }
        }

        /// <summary>
        /// 是否是主键
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsPrimaryKey
        {
            get
            {
                if (MyString.StrEqual(this.Name, this.m_ParentRecord.PrimaryKey, true))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary> 
        /// 是否是二进制字段 
        /// </summary> 
        public bool IsBinaryField
        {
            get
            {
                if (this.DataType.Equals(Type.GetType("System.Byte[]"))) return true;
                else return false;
            }
        }

        /// <summary>
        /// 判断是不是一个存储数字的字段
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsNumberField
        {
            get
            {
                if (this.DataType.Equals(typeof(decimal)) || this.DataType.Equals(typeof(int)) || this.DataType.Equals(typeof(float)) || this.DataType.Equals(typeof(double)) || this.DataType.Equals(typeof(System.Int64)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 判断本字段是不是一个存储时间日期的字段
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsDateTimeField
        {
            get
            {
                if (this.DataType.Equals(Type.GetType("System.Date")) | this.DataType.Equals(Type.GetType("System.DateTime")))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 判断本字段是不是一个存储字符串的字段
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsStringField
        {
            get
            {
                if (this.DataType.Equals(typeof(string))) return true;
                else return false;
            }
        }

        /// <summary> 
        /// 是否只读 
        /// </summary> 
        public bool ReadOnly
        {
            get
            {
                return SchemaColumn.ReadOnly;
            }
        }

        /// <summary>
        /// 判断Value是否为System.DbNull
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsNull
        {
            get
            {
                if (this.Value.GetType().Equals(typeof(System.DBNull))) return true;
                else return false;
            }
        }

        /// <summary> 
        /// 是否允许为NULL 
        /// </summary> 
        public bool AllowNull
        {
            get
            {
                return SchemaColumn.AllowDBNull;
            }
        }

        #endregion

        #region "私有属性"

        /// <summary> 
        /// 构架信息列 
        /// </summary> 
        private DataColumn SchemaColumn
        {
            get
            {
                try
                {
                    return m_ParentRecord.SchemaTable.Columns[m_Column.Ordinal];
                }
                catch
                {
                    return m_Column;
                }
            }
        }

        #endregion

        #region "公共实例方法"

        public RdbField(RdbRecord oRecord, DataRow oRow, DataColumn oColumn)
        {
            this.m_ParentRecord = oRecord;
            this.m_Row = oRow;
            this.m_Column = oColumn;
        }

        /// <summary>
        /// 功能: 将二进制字段的内容保存为文件
        /// </summary>
        /// <param name="sFilePath"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool ExtractAsFile(string sFilePath)
        {
            try
            {
                byte[] tempValue = (byte[])this.Value;
                MyIO.Write(sFilePath, tempValue);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 功能: 将二进制字段的内容转换成字符串
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public string ExtractAsString()
        {
            try
            {
                MemoryStream oStream = new MemoryStream((byte[])this.Value);
                BinaryReader oBr = new BinaryReader(oStream);
                string sString = oBr.ReadString();
                oStream.Close();
                oBr.Close();
                return sString;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        #endregion

        #region "私有实例方法"

        /// <summary> 
        /// 设置字段值 
        /// </summary> 
        /// <param name="vValue"></param> 
        private void SetValue(object vValue)
        {
            if (vValue is System.DBNull)
            {
                vValue = DBNull.Value;
            }
            else
            {
                if (this.IsStringField)
                {
                    //字符串类型
                    String tempValue = Convert.ToString(vValue);
                    if (string.IsNullOrEmpty(tempValue))
                    {
                        vValue = DBNull.Value;
                    }
                    else
                    {
                        //如果长度超过了的字段的容量,自动截断
                        if (MyString.GetByteLength(tempValue) > this.MaxLength)
                        {
                            vValue = MyString.GetSubStringB(tempValue, this.MaxLength);
                        }
                    }
                }
                else if (this.IsBinaryField)
                {
                    //二进制类型
                    if (vValue.GetType().Equals(typeof(string)))
                    {
                        //将字符串转换成Byte数组
                        vValue = System.Text.Encoding.UTF8.GetBytes(vValue.ToString());
                    }
                }
                else if (this.IsDateTimeField)
                {
                    //日期类型, 将空字符串转换成DBNULL
                    if (MyDatetime.IsValidateDateString(Convert.ToString(vValue)) == false)
                    {
                        vValue = DBNull.Value;
                    }
                }
                else if (this.IsNumberField)
                {
                    //数字类型
                    if (string.IsNullOrEmpty(vValue.ToString()))
                    {
                        vValue = DBNull.Value;
                    }
                    else
                    {
                        vValue = Convert.ToDouble(Convert.ToString(vValue));
                    }
                }
            }
            m_Row[m_Column] = vValue;
        }

        /// <summary> 
        /// 获取字段值 
        /// </summary> 
        /// <returns></returns> 
        private object GetValue()
        {
            object vValue = m_Row[m_Column];
            if (vValue is System.DBNull) return "";
            else return m_Row[m_Column];
        }

        #endregion
    }
}