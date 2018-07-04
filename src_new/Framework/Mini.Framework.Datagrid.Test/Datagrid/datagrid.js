
(function ($, win, ud) {
    function Datagrid() {
        this.config = {
            server: "Datagrid_DataTransfer.ashx",
            el: "",
            GridCODE: "",
            EnableCellEdit: false, //单击编辑（单元格编辑状态）
            EnableRowEdit: false, //单击编辑（整行编辑状态）
            getQueryParam: function () { },

            //自己额外加的一个属性，不显示行图标
            //noRowIcon: true,
            pagination: true,
            onSortColumn: function (sort, order) {
                console.log(sort, order);
            },
            onDblClickCell: function (index, field, value) {
                console.info("onDblClickCell:" + index);
            }
        };
        this.create = function (config) {
            return new EasyUIDatagrid($.extend(this.config, config));
        };
    };

    function EasyUIDatagrid(config) {
        var me = this;
        this.dg = $(config.el);
        this.config = $.extend({}, config);

        this.init = function (callback) {
            /// <summary>初始化</summary>
            var mediator = new Mediator();
            mediator.subscribe("inited", function (data) {
                //客户端then的写法
                mediator.publishThen(data);
                //客户端callback的写法
                if (callback != null) callback.call(me, data);
                //自动加载首页数据
                if (data.OpData.LoadDataAfterInital) {
                    me.load();
                }
            });
            var postData = { OpType: 'init', GridCODE: me.config.GridCODE };
            var param = { postData: postData, mediator: mediator, server: me.config.server };

            me.dg.datagrid({}).datagrid("loading");
            $.get(param.server, param.postData)
                .done(function (data, textStatus, jqXHR) {
                    console.info(data);
                    if (!data.OpResult) return;
                    if (!data.OpData.Columns || data.OpData.Columns.length == 0) return;
                    var columns = [];
                    for (var i = 0; i < data.OpData.Columns.length; i++) {
                        columns.push(eval("(" + data.OpData.Columns[i].Config + ")"));
                    }
                    console.info(columns);
                    var config = $.extend({}, me.config, data.Config, { columns: [columns] });
                    me.dg.datagrid(config);
                    //回调方法
                    var mediator = param.mediator;
                    if (mediator) mediator.publish("inited", data);
                    console.log(config);
                    //是否启用编辑
                    if (config.EnableCellEdit) {
                        me.dg.datagrid("enableCellEditing");
                    }
                    if (config.EnableRowEdit) {
                        me.dg.datagrid("enableRowEditing");
                    }
                });
            return mediator;
        };
        this.load = function (callback) {
            /// <summary>加载数据</summary>
            me.dg.datagrid("loading");
            var pager = me.dg.datagrid("getPager");
            var pagerOpts = pager.pagination("options");
            var postData = { OpType: 'loadData', GridCODE: me.config.GridCODE, pageSize: pagerOpts.pageSize, pageNumber: pagerOpts.pageNumber };
            //添加查询参数
            if (me.config.getQueryParam) postData.QueryParam = JSON.stringify(me.config.getQueryParam());
            console.log(postData.QueryParam);
            var param = { postData: postData, mediator: null, server: me.config.server };
            $.post(param.server, param.postData)
                .done(function (data, textStatus, jqXHR) {
                    console.info(data, "showdata");
                    if (!data.OpResult) return;
                    console.log(data.OpData.rows);
                    me.dg.datagrid({ data: data.OpData.rows });

                    //分页控件的处理
                    var pager = me.dg.datagrid("getPager");
                    pager.pagination({
                        onSelectPage: function (pageNumber, pageSize) {
                            pager.pagination("refresh", { pageNumber: pageNumber });
                            me.load();
                        },
                        onChangePageSize: function (pageSize) {
                            pager.pagination("refresh", { pageSize: pageSize });
                            me.load();
                        }
                    });
                    pager.pagination("refresh", { total: data.OpData.recordCount, pageNumber: pagerOpts.pageNumber });
                    //回调观察者处理
                    var mediator = param.mediator;
                    if (mediator) mediator.publish("loaded", data);
                });
        };
        this.export = function () {
            /// <summary>导出</summary>
            me.dg.datagrid("loading");
            var pager = me.dg.datagrid("getPager");
            var pagerOpts = pager.pagination("options");
            var postData = { OpType: 'exportData', GridCODE: me.config.GridCODE, pageSize: pagerOpts.pageSize, pageNumber: pagerOpts.pageNumber };
            $.post(me.config.server, postData)
                .done(function (data, textStatus, jqXHR) {
                    me.dg.datagrid("loaded");
                    var callBackBreak = false;
                    if (!data.OpResult) {
                        $.messager.alert('提醒', data.OpResult.Message);
                        return;
                    }
                    if (me.config.onExport)
                        callBackBreak = me.config.onExport.call(me, changeData);
                    //如果返回true，则中断后面的处理，否则进行后面的处理
                    if (!callBackBreak) {
                        alert(data.OpData);
                        window.open(data.OpData);
                    }
                });
        };
        this.save = function () {
            /// <summary>保存操作</summary>
            console.log(me.dg.datagrid("getData"), "showdata");
            //var changeData = me.dg.datagrid("getChanges");
            var changeData = me.dg.datagrid("getData").rows;
            console.info(changeData, "save");
            if (changeData.length === 0) {
                if (me.config.onCancelSave) me.config.onCancelSave.call(me, changeData);
                return;
            }
            if (me.config.onBeforeSave) changeData = me.config.onBeforeSave.call(me, changeData);
            if (!changeData) return;
            me.dg.datagrid("loading");
            $.post(me.config.dataSaveUrl, changeData)
                .done(function () {
                    me.dg.datagrid("loaded");
                    if (me.config.onAfterSave) me.config.onAfterSave.call(me, changeData);
                });
        };
    };

    var datagrid = new Datagrid();
    $.extend({ datagrid: datagrid });
})(jQuery, window, undefined);


