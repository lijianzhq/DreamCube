(function ($, w, udf) {
    /**
    * 获取本插件所在的web的目录url
    * */
    function getFolderUrl() {
        var scripts = document.scripts;
        for (var i = 0; i < scripts.length; i++) {
            var js = scripts[i];
            var index = js.src.lastIndexOf('/webuploader/js/uploadbar.js');
            if (index > 0) {
                return js.src.substr(0, index) + "/webuploader/";
            }
        }
        console.log("folder name[webuploader] or file name [uploadbar.js] has been changed!");
        return "";
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
        this.configs = function () {
            var config = {
                index: 0,//当前的barindex（一个页面可以有多个这种bar）
                templateid: "uploadbar_htmltemplate",
                serverurl: getFolderUrl() + "html/fileData.json",
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

    function viewModel() {
        this.addFile = function () {
            var me = this,
                mediator = new Mediator(),
                winIndex;//弹出窗口的index值

            mediator.subscribe("uploadFinished", function () {
                //alert('uploadFinished');
                //$(".layui-layer-btn0").show();
                //$(".layui-layer-btn1").hide();
            });
            mediator.subscribe("fileQueued", function () {
                $(".layui-layer-btn0").show();
            });
            mediator.subscribe("uploadError", function () {
                $(".layui-layer-btn0").show();
                $(".layui-layer-btn0").text("重试");
            });
            mediator.subscribe("uploadComplete", function (file) {
                me.files.push({
                    "fileName": file.name
                });
            });

            this.winConfig = {
                type: 2,
                title: '文件上传',
                shadeClose: true,
                shade: 0.2,
                area: ['65%', '55%'],
                btn: ['开始上传', '正在上传', '关闭'], //可以无限个按钮
                btn1: function () {
                    //alert(me.iframeWin)
                    //alert(me.iframeWin.uploader)
                    //alert(me.iframeWin.uploader);
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
                        me.iframeWin.uploader.registerMediator(mediator);
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
        this.files = ko.observableArray();
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

    var rs = [getFolderUrl() + 'lib/webuploader0.1.5/webuploader.js'
        , getFolderUrl() + 'lib/knockout/knockout-3.4.2.js'
        , getFolderUrl() + 'lib/layer/layer.js'
        , getFolderUrl() + 'js/mediator.js'];

    //加载依赖的资源文件
    $.when($.getScript(rs[0]), $.getScript(rs[1]), $.getScript(rs[2]), $.getScript(rs[3]))
        .done(function () {
            if (bar.callback)
                bar.callback();
        });

    $.extend({ uploadbar: bar });

    $.fn.extend({
        "makeuploadbar": function (sTableName, sPrimaryKey, config) {
            var configs = $.extend(config, bar.configs());
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
                    var model = new viewModel();
                    ko.applyBindings(model);
                    //同时加载附件栏的附件
                    $.get(configs.serverurl)
                        .done(function (responseText) {
                            for (var i = 0; i < responseText.length; i++) {
                                //alert(JSON.stringify(responseText[i]));
                                model.files.push(responseText[i]);
                            }
                        });
                })
                .fail(function () { alert("error") });
        }
    });
})(jQuery, window, undefined);
