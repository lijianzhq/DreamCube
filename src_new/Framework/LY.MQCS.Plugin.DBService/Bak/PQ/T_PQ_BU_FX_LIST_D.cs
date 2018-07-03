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
    ///返修车RFID过车记录
    /// </summary>
    public partial class T_PQ_BU_FX_LIST_D
    {
        
    /// <summary>
    ///返修车RFID过车记录ID
    /// </summary>
    public string FX_LIST_D_ID { get; set; }
        
    /// <summary>
    ///返修区域编码
    /// </summary>
    public string AREA_CODE { get; set; }
        
    /// <summary>
    ///进入RFID编号或用户名
    /// </summary>
    public string RFID_CODE_IN { get; set; }
        
    /// <summary>
    ///驶出RFID编号或用户名
    /// </summary>
    public string RFID_CODE_OUT { get; set; }
        
    /// <summary>
    ///进入时间
    /// </summary>
    public System.DateTime DATE_IN { get; set; }
        
    /// <summary>
    ///驶出时间
    /// </summary>
    public System.DateTime DATE_OUT { get; set; }
        
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
    ///进入扫描方式1;rfid,2:手持机
    /// </summary>
    public string DATE_IN_WAY { get; set; }
        
    /// <summary>
    ///驶出扫描方式1;rfid,2:手持机
    /// </summary>
    public string DATE_OUT_WAY { get; set; }
    
        public virtual T_PQ_BU_FX_LIST T_PQ_BU_FX_LIST { get; set; }
    }
}
