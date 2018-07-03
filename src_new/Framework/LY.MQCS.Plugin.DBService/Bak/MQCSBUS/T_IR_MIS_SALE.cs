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
    ///销售数据接口表
    /// </summary>
    public partial class T_IR_MIS_SALE
    {
        
    /// <summary>
    ///销售接口表ID
    /// </summary>
    public string IS_SALE_DATA_ID { get; set; }
        
    /// <summary>
    ///销售数据ID
    /// </summary>
    public string SALE_REOCRD_ID { get; set; }
        
    /// <summary>
    ///一网编码
    /// </summary>
    public string NET_CODE { get; set; }
        
    /// <summary>
    ///VIN码
    /// </summary>
    public string VIN { get; set; }
        
    /// <summary>
    ///财务审核结束时间
    /// </summary>
    public string FINAN_CHECK_TIME { get; set; }
        
    /// <summary>
    ///车辆生产日期
    /// </summary>
    public string PROC_DATE { get; set; }
        
    /// <summary>
    ///车系名称
    /// </summary>
    public string CAR_SERIES_NAME { get; set; }
        
    /// <summary>
    ///车型名称
    /// </summary>
    public string CAR_TYPE_NAME { get; set; }
        
    /// <summary>
    ///生产所属月
    /// </summary>
    public string PROC_IN_MONTH { get; set; }
        
    /// <summary>
    ///网点简称
    /// </summary>
    public string NET_SHOR_NAME { get; set; }
        
    /// <summary>
    ///网点所属的大区
    /// </summary>
    public string BIG_EARE { get; set; }
        
    /// <summary>
    ///销售日期
    /// </summary>
    public string SALE_DATE { get; set; }
        
    /// <summary>
    ///销售所属月
    /// </summary>
    public string REPAIR_IN_MONTH { get; set; }
        
    /// <summary>
    ///车型
    /// </summary>
    public string CAR_TYPE_CODE { get; set; }
        
    /// <summary>
    ///保修统计日期
    /// </summary>
    public string MAINTEGERAIN_STAT_DATE { get; set; }
        
    /// <summary>
    ///专营店给客户的交车日
    /// </summary>
    public System.DateTime DLR_TO_CUST_DATE { get; set; }
        
    /// <summary>
    ///车系
    /// </summary>
    public string CAR_SERIES_CODE { get; set; }
        
    /// <summary>
    ///消退标志
    /// </summary>
    public decimal FLAG_BACK { get; set; }
        
    /// <summary>
    ///消退时间
    /// </summary>
    public string DATE_BACK { get; set; }
        
    /// <summary>
    ///是否迁移
    /// </summary>
    public string IS_FLAG { get; set; }
        
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
    ///PROVINCE_ID
    /// </summary>
    public string PROVINCE_ID { get; set; }
        
    /// <summary>
    ///PROVINCE_NAME
    /// </summary>
    public string PROVINCE_NAME { get; set; }
        
    /// <summary>
    ///ENGINE_NO
    /// </summary>
    public string ENGINE_NO { get; set; }
        
    /// <summary>
    ///ENGINE_AO
    /// </summary>
    public string ENGINE_AO { get; set; }
        
    /// <summary>
    ///CAR_COLOR_CODE
    /// </summary>
    public string CAR_COLOR_CODE { get; set; }
        
    /// <summary>
    ///CAR_COLOR_NAME
    /// </summary>
    public string CAR_COLOR_NAME { get; set; }
        
    /// <summary>
    ///EIGTEEN_CODE
    /// </summary>
    public string EIGTEEN_CODE { get; set; }
        
    /// <summary>
    ///CAR_CONFIG
    /// </summary>
    public string CAR_CONFIG { get; set; }
        
    /// <summary>
    ///BASE_SERIES_CODE
    /// </summary>
    public string BASE_SERIES_CODE { get; set; }
        
    /// <summary>
    ///变速箱型式
    /// </summary>
    public string TRAN_TYPE { get; set; }
        
    /// <summary>
    ///品牌：NISSAN、启辰、INFINIT、进口NISSAN。业务提供了NISSAN、启辰的变速箱规则；进口车比较特殊（6DCT，6档双离合变速箱；7AMT，7档手自一体变速箱）
    /// </summary>
    public string CAR_BRAND_CODE { get; set; }
    }
}