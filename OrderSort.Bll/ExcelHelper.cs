using OrderSort.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSort.Bll
{
    public class ExcelHelper
    {
        /// <summary>
        ///  导出excel
        /// </summary>
        public void DownloadExcel()
        {
            DataTable dt = GetData();
            if (dt.Rows.Count > 0)
            {
                DataTable newDt = new DataTable();
                newDt.Columns.Add(new DataColumn("序号", typeof(string)));
                newDt.Columns[0].Caption = "序号";
                newDt.Columns.Add(new DataColumn("小区名称", typeof(string)));
                newDt.Columns[1].Caption = "小区名称";
                newDt.Columns.Add(new DataColumn("建筑面积(㎡)", typeof(string)));
                newDt.Columns[2].Caption = "建筑面积(㎡)";
                newDt.Columns.Add(new DataColumn("评估机构", typeof(string)));
                newDt.Columns[3].Caption = "评估机构|12";
                newDt.Columns.Add(new DataColumn("评估单价(元/㎡)", typeof(string)));
                newDt.Columns[4].Caption = "评估单价(元/㎡)";
                newDt.Columns.Add(new DataColumn("评估值(万元)", typeof(string)));
                newDt.Columns[5].Caption = "评估值(万元)";
                newDt.Columns.Add(new DataColumn("查询人", typeof(string)));
                newDt.Columns[6].Caption = "查询人";
                newDt.Columns.Add(new DataColumn("所属城市", typeof(string)));
                newDt.Columns[7].Caption = "所属城市";
                newDt.Columns.Add(new DataColumn("评估日期", typeof(string)));
                newDt.Columns[8].Caption = "评估日期";
                int i = 0;
                foreach (DataRow row in dt.Rows)
                {
                    i++;
                    DataRow dr = newDt.NewRow();
                    dr["序号"] = i.ToString();
                    dr["小区名称"] = row["CommunityName"] != null ? row["CommunityName"].ToString() : "";
                    dr["建筑面积(㎡)"] = row["BuildingArea"] != null ? row["BuildingArea"].ToString() : "";
                    dr["评估机构"] = row["EvalOrg"] != null ? row["EvalOrg"].ToString() : "";
                    dr["评估单价(元/㎡)"] = row["Price"] != null ? row["Price"].ToString() : "";
                    dr["评估值(万元)"] = row["Evaluation"] != null ? row["Evaluation"] : 0.00;
                    dr["查询人"] = row["Creator"] != null ? row["Creator"].ToString() : "";
                    dr["所属城市"] = row["DataName"] != null ? row["DataName"].ToString() : "";
                    dr["评估日期"] = row["CreateDate"] != null ? row["CreateDate"].ToString() : "";
                    newDt.Rows.Add(dr);
                }
                try
                {
                    ExcelExportUtils.ExportDataTable2ExcelByNPOI(newDt, "我司查询统计" + DateTime.Now.ToString("yyyyMMddHHmmss"), false, "");
                    //CreateExcel_t(newDt, "我司查询统计" + DateTime.Now.ToString("yyyyMMddHHmmss"));
                }
                catch (Exception ex)
                {

                }

            }
        }
        /// <summary>
        ///  获取导出数据
        /// </summary>
        /// <returns></returns>
        private DataTable GetData()
        {
            string sql = "select a.CommunityName,CONVERT(decimal(18,2),a.BuildingArea) as BuildingArea,a.EvalOrg,a.Price,CONVERT(decimal(18,2),a.Evaluation/10000) as Evaluation,a.Creator,b.DataName,a.CreateDate from Temp_Evaluation a left join DataDictionary b on a.CityId=b.DataId";
            //string strWhere = GetSearchStr();
            //sql = sql + " where " + strWhere + " order by a.Id desc";
            DataTable dt = new DataTable();
            return dt;
        }
        /// <summary>
        ///  读取excel数据到datatable
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static DataTable ReadExcelToDataTable(string filePath)
        {
            //DataTable dt = ExcelExportUtils.ReadExcelToDataTable(filePath,null,true);
            DataTable dt = ExcelExportUtils.ExcelToDataTableFormPath(true, filePath);
            return dt;
        }
    }
}
