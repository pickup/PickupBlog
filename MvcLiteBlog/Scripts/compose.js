

function init (composeEditor) {

    var css = appPath + 'Content/Admin.css';

    var config = {
        toolbar:
		[
			['Source', 'Bold', 'Italic', 'Underline',  '-', 'NumberedList', 'BulletedList', '-', 'Link', 'Unlink', 'TextColor', 'BGColor', 'CreateDiv', 'ShowBlocks',  'AddCode'], '/',
            ['Paste', 'PasteText', 'Find', 'Replace',  'Indent', 'Outdent', 'JustifyLeft','JustifyCenter','JustifyRight','JustifyBlock', 'Image', 'Font', 'FontSize']
		],
        height: '400px',
        entities : 'false',
        contentsCss : css
    };


    var editor = CKEDITOR.replace(composeEditor, config);

    editor.on('pluginsLoaded', function (ev) {
        // If our custom dialog has not been registered, do that now.
        if (!CKEDITOR.dialog.exists('addCodeDialog')) {
            // We need to do the following trick to find out the dialog
            // definition file URL path. In the real world, you would simply
            // point to an absolute path directly, like "/mydir/mydialog.js".
            var href = appPath + 'Scripts/addCodeDialog.js';

            // Finally, register the dialog.
            CKEDITOR.dialog.add('addCodeDialog', href);
        }

        // Register the command used to open the dialog.
        editor.addCommand('addCodeDialogCmd', new CKEDITOR.dialogCommand('addCodeDialog'));

        // Add the a custom toolbar buttons, which fires the above
        // command..
        editor.ui.addButton('AddCode',
						{
						    label: 'Add Code',
						    command: 'addCodeDialogCmd'
						});
    });

}



