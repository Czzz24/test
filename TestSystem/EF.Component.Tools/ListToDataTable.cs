using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Collections;

namespace EF.Component.Tools
{
    public class ListToDataTable<T>
    {
        public DataTable IListOut(IList<T> ResList)
        {
            DataTable TempDT = new DataTable();

            //此处遍历IList的结构并建立同样的DataTable
            System.Reflection.PropertyInfo[] p = ResList[0].GetType().GetProperties();

            foreach (System.Reflection.PropertyInfo pi in p)
            {
                TempDT.Columns.Add(pi.Name, System.Type.GetType(pi.PropertyType.ToString()));
            }

            for (int i = 0; i < ResList.Count; i++)
            {
                ArrayList TempList = new ArrayList();
                //将IList中的一条记录写入ArrayList
                foreach (System.Reflection.PropertyInfo pi in p)
                {
                    object oo = pi.GetValue(ResList[i], null);
                    TempList.Add(oo);
                }

                object[] itm = new object[p.Length];
                //遍历ArrayList向object[]里放数据
                for (int j = 0; j < TempList.Count; j++)
                {
                    itm.SetValue(TempList[j], j);
                }
                //将object[]的内容放入DataTable
                TempDT.LoadDataRow(itm, true);
            }
            //返回DataTable
            return TempDT;
        }
    }
}
