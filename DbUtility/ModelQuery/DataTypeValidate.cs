using System;
using System.Reflection;

namespace FireflySoft.DbUtility.ModelQuery
{
    public class DataTypeValidate
    {
        public static bool IsGuid(string str)
        {
            try
            {
                new Guid(str);
                return true;
            }
            catch
            {
            }

            return false;
        }

        public static bool IsInt(string str)
        {
            bool result;
            try
            {
                int.Parse(str);
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public static bool IsPositiveInt(string str)
        {
            bool result = false;
            try
            {
                int val = int.Parse(str);
                if (val > 0)
                {
                    result = true;
                }
            }
            catch
            {
            }

            return result;
        }

        public static bool IsBool(string str)
        {
            bool result;
            try
            {
                bool.Parse(str);
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public static bool IsDateTime(string str)
        {
            bool result;
            try
            {
                DateTime.Parse(str);
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public static bool CheckValid<T>(object obj)
        {
            bool result;
            try
            {
                if (obj.GetType() != typeof(T) && obj.GetType() != typeof(int))
                {
                    result = false;
                }
                else
                {
                    T t = (T)((object)obj);
                    FieldInfo[] fields = typeof(T).GetFields();
                    FieldInfo[] array = fields;
                    for (int i = 0; i < array.Length; i++)
                    {
                        FieldInfo fieldInfo = array[i];
                        if (fieldInfo.Name == t.ToString())
                        {
                            result = true;
                            return result;
                        }
                    }
                    result = false;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }
    }
}
