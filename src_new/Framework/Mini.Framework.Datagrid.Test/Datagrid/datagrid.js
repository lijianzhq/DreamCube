
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
                    //me.config是默认值，data.config是返回的值，最后的是返回的列参数
                    columns = [{ field: 'DISTRI_GROUP_CODE', title: '分发目标班组', width: 180 },
                    {
                        field: 'EFFECT_CONF_SCHE', title: '效果确认方案', width: 180,
                        formatter: function (value, row) {
                            return row.EFFECT_CONF_SCHE_NAME; //return返回的必须是textField字段的值
                        },
                        editor: {
                            type: 'combobox',
                            options: {
                                editable: false,
                                valueField: 'EFFECT_CONF_SCHE', //必须与列的field字段一致
                                textField: 'EFFECT_CONF_SCHE_NAME',
                                onBeforeLoad: function (data) {
                                    var editor = $(this);
                                    var postData = { OpType: 'loadColumnEditData', GridCODE: me.config.GridCODE, FieldCODE: 'EFFECT_CONF_SCHE' };
                                    data = $.extend(data, postData);
                                },
                                loadFilter: function (data) {
                                    if (data.OpResult) {
                                        return data.OpData;
                                    }
                                    return [];
                                },
                                url: me.config.server,
                                required: true
                            }
                        }
                    },
                    {
                        field: '问题点', title: '问题点', width: 180,
                        editor: {
                            type: 'text'
                        }
                    },
                    { field: '发生日期', title: '发生日期', width: 60, align: 'right' },
                    { field: '品情来源', title: '品情来源', width: 80 },
                    { field: '车型', title: '车型', width: 80 },
                    { field: '车号', title: '车号', width: 80 },
                    { field: '不良区分', title: '不良区分', width: 80 },
                    { field: '发生工站', title: '发生工站', width: 80 },
                    { field: '责任人', title: '责任人', width: 80 },
                    { field: '不良件数', title: '不良件数', width: 80 },
                    { field: '是否再发', title: '是否再发', width: 80 }];
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


