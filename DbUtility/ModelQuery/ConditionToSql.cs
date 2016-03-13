using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace FireflySoft.DbUtility.ModelQuery
{
    /// <summary>
    /// 将QueryCondition数组转换为SQLServer查询语句
    /// </summary>
    public class ConditionToSql
    {
        /// <summary>
        /// 转换查询条件数组为SqlServer文本
        /// </summary>
        /// <param name="conditionArray"></param>
        /// <returns></returns>
        public static string ToSqlText(QueryCondition[] conditionArray)
        {
            return ToDbSqlText(conditionArray, "[]");
        }

        /// <summary>
        /// 转换查询条件数组为MySql文本
        /// </summary>
        /// <param name="conditionArray"></param>
        /// <returns></returns>
        public static string ToMySqlText(QueryCondition[] conditionArray)
        {
            return ToDbSqlText(conditionArray, "``");
        }

        /// <summary>
        /// 转换查询条件数组为Sql文本
        /// </summary>
        /// <param name="conditionArray"></param>
        /// <returns></returns>
        public static string ToDbSqlText(QueryCondition[] conditionArray, string escapeSymbol)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (conditionArray != null && conditionArray.Length > 0)
            {
                Dictionary<string, int> parameterDictionary = new Dictionary<string, int>();
                for (int i = 0; i < conditionArray.Length; i++)
                {
                    QueryCondition conditon = conditionArray[i];
                    stringBuilder.Append(ConditionToSql.ToSubSqlText(conditon, parameterDictionary, escapeSymbol));
                }

                parameterDictionary.Clear();
                return ConditionToSql.FixSQLText(stringBuilder.ToString());
            }

            return string.Empty;
        }

        /// <summary>
        /// 转换查询条件数组为SqlServer参数
        /// </summary>
        /// <param name="conditionArray"></param>
        /// <returns></returns>
        public static SqlParameter[] ToSqlParas(QueryCondition[] conditionArray)
        {
            var paras = ToDbSqlParas(conditionArray, DbType.SQLServer);
            var sqlParas = new SqlParameter[paras.Length];

            for (int i = 0; i < paras.Length; i++)
            {
                sqlParas[i] = (SqlParameter)paras[i];
            }

            return sqlParas;
        }

        /// <summary>
        /// 转换查询条件数组为MySql参数
        /// </summary>
        /// <param name="conditionArray"></param>
        /// <returns></returns>
        public static MySqlParameter[] ToMySqlParas(QueryCondition[] conditionArray)
        {
            var paras = ToDbSqlParas(conditionArray, DbType.MySQL);
            var mySQLParas = new MySqlParameter[paras.Length];

            for (int i = 0; i < paras.Length; i++)
            {
                mySQLParas[i] = (MySqlParameter)paras[i];
            }

            return mySQLParas;
        }

        /// <summary>
        /// 转换查询条件数组为基类Db参数
        /// </summary>
        /// <param name="conditionArray"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        private static DbParameter[] ToDbSqlParas(QueryCondition[] conditionArray, DbType dbType)
        {
            List<DbParameter> parameterList = new List<DbParameter>();

            if (conditionArray != null && conditionArray.Length > 0)
            {
                Dictionary<string, int> parameterDictionary = new Dictionary<string, int>();
                for (int i = 0; i < conditionArray.Length; i++)
                {
                    QueryCondition conditon = conditionArray[i];
                    ConditionToSql.ToSubSqlParas(conditon, parameterList, parameterDictionary, dbType);
                }

                parameterDictionary.Clear();
            }

            return parameterList.ToArray();
        }

        /// <summary>
        /// 修正SQL语句，去掉SQL语句前的条件连接类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string FixSQLText(string str)
        {
            string text = str.Trim();
            if (text.StartsWith(LinkType.And.ToString()))
            {
                text = text.Substring(3);
            }
            else
            {
                if (text.StartsWith(LinkType.Or.ToString()))
                {
                    text = text.Substring(2);
                }
            }
            return text.Trim();
        }

        /// <summary>
        /// 转换单个查询条件为Sql文本
        /// </summary>
        /// <param name="conditon"></param>
        /// <param name="parameterDictionary"></param>
        /// <returns></returns>
        private static string ToSubSqlText(QueryCondition conditon, Dictionary<string, int> parameterDictionary, string escapeSymbol)
        {
            if (conditon != null && ConditionToSql.CheckConditionIsValid(conditon))
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(" ");
                stringBuilder.Append(ConditionToSql.GetLinkType(conditon.LinkType));
                stringBuilder.Append(" ");

                // 如果有子级查询条件，则遍历构造子级查询条件
                if (conditon.SubQuery != null && conditon.SubQuery.Length > 0)
                {
                    string subSqlText = string.Empty;
                    QueryCondition[] subQuery = conditon.SubQuery;

                    for (int i = 0; i < subQuery.Length; i++)
                    {
                        QueryCondition subConditon = subQuery[i];
                        subSqlText += ConditionToSql.ToSubSqlText(subConditon, parameterDictionary, escapeSymbol);
                    }

                    if (!string.IsNullOrEmpty(subSqlText))
                    {
                        stringBuilder.Append("(");
                        stringBuilder.Append(ConditionToSql.FixSQLText(subSqlText));
                        stringBuilder.Append(")");
                    }
                }
                else
                {
                    stringBuilder.Append(escapeSymbol[0]);
                    stringBuilder.Append(conditon.Property.ToString());
                    stringBuilder.Append(escapeSymbol[1]);
                    stringBuilder.Append(" ");
                    stringBuilder.Append(ConditionToSql.GetCompareType(conditon));
                    stringBuilder.Append(" ");

                    if (conditon.CompareType == CompareType.In || conditon.CompareType == CompareType.NotIn)
                    {
                        stringBuilder.Append("(");
                    }

                    stringBuilder.Append(ConditionToSql.GetParameterName(conditon, parameterDictionary));

                    if (conditon.CompareType == CompareType.In || conditon.CompareType == CompareType.NotIn)
                    {
                        stringBuilder.Append(")");
                    }
                }

                return stringBuilder.ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// 转换单个查询条件为Sql参数
        /// </summary>
        /// <param name="conditon"></param>
        /// <param name="parameterList"></param>
        /// <param name="parameterDictionary"></param>
        private static void ToSubSqlParas(QueryCondition conditon, List<DbParameter> parameterList, Dictionary<string, int> parameterDictionary, DbType dbType)
        {
            if (conditon != null && ConditionToSql.CheckConditionIsValid(conditon))
            {
                if (conditon.SubQuery != null && conditon.SubQuery.Length > 0)
                {
                    QueryCondition[] subQuery = conditon.SubQuery;
                    for (int i = 0; i < subQuery.Length; i++)
                    {
                        QueryCondition subConditon = subQuery[i];
                        ConditionToSql.ToSubSqlParas(subConditon, parameterList, parameterDictionary, dbType);
                    }
                }

                if (!string.IsNullOrEmpty(conditon.Property.ToString()))
                {
                    ConditionToSql.GetParameterObject(conditon, parameterList, parameterDictionary, dbType);
                }
            }
        }

        /// <summary>
        /// 获取单个查询条件对应的参数名称
        /// </summary>
        /// <param name="conditon"></param>
        /// <param name="parameterDictionary"></param>
        /// <returns></returns>
        private static string GetParameterName(QueryCondition conditon, Dictionary<string, int> parameterDictionary)
        {
            var parameterName = string.Empty;
            var conditionName = conditon.ConditionName.ToString();
            bool isArray = false;
            if (conditon.Value != null)
            {
                isArray = conditon.Value.GetType().IsArray;
            }

            if (isArray)
            {
                var conditonValueEnum = (System.Collections.IEnumerable)conditon.Value;

                foreach (var v in conditonValueEnum)
                {
                    var tmpParameterName = "@" + conditionName;
                    tmpParameterName = GetUniqueParameterName(tmpParameterName, parameterDictionary);

                    parameterName += tmpParameterName + ",";
                }

                if (!string.IsNullOrEmpty(parameterName))
                {
                    parameterName = parameterName.Substring(0, parameterName.Length - 1);
                }
            }
            else
            {
                if (conditon.CompareType == CompareType.GET || conditon.CompareType == CompareType.GT)
                {
                    parameterName = "@min" + conditionName;
                }
                else if (conditon.CompareType == CompareType.LT || conditon.CompareType == CompareType.LET)
                {
                    parameterName = "@max" + conditionName;
                }
                else
                {
                    parameterName = "@" + conditionName;
                }

                parameterName = GetUniqueParameterName(parameterName, parameterDictionary);
            }

            return parameterName;
        }

        /// <summary>
        /// 获取唯一的参数名
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="parameterDictionary"></param>
        /// <returns></returns>
        private static string GetUniqueParameterName(string parameterName, Dictionary<string, int> parameterDictionary)
        {
            // 参数名如果已经存在，则在参数名之后添加编号
            if (parameterDictionary.ContainsKey(parameterName))
            {
                parameterDictionary[parameterName] = parameterDictionary[parameterName] + 1;
                parameterName += parameterDictionary[parameterName].ToString();
            }
            else
            {
                parameterDictionary.Add(parameterName, 0);
            }

            return parameterName;
        }

        /// <summary>
        /// 获取单个条件对应的参数值对象
        /// </summary>
        /// <param name="conditon"></param>
        /// <param name="parameterList"></param>
        /// <param name="parameterDictionary"></param>
        /// <returns></returns>
        private static string GetParameterObject(QueryCondition conditon, List<DbParameter> parameterList, Dictionary<string, int> parameterDictionary, DbType dbType)
        {
            string parameterName = string.Empty;
            string conditionName = conditon.ConditionName.ToString();
            bool isArray = false;
            if (conditon.Value != null)
            {
                isArray = conditon.Value.GetType().IsArray;
            }

            if (isArray)
            {
                var conditonValueEnum = (System.Collections.IEnumerable)conditon.Value;

                foreach (var v in conditonValueEnum)
                {
                    var tmpParameterName = "@" + conditionName;
                    tmpParameterName = GetUniqueParameterName(tmpParameterName, parameterDictionary);

                    parameterList.Add(ConditionToSql.CreateParameterObject(tmpParameterName, v, dbType));
                }
            }
            else
            {
                if (conditon.CompareType == CompareType.GET || conditon.CompareType == CompareType.GT)
                {
                    parameterName = "@min" + conditionName;
                }
                else
                {
                    if (conditon.CompareType == CompareType.LT || conditon.CompareType == CompareType.LET)
                    {
                        parameterName = "@max" + conditionName;
                    }
                    else
                    {
                        parameterName = "@" + conditionName;
                    }
                }

                parameterName = GetUniqueParameterName(parameterName, parameterDictionary);

                parameterList.Add(ConditionToSql.CreateParameterObject(parameterName, conditon.Value, dbType));
            }

            return parameterName;
        }

        /// <summary>
        /// 获取条件之间的连接类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static string GetLinkType(LinkType type)
        {
            switch (type)
            {
                case LinkType.And:
                    return "And";
                case LinkType.Or:
                    return "Or";
            }

            return string.Empty;
        }

        /// <summary>
        /// 获取条件的比较类型
        /// </summary>
        /// <param name="conditon"></param>
        /// <returns></returns>
        private static string GetCompareType(QueryCondition conditon)
        {
            if (conditon.Value == null)
            {
                if (CompareType.Equal == conditon.CompareType)
                {
                    conditon.CompareType = CompareType.Is;
                }
                else
                {
                    if (CompareType.NotEqual == conditon.CompareType)
                    {
                        conditon.CompareType = CompareType.IsNot;
                    }
                }
            }
            switch (conditon.CompareType)
            {
                case CompareType.Like:
                    return "like";
                case CompareType.GET:
                    return ">=";
                case CompareType.GT:
                    return ">";
                case CompareType.LT:
                    return "<";
                case CompareType.LET:
                    return "<=";
                case CompareType.Equal:
                    return "=";
                case CompareType.NotEqual:
                    return "<>";
                case CompareType.In:
                    return "in";
                case CompareType.NotIn:
                    return "not in";
                case CompareType.Is:
                    return "is";
                case CompareType.IsNot:
                    return "is not";
                default:
                    return "=";
            }
        }

        /// <summary>
        /// 判断单个查询条件是否有效
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        private static bool CheckConditionIsValid(QueryCondition condition)
        {
            return (condition.SubQuery != null && condition.SubQuery.Length > 0) || condition.Value != null || condition.CompareType == CompareType.Equal || condition.CompareType == CompareType.NotEqual || condition.CompareType == CompareType.Is || condition.CompareType == CompareType.IsNot;
        }

        /// <summary>
        /// 创建Parameter对象
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static DbParameter CreateParameterObject(string name, object value, DbType dbtype)
        {
            DbParameter para = null;
            switch (dbtype)
            {
                case DbType.SQLServer:
                    para = new SqlParameter(name, (value == null) ? DBNull.Value : value);
                    break;
                case DbType.MySQL:
                    para = new MySqlParameter(name, (value == null) ? DBNull.Value : value);
                    break;
            }

            return para;
        }
    }
}
