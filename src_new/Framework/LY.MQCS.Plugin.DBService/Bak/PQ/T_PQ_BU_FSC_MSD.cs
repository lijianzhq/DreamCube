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
    ///熔射单元测量数据表
    /// </summary>
    public partial class T_PQ_BU_FSC_MSD
    {
        
    /// <summary>
    ///熔射单元测量数据ID
    /// </summary>
    public string FSC_MSD_ID { get; set; }
        
    /// <summary>
    ///工厂编码
    /// </summary>
    public string FACTORY_CODE { get; set; }
        
    /// <summary>
    ///二维码
    /// </summary>
    public string QR_CODE { get; set; }
        
    /// <summary>
    ///时间
    /// </summary>
    public System.DateTime FSC_DATE { get; set; }
        
    /// <summary>
    ///测量时长
    /// </summary>
    public decimal FSC_MSD_DATE { get; set; }
        
    /// <summary>
    ///送丝速度1
    /// </summary>
    public decimal FSC_SPEED { get; set; }
        
    /// <summary>
    ///送丝速度2
    /// </summary>
    public decimal FSC_SPEED_TO { get; set; }
        
    /// <summary>
    ///电压
    /// </summary>
    public decimal FSC_VOLTAGE { get; set; }
        
    /// <summary>
    ///电流
    /// </summary>
    public decimal FSC_ET_CURRENT { get; set; }
        
    /// <summary>
    ///一级氮气流量
    /// </summary>
    public decimal FSC_NITROGEN { get; set; }
        
    /// <summary>
    ///二级氮气流量
    /// </summary>
    public decimal FSC_NITROGEN_TO { get; set; }
        
    /// <summary>
    ///一级氮气压力
    /// </summary>
    public decimal FSC_PRESSURE { get; set; }
        
    /// <summary>
    ///二级氮气压力
    /// </summary>
    public decimal FSC_PRESSURE_TO { get; set; }
        
    /// <summary>
    ///喷枪抽风流量
    /// </summary>
    public decimal FSC_SPRAY { get; set; }
        
    /// <summary>
    ///工件抽风流量
    /// </summary>
    public decimal FSC_WORKPIECE { get; set; }
        
    /// <summary>
    ///喷枪移动速度
    /// </summary>
    public decimal FSC_MOVING_SPEED { get; set; }
        
    /// <summary>
    ///原始文件路径
    /// </summary>
    public string FSC_FILE_PATH { get; set; }
        
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
    }
}