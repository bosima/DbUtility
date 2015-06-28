using Demo.ModelQuery.DAL;
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
            SysUserDAL dal = new SysUserDAL();
            var list = dal.GetList(new ModelQuery.Model.SysUserQueryModel()
            {
                Status = 1
            });

            Console.WriteLine("查询结果数量：" + list.Count);
            Console.ReadLine();
        }
    }
}
