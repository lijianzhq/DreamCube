﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<T_DB_BU_IMPORT_TMP> T_DB_BU_IMPORT_TMP { get; set; }
        public virtual DbSet<T_DB_BU_IMPORT_TMP2> T_DB_BU_IMPORT_TMP2 { get; set; }
        public virtual DbSet<T_DB_BU_INSTRING_TMP> T_DB_BU_INSTRING_TMP { get; set; }
        public virtual DbSet<T_DB_BU_LOG_APPLICATION> T_DB_BU_LOG_APPLICATION { get; set; }
        public virtual DbSet<T_DB_BU_PART_MODEL_IMP> T_DB_BU_PART_MODEL_IMP { get; set; }
        public virtual DbSet<T_DB_DB_AFFICHE> T_DB_DB_AFFICHE { get; set; }
        public virtual DbSet<T_DB_DB_ALARM_D> T_DB_DB_ALARM_D { get; set; }
        public virtual DbSet<T_DB_DB_ALARM_M> T_DB_DB_ALARM_M { get; set; }
        public virtual DbSet<T_DB_DB_AREA> T_DB_DB_AREA { get; set; }
        public virtual DbSet<T_DB_DB_ASSEMBLY> T_DB_DB_ASSEMBLY { get; set; }
        public virtual DbSet<T_DB_DB_ATTA_FILE> T_DB_DB_ATTA_FILE { get; set; }
        public virtual DbSet<T_DB_DB_AUTO_RUN> T_DB_DB_AUTO_RUN { get; set; }
        public virtual DbSet<T_DB_DB_BAD_TYPE> T_DB_DB_BAD_TYPE { get; set; }
        public virtual DbSet<T_DB_DB_BIG_AREA> T_DB_DB_BIG_AREA { get; set; }
        public virtual DbSet<T_DB_DB_CAR_COLOR> T_DB_DB_CAR_COLOR { get; set; }
        public virtual DbSet<T_DB_DB_CAR_CONFIG> T_DB_DB_CAR_CONFIG { get; set; }
        public virtual DbSet<T_DB_DB_CAR_TYPE> T_DB_DB_CAR_TYPE { get; set; }
        public virtual DbSet<T_DB_DB_CAR_TYPE_INFO> T_DB_DB_CAR_TYPE_INFO { get; set; }
        public virtual DbSet<T_DB_DB_CAR_TYPE_INTERN> T_DB_DB_CAR_TYPE_INTERN { get; set; }
        public virtual DbSet<T_DB_DB_CITY> T_DB_DB_CITY { get; set; }
        public virtual DbSet<T_DB_DB_COST_CENTER> T_DB_DB_COST_CENTER { get; set; }
        public virtual DbSet<T_DB_DB_COUNTY> T_DB_DB_COUNTY { get; set; }
        public virtual DbSet<T_DB_DB_DEPT> T_DB_DB_DEPT { get; set; }
        public virtual DbSet<T_DB_DB_DEPT_SEC_USER> T_DB_DB_DEPT_SEC_USER { get; set; }
        public virtual DbSet<T_DB_DB_EMAIL_D> T_DB_DB_EMAIL_D { get; set; }
        public virtual DbSet<T_DB_DB_EMAIL_M> T_DB_DB_EMAIL_M { get; set; }
        public virtual DbSet<T_DB_DB_EMP> T_DB_DB_EMP { get; set; }
        public virtual DbSet<T_DB_DB_EQUIP> T_DB_DB_EQUIP { get; set; }
        public virtual DbSet<T_DB_DB_FAC_CAR_TYPE> T_DB_DB_FAC_CAR_TYPE { get; set; }
        public virtual DbSet<T_DB_DB_FAC_LINK_MAN_INFO> T_DB_DB_FAC_LINK_MAN_INFO { get; set; }
        public virtual DbSet<T_DB_DB_FAC_SEC> T_DB_DB_FAC_SEC { get; set; }
        public virtual DbSet<T_DB_DB_FACTORY> T_DB_DB_FACTORY { get; set; }
        public virtual DbSet<T_DB_DB_GSERIES_MFGSERIES> T_DB_DB_GSERIES_MFGSERIES { get; set; }
        public virtual DbSet<T_DB_DB_INNER_COMP_SEC> T_DB_DB_INNER_COMP_SEC { get; set; }
        public virtual DbSet<T_DB_DB_INNER_DUTY_COMP> T_DB_DB_INNER_DUTY_COMP { get; set; }
        public virtual DbSet<T_DB_DB_IP_INFO> T_DB_DB_IP_INFO { get; set; }
        public virtual DbSet<T_DB_DB_IP_INFO_TEMP> T_DB_DB_IP_INFO_TEMP { get; set; }
        public virtual DbSet<T_DB_DB_IP_INFO2016> T_DB_DB_IP_INFO2016 { get; set; }
        public virtual DbSet<T_DB_DB_LOG_COMP> T_DB_DB_LOG_COMP { get; set; }
        public virtual DbSet<T_DB_DB_LOOKUP_TYPE> T_DB_DB_LOOKUP_TYPE { get; set; }
        public virtual DbSet<T_DB_DB_LOOKUP_VALUE> T_DB_DB_LOOKUP_VALUE { get; set; }
        public virtual DbSet<T_DB_DB_MAC_TYPE> T_DB_DB_MAC_TYPE { get; set; }
        public virtual DbSet<T_DB_DB_NOTICE_M> T_DB_DB_NOTICE_M { get; set; }
        public virtual DbSet<T_DB_DB_NOTICE_OBJ_D> T_DB_DB_NOTICE_OBJ_D { get; set; }
        public virtual DbSet<T_DB_DB_PART> T_DB_DB_PART { get; set; }
        public virtual DbSet<T_DB_DB_PART_BILL> T_DB_DB_PART_BILL { get; set; }
        public virtual DbSet<T_DB_DB_PART_CPL_PLAN> T_DB_DB_PART_CPL_PLAN { get; set; }
        public virtual DbSet<T_DB_DB_PART_CPL_PROD> T_DB_DB_PART_CPL_PROD { get; set; }
        public virtual DbSet<T_DB_DB_PART_MODEL> T_DB_DB_PART_MODEL { get; set; }
        public virtual DbSet<T_DB_DB_PART_RANGE> T_DB_DB_PART_RANGE { get; set; }
        public virtual DbSet<T_DB_DB_PART_SWITCH> T_DB_DB_PART_SWITCH { get; set; }
        public virtual DbSet<T_DB_DB_POST> T_DB_DB_POST { get; set; }
        public virtual DbSet<T_DB_DB_PRO_LINE> T_DB_DB_PRO_LINE { get; set; }
        public virtual DbSet<T_DB_DB_PRO_LINE_D> T_DB_DB_PRO_LINE_D { get; set; }
        public virtual DbSet<T_DB_DB_PROVINCE> T_DB_DB_PROVINCE { get; set; }
        public virtual DbSet<T_DB_DB_SECTION> T_DB_DB_SECTION { get; set; }
        public virtual DbSet<T_DB_DB_SERIAL_CPL_REL> T_DB_DB_SERIAL_CPL_REL { get; set; }
        public virtual DbSet<T_DB_DB_SMALL_AREA> T_DB_DB_SMALL_AREA { get; set; }
        public virtual DbSet<T_DB_DB_SPEC_ID_PART> T_DB_DB_SPEC_ID_PART { get; set; }
        public virtual DbSet<T_DB_DB_SUPPLIER> T_DB_DB_SUPPLIER { get; set; }
        public virtual DbSet<T_DB_DB_SUPPLIER_D> T_DB_DB_SUPPLIER_D { get; set; }
        public virtual DbSet<T_DB_DB_TEAM> T_DB_DB_TEAM { get; set; }
        public virtual DbSet<T_DB_DB_USER_GROUP_D> T_DB_DB_USER_GROUP_D { get; set; }
        public virtual DbSet<T_DB_DB_USER_GROUP_M> T_DB_DB_USER_GROUP_M { get; set; }
        public virtual DbSet<T_DB_DB_VE_INFO> T_DB_DB_VE_INFO { get; set; }
        public virtual DbSet<T_DB_DB_WORKSHOP> T_DB_DB_WORKSHOP { get; set; }
        public virtual DbSet<T_DB_DB_FAC_SEC171125> T_DB_DB_FAC_SEC171125 { get; set; }
        public virtual DbSet<T_DB_DB_INNER_COMP_SEC171125> T_DB_DB_INNER_COMP_SEC171125 { get; set; }
        public virtual DbSet<T_DB_DB_INNER_DUTY_COMP171125> T_DB_DB_INNER_DUTY_COMP171125 { get; set; }
        public virtual DbSet<T_DB_DB_IP_INFO2> T_DB_DB_IP_INFO2 { get; set; }
        public virtual DbSet<T_DB_DB_ROLE_MENU> T_DB_DB_ROLE_MENU { get; set; }
    }
}