#if !NET20
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mini.Foundation.Basic.Utility
{
    /// <summary>
    /// 读取json字符串的属性，注意：只读取最外层的属性
    /// 例如:{aa:'12',bb:{name:'lj',show:function(){}}}，最终只会读取到两个键值对，一个是aa，一个是bb
    /// </summary>
    public class JsonPropertyReader
    {
        Int32 _pos = 0;
        String _value = String.Empty;
        static Char[] _spaceChar = new Char[] { Char.MinValue, '\\', ' ', '\r', '\n', '\t' };

        public JsonPropertyReader(String jsonStr)
        {
            _value = jsonStr;
        }

        public virtual Dictionary<String, String> Read()
        {
            var dic = new Dictionary<String, String>();
            //消耗掉前面的空格
            PassEmptyChar();
            //首先找到了括号的开始位置（去掉空白字符）
            while (_value[_pos++] != '{') ;
            do
            {
                PassEmptyChar();
                //如果是对象结束符，则退出循环了
                if (_value[_pos] == '}')
                    break;
                String key = ReadKey();
                //找到冒号的位置（去掉key与冒号之间的空白字符）
                while (_value[_pos++] != ':') ;
                //去掉冒号与右边的字符
                PassEmptyChar();
                //读取右边的值
                String value = ReadValue();
                dic.Add(key, value);
                //读取下一个非逗号的字符
                while (_pos < _value.Length && _value[_pos++] != ',') ;
            } while (_pos < _value.Length);
            return dic;
        }

        protected virtual void PassEmptyChar()
        {
            //消耗掉前面的空格
            while (_spaceChar.Contains(_value[_pos]))
                _pos++;
        }

        /// <summary>
        /// 读取key
        /// </summary>
        /// <returns></returns>
        protected virtual String ReadKey()
        {
            var sb = new StringBuilder();
            char endChar = char.MinValue;//结束标识符
            if (_value[_pos] == '"')
            {
                _pos++;
                endChar = '"';
            }
            else if (_value[_pos] == '\'')
            {
                _pos++;
                endChar = '\'';
            }
            do
            {
                sb.Append(_value[_pos]);
                _pos++;
            } while (_value[_pos] != endChar && _value[_pos] != ':');//不为结束符，并且不为冒号，继续读取作为key值的一部分
            return sb.ToString();
        }

        protected virtual String ReadValue()
        {
            var sb = new StringBuilder();
            var stack = new Stack<Char>();
            stack.Push(',');//结束符号，当这个符号出栈，并且栈元素为0，则代表value值读完了
            do
            {
                char c = _value[_pos++];
                if (c == '"')//发现冒号
                {
                    if (stack.Peek() == '"')//如果栈顶也是"，则直接返回字符串内容，读取完毕
                    {
                        stack.Pop();//符号出栈
                        if (stack.Count() == 0)
                            return sb.ToString();
                    }
                    else
                    {
                        stack.Push('"');
                        goto add;
                    }
                }
                else if (c == '\'')//发现'
                {
                    if (stack.Peek() == '\'')//如果栈顶也是'，则直接返回字符串内容，读取完毕
                    {
                        stack.Pop();//符号出栈
                        if (stack.Count() == 0)
                            return sb.ToString();
                    }
                    else
                    {
                        stack.Push('\'');
                        goto add;
                    }
                }
                else if (c == '[' || c == '{' || c == '(')//发现双括号的左边部分
                {
                    stack.Push(c);
                    goto add;
                }
                else if (c == ']' || c == '}' || c == ')')//发现双括号的右边部分
                {
                    stack.Pop();//符号出栈
                    if (stack.Count() == 0)
                        return sb.ToString();
                }
                else if (c == ',')
                {
                    if (stack.Peek() == ',')//如果栈顶也是逗号，则直接返回字符串内容，读取完毕
                    {
                        stack.Pop();//符号出栈
                        _pos--;//回退到逗号位置
                        if (stack.Count() == 0)
                            return sb.ToString();
                    }
                }
                else
                {
                    goto add;
                }
                add: sb.Append(c);
            } while (_pos < _value.Length);
            return String.Empty;
        }
    }
}

#endif