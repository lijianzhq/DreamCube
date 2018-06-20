(function ($, w, udf) {
    function webuploaderbar() {
        var me = this;

        /**
         * 获取配置的方法
         * */
        this.configs = function () {
            var config = {
                index: 0,//当前的barindex（一个页面可以有多个这种bar）
                chunked: true, //分片处理大文件
                chunkSize: 2 * 1024 * 1024,
                server: 'FileSave.ashx', //接收文件数据的页面
                merge: 'MergeFile.ashx', //合并文件的页面
                disableGlobalDnd: true,
                threads: 1, //上传并发数
                //由于Http的无状态特征，在往服务器发送数据过程传递一个进入当前页面是生成的GUID作为标示
                //formData: { guid: GUID },
                fileNumLimit: 100,  //上传文件的数量限制
                compress: false, //图片在上传前不进行压缩
                fileSizeLimit: 6 * 1024 * 1024 * 1024,    // 所有文件总大小限制 6G
                fileSingleSizeLimit: 5 * 1024 * 1024 * 1024,    // 单个文件大小限制 5 G
                templateid: "webuploaderbar_htmltemplate",
                swf: me.getFolderUrl() + 'webuploader0.1.5/Uploader.swf',
            };
            var config2 = {
                pick: {
                    id: '#' + config.templateid + "_" + config.index,
                    label: '＋'
                },
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
            var winIndex = layer.open({
                type: 2,
                title: '文件上传',
                shadeClose: true,
                shade: 0.2,
                area: ['50%', '50%'],
                btn: ['开始上传', '取消'], //可以无限个按钮
                btn1: function () {
                    //alert(1);
                },
                btn2: function () {
                    layer.confirm('确定退出？', { icon: 3, title: '询问' }, function (index) {
                        //alert(index);
                        layer.close(index);
                        layer.close(winIndex);
                    });
                    return false;
                },
                maxmin: true, //开启最大化最小化按钮
                content: [bar.getFolderUrl() + 'html/uploadFileDialog.html' + "?rd=" + Math.random(), 'no']
            });
            return false;
        }
    };

    var bar = new webuploaderbar();
    var barVM = [];

    $.fn.extend({
        "makewebuploaderbar": function (config) {
            var configs = $.extend(config, bar.configs(), { index: barVM.length });
            if ($("#" + configs.templateid).length == 0) {
                var me = $(this);
                $.get(bar.getHtmlTemplateFileUrl(), { stamp: Math.random() })
                    .done(function (responseText) {
                        responseText = responseText.replace(/{templateid}/gi, configs.templateid)
                            .replace(/{addfilebtnid}/gi, configs.pick.id.substr(1));
                        me.append(responseText);
                        me.append(configs.barHtml);
                        ko.applyBindings(new viewModel());

                        //var uploader = bar.uploader.create(configs);
                        //barVM.push(ko.observable(new viewModel()));
                    });
            }
            else {
                $(this).append(barHtml);
                ko.applyBindings(barVM);
            }
        }
    });
})(jQuery, window, undefined);
