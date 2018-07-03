//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace LY.MQCS.Plugin.DBService.Bak.PQ
{
    using System;
    using System.Collections.Generic;
    
    
    /// <summary>
    ///车辆不良信息表
    /// </summary>
    public partial class T_PQ_BU_VE_BAD_INFO
    {
        
    /// <summary>
    ///车辆不良信息表ID
    /// </summary>
    public string VE_BAD_INFO_ID { get; set; }
        
    /// <summary>
    ///生产编号
    /// </summary>
    public string PROC_CODE { get; set; }
        
    /// <summary>
    ///VIN码
    /// </summary>
    public string VIN { get; set; }
        
    /// <summary>
    ///车型
    /// </summary>
    public string CAR_TYPE { get; set; }
        
    /// <summary>
    ///车型编码
    /// </summary>
    public string CAR_TYPE_CODE { get; set; }
        
    /// <summary>
    ///下线日期
    /// </summary>
    public System.DateTime OFFLINE_DATE { get; set; }
        
    /// <summary>
    ///状态
    /// </summary>
    public string STATUS { get; set; }
        
    /// <summary>
    ///锁定方式
    /// </summary>
    public string LOCK_MODE { get; set; }
        
    /// <summary>
    ///锁定人
    /// </summary>
    public string LOCK_MAN { get; set; }
        
    /// <summary>
    ///锁定时间
    /// </summary>
    public System.DateTime LOCK_TIME { get; set; }
        
    /// <summary>
    ///解锁方式
    /// </summary>
    public string UNLOCK_MODE { get; set; }
        
    /// <summary>
    ///解锁人
    /// </summary>
    public string UNLOCK_MAN { get; set; }
        
    /// <summary>
    ///解锁时间
    /// </summary>
    public System.DateTime UNLOCK_TIME { get; set; }
        
    /// <summary>
    ///锁车原因
    /// </summary>
    public string LOCK_REASON { get; set; }
        
    /// <summary>
    ///解锁原因
    /// </summary>
    public string UNLOCK_REASON { get; set; }
        
    /// <summary>
    ///排列顺序
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
        
    /// <summary>
    ///不良批次号
    /// </summary>
    public string BAD_CODE { get; set; }
    }
}
