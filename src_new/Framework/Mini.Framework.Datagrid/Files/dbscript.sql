
DROP TRIGGER TR_T_PQ_BU_DATAGRID_COL;
DROP TABLE T_PQ_BU_DATAGRID_COL;
DROP TABLE  T_PQ_BU_DATAGRID;
DROP SEQUENCE SQ_T_PQ_BU_DATAGRID_COL;

-- Create table
create table T_PQ_BU_DATAGRID
(
  CODE                NVARCHAR2(128) not null,
  CONFIG              NCLOB,
  SQL                 NCLOB,
  LOADDATAAFTERINITAL NUMBER(1) not null,
  CREATEON            DATE not null,
  LASTUPDATEON        DATE not null,
  CREATEBY            NVARCHAR2(100),
  LASTUPDATEBY        NVARCHAR2(100),
  ORDERNO             NUMBER(10) not null
);
-- Create/Recreate primary, unique and foreign key constraints 
alter table T_PQ_BU_DATAGRID
  add constraint PK_T_PQ_BU_DATAGRID primary key (CODE)
  using index 
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

  -- Create table
create table T_PQ_BU_DATAGRID_COL
(
  ID                    NUMBER(10) not null,
  GRIDCODE              NVARCHAR2(2000),
  CONFIG                NVARCHAR2(2000),
  EDITDATASQL           NVARCHAR2(2000),
  CREATEON              DATE not null,
  LASTUPDATEON          DATE not null,
  CREATEBY              NVARCHAR2(100),
  LASTUPDATEBY          NVARCHAR2(100),
  ORDERNO               NUMBER(10) not null,
  T_PQ_BU_DATAGRID_CODE NVARCHAR2(128)
);
-- Create/Recreate primary, unique and foreign key constraints 
alter table T_PQ_BU_DATAGRID_COL
  add constraint PK_T_PQ_BU_DATAGRID_COL primary key (ID)
  using index 
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table T_PQ_BU_DATAGRID_COL
  add constraint FK_T_PQ_BU_DATAGRID_1749833684 foreign key (T_PQ_BU_DATAGRID_CODE)
  references T_PQ_BU_DATAGRID (CODE);
-- Create/Recreate indexes 
create index IX_T_PQ_BU_DATAGRID_1754199859 on T_PQ_BU_DATAGRID_COL (T_PQ_BU_DATAGRID_CODE)
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

  -- Create sequence 
create sequence SQ_T_PQ_BU_DATAGRID_COL
minvalue 1
maxvalue 9999999999999999999999999999
start with 21
increment by 1
cache 20;


create or replace trigger TR_T_PQ_BU_DATAGRID_COL
before insert on T_PQ_BU_DATAGRID_COL
for each row
begin
  select SQ_T_PQ_BU_DATAGRID_COL.nextval into :new.ID from dual;
end;