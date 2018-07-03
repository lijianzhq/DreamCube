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
    ///水密涂胶检测(水密检测/涂胶评价)
    /// </summary>
    public partial class T_PQ_BU_WATERTIGHT_CHECK
    {
        
    /// <summary>
    ///水密涂胶检测ID
    /// </summary>
    public string WATERTIGHT_CHECK_ID { get; set; }
        
    /// <summary>
    ///车身号
    /// </summary>
    public string CAR_BODY_CODE { get; set; }
        
    /// <summary>
    ///检测部位
    /// </summary>
    public string CHECK_PART_POS_CODE { get; set; }
        
    /// <summary>
    ///不良描述
    /// </summary>
    public string BAD_DESC { get; set; }
        
    /// <summary>
    ///特记
    /// </summary>
    public string SPECIAL_NOTE { get; set; }
        
    /// <summary>
    ///检测结果
    /// </summary>
    public decimal CHECK_RESULT { get; set; }
        
    /// <summary>
    ///不良点数合计
    /// </summary>
    public int BAD_QTY_TOTAL { get; set; }
        
    /// <summary>
    ///判断结果
    /// </summary>
    public string JUDGE_RESULT { get; set; }
        
    /// <summary>
    ///备注
    /// </summary>
    public string REMARK { get; set; }
        
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
    
        public virtual T_PQ_BU_QUALITY_CHECK_V T_PQ_BU_QUALITY_CHECK_V { get; set; }
    }
}
