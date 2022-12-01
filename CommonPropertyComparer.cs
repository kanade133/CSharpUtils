using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpUtils
{
    class CommonPropertyComparer<T> : IComparer<T>
    {
        /// <summary>
        /// 0:反射形式 1:委托形式
        /// </summary>
        private int m_mode;
        private List<PropertyOffset> m_listPropertyOffset = new List<PropertyOffset>();
        private List<FunctionOffset> m_listFunctionOffset = new List<FunctionOffset>();

        /// <summary>
        /// 传入属性名和排序方式
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="isAsc"></param>
        public CommonPropertyComparer(string propertyName, bool isAsc)
        {
            var po = new PropertyOffset()
            {
                PropertyInfo = typeof(T).GetProperty(propertyName),
                Offset = isAsc ? 1 : -1,
            };
            m_listPropertyOffset.Add(po);
            m_mode = 0;
        }
        /// <summary>
        /// 传入属性名和排序方式的分组
        /// </summary>
        /// <param name="dicPropertyNameIsAsc"></param>
        public CommonPropertyComparer(params KeyValuePair<string, bool>[] dicPropertyNameIsAsc)
        {
            foreach (var kv in dicPropertyNameIsAsc)
            {
                var po = new PropertyOffset()
                {
                    PropertyInfo = typeof(T).GetProperty(kv.Key),
                    Offset = kv.Value ? 1 : -1,
                };
                m_listPropertyOffset.Add(po);
            }
            m_mode = 0;
        }
        /// <summary>
        /// 传入取值器和排序方式
        /// </summary>
        /// <param name="valueSelector"></param>
        /// <param name="isAsc"></param>
        public CommonPropertyComparer(Func<T, object> valueSelector, bool isAsc)
        {
            var fo = new FunctionOffset()
            {
                ValueSelector = valueSelector,
                Offset = isAsc ? 1 : -1,
            };
            m_listFunctionOffset.Add(fo);
            m_mode = 1;
        }
        /// <summary>
        /// 传入取值器和排序方式的分组
        /// </summary>
        /// <param name="dicValueSelectorIsAsc"></param>
        public CommonPropertyComparer(params KeyValuePair<Func<T, object>, bool>[] dicValueSelectorIsAsc)
        {
            foreach (var kv in dicValueSelectorIsAsc)
            {
                var fo = new FunctionOffset()
                {
                    ValueSelector = kv.Key,
                    Offset = kv.Value ? 1 : -1,
                };
                m_listFunctionOffset.Add(fo);
            }
            m_mode = 1;
        }

        /// <summary>
        /// 重写的比对方法
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(T x, T y)
        {
            if (x == null)
            {
                return 1;
            }
            else if (y == null)
            {
                return -1;
            }
            switch (m_mode)
            {
                case 0: return PropertyCompare(x, y);
                case 1: return FunctionCompare(x, y);
                default: return 0;
            }
        }
        /// <summary>
        /// 反射比对
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private int PropertyCompare(T x, T y)
        {
            int result = 0;
            foreach (var po in m_listPropertyOffset)
            {
                dynamic value1 = po.PropertyInfo.GetValue(x);
                dynamic value2 = po.PropertyInfo.GetValue(y);
                if (value1 == null)
                {
                    result = 1 * po.Offset;
                }
                else if (value2 == null)
                {
                    result = -1 * po.Offset;
                }
                else if (value1 == value2)
                {
                    result = 0;
                }
                else
                {
                    result = value1.CompareTo(value2) * po.Offset;
                }
                if (result != 0)
                {
                    break;
                }
            }
            return result;
        }
        /// <summary>
        /// 委托比对
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private int FunctionCompare(T x, T y)
        {
            int result = 0;
            foreach (var fo in m_listFunctionOffset)
            {
                dynamic value1 = fo.ValueSelector(x);
                dynamic value2 = fo.ValueSelector(y);
                if (value1 == null)
                {
                    result = 1 * fo.Offset;
                }
                else if (value2 == null)
                {
                    result = -1 * fo.Offset;
                }
                else if (value1 == value2)
                {
                    result = 0;
                }
                else
                {
                    result = value1.CompareTo(value2) * fo.Offset;
                }
                if (result != 0)
                {
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 自定义反射比对用存储
        /// </summary>
        private struct PropertyOffset
        {
            public System.Reflection.PropertyInfo PropertyInfo;
            public int Offset;
        }
        /// <summary>
        /// 自定义委托比对用存储
        /// </summary>
        private struct FunctionOffset
        {
            public Func<T, object> ValueSelector;
            public int Offset;
        }
    }
}
