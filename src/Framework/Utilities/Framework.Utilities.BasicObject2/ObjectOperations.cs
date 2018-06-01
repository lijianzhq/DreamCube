using System;
using DreamCube.Foundation.Basic.Utility;

public class ObjectOperations
{
    /// <summary>
    /// ������ aObjectX ��������ָ���е�������������
    /// </summary>
    /// <param name="aObjectX">ObjectX ����</param>
    /// <param name="sColumn">����</param>
    /// <param name="sConnectStr">�����ַ���</param>
    /// <param name="bFilterSameColumn">�Ƿ�Ҫ����������ͬ���У��������������������������ͬ����ֻ��������һ����</param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static string ConnectObjectColumn(ObjectX[] aObjectX, string sColumn, string sConnectStr, bool bFilterSameColumn = false)
    {
        string sReturn = string.Empty;
        string sVal = string.Empty;
        if (aObjectX != null)
        {
            foreach (ObjectX oObject in aObjectX)
            {
                sVal = Convert.ToString(oObject.GetItemValue(sColumn));
                if (string.IsNullOrEmpty(sVal) == false)
                {
                    if (!(bFilterSameColumn && MyString.ContainsEx(sReturn, sVal, sConnectStr, true)))
                    {
                        sReturn = MyString.Connect(sReturn, sVal, sConnectStr);
                    }
                }
            }
        }
        return sReturn;
    }

    /// <summary>
    /// ��ObjectX���������У�ָ�������
    /// </summary>
    /// <param name="aObjectX"></param>
    /// <param name="sColumn"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static double SumObjectColumn(ObjectX[] aObjectX, string sColumn)
    {
        double dReturn = 0;
        double dVal = 0;
        if (aObjectX != null)
        {
            foreach (ObjectX oObject in aObjectX)
            {
                dVal = Convert.ToDouble(Convert.ToString(oObject.GetItemValue(sColumn)));
                dReturn = dReturn + dVal;
            }
        }
        return dReturn;
    }
}