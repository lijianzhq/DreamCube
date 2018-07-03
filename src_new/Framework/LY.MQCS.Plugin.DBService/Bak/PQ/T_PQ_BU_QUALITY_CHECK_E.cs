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
    ///发动机品质检测主信息
    /// </summary>
    public partial class T_PQ_BU_QUALITY_CHECK_E
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public T_PQ_BU_QUALITY_CHECK_E()
        {
            this.T_PQ_BU_CHRO_CHECK_E = new HashSet<T_PQ_BU_CHRO_CHECK_E>();
            this.T_PQ_BU_EQUIP_CHECK_E = new HashSet<T_PQ_BU_EQUIP_CHECK_E>();
            this.T_PQ_BU_FIREBOX_HEIGHT = new HashSet<T_PQ_BU_FIREBOX_HEIGHT>();
            this.T_PQ_BU_HARD_CHECK = new HashSet<T_PQ_BU_HARD_CHECK>();
            this.T_PQ_BU_MATERIAL_CHECK = new HashSet<T_PQ_BU_MATERIAL_CHECK>();
            this.T_PQ_BU_MOMENT_MEASURE_E = new HashSet<T_PQ_BU_MOMENT_MEASURE_E>();
            this.T_PQ_BU_PER_CHECK = new HashSet<T_PQ_BU_PER_CHECK>();
            this.T_PQ_BU_PRE_SAND_CHECK = new HashSet<T_PQ_BU_PRE_SAND_CHECK>();
            this.T_PQ_BU_R_MATERIAL_CHECK = new HashSet<T_PQ_BU_R_MATERIAL_CHECK>();
            this.T_PQ_BU_SIZE_CHECK = new HashSet<T_PQ_BU_SIZE_CHECK>();
            this.T_PQ_BU_SPECTRAL_CHECK = new HashSet<T_PQ_BU_SPECTRAL_CHECK>();
            this.T_PQ_BU_SURFACE_CHECK_E = new HashSet<T_PQ_BU_SURFACE_CHECK_E>();
            this.T_PQ_BU_TES_D_EVAL = new HashSet<T_PQ_BU_TES_D_EVAL>();
        }
    
        
    /// <summary>
    ///发动机品质检测主信息ID
    /// </summary>
    public string QUALITY_CHECK_E_ID { get; set; }
        
    /// <summary>
    ///品质检测项目编码
    /// </summary>
    public string CP_CHECK_CODE { get; set; }
        
    /// <summary>
    ///工艺编码
    /// </summary>
    public string CRAFT_CODE { get; set; }
        
    /// <summary>
    ///工厂编码
    /// </summary>
    public string FACTORY_CODE { get; set; }
        
    /// <summary>
    ///车间编码
    /// </summary>
    public string WORKSHOP_CODE { get; set; }
        
    /// <summary>
    ///生产线编码
    /// </summary>
    public string PRO_LINE_CODE { get; set; }
        
    /// <summary>
    ///工序编码
    /// </summary>
    public string PROCESS_CODE { get; set; }
        
    /// <summary>
    ///车系编码
    /// </summary>
    public string CAR_SERIES_CODE { get; set; }
        
    /// <summary>
    ///车型编码
    /// </summary>
    public string CAR_TYPE_CODE { get; set; }
        
    /// <summary>
    ///发动机号
    /// </summary>
    public string ENGINE_NO { get; set; }
        
    /// <summary>
    ///零件编码
    /// </summary>
    public string PART_NO { get; set; }
        
    /// <summary>
    ///检测项目
    /// </summary>
    public string CHECK_ITEM { get; set; }
        
    /// <summary>
    ///检测方式
    /// </summary>
    public string CHECK_WAY { get; set; }
        
    /// <summary>
    ///检测频率
    /// </summary>
    public string CHECK_FREQ { get; set; }
        
    /// <summary>
    ///检测时间
    /// </summary>
    public System.DateTime CHECK_DATE { get; set; }
        
    /// <summary>
    ///检查地点
    /// </summary>
    public string CHECK_ADDR { get; set; }
        
    /// <summary>
    ///班别
    /// </summary>
    public string SCHEDULE_TYPE { get; set; }
        
    /// <summary>
    ///检测人
    /// </summary>
    public string CHECK_MAN { get; set; }
        
    /// <summary>
    ///判断结果
    /// </summary>
    public string JUDGE_RESULT { get; set; }
        
    /// <summary>
    ///平均值
    /// </summary>
    public decimal AVG_VAL { get; set; }
        
    /// <summary>
    ///最大值
    /// </summary>
    public decimal MAX_VAL { get; set; }
        
    /// <summary>
    ///最小值
    /// </summary>
    public decimal MIN_VAL { get; set; }
        
    /// <summary>
    ///公差
    /// </summary>
    public decimal TOLERANCE_VAL { get; set; }
        
    /// <summary>
    ///标准差
    /// </summary>
    public decimal STAN_DEVI { get; set; }
        
    /// <summary>
    ///合格率
    /// </summary>
    public decimal PASS_RATE { get; set; }
        
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
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<T_PQ_BU_CHRO_CHECK_E> T_PQ_BU_CHRO_CHECK_E { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<T_PQ_BU_EQUIP_CHECK_E> T_PQ_BU_EQUIP_CHECK_E { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<T_PQ_BU_FIREBOX_HEIGHT> T_PQ_BU_FIREBOX_HEIGHT { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<T_PQ_BU_HARD_CHECK> T_PQ_BU_HARD_CHECK { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<T_PQ_BU_MATERIAL_CHECK> T_PQ_BU_MATERIAL_CHECK { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<T_PQ_BU_MOMENT_MEASURE_E> T_PQ_BU_MOMENT_MEASURE_E { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<T_PQ_BU_PER_CHECK> T_PQ_BU_PER_CHECK { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<T_PQ_BU_PRE_SAND_CHECK> T_PQ_BU_PRE_SAND_CHECK { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<T_PQ_BU_R_MATERIAL_CHECK> T_PQ_BU_R_MATERIAL_CHECK { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<T_PQ_BU_SIZE_CHECK> T_PQ_BU_SIZE_CHECK { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<T_PQ_BU_SPECTRAL_CHECK> T_PQ_BU_SPECTRAL_CHECK { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<T_PQ_BU_SURFACE_CHECK_E> T_PQ_BU_SURFACE_CHECK_E { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<T_PQ_BU_TES_D_EVAL> T_PQ_BU_TES_D_EVAL { get; set; }
    }
}
