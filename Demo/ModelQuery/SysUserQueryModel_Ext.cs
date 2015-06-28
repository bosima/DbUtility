using FireflySoft.DbUtility.ModelQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo.ModelQuery.Model
{
    /// <summary>
    /// SysUser
    /// </summary>
    public partial class SysUserQueryModel
    {
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
    }
}
