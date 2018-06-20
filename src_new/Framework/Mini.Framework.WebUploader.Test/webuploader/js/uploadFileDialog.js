(function ($) {
    // 当domReady的时候开始初始化
    $(function () {
        var uploader,// WebUploader实例
            GUID = WebUploader.Base.guid(), //当前页面是生成的GUID作为标示
            $table = $("#tb_fileList"),
            $uploadBtn = $('#selectFileBtn');

        //初始化表格
        $table.bootstrapTable({
            classes: 'table table-hover',
            uniqueId: 'f_id',
            columns: [{
                field: '',
                title: '序号',
                align: 'center',
                halign: 'center',
                class: 'col-md-1 col-lg-1 col-xs-1',
                formatter: function (value, row, index) { return index + 1; },
            }, {
                field: 'f_name',
                title: '文件名',
                align: 'center',
                halign: 'center',
                class: 'col-md-8 col-lg-8 col-xs-8',
            }, {
                field: 'f_size2',
                title: '文件大小',
                align: 'center',
                halign: 'center',
                class: 'col-md-1 col-lg-1 col-xs-1',
            }, {
                field: 'f_status',
                title: '状态',
                align: 'center',
                halign: 'center',
                class: 'col-md-1 col-lg-1 col-xs-1',
            }, {
                field: '',
                title: '操作',
                align: 'center',
                halign: 'center',
                class: 'col-md-1 col-lg-1 col-xs-1',
                formatter: function (value, row, index) {
                    return "<a href='#' role='button' onclick='uploader.removeFile(\"" + row.f_id + "\")'>移除</a>";
                },
            }, {
                field: 'f_id',
                title: 'id',
            }]
        }).bootstrapTable('hideColumn', 'f_id');

        //重新计算宽度和高度
        var resetHeight = function () {
            var tableHeight = $(window).height() - 45 - 10;//45为按钮区域的高度，10为padding的高度
            $table.bootstrapTable('resetView', { height: tableHeight });
        };
        resetHeight();
        $(window).resize(function () {
            resetHeight();
        });

        //初始化uploader
        uploader = WebUploader.create({
            pick: {
                id: $uploadBtn,
                label: '点击选择文件'
            },
            formData: {
                uid: 123
            },
            swf: 'webuploader0.1.5/Uploader.swf',
            chunked: true, //分片处理大文件
            chunkSize: 2 * 1024 * 1024,
            server: 'FileSave.ashx',
            disableGlobalDnd: true,
            threads: 1, //上传并发数
            //由于Http的无状态特征，在往服务器发送数据过程传递一个进入当前页面是生成的GUID作为标示
            formData: { guid: GUID },
            fileNumLimit: 100,  //上传文件的数量限制
            compress: false, //图片在上传前不进行压缩
            fileSizeLimit: 6 * 1024 * 1024 * 1024,    // 所有文件总大小限制 6G
            fileSingleSizeLimit: 5 * 1024 * 1024 * 1024    // 单个文件大小限制 5 G
        });

        uploader.on('dialogOpen', function () {
            console.log('here');
        });

        uploader.on('ready', function () {
            window.uploader = uploader;
        });

        // 当有文件添加进来时执行，负责view的创建
        function addFile(file) {
            var fileRow = {
                "f_id": file.id,
                "f_name": file.name,
                "f_size": file.size,
                "f_size2": WebUploader.formatSize(file.size),
                "f_status": file.statusText,
            };
            //viewModel.files.push(fileRow);
            //$table.bootstrapTable('append', viewModel.files);
            $table.bootstrapTable('append', fileRow);
        }

        /**
         * 从数组中移除文件
         * @param {object} file webuploader的file对象
         */
        function removeFile(file) {
            $table.bootstrapTable('removeByUniqueId', file.id);
        }

        uploader.onUploadProgress = function (file, percentage) {
        };

        uploader.onFileQueued = function (file) {
            addFile(file);
        };

        uploader.onFileDequeued = function (file) {
            removeFile(file);
        };

        //all算是一个总监听器
        uploader.on('all', function (type, arg1, arg2) {
            console.log("all监听：", type, arg1, arg2);
        });

        // 文件上传成功,合并文件。
        uploader.on('uploadSuccess', function (file, response) {
            if (response.Chunked) {
                jQuery.ajax({
                    url: "MergeFile.ashx",
                    type: "post",
                    data: { guid: GUID, id: file.id, fileName: file.name },
                    dataType: "json",
                    success: function (msg) {
                        alert(msg);
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert("XMLHttpRequest.status:" + XMLHttpRequest.status);
                        alert("XMLHttpRequest.readyState:" + XMLHttpRequest.readyState);
                        alert("XMLHttpRequest.textStatus:" + textStatus);
                    },
                    complete: function (XMLHttpRequest, textStatus) {
                        this; // 调用本次AJAX请求时传递的options参数
                    }
                });
            }
        });

        uploader.onError = function (code) {
            alert('Eroor: ' + code);
        };
    });

})(jQuery);

