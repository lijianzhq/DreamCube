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
    /// �ֶ��� 
    /// </summary> 
    public class RdbField
    {
        #region "�ֶ�"

        private DataRow m_Row = null;
        private DataColumn m_Column = null;
        private RdbRecord m_ParentRecord = null;

        #endregion

        #region "��������"

        /// <summary> 
        /// �ֶ�ֵ 
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
        /// ����: ��ȫ���ֶ�ֵ
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
        /// �ֶ����� 
        /// </summary> 
        public string Name
        {
            get
            {
                return SchemaColumn.Caption;
            }
        }

        /// <summary> 
        /// ��󳤶� 
        /// </summary> 
        public int MaxLength
        {
            get
            {
                return SchemaColumn.MaxLength;
            }
        }

        /// <summary> 
        /// ��������
        /// </summary> 
        public Type DataType
        {
            get
            {
                return SchemaColumn.DataType;
            }
        }

        /// <summary> 
        /// �е�Ĭ��ֵ
        /// </summary>
        public object DefaultValue
        {
            get
            {
                return SchemaColumn.DefaultValue;
            }
        }

        /// <summary>
        /// �Ƿ�������
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
        /// �Ƿ��Ƕ������ֶ� 
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
        /// �ж��ǲ���һ���洢���ֵ��ֶ�
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
        /// �жϱ��ֶ��ǲ���һ���洢ʱ�����ڵ��ֶ�
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
        /// �жϱ��ֶ��ǲ���һ���洢�ַ������ֶ�
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
        /// �Ƿ�ֻ�� 
        /// </summary> 
        public bool ReadOnly
        {
            get
            {
                return SchemaColumn.ReadOnly;
            }
        }

        /// <summary>
        /// �ж�Value�Ƿ�ΪSystem.DbNull
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
        /// �Ƿ�����ΪNULL 
        /// </summary> 
        public bool AllowNull
        {
            get
            {
                return SchemaColumn.AllowDBNull;
            }
        }

        #endregion

        #region "˽������"

        /// <summary> 
        /// ������Ϣ�� 
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

        #region "����ʵ������"

        public RdbField(RdbRecord oRecord, DataRow oRow, DataColumn oColumn)
        {
            this.m_ParentRecord = oRecord;
            this.m_Row = oRow;
            this.m_Column = oColumn;
        }

        /// <summary>
        /// ����: ���������ֶε����ݱ���Ϊ�ļ�
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
        /// ����: ���������ֶε�����ת�����ַ���
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

        #region "˽��ʵ������"

        /// <summary> 
        /// �����ֶ�ֵ 
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
                    //�ַ�������
                    String tempValue = Convert.ToString(vValue);
                    if (string.IsNullOrEmpty(tempValue))
                    {
                        vValue = DBNull.Value;
                    }
                    else
                    {
                        //������ȳ����˵��ֶε�����,�Զ��ض�
                        if (MyString.GetByteLength(tempValue) > this.MaxLength)
                        {
                            vValue = MyString.GetSubStringB(tempValue, this.MaxLength);
                        }
                    }
                }
                else if (this.IsBinaryField)
                {
                    //����������
                    if (vValue.GetType().Equals(typeof(string)))
                    {
                        //���ַ���ת����Byte����
                        vValue = System.Text.Encoding.UTF8.GetBytes(vValue.ToString());
                    }
                }
                else if (this.IsDateTimeField)
                {
                    //��������, �����ַ���ת����DBNULL
                    if (MyDatetime.IsValidateDateString(Convert.ToString(vValue)) == false)
                    {
                        vValue = DBNull.Value;
                    }
                }
                else if (this.IsNumberField)
                {
                    //��������
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
        /// ��ȡ�ֶ�ֵ 
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