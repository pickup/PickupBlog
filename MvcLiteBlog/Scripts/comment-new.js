$(document).ready(
function () {
    var css = appPath + 'Content/Admin.css';
    var config = {
        toolbar:
		[
			['Bold', 'Italic', 'Underline', '-', 'NumberedList', 'BulletedList', 'TextColor', 'BGColor', 'Indent', 'Outdent', 'JustifyLeft', 'JustifyCenter', 'JustifyRight']
		],
        height: '200px',
        entities: 'false',
        contentsCss: css
    };

    var editor = CKEDITOR.replace('commentEditor', config);

    var placeholder = $('#commentEditor').attr('placeholder');
    placeholder = "<p style=\"color:#666;\">" + placeholder + "</p>";
    editor.setData(placeholder);

    editor.on('blur', function (e) {
        var data = e.editor.getData();
        var placeholder = $('#commentEditor').attr('placeholder');
        placeholder = "<p style=\"color:#666;\">" + placeholder + "</p>";
        if (data == '') {
            e.editor.loadSnapshot(placeholder);
        }
    });

    editor.on('focus', function (e) {
        var placeholder = $('#commentEditor').attr('placeholder');
        var data = e.editor.getData();

        if ($.trim($(data).text()) == placeholder) {
            // hack as setData() does not work with Chrome
            e.editor.loadSnapshot('');
            e.editor.focus();
        }
    });

    $('#addCommentLink').click(function (e) {

        var path = appPath + 'Comment/Create';
        var editor = CKEDITOR.instances.commentEditor;
        var editorText = editor.getData();
        if ($.trim($(editorText).text()) == $('#commentEditor').attr('placeholder')) {
            editor.loadSnapshot('');
        }

        editor.updateElement();

        $.post(path, $('form :input').serialize(), function (data) {
            if (data.Result == false) {
                $('#commentResult').html(data.Message);
            }
            else {
                location.reload(true);
            }
        });
        e.preventDefault(true);
    });
});