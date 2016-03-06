using FireflySoft.DbUtility;
using FireflySoft.DbUtility.ModelQuery;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Demo.ModelQuery.DAL
{
    /// <summary>
    /// SysUser
    /// </summary>
    public partial class SysUserDAL
    {
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from SysUser");
            strSql.Append(" where ");
            strSql.Append(" ID = @ID  ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)
			};
            parameters[0].Value = ID;

            return SQLServerHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Demo.ModelQuery.Model.SysUserModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into SysUser(");
            strSql.Append("[dr],[ts],[UserName],[Password],[RealName],[Phone],[Mobile],[Email],[Status],[Description]");
            strSql.Append(") values (");
            strSql.Append("@dr,@ts,@UserName,@Password,@RealName,@Phone,@Mobile,@Email,@Status,@Description");
            strSql.Append(") ");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
			            new SqlParameter("@dr", SqlDbType.Bit,1) ,            
                        new SqlParameter("@ts", SqlDbType.DateTime) ,            
                        new SqlParameter("@UserName", SqlDbType.NVarChar,20) ,            
                        new SqlParameter("@Password", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@RealName", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Phone", SqlDbType.VarChar,20) ,            
                        new SqlParameter("@Mobile", SqlDbType.VarChar,20) ,            
                        new SqlParameter("@Email", SqlDbType.VarChar,80) ,            
                        new SqlParameter("@Status", SqlDbType.Int,4) ,            
                        new SqlParameter("@Description", SqlDbType.NVarChar,200)             
              
            };

            parameters[0].Value = model.dr;
            parameters[1].Value = model.ts;
            parameters[2].Value = model.UserName;
            parameters[3].Value = model.Password;
            parameters[4].Value = model.RealName;
            parameters[5].Value = model.Phone;
            parameters[6].Value = model.Mobile;
            parameters[7].Value = model.Email;
            parameters[8].Value = model.Status;
            parameters[9].Value = model.Description;

            object obj = SQLServerHelper.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {

                return Convert.ToInt32(obj);

            }
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Demo.ModelQuery.Model.SysUserModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update SysUser set ");

            strSql.Append(" [dr] = @dr , ");
            strSql.Append(" [ts] = @ts , ");
            strSql.Append(" [UserName] = @UserName , ");
            strSql.Append(" [Password] = @Password , ");
            strSql.Append(" [RealName] = @RealName , ");
            strSql.Append(" [Phone] = @Phone , ");
            strSql.Append(" [Mobile] = @Mobile , ");
            strSql.Append(" [Email] = @Email , ");
            strSql.Append(" [Status] = @Status , ");
            strSql.Append(" [Description] = @Description  ");
            strSql.Append(" where ID=@ID ");

            SqlParameter[] parameters = {
			            new SqlParameter("@ID", SqlDbType.Int,4) ,            
                        new SqlParameter("@dr", SqlDbType.Bit,1) ,            
                        new SqlParameter("@ts", SqlDbType.DateTime) ,            
                        new SqlParameter("@UserName", SqlDbType.NVarChar,20) ,            
                        new SqlParameter("@Password", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@RealName", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Phone", SqlDbType.VarChar,20) ,            
                        new SqlParameter("@Mobile", SqlDbType.VarChar,20) ,            
                        new SqlParameter("@Email", SqlDbType.VarChar,80) ,            
                        new SqlParameter("@Status", SqlDbType.Int,4) ,            
                        new SqlParameter("@Description", SqlDbType.NVarChar,200)             
              
            };

            parameters[0].Value = model.ID;
            parameters[1].Value = model.dr;
            parameters[2].Value = model.ts;
            parameters[3].Value = model.UserName;
            parameters[4].Value = model.Password;
            parameters[5].Value = model.RealName;
            parameters[6].Value = model.Phone;
            parameters[7].Value = model.Mobile;
            parameters[8].Value = model.Email;
            parameters[9].Value = model.Status;
            parameters[10].Value = model.Description;
            int rows = SQLServerHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from SysUser ");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)
			};
            parameters[0].Value = ID;


            int rows = SQLServerHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string IDlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from SysUser ");
            strSql.Append(" where ID in (" + IDlist + ")  ");
            int rows = SQLServerHelper.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        private List<Demo.ModelQuery.Model.SysUserModel> DataTableToList(DataTable dt)
        {
            List<Demo.ModelQuery.Model.SysUserModel> modelList = new List<Demo.ModelQuery.Model.SysUserModel>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                Demo.ModelQuery.Model.SysUserModel model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new Demo.ModelQuery.Model.SysUserModel();
                    if (dt.Rows[n]["ID"].ToString() != "")
                    {
                        model.ID = int.Parse(dt.Rows[n]["ID"].ToString());
                    }
                    if (dt.Rows[n]["dr"].ToString() != "")
                    {
                        if ((dt.Rows[n]["dr"].ToString() == "1") || (dt.Rows[n]["dr"].ToString().ToLower() == "true"))
                        {
                            model.dr = true;
                        }
                        else
                        {
                            model.dr = false;
                        }
                    }
                    if (dt.Rows[n]["ts"].ToString() != "")
                    {
                        model.ts = DateTime.Parse(dt.Rows[n]["ts"].ToString());
                    }
                    model.UserName = dt.Rows[n]["UserName"].ToString();
                    model.Password = dt.Rows[n]["Password"].ToString();
                    model.RealName = dt.Rows[n]["RealName"].ToString();
                    model.Phone = dt.Rows[n]["Phone"].ToString();
                    model.Mobile = dt.Rows[n]["Mobile"].ToString();
                    model.Email = dt.Rows[n]["Email"].ToString();
                    if (dt.Rows[n]["Status"].ToString() != "")
                    {
                        model.Status = int.Parse(dt.Rows[n]["Status"].ToString());
                    }
                    model.Description = dt.Rows[n]["Description"].ToString();


                    modelList.Add(model);
                }
            }
            return modelList;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Demo.ModelQuery.Model.SysUserModel GetModel(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append("  from SysUser ");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)
			};
            parameters[0].Value = ID;


            Demo.ModelQuery.Model.SysUserModel model = new Demo.ModelQuery.Model.SysUserModel();
            DataSet ds = SQLServerHelper.Query(strSql.ToString(), parameters);

            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataTableToList(ds.Tables[0])[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取符合查询条件的全部数据
        /// </summary>
        public List<Demo.ModelQuery.Model.SysUserModel> GetList(Demo.ModelQuery.Model.SysUserQueryModel query)
        {
            // 从查询条件获取SQL条件语句
            string strWhere = ConditionToSql.ToSqlText(query.Condition);
            SqlParameter[] paras = ConditionToSql.ToSqlParas(query.Condition);

            // 构造SQL查询语句
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append(" FROM SysUser ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            // 执行查询
            DataSet ds = SQLServerHelper.Query(strSql.ToString(), paras);

            // 转换查询结果为List<T>,并返回
            return DataTableToList(ds.Tables[0]);
        }

        /// <summary>
        /// 获取符合查询条件的前几行数据
        /// </summary>
        public List<Demo.ModelQuery.Model.SysUserModel> GetList(int Top, Demo.ModelQuery.Model.SysUserQueryModel query, string filedOrder)
        {
            // 从查询条件获取SQL条件语句
            string strWhere = ConditionToSql.ToSqlText(query.Condition);
            SqlParameter[] paras = ConditionToSql.ToSqlParas(query.Condition);

            // 构造SQL查询语句
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" * ");
            strSql.Append(" FROM SysUser ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);


            // 执行查询
            DataSet ds = SQLServerHelper.Query(strSql.ToString(), paras);

            // 转换查询结果为List<T>,并返回
            return DataTableToList(ds.Tables[0]);
        }

        /// <summary>
        /// 获取符合查询条件的全部记录数
        /// </summary>
        public int InfoCount(Demo.ModelQuery.Model.SysUserQueryModel query)
        {
            // 从查询条件获取SQL条件语句
            string strWhere = ConditionToSql.ToSqlText(query.Condition);
            SqlParameter[] paras = ConditionToSql.ToSqlParas(query.Condition);

            return InfoCount(strWhere, paras);
        }
        /// <summary>
        /// 获得记录数，不包含逻辑删除记录
        /// </summary>
        private int InfoCount(string strWhere, SqlParameter[] paras)
        {
            string strTableName = "SysUser";
            return SQLServerHelper.GetCount(strTableName, strWhere, paras);
        }

        /// <summary>
        /// 获取符合查询条件的分页数据
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="page">分页条件</param>
        /// <returns></returns>
        public List<Demo.ModelQuery.Model.SysUserModel> GetList(Demo.ModelQuery.Model.SysUserQueryModel query, string sortFiled, ref PageInfo page)
        {
            //if(string.IsNullOrEmpty(sortFiled)){
            //	sortFiled="AddTime DESC";
            //}

            // 从查询条件获取SQL条件语句
            string strWhere = ConditionToSql.ToSqlText(query.Condition);
            SqlParameter[] paras = ConditionToSql.ToSqlParas(query.Condition);

            // 总记录数
            page.RecordCount = InfoCount(strWhere, paras);

            // 分页数据
            string strSQL = PageQueryHelper.GetPageSQL(page.CurrentPage, page.PageSize, strWhere, sortFiled, "SysUser", "ID");
            DataSet ds = SQLServerHelper.Query(strSQL, paras);

            return DataTableToList(ds.Tables[0]);
        }
    }
}
