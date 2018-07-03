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
    ///不良流出管理附件数据表
    /// </summary>
    public partial class T_PQ_BU_BAD_OUT_ATTA
    {
        
    /// <summary>
    ///不良流出管理附件数据表ID
    /// </summary>
    public string BAD_OUT_ATTA_ID { get; set; }
        
    /// <summary>
    ///附件分类（lookup_value_code）
    /// </summary>
    public string ATTA_TYPE { get; set; }
        
    /// <summary>
    ///工厂编码（SAP_FACTORY_CODE）
    /// </summary>
    public string SAP_FACTORY_CODE { get; set; }
        
    /// <summary>
    ///科室编码（lookup_value_code）
    /// </summary>
    public string SECTION_CODE { get; set; }
        
    /// <summary>
    ///班组编码（lookup_value_code）
    /// </summary>
    public string TEAM_CODE { get; set; }
        
    /// <summary>
    ///上传人（user_name）
    /// </summary>
    public string UPLOAD_MAN { get; set; }
        
    /// <summary>
    ///上传时间
    /// </summary>
    public System.DateTime UPLOAD_TIME { get; set; }
        
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
    ///内容简介
    /// </summary>
    public string CONTENT_S_DESC { get; set; }
    }
}
