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
    ///发动机装配批次信息表
    /// </summary>
    public partial class T_PQ_BU_E_ASSE_BAT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public T_PQ_BU_E_ASSE_BAT()
        {
            this.T_PQ_BU_E_ASSE_BAT_PART = new HashSet<T_PQ_BU_E_ASSE_BAT_PART>();
        }
    
        
    /// <summary>
    ///发动机装配批次信息ID
    /// </summary>
    public string E_ASSE_BAT_ID { get; set; }
        
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
    ///发动机号
    /// </summary>
    public string ENGINE_NO { get; set; }
        
    /// <summary>
    ///发动机状态
    /// </summary>
    public string ENGINE_STATUS { get; set; }
        
    /// <summary>
    ///发动机番号
    /// </summary>
    public string ENGINE_CODE { get; set; }
        
    /// <summary>
    ///状态更改次数
    /// </summary>
    public decimal STATUS_UPD_QTY { get; set; }
        
    /// <summary>
    ///状态更改来源
    /// </summary>
    public string STATUS_CHANGE_SOURCE { get; set; }
        
    /// <summary>
    ///主轴瓦等级
    /// </summary>
    public string MAIN_SHAFT_TILE_GRADE { get; set; }
        
    /// <summary>
    ///连杆瓦等级
    /// </summary>
    public string CONNECTING_ROD_GRADE { get; set; }
        
    /// <summary>
    ///活塞等级
    /// </summary>
    public string PISTON_GRADE { get; set; }
        
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
    ///工厂名称
    /// </summary>
    public string FACTORY_NAME { get; set; }
        
    /// <summary>
    ///状态逻辑值
    /// </summary>
    public short LOGIC_STATUS { get; set; }
        
    /// <summary>
    ///不良品数据ID
    /// </summary>
    public string BAD_DATA_ID { get; set; }
        
    /// <summary>
    ///状态更改人
    /// </summary>
    public string STATUS_CHANGE_MAN { get; set; }
        
    /// <summary>
    ///状态更改时间
    /// </summary>
    public System.DateTime CHANGE_TIME { get; set; }
        
    /// <summary>
    ///责任部门
    /// </summary>
    public string DUTY_DEPT { get; set; }
        
    /// <summary>
    ///变更批次
    /// </summary>
    public string CHANGE_BAT { get; set; }
        
    /// <summary>
    ///变更备注
    /// </summary>
    public string CHANGE_REMARK { get; set; }
        
    /// <summary>
    ///不良大类
    /// </summary>
    public string BAD_BIG_TYPE { get; set; }
        
    /// <summary>
    ///不良类型
    /// </summary>
    public string BAD_TYPE { get; set; }
        
    /// <summary>
    ///上线时间
    /// </summary>
    public System.DateTime ONLINE_TIME { get; set; }
        
    /// <summary>
    ///下线时间
    /// </summary>
    public System.DateTime OFFLINE_TIME { get; set; }
        
    /// <summary>
    ///缸体Jr等级
    /// </summary>
    public string CYLINDER_JR_GRADE { get; set; }
        
    /// <summary>
    ///接受时间
    /// </summary>
    public System.DateTime RECEIVE_TIME { get; set; }
        
    /// <summary>
    ///曲轴JrPIN等级
    /// </summary>
    public string CRANKSHAFT_JRPIN_GRADE { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<T_PQ_BU_E_ASSE_BAT_PART> T_PQ_BU_E_ASSE_BAT_PART { get; set; }
    }
}
