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
    ///锻造批次数据表
    /// </summary>
    public partial class T_PQ_BU_FORGE_BAT_DATA
    {
        
    /// <summary>
    ///锻造批次数据ID
    /// </summary>
    public string FORGE_BAT_DATA_ID { get; set; }
        
    /// <summary>
    ///部品类型
    /// </summary>
    public string PART_TYPE { get; set; }
        
    /// <summary>
    ///工厂编码
    /// </summary>
    public string FACTORY_CODE { get; set; }
        
    /// <summary>
    ///生产线编码
    /// </summary>
    public string PRO_LINE_CODE { get; set; }
        
    /// <summary>
    ///工程
    /// </summary>
    public string ENGINEERING_TYPE { get; set; }
        
    /// <summary>
    ///设备编码
    /// </summary>
    public string EQUIP_CODE { get; set; }
        
    /// <summary>
    ///机种
    /// </summary>
    public string MAC_TYPE { get; set; }
        
    /// <summary>
    ///炉号信息
    /// </summary>
    public string FURNACE_INFO { get; set; }
        
    /// <summary>
    ///时间
    /// </summary>
    public System.DateTime TIME_VAL { get; set; }
        
    /// <summary>
    ///班次信息
    /// </summary>
    public string CLASS_TIMES_INFO { get; set; }
        
    /// <summary>
    ///模具号
    /// </summary>
    public string MOLD_CODE { get; set; }
        
    /// <summary>
    ///装箱时间
    /// </summary>
    public System.DateTime PACK_TIME { get; set; }
        
    /// <summary>
    ///装箱lot条形码
    /// </summary>
    public string PACK_LOT_BAR_CODE { get; set; }
        
    /// <summary>
    ///零件编码
    /// </summary>
    public string PART_NO { get; set; }
        
    /// <summary>
    ///数量
    /// </summary>
    public decimal QTY { get; set; }
        
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
    ///工厂名称
    /// </summary>
    public string FACTORY_NAME { get; set; }
        
    /// <summary>
    ///混箱标识
    /// </summary>
    public string DISORDER_MARK { get; set; }
        
    /// <summary>
    ///加工上线时间
    /// </summary>
    public System.DateTime PRO_ONLINE_TIME { get; set; }
        
    /// <summary>
    ///热处理时间 只带轮有值
    /// </summary>
    public System.DateTime HEAT_TREATMENT_TIME { get; set; }
        
    /// <summary>
    ///热处理班次 只带轮有值
    /// </summary>
    public string HEAT_TREATMENT_CLASS_TIMES { get; set; }
        
    /// <summary>
    ///冷锻时间 只带轮机种FKK2 SECD有值
    /// </summary>
    public System.DateTime COLD_FORGING_TIME { get; set; }
        
    /// <summary>
    ///冷锻模具号 只带轮机种FKK2 SECD有值
    /// </summary>
    public string COLD_FORGING_MOLD_CODE { get; set; }
        
    /// <summary>
    ///冷锻班次 只带轮机种FKK2 SECD有值
    /// </summary>
    public string COLD_FORGING_CLASS_TIMES { get; set; }
    }
}
