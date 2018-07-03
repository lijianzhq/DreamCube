//扩展datagrid，支持cell单元格编辑
$.extend($.fn.datagrid.methods, {
    editCell: function (jq, param) {
        return jq.each(function () {
            var dg = $(this);
            var opts = $(this).datagrid('options');
            var fields = $(this).datagrid('getColumnFields', true).concat($(this).datagrid('getColumnFields'));
            for (var i = 0; i < fields.length; i++) {
                var col = $(this).datagrid('getColumnOption', fields[i]);
                col.editor1 = col.editor;
                if (fields[i] != param.field) {
                    col.editor = null;
                }
            }
            dg.datagrid("doBeginEdit", param);
            for (var i = 0; i < fields.length; i++) {
                var col = $(this).datagrid('getColumnOption', fields[i]);
                col.editor = col.editor1;
            }
        });
    },
    editRow: function (jq, param) {
        return jq.each(function () {
            var dg = $(this);
            dg.datagrid("doBeginEdit", param);
        });
    },
    doBeginEdit: function (jq, param) {
        return jq.each(function () {
            var dg = $(this);
            dg.datagrid('beginEdit', param.index);

            var ed = $(this).datagrid('getEditor', param);
            if (ed) {
                if ($(ed.target).hasClass('textbox-f')) {
                    $(ed.target).textbox('textbox').focus();
                } else {
                    $(ed.target).focus();
                }
            }

            var keyupCallback = function (event) {
                switch (event.keyCode) {
                    case 27:
                        dg.datagrid('cancelEdit', param.index);
                        dg.datagrid("options").editIndex = undefined;
                        break;
                    case 13:
                        if (!dg.datagrid("options").goToEndEdit(param.index, param.field)) {
                            //退出编辑失败的时候 （也就是校验输入失败），重新再绑定事件）
                            $(document.body).one("keyup", keyupCallback);
                        }
                        break;
                }
            };
            //增加键盘事件
            //$(document.body).one("keyup", keyupCallback);
        });
    },
    enterEditing: function (jq) {
        return jq.each(function () {
            var dg = $(this);
            var opts = dg.datagrid('options');
            opts.oldOnClickCell = opts.onClickCell;
            opts.goToEndEdit = function (index, field) {
                if (opts.editIndex != undefined) {
                    if (dg.datagrid('validateRow', opts.editIndex)) {
                        //增加对combobox的处理
                        var ed = dg.datagrid('getEditor', { index: opts.editIndex, field: opts.editField });
                        if (ed && ed.type === "combobox") {
                            var comboboxOpts = $(ed.target).combobox('options');
                            var text = $(ed.target).combobox('getText');
                            var value = $(ed.target).combobox('getValue');
                            dg.datagrid('getRows')[opts.editIndex][comboboxOpts.valueField] = value;
                            dg.datagrid('getRows')[opts.editIndex][comboboxOpts.textField] = text;
                        }
                        if (opts.onBeforeEndEdit) opts.onBeforeEndEdit.call(dg, opts.editIndex, opts.editField);
                        dg.datagrid('endEdit', opts.editIndex);
                        opts.editIndex = undefined;
                        return true;
                    } else {
                        return false;
                    }
                }
                return true;
            };
            opts.onClickCell = function (index, field) {
                if (!opts.goToEndEdit(index, field)) {
                    dg.datagrid("unselectAll");
                }
                else {
                    dg.datagrid("unselectAll")
                        .datagrid('selectRow', index);
                    if (opts.editCell) {
                        dg.datagrid('editCell', {
                            index: index,
                            field: field
                        });
                    }
                    else {
                        dg.datagrid('editRow', {
                            index: index,
                            field: field
                        });
                    }
                    opts.editIndex = index;
                    opts.editField = field;
                }
                opts.oldOnClickCell.call(this, index, field);
            }
        });
    },
    enableCellEditing: function (jq) {
        return jq.each(function () {
            var dg = $(this);
            dg.datagrid("options").editCell = true;
            dg.datagrid("enterEditing");
        });
    },
    enableRowEditing: function (jq) {
        return jq.each(function () {
            var dg = $(this);
            dg.datagrid("enterEditing");
        });
    }
});