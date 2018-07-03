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
    ///整车质量检查信息
    /// </summary>
    public partial class T_PQ_BU_CAR_PER_CHECK
    {
        
    /// <summary>
    ///整车性能检测ID
    /// </summary>
    public string CAR_PER_CHECK_ID { get; set; }
        
    /// <summary>
    ///投产编号
    /// </summary>
    public string PUT_INTO_CODE { get; set; }
        
    /// <summary>
    ///车身颜色编码
    /// </summary>
    public string CAR_COLOR_CODE { get; set; }
        
    /// <summary>
    ///内饰色编码
    /// </summary>
    public string INNER_COLOR_CODE { get; set; }
        
    /// <summary>
    ///钥匙号
    /// </summary>
    public string CAR_KEY_NO { get; set; }
        
    /// <summary>
    ///整车出厂日期
    /// </summary>
    public System.DateTime CAR_FACTORY_DATE { get; set; }
        
    /// <summary>
    ///下线计划时间
    /// </summary>
    public string OFFLINE_PLAN_DATE { get; set; }
        
    /// <summary>
    ///交车计划时间
    /// </summary>
    public string DELIVERY_PLAN_DATE { get; set; }
        
    /// <summary>
    ///检测类型
    /// </summary>
    public string TEST_TYPE { get; set; }
        
    /// <summary>
    ///检测条件
    /// </summary>
    public string CHECK_CONDITION { get; set; }
        
    /// <summary>
    ///检测基准-最小值
    /// </summary>
    public decimal CB_MIN_VAL { get; set; }
        
    /// <summary>
    ///检测基准-最大值
    /// </summary>
    public decimal CB_MAX_VAL { get; set; }
        
    /// <summary>
    ///检测结果
    /// </summary>
    public decimal CHECK_RESULT { get; set; }
        
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