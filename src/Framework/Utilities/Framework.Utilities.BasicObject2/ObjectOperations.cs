using System;
using DreamCube.Foundation.Basic.Utility;

public class ObjectOperations
{
    /// <summary>
    /// 把数组 aObjectX 各个对象指定列的内容连接起来
    /// </summary>
    /// <param name="aObjectX">ObjectX 数组</param>
    /// <param name="sColumn">列名</param>
    /// <param name="sConnectStr">连接字符串</param>
    /// <param name="bFilterSameColumn">是否要过滤内容相同的列（即如果两个对象的列内容如果相同，则只返回其中一个）</param>
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
    /// 把ObjectX对象数组中，指定列求和
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