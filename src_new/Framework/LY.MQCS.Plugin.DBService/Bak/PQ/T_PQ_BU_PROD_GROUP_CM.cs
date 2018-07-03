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
    ///生产班组原因对策数据表
    /// </summary>
    public partial class T_PQ_BU_PROD_GROUP_CM
    {
        
    /// <summary>
    ///生产班组原因对策数据表ID
    /// </summary>
    public string PROD_GROUP_CM_ID { get; set; }
        
    /// <summary>
    ///问题数据表名
    /// </summary>
    public string QUES_DATA_TAB { get; set; }
        
    /// <summary>
    ///问题数据ID值
    /// </summary>
    public string QUES_DATA_ID { get; set; }
        
    /// <summary>
    ///发生工作站（lookup_value_code）
    /// </summary>
    public string HAP_STAT { get; set; }
        
    /// <summary>
    ///责任人（user_name）
    /// </summary>
    public string DUTY_MAN { get; set; }
        
    /// <summary>
    ///原因
    /// </summary>
    public string REASON_DESC { get; set; }
        
    /// <summary>
    ///对策 
    /// </summary>
    public string CM_DESC { get; set; }
        
    /// <summary>
    ///责任担当人（user_name）
    /// </summary>
    public string DUTY_TAKE_MAN { get; set; }
        
    /// <summary>
    ///是否基准修订
    /// </summary>
    public string IS_BASE_UPD { get; set; }
        
    /// <summary>
    ///是否OPL教育
    /// </summary>
    public string IS_OPL_EDU { get; set; }
        
    /// <summary>
    ///是否纳入管理项目
    /// </summary>
    public string IS_MANA_PROJ { get; set; }
        
    /// <summary>
    ///作业观察
    /// </summary>
    public string WORK_OBSERVE { get; set; }
        
    /// <summary>
    ///计划完成日期
    /// </summary>
    public System.DateTime PLAN_FINISHED_DATE { get; set; }
        
    /// <summary>
    ///完成提醒日期
    /// </summary>
    public System.DateTime FINISHED_WARN_DATE { get; set; }
        
    /// <summary>
    ///效果确认方案（指定）（1：Q班；2：班组组长）
    /// </summary>
    public string EFFECT_CONF_SCHE { get; set; }
        
    /// <summary>
    ///效果确认人（实际）（user_name）
    /// </summary>
    public string EFFECT_CONF_MAN { get; set; }
        
    /// <summary>
    ///效果确认备注信息
    /// </summary>
    public string EFFECT_CONF_REMARK { get; set; }
        
    /// <summary>
    ///对策记录人（user_name）
    /// </summary>
    public string CM_RECORD_MAN { get; set; }
        
    /// <summary>
    ///对策记录日期
    /// </summary>
    public System.DateTime CM_RECORD_DATE { get; set; }
        
    /// <summary>
    ///分发人（user_name）
    /// </summary>
    public string DISTRI_MAN { get; set; }
        
    /// <summary>
    ///分发日期
    /// </summary>
    public System.DateTime DISTRI_DATE { get; set; }
        
    /// <summary>
    ///被分发班组编码（lookup_value_code）
    /// </summary>
    public string DISTRI_GROUP_CODE { get; set; }
        
    /// <summary>
    ///当前状态（1：已分发待记录对策；2：已记录对策待确认；3：已确认）
    /// </summary>
    public string CUR_STATUS { get; set; }
        
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