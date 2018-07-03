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
    ///联合色差测量从表
    /// </summary>
    public partial class T_PQ_BU_COMB_COLOR_MEAS_D
    {
        
    /// <summary>
    ///联合色差测量从表ID
    /// </summary>
    public string COMB_COLOR_MEAS_D_ID { get; set; }
        
    /// <summary>
    ///部位
    /// </summary>
    public string PART_POS { get; set; }
        
    /// <summary>
    ///明度
    /// </summary>
    public decimal LIGHT_VAL { get; set; }
        
    /// <summary>
    ///红相
    /// </summary>
    public decimal RED_VAL { get; set; }
        
    /// <summary>
    ///黄相
    /// </summary>
    public decimal YELLOW_VAL { get; set; }
        
    /// <summary>
    ///明度正面
    /// </summary>
    public decimal LIGHT_POSITIVE { get; set; }
        
    /// <summary>
    ///红相正面
    /// </summary>
    public decimal RED_POSITIVE { get; set; }
        
    /// <summary>
    ///黄相正面
    /// </summary>
    public decimal YELLOW_POSITIVE { get; set; }
        
    /// <summary>
    ///明度底面
    /// </summary>
    public decimal LIGHT_BOTTOM { get; set; }
        
    /// <summary>
    ///红相底面
    /// </summary>
    public decimal RED_BOTTOM { get; set; }
        
    /// <summary>
    ///黄相底面
    /// </summary>
    public decimal YELLOW_BOTTOM { get; set; }
        
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
    ///部位类型 1:车身;2:保杠
    /// </summary>
    public string PART_POS_TYPE { get; set; }
        
    /// <summary>
    ///部位类型:1-FL；2-FR；3-RL；4-RR
    /// </summary>
    public string PART_POS_SMALL_TYPE { get; set; }
    
        public virtual T_PQ_BU_COMB_COLOR_MEAS_M T_PQ_BU_COMB_COLOR_MEAS_M { get; set; }
    }
}
