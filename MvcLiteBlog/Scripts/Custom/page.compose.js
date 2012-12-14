$(function () {

	// initialize the editor
	var css = appPath + 'Content/Admin.css';

	var config = {
		toolbar:
		[
			['Source', 'Bold', 'Italic', 'Underline', '-', 'NumberedList', 'BulletedList', '-', 'Link', 'Unlink', 'TextColor', 'BGColor', 'CreateDiv', 'ShowBlocks'], '/',
            ['Paste', 'PasteText', 'Find', 'Replace', 'Indent', 'Outdent', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock', 'Image', 'Font', 'FontSize']
		],
		height: '400px',
		entities: 'false',
		contentsCss: css
	};


	var editor = CKEDITOR.replace('PageEditor', config);

	// click handler for saving
	$('input[name=Save]').click(
		function (e) {

			// prevent submission
			e.preventDefault();

			var url = appPath + "Page/Save";

			$('#PageEditor').val(editor.getData());

			$.post(
					url,
					$('form :input').serialize(),
					function (data) {
						$('input[name=FileId]').val(data.FileId);
						$('#saveMessage').html(data.Content);
					});
		});

	// trigger auto-save every minute
	setInterval(function () {
		$('input[name=Save]').click();
	}, 60000);

});