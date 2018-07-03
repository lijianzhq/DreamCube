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
    ///UCF检测
    /// </summary>
    public partial class T_PQ_BU_UCF_CHECK
    {
        
    /// <summary>
    ///UCF检测ID
    /// </summary>
    public string UCF_CHECK_ID { get; set; }
        
    /// <summary>
    ///检测项目细分
    /// </summary>
    public string CHECK_ITEM_D { get; set; }
        
    /// <summary>
    ///检测部位
    /// </summary>
    public string CHECK_PART_POS_CODE { get; set; }
        
    /// <summary>
    ///检测点位
    /// </summary>
    public string CHECK_POINT { get; set; }
        
    /// <summary>
    ///检测基准-间隙规格
    /// </summary>
    public decimal CB_GAP_SPECI { get; set; }
        
    /// <summary>
    ///检测基准-间隙公差
    /// </summary>
    public decimal CB_GAP_TOLERANCE { get; set; }
        
    /// <summary>
    ///检测基准-面差规格
    /// </summary>
    public decimal CB_FACE_SPECI { get; set; }
        
    /// <summary>
    ///检测基准-面差公差
    /// </summary>
    public decimal CB_FACE_TOLERANCE { get; set; }
        
    /// <summary>
    ///检测结果-间隙值
    /// </summary>
    public decimal CR_GAP_VAL { get; set; }
        
    /// <summary>
    ///检测结果-面差值
    /// </summary>
    public decimal CR_TOLERANCE_VAL { get; set; }
        
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