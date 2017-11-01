using System;
using System.Collections;
using System.Collections.Generic;

namespace DreamCube.Foundation.Basic.Utility
{
    public static class MyArrayList
    {
        /// <summary>
        /// 把ArrayList的值复制到List集合中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static T[] CopyToArray<T>(ArrayList input)
        {
            List<T> result = CopyToList<T>(input);
            return result.ToArray();
        }

        /// <summary>
        /// 把ArrayList的值复制到List集合中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static List<T> CopyToList<T>(ArrayList input)
        {
            List<T> result = new List<T>();
            String type = typeof(T).Name.ToString();
            switch (type)
            {
                case "String":
                    List<String> tempValue1 = result as List<String>;
                    foreach (String item in input)
                        tempValue1.Add(item);
                    break;
                case "Int32":
                    List<Int32> tempValue2 = result as List<Int32>;
                    foreach (Int32 item in input)
                        tempValue2.Add(item);
                    break;
            }
            return result as List<T>;
        }
    }
}
