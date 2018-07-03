using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LY.MQCS.Plugin.DBService.PQ;

namespace LY.MQCS.Plugin.DBService.Test
{
    class Program
    {
        static void Main(string[] args)
        {

            var grid = new EasyUIDataGrid()
            {
                CODE = "QUES_DISTRIBUTE",
                SQL = @"select * from V1_ALL_QUES
            where 1=1
            #isNotNullOrEmpty<param1,sql{ and 问题点 like :param1 }>",
                LoadDataAfterInital = true,
                Columns = new List<EasyUIDataGridColumn>() {
                                    new EasyUIDataGridColumn(){
                                          Config = "{ field: 'DISTRI_GROUP_CODE', title: '分发目标班组', width: 180 }"
                                    },
                                    new EasyUIDataGridColumn(){
                                          Config = @"{ field: 'EFFECT_CONF_SCHE', title: '效果确认方案', width: 180,
                                                        formatter: function(value, row) {
                                                                return row.DISTRI_GROUP_CODE;
                                                        },
                                                        editor:{
                                                                type: 'combobox',
                                                                options:{
                                                                    valueField: 'EFFECT_CONF_SCHE',
                                                                    textField: 'NAME',
                                                                    required: true
                                                                 }
                                                         }
                                                     }",
                                           EditDataSQL = "select LOOKUP_VALUE_CODE EFFECT_CONF_SCHE,LOOKUP_VALUE_NAME EFFECT_CONF_SCHE_NAME from T_DB_DB_LOOKUP_VALUE where LOOKUP_TYPE_CODE='118_EFFECT_CONF_SCHE'"
                                    },
                                    new EasyUIDataGridColumn(){
                                          Config = "{ field: '问题点', title: '问题点', width: 180,sortable:true }"
                                    },
                                    new EasyUIDataGridColumn(){
                                          Config = "{ field: '发生日期', title: '发生日期', width: 60, align: 'right' }"
                                    },
                                    new EasyUIDataGridColumn(){
                                          Config = "{ field: '品情来源', title: '品情来源', width: 80 }"
                                    },
                                    new EasyUIDataGridColumn(){
                                          Config = "{ field: '车型', title: '车型', width: 80 }"
                                    },
                                    new EasyUIDataGridColumn(){
                                          Config = "{ field: '车号', title: '车号', width: 80 }"
                                    },
                                    new EasyUIDataGridColumn(){
                                          Config = "{ field: '不良区分', title: '不良区分', width: 80 }"
                                    }
                                    ,
                                    new EasyUIDataGridColumn(){
                                          Config = "{ field: '发生工站', title: '发生工站', width: 80 }"
                                    }
                                    ,
                                    new EasyUIDataGridColumn(){
                                          Config = "{ field: '责任人', title: '责任人', width: 80 }"
                                    }
                                    ,
                                    new EasyUIDataGridColumn(){
                                          Config = "{ field: '不良件数', title: '不良件数', width: 80 }"
                                    }
                                    ,
                                    new EasyUIDataGridColumn(){
                                          Config = "{ field: '是否再发', title: '是否再发', width: 80 }"
                                    }
                                }
            };

            using (var db = LYDBCommon.GetDB_PQ())
            {
                Console.WriteLine(db.EasyUIDataGrid.Add(grid));
                db.SaveChanges();
                //Console.WriteLine(db.EasyUIDataGrid.ToList().Count());
            }
            Console.Read();
        }
    }
}
