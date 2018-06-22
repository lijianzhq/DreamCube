
-- Create table
create table UPLOADFILE
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
  ORDERNO      NUMBER(10) not null
);
-- Create/Recreate primary, unique and foreign key constraints 
alter table UPLOADFILE
  add constraint PK_UPLOADFILE primary key (CODE)
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
create table UPLOADFILEOPHISTORY
(
  ID             NUMBER(10) not null,
  UPLOADFILECODE NVARCHAR2(50),
  OPTYPE         NUMBER(10) not null,
  CREATEON       DATE not null,
  LASTUPDATEON   DATE not null,
  CREATEBY       NVARCHAR2(100),
  LASTUPDATEBY   NVARCHAR2(100),
  ORDERNO        NUMBER(10) not null
)
tablespace GUIYANG
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
-- Create/Recreate primary, unique and foreign key constraints 
alter table UPLOADFILEOPHISTORY
  add constraint PK_UPLOADFILEOPHISTORY primary key (ID)
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
alter table UPLOADFILEOPHISTORY
  add constraint FK_UPLOADFILEOPHIST_1708377677 foreign key (UPLOADFILECODE)
  references UPLOADFILE (CODE);
-- Create/Recreate indexes 
create index IX_UPLOADFILEOPHISTO_390456111 on UPLOADFILEOPHISTORY (UPLOADFILECODE)
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
create sequence SQ_UPLOADFILEOPHISTORY
minvalue 1
maxvalue 9999999999999999999999999999
start with 21
increment by 1
cache 20;

--触发器
create or replace trigger TR_UPLOADFILEOPHISTORY
before insert on UPLOADFILEOPHISTORY
for each row
begin
  select SQ_UPLOADFILEOPHISTORY.nextval into :new.ID from dual;
end;