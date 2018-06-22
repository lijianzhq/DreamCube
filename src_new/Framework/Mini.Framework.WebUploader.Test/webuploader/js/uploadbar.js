(function ($, w, udf) {
    //获得当前js文件的url
    var currentPath = document.currentScript ? document.currentScript.src : function () {
        var js = document.scripts
            , last = js.length - 1
            , src;
        for (var i = last; i > 0; i--) {
            if (js[i].readyState === 'interactive') {
                src = js[i].src;
                break;
            }
        }
        return src || js[last].src;
    }();
    /**
    * 获取本插件所在的web的目录url
    * */
    function getFolderUrl() {
        //获得js文件的path目录路径，再往上递归一层，就是插件路径了
        var jsPath = currentPath.substring(0, currentPath.lastIndexOf('/') + 1);
        if (jsPath.lastIndexOf("/") === jsPath.length - 1)
            jsPath = jsPath.substring(0, jsPath.length - 1);
        //console.log(jsPath.substring(0, jsPath.lastIndexOf('/') + 1));
        return jsPath.substring(0, jsPath.lastIndexOf('/') + 1);
    };

    /**
     * 获取html模板文件的url
     * */
    function getHtmlTemplateFileUrl() {
        var sFolderUrl = getFolderUrl();
        return sFolderUrl + "html/bartemplate.html";
    };

    function uploadbar() {
        var me = this;
        /**
         * 获取配置的方法
         * */
        this.defaultConfigs = function () {
            var config = {
                index: 0,//当前的barindex（一个页面可以有多个这种bar）
                templateid: "uploadbar_htmltemplate",
                serverurl: getFolderUrl() + "DataTransfer.ashx"
            };
            var config2 = {
                barHtml: "<div data-bind=\"template: '" + config.templateid + "'\"></div>"
            };
            return $.extend(config, config2);
        };

        this.ready = function (callback) {
            me.callback = callback;
        };
    };

    function viewModel(refTableName, refTableCode, barCode, configs) {
        this.configs = configs;
        this.refTableName = refTableName;
        this.refTableCode = refTableCode;
        this.barCode = barCode;
        this.checkedAll = false;
        //以下是方法
        this.addFile = function () {
            var me = this,
                mediator = new Mediator(),
                winIndex;//弹出窗口的index值

            mediator.subscribe("uploadFinished", function () {
                //alert('uploadFinished');
                //$(".layui-layer-btn0").show();
                //$(".layui-layer-btn1").hide();
                layer.close(winIndex);
            });
            mediator.subscribe("fileQueued", function () {
                $(".layui-layer-btn0").show();
            });
            mediator.subscribe("fileDequeued", function () {
                var num = me.iframeWin.uploader.getNeedUploadFilesCount();
                if (num <= 0) $(".layui-layer-btn0").hide();
            });
            mediator.subscribe("uploadError", function () {
                $(".layui-layer-btn0").show();
            });
            mediator.subscribe("uploadSuccess", function (file) {
                //me.files.push({
                //    "FileName": file.name
                //});
                me.appendNewFile({
                    FileName: file.name,
                    CODE: file.FileCode
                });
            });

            mediator.subscribe("uploadComplete", function (file) {

            });

            this.winConfig = {
                type: 2,
                title: '文件上传',
                shadeClose: true,
                shade: 0.2,
                area: ['65%', '60%'],
                btn: ['开始上传', '正在上传', '关闭'], //可以无限个按钮
                btn1: function () {
                    //alert(me.iframeWin)
                    //alert(me.iframeWin.uploader)
                    $(".layui-layer-btn0").hide();
                    //$(".layui-layer-btn1").show();
                    me.iframeWin.uploader.startUpload();
                },
                btn2: function () {
                    return false;
                },
                btn3: function (index, layero) {
                    me.winConfig.cancel();
                    return false;
                },
                success: function (layero, index) {
                    var body = layer.getChildFrame('body', index);
                    var iframeWin = window[layero.find('iframe')[0]['name']]; //得到iframe页的窗口对象，执行iframe页的方法：iframeWin.method();    
                    me.iframeWin = iframeWin;
                    //注册围观者
                    $(me.iframeWin.document).ready(function () {
                        me.iframeWin.uploader.registerConfig({ "Mediator": mediator, "RefTableName": refTableName, "RefTableCode": refTableCode, "BarCode": barCode });
                    });

                    //隐藏按钮
                    $(".layui-layer-btn0").hide();
                    $(".layui-layer-btn1").addClass("layui-btn-disabled").hide()
                },
                cancel: function (index, layero) {
                    var num = me.iframeWin.uploader.getNeedUploadFilesCount();
                    //alert(num);
                    if (num > 0) {
                        layer.confirm('还有未成功上传的文件，确定退出吗？', { icon: 0, title: '询问' }, function (index) {
                            layer.close(index);
                            layer.close(winIndex);
                        });
                    }
                    else {
                        layer.close(index);
                        layer.close(winIndex);
                    }
                    return false;
                },
                maxmin: true, //开启最大化最小化按钮
                content: [getFolderUrl() + 'html/uploadFileDialog.html', 'no']
            };
            winIndex = layer.open(this.winConfig);
            //隐藏按钮
            $(".layui-layer-btn0").hide();
            $(".layui-layer-btn1").addClass("layui-btn-disabled").hide()
            return false;
        };
        this.removeFile = function () {
            var checkFileCode = [];
            var me = this;
            for (var i = 0; i < me.files().length; i++) {
                var item = me.files()[i]();
                if (item.selected() === true) {
                    checkFileCode.push(item.CODE);
                }
            }
            if (checkFileCode.length > 0) {
                layer.confirm('此操作不可恢复，确定要删除选定附件吗？', { icon: 0, title: '询问' }, function (index) {
                    $.post(me.configs.serverurl, { optype: 'deleteFile', RmFCodes: JSON.stringify(checkFileCode) })
                        .done(function (response) {
                            if (response.Status === true) {
                                me.files.remove(function (item) {
                                    return item().selected();
                                });
                            }
                            layer.close(index);
                        });
                });
            }
        };
        this.selectFile = function () {
            var me = this;
            for (var i = 0; i < me.files().length; i++) {
                me.files()[i]().selected(!me.checkedAll);
            }
            me.checkedAll = !me.checkedAll;
        };
        //这里的file数字，对象要对应数据库对象字段，以数据库对象字段为准绑定页面
        this.files = ko.observableArray();
        this.appendNewFile = function (file) {
            var me = this;
            if (file.selected === undefined)
                file.selected = ko.observable(false);
            me.files.push(ko.observable(file));
        };
    };

    var bar = new uploadbar();

    var css = [getFolderUrl() + 'lib/layer/theme/default/layer.css'
        , getFolderUrl() + 'css/uploadbar.css'];
    var loadCSS = function (urls) {
        if (!(urls && urls.length)) return;
        for (var i = 0; i < urls.length; i++) {
            $("head").append("<link>");
            var css = $("head").children(":last");
            css.attr({
                rel: "stylesheet",
                type: "text/css",
                href: urls[i]
            });
        }
    };
    loadCSS(css);

    var rs = [getFolderUrl() + 'lib/compatibility/console.js'
        , getFolderUrl() + 'lib/knockout/knockout-3.4.2-min.js'
        , getFolderUrl() + 'lib/json2.js'
        , getFolderUrl() + 'lib/layer/layer-min.js'
        , getFolderUrl() + 'lib/mediator.js'];

    //加载依赖的资源文件
    $.when($.getScript(rs[0]), $.getScript(rs[1]), $.getScript(rs[2]), $.getScript(rs[3]), $.getScript(rs[4]))
        .done(function () {
            if (bar.callback)
                bar.callback();
        });

    $.extend({ uploadbar: bar });

    $.fn.extend({
        "makeuploadbar": function (sTableName, sPrimaryKey, sBarCode, config) {
            var configs = $.extend(config, bar.defaultConfigs());
            var $barContainer = $(this);
            var df = $.Deferred();
            var loadTemplate = function (df) {
                var tasks = function () {
                    //首先需要加载模板html（模板html只需要加载一次即可）
                    if ($("#" + configs.templateid).length > 0) df.resolve();
                    $.get(getHtmlTemplateFileUrl())
                        .done(function (responseText) {
                            responseText = responseText.replace(/{templateid}/gi, configs.templateid);
                            $(document.body).append(responseText);
                            df.resolve();
                        });
                };
                setTimeout(tasks, 1);
                return df.promise();
            };
            $.when(loadTemplate(df))
                .done(function () {
                    //加载模板完毕则生成附件栏
                    $barContainer.append(configs.barHtml);
                    var model = new viewModel(sTableName, sPrimaryKey, sBarCode, configs);
                    ko.applyBindings(model, $barContainer[0]);
                    //同时加载附件栏的附件
                    $.get(configs.serverurl, { optype: 'loadFile', RefTableName: sTableName, RefTableCode: sPrimaryKey, BarCode: sBarCode })
                        .done(function (response) {
                            var files = response.Result;
                            for (var i = 0; i < files.length; i++) {
                                //alert(JSON.stringify(responseText[i]));
                                //model.files.push(files[i]);
                                model.appendNewFile(files[i]);
                            }
                        });
                })
                .fail(function () { alert("error") });
        }
    });
})(jQuery, window, undefined);
