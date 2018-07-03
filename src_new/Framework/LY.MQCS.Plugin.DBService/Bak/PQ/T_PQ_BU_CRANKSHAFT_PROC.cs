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
    ///曲轴加工信息表
    /// </summary>
    public partial class T_PQ_BU_CRANKSHAFT_PROC
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public T_PQ_BU_CRANKSHAFT_PROC()
        {
            this.T_PQ_BU_CS_MEAS_MAC = new HashSet<T_PQ_BU_CS_MEAS_MAC>();
        }
    
        
    /// <summary>
    ///曲轴加工信息ID
    /// </summary>
    public string CRANKSHAFT_PROC_ID { get; set; }
        
    /// <summary>
    ///零件名称
    /// </summary>
    public string PART_NAME { get; set; }
        
    /// <summary>
    ///部品状态
    /// </summary>
    public string PART_STATUS { get; set; }
        
    /// <summary>
    ///工厂编码
    /// </summary>
    public string FACTORY_CODE { get; set; }
        
    /// <summary>
    ///生产线编码
    /// </summary>
    public string PRO_LINE_CODE { get; set; }
        
    /// <summary>
    ///机种
    /// </summary>
    public string MAC_TYPE { get; set; }
        
    /// <summary>
    ///二维码
    /// </summary>
    public string QR_CODE { get; set; }
        
    /// <summary>
    ///上线工序
    /// </summary>
    public string ONLINE_PROCESS { get; set; }
        
    /// <summary>
    ///上线设备号
    /// </summary>
    public string ONLINE_EQUIP_CODE { get; set; }
        
    /// <summary>
    ///上线时间
    /// </summary>
    public System.DateTime ONLINE_TIME { get; set; }
        
    /// <summary>
    ///OP10搬出时间
    /// </summary>
    public System.DateTime OP10_OUT_TIME { get; set; }
        
    /// <summary>
    ///锻造装箱lot条形码
    /// </summary>
    public string FORGE_LOT_BAR_CODE { get; set; }
        
    /// <summary>
    ///上线状态
    /// </summary>
    public string ONLINE_STATUS { get; set; }
        
    /// <summary>
    ///淬火机过机工序
    /// </summary>
    public string QUENCH_MAC_PROCESS { get; set; }
        
    /// <summary>
    ///淬火机过机设备号
    /// </summary>
    public string QUENCH_MAC_EQUIP_CODE { get; set; }
        
    /// <summary>
    ///淬火机过机时间
    /// </summary>
    public System.DateTime QUENCH_MAC_TIME { get; set; }
        
    /// <summary>
    ///OP90搬入时间
    /// </summary>
    public System.DateTime OP90_IN_TIME { get; set; }
        
    /// <summary>
    ///连杆颈加热时间
    /// </summary>
    public string CONN_ROD_HEAT_TIME { get; set; }
        
    /// <summary>
    ///法兰轴颈加热时间
    /// </summary>
    public string FLANGE_HEAT_TIME { get; set; }
        
    /// <summary>
    ///主轴颈加热时间
    /// </summary>
    public string SPINDLE_HEAT_TIME { get; set; }
        
    /// <summary>
    ///淬火机过机状态
    /// </summary>
    public string QUENCH_MAC_STATUS { get; set; }
        
    /// <summary>
    ///轴颈磨床过机工序
    /// </summary>
    public string SHAFT_MAC_PROCESS { get; set; }
        
    /// <summary>
    ///轴颈磨床过机设备号
    /// </summary>
    public string SHAFT_MAC_EQUIP_CODE { get; set; }
        
    /// <summary>
    ///轴颈磨床过机时间
    /// </summary>
    public System.DateTime SHAFT_MAC_TIME { get; set; }
        
    /// <summary>
    ///轴颈磨床过机搬入时间
    /// </summary>
    public System.DateTime SHAFT_MAC_IN_TIME { get; set; }
        
    /// <summary>
    ///轴颈磨床过机搬出时间
    /// </summary>
    public System.DateTime SHAFT_MAC_OUT_TIME { get; set; }
        
    /// <summary>
    ///轴颈磨床过机状态
    /// </summary>
    public string SHAFT_MAC_STATUS { get; set; }
        
    /// <summary>
    ///后端孔系加工中心过机工序
    /// </summary>
    public string BACK_MAC_PROCESS { get; set; }
        
    /// <summary>
    ///后端孔系加工中心过机设备号
    /// </summary>
    public string BACK_MAC_EQUIP_CODE { get; set; }
        
    /// <summary>
    ///后端孔系加工中心过机时间
    /// </summary>
    public System.DateTime BACK_MAC_TIME { get; set; }
        
    /// <summary>
    ///后端孔系加工中心过机搬入时间
    /// </summary>
    public System.DateTime BACK_MAC_IN_TIME { get; set; }
        
    /// <summary>
    ///后端孔系加工中心过机搬出时间
    /// </summary>
    public System.DateTime BACK_MAC_OUT_TIME { get; set; }
        
    /// <summary>
    ///后端孔系加工中心过机状态
    /// </summary>
    public string BACK_MAC_STATUS { get; set; }
        
    /// <summary>
    ///小头端磨床过机工序
    /// </summary>
    public string S_GRIND_MAC_PROCESS { get; set; }
        
    /// <summary>
    ///小头端磨床过机设备号
    /// </summary>
    public string S_GRIND_MAC_EQUIP_CODE { get; set; }
        
    /// <summary>
    ///小头端磨床过机时间
    /// </summary>
    public System.DateTime S_GRIND_MAC_TIME { get; set; }
        
    /// <summary>
    ///小头端磨床过机搬入时间
    /// </summary>
    public System.DateTime S_GRIND_MAC_IN_TIME { get; set; }
        
    /// <summary>
    ///小头端磨床过机搬出时间
    /// </summary>
    public System.DateTime S_GRIND_MAC_OUT_TIME { get; set; }
        
    /// <summary>
    ///小头端磨床过机状态
    /// </summary>
    public string S_GRIND_MAC_STATUS { get; set; }
        
    /// <summary>
    ///大头端磨床过机工序
    /// </summary>
    public string B_GRIND_MAC_PROCESS { get; set; }
        
    /// <summary>
    ///大头端磨床过机设备号
    /// </summary>
    public string B_GRIND_MAC_EQUIP_CODE { get; set; }
        
    /// <summary>
    ///大头端磨床过机时间
    /// </summary>
    public System.DateTime B_GRIND_MAC_TIME { get; set; }
        
    /// <summary>
    ///大头端磨床过机搬入时间
    /// </summary>
    public System.DateTime B_GRIND_MAC_IN_TIME { get; set; }
        
    /// <summary>
    ///大头端磨床过机搬出时间
    /// </summary>
    public System.DateTime B_GRIND_MAC_OUT_TIME { get; set; }
        
    /// <summary>
    ///大头端磨床过机状态
    /// </summary>
    public string B_GRIND_MAC_STATUS { get; set; }
        
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
    ///上线不良大类
    /// </summary>
    public string ON_BAD_B_TYPE { get; set; }
        
    /// <summary>
    ///淬火机过机不良大类
    /// </summary>
    public string QUENCH_MAC_BAD_B_TYPE { get; set; }
        
    /// <summary>
    ///轴颈磨床过机不良大类
    /// </summary>
    public string SHAFT_MAC_BAD_B_TYPE { get; set; }
        
    /// <summary>
    ///后端孔系加工中心过机不良大类
    /// </summary>
    public string BACK_MAC_BAD_B_TYPE { get; set; }
        
    /// <summary>
    ///小头端磨床过机不良大类
    /// </summary>
    public string S_GRIND_MAC_BAD_B_TYPE { get; set; }
        
    /// <summary>
    ///大头端磨床过机不良大类
    /// </summary>
    public string B_GRIND_MAC_BAD_B_TYPE { get; set; }
        
    /// <summary>
    ///零件编码
    /// </summary>
    public string PART_NO { get; set; }
        
    /// <summary>
    ///工厂名称
    /// </summary>
    public string FACTORY_NAME { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<T_PQ_BU_CS_MEAS_MAC> T_PQ_BU_CS_MEAS_MAC { get; set; }
    }
}
