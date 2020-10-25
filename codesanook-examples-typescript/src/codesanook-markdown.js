document.addEventListener("DOMContentLoaded", function () {

    var easyMDE = new EasyMDE({
        autoDownloadFontAwesome: false,
        spellChecker: false,
        renderingConfig: {
            singleLineBreaks: true,
            codeSyntaxHighlighting: true,
        },
        promptURLs: true,
        toolbar: [
            'bold',
            'italic',
            'heading',
            '|',
            'quote',
            'unordered-list',
            'ordered-list',
            '|',
            {
                name: 'link',
                action: (editor) => {
                    var codeMirror = editor.codemirror;
                    var stat = editor.getState(codeMirror);
                    var options = editor.options;
                    var url = 'https://';
                    if (options.promptURLs) {
                        url = prompt(options.promptTexts.link, 'https://');
                        if (!url) {
                            return false;
                        }
                    }
                    replaceSelection(codeMirror, stat.link, options.insertTexts.link, url);
                },
                className: 'fa fa-link',
                title: 'Create Link',
            },
            {
                name: 'link-image',
                action: (editor) => {
                    var wmd = $('#wmd-input-Body');
                    var adminIndex = location.href.toLowerCase().indexOf("/admin/");
                    if (adminIndex === -1) return;
                    var url = location.href.substr(0, adminIndex) + "/Admin/Orchard.MediaLibrary?dialog=true";
                    $.colorbox({
                        href: url,
                        iframe: true,
                        reposition: true,
                        width: "90%",
                        height: "90%",
                        onLoad: function () {
                            // hide the scrollbars from the main window
                            $('html, body').css('overflow', 'hidden');
                        },
                        onClosed: function () {
                            $('html, body').css('overflow', '');

                            var selectedData = $.colorbox.selectedData;

                            if (selectedData == null) // Dialog cancelled, do nothing
                                return;

                            var newContent = '';
                            for (var i = 0; i < selectedData.length; i++) {
                                var renderMedia = location.href.substr(0, adminIndex) + "/Admin/Orchard.MediaLibrary/MediaItem/" + selectedData[i].id + "?displayType=Raw";
                                $.ajax({
                                    async: false,
                                    type: 'GET',
                                    url: renderMedia,
                                    success: function (data) {
                                        newContent += data;
                                    }
                                });
                            }

                            var result = $.parseHTML(newContent);
                            var img = $(result).filter('img');
                            // if this is an image, use the callback which will format it in markdown
                            if (img.length > 0 && img.attr('src')) {

                                var codeMirror = editor.codemirror;
                                var stat = editor.getState(codeMirror);
                                var options = editor.options;
                                replaceSelection(codeMirror, stat.image, options.insertTexts.image, img.attr('src'));
                            }

                            // otherwise, insert the raw HTML
                            else {
                                if (wmd.selection) {
                                    wmd.selection.replace('.*', newContent);
                                } else {
                                    wmd.text(newContent);
                                }
                                callback();
                            }
                        }
                    });
                },
                className: 'fa fa-image',
                title: 'Import Image',

            },
            '|',
            'preview',
            'side-by-side',
            'fullscreen',
            '|',
            'guide',
        ]
    });
});

function replaceSelection(codeMirror, active, startEnd, url) {

    var imageDescription = "Enter image description here";
    var linkDescription = "Enter link description here";
    if (/editor-preview-active/.test(codeMirror.getWrapperElement().lastChild.className))
        return;

    var text;
    var start = startEnd[0];
    var end = startEnd[1];
    var isImage = start === "![";
    var startPoint = {}, endPoint = {};

    Object.assign(startPoint, codeMirror.getCursor('start'));
    Object.assign(endPoint, codeMirror.getCursor('end'));
    if (url) {
        start = start.replace('#url#', url);  // url is in start for upload-image
        end = end.replace('#url#', url);
    }

    if (active) {
        text = codeMirror.getLine(startPoint.line);
        start = text.slice(0, startPoint.ch) + (isImage ? imageDescription : linkDescription);
        end = text.slice(startPoint.ch);
        codeMirror.replaceRange(start + end, {
            line: startPoint.line,
            ch: 0,
        });
    } else {
        text = codeMirror.getSelection();
        if (!text) {
            start = isImage ? '![' + imageDescription : '[' + linkDescription;
        }

        codeMirror.replaceSelection(start + text + end);
        startPoint.ch += start.length;
        if (startPoint !== endPoint) {
            endPoint.ch += start.length;
        }
    }

    codeMirror.setSelection(startPoint, endPoint);
    codeMirror.focus();
}

