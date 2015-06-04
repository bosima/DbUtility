using System;
using System.Text;

namespace FireflySoft.DbUtility.ModelQuery
{
    public class PageQueryHelper
    {
        /// <summary>
        /// 获取分页SQL语句
        /// </summary>
        /// <param name="currPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="sortField">排序字段</param>
        /// <param name="tableName">表名</param>
        /// <param name="pageField">分页字段</param>
        /// <returns></returns>
        public static string GetPageSQL(int currPage, int pageSize, string strWhere, string sortField, string tableName, string pageField)
        {
            var sql = GetPageSQL(currPage, pageSize, strWhere, sortField, "", tableName, pageField, "*");
            return sql;
        }

        /// <summary>
        /// 获取分页SQL语句
        /// </summary>
        /// <param name="currPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="sortField">排序字段</param>
        /// <param name="sorDirection">排序方向</param>
        /// <param name="tableName">表名</param>
        /// <param name="pageField">分页字段</param>
        /// <returns></returns>
        public static string GetPageSQL(int currPage, int pageSize, string strWhere, string sortField, string sorDirection, string tableName, string pageField)
        {
            var sql = GetPageSQL(currPage, pageSize, strWhere, sortField, sorDirection, tableName, pageField, "*");
            return sql;
        }

        /// <summary>
        /// 获取分页SQL语句
        /// </summary>
        /// <param name="currPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="sortField">排序字段</param>
        /// <param name="sorDirection">排序方向</param>
        /// <param name="tableName">表名</param>
        /// <param name="pageField">分页字段</param>
        /// <param name="selectCols">查询字段</param>
        /// <returns></returns>
        public static string GetPageSQL(int currPage, int pageSize, string strWhere, string sortField, string sorDirection, string tableName, string pageField, string selectCols)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("select");
            stringBuilder.Append(" top ");
            stringBuilder.Append(pageSize);
            stringBuilder.Append(" ");
            stringBuilder.Append(selectCols);
            stringBuilder.Append(" from ");
            stringBuilder.Append(tableName);
            string strWhere2 = GetPageCondition(currPage, pageSize, strWhere, sortField, sorDirection, tableName, pageField);
            if (strWhere2 != "")
            {
                stringBuilder.Append(" where ");
                stringBuilder.Append(strWhere2);
            }
            if (sortField != string.Empty)
            {
                stringBuilder.Append(" order by ");
                stringBuilder.Append(sortField);
                stringBuilder.Append(" ");
                stringBuilder.Append(sorDirection);
            }
            else
            {
                stringBuilder.Append(" order by ");
                stringBuilder.Append(pageField);
                stringBuilder.Append(" desc");
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 获取分页查询条件
        /// </summary>
        /// <param name="currPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="strWhere"></param>
        /// <param name="sortField"></param>
        /// <param name="sorDirection"></param>
        /// <param name="tableName"></param>
        /// <param name="pageField"></param>
        /// <returns></returns>
        private static string GetPageCondition(int currPage, int pageSize, string strWhere, string sortField, string sorDirection, string tableName, string pageField)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (currPage > 1)
            {
                if (strWhere.Trim() != "")
                {
                    stringBuilder.Append(strWhere);
                    stringBuilder.Append(" and ");
                }
                stringBuilder.Append(pageField);
                stringBuilder.Append(" not in (select top ");
                stringBuilder.Append((currPage - 1) * pageSize);
                stringBuilder.Append(" ");
                stringBuilder.Append(pageField);
                stringBuilder.Append(" from ");
                stringBuilder.Append(tableName);
                if (strWhere.Trim() != "")
                {
                    stringBuilder.Append(" where ");
                    stringBuilder.Append(strWhere);
                }
                if (sortField != string.Empty)
                {
                    stringBuilder.Append(" order by ");
                    stringBuilder.Append(sortField);
                    stringBuilder.Append(" ");
                    stringBuilder.Append(sorDirection);
                    stringBuilder.Append(")");
                }
                else
                {
                    stringBuilder.Append(" order by ");
                    stringBuilder.Append(pageField);
                    stringBuilder.Append(" desc)");
                }
                return stringBuilder.ToString();
            }
            return strWhere;
        }
    }
}
