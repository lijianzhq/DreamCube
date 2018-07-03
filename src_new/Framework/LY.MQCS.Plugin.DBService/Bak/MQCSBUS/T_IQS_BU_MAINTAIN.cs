//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace LY.MQCS.Plugin.DBService.Bak.MQCSBUS
{
    using System;
    using System.Collections.Generic;
    
    
    /// <summary>
    ///保修数据业务表
    /// </summary>
    public partial class T_IQS_BU_MAINTAIN
    {
        
    /// <summary>
    ///保修数据业务表ID
    /// </summary>
    public string MAINTAIN_DATA_ID { get; set; }
        
    /// <summary>
    ///一网编码
    /// </summary>
    public string NET_CODE { get; set; }
        
    /// <summary>
    ///保修单号
    /// </summary>
    public string GUARANTNO { get; set; }
        
    /// <summary>
    ///同意编号
    /// </summary>
    public string AGREE_NO { get; set; }
        
    /// <summary>
    ///VIN码
    /// </summary>
    public string VIN { get; set; }
        
    /// <summary>
    ///发动机号
    /// </summary>
    public string ENGINE { get; set; }
        
    /// <summary>
    ///购车日期
    /// </summary>
    public System.DateTime BUY_CAR_DATE { get; set; }
        
    /// <summary>
    ///行驶里程
    /// </summary>
    public decimal MILEAGE { get; set; }
        
    /// <summary>
    ///联系人
    /// </summary>
    public string LINK_MAN { get; set; }
        
    /// <summary>
    ///联系电话
    /// </summary>
    public string LINK_TEL { get; set; }
        
    /// <summary>
    ///修理日期
    /// </summary>
    public System.DateTime REPAIR_DATE { get; set; }
        
    /// <summary>
    ///故障现象码
    /// </summary>
    public string CS_CODE { get; set; }
        
    /// <summary>
    ///故障原因码
    /// </summary>
    public string CT_CODE { get; set; }
        
    /// <summary>
    ///派工单号
    ///安装里程
    ///安装日期
    ///安装派工单号
    ///故障现象及原因描述
    ///工时费小计
    ///备件费小计
    ///其它费用小计
    ///费用合计
    ///自动审核标志
    ///财务审核结束时间
    ///车辆生产日期
    ///车牌号
    ///车系名称
    ///车型名称
    ///故障部位代码
    ///故障现象名称
    ///故障原因名称
    ///生产所属月
    ///网点简称
    ///网点全称
    ///网点所属的大区
    ///相差的天数
    ///销售日期
    ///销售所属月
    ///修理月
    ///业务审核日期
    ///主凶件号（不含供应商编码）
    ///主凶件名称
    ///主凶件数量
    ///保修类别码
    ///车型
    ///保修统计日期
    ///车系
    ///派工单号
    /// </summary>
    public string REPAIR_CODE { get; set; }
        
    /// <summary>
    ///安装里程
    /// </summary>
    public decimal FIX_MILEAGE { get; set; }
        
    /// <summary>
    ///安装日期
    /// </summary>
    public string FIX_DATE { get; set; }
        
    /// <summary>
    ///安装派工单号
    /// </summary>
    public string ASSEMB_CODE { get; set; }
        
    /// <summary>
    ///故障现象及原因描述
    /// </summary>
    public string REASON { get; set; }
        
    /// <summary>
    ///工时费小计
    /// </summary>
    public decimal WORK_FEE { get; set; }
        
    /// <summary>
    ///备件费小计
    /// </summary>
    public decimal PART_FEE { get; set; }
        
    /// <summary>
    ///其它费用小计
    /// </summary>
    public decimal OTHE_FEE { get; set; }
        
    /// <summary>
    ///费用合计
    /// </summary>
    public decimal TOTAL_FEE { get; set; }
        
    /// <summary>
    ///自动审核标志
    /// </summary>
    public string IS_AUTO_CHECK { get; set; }
        
    /// <summary>
    ///财务审核结束时间
    /// </summary>
    public string FINAN_CHECK_TIME { get; set; }
        
    /// <summary>
    ///车辆生产日期
    /// </summary>
    public string PROC_DATE { get; set; }
        
    /// <summary>
    ///车牌号
    /// </summary>
    public string CAR_NO { get; set; }
        
    /// <summary>
    ///车系名称
    /// </summary>
    public string CAR_SERIES_NAME { get; set; }
        
    /// <summary>
    ///车型名称
    /// </summary>
    public string CAR_TYPE_NAME { get; set; }
        
    /// <summary>
    ///故障部位代码
    /// </summary>
    public string FAULT_PART_CODE { get; set; }
        
    /// <summary>
    ///故障现象名称
    /// </summary>
    public string CS_NAME { get; set; }
        
    /// <summary>
    ///故障原因名称
    /// </summary>
    public string CT_NAME { get; set; }
        
    /// <summary>
    ///生产所属月
    /// </summary>
    public string PROC_IN_MONTH { get; set; }
        
    /// <summary>
    ///网点简称
    /// </summary>
    public string NET_SHOR_NAME { get; set; }
        
    /// <summary>
    ///网点全称
    /// </summary>
    public string NET_FULL_NAME { get; set; }
        
    /// <summary>
    ///网点所属的大区
    /// </summary>
    public string BIG_EARE { get; set; }
        
    /// <summary>
    ///相差的天数
    /// </summary>
    public decimal DIFF_DAYS { get; set; }
        
    /// <summary>
    ///销售日期
    /// </summary>
    public System.DateTime SALE_DATE { get; set; }
        
    /// <summary>
    ///销售所属月
    /// </summary>
    public string REPAIR_IN_MONTH { get; set; }
        
    /// <summary>
    ///修理月
    /// </summary>
    public string REPIRT_MONTH { get; set; }
        
    /// <summary>
    ///业务审核日期
    /// </summary>
    public string BUSINESS_CHECK_DATE { get; set; }
        
    /// <summary>
    ///主凶件号（不含供应商编码）
    /// </summary>
    public string NO_SUPPLY_PFP_CODE { get; set; }
        
    /// <summary>
    ///主凶件名称
    /// </summary>
    public string PFP_NAME { get; set; }
        
    /// <summary>
    ///主凶件数量
    /// </summary>
    public string PFP_QTY { get; set; }
        
    /// <summary>
    ///保修类别码
    /// </summary>
    public string MAINTEGERAIN_TYPE_CODE { get; set; }
        
    /// <summary>
    ///车型编码
    /// </summary>
    public string CAR_TYPE_CODE { get; set; }
        
    /// <summary>
    ///保修统计日期
    /// </summary>
    public string MAINTEGERAIN_STAT_DATE { get; set; }
        
    /// <summary>
    ///车系编码
    /// </summary>
    public string CAR_SERIES_CODE { get; set; }
        
    /// <summary>
    ///供应商编码
    /// </summary>
    public string SUPPLIER_CODE { get; set; }
        
    /// <summary>
    ///供应商简称
    /// </summary>
    public string SUPPLIER_SHORT_NAME { get; set; }
        
    /// <summary>
    ///修理费
    /// </summary>
    public decimal COMP_DLR_FEE { get; set; }
        
    /// <summary>
    ///流水修理费
    /// </summary>
    public decimal COMP_DLR_WORK_FEE { get; set; }
        
    /// <summary>
    ///其它修理费
    /// </summary>
    public decimal COMP_DLR_OTHER_FEE { get; set; }
        
    /// <summary>
    ///责任类别
    /// </summary>
    public string PFP_SORT { get; set; }
        
    /// <summary>
    ///责任单位
    /// </summary>
    public string PFP_SID { get; set; }
        
    /// <summary>
    ///其他费用系数
    /// </summary>
    public decimal OTHE_COEF { get; set; }
        
    /// <summary>
    ///备注
    /// </summary>
    public string REMARK { get; set; }
        
    /// <summary>
    ///车辆品牌
    /// </summary>
    public string BRAND_CODE { get; set; }
        
    /// <summary>
    ///退回日期
    /// </summary>
    public string DATE_BACK { get; set; }
        
    /// <summary>
    ///倒扣日期
    /// </summary>
    public string DATE_CLAIM { get; set; }
        
    /// <summary>
    ///拒绝日期
    /// </summary>
    public string DATE_REFUSE { get; set; }
        
    /// <summary>
    ///是否退回
    /// </summary>
    public decimal FLAG_BACK { get; set; }
        
    /// <summary>
    ///是否拒绝
    /// </summary>
    public decimal FLAG_REFUSE { get; set; }
        
    /// <summary>
    ///网点品牌
    /// </summary>
    public string NETBRAND { get; set; }
        
    /// <summary>
    ///备件系数
    /// </summary>
    public decimal PART_COEF { get; set; }
        
    /// <summary>
    ///工时费系数
    /// </summary>
    public decimal WORK_COEF { get; set; }
        
    /// <summary>
    ///召回编号
    /// </summary>
    public string RECALL_CODE { get; set; }
        
    /// <summary>
    ///专案编号
    /// </summary>
    public string CASE_CODE { get; set; }
        
    /// <summary>
    ///排列顺序
    /// </summary>
    public decimal ORDER_NO { get; set; }
        
    /// <summary>
    ///默认值：1
    /// </summary>
    public string IS_ENABLE { get; set; }
        
    /// <summary>
    ///默认值：sys_guid()
    /// </summary>
    public string UPDATE_CONTROL_ID { get; set; }
        
    /// <summary>
    ///默认值：88888
    /// </summary>
    public string SDP_USER_ID { get; set; }
        
    /// <summary>
    ///默认值：2
    /// </summary>
    public string SDP_ORG_ID { get; set; }
        
    /// <summary>
    ///创建人
    /// </summary>
    public string CREATOR { get; set; }
        
    /// <summary>
    ///创建时间
    /// </summary>
    public System.DateTime CREATED_DATE { get; set; }
        
    /// <summary>
    ///修改人
    /// </summary>
    public string MODIFIER { get; set; }
        
    /// <summary>
    ///修改时间
    /// </summary>
    public System.DateTime LAST_UPDATED_DATE { get; set; }
        
    /// <summary>
    ///销售到生产月间隔
    /// </summary>
    public decimal SALE_PRODUCT_MONTH { get; set; }
        
    /// <summary>
    ///修理月到销售月间隔
    /// </summary>
    public decimal REPARE_SALE_MONTH { get; set; }
        
    /// <summary>
    ///销售到生产月间隔与修理月到销售月间隔之和
    /// </summary>
    public decimal ALL_MONTH { get; set; }
        
    /// <summary>
    ///PFP
    /// </summary>
    public string PFP { get; set; }
        
    /// <summary>
    ///省份ID
    /// </summary>
    public string PROVINCE_ID { get; set; }
        
    /// <summary>
    ///省份名称
    /// </summary>
    public string PROVINCE_NAME { get; set; }
        
    /// <summary>
    ///城市编码
    /// </summary>
    public string CITY_CODE { get; set; }
        
    /// <summary>
    ///城市名称
    /// </summary>
    public string CITY_NAME { get; set; }
        
    /// <summary>
    ///FAULT_PART_NAME
    /// </summary>
    public string FAULT_PART_NAME { get; set; }
    }
}
