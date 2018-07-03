//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace LY.MQCS.Plugin.DBService.Bak.DB
{
    using System;
    using System.Collections.Generic;
    
    
    /// <summary>
    ///自动运行配置表
    /// </summary>
    public partial class T_DB_DB_AUTO_RUN
    {
        
    /// <summary>
    ///自动运行配置表ID:自动运行配置表ID
    /// </summary>
    public string AUTO_RUN_ID { get; set; }
        
    /// <summary>
    ///自动执行项名称:自动执行项名称
    /// </summary>
    public string AUTO_RUN_NAME { get; set; }
        
    /// <summary>
    ///自动执行年 :自动执行年，如：2014,2015
    /// </summary>
    public string RUN_YEAR { get; set; }
        
    /// <summary>
    ///自动执行月 :自动执行月，如：01,02
    /// </summary>
    public string RUN_MONTH { get; set; }
        
    /// <summary>
    ///自动执行日 :自动执行日，如：01,02。等于End时代表最后一天
    /// </summary>
    public string RUN_DAY { get; set; }
        
    /// <summary>
    ///自动执行时间 :自动执行时间，如：09:00,10:00
    /// </summary>
    public string RUN_TIME { get; set; }
        
    /// <summary>
    ///自动执行存储过程:自动执行存储过程
    /// </summary>
    public string PROC_NAME { get; set; }
        
    /// <summary>
    ///动态库:动态库
    /// </summary>
    public string DLL_NAME { get; set; }
        
    /// <summary>
    ///类名:类名
    /// </summary>
    public string CLASS_NAME { get; set; }
        
    /// <summary>
    ///方法名:方法名
    /// </summary>
    public string METHOD_NAME { get; set; }
        
    /// <summary>
    ///备注:备注
    /// </summary>
    public string REMARK { get; set; }
        
    /// <summary>
    ///执行间隔（秒） :执行间隔（秒），如果同时设置了此字段和run_time字段，则以此字段设置执行
    /// </summary>
    public decimal RUN_INTERVAL { get; set; }
        
    /// <summary>
    ///是否启用:是否启用，默认1
    /// </summary>
    public string IS_START { get; set; }
        
    /// <summary>
    ///存储过程是否有参数 :存储过程是否有参数，默认0,0表示没有参数；1表示有参数且参数为p_res out integer, p_showmsg  out varchar2,p_writemsg out varchar2
    /// </summary>
    public string IS_PROC_HAVE_PARA { get; set; }
        
    /// <summary>
    ///DLL方法参数 :DLL方法参数：格式参数1,参数2,参数3......
    /// </summary>
    public string DLL_PARA { get; set; }
        
    /// <summary>
    ///排序号
    /// </summary>
    public decimal ORDER_NO { get; set; }
        
    /// <summary>
    ///是否可用
    /// </summary>
    public string IS_ENABLE { get; set; }
        
    /// <summary>
    ///创建人
    /// </summary>
    public string CREATOR { get; set; }
        
    /// <summary>
    ///创建日期
    /// </summary>
    public System.DateTime CREATED_DATE { get; set; }
        
    /// <summary>
    ///最后更新人员
    /// </summary>
    public string MODIFIER { get; set; }
        
    /// <summary>
    ///最后更新时间
    /// </summary>
    public System.DateTime LAST_UPDATED_DATE { get; set; }
        
    /// <summary>
    ///SDP用户ID
    /// </summary>
    public string SDP_USER_ID { get; set; }
        
    /// <summary>
    ///SDP组织ID
    /// </summary>
    public string SDP_ORG_ID { get; set; }
        
    /// <summary>
    ///并发控制字段
    /// </summary>
    public string UPDATE_CONTROL_ID { get; set; }
    }
}