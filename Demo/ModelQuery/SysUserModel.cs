using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace Demo.ModelQuery.Model
{
    /// <summary>
    /// SysUser
    /// </summary>
    public partial class SysUserModel
    {
        #region 类属性

        /// <summary>
        /// ID
        /// </summary>
        public int ID { set; get; }

        /// <summary>
        /// UserName
        /// </summary>
        public string UserName { set; get; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { set; get; }

        /// <summary>
        /// RealName
        /// </summary>
        public string RealName { set; get; }

        /// <summary>
        /// Phone
        /// </summary>
        public string Phone { set; get; }

        /// <summary>
        /// Mobile
        /// </summary>
        public string Mobile { set; get; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { set; get; }

        /// <summary>
        /// Status
        /// </summary>
        public int? Status { set; get; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { set; get; }

        /// <summary>
        /// dr
        /// </summary>
        public bool dr { set; get; }

        /// <summary>
        /// ts
        /// </summary>
        public DateTime? ts { set; get; }


        /// <summary>
        /// 获取属性描述
        /// </summary>
        public Dictionary<string, string> GetPropertiesDescription()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("ID", "ID");
            dic.Add("UserName", "UserName");
            dic.Add("Password", "Password");
            dic.Add("RealName", "RealName");
            dic.Add("Phone", "Phone");
            dic.Add("Mobile", "Mobile");
            dic.Add("Email", "Email");
            dic.Add("Status", "Status");
            dic.Add("Description", "Description");
            dic.Add("dr", "dr");
            dic.Add("ts", "ts");
            return dic;
        }
        #endregion
    }
}