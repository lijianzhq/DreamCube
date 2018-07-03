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
    ///面震检测数据表
    /// </summary>
    public partial class T_PQ_BU_SURF_VIBR_CHECK
    {
        
    /// <summary>
    ///面震检测数据ID
    /// </summary>
    public string SURF_VIBR_CHECK_ID { get; set; }
        
    /// <summary>
    ///测量时间
    /// </summary>
    public System.DateTime MEASURE_DATE { get; set; }
        
    /// <summary>
    ///测量人
    /// </summary>
    public string MEASURE_MAN { get; set; }
        
    /// <summary>
    ///设备编码
    /// </summary>
    public string EQUIP_CODE { get; set; }
        
    /// <summary>
    ///工厂编码
    /// </summary>
    public string FACTORY_CODE { get; set; }
        
    /// <summary>
    ///车间编码
    /// </summary>
    public string WORKSHOP_CODE { get; set; }
        
    /// <summary>
    ///生产线编码
    /// </summary>
    public string PRO_LINE_CODE { get; set; }
        
    /// <summary>
    ///车型编码
    /// </summary>
    public string CAR_TYPE_CODE { get; set; }
        
    /// <summary>
    ///VIN码
    /// </summary>
    public string VIN { get; set; }
        
    /// <summary>
    ///CCR编号
    /// </summary>
    public string CCR_CODE { get; set; }
        
    /// <summary>
    ///18位码
    /// </summary>
    public string EIGTEEN_CODE { get; set; }
        
    /// <summary>
    ///实际值
    /// </summary>
    public decimal ACTUAL_VAL { get; set; }
        
    /// <summary>
    ///判定标准
    /// </summary>
    public decimal JUDGE_STAN { get; set; }
        
    /// <summary>
    ///检测结果
    /// </summary>
    public string CHECK_RESULT { get; set; }
        
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
    ///位置 1 前振动 2 后振动
    /// </summary>
    public string POS_TYPE { get; set; }
        
    /// <summary>
    ///前面震
    /// </summary>
    public decimal Q_SURFACE_SHOCK { get; set; }
        
    /// <summary>
    ///后面震
    /// </summary>
    public decimal H_SURFACE_SHOCK { get; set; }
    }
}
