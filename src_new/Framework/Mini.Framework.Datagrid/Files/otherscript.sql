
--这个sql模板，主要是用于查询某个datagrid的所有列，然后到处sql脚本，快速复制一个datagrid的方法
select 
'REPLAY_QUES_LIST' as gridcode
,to_char(t.config) as config
,to_char(t.editdatasql) as editdatasql
,t.orderno
,t.isenable
,t.createon
,t.lastupdateon
from T_PQ_BU_DATAGRID_COL t
where gridcode='QUES_REPLAY_ANALYSE';