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
    ///部品工程信息表
    /// </summary>
    public partial class T_PQ_BU_PART_PROJ
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public T_PQ_BU_PART_PROJ()
        {
            this.T_PQ_BU_PART_PROCESS = new HashSet<T_PQ_BU_PART_PROCESS>();
        }
    
        
    /// <summary>
    ///部品工程信息ID
    /// </summary>
    public string PART_PROJ_ID { get; set; }
        
    /// <summary>
    ///工厂编码
    /// </summary>
    public string FACTORY_CODE { get; set; }
        
    /// <summary>
    ///发动机号
    /// </summary>
    public string ENGINE_NO { get; set; }
        
    /// <summary>
    ///机种 默认值为“-”。机种从上线工序解析，现阶段上线工序读取率达不到100%，避免影响报表统计数据，读取不到的机种默认为“-”
    /// </summary>
    public string MAC_TYPE { get; set; }
        
    /// <summary>
    ///生产线编码
    /// </summary>
    public string PRO_LINE_CODE { get; set; }
        
    /// <summary>
    ///模具号
    /// </summary>
    public string MOLD_CODE { get; set; }
        
    /// <summary>
    ///部品类别
    /// </summary>
    public string PART_SORT { get; set; }
        
    /// <summary>
    ///部品二维码
    /// </summary>
    public string PART_2D_CODE { get; set; }
        
    /// <summary>
    ///零件编码
    /// </summary>
    public string PART_NO { get; set; }
        
    /// <summary>
    ///零件名称
    /// </summary>
    public string PART_NAME { get; set; }
        
    /// <summary>
    ///部品状态
    /// </summary>
    public string PART_STATUS { get; set; }
        
    /// <summary>
    ///不良大类
    /// </summary>
    public string BAD_BIG_TYPE { get; set; }
        
    /// <summary>
    ///不良类型
    /// </summary>
    public string BAD_TYPE { get; set; }
        
    /// <summary>
    ///状态逻辑值
    /// </summary>
    public short LOGIC_STATUS { get; set; }
        
    /// <summary>
    ///部品状态更改次数
    /// </summary>
    public int STATUS_CHANGE_NUM { get; set; }
        
    /// <summary>
    ///状态更改来源
    /// </summary>
    public string STATUS_CHANGE_SOURCE { get; set; }
        
    /// <summary>
    ///状态更改人
    /// </summary>
    public string STATUS_CHANGE_MAN { get; set; }
        
    /// <summary>
    ///状态更改时间
    /// </summary>
    public System.DateTime CHANGE_TIME { get; set; }
        
    /// <summary>
    ///匹配标记
    /// </summary>
    public string MATCH_MARK { get; set; }
        
    /// <summary>
    ///是否临时条码
    /// </summary>
    public string TEMP_QR_CODE { get; set; }
        
    /// <summary>
    ///装箱标签号
    /// </summary>
    public string PACK_LABEL_CODE { get; set; }
        
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
    ///变更批次
    /// </summary>
    public string CHANGE_BAT { get; set; }
        
    /// <summary>
    ///变更备注
    /// </summary>
    public string CHANGE_REMARK { get; set; }
        
    /// <summary>
    ///判定工序
    /// </summary>
    public string DECI_PROCESS { get; set; }
        
    /// <summary>
    ///责任部门
    /// </summary>
    public string DUTY_DEPT { get; set; }
        
    /// <summary>
    ///工厂名称
    /// </summary>
    public string FACTORY_NAME { get; set; }
        
    /// <summary>
    ///不良数据信息ID
    /// </summary>
    public string BAD_DATA_ID { get; set; }
        
    /// <summary>
    ///判定逻辑工序
    /// </summary>
    public string DECI_LOGIC_PROCESS { get; set; }
        
    /// <summary>
    ///是否成品 1是 0否
    /// </summary>
    public string IS_PRODUCE { get; set; }
        
    /// <summary>
    ///统计时间
    /// </summary>
    public System.DateTime STATISTIC_TIME { get; set; }
        
    /// <summary>
    ///审批态度
    /// </summary>
    public string APPROVAL_ATTI { get; set; }
        
    /// <summary>
    ///最新变更状态 默认值01。不考虑待处理事项
    /// </summary>
    public string PART_STATUS_LUPDATE { get; set; }
        
    /// <summary>
    ///待处理事项次数 默认值0
    /// </summary>
    public decimal TO_DEAL_QTY { get; set; }
        
    /// <summary>
    ///OFFLINE_TIME
    /// </summary>
    public System.DateTime OFFLINE_TIME { get; set; }
        
    /// <summary>
    ///成品二维码
    /// </summary>
    public string PROD_2D_CODE { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<T_PQ_BU_PART_PROCESS> T_PQ_BU_PART_PROCESS { get; set; }
    }
}
