
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




    //$('#commentEditor').ckeditor(config);

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
        //alert($.trim($(data).text()));

        if ($.trim($(data).text()) == placeholder) {
            // hack as setData() does not work with Chrome
            e.editor.loadSnapshot('');
            e.editor.focus();
        }
    });

    $('div#commentForm').dialog({
        autoOpen: false,
        draggable: false,
        modal: true,
        resizable: false,
        zIndex: 10,
        closeText: '',
        title: '',
        width: 600,
        position: 'center'
    });

    $('a.showCommentLink').each(
    function () {

        $(this).click(
            function (e) {
                e.preventDefault();
                $(this).parents('div.post').find('div.commentView').toggle();
            });

    });

    $('a.showCommentLink').click();

    $('a', '.button').button();

    $('a.viewCommentLink').each(
    function () {

        $(this).click(
            function (e) {
                e.preventDefault();
                $('span#commentResult').text('');
                $('#_hdnFileID').attr('value', $(this).parents('div.post').attr('id'));
                $('div#commentForm').dialog('open');
            });

    });


    $('#closeCommentLink').click(function (e) {
        e.preventDefault();
        $('div#commentForm').dialog('close');
    });

    $('#addCommentLink').click(function (e) {
        //$('#commentEditor').val(CKEDITOR.instances.commentEditor.getData());
        var path = appPath + 'Comment/Create';

        //var editorText = CKEDITOR.instances.commentEditor.getData();
        var editor = CKEDITOR.instances.commentEditor;
        var editorText = editor.getData();
        if ($.trim($(editorText).text()) == $('#commentEditor').attr('placeholder')) {
            // hack as setData does not work with Chrome
            editor.loadSnapshot('');
        }

        editor.updateElement();

        // $('#commentEditor').val(editorText);


        $.post(path, $('form :input').serialize(), function (data) {
            if (data.Result == false) {
                $('#commentResult').html(data.Message);
            }
            else {
                var fileId = $('#_hdnFileID').val();
                $('#' + fileId).find('.successMessage').text(data.Message);
                setTimeout(function () { $('#' + fileId).find('.successMessage').text(''); }, 3000);

                var path = appPath + 'Post/GetComments/' + $('#_hdnFileID').val();

                $.get(path,
                    function (data) {

                        var sel = $('div.post').filter(function () {
                            if ($(this).attr('id') == $('#_hdnFileID').val()) {
                                return true;
                            }
                        });


                        sel.find('div.commentView').html(data)

                        var cmntCount = $(data).find('div.commentHeader').length;

                        var cmntText = 'No comments';

                        if (cmntCount == 1)
                            cmntText = '1 Comment';

                        if (cmntCount > 1)
                            cmntText = cmntCount.toString() + ' Comments';

                        sel.find('a.showCommentLink').html(cmntText);
                    });

                $('div#commentForm').dialog('close');

            }
        });

        e.preventDefault(true);

    });
});