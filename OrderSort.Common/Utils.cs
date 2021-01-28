
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Xml;
using System.Xml.Serialization;

namespace OrderSort.Common
{
    /// <summary>
    /// 工具类
    /// 创建人:niu
    /// 创建日期:2017.03.14
    /// </summary>
    public class Utils
    {
        /**/
        /// <summary>
        /// 判断对象是否为空，如果为空则返回true；反之返回false;
        /// </summary>
        public static bool IsNull(object obj)
        {
            if (obj == null)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 检查字符串是否是null或空字符
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(string input)
        {
            if (input == null || input.Trim().Length == 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 替换全部空格
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ReplaceAllBlank(string input)
        {
            if (IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            Regex regex = new Regex(" ");
            return regex.Replace(input, "");
        }
        /// <summary>
        /// 替换掉所有的回车换行
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string RemoveBr(string input)
        {
            return Regex.Replace(input, @"[\r|\n|\t|\v]", "");
        }
        /// <summary>
        /// 只保留汉字
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string RemoveNumber(string input)
        {
            //return Regex.Replace(input, @"[^\u4e00-\u9fa5]", "");
            //解决生僻字也被过滤掉问题 2020.11.16  yb
            return Regex.Replace(input, @"\d", "");
        }
        /// <summary>
        /// 判断字符串全部是否是中文或生僻字。返回true 则表示含有非中文或生僻字
        /// </summary>
        /// <param name="str_chinese"></param>
        /// <returns></returns>
        public static bool IsChinese(string str_chinese)
        {
            bool b = false;
            for (int i = 0; i < str_chinese.Length; i++)
            {
                Regex reg = new Regex(@"[\u4e00-\u9fa5]");
                if (!reg.IsMatch(str_chinese[i].ToString()))
                {

                    b = true;
                    break;
                }
            }

            return b;

        }
        /// <summary>
        /// 从汉字转换到16进制
        /// </summary>
        /// <param name="s">需要转换的字符串</param>
        /// <returns>返回16进制</returns>
        public static string GetHexFromChs(string CustName)
        {
            byte[] buffer = Encoding.Unicode.GetBytes(CustName);
            return "0x" + BitConverter.ToString(buffer, 0).Replace("-", string.Empty);
        }
        /// <summary>
        /// 检查输入值是否是日期
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        public static bool IsDate(string strDate)
        {
            if (!IsNullOrEmpty(strDate))
            {
                strDate = strDate.Trim();
            }
            else
            {
                return false;
            }
            Regex regex = new Regex(@"^([1|2]\d{3})-((0[1-9])|(1[0-2]))-((0[1-9])|([1|2]\d)|(3[0-1]))$", RegexOptions.Compiled);
            if (regex.IsMatch(strDate))
            {
                Match match = regex.Match(strDate);
                if (match.Success)
                {
                    try
                    {
                        Group yearGroup = match.Groups[1];
                        Group monthGroup = match.Groups[2];
                        Group dayGroup = match.Groups[5];

                        DateTime date = new DateTime(Int32.Parse(yearGroup.Value), Int32.Parse(monthGroup.Value), Int32.Parse(dayGroup.Value));
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
                return false;
            }
            return false;
        }
        /// <summary>
        /// 检查是否是有效的手机号
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static bool IsMobile(string mobile)
        {
            if (!IsNullOrEmpty(mobile))
            {
                mobile = mobile.Trim();
            }
            else
            {
                return false;
            }
            return Regex.IsMatch(mobile, @"^(?:1[3456789][0-9]{9})");
        }
        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(object expression, int defValue)
        {
            return TypeConverter.ObjectToInt(expression, defValue);
        }
        /// <summary>
        /// string型转换为bool型 
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(object expression, bool defValue)
        {
            if (expression != null)
                return TypeConverter.StrToBool(expression.ToString(), defValue);

            return defValue;
        }
        /// <summary>
        /// string型转换为日期型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的日期类型结果</returns>
        public static DateTime StrToDatetime(string strValue, DateTime defValue)
        {
            DateTime dt;
            try
            {
                if (!string.IsNullOrEmpty(strValue))
                {
                    dt = Convert.ToDateTime(strValue);
                }
                else
                {
                    dt = defValue;
                }
            }
            catch
            {
                dt = defValue;
            }
            return dt;
        }
        public static string StrToDatetimeFormat(string strValue, string defValue, string format, string oldFormat)
        {
            string dt;
            try
            {
                if (!string.IsNullOrEmpty(strValue))
                {
                    dt = DateTime.ParseExact(strValue, oldFormat, new CultureInfo("zh-CN"), DateTimeStyles.AllowWhiteSpaces).ToString(format);
                }
                else
                {
                    dt = defValue;
                }
            }
            catch
            {
                dt = defValue;
            }
            return dt;
        }
        /// <summary>
        /// 将字符串转换为Int32类型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(string expression, int defValue)
        {
            return TypeConverter.StrToInt(expression, defValue);
        }

        /// <summary>
        /// Object型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(object strValue, float defValue)
        {
            return TypeConverter.StrToFloat(strValue, defValue);
        }

        /// <summary>
        /// Object型转换为double型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的double类型结果</returns>
        public static double StrToDouble(object strValue, double defValue)
        {
            return TypeConverter.StrToDouble(strValue, defValue);
        }
        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(string strValue, float defValue)
        {
            return TypeConverter.StrToFloat(strValue, defValue);
        }
        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static decimal StrToDecimal(string strValue, decimal defValue)
        {
            return TypeConverter.StrToDecimal(strValue, defValue);
        }

        /// <summary>
        /// Object型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static decimal StrToDecimal(object strValue, decimal defValue)
        {
            return TypeConverter.StrToDecimal(strValue, defValue);
        }
        /// <summary>
        /// 是否为ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string strValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = HttpUtility.UrlEncode(strValue);
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string key, string strValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Values[key] = HttpUtility.UrlEncode(strValue);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        /// <param name="strValue">过期时间(分钟)</param>
        public static void WriteCookie(string strName, string strValue, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = HttpUtility.UrlEncode(strValue);
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string strName)
        {
            for (int i = 0; i < HttpContext.Current.Request.Cookies.Count; i++)
            {
                if (HttpContext.Current.Request.Cookies[i].Name.Replace(",", "").Replace(" ", "") == strName)
                    strName = HttpContext.Current.Request.Cookies[i].Name;
            }
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null)
                return HttpUtility.UrlDecode(HttpContext.Current.Request.Cookies[strName].Value.ToString());

            return "";
        }



        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string strName, string key)
        {
            for (int i = 0; i < HttpContext.Current.Request.Cookies.Count; i++)
            {
                if (HttpContext.Current.Request.Cookies[i].Name.Replace(",", "").Trim() == strName)
                    strName = HttpContext.Current.Request.Cookies[i].Name;
            }
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null && HttpContext.Current.Request.Cookies[strName][key] != null)
                return HttpUtility.UrlDecode(HttpContext.Current.Request.Cookies[strName][key].ToString());

            return "";
        }
        /// <summary>
        /// 清除cookie
        /// </summary>
        /// <param name="cookieName"></param>
        public static void ClearCookie(string cookieName)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie != null)
            {
                cookie.Values.Clear();
                cookie.Expires = DateTime.Now.AddYears(-1);
                HttpContext.Current.Response.Cookies.Set(cookie);
            }
        }
        /// <summary>
        /// 写后台登录cookie
        /// </summary>
        /// <param name="YwEmployeeId">员工ID</param>
        /// <param name="RoleId">角色ID</param>
        /// <param name="name">姓名</param>
        /// <param name="name">上次登录时间</param>
        public static void WriteAdminCookie(int YwEmployeeId, string RoleId, string name, string PrevLoginTime)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies["ywAdmin"];
            if (cookie == null)
            {
                cookie = new HttpCookie("ywAdmin");
            }
            cookie.HttpOnly = true;
            cookie.Values["YwEmployeeId"] = YwEmployeeId.ToString();
            cookie.Values["RoleId"] = RoleId;
            cookie.Values["name"] = name;
            cookie.Values["LoginTime"] = PrevLoginTime;

            cookie.Value = DESEncrypt.Encrypt(cookie.Value);  //2019.10.21  yb 登录cookie加密保存

            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        /// <summary>
        /// 得到后台登录cookie信息
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        // [Obsolete("该方法已过时，不再使用，请使用GetAdminRedis方法代替", true)]
        public static string GetAdminCookie(string strName)
        {
            string result = string.Empty;
            //if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies["ywAdmin"] != null)
            //{
            //    try
            //    {
            //        var value = DESEncrypt.Decrypt(HttpContext.Current.Request.Cookies["ywAdmin"].Value);
            //        if (!string.IsNullOrEmpty(value))
            //        {
            //            List<string> valueList = GetStrArray(value, '&', false);
            //            if (valueList != null && valueList.Count > 0)
            //            {
            //                foreach (var item in valueList)
            //                {
            //                    List<string> itemList = GetStrArray(item, '=', false);
            //                    if (itemList != null && itemList.Count == 2)
            //                    {
            //                        if (itemList[0].ToLower() == strName.ToLower())
            //                        {
            //                            result = HttpUtility.UrlDecode(itemList[1]);
            //                            break;
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //    }
            //}

            //if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies["ywAdmin"] != null && HttpContext.Current.Request.Cookies["ywAdmin"][strName] != null)
            //    return HttpUtility.UrlDecode(HttpContext.Current.Request.Cookies["ywAdmin"][strName].ToString());
            switch (strName)
            {
                case "RoleId":
                    strName = "RoleSid";
                    break;
                case "YwEmployeeId":
                    strName = "EmpId";
                    break;
                case "name":
                    strName = "EmpName";
                    break;
                case "YwEmployeeName":
                    strName = "EmpName";
                    break;
            }
            result = GetAdminRedis(strName);
            return result;
        }
        /// <summary>
        /// 得到后台登录redis缓存信息
        /// </summary>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static string GetAdminRedis(string searchName)
        {
            string result = string.Empty;
            //string key = HttpContext.Current.Request.Headers["empId"];

            //if (HttpContext.Current.Request.Cookies["ywAdmin"] != null && HttpContext.Current.Request.Cookies["AuthToken"] != null)
            //{
            //    string key = DESEncrypt.Decrypt(HttpContext.Current.Request.Cookies["ywAdmin"].Value);
            //    string dataKey = HttpContext.Current.Request.Cookies["AuthToken"].Value;
            //    Redis.RedisHelper rh = new Redis.RedisHelper();
            //    var objUserAuth = rh.HashGet<BanChenGuar.Model.ObjUserAuth>(key, dataKey);
            //    if (objUserAuth != null)
            //    {
            //        // 获取类型
            //        Type modeType = typeof(BanChenGuar.Model.ObjUserAuth);
            //        // 获取属性集合
            //        System.Reflection.PropertyInfo[] properties = modeType.GetProperties();
            //        // 遍历属性
            //        foreach (var property in properties)
            //        {
            //            // 属性名称
            //            string name = property.Name;
            //            if (name == searchName)
            //            {
            //                result = Convert.ToString(property.GetValue(objUserAuth, null));
            //            }
            //        }
            //    }
            //}

            return result;
        }
        /// <summary>
        ///  获取总的页面权限
        /// </summary>
        /// <returns></returns>
        //public static List<Model.ObjPageInfo> GetAllPagePermit()
        //{
        //    List<Model.ObjPageInfo> listPageInfo = new List<Model.ObjPageInfo>();
        //    Redis.RedisHelper rh = new Redis.RedisHelper();
        //    listPageInfo = rh.HashGet<List<Model.ObjPageInfo>>(RedisKeys.HashKeys.AllPagePermit.ToString(), "allpagepermit");
        //    return listPageInfo;
        //}
        /// <summary>
        /// 清除后台登录Cookie
        /// </summary>
        public static void ClearAdminCookie()
        {
            ClearCookie("ywAdmin");
        }
        /// <summary>
        /// 写文件系统登录Cookie
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="UserRolesid">角色ID</param>
        /// <param name="EmployeeSidFile">员工ID</param>
        public static void WriteFileSysCookie(string username, string UserRolesid, string EmployeeSidFile)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies["fileAdmin"];
            if (cookie == null)
            {
                cookie = new HttpCookie("fileAdmin");
            }
            cookie.HttpOnly = true;
            cookie.Values["LoginUserName"] = HttpContext.Current.Server.UrlEncode(username);
            cookie.Values["username"] = HttpContext.Current.Server.UrlEncode(username);
            cookie.Values["fileusername"] = HttpContext.Current.Server.UrlEncode(username);
            cookie.Values["UserRolesid"] = HttpContext.Current.Server.UrlEncode(UserRolesid);
            cookie.Values["EmployeeSidFile"] = HttpContext.Current.Server.UrlEncode(EmployeeSidFile);

            cookie.Value = DESEncrypt.Encrypt(cookie.Value);  //2019.10.21  yb 登录cookie加密保存

            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        /// <summary>
        /// 得到文件系统登录Cookie信息
        /// </summary>
        /// <param name="strName">Cookie名称</param>
        /// <returns></returns>
        public static string GetFileSysCookie(string strName)
        {
            string result = string.Empty;
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies["fileAdmin"] != null)
            {
                try
                {
                    var value = DESEncrypt.Decrypt(HttpContext.Current.Request.Cookies["fileAdmin"].Value);
                    if (!string.IsNullOrEmpty(value))
                    {
                        List<string> valueList = GetStrArray(value, '&', false);
                        if (valueList != null && valueList.Count > 0)
                        {
                            foreach (var item in valueList)
                            {
                                List<string> itemList = GetStrArray(item, '=', false);
                                if (itemList != null && itemList.Count == 2)
                                {
                                    if (itemList[0].ToLower() == strName.ToLower())
                                    {
                                        result = HttpUtility.UrlDecode(itemList[1]);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {

                }
            }

            //if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies["fileAdmin"] != null && HttpContext.Current.Request.Cookies["fileAdmin"][strName] != null)
            //return HttpUtility.UrlDecode(HttpContext.Current.Request.Cookies["fileAdmin"][strName].ToString());

            return result;
        }
        /// <summary>
        /// 清除文件系统登录Cookie
        /// </summary>
        public static void ClearFileSysCookie()
        {
            ClearCookie("fileAdmin");
        }
        /// <summary>
        /// 构造一个还款登记的DataTable
        /// </summary>
        /// <returns>DataTable</returns>
        public static DataTable GetHKDJTable()
        {
            DataTable dt = new DataTable();
            dt.TableName = "WorkFloWSLI_HKDJ";
            String[] columns = { "Pronum", "ProNumInChild", "custsid", "tzdetailsid", "HKtypeDicSid", "HKdateTime", "HKQiShu", "HKlixi", "HkBenJin", "FaXi", "OperDatetime", "OperempSid", "TJflag", "shlixi", "shbj", "fxflag", "CreateDatetime", "CreateUserId" };
            IList<DataColumn> dataColumns = new List<DataColumn>();
            foreach (String column in columns)
            {
                dt.Columns.Add(new DataColumn(column, typeof(String)));
            }
            return dt;
        }
        /// <summary>
        /// 构造一个还款登记的DataTable(用于借款人)
        /// </summary>
        /// <returns></returns>
        public static DataTable GetHKDJTableJkr()
        {
            DataTable dt = new DataTable();
            dt.TableName = "WorkFloWSLI_JKR_HKDJ";
            String[] columns = { "Pronum", "ProNumInChild", "HKtypeDicSid", "HKdateTime", "HKQiShu", "HKlixi", "HkBenJin", "FaXi", "OperDatetime", "OperempSid", "TJflag", "shlixi", "shbj", "YJFlag", "fxflag", "IsNoCherck", "CreateDatetime", "CreateUserId" };
            IList<DataColumn> dataColumns = new List<DataColumn>();
            foreach (String column in columns)
            {
                if (column == "HKtypeDicSid" || column == "HKQiShu" || column == "OperempSid")
                {
                    dt.Columns.Add(new DataColumn(column, typeof(int)));
                }
                else if (column == "HKlixi" || column == "HkBenJin" || column == "FaXi" || column == "shlixi" || column == "shbj")
                {
                    dt.Columns.Add(new DataColumn(column, typeof(decimal)));
                }
                else if (column == "TJflag" || column == "YJFlag" || column == "IsNoCherck")
                {
                    dt.Columns.Add(new DataColumn(column, typeof(bool)));
                }
                else
                {
                    dt.Columns.Add(new DataColumn(column, typeof(String)));
                }
            }
            return dt;
        }
        /// <summary>
        /// 还款登记列映射
        /// </summary>
        /// <returns>IList<String[]></returns>
        public static IList<String[]> GetHKDJColumnsMapping()
        {
            IList<String[]> columns = new List<String[]>();
            columns.Add(new String[] { "Pronum", "Pronum" });
            columns.Add(new String[] { "ProNumInChild", "ProNumInChild" });
            columns.Add(new String[] { "custsid", "custsid" });
            columns.Add(new String[] { "tzdetailsid", "tzdetailsid" });
            columns.Add(new String[] { "HKtypeDicSid", "HKtypeDicSid" });
            columns.Add(new String[] { "HKdateTime", "HKdateTime" });
            columns.Add(new String[] { "HKQiShu", "HKQiShu" });
            columns.Add(new String[] { "HKlixi", "HKlixi" });
            columns.Add(new String[] { "HkBenJin", "HkBenJin" });
            columns.Add(new String[] { "FaXi", "FaXi" });
            columns.Add(new String[] { "OperDatetime", "OperDatetime" });
            columns.Add(new String[] { "OperempSid", "OperempSid" });
            columns.Add(new String[] { "TJflag", "TJflag" });
            columns.Add(new String[] { "shlixi", "shlixi" });
            columns.Add(new String[] { "shbj", "shbj" });
            columns.Add(new String[] { "fxflag", "fxflag" });
            columns.Add(new String[] { "CreateDatetime", "CreateDatetime" });
            columns.Add(new String[] { "CreateUserId", "CreateUserId" });
            return columns;
        }
        /// <summary>
        /// 还款登记列映射(用于借款人)
        /// </summary>
        /// <returns>IList<String[]></returns>
        public static IList<String[]> GetHKDJColumnsMappingJkr()
        {
            IList<String[]> columns = new List<String[]>();
            columns.Add(new String[] { "Pronum", "Pronum" });
            columns.Add(new String[] { "ProNumInChild", "ProNumInChild" });
            columns.Add(new String[] { "HKtypeDicSid", "HKtypeDicSid" });
            columns.Add(new String[] { "HKdateTime", "HKdateTime" });
            columns.Add(new String[] { "HKQiShu", "HKQiShu" });
            columns.Add(new String[] { "HKlixi", "HKlixi" });
            columns.Add(new String[] { "HkBenJin", "HkBenJin" });
            columns.Add(new String[] { "FaXi", "FaXi" });
            columns.Add(new String[] { "OperDatetime", "OperDatetime" });
            columns.Add(new String[] { "OperempSid", "OperempSid" });
            columns.Add(new String[] { "TJflag", "TJflag" });
            columns.Add(new String[] { "shlixi", "shlixi" });
            columns.Add(new String[] { "shbj", "shbj" });
            columns.Add(new String[] { "YJFlag", "YJFlag" });
            columns.Add(new String[] { "fxflag", "fxflag" });
            columns.Add(new String[] { "IsNoCherck", "IsNoCherck" });
            columns.Add(new String[] { "CreateDatetime", "CreateDatetime" });
            columns.Add(new String[] { "CreateUserId", "CreateUserId" });
            return columns;
        }
        public static string DataTablePageSizeJson(DataTable dt, string pageCount, string dataCount, string pageIndex)
        {
            if (dt.Rows.Count == 0)
                return "{\"data\":[]}";
            string json = DataTable2Json(dt);
            json = json.Remove(json.Length - 1);
            json += ",\"pagecount\":" + pageCount + ",\"datacount\":" + dataCount + ",\"pageindex\":" + pageIndex + "}";
            return json;
        }
        public static string DataTablePageSizeJson2(DataTable dt, string pageCount, string dataCount, string pageIndex, object dataExt)
        {
            if (dt.Rows.Count == 0)
                return "{\"data\":[]}";
            string json = DataTable2Json(dt);
            json = json.Remove(json.Length - 1);
            json += ",\"pagecount\":" + pageCount + ",\"datacount\":" + dataCount + ",\"pageindex\":" + pageIndex + $",\"DataExt\":{JsonHelper.SerializeObject(dataExt)}}}";
            return json;
        }
        public static string DataTablePageSizeJson(DataTable dt, string pageCount, string dataCount, string pageIndex, bool isCustom)
        {
            if (dt.Rows.Count == 0)
                return "{\"data\":[]}";
            string json = DataTable2Json(dt);
            json = json.Remove(json.Length - 1);
            json += ",\"pagecount\":" + pageCount + ",\"datacount\":" + dataCount + ",\"pageindex\":" + pageIndex;
            if (!isCustom)
            {
                json += "}";
            }
            return json;
        }
        /// <summary>          
        /// DataTable转换成json格式          
        /// </summary>          
        /// <param name="dt">要转换的DataTable</param>          
        /// <returns>返回json字符串</returns>          
        public static string DataTable2Json(DataTable dt)
        {
            if (dt.Rows.Count == 0)
                return "";
            StringBuilder json = new StringBuilder();
            json.Append("{\"data\":[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                json.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    json.Append("\"");
                    json.Append(dt.Columns[j].ColumnName);
                    json.Append("\":\"");
                    json.Append(RemoveBr(dt.Rows[i][j].ToString().Replace("\"", "'")));
                    json.Append("\",");
                }
                json.Remove(json.Length - 1, 1);
                json.Append("},");
            }
            json.Remove(json.Length - 1, 1);
            json.Append("]}");
            return json.ToString();
        }
        /// <summary>
        /// 快速输出json
        /// </summary>
        /// <param name="success">"true" or "false"</param>
        /// <param name="msg">输出信息</param>
        public static void ResponseText(string success, string msg)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write("{\"success\":" + success + ",\"msg\":\"" + msg + "\"}");
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
        public static void ResponseText(object res)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(JsonHelper.SerializeObject(res));
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
        /// <summary>
        /// 快速输出json
        /// </summary>
        /// <param name="success">"true" or "false"</param>
        /// <param name="msg">输出信息</param>
        /// <param name="data">输出数据</param>
        public static void ResponseText(string success, string msg, string data)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write("{\"success\":" + success + ",\"msg\":\"" + msg + "\",\"data\":" + data + "}");
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
        public static void ResponseJsonData(string success, string msg, DataTable dt)
        {
            StringBuilder json = new StringBuilder();
            json.Append("[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                json.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    json.Append("\"");
                    json.Append(dt.Columns[j].ColumnName);
                    json.Append("\":\"");
                    json.Append(RemoveBr(dt.Rows[i][j].ToString().Replace("\"", "'")));
                    json.Append("\",");
                }
                json.Remove(json.Length - 1, 1);
                json.Append("},");
            }
            json.Remove(json.Length - 1, 1);
            json.Append("]");

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write("{\"success\":" + success + ",\"msg\":\"" + msg + "\",\"data\":" + json.ToString() + "}");
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
        /// <summary>
        /// 在页面弹出提示框
        /// </summary>
        /// <param name="page">页面</param>
        /// <param name="strMsg">提示信息</param>
        public static void Alert(Page page, string strMsg)
        {
            page.ClientScript.RegisterClientScriptBlock(page.GetType(), "", "window.top.layerUtils.alert('" + strMsg + "');", true);
        }
        /// <summary>
        /// 在页面弹出操作成功提示框
        /// </summary>
        /// <param name="page">页面</param>
        /// <param name="strMsg">提示信息</param>
        public static void SuccessTip(Page page, string strMsg)
        {
            page.ClientScript.RegisterClientScriptBlock(page.GetType(), "", "window.top.layerUtils.updateSuccessTip('" + strMsg + "');", true);
        }
        /// <summary>
        /// 判断字符串是否存在操作数据库的安全隐患, 并返回字符串
        /// </summary>
        public static string IsSafetys(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            string str = s.Replace("%20", " ");
            str = str.Replace(@"\s", " ");
            string pattern = @"exec |select |sp_executesql |EXECUTE |convert |count |cast |sysobjects |drop |creat |insert |delete from |count\(|drop table|update |truncate |asc\(|mid\(|char\(|xp_cmdshell|exec master|net localgroup administrators|net user|""|\'| or ";
            Regex reg = new Regex(pattern, RegexOptions.IgnoreCase);
            str = reg.Replace(str, "");
            return str;
        }
        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="MianStr">要截取的字符串</param>
        /// <param name="nleng">要截取的长度</param>
        /// <returns>返回字符串</returns>
        public static string CutSrings(string MianStr, int nleng)
        {
            nleng = nleng * 2;
            MianStr = clearHtml(MianStr).Trim();
            int byteLen = Encoding.Default.GetByteCount(MianStr);
            if (byteLen > nleng)
            {
                string resultStr = "";
                for (int i = 0; i < MianStr.Length; i++)
                {
                    byte[] tempByte = System.Text.Encoding.Default.GetBytes(resultStr);
                    if (tempByte.Length < nleng)
                    {

                        resultStr += MianStr.Substring(i, 1);
                    }
                    else
                    {
                        break;
                    }
                }
                return resultStr;
            }
            else
            {
                return MianStr;
            }
            //MianStr = clearHtml(MianStr);
            //if (MianStr.Length > nleng)
            //{
            //    return MianStr.Substring(0, nleng);
            //}
            //else
            //{
            //    return MianStr;
            //}
        }

        /// <summary>
        /// 根据字节长度来截取字符串
        /// </summary>
        ///<param name="origStr">原始字符串</param>
        ///<param name="length">提取前length个字节</param>
        /// <returns></returns> 
        public static String SubstringByByte(string origStr, int length)
        {
            byte[] bytes = System.Text.Encoding.Unicode.GetBytes(origStr);
            int n = 0; //  表示当前的字节数
            int i = 0; //  要截取的字节数
            for (; i < bytes.GetLength(0) && n < length; i++)
            {
                //  偶数位置，如0、2、4等，为UCS2编码中两个字节的第一个字节
                if (i % 2 == 0)
                {
                    n++; //  在UCS2第一个字节时n加1
                }
                else
                {
                    //  当UCS2编码的第二个字节大于0时，该UCS2字符为汉字，一个汉字算两个字节
                    if (bytes[i] > 0)
                    {
                        n++;
                    }
                }
            }
            //  如果i为奇数时，处理成偶数
            if (i % 2 == 1)
            {
                //  该UCS2字符是汉字时，去掉这个截一半的汉字
                if (bytes[i] > 0)
                    i = i - 1;
                //  该UCS2字符是字母或数字，则保留该字符
                else
                    i = i + 1;
            }
            return Encoding.Unicode.GetString(bytes, 0, i);
        }

        /// <summary>
        /// 去除HTML标记
        /// </summary>
        /// <param name="Htmlstring">包括HTML的源码 </param>
        /// <returns>已经去除后的文字</returns>
        public static string clearHtml(string Htmlstring)
        {
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Htmlstring.Replace("\r\n", "");
            Htmlstring = Htmlstring.Replace("&nbsp;", "");
            Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();
            return Htmlstring;
        }
        /// <summary>
        /// 根据数据表获取该表第一列的字符串形式
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <returns></returns>
        public static string GetStringByDataTable(DataTable dt)
        {
            string _proNums = string.Empty;
            if (dt != null && dt.Rows.Count > 0)
            {
                int _sCount = 0;
                foreach (DataRow drs in dt.Rows)
                {
                    if (_sCount == 0)
                    {
                        _proNums = "'" + drs[0].ToString() + "'";
                    }
                    else
                    {
                        _proNums += ",'" + drs[0].ToString() + "'";
                    }
                    _sCount++;
                }
            }
            return _proNums;
        }
        /// <summary>
        /// 获取还款计划书的日期相差天数
        /// </summary>
        /// <param name="DateTime1"></param>
        /// <param name="DateTime2"></param>
        /// <returns></returns>
        public static string DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            string dateDiff = null;
            //时间差的天数
            int Sumday = 0;

            //起始时间月

            int MonthSta1 = DateTime1.Month;
            //起始时间日

            int days1 = DateTime1.Day;
            //截至时间月

            int MonthEnd2 = DateTime2.Month;
            //截至时间日

            int days2 = DateTime2.Day;
            //若果月份相同，则直接拿天数相减

            if (MonthSta1 == MonthEnd2)
            {
                //2017.03.01 niu 修正如果当前期的截止日期是2月底的情况，计算收益不正确（平年少2天，闰年少1天）
                if (MonthEnd2 == 2 && days2 == DateTime.DaysInMonth(DateTime2.Year, MonthEnd2))//如果是2月底(平年28日，闰年29日)
                {
                    days2 = 30;//按30天来计算
                }
                Sumday = days2 - days1;
            }
            else
            {
                int stataDay = 0;
                //如果大月，天数是31，则拿31天减
                if (days1 == 31)
                {
                    stataDay = 31 - days1;
                }
                else
                { //跨月则拿起始时间天所有的日，拿30天算。

                    stataDay = 30 - days1;
                }
                Sumday = stataDay + days2;
            }
            //生成还款计划书时，31日放款的项目，首次付息时应该是20天的利息  20110801
            if (days1 == 31)
            {
                Sumday = Sumday - 1;
            }
            dateDiff = Sumday.ToString();

            return dateDiff;
        }
        /// <summary>
        /// 把InputStream转换成字节
        /// </summary>
        /// <param name="InputStream">InputStream</param>
        /// <returns></returns>
        public static byte[] InputStreamToBytes(Stream InputStream)
        {
            int lengt = Convert.ToInt32(InputStream.Length);
            byte[] arr = new byte[lengt];
            InputStream.Read(arr, 0, lengt);
            InputStream.Dispose();
            return arr;
        }
        /// <summary>
        /// 获取是类上海还是类郑州的地区
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public static string GetCity(string city)
        {
            string result = "";
            string[] theSameCitySh = { "上海", "苏州", "杭州", "无锡", "昆山", "常州", "宁波", "嘉兴", "南通", "合肥", "长沙", "南昌" };
            if (Array.IndexOf<string>(theSameCitySh, city) != -1)
            {
                result = "上海";
            }
            else
            {
                result = "郑州";
            }
            return result;
        }

        #region 时间戳 转换
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp(System.DateTime time)
        {
            long ts = ConvertDateTimeToInt(time);
            return ts.ToString();
        }
        /// <summary>  
        /// 将c# DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time">时间</param>  
        /// <returns>long</returns>  
        public static long ConvertDateTimeToInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
            return t;
        }

        /// <summary>        
        /// 时间戳转为C#格式时间        
        /// </summary>        
        /// <param name=”timeStamp”></param>        
        /// <returns></returns>        
        public static DateTime ConvertStringToDateTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }
        #endregion

