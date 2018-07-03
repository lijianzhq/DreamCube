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
    ///仲裁分类
    /// </summary>
    public partial class T_PQ_BU_ARBITRATION_TYPE
    {
        
    /// <summary>
    ///仲裁分类ID
    /// </summary>
    public string ARBITRATION_TYPE_ID { get; set; }
        
    /// <summary>
    ///责任类别编码
    /// </summary>
    public string PFP_SORT_CODE { get; set; }
        
    /// <summary>
    ///仲裁意见
    /// </summary>
    public string ARBITRATION_OPINION { get; set; }
        
    /// <summary>
    ///仲裁日期
    /// </summary>
    public System.DateTime ARBITRATION_DATE { get; set; }
        
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
    ///申诉序号
    /// </summary>
    public string APPEAL_SEQ { get; set; }
        
    /// <summary>
    ///责任类别编码1
    /// </summary>
    public string PFP_SORT_CODE_ONE { get; set; }
        
    /// <summary>
    ///责任单位编码1
    /// </summary>
    public string DUTY_COMP_CODE_ONE { get; set; }
        
    /// <summary>
    ///承担比例1
    /// </summary>
    public decimal BEAR_RATIO_ONE { get; set; }
        
    /// <summary>
    ///责任类别编码2
    /// </summary>
    public string PFP_SORT_CODE_TWO { get; set; }
        
    /// <summary>
    ///责任单位编码2
    /// </summary>
    public string DUTY_COMP_CODE_TWO { get; set; }
        
    /// <summary>
    ///承担比例2
    /// </summary>
    public decimal BEAR_RATIO_TWO { get; set; }
        
    /// <summary>
    ///责任类别编码3
    /// </summary>
    public string PFP_SORT_CODE_THREE { get; set; }
        
    /// <summary>
    ///责任单位编码3
    /// </summary>
    public string DUTY_COMP_CODE_THREE { get; set; }
        
    /// <summary>
    ///承担比例3
    /// </summary>
    public decimal BEAR_RATIO_THREE { get; set; }
        
    /// <summary>
    ///原责任类别编码1
    /// </summary>
    public string OLD_PFP_SORT_CODE_ONE { get; set; }
        
    /// <summary>
    ///原责任单位编码1
    /// </summary>
    public string OLD_DUTY_COMP_CODE_ONE { get; set; }
        
    /// <summary>
    ///原承担比例1
    /// </summary>
    public decimal OLD_BEAR_RATIO_ONE { get; set; }
        
    /// <summary>
    ///原责任类别编码2
    /// </summary>
    public string OLD_PFP_SORT_CODE_TWO { get; set; }
        
    /// <summary>
    ///原责任单位编码2
    /// </summary>
    public string OLD_DUTY_COMP_CODE_TWO { get; set; }
        
    /// <summary>
    ///原承担比例2
    /// </summary>
    public decimal OLD_BEAR_RATIO_TWO { get; set; }
        
    /// <summary>
    ///原责任类别编码3
    /// </summary>
    public string OLD_PFP_SORT_CODE_THREE { get; set; }
        
    /// <summary>
    ///原责任单位编码3
    /// </summary>
    public string OLD_DUTY_COMP_CODE_THREE { get; set; }
        
    /// <summary>
    ///原承担比例3
    /// </summary>
    public decimal OLD_BEAR_RATIO_THREE { get; set; }
        
    /// <summary>
    ///承担比例设定日期
    /// </summary>
    public System.DateTime BEAR_RATE_SET_DATE { get; set; }
        
    /// <summary>
    ///承担比例变更备注
    /// </summary>
    public string BEAR_RATE_CHAN_REMARK { get; set; }
        
    /// <summary>
    ///索赔类型1
    /// </summary>
    public string CLAIM_TYPE_ONE { get; set; }
        
    /// <summary>
    ///索赔类型2
    /// </summary>
    public string CLAIM_TYPE_TWO { get; set; }
        
    /// <summary>
    ///索赔类型3
    /// </summary>
    public string CLAIM_TYPE_THREE { get; set; }
        
    /// <summary>
    ///原索赔类型1
    /// </summary>
    public string OLD_CLAIM_TYPE_ONE { get; set; }
        
    /// <summary>
    ///原索赔类型2
    /// </summary>
    public string OLD_CLAIM_TYPE_TWO { get; set; }
        
    /// <summary>
    ///原索赔类型3
    /// </summary>
    public string OLD_CLAIM_TYPE_THREE { get; set; }
        
    /// <summary>
    ///科室ID1
    /// </summary>
    public string SECTION_ID_ONE { get; set; }
        
    /// <summary>
    ///科室ID2
    /// </summary>
    public string SECTION_ID_TWO { get; set; }
        
    /// <summary>
    ///科室ID3
    /// </summary>
    public string SECTION_ID_THREE { get; set; }
    
        public virtual T_PQ_BU_APPEAL_ORDER T_PQ_BU_APPEAL_ORDER { get; set; }
    }
}