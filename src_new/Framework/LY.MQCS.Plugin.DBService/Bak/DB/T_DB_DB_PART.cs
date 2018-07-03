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
    ///零件信息表
    /// </summary>
    public partial class T_DB_DB_PART
    {
        
    /// <summary>
    ///零件ID
    /// </summary>
    public string PART_ID { get; set; }
        
    /// <summary>
    ///零件编码
    /// </summary>
    public string PART_NO { get; set; }
        
    /// <summary>
    ///零件名称
    /// </summary>
    public string PART_NAME { get; set; }
        
    /// <summary>
    ///机种
    /// </summary>
    public string MAC_TYPE { get; set; }
        
    /// <summary>
    ///SNP
    /// </summary>
    public int SNP_NUM { get; set; }
        
    /// <summary>
    ///供应商编码
    /// </summary>
    public string SUPPLIER_CODE { get; set; }
        
    /// <summary>
    ///供应商名称
    /// </summary>
    public string SUPPLIER_NAME { get; set; }
        
    /// <summary>
    ///零件来源
    /// </summary>
    public string PART_SOURCE { get; set; }
        
    /// <summary>
    ///系统模块
    /// </summary>
    public string SYS_MODULE { get; set; }
        
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
    ///部品类别
    /// </summary>
    public string PART_SORT { get; set; }
        
    /// <summary>
    ///是否成品 1是 0否
    /// </summary>
    public string IS_PRODUCE { get; set; }
        
    /// <summary>
    ///零件类型 1-发动机部品，2-发动机整机，3-变速箱部品，4-变速箱-整机
    /// </summary>
    public string PART_TYPE { get; set; }
    }
}
