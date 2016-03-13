using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlTypes;
using FireflySoft.DbUtility.ModelQuery;

namespace Demo.SQLServerModelQuery.Model
{

    /// <summary>
    /// SysUser
    /// </summary>
    public partial class SysUserQueryModel
    {
        private List<QueryCondition> m_ConditionList = null;
        private QueryCondition[] m_Condition = null;

        public SysUserQueryModel()
        {
            m_ConditionList = new List<QueryCondition>();
        }

        /// <summary>
        /// 查询条件
        /// </summary>
        public QueryCondition[] Condition
        {
            get
            {
                return m_ConditionList.ToArray();
            }

            set
            {
                // 从客户端传递过来的QueryCondition，首先加入到List中
                m_Condition = value;

                if (value != null)
                {
                    foreach (QueryCondition temp in value)
                    {
                        m_ConditionList.Add(temp);
                    }
                }
            }
        }

        /// <summary>
        /// ID
        /// </summary>
        public int? ID
        {
            get
            {
                var con = m_ConditionList.Where(t => t.ConditionName == "ID").FirstOrDefault();
                if (con != null)
                {
                    return (int)con.Value;
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
                        Property = "ID",
                        ConditionName = "ID",
                        Value = value
                    });
                }
            }
        }

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

        /// <summary>
        /// Password
        /// </summary>
        public string Password
        {
            get
            {
                var con = m_ConditionList.Where(t => t.ConditionName == "Password").FirstOrDefault();
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
                        Property = "Password",
                        ConditionName = "Password",
                        Value = value
                    });
                }
            }
        }

        /// <summary>
        /// RealName
        /// </summary>
        public string RealName
        {
            get
            {
                var con = m_ConditionList.Where(t => t.ConditionName == "RealName").FirstOrDefault();
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
                        Property = "RealName",
                        ConditionName = "RealName",
                        Value = value
                    });
                }
            }
        }

        /// <summary>
        /// Phone
        /// </summary>
        public string Phone
        {
            get
            {
                var con = m_ConditionList.Where(t => t.ConditionName == "Phone").FirstOrDefault();
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
                        Property = "Phone",
                        ConditionName = "Phone",
                        Value = value
                    });
                }
            }
        }

        /// <summary>
        /// Mobile
        /// </summary>
        public string Mobile
        {
            get
            {
                var con = m_ConditionList.Where(t => t.ConditionName == "Mobile").FirstOrDefault();
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
                        Property = "Mobile",
                        ConditionName = "Mobile",
                        Value = value
                    });
                }
            }
        }

        /// <summary>
        /// Email
        /// </summary>
        public string Email
        {
            get
            {
                var con = m_ConditionList.Where(t => t.ConditionName == "Email").FirstOrDefault();
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
                        Property = "Email",
                        ConditionName = "Email",
                        Value = value
                    });
                }
            }
        }

        /// <summary>
        /// Status
        /// </summary>
        public int? Status
        {
            get
            {
                var con = m_ConditionList.Where(t => t.ConditionName == "Status").FirstOrDefault();
                if (con != null)
                {
                    return (int)con.Value;
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
                        Property = "Status",
                        ConditionName = "Status",
                        Value = value
                    });
                }
            }
        }

        /// <summary>
        /// Description
        /// </summary>
        public string Description
        {
            get
            {
                var con = m_ConditionList.Where(t => t.ConditionName == "Description").FirstOrDefault();
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
                        Property = "Description",
                        ConditionName = "Description",
                        Value = value
                    });
                }
            }
        }

        /// <summary>
        /// dr
        /// </summary>
        public bool? dr
        {
            get
            {
                var con = m_ConditionList.Where(t => t.ConditionName == "dr").FirstOrDefault();
                if (con != null)
                {
                    return (bool)con.Value;
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
                        Property = "dr",
                        ConditionName = "dr",
                        Value = value
                    });
                }
            }
        }

        /// <summary>
        /// ts
        /// </summary>		
        public DateTime? minTs
        {
            get
            {
                var con = m_ConditionList.Where(t => t.ConditionName == "ts").Where(t => t.CompareType == CompareType.GET).FirstOrDefault();
                if (con != null)
                {
                    return (DateTime)con.Value;
                }

                return null;
            }

            set
            {
                if (value != null)
                {
                    m_ConditionList.Add(new QueryCondition()
                    {
                        CompareType = CompareType.GET,
                        LinkType = LinkType.And,
                        Property = "ts",
                        ConditionName = "ts",
                        Value = value
                    });
                }
            }
        }

        /// <summary>
        /// ts
        /// </summary>		
        public DateTime? maxTs
        {
            get
            {
                var con = m_ConditionList.Where(t => t.ConditionName == "ts").Where(t => t.CompareType == CompareType.LET).FirstOrDefault();
                if (con != null)
                {
                    return (DateTime)con.Value;
                }

                return null;
            }
            set
            {
                if (value != null)
                {
                    m_ConditionList.Add(new QueryCondition()
                    {
                        CompareType = CompareType.LET,
                        LinkType = LinkType.And,
                        Property = "ts",
                        ConditionName = "ts",
                        Value = value
                    });
                }
            }
        }
    }
}