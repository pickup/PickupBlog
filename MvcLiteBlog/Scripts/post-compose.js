$(document).ready(
            function () {

                // initialize editor
                init('PostEditor');

                // initialize calendar control
                $('input[name=PublishDate]').datepicker();

                // click handler for saving
                $('input[name=Save]').click(
                    function (e) {

                        // prevent submission
                        e.preventDefault();

                        var url = appPath + "Post/Save";

                        $.post(
                                url,
                                $('form :input').serialize(),
                                function (data) {
                                    $('#saveMessage').html(data);
                                });
                    });

                // trigger auto-save every minute
                setInterval(function () {
                    $('input[name=Save]').click();
                }, 60000);


                // click handler for deleting attachments
                $('.deleteAttachment').live("click",
                    function (e) {

                        e.preventDefault();

                        var docId = $(this).parent().siblings().text();
                        docId = docId.substring(docId.lastIndexOf('/') + 1);

                        var url = appPath + "Post/DeleteAttachment/" + docId;
                        $.get(url,
                              function (data) {
                                  $('#attachments').replaceWith(data);
                              });

                    });

            });