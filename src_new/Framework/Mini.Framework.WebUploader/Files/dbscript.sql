DROP trigger TR_T_PQ_BU_UPLOADFILEOPHISTORY;
DROP SEQUENCE SQ_T_PQ_BU_UPLOADFILEOPHISTORY;
DROP TABLE T_PQ_BU_UPLOADFILEOPHISTORY;
DROP TABLE T_PQ_BU_UPLOADFILE;

-- Create table
create table T_PQ_BU_UPLOADFILE
(
  CODE         NVARCHAR2(50) not null,
  FILENAME     NVARCHAR2(500),
  REFTABLENAME NVARCHAR2(50),
  REFTABLECODE NVARCHAR2(50),
  BARCODE      NVARCHAR2(50),
  SAVEPATH     NVARCHAR2(500),
  STATUS       NUMBER(10) not null,
  ATTRIBUTE1   NVARCHAR2(2000),
  ATTRIBUTE2   NVARCHAR2(2000),
  ATTRIBUTE3   NVARCHAR2(2000),
  ATTRIBUTE4   NVARCHAR2(2000),
  ATTRIBUTE5   NVARCHAR2(2000),
  CREATEON     DATE not null,
  LASTUPDATEON DATE not null,
  CREATEBY     NVARCHAR2(100),
  LASTUPDATEBY NVARCHAR2(100),
  ISENABLE     NUMBER(1)  DEFAULT '1' NULL,
  ORDERNO      NUMBER(10) not null
);
-- Create/Recreate primary, unique and foreign key constraints 
alter table T_PQ_BU_UPLOADFILE
  add constraint PK_T_PQ_BU_UPLOADFILE primary key (CODE)
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
create table T_PQ_BU_UPLOADFILEOPHISTORY
(
  ID             NUMBER(10) not null,
  UPLOADFILECODE NVARCHAR2(50),
  OPTYPE         NUMBER(10) not null,
  CREATEON       DATE not null,
  LASTUPDATEON   DATE not null,
  CREATEBY       NVARCHAR2(100),
  LASTUPDATEBY   NVARCHAR2(100),
  ISENABLE     NUMBER(1)  DEFAULT '1' NULL,
  ORDERNO        NUMBER(10) not null
);
-- Create/Recreate primary, unique and foreign key constraints 
alter table T_PQ_BU_UPLOADFILEOPHISTORY
  add constraint PK_T_PQ_BU_UPLOADFILEOPHISTORY primary key (ID)
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
alter table T_PQ_BU_UPLOADFILEOPHISTORY
  add constraint FK_T_PQ_BU_UPLOADFI_1070844926 foreign key (UPLOADFILECODE)
  references T_PQ_BU_UPLOADFILE (CODE);
-- Create/Recreate indexes 
create index IX_T_PQ_BU_UPLOADFIL_491304152 on T_PQ_BU_UPLOADFILEOPHISTORY (UPLOADFILECODE)
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
create sequence SQ_T_PQ_BU_UPLOADFILEOPHISTORY
minvalue 1
maxvalue 9999999999999999999999999999
start with 21
increment by 1
cache 20;

create or replace trigger TR_T_PQ_BU_UPLOADFILEOPHISTORY
before insert on T_PQ_BU_UPLOADFILEOPHISTORY
for each row
begin
  select SQ_T_PQ_BU_UPLOADFILEOPHISTORY.nextval into :new.ID from dual;
end;