(function ($) {
    $.workmate.ajax = {
        processError: function (response) {
            switch (response.ExceptionType) {
                case "System.Security.SecurityException":
                    $.workmate.systemMessages.showMessage($.workmate.systemMessages.error, $.workmate.global.text.permissionDenied, $.workmate.systemMessages.displayType.alert);
                    break;
                default:
                    $.ajax(
        {
            type: "POST",
            url: $.workmate.global.services.logController.url + $.workmate.global.services.logController.methods.LogException,
            data: "{ message: '" + response.Message.replace(/'/g, "\\'") + "', exceptionType : '" + response.ExceptionType.replace(/'/g, "\\'") + "' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        });
                    $.workmate.systemMessages.showMessage($.workmate.systemMessages.error, $.workmate.global.text.generalError, $.workmate.systemMessages.displayType.alert);
                    break
            }
        },
        onAjaxError: function (xmlHttpRequest) {
            this.processError(json_parse(xmlHttpRequest.responseText))
        },
        onAjaxErrorWithThrobber: function (xmlHttpRequest, senderId) {
            this.processError(json_parse(xmlHttpRequest.responseText));
            if (senderId !== null) {
                $.workmate.throbber.hideThrobber(senderId)
            }
        }
    }
})(jQuery);
(function ($) {
    $.workmate.throbber = {
        throbberOptions: {
            Image: $.workmate.global.paths.throbber,
            Delay: 0
        },
        getthrobberId: function (id) {
            return id + '_throbber'
        },
        showThrobber: function (id, throbberClass, throbberImage) {
            if (throbberClass === null || throbberClass == "undefined") {
                throbberClass = ""
            }
            if (throbberImage === null || throbberImage == "undefined") {
                throbberImage = $.workmate.throbber.throbberOptions.Image
            }
            var elementToHide = $("#" + id);
            window.clearTimeout(elementToHide.data("throbber_timeout"));
            elementToHide.data("throbber_timeout", window.setTimeout(function () {
                var throbber = $('<img src="' + throbberImage + '" id="' + $.workmate.throbber.getthrobberId(elementToHide.attr("id")) + '" class="' + throbberClass + '" />');
                throbber.data("element_id", elementToHide.attr("id"));
                elementToHide.hide().after(throbber)
            }, this.throbberOptions.Delay))
        },
        hideThrobber: function (id) {
            var throbber = ($("#" + this.getthrobberId(id)));
            if (throbber !== null) {
                var hiddenElement = $("#" + throbber.data("element_id"));
                hiddenElement.show();
                window.clearTimeout(throbber.parent().data("throbber_timeout"));
                throbber.remove()
            }
        }
    }
})(jQuery);
(function ($) {
    $.workmate.popups = {
        open: function (url, status, toolbar, location, menubar, directories, resizable, scrollbars, width, height, windowname) {
            var c = "width=" + width;
            c += ",height=" + height;
            c += ",status=" + status;
            c += ",toolbar=" + toolbar;
            c += ",location=" + location;
            c += ",menubar=" + menubar;
            c += ",directories=" + directories;
            c += ",resizable=" + resizable;
            c += ",scrollbars=" + scrollbars;
            var a = null;
            try {
                a = window.open(url, windowname, c)
            }
            catch (err)
      { }
            if (a !== null) {
                a.focus()
            }
            return a
        }
    }
})(jQuery);
(function ($) {
    $.workmate.systemMessages = {
        messageType: {
            error: 0,
            warning: 1,
            information: 2,
            success: 3
        },
        displayType: {
            alert: 0,
            softAlert: 1,
            fade: 2
        },
        showMessage: function (messageType, message, displayType, subHeading, closeText) {
            switch (displayType) {
                case $.workmate.systemMessages.displayType.softAlert:
                    if (typeof $.fn.colorbox != "function") {
                        alert(message);
                        return
                    }
                    var messageClass = "lightboxinformation";
                    switch (messageType) {
                        case $.workmate.systemMessages.messageType.error:
                            messageClass = "lightboxerror";
                            break;
                        case $.workmate.systemMessages.messageType.warning:
                            messageClass = "lightboxwarning";
                            break;
                        case $.workmate.systemMessages.messageType.information:
                            messageClass = "lightboxinformation";
                            break;
                        case $.workmate.systemMessages.messageType.success:
                            messageClass = "lightboxsuccess";
                            break
                    }
                    $('#globalAlert').remove();
                    $('#globalAlertBox').remove();
                    $('body').append('<div id="globalAlert"></div>');
                    $('body').append('<div style="display:none">' + '<div id="globalAlertBox" style="padding:10px; background:#fff;" class="' + messageClass + '">' + (subHeading == null ? '' : '<h3 class="sub-heading">' + subHeading + '</h3>') + '<p>' + message + '</p>' + (closeText == null ? '' : '<div style="margin-top:15px;"><a href="#" onclick="$.fn.colorbox.close(); return false;" class="button-lge-green right"><div>' + 'Close' + '</div></a></div>') + '</div>' + '</div>');
                    $('#globalAlert').colorbox(
        {
            open: true,
            width: "300px",
            inline: true,
            href: "#globalAlertBox",
            overlayClose: true,
            title: false,
            close: "Continue"
        });
                    break;
                case $.workmate.systemMessages.displayType.fade:
                case $.workmate.systemMessages.displayType.alert:
                    alert(message);
                    break;
                default:
                    alert(message);
                    break
            }
        },
        showGenericMessage: function (messageType, errorCode, displayType) {
            var message = $.workmate.global.text.generalError;
            switch (errorCode) {
                case -1:
                    message = $.workmate.global.text.permissionDenied;
                    break;
                case -3:
                    message = $.workmate.global.text.invalidHtml;
                    break;
                case -4:
                    message = $.workmate.global.text.validationFailed;
                    break;
                case -6:
                    message = $.workmate.global.text.reachedLimit;
                    break;
                case -5:
                case -2:
                    message = $.workmate.global.text.generalError;
                    break;
                default:
                    message = $.workmate.global.text.generalError;
                    break
            }
            $.workmate.systemMessages.showMessage(messageType, message, displayType)
        }
    }
})(jQuery);