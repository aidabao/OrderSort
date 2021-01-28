using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace OrderSort.Common
{
    /// <summary>
    /// 类型转换工具类
    /// 创建人:niu
    /// 创建日期:2017.03.14
    /// </summary>
    public class TypeConverter
    {
        /// <summary>
        /// string型转换为bool型 
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(object expression, bool defValue)
        {
            if (expression != null)
                return StrToBool(expression.ToString(), defValue);

            return defValue;
        }

        /// <summary>
        /// string型转换为bool型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(string expression, bool defValue)
        {

            if (expression != null)
            {
                if (string.Compare(expression, "true", true) == 0)
                {
                    //System.Web.HttpContext.Current.Response.Write(string.Compare(expression, "true", true));
                    return true;
                }
                else if (string.Compare(expression, "false", true) == 0)
                    return false;
            }
            return defValue;
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int ObjectToInt(object expression)
        {
            return ObjectToInt(expression, 0);
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int ObjectToInt(object expression, int defValue)
        {
            if (expression != null)
                return StrToInt(expression.ToString(), defValue);

            return defValue;
        }

        /// <summary>
        /// 将对象转换为Int32类型,转换失败返回0
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(string str)
        {
            return StrToInt(str, 0);
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(string str, int defValue)
        {
            if (string.IsNullOrEmpty(str) || str.Trim().Length >= 12 || !Regex.IsMatch(str.Trim(), @"^([-]|[0-9])[0-9]*(\.\w*)?$"))
                return defValue;

            int rv;
            if (Int32.TryParse(str, out rv))
                return rv;

            return Convert.ToInt32(StrToFloat(str, defValue));
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(object strValue, float defValue)
        {
            if ((strValue == null))
                return defValue;

            return StrToFloat(strValue.ToString(), defValue);
        }

        /// <summary>
        /// string型转换为double型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static double StrToDouble(object strValue, double defValue)
        {
            if ((strValue == null))
                return defValue;

            return StrToDouble(strValue.ToString(), defValue);
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float ObjectToFloat(object strValue, float defValue)
        {
            if ((strValue == null))
                return defValue;

            return StrToFloat(strValue.ToString(), defValue);
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float ObjectToFloat(object strValue)
        {
            return ObjectToFloat(strValue.ToString(), 0);
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(object strValue)
        {
            if ((strValue == null))
                return 0;

            return StrToFloat(strValue.ToString(), 0);
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(string strValue, float defValue)
        {
            if ((strValue == null) || (strValue.Length > 16))
                return defValue;

            float intValue = defValue;
            if (strValue != null)
            {
                bool IsFloat = Regex.IsMatch(strValue, @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                if (IsFloat)
                    float.TryParse(strValue, out intValue);
            }
            return intValue;
        }

        /// <summary>
        /// string型转换为double型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的double类型结果</returns>
        public static double StrToDouble(string strValue, double defValue)
        {
            if ((strValue == null) || (strValue.Length > 16))
                return defValue;

            double intValue = defValue;
            if (strValue != null)
            {
                bool IsFloat = Regex.IsMatch(strValue, @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                if (IsFloat)
                    double.TryParse(strValue, out intValue);
            }
            return intValue;
        }


        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static decimal StrToDecimal(object strValue, decimal defValue)
        {
            if ((strValue == null))
                return defValue;

            return StrToDecimal(strValue.ToString(), defValue);
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static decimal ObjectToDecimal(object strValue, decimal defValue)
        {
            if ((strValue == null))
                return defValue;

            return StrToDecimal(strValue.ToString(), defValue);
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static decimal ObjectToDecimal(object strValue)
        {
            return ObjectToDecimal(strValue.ToString(), 0);
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <returns>转换后的int类型结果</returns>
        public static decimal StrToDecimal(object strValue)
        {
            if ((strValue == null))
                return 0;

            return StrToDecimal(strValue.ToString(), 0);
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static decimal StrToDecimal(string strValue, decimal defValue)
        {
            if ((strValue == null))
                return defValue;
            decimal intValue = defValue;
            if (strValue != null)
            {
                bool IsFloat = Regex.IsMatch(strValue, @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                if (IsFloat)
                    decimal.TryParse(strValue, out intValue);
            }
            return intValue;
        }

        /// <summary> 
        /// DataSet装换为泛型集合 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="p_DataSet">DataSet</param> 
        /// <param name="p_TableIndex">待转换数据表索引</param> 
        /// <returns></returns> 
        public static List<T> DataSetToList<T>(DataSet p_DataSet, int p_TableIndex)
        {
            if (p_DataSet == null || p_DataSet.Tables.Count < 0)
                return null;
            if (p_TableIndex > p_DataSet.Tables.Count - 1)
                return null;
            if (p_TableIndex < 0)
                p_TableIndex = 0;

            DataTable p_Data = p_DataSet.Tables[p_TableIndex];
            // 返回值初始化 
            List<T> result = new List<T>();
            for (int i = 0; i < p_Data.Rows.Count; i++)
            {
                /// Activator.CreateInstrance C#在类工厂中动态创建类的实例
                T _t = (T)Activator.CreateInstance(typeof(T));
                PropertyInfo[] propertys = _t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    for (int j = 0; j < p_Data.Columns.Count; j++)
                    {
                        /// 属性与字段名称一致的进行赋值 
                        if (pi.Name.Equals(p_Data.Columns[j].ColumnName))
                        {
                            /// 数据库NULL值单独处理 
                            if (p_Data.Rows[i][j] != DBNull.Value)
                                /// SetValue 将给定对象的属性值设置为给定值。
                                pi.SetValue(_t, Convert.ChangeType(p_Data.Rows[i][j], pi.PropertyType), null);
                            else
                                pi.SetValue(_t, null, null);
                            break;
                        }
                    }
                }
                result.Add(_t);
            }
            return result;
        }
        /// <summary> 
        /// DataSet装换为泛型集合 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="p_DataSet">DataSet</param> 
        /// <param name="p_TableName">待转换数据表名称</param> 
        /// <returns></returns> 
        public static List<T> DataSetToList<T>(DataSet p_DataSet, string p_TableName)
        {
            int _TableIndex = 0;
            if (p_DataSet == null || p_DataSet.Tables.Count < 0)
                return null;
            if (string.IsNullOrEmpty(p_TableName))
                return null;
            for (int i = 0; i < p_DataSet.Tables.Count; i++)
            {
                /// 获取Table名称在Tables集合中的索引值 
                if (p_DataSet.Tables[i].TableName.Equals(p_TableName))
                {
                    _TableIndex = i;
                    break;
                }
            }
            return DataSetToList<T>(p_DataSet, _TableIndex);
        }

        /// <summary>
        /// 泛型集合转DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static DataTable ListToDt<T>(IEnumerable<T> collection)
        {
            var props = typeof(T).GetProperties();
            var dt = new DataTable();
            dt.Columns.AddRange(props.Select(p => new
            DataColumn(p.Name, p.PropertyType)).ToArray());
            if (collection.Count() > 0)
            {
                for (int i = 0; i < collection.Count(); i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in props)
                    {
                        object obj = pi.GetValue(collection.ElementAt(i), null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    dt.LoadDataRow(array, true);
                }
            }
            return dt;
        }
        /// <summary>
        /// 将DataRow赋值给model中同名属性
        /// </summary>
        /// <typeparam name="T">泛型：model的类型</typeparam>
        /// <param name="objmodel">model实例</param>
        /// <param name="dtRow">DataTable行数据</param>
        public static T TableRowToModel<T>(T objmodel, DataRow dtRow)
        {

            //获取model的类型
            Type modelType = typeof(T);
            //获取model中的属性
            PropertyInfo[] modelpropertys = modelType.GetProperties();
            //遍历model每一个属性并赋值DataRow对应的列
            foreach (PropertyInfo pi in modelpropertys)
            {
                //获取属性名称
                String name = pi.Name;
                if (dtRow.Table.Columns.Contains(name))
                {
                    //非泛型
                    if (!pi.PropertyType.IsGenericType)
                    {
                        pi.SetValue(objmodel, string.IsNullOrEmpty(dtRow[name].ToString()) ? null : Convert.ChangeType(dtRow[name], pi.PropertyType), null);
                    }
                    //泛型Nullable<>
                    else
                    {
                        Type genericTypeDefinition = pi.PropertyType.GetGenericTypeDefinition();
                        //model属性是可为null类型，进行赋null值
                        if (genericTypeDefinition == typeof(Nullable<>))
                        {
                            //返回指定可以为 null 的类型的基础类型参数
                            pi.SetValue(objmodel, string.IsNullOrEmpty(dtRow[name].ToString()) ? null : Convert.ChangeType(dtRow[name], Nullable.GetUnderlyingType(pi.PropertyType)), null);
                        }
                    }
                }
            }
            return objmodel;
        }

        /// <summary>
        /// 将DataRow赋值给model中同名属性
        /// </summary>
        /// <typeparam name="T">泛型：model的类型</typeparam>
        /// <param name="objmodel">model实例</param>
        /// <param name="dtRow">DataTable行数据</param>
        public static T TableToModel<T>(DataTable dt) where T : new()
        {
            T objModel = new T();
            if (dt == null || dt.Rows.Count == 0)
            {
                return objModel;
            }
            //获取model的类型
            Type modelType = typeof(T);
            //获取model中的属性
            PropertyInfo[] modelpropertys = modelType.GetProperties();
            //遍历model每一个属性并赋值DataRow对应的列
            foreach (PropertyInfo pi in modelpropertys)
            {
                //获取属性名称
                String name = pi.Name;
                if (dt.Columns.Contains(name))
                {
                    //非泛型
                    if (!pi.PropertyType.IsGenericType)
                    {
                        pi.SetValue(objModel, string.IsNullOrEmpty(dt.Rows[0][name].ToString()) ? null : Convert.ChangeType(dt.Rows[0][name], pi.PropertyType), null);
                    }
                    //泛型Nullable<>
                    else
                    {
                        Type genericTypeDefinition = pi.PropertyType.GetGenericTypeDefinition();
                        //model属性是可为null类型，进行赋null值
                        if (genericTypeDefinition == typeof(Nullable<>))
                        {
                            //返回指定可以为 null 的类型的基础类型参数
                            pi.SetValue(objModel, string.IsNullOrEmpty(dt.Rows[0][name].ToString()) ? null : Convert.ChangeType(dt.Rows[0][name], Nullable.GetUnderlyingType(pi.PropertyType)), null);
                        }
                    }
                }
            }
            return objModel;
        }

        /// <summary>
        /// 将DataRow赋值给model中同名属性
        /// </summary>
        /// <typeparam name="T">泛型：model的类型</typeparam>
        /// <param name="objmodel">model实例</param>
        /// <param name="dtRow">DataTable行数据</param>
        public static T TableRowToModelNoNull<T>(T objmodel, DataRow dtRow)
        {

            //获取model的类型
            Type modelType = typeof(T);
            //获取model中的属性
            PropertyInfo[] modelpropertys = modelType.GetProperties();
            //遍历model每一个属性并赋值DataRow对应的列
            foreach (PropertyInfo pi in modelpropertys)
            {
                //获取属性名称
                String name = pi.Name;
                if (dtRow.Table.Columns.Contains(name))
                {
                    //非泛型
                    if (!pi.PropertyType.IsGenericType)
                    {
                        pi.SetValue(objmodel, string.IsNullOrEmpty(dtRow[name].ToString()) ? "" : Convert.ChangeType(dtRow[name], pi.PropertyType), null);
                    }
                    //泛型Nullable<>
                    else
                    {
                        Type genericTypeDefinition = pi.PropertyType.GetGenericTypeDefinition();
                        //model属性是可为null类型，进行赋null值
                        if (genericTypeDefinition == typeof(Nullable<>))
                        {
                            //返回指定可以为 null 的类型的基础类型参数
                            pi.SetValue(objmodel, string.IsNullOrEmpty(dtRow[name].ToString()) ? "" : Convert.ChangeType(dtRow[name], Nullable.GetUnderlyingType(pi.PropertyType)), null);
                        }
                    }
                }
            }
            return objmodel;
        }

        /// <summary>
        /// 将V对象转换为T对象,不支持泛型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="t"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static T ModelConvert<T, V>(T t, V v)
        {
            if (v == null)
                return t;
            Type vType = v.GetType();
            //获取v中的属性
            PropertyInfo[] vpropertys = vType.GetProperties();

            Type tType = t.GetType();
            //获取t中属性
            PropertyInfo[] tpropertys = tType.GetProperties();
            if (!vType.IsGenericType && !tType.IsGenericType)
                foreach (PropertyInfo pi in vpropertys)
                {
                    foreach (PropertyInfo tpi in tpropertys)
                    {
                        if (!pi.PropertyType.IsGenericType && !tpi.PropertyType.IsGenericType)//非泛型
                        {
                            if (pi.Name.ToLower() == tpi.Name.ToLower())
                            {

                                tpi.SetValue(t, Convert.ChangeType(pi.GetValue(v, null), tpi.PropertyType), null);
                            }
                        }
                        else
                        {
                            //处理泛型                           
                        }
                    }
                }
            //else {
            //    //处理泛型
            //    Type vgenericTypeDefinition = v.GetType().GetGenericTypeDefinition();
            //    Type tgenericTypeDefinition = t.GetType().GetGenericTypeDefinition();
            //    PropertyInfo[] tgenericpropertys = tgenericTypeDefinition.GetProperties();
            //    PropertyInfo[] vgenericpropertys = vgenericTypeDefinition.GetProperties();

            //    foreach (PropertyInfo vgeneric in vgenericpropertys)
            //    {
            //        foreach (PropertyInfo tgeneric in tgenericpropertys)
            //        {
            //            if (vgeneric.PropertyType == tgeneric.PropertyType && vgeneric.Name.ToLower() == tgeneric.Name.ToLower())
            //            {
            //                tgeneric.SetValue(t, vgeneric.GetValue(v, null), null);
            //            }
            //        }
            //    } 

            //}
            return t;
        }

        /// <summary>
        /// 将V对象转换为T对象,不支持泛型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="t"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static T ModelConvert<T, V>(V v) where T : new()
        {
            T t = new T();
            if (v == null)
                return t;
            Type vType = v.GetType();
            //获取v中的属性
            PropertyInfo[] vpropertys = vType.GetProperties();

            Type tType = t.GetType();
            //获取t中属性
            PropertyInfo[] tpropertys = tType.GetProperties();
            if (!vType.IsGenericType && !tType.IsGenericType)
                foreach (PropertyInfo pi in vpropertys)
                {
                    foreach (PropertyInfo tpi in tpropertys)
                    {
                        if (!pi.PropertyType.IsGenericType && !tpi.PropertyType.IsGenericType)//非泛型
                        {
                            if (pi.Name.ToLower() == tpi.Name.ToLower())
                            {

                                tpi.SetValue(t, Convert.ChangeType(pi.GetValue(v, null), tpi.PropertyType), null);
                            }
                        }
                        else
                        {
                            //处理泛型                           
                        }
                    }
                }

            return t;
        }

        /// <summary>
        /// 对泛型数据进行对应转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="t"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static List<T> ModelConvert<T, V>(List<T> t, List<V> v)
        {
            if (v == null)
                return t;
            foreach (V _v in v)
            {
                T _t = (T)Activator.CreateInstance(typeof(T));
                PropertyInfo[] tpropertys = _t.GetType().GetProperties();

                PropertyInfo[] vpropertys = _v.GetType().GetProperties();
                foreach (PropertyInfo tpi in tpropertys)
                {
                    foreach (PropertyInfo vpi in vpropertys)
                    {
                        /// 属性与字段名称一致的进行赋值 
                        if (tpi.Name.ToLower().Equals(vpi.Name.ToLower()))
                        {
                            if (vpi.GetValue(_v, null) != DBNull.Value)
                                tpi.SetValue(_t, Convert.ChangeType(vpi.GetValue(_v, null), tpi.PropertyType), null);
                            else
                                tpi.SetValue(_t, null, null);
                        }
                    }
                }
                t.Add(_t);
            }

            return t;
        }

        /// <summary>
        /// table转换为泛型集合 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> DataTableToList<T>(DataTable dt) where T : new()
        {
            List<T> models = null;
            if (dt == null) { models = new List<T>(); models.Add(new T()); return models; }
            DataRowCollection rows = dt.Rows;
            DataColumnCollection dcc = dt.Columns;
            models = new List<T>(rows.Count);
            foreach (DataRow dr in rows)
            {
                models.Add(FillModel<T>(dr, dcc));
            }

            return models;
        }

        /// <summary>
        /// 填充实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static T FillModel<T>(DataRow row, DataColumnCollection columns) where T : new()
        {
            if (row == null) throw new ArgumentNullException();
            if (columns == null || columns.Count == 0) throw new ArgumentNullException("为空或列集合中列个数为0");
            PropertyInfo[] propertys = (typeof(T)).GetProperties();
            int len = propertys.Length;
            if (len == 0) throw new Exception("实体类中不包含任何属性成员");
            T obj = new T();
            for (int i = 0; i < len; i++)
            {
                string columnName = propertys[i].Name;
                if (columns.Contains(columnName))
                {
                    if (row[columnName] != DBNull.Value)
                    {
                        if (propertys[i].PropertyType.Name.StartsWith("Nullable`"))
                        {
                            propertys[i].SetValue(obj
                                , Convert.ChangeType(row[columnName], Nullable.GetUnderlyingType(propertys[i].PropertyType))
                                , null);
                        }
                        else
                        {
                            propertys[i].SetValue(obj
                                , Convert.ChangeType(row[columnName], propertys[i].PropertyType)
                                , null);
                        }
                    }
                }
            }
            return obj;
        }
    }
}
