/******************************************************************************/
/*****************************SqlCe数据库**********************************/
/******************************************************************************/

/*系统全局参数表*/
CREATE TABLE SystemGlobalVal(
	id INT IDENTITY(1,1) PRIMARY KEY,
	syskey NVARCHAR(100),  /*参数的Key值*/
	value NTEXT /*参数的值*/
);


/*当前数据库版本号*/
CREATE TABLE DBVersion(
	des NVARCHAR(100), /*版本的描述内容*/
	version FLOAT, /*版本号*/
	lastUpdateTime DATETIME /*最后更新日期*/
);