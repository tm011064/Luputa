(function ($) {
  $.workmate.content = {
    









        settings: {
            saveSuccessMessage: ''
        },
        init: function (saveSuccessMessage) {
            $.workmate.content.settings.saveSuccessMessage = saveSuccessMessage
        },
        showTextArea: function (textWrapperId, previewWrapperId) {
            $("#" + textWrapperId).show();
            $("#" + previewWrapperId).hide()
        },
        showPreview: function (textWrapperId, previewWrapperId, textAreaId) {
            $("#" + previewWrapperId).html($("#" + textAreaId).attr("value"));
            $("#" + textWrapperId).hide();
            $("#" + previewWrapperId).show()
        },
        save: function (textAreaId, buttonsWrapperId, resourceKey) {
            $.workmate.throbber.showThrobber(buttonsWrapperId);
            $.ajax(
      {
          type: "POST",
          url: $.workmate.global.services.contentController.url + $.workmate.global.services.contentController.methods.UpdateContentBlock,
          data: "{ placeHolderKey: '" + resourceKey + "', text: '" + $("#" + textAreaId).val().replace(/'/g, "\\'") + "' }",
          contentType: "application/json; charset=utf-8",
          dataType: "json",
          success: function (msg, id) {
              $.workmate.content.onSaveSuccess(msg, buttonsWrapperId)
          },
          error: function (xmlHttpRequest, id) {
              $.workmate.ajax.onAjaxErrorWithThrobber(xmlHttpRequest, senderId)
          }
      })
        },
        onSaveSuccess: function (result, senderId) {
            switch (result.d) {
                case 0:
                    $.workmate.systemMessages.showMessage($.workmate.systemMessages.success, $.workmate.content.settings.saveSuccessMessage, $.workmate.systemMessages.displayType.fade);
                    break;
                default:
                    $.workmate.systemMessages.showGenericMessage($.workmate.systemMessages.error, result.d, $.workmate.systemMessages.displayType.alert);
                    break
            }
            $.workmate.throbber.hideThrobber(senderId)
        }
    }
})(jQuery);