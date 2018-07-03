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
    ///外发部品数据主表
    /// </summary>
    public partial class T_PQ_BU_SEND_PART_M
    {
        
    /// <summary>
    ///外发部品数据主表ID
    /// </summary>
    public string SEND_PART_M_ID { get; set; }
        
    /// <summary>
    ///外发人	
    /// </summary>
    public string SEND_OUT_MAN { get; set; }
        
    /// <summary>
    ///外发时间	
    /// </summary>
    public System.DateTime SEND_OUT_TIME { get; set; }
        
    /// <summary>
    ///外发去向	
    /// </summary>
    public string SEND_OUT_DIRECTION { get; set; }
        
    /// <summary>
    ///外发备注	
    /// </summary>
    public string SEND_OUT_REMARK { get; set; }
        
    /// <summary>
    ///部品类别	
    /// </summary>
    public string PART_SORT { get; set; }
        
    /// <summary>
    ///外发数量
    /// </summary>
    public int SEND_OUT_QTY { get; set; }
        
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
    ///外发类型
    /// </summary>
    public string SEND_OUT_TYPE { get; set; }
        
    /// <summary>
    ///装箱号
    /// </summary>
    public string PACK_CODE { get; set; }
        
    /// <summary>
    ///工厂编码
    /// </summary>
    public string FACTORY_CODE { get; set; }
        
    /// <summary>
    ///工厂名称
    /// </summary>
    public string FACTORY_NAME { get; set; }
        
    /// <summary>
    ///客户标签号
    /// </summary>
    public string CUSTOM_PACK_CODE { get; set; }
    }
}