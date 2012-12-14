(function ($) {

    $.fn.simpletree = function (options) {

        // Create some defaults, extending them with any options that were provided
        var settings = $.extend({
            expandAll: true,
            expandFirstItem: false,
            expandItem: '',
            expandFirstLevel: false
        }, options);

        return this.each(function () {
            $(this).addClass('tree');
            $(this).find('li:has(ul)').addClass('node expanded');
            $(this).find('li').not(':has(ul)').addClass('leaf');

            $(this).find('.leaf').click(
                function (e) {
                    e.stopPropagation();
                });

            $(this).find('.node').click(
                function (e) {
                    if ($(this).hasClass('expanded')) {
                        $(this).children('ul').hide();
                        $(this).addClass('collapsed');
                        $(this).removeClass('expanded');
                    }
                    else if ($(this).hasClass('collapsed')) {
                        $(this).children('ul').show();
                        $(this).addClass('expanded');
                        $(this).removeClass('collapsed');
                    }
                    e.stopPropagation();
                });

            if (settings.expandAll == false) {
                $(this).find('.node').each(
                    function () {
                        $(this).children('ul').hide();
                        $(this).addClass('collapsed');
                        $(this).removeClass('expanded');
                    });

                if (settings.expandFirstItem == true) {
                    $(this).children('li.node:eq(0)').find('.node').andSelf().each(
                        function () {
                            $(this).children('ul').show();
                            $(this).addClass('expanded');
                            $(this).removeClass('collapsed');
                        });
                }

                if (settings.expandItem != '') {
                    $(this).children('li').each(function () {
                        var item = $(this).clone().find('ul').remove().end().text().trim();
                        if (item == settings.expandItem) {
                            $(this).find('.node').andSelf().each(
                                function () {
                                    $(this).children('ul').show();
                                    $(this).addClass('expanded');
                                    $(this).removeClass('collapsed');
                                }
                            );
                        }
                    });
                }

                if (settings.expandFirstLevel == true) {
                    $(this).children('li.node').each(function () {
                        if($(this).hasClass('collapsed')) {
                            $(this).children('ul').show();
                            $(this).addClass('expanded');
                            $(this).removeClass('collapsed');
                        }
                    });
                }

                $(this).find('.leaf').css('list-style', 'none');
            }


        });

    };
})(jQuery);
