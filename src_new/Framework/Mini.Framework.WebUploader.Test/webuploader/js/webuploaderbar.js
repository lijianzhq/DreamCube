(function ($, w, udf) {
    function webuploaderbar() {
        var me = this;

        /**
         * 获取配置的方法
         * */
        this.configs = function () {
            var config = {
                index: 0,//当前的barindex（一个页面可以有多个这种bar）
                templateid: "webuploaderbar_htmltemplate",
                serverurl: me.getFolderUrl() + "html/fileData.json",
            };
            var config2 = {
                barHtml: "<div data-bind=\"template: '" + config.templateid + "'\"></div>"
            };
            return $.extend(config, config2);
        };

        /**
        * 获取本插件所在的web的目录url
        * */
        this.getFolderUrl = function () {
            var scripts = document.scripts;
            for (var i = 0; i < scripts.length; i++) {
                var js = scripts[i];
                var index = js.src.lastIndexOf('/webuploader/js/webuploaderbar.js');
                if (index > 0) {
                    return js.src.substr(0, index) + "/webuploader/";
                }
            }
            console.log("folder name[webuploader] or file name [webuploaderbar.js] has been changed!");
            return "";
        };

        /**
         * 获取html模板文件的url
         * */
        this.getHtmlTemplateFileUrl = function () {
            var sFolderUrl = me.getFolderUrl();
            return sFolderUrl + "html/bartemplate.html";
        };

        function Uploader() {
            var parent = me;
            this.create = function (configs) {
                // 实例化
                var uploader = WebUploader.create(configs);
                uploader.vm = {
                    files: []
                };//viewmodel
                uploader.onFileQueued = function (file) {
                    var it = uploader;
                    it.vm.files.push(file);
                };
                return uploader;
            }
        };
        this.uploader = new Uploader();
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

            this.winConfig = {
                type: 2,
                title: '文件上传',
                shadeClose: true,
                shade: 0.2,
                area: ['65%', '55%'],
                btn: ['开始上传', '正在上传', '取消'], //可以无限个按钮
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
                content: [bar.getFolderUrl() + 'html/uploadFileDialog.html' + "?rd=" + Math.random(), 'no']
            };
            winIndex = layer.open(this.winConfig);
            //隐藏按钮
            $(".layui-layer-btn0").hide();
            $(".layui-layer-btn1").addClass("layui-btn-disabled").hide()
            return false;
        };
        this.files = ko.observableArray();
    };

    var bar = new webuploaderbar();

    $.fn.extend({
        "makewebuploaderbar": function (sTableName, sPrimaryKey, config) {
            var configs = $.extend(config, bar.configs());
            var $barContainer = $(this);
            var df = $.Deferred();
            var loadTemplate = function (df) {
                var tasks = function () {
                    //首先需要加载模板html（模板html只需要加载一次即可）
                    if ($("#" + configs.templateid).length > 0) df.resolve();
                    $.get(bar.getHtmlTemplateFileUrl())
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
