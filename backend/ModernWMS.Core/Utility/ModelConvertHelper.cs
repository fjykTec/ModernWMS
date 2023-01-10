
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace ModernWMS.Core.Utility
{
    /// <summary>
    /// dataTable convert to model
    /// </summary>
    /// <typeparam name="T">model</typeparam>
    public static class ModelConvertHelper<T> where T : new()
    {
        /// <summary>
        ///  dataTable convert to model list
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <returns></returns>
        public static List<T> ConvertToModel(DataTable dt)
        {
            List<T> ts = new List<T>();

            Type type = typeof(T);
            string tempName = "";

            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                string colvalue = "";
                try
                {
                    PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;
                    if (dt.Columns.Contains(tempName))
                    {
                        if (!pi.CanWrite)
                        {
                            continue;
                        }
                        string TypeName = pi.PropertyType.FullName;
                        string value = dr[tempName] == DBNull.Value ? "" : Convert.ToString(dr[tempName]);
                        colvalue = value;
                        if (pi.PropertyType.Name.ToLower().Contains("datetime"))
                        {
                            pi.SetValue(t, string.IsNullOrEmpty(value) ? Convert.ToDateTime("1900-01-01") : Convert.ChangeType(value, pi.PropertyType), null);
                        }
                        else if (pi.PropertyType.Name.ToLower().Contains("double"))
                        {
                            if (string.IsNullOrEmpty(value))
                            {
                                pi.SetValue(t, 0M);
                            }
                            else
                            {
                                pi.SetValue(t, string.IsNullOrEmpty(value) ? Convert.ToDouble(0) : Convert.ChangeType(value, pi.PropertyType), null);
                            }
                        }
                        else if (pi.PropertyType.Name.ToLower().Contains("decimal"))
                        {
                            if (string.IsNullOrEmpty(value))
                            {
                                pi.SetValue(t, 0M);
                            }
                            else
                            {
                                pi.SetValue(t, string.IsNullOrEmpty(value) ? Convert.ToDecimal(0) : Convert.ChangeType(value, pi.PropertyType), null);
                            }
                        }
                        else if (pi.PropertyType.Name.ToLower().Contains("int"))
                        {
                            pi.SetValue(t, string.IsNullOrEmpty(value) ? 0 : Convert.ChangeType(value, pi.PropertyType), null);
                        }
                        else if (pi.PropertyType.Name.ToLower().Contains("bool"))
                        {
                            pi.SetValue(t, string.IsNullOrEmpty(value) ? false : Convert.ChangeType(value, pi.PropertyType), null);
                        }
                        else if (pi.PropertyType.Name.ToLower().Contains("string"))
                        {
                            pi.SetValue(t, string.IsNullOrEmpty(value) ? "" : Convert.ChangeType(value, pi.PropertyType), null);
                        }
                        else if (!pi.PropertyType.IsGenericType)
                        {
                            pi.SetValue(t, string.IsNullOrEmpty(value) ? null : Convert.ChangeType(value, pi.PropertyType), null);
                        }
                        else
                        {
                            Type genericTypeDefinition = pi.PropertyType.GetGenericTypeDefinition();
                            if (genericTypeDefinition == typeof(Nullable<>))
                            {
                                pi.SetValue(t, string.IsNullOrEmpty(value) ? null : Convert.ChangeType(value, Nullable.GetUnderlyingType(pi.PropertyType)), null);
                            }
                        }
                    }
                }
                ts.Add(t);
                }
                catch (Exception  ) { }
            }
            return ts;
        }

    }
}
