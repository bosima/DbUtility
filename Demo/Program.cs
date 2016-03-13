using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            //Demo.SQLServerModelQuery.DAL.SysUserDAL dal = new Demo.SQLServerModelQuery.DAL.SysUserDAL();
            //var list = dal.GetList(new Demo.SQLServerModelQuery.Model.SysUserQueryModel()
            //{
            //    Status = 1
            //});
            //Console.WriteLine("SQLServer 查询结果数量：" + list.Count);


            Demo.MySQLModelQuery.DAL.SysUserDAL mySQLDAL = new Demo.MySQLModelQuery.DAL.SysUserDAL();

            //var userID = mySQLDAL.Add(new MySQLModelQuery.Model.SysUserModel()
            //{
            //    Description = "备注4",
            //    dr = false,
            //    Email = "houliu@126.com",
            //    Mobile = "13823455432",
            //    Password = "666666",
            //    Phone = "021-1212122",
            //    RealName = "猴六",
            //    Status = 1,
            //    ts = DateTime.Now,
            //    UserName = "houliu"
            //});
            //Console.WriteLine("新用户ID：" + userID);

            var result = mySQLDAL.Update(new MySQLModelQuery.Model.SysUserModel()
            {
                ID = 4,
                Description = "备注4",
                dr = false,
                Email = "houliu@126.com",
                Mobile = "13823455432",
                Password = "666666",
                Phone = "021-1212122",
                RealName = "猴六",
                Status = 0,
                ts = DateTime.Now,
                UserName = "houliu"
            });
            Console.WriteLine("更新成功");

            var mySQLList = mySQLDAL.GetList(new Demo.MySQLModelQuery.Model.SysUserQueryModel()
            {
                Status = 1
            });
            Console.WriteLine("MySQL 查询结果数量：" + mySQLList.Count);

            Console.ReadLine();
        }
    }
}
