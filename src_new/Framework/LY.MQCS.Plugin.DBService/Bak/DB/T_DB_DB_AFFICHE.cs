//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace LY.MQCS.Plugin.DBService.Bak.DB
{
    using System;
    using System.Collections.Generic;
    
    
    /// <summary>
    ///公告主表
    /// </summary>
    public partial class T_DB_DB_AFFICHE
    {
        
    /// <summary>
    ///公告ID
    /// </summary>
    public string AFFICHE_ID { get; set; }
        
    /// <summary>
    ///公告标题
    /// </summary>
    public string AFFICHE_TITLE { get; set; }
        
    /// <summary>
    ///公告内容
    /// </summary>
    public string AFFICHE_CONTENT { get; set; }
        
    /// <summary>
    ///公告状态:1-新建;2已审核;3审核驳回;4已发布;
    /// </summary>
    public string AFFICHE_STATUS { get; set; }
        
    /// <summary>
    ///紧急度:0-一般;1-紧急;2-特急
    /// </summary>
    public string EMER_TYPE { get; set; }
        
    /// <summary>
    ///紧急度结束日期
    /// </summary>
    public System.DateTime EMER_END_DAY { get; set; }
        
    /// <summary>
    ///点击次数
    /// </summary>
    public decimal CLICK_NUM { get; set; }
        
    /// <summary>
    ///排序号
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
    ///是否置顶
    /// </summary>
    public string IS_TOP { get; set; }
        
    /// <summary>
    ///置顶时间
    /// </summary>
    public System.DateTime IS_TOP_TIME { get; set; }
        
    /// <summary>
    ///组织类别
    /// </summary>
    public string ORG_TYPE { get; set; }
    }
}