using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FireflySoft.DbUtility.ModelQuery
{
    public class MySQLPageQueryHelper
    { /// <summary>
      /// 获取分页SQL语句
      /// </summary>
      /// <param name="currPage">当前页码</param>
      /// <param name="pageSize">分页大小</param>
      /// <param name="strWhere">查询条件</param>
      /// <param name="sortField">排序字段</param>
      /// <param name="tableName">表名</param>
      /// <param name="pageField">分页字段</param>
      /// <returns></returns>
        public static string GetPageSQL(int currPage, int pageSize, string strWhere, string sortField, string tableName)
        {
            var sql = GetPageSQL(currPage, pageSize, strWhere, sortField, "", tableName, "*");
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
        public static string GetPageSQL(int currPage, int pageSize, string strWhere, string sortField, string sorDirection, string tableName)
        {
            var sql = GetPageSQL(currPage, pageSize, strWhere, sortField, sorDirection, tableName, "*");
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
        public static string GetPageSQL(int currPage, int pageSize, string strWhere, string sortField, string sorDirection, string tableName, string selectCols)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("SELECT ");
            stringBuilder.Append(selectCols);
            stringBuilder.Append(" FROM ");
            stringBuilder.Append(tableName);

            if (!string.IsNullOrWhiteSpace(strWhere))
            {
                stringBuilder.Append(" WHERE ");
                stringBuilder.Append(strWhere);
            }

            if (!string.IsNullOrWhiteSpace(sortField))
            {
                stringBuilder.Append(" ORDER BY ");
                stringBuilder.Append(sortField);
            }

            if (!string.IsNullOrWhiteSpace(sorDirection))
            {
                stringBuilder.Append(" ");
                stringBuilder.Append(sorDirection);
                stringBuilder.Append(" ");
            }

            var startIndex = (currPage - 1) * pageSize;
            stringBuilder.Append(" LIMIT ");
            stringBuilder.Append(startIndex.ToString());
            stringBuilder.Append(",");
            stringBuilder.Append(pageSize.ToString());

            return stringBuilder.ToString();
        }
    }
}
