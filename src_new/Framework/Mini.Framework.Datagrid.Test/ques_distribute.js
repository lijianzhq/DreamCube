
$(document).ready(function () {
    //传入GridCODE，对应数据库的gridcode值（必须）
    //参数el对应页面的元素（必须）
    //getQueryParam用于返回查询参数对象（必须）
    var grid = $.datagrid.create({
        el: $('#tb_datagrid'),
        GridCODE: '123',
        getQueryParam: function () {
            return {};
        },
        EnableCellEdit: true, //是否启用单元格编辑 
        EnableRowEdit: false, //是否启用行编辑
        dataSaveUrl: "GRM0O_1703_03_DataTransfer.ashx",
        onCancelSave: function (data) {
            /// <summary>当修改数据行数获取不到的时候，底层会cancel操作，cancel前回调的方法</summary>
        },
        onBeforeSave: function (data) {
            /// <summary>保存post数据前回调；返回null可以中断底层提交数据</summary>
            data = { OpType: "save_distribute", Data: JSON.stringify(data) };
            return data;
        },
        onAfterSave: function (data) {
            /// <summary>保存post数据后回调</summary>
        }
    });

    $("#btn_save").click(function () {
        grid.save();
    });

    //初始化
    grid.init();
});
