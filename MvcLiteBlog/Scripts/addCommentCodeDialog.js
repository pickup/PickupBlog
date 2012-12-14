CKEDITOR.dialog.add('addCodeDialog', function (editor) {
    return {
        title: 'Add Code Snippet',
        minWidth: 500,
        minHeight: 300,


        onOk: function () {
            var editor = this.getParentEditor();
            var pre = new CKEDITOR.dom.element('pre');
            pre.setAttributes({
                'class': 'commentCode',
                'language': this.getContentElement('addCodeTab', 'language').getValue()
            })

            // it encodes automatically
            pre.setText(this.getContentElement('addCodeTab', 'codeSnippet').getValue());
            editor.insertElement(pre);
        },

        contents: [
			{
			    id: 'addCodeTab',
			    label: 'Add Code',
			    title: 'Add Code',
			    elements:
				[
					{
					    id: 'codeSnippet',
					    type: 'textarea',
					    label: 'Code snippet',
					    rows: 10,
					    cols: 50,
					    onShow: function () {
					        var editor = this.getDialog().getParentEditor();
					        var elem = editor.getSelection().getStartElement();
					        if (elem.getName() == 'pre') {
					            this.setValue(elem.getText());
					            var lang = elem.getAttribute('language');
					            this.getDialog().getContentElement('addCodeTab', 'language').setValue(lang);
					        }
					    }
					},
                    {
                        id: 'language',
                        type: 'select',
                        label: 'Language',
                        items:
							[

								['C#', 'C#'],
								['VB.NET', 'VB.NET'],
								['HTML', 'HTML'],
								['JScript', 'JScript'],
								['XML', 'XML'],
                                ['SQL', 'SQL']
							]
                    }
				]
			}
		]
    };
});
