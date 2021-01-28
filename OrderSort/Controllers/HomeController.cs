using OrderSort.Bll;
using OrderSort.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

using OrderSort.Common;
using System.Text.RegularExpressions;
using System.IO;

namespace OrderSort.Controllers
{
    public class HomeController : Controller
    {
        public List<string> Sort = new List<string>{ "S", "M","L","XL","2XL","3XL","4XL","5XL","25","26","27","28","29","30","31","32","33","34" };
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public void test()
        {
            string filePath = "E:\\MyProject\\OrderSort\\OrderSort\\Upload\\345.xlsx";
            ExcelHelper.ReadExcelToDataTable(filePath);
        }
        [HttpPost]
        public JsonResult UploadFile()
        {
            //定义
            ResponseDTO<string> obj = new ResponseDTO<string>();
            try
            {
                HttpPostedFileBase file = Request.Files[0];
                var path = "/Upload/";
                var mapPath = HttpContext.Server.MapPath(path); //硬盘物理目录
                var fileName = "淘宝发货订单";
                var mapPathFullPath = mapPath + fileName + ".xlsx"; //硬盘物理路径
                if (!Directory.Exists(mapPath))
                {
                    Directory.CreateDirectory(mapPath);
                }
                // 先删除原有文件
                if (System.IO.File.Exists(mapPathFullPath))
                {
                    System.IO.File.Delete(mapPathFullPath);
                }
                file.SaveAs(mapPathFullPath);
                obj.code = 1;
                obj.msg = "上传成功";
            }
            catch (Exception ex)
            {
                obj.code = 0;
                obj.msg = ex.Message;
                throw new Exception(ex.Message);
            }
            return Json(obj);
        }
        public JsonResult GetJson()
        {
            ResponseDTO<List<Order>> res = new ResponseDTO<List<Order>>();
            res.code = 200;
            res.msg = "";
            res.data = new List<Order>();
            try
            {
                int page = Utils.StrToInt(Request.Params["page"].ToString(), 1);                
                var path = "/Upload/";
                var mapPath = HttpContext.Server.MapPath(path); //硬盘物理目录
                string filePath = mapPath + "\\淘宝发货订单.xlsx";
                DataTable dt = ExcelHelper.ReadExcelToDataTable(filePath);
                List<Order> listOrder = new List<Order>();
                foreach (DataRow row in dt.Rows)
                {
                    string tempCode = row["商家编码"].ToString();
                    // 获取货号
                    string tempSize = Utils.ZhongDaoYing(row["规格"].ToString());
                    string sku = tempSize.Split(new char[] { ',' })[0].ToString();
                    if (Regex.IsMatch(sku, "[0-9]"))
                    {
                        // 有货号则去除商家编码中的货号，使用小货号
                        //// 取出小货号
                        //var list = Regex.Matches(sku, "[0-9]", RegexOptions.IgnoreCase);
                        //var tempSku = "";
                        //for (int i = list.Count - 1; i >= 0; i--)
                        //{
                        //    tempSku = list[i] + tempSku;
                        //}
                        //tempCode = Regex.Replace(tempCode, "[0-9]", "", RegexOptions.IgnoreCase);
                    }
                    else
                    {
                        // 取出大货号，拼接到sku上
                        var list = Regex.Matches(tempCode, "[0-9]", RegexOptions.IgnoreCase);
                        for (int i = list.Count - 1; i >= 0; i--)
                        {
                            sku = list[i] + sku;
                        }

                    }
                    // 去除商家编码中的大货号
                    tempCode = Regex.Replace(tempCode, "[0-9]", "", RegexOptions.IgnoreCase);
                    listOrder.Add(new Order()
                    {
                        Id = Utils.StrToInt(row["column0"].ToString(), 0),
                        Code = tempCode,
                        Sku = sku,
                        Size = tempSize.Split(new char[] { ',' })[1].ToString().IndexOf('[') > -1 ? tempSize.Split(new char[] { ',' })[1].ToString().Substring(0, tempSize.Split(new char[] { ',' })[1].ToString().IndexOf('[')) : tempSize.Split(new char[] { ',' })[1].ToString(),
                        Num = Utils.StrToInt(row["数量"].ToString(), 0),
                        Total = Utils.StrToInt(row["合计"].ToString(), 0)
                    });

                }
                var tempListOrder = new List<Order>();
                // 列表排序
                //1， 计算出共有多少商家
                var codeList = listOrder.Select(c => c.Code).Distinct();
                // 2，取出所有同一商家下的sku
                foreach (var code in codeList)
                {
                    var skuList = listOrder.FindAll(c => c.Code == code);
                    // 2-1, 计算出此商家下面有多少货号
                    var skus = skuList.Select(c => c.Sku).Distinct();
                    // 2-2，同一商家，同一货号商品
                    foreach (var sku in skus)
                    {
                        var sizes = skuList.FindAll(c => c.Sku == sku);
                        // 2-3，按照指定顺序排列
                        foreach (var sortItem in Sort)
                        {
                            var obj = sizes.Find(c => c.Size == sortItem);
                            if (null != obj)
                            {
                                tempListOrder.Add(obj);
                            }
                        }
                    }
                }
                res.count = tempListOrder.Count;
                //res.data = tempListOrder.Skip((page-1)*10).Take(10).ToList();            
                res.data = tempListOrder;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}