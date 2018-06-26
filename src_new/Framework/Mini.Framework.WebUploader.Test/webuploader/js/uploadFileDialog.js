(function ($) {
    // 当domReady的时候开始初始化
    $(function () {
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
            console.log(jsPath);
            console.log(jsPath.substring(0, jsPath.lastIndexOf('/') + 1));
            return jsPath.substring(0, jsPath.lastIndexOf('/') + 1);
        };

        /**
         * 获取html模板文件的url
         * */
        function getHtmlTemplateFileUrl() {
            var sFolderUrl = me.getFolderUrl();
            return sFolderUrl + "html/bartemplate.html";
        };

        var uploaderWrapper = function () {
            var me = this,
                refTableName, refTableCode, barCode,
                uploader,
                fileCount = 0,//等待上传的文件
                mediator,//围观者
                GUID = WebUploader.Base.guid(), //当前页面是生成的GUID作为标示
                $table = $("#tb_fileList"),
                $uploadBtn = $('#selectFileBtn');
            var configs = {
                pick: {
                    id: $uploadBtn,
                    label: '点击选择文件'
                },
                swf: getFolderUrl() + 'lib/webuploader0.1.5/Uploader.swf',
                chunked: true, //分片处理大文件
                chunkSize: 2 * 1024 * 1024,
                server: 'FileTransfer.ashx',
                merge: 'FileTransfer.ashx',
                disableGlobalDnd: true,
                threads: 1, //上传并发数
                //由于Http的无状态特征，在往服务器发送数据过程传递一个进入当前页面是生成的GUID作为标示
                formData: { guid: GUID },
                fileNumLimit: 100,  //上传文件的数量限制
                compress: false, //图片在上传前不进行压缩
                fileSizeLimit: 6 * 1024 * 1024 * 1024,    // 所有文件总大小限制 6G
                fileSingleSizeLimit: 5 * 1024 * 1024 * 1024    // 单个文件大小限制 5 G
            };

            //初始化表格
            $table.bootstrapTable({
                classes: 'table table-hover',
                uniqueId: 'f_id',
                columns: [{
                    field: '',
                    title: '序号',
                    align: 'center',
                    halign: 'center',
                    "class": 'col-md-1 col-lg-1 col-xs-1',
                    formatter: function (value, row, index) { return index + 1; }
                }, {
                    field: 'f_name',
                    title: '文件名',
                    align: 'center',
                    halign: 'center',
                    "class": 'col-md-7 col-lg-7 col-xs-7'
                }, {
                    field: 'f_size2',
                    title: '文件大小',
                    align: 'center',
                    halign: 'center',
                    "class": 'col-md-1 col-lg-1 col-xs-1'
                }, {
                    field: 'f_progress',
                    title: '进度',
                    align: 'center',
                    halign: 'center',
                    "class": 'col-md-2 col-lg-2 col-xs-2',
                    formatter: function (value, row, index) {
                        var res = row.f_progress;
                        console.log("res:" + res);
                        if (res >= 100) {
                            return ["<div class='progress'><div class='progress-bar' role='progressbar' aria-valuenow='50' aria-valuemin='0' aria-valuemax='100' style='width:" + res.toFixed(2) + "%;'>" + res.toFixed(2) + "%</div></div>"];
                        }
                        else {
                            return ["<div class='progress progress-striped active'><div class='progress-bar' role='progressbar' aria-valuenow='50' aria-valuemin='0' aria-valuemax='100' style='width:" + res.toFixed(2) + "%;'>" + res.toFixed(2) + "%</div></div>"];
                        }
                    }
                }, {
                    field: 'f_status',
                    title: '状态',
                    align: 'center',
                    halign: 'center',
                    "class": 'col-md-1 col-lg-1 col-xs-1',
                    formatter: function (value, row, index) {
                        if (value == 0) {
                            return "<a href='#' role='button' onclick='uploader.removeFile(\"" + row.f_id + "\")'>移除</a>";
                        }
                        else if (value == -1) {
                            return "<a href='#' role='button' onclick='uploader.removeFile(\"" + row.f_id + "\")'>移除</a><br><span style='font-size:7px;color:red'>出错，请重试</span>";
                        }
                        else if (value > 0 && value < 100) {
                            return '上传中...';
                        }
                        else {
                            return '已上传';
                        }
                    }
                }, {
                    field: 'f_id',
                    title: 'id'
                }]
            }).bootstrapTable('hideColumn', 'f_id');

            //重新计算宽度和高度
            var resetHeight = function () {
                var tableHeight = $(window).height() - 45 - 30;//45为按钮区域的高度，10为padding的高度
                $table.bootstrapTable('resetView', { height: tableHeight });
            };
            resetHeight();

            //妈蛋，解决window.resize在ie7会死循环的问题
            var n = 0;
            $(window).resize(function () {
                if (n % 2 == 0) {
                    resetHeight();
                }
                n++;
            });

            /**
             * 广播事件
             * @param {any} event
             */
            var publishEvent = function (event, data) {
                if (event == "uploadFinished") {
                    uploader.enable();
                }
                if (me.mediator) {
                    me.mediator.publish(event, data);
                }
            };

            //***************************************uploader控件***************************************
            //初始化uploader
            uploader = WebUploader.create(configs);

            uploader.on('dialogOpen', function () {
                console.log('here');
            });

            uploader.on('ready', function () {
                console.log('ready');
            });

            uploader.on('uploadBeforeSend', function (object, data, headers) {
                //object { Object }
                //data { Object } 默认的上传参数，可以扩展此对象来控制上传参数。
                //headers { Object } 可以扩展此对象来控制上传头部。
                //alert(JSON.stringify(object));
                //alert(JSON.stringify(data));
                //记录文件的分片数
                data.optype = 'save';
                data.RefTableName = me.refTableName;
                data.RefTableCode = me.refTableCode;
                data.BarCode = me.barCode;
                object.file.chunks = object.chunks;
            });

            uploader.on('uploadAccept', function (object, ret) {
                object.file.fileSavePath = ret.FileSavePath;
            });

            uploader.on('uploadProgress', function (file, percentage) {
                console.log("fileStatus:" + file.getStatus());
                //if (file.statusText) return;//状态说明，常在error时使用，用http, abort,server等来标记是由于什么原因导致文件错误。
                //对于分片的文件，最后还有一步合并的
                //根据文件大小，动态计算需要保留合并文件的时间进度部分
                //var times = file.size / 1024 / 1024 * 0.01;//1m预留0.01的进度部分
                //if (file.chunks > 1 && percentage > (1 - times)) {
                if (file.chunks > 1 && percentage > 0.95) {
                }
                else {
                    file.percentage = percentage;
                    updateFileProgressHtml(file, percentage);
                }
            });

            uploader.on('fileQueued', function (file) {
                //file.trigger('statuschange', WebUploader.File.Status.QUEUED, null);
                fileCount++;
                addFileHtml(file);

                //回调给围观者，告诉围观者有待上传的文件
                var num = me.getNeedUploadFilesCount();
                if (num > 0) {
                    publishEvent("fileQueued");
                }
            });

            uploader.on('fileDequeued', function (file) {
                //file.trigger('statuschange', null, WebUploader.File.Status.QUEUED);
                fileCount--;
                removeFileHtml(file);
                publishEvent("fileDequeued");
            });

            uploader.on('uploadFinished', function () {
                //控件触发的事件不算最终完成，还可能需要考虑合并文件的时间
                //publishEvent("uploadFinished");
                //uploader.enable();
            });

            uploader.on('uploadComplete', function (file) {
                publishEvent("uploadComplete", file);
            });

            //all算是一个总监听器
            uploader.on('all', function (type, arg1, arg2) {
                console.log("all监听：", type, arg1, arg2);
            });

            // 文件上传成功,合并文件。
            uploader.on('uploadSuccess', function (file, response) {
                //不需要这样判断，直接服务器端通过响应吗告诉控件失败
                //if (!response.Status) {
                //    console.log(uploader.getStats());
                //    //服务器返回失败
                //    uploader.addFiles(file);
                //    file.trigger('statuschange', WebUploader.File.Status.ERROR, WebUploader.File.Status.COMPLETE);
                //    uploader.trigger('uploadError', file, 'server');
                //    console.log(uploader.getStats());
                //    console.log(response);
                //    return;
                //}
                fileCount--;
                if (response.Chunked) {
                    //定时去刷新那个滚动条
                    var updateProgress = function () {
                        var step = 0.01;
                        console.log(file.percentage, "updateProgress");
                        if (file.percentage < 1 - step) {
                            file.percentage += step;
                            updateFileProgressHtml(file, file.percentage);
                            setTimeout(updateProgress, 100);
                        }
                    };
                    updateProgress();
                    var resetFile = function (file, reason) {
                        uploader.trigger('uploadError', file, reason);
                        //这里要处理的问题很多，暂时不解决了
                    };
                    jQuery.ajax({
                        url: configs.merge,
                        type: "post",
                        data: {
                            guid: GUID,
                            id: file.id,
                            name: file.name,
                            optype: 'merge',
                            fileSavePath: file.fileSavePath,
                            chunks: file.chunks,
                            RefTableName: me.refTableName,
                            RefTableCode: me.refTableCode,
                            BarCode: me.barCode,
                        },
                        dataType: "json",
                        success: function (msg) {
                            //alert(msg);
                            //完成合并之后要触发事件
                            if (msg.Status === true) {
                                file.FileCode = msg.FileCode;
                                updateFileSuccess(file);
                            }
                            else {
                                resetFile(file, 'server');
                            }
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            console.error('merge file error!');
                            console.error("XMLHttpRequest.status:" + XMLHttpRequest.status);
                            console.error("XMLHttpRequest.readyState:" + XMLHttpRequest.readyState);
                            console.error("XMLHttpRequest.textStatus:" + textStatus);
                            //alert("XMLHttpRequest.status:" + XMLHttpRequest.status);
                            //alert("XMLHttpRequest.readyState:" + XMLHttpRequest.readyState);
                            //alert("XMLHttpRequest.textStatus:" + textStatus);
                            resetFile(file, 'server');
                        },
                        complete: function (XMLHttpRequest, textStatus) {
                            this; // 调用本次AJAX请求时传递的options参数
                        }
                    });
                }
                else {
                    file.FileCode = response.FileCode;
                    updateFileSuccess(file);
                }
            });

            uploader.on('uploadError', function (file, reason) {
                console.error('uploadError:' + reason);
                var row = $table.bootstrapTable('getRowByUniqueId', file.id);
                row.f_status = -1;
                row.f_progress = 0;
                $table.bootstrapTable('updateRow', row);
                publishEvent("uploadError");
            });

            uploader.onError = function (code) {
                //alert('Eroor: ' + code);
                console.error('Error: ' + code);
            };

            //***************************************内部方法***************************************

            /**
            * 创建文件列表视图
            * @param {any} file webuploader的file对象
            */
            var addFileHtml = function (file) {
                var fileRow = {
                    "f_id": file.id,
                    "f_name": file.name,
                    "f_size": file.size,
                    "f_size2": WebUploader.formatSize(file.size),
                    "f_progress": 0,
                    "f_status": 0,
                    "f_op": null
                };
                $table.bootstrapTable('append', fileRow);
            };

            /**
             * 从数组中移除文件
             * @param {object} file webuploader的file对象
             */
            var removeFileHtml = function (file) {
                $table.bootstrapTable('removeByUniqueId', file.id);
            };

            /**
              * 从数组中移除文件
              * 上传文件成功调用的处理方法
              * @param {object} file webuploader的file对象
              * @param {number} percentage 上传的百分比
              */
            var updateFileProgressHtml = function (file, percentage) {
                //alert(JSON.stringify(row));
                var row = $table.bootstrapTable('getRowByUniqueId', file.id);
                row.f_progress = percentage * 100;
                $table.bootstrapTable('updateRow', row);
            };

            /**
             * 上传文件成功调用的处理方法
             * @param {object} file webuploader的file对象
             */
            var updateFileSuccess = function (file) {
                if (fileCount == 0) {
                    publishEvent("uploadFinished");
                }
                file.percentage = 1;
                publishEvent("uploadSuccess", file);
                var row = $table.bootstrapTable('getRowByUniqueId', file.id);
                row.f_status = 100;
                row.f_progress = 100;
                $table.bootstrapTable('updateRow', row);
            };

            /**
             * 文件上传成功后更新html内容
             * @param {any} file
             */
            var updateFileUploadCompleteHtml = function (file) {
            };

            //***************************************对外公开方法***************************************

            /**
             * 移除文件 
             * @param {any} sFileID webuploader的file对象的id
             */
            this.removeFile = function (sFileID) {
                uploader.removeFile(sFileID);
            };

            /**
             * 执行上传文件
             * @param {any} sFileID
             */
            this.startUpload = function () {
                uploader.disable();
                console.log(uploader.getStats());
                uploader.retry();
                uploader.upload();
            };

            /**
             * 获取队列中还没有成功上传的文件数
             * */
            this.getNeedUploadFilesCount = function () {
                //var stats = uploader.getStats();
                //alert(JSON.stringify(uploader.queue));
                //alert(JSON.stringify(stats));
                return fileCount;
            };

            /**
             * 注册围观者（用于接收页面传出去的事件）
             * @param {any} mediator
             */
            this.registerConfig = function (config) {
                me.mediator = config.Mediator;
                me.refTableCode = config.RefTableCode;
                me.refTableName = config.RefTableName;
                me.barCode = config.BarCode;
            };
        };

        //对外公开唯一接口
        window.uploader = new uploaderWrapper();
    });

})(jQuery);

