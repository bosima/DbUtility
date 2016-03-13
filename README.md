# DbUtility
Relational database operation utility class library, support SQLServer and MySQL.

ModelQuery
通过定义一个查询类及其属性实现对特定业务数据的查询操作。

    /// <summary>
    /// SysUser
    /// </summary>
    public partial class SysUserQueryModel
    {
        ...
    }

1、每个属性代表一个查询条件，属性的值可以是一个单一的数据，也可以是一组数据：

UserName属性的值是一个单一的数据，表示UserName=value的查询条件。

        /// <summary>
        /// UserName
        /// </summary>
        public string UserName
        {
            get
            {
                var con = m_ConditionList.Where(t => t.ConditionName == "UserName").FirstOrDefault();
                if (con != null)
                {
                    return (string)con.Value;
                }

                return null;
            }

            set
            {
                if (value != null)
                {
                    m_ConditionList.Add(new QueryCondition()
                    {
                        CompareType = CompareType.Equal,
                        LinkType = LinkType.And,
                        Property = "UserName",
                        ConditionName = "UserName",
                        Value = value
                    });
                }
            }
        }

MutliStatus属性的值是一个int数组，表示Status in (value1,value2,...)的查询条件。

        /// <summary>
        /// MutliStatus
        /// </summary>
        public int[] MutliStatus
        {
            get
            {
                var con = m_ConditionList.Where(t => t.ConditionName == "MutliStatus").FirstOrDefault();
                if (con != null)
                {
                    return (int[])con.Value;
                }

                return null;
            }

            set
            {
                if (value != null)
                {
                    m_ConditionList.Add(new QueryCondition()
                    {
                        CompareType = CompareType.In,
                        LinkType = LinkType.And,
                        Property = "Status",
                        ConditionName = "MutliStatus",
                        Value = value
                    });
                }
            }
        }

2、每个查询条件可以定义查询比较的方式（等于、不等于、包含、不包含等），并在赋值时初始化：

请查看1中的属性赋值部分： CompareType = CompareType.In ，CompareType定义如下：

	public enum CompareType
	{
		Like,
		GET,
		GT,
		LT,
		LET,
		Equal,
		NotEqual,
		In,
		NotIn,
		Is,
		IsNot
	}

3、查询条件之间的关系（如And、Or等）由相关的属性定义，并在赋值时初始化：

请查看1中的属性赋值部分：LinkType = LinkType.And，LinkType定义如下：

	public enum LinkType
	{
		And,
		Or,
		Nothing
	}

4、可以对同一个数据字段定义不同的查询条件（也就是属性），方便在不同的需求下使用：

NoUserName属性针对UserName字段，表示UserName<>value的查询条件。

        /// <summary>
        /// NoUserName
        /// </summary>
        public string NoUserName
        {
            get
            {
                var con = m_ConditionList.Where(t => t.ConditionName == "NoUserName").FirstOrDefault();
                if (con != null)
                {
                    return (string)con.Value;
                }

                return null;
            }

            set
            {
                if (value != null)
                {
                    m_ConditionList.Add(new QueryCondition()
                    {
                        CompareType = CompareType.NotEqual,
                        LinkType = LinkType.And,
                        Property = "UserName",
                        ConditionName = "NoUserName",
                        Value = value
                    });
                }
            }
        }

5、最终根据查询类的实例生成参数化的查询条件和参数数组，提交给数据库操作类执行：

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
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), paras);

            // 转换查询结果为List<T>,并返回
            return DataTableToList(ds.Tables[0]);
        }

6、以上这些定义完毕，来看看怎么使用：

            SysUserDAL dal = new SysUserDAL();
            var list = dal.GetList(new ModelQuery.Model.SysUserQueryModel()
            {
                Status = 1
            });

            Console.WriteLine("查询结果数量：" + list.Count);
            Console.ReadLine();

方便安全高效，再也不用担心拼SQL的烦扰了。

但是实际情况中一条记录往往都有很多的查询条件，如果一个个定义岂不是累尿了。

目前有很多的代码生成方案可以帮我们解决这些问题，这里提供一个解决方案：使用基于数据表的动软代码生成器的自定义模板功能生成模型类、模型查询类、数据操作类，然后使用起来就像上边一样so easy了。

模板已经放到/Demo/Template目录下，可以根据自己的需要修改，模板语言使用C#编码风格，好用又easy。