        #region  排序拼接字符串
        public static string ToUrl(SortedDictionary<string, object> parameters)
        {
            string buff = "";
            foreach (KeyValuePair<string, object> pair in parameters)
            {
                if (pair.Value == null)
                {
                    throw new Exception("内部含有值为null的字段!");
                }

                if (pair.Value.ToString() != "")
                {
                    buff += pair.Key + "=" + pair.Value + "&";
                }
            }
            buff = buff.Trim('&');

            return buff;
        }
        #endregion

        #region 把字符串按照分隔符转换成 List
        /// <summary>
        /// 把字符串按照分隔符转换成 List
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="speater">分隔符</param>
        /// <param name="toLower">是否转换为小写</param>
        /// <returns></returns>
        public static List<string> GetStrArray(string str, char speater, bool toLower)
        {
            if (string.IsNullOrEmpty(str))
            {
                str = "";
            }
            List<string> list = new List<string>();
            string[] ss = str.Split(speater);
            foreach (string s in ss)
            {
                if (!string.IsNullOrEmpty(s) && s != speater.ToString())
                {
                    string strVal = s;
                    if (toLower)
                    {
                        strVal = s.ToLower();
                    }
                    list.Add(strVal);
                }
            }
            return list;
        }
        #endregion

        #region 把 List<string> 按照分隔符组装成 string
        /// <summary>
        /// 把 List<string> 按照分隔符组装成 string
        /// </summary>
        /// <param name="list"></param>
        /// <param name="speater"></param>
        /// <returns></returns>
        public static string GetArrayStr(List<string> list, string speater)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == list.Count - 1)
                {
                    sb.Append(list[i]);
                }
                else
                {
                    sb.Append(list[i]);
                    sb.Append(speater);
                }
            }
            return sb.ToString();
        }
        #endregion

        #region 检查身份证号
        /// <summary>
        /// 验证身份证号码
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static bool CheckIDCard(string Id)
        {
            if (Id.Length == 18)
            {
                bool check = CheckIDCard18(Id);
                return check;
            }
            else if (Id.Length == 15)
            {
                bool check = CheckIDCard15(Id);
                return check;
            }
            else
            {
                return false;
            }
        }

        private static bool CheckIDCard18(string Id)
        {
            long n = 0;
            if (long.TryParse(Id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                return false;//省份验证
            }
            string birth = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = Id.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != Id.Substring(17, 1).ToLower())
            {
                return false;//校验码验证
            }
            return true;//符合GB11643-1999标准
        }
        private static bool CheckIDCard15(string Id)
        {
            long n = 0;
            if (long.TryParse(Id, out n) == false || n < Math.Pow(10, 14))
            {
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                return false;//省份验证
            }
            string birth = Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            return true;//符合15位身份证标准
        }

        
        /// <summary>
        /// 根据身份证获取性别
        /// </summary>
        /// <param name="IdCard"></param>
        /// <returns></returns>
        public static string GetSexFromIdCard(string IdCard)
        {
            string rtn;
            string tmp = "";
            if (IdCard.Length == 15)
            {
                tmp = IdCard.Substring(IdCard.Length - 3);
            }
            else if (IdCard.Length == 18)
            {
                tmp = IdCard.Substring(IdCard.Length - 4);
                tmp = tmp.Substring(0, 3);
            }
            else
            {
                return "";
            }
            int sx = int.Parse(tmp);
            int outNum;
            Math.DivRem(sx, 2, out outNum);
            if (outNum == 0)
            {
                rtn = "女";
            }
            else
            {
                rtn = "男";
            }
            return rtn;
        }

        /// <summary>
        /// 根据出生日期，计算精确的年龄
        /// </summary>
        /// <param name="birthDate">生日</param>
        /// <returns></returns>
        public static int GetAgeByBirthDay(string birthDay)
        {
            DateTime birthDate = DateTime.Parse(birthDay);
            DateTime nowDateTime = DateTime.Now;
            int age = nowDateTime.Year - birthDate.Year;
            //再考虑月、天的因素
            if (nowDateTime.Month < birthDate.Month || (nowDateTime.Month == birthDate.Month && nowDateTime.Day < birthDate.Day))
            {
                age--;
            }
            return age;
        }
        
        #endregion

        #region  xml对象转换
        /// <summary>
        /// 将自定义对象序列化为XML字符串
        /// </summary>
        /// <param name="myObject">自定义对象实体</param>
        /// <returns>序列化后的XML字符串</returns>
        public static string SerializeToXml<T>(T myObject)
        {
            if (myObject != null)
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));

                MemoryStream stream = new MemoryStream();
                XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8);
                writer.Formatting = Formatting.None;//缩进
                xs.Serialize(writer, myObject);

                stream.Position = 0;
                StringBuilder sb = new StringBuilder();
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        sb.Append(line);
                    }
                    reader.Close();
                }
                writer.Close();
                return sb.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// 将XML字符串反序列化为对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="xml">XML字符</param>
        /// <returns></returns>
        public static T DeserializeToObject<T>(string xml)
        {
            T myObject;
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StringReader reader = new StringReader(xml);
            myObject = (T)serializer.Deserialize(reader);
            reader.Close();
            return myObject;
        }
        #endregion

        /// <summary>
        /// ToString() 方法
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToS(object obj)
        {
            if (obj == null) return "";
            return obj.ToString();
        }
        public static string ReplaceSafetys(string s)
        {
            //string str = s.Replace("%20", " ");
            //str = str.Replace(@"\s", " ");
            string str = s;
            string pattern = @"exec |sp_executesql |EXECUTE |drop |create |delete from |update |truncate |exec master|net localgroup administrators|net user ";
            Regex reg = new Regex(pattern, RegexOptions.IgnoreCase);
            str = reg.Replace(str, "");
            return str;
        }
        /// <summary>
        /// 生成短信验证码
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string GetValidationNum(int count = 6)
        {
            string str = "0123456789";
            string validatecode = "";
            Random rd = new Random();
            for (int i = 0; i < count; i++)
            {
                validatecode += str.Substring(rd.Next(0, str.Length), 1);
            }
            return validatecode;
        }
        /// <summary>
        /// 获取当前日期
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentDate()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 检查请求来源是PC还是Mobile
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static bool CheckPC(HttpRequestBase request)
        {
            bool isPC = true;
            string strUserAgent = request.UserAgent.ToString().ToLower();
            bool isMobile = request.Browser.IsMobileDevice;
            if (isMobile || strUserAgent.Contains("iphone") || strUserAgent.Contains("blackberry") || strUserAgent.Contains("mobile") || strUserAgent.Contains("windows ce") || strUserAgent.Contains("opera mini") || strUserAgent.Contains("palm") || strUserAgent.Contains("samsung") || strUserAgent.Contains("htc"))
            {
                //移动端
                isPC = false;
            }

            return isPC;
        }



        //中文逗号转英文逗号
        public static string ZhongDaoYing(string Zhong)
        {
            string rest = "";
            char douHao = '，';
            int a = douHao;
            // MessageBox.Show(a.ToString());
            for (int i = 0; i < Zhong.Length; i++)
            {
                char c = Zhong[i];
                if (c == douHao)
                {
                    rest += ",";
                }
                else
                {
                    rest += Zhong[i];
                }
            }
            return rest;
        }


        /// <summary>
        /// 获取字符串中的数字 
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>数字</returns>
        public static decimal GetNumber(string str)
        {
            decimal result = 0;
            if (str != null && str != string.Empty)
            {
                // 正则表达式剔除非数字字符（不包含小数点.）
                //str = Regex.Replace(str, @"[^/d./d]", "");　　　　　　　　　 str = Regex.Replace(str, @"[^\d.\d]", "");
                // 如果是数字，则转换为decimal类型
                if (Regex.IsMatch(str, @"^[+-]?\d*[.]?\d*$"))
                {
                    result = decimal.Parse(str);
                }
            }
            return result;
        }

        /// <summary>
        /// 获取字符串中的数字
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>数字</returns>
        public static int GetNumberInt(string str)
        {
            int result = 0;
            if (str != null && str != string.Empty)
            {
                // 正则表达式剔除非数字字符（不包含小数点.）
                str = Regex.Replace(str, @"[^\d.\d]", "");
                // 如果是数字，则转换为decimal类型
                if (Regex.IsMatch(str, @"^[+-]?\d*[.]?\d*$"))
                {
                    result = int.Parse(str);
                }
            }
            return result;
        }

        /// <summary>
        /// add by pino for 传入两个model和赋值的字段名称，自动实现从源model将值赋值到目标model
        /// </summary>
        /// <typeparam name="TSource">Source Model Type</typeparam>
        /// <typeparam name="TTarget">Target Model Type</typeparam>
        /// <param name="source">SourceModel</param>
        /// <param name="target">TargetModel</param>
        /// <param name="sourceProperty">Source Model PropertyName</param>
        /// <param name="targetProperty">Target Model PropertyName</param>
        [Obsolete("该方法不支持不同类型的转换。", true)]
        public static void SetValue<TSource, TTarget>(TSource source, TTarget target, string sourceProperty, string targetProperty)
        {
            var sourceType = source.GetType();
            var targetType = target.GetType();
            var source_Property = sourceType.GetProperty(sourceProperty);
            if (source_Property != null)
            {
                var sourcevalue = source_Property.GetValue(source);
                if (sourcevalue != null)
                {
                    var target_Property = targetType.GetProperty(targetProperty);
                    if (target_Property != null)
                    {

                        target_Property.SetValue(target, Convert.ChangeType(sourcevalue, target_Property.PropertyType));
                    }
                }

            }

        }

        /// <summary>
        /// 反射实现两个类的对象之间相同属性的值的复制
        /// 适用于没有新建实体之间
        /// </summary>
        /// <typeparam name="D">返回的实体</typeparam>
        /// <typeparam name="S">数据源实体</typeparam>
        /// <param name="d">返回的实体</param>
        /// <param name="s">数据源实体</param>
        /// <returns></returns>
        public static D MapperToModel<D, S>(D d, S s)
        {
            try
            {
                var Types = s.GetType();//获得类型  
                var Typed = typeof(D);
                foreach (PropertyInfo sp in Types.GetProperties())//获得类型的属性字段  
                {
                    foreach (PropertyInfo dp in Typed.GetProperties())
                    {
                        if (dp.Name == sp.Name && CompareType(dp.PropertyType, sp.PropertyType) && dp.Name != "Error" && dp.Name != "Item")//判断属性名是否相同  
                        {
                            dp.SetValue(d, sp.GetValue(s, null), null);//获得s对象属性的值复制给d对象的属性  
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return d;
        }
        /// <summary>
        /// 验证类型是否相等 cxy
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static bool CompareType(Type a, Type b)
        {
            if (a == b) return true;
            if (a.IsGenericType && a.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(a);
                a = nullableConverter.UnderlyingType;
            }
            if (a == b) return true;
            if (b.IsGenericType && b.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(b);
                b = nullableConverter.UnderlyingType;
            }
            if (a == b) return true;
            return false;
        }
        
        /// *********************BC*************************************
        ///  创建者:张金辉
        ///  创建时间:2020-09-16 11:39:05
        ///  修改者:张金辉
        ///  修改时间:2020-09-25 14:14:20
        /// <summary>
        /// 获取IP地址
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetIpAddress()
        {
            string result = String.Empty;
            result = HttpContext.Current.Request.ServerVariables["HTTP_CDN_SRC_IP"];
            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.UserHostAddress;

            if (string.IsNullOrEmpty(result) || !IsIP(result))
                return "127.0.0.1";

            return result;
        }

        /// *********************BC*************************************
        ///  创建者:张金辉
        ///  创建时间:2020-09-16 11:39:05
        ///  修改者:张金辉
        ///  修改时间:2020-09-25 16:09:18
        /// <summary>
        /// 获取浏览器名称.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetBrowser()
        {
            string result = HttpContext.Current.Request.Browser.Browser;
            return result;
        }
    }
}
