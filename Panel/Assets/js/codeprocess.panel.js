var uploadPictureId = null;
var uploadWriterPictureId = null;
var uploadIconId = null;
var uploadDocumentId = null;
var editorList = [];
var AttachFile2 = null;
var AttachFile1 = null;

$(document).ready(function () {

    initCodeprocessValues();
});

String.prototype.GetInteger = function () {
    try {
        var output = parseInt(this);
        if (isNaN(output) === true)
            return 0;
        return output;
    }
    catch (ex) { return 0; }
};

jQuery.fn.extend({
    selected: function () {
        var value = $(this).find(":selected").attr("value");
        return value === undefined ? null : value;
    }
});

function initCodeprocessValues() {

    var components = $("[codeprocess]");
    for (var i = 0; i < components.length; i++) {
        var cp = components[i];
        var value = $(cp).attr("codeprocess");
        var Id = $(cp).attr("id");
        if (value === "editor") {
            initCodeprocessEditor(Id);
        } else if (value === "date") {
            initCodeprocessDatetime(Id);
        } else if (value === "date-init") {
            initCodeprocessDatetime(Id);
            let today = new Date().toLocaleDateString('fa-IR');
            $(cp).val(today);
        } else if (value === "upload") {

            initCodeprocessUpload(Id);
        } else if (value === "upload-n") {
            initializeCodeprocessUploadScript(Id);
        } else if (value === "upload-n-1") {
            initializeCodeprocessUploadScript(Id);
        } else if (value === "upload-n-2") {
            initializeCodeprocessUploadScript(Id);
        } else if (value === "document") {
            initializeCodeprocessUploadDocumentScript(Id);
        }
        else if (value === "documentAttachFile1") {
            initializeCodeprocessUploadDocumentAttachFile1Script(Id);
        }
        else if (value === "documentAttachFile2") {
            initializeCodeprocessUploadDocumentAttachFile2Script(Id);
        }
        else if (value === "upload-cdnVideo") {
            initCodeprocessUploadCdnVideo(Id);
        }
        else if (value === "upload-cdnVideoDemo") {
            initCodeprocessUploadCdnVideoDemo(Id);
        }
        else if (value === "upload-cdnPicture") {
            initCodeprocessUploadCdnPicture(Id);
        }
        else if (value === "upload-cdnFileDemo") {
            initCodeprocessUploadcdnFileDemo(Id);
        }
        else if (value === "upload-cdnSWF") {
            initCodeprocessUploadCdnSWF(Id);
        }
        else if (value === "upload-cdnPdf") {
            initCodeprocessUploadCdnPdf(Id);
        }
        else if (value === "upload-cdnPdfDemo") {
            initCodeprocessUploadCdnPdfDemo(Id);
        } else if (value === "upload-cdnPicture") {
            initCodeprocessUploadCdnPicture(Id);
        }
        else if (value === "upload-cdnMusic") {
            initCodeprocessUploadCdnMusic(Id);
        }
        else if (value === "upload-cdnMusicDemo") {
            initCodeprocessUploadCdnMusicDemo(Id);
        }
        else if (value === "upload-cdnZip") {
            initCodeprocessUploadCdnZip(Id);
        }

        else if (value === "upload-cdnAudio") {
            initCodeprocessUploadCdnAudio(Id);
        } else if (value === "upload-cdnAudioDemo") {
            initCodeprocessUploadCdnAudioDemo(Id);
        }
        else if (value === "show-part") {
            initCodeprocessShowPart(Id);
        }
    }

    function initCodeprocessShowPart(Id) {
        var selectedId = $("#" + Id).find(":selected").attr("value");
        $("[show-part='" + selectedId + "']").show();
        $("#" + Id).change(function () {
            selectedId = $(this).find(":selected").attr("value");
            $("[show-part]").hide();
            $("[show-part='" + selectedId + "']").show();
        });
    }

    function initializeCodeprocessUploadDocumentScript(Id) {
        $("#" + Id).change(function () {

            var fileUpload = $("#" + Id).get(0);
            var files = fileUpload.files;

            var data = new FormData();
            for (var i = 0; i < files.length; i++) {
                data.append(files[i].name, files[i]);
            }
            var request = createRequest();
            request.url = base_admin_url + "/Upload/UploadDocument";
            request.type = "POST";
            request.data = data,
                request.contentType = false;
            request.processData = false;
            request.beforeSend = function () {
                $("#modalLoading").modal("show");
            }
            request.error = function () {
                createMessage(MESSAGE_TYPE_ERROR, "خطا", "خطا در هنگام آپلود تصویر");
            };
            request.success = function (entity) {
                uploadDocumentId = entity.Id;
                $("#modalLoading").modal("hide");
            };
            $.ajax(request);
        });
    }
    function initializeCodeprocessUploadDocumentAttachFile1Script(Id) {
        $("#" + Id).dropify({
            messages: {
                'default': 'فایل را اینجا بیندازید',
                'replace': 'برای تغییر تصویر کلیک کنید',
                'remove': 'حذف',
                'error': 'خطا در هنگام ارسال تصویر'
            },
            error: {
                'fileSize': 'The file size is too big (1M max).'
            }
        });
        $("#" + Id).change(function () {

            var fileUpload = $("#" + Id).get(0);
            var files = fileUpload.files;

            var data = new FormData();
            for (var i = 0; i < files.length; i++) {
                data.append(files[i].name, files[i]);
            }
            var request = createRequest();
            request.url = base_admin_url + "/Upload/UploadDocument";
            request.type = "POST";
            request.data = data,
                request.contentType = false;
            request.processData = false;
            request.beforeSend = function () {
                $("#modalLoading").modal("show");
            }
            request.error = function () {
                createMessage(MESSAGE_TYPE_ERROR, "خطا", "خطا در هنگام آپلود تصویر");
            };
            request.success = function (entity) {
                AttachFile1 = entity.Id;
                $("#modalLoading").modal("hide");
            };
            $.ajax(request);
        });
    }
    function initializeCodeprocessUploadDocumentAttachFile2Script(Id) {
        $("#" + Id).dropify({
            messages: {
                'default': 'فایل را اینجا بیندازید',
                'replace': 'برای تغییر تصویر کلیک کنید',
                'remove': 'حذف',
                'error': 'خطا در هنگام ارسال تصویر'
            },
            error: {
                'fileSize': 'The file size is too big (1M max).'
            }
        });
        $("#" + Id).change(function () {

            var fileUpload = $("#" + Id).get(0);
            var files = fileUpload.files;

            var data = new FormData();
            for (var i = 0; i < files.length; i++) {
                data.append(files[i].name, files[i]);
            }
            var request = createRequest();
            request.url = base_admin_url + "/Upload/UploadDocument";
            request.type = "POST";
            request.data = data,
                request.contentType = false;
            request.processData = false;
            request.beforeSend = function () {
                $("#modalLoading").modal("show");
            }
            request.error = function () {
                createMessage(MESSAGE_TYPE_ERROR, "خطا", "خطا در هنگام آپلود تصویر");
            };
            request.success = function (entity) {
                AttachFile2 = entity.Id;
                $("#modalLoading").modal("hide");
            };
            $.ajax(request);
        });
    }

    function initializeCodeprocessUploadScript(Id) {
        $("#" + Id).dropify({
            messages: {
                'default': 'فایل را اینجا بیندازید',
                'replace': 'برای تغییر تصویر کلیک کنید',
                'remove': 'حذف',
                'error': 'خطا در هنگام ارسال تصویر'
            },
            error: {
                'fileSize': 'The file size is too big (1M max).'
            }
        });
    }
    function initCodeprocessUploadCdnVideo(Id) {

        $("#" + Id).dropify({
            messages: {
                'default': 'فایل را اینجا بیندازید',
                'replace': 'برای تغییر تصویر کلیک کنید',
                'remove': 'حذف',
                'error': 'خطا در هنگام ارسال تصویر'
            },
            error: {
                'fileSize': 'The file size is too big (1M max).'
            }
        });

        $("#" + Id).change(function () {
            if (document.getElementById(Id).files.length < 1)
                return;
            $("#modalLoading").modal("show");
            var formData = new FormData();
            for (var i = 0; i < document.getElementById(Id).files.length; i++)
                formData.append("files", document.getElementById(Id).files[i]);
            $.ajax({
                type: "POST",
                url: "/cdn/file/upload?isPrivate=true",
                data: formData,
                contentType: false,
                processData: false,
                success: function (response) {
                    if (response.status == "success") {
                        $("#VideoFileId").val(response.FileId);
                        $("#modalLoading").modal("hide");
                    } else {
                        createMessage(MESSAGE_TYPE_ERROR, "خطا", response.message);
                        $("#modalLoading").modal("hide");
                    }

                },
                error: function (xhr, ajaxOptions, thrownError) {
                    createMessage(MESSAGE_TYPE_ERROR, "خطا", "خطا در هنگام آپلود تصویر");
                    $("#modalLoading").modal("hide");
                }
            });

        });
    }

    function initCodeprocessUploadCdnVideoDemo(Id) {

        $("#" + Id).dropify({
            messages: {
                'default': 'فایل را اینجا بیندازید',
                'replace': 'برای تغییر تصویر کلیک کنید',
                'remove': 'حذف',
                'error': 'خطا در هنگام ارسال تصویر'
            },
            error: {
                'fileSize': 'The file size is too big (1M max).'
            }
        });

        $("#" + Id).change(function () {
            if (document.getElementById(Id).files.length < 1)
                return;
            $("#modalLoading").modal("show");
            var formData = new FormData();
            for (var i = 0; i < document.getElementById(Id).files.length; i++)
                formData.append("files", document.getElementById(Id).files[i]);
            $.ajax({
                type: "POST",
                url: "/cdn/file/upload",
                data: formData,
                contentType: false,
                processData: false,
                success: function (response) {
                    if (response.status == "success") {

                        $("#VideoDemoFileId").val(response.FileId);

                        $("#modalLoading").modal("hide");
                    } else {
                        createMessage(MESSAGE_TYPE_ERROR, "خطا", response.message);
                        $("#modalLoading").modal("hide");
                    }

                },
                error: function (xhr, ajaxOptions, thrownError) {
                    createMessage(MESSAGE_TYPE_ERROR, "خطا", "خطا در هنگام آپلود تصویر");
                    $("#modalLoading").modal("hide");
                }
            });

        });
    }
    function initCodeprocessUploadCdnPicture(Id) {

        $("#" + Id).dropify({
            messages: {
                'default': 'فایل را اینجا بیندازید',
                'replace': 'برای تغییر تصویر کلیک کنید',
                'remove': 'حذف',
                'error': 'خطا در هنگام ارسال تصویر'
            },
            error: {
                'fileSize': 'The file size is too big (1M max).'
            }
        });

        $("#" + Id).change(function () {
            if (document.getElementById(Id).files.length < 1)
                return;
            $("#modalLoading").modal("show");
            var formData = new FormData();
            for (var i = 0; i < document.getElementById(Id).files.length; i++)
                formData.append("files", document.getElementById(Id).files[i]);
            $.ajax({
                type: "POST",
                url: "/cdn/file/upload",
                data: formData,
                contentType: false,
                processData: false,
                success: function (response) {
                    if (response.status == "success") {
                        $("#FileId").val(response.FileId);

                        $("#modalLoading").modal("hide");
                    } else {
                        createMessage(MESSAGE_TYPE_ERROR, "خطا", response.message);
                        $("#modalLoading").modal("hide");
                    }

                },
                error: function (xhr, ajaxOptions, thrownError) {
                    createMessage(MESSAGE_TYPE_ERROR, "خطا", "خطا در هنگام آپلود تصویر");
                    $("#modalLoading").modal("hide");
                }
            });

        });
    }

    function initCodeprocessUploadcdnFileDemo(Id) {

        $("#" + Id).dropify({
            messages: {
                'default': 'فایل را اینجا بیندازید',
                'replace': 'برای تغییر تصویر کلیک کنید',
                'remove': 'حذف',
                'error': 'خطا در هنگام ارسال تصویر'
            },
            error: {
                'fileSize': 'The file size is too big (1M max).'
            }
        });

        $("#" + Id).change(function () {
            if (document.getElementById(Id).files.length < 1)
                return;
            $("#modalLoading").modal("show");
            var formData = new FormData();
            for (var i = 0; i < document.getElementById(Id).files.length; i++)
                formData.append("files", document.getElementById(Id).files[i]);
            $.ajax({
                type: "POST",
                url: "/cdn/file/upload",
                data: formData,
                contentType: false,
                processData: false,
                success: function (response) {
                    if (response.status == "success") {

                        $("#FileId").val(response.FileId);

                        $("#modalLoading").modal("hide");
                    } else {
                        createMessage(MESSAGE_TYPE_ERROR, "خطا", response.message);
                        $("#modalLoading").modal("hide");
                    }

                },
                error: function (xhr, ajaxOptions, thrownError) {
                    createMessage(MESSAGE_TYPE_ERROR, "خطا", "خطا در هنگام آپلود تصویر");
                    $("#modalLoading").modal("hide");
                }
            });

        });
    }
    function initCodeprocessUploadCdnSWF(Id) {

        $("#" + Id).dropify({
            messages: {
                'default': 'فایل را اینجا بیندازید',
                'replace': 'برای تغییر تصویر کلیک کنید',
                'remove': 'حذف',
                'error': 'خطا در هنگام ارسال تصویر'
            },
            error: {
                'fileSize': 'The file size is too big (1M max).'
            }
        });

        $("#" + Id).change(function () {
            if (document.getElementById(Id).files.length < 1)
                return;
            $("#modalLoading").modal("show");
            var formData = new FormData();
            for (var i = 0; i < document.getElementById(Id).files.length; i++)
                formData.append("files", document.getElementById(Id).files[i]);
            $.ajax({
                type: "POST",
                url: "/cdn/file/upload",
                data: formData,
                contentType: false,
                processData: false,
                success: function (response) {
                    if (response.status == "success") {
                        $("#SwfFileId").val(response.FileId);
                        $("#modalLoading").modal("hide");
                    } else {
                        createMessage(MESSAGE_TYPE_ERROR, "خطا", response.message);
                        $("#modalLoading").modal("hide");
                    }

                },
                error: function (xhr, ajaxOptions, thrownError) {
                    createMessage(MESSAGE_TYPE_ERROR, "خطا", "خطا در هنگام آپلود تصویر");
                    $("#modalLoading").modal("hide");
                }
            });

        });
    }
    function initCodeprocessUploadCdnPdf(Id) {

        $("#" + Id).dropify({
            messages: {
                'default': 'فایل را اینجا بیندازید',
                'replace': 'برای تغییر تصویر کلیک کنید',
                'remove': 'حذف',
                'error': 'خطا در هنگام ارسال تصویر'
            },
            error: {
                'fileSize': 'The file size is too big (1M max).'
            }
        });

        $("#" + Id).change(function () {
            if (document.getElementById(Id).files.length < 1)
                return;
            $("#modalLoading").modal("show");
            var formData = new FormData();
            for (var i = 0; i < document.getElementById(Id).files.length; i++)
                formData.append("files", document.getElementById(Id).files[i]);
            $.ajax({
                type: "POST",
                url: "/cdn/file/upload",
                data: formData,
                contentType: false,
                processData: false,
                success: function (response) {
                    if (response.status == "success") {
                        $("#PdfFileId").val(response.FileId);
                        $("#modalLoading").modal("hide");
                    } else {
                        createMessage(MESSAGE_TYPE_ERROR, "خطا", response.message);
                        $("#modalLoading").modal("hide");
                    }

                },
                error: function (xhr, ajaxOptions, thrownError) {
                    createMessage(MESSAGE_TYPE_ERROR, "خطا", "خطا در هنگام آپلود تصویر");
                    $("#modalLoading").modal("hide");
                }
            });

        });
    }
    function initCodeprocessUploadCdnPdfDemo(Id) {

        $("#" + Id).dropify({
            messages: {
                'default': 'فایل را اینجا بیندازید',
                'replace': 'برای تغییر تصویر کلیک کنید',
                'remove': 'حذف',
                'error': 'خطا در هنگام ارسال تصویر'
            },
            error: {
                'fileSize': 'The file size is too big (1M max).'
            }
        });

        $("#" + Id).change(function () {
            if (document.getElementById(Id).files.length < 1)
                return;
            $("#modalLoading").modal("show");
            var formData = new FormData();
            for (var i = 0; i < document.getElementById(Id).files.length; i++)
                formData.append("files", document.getElementById(Id).files[i]);
            $.ajax({
                type: "POST",
                url: "/cdn/file/upload",
                data: formData,
                contentType: false,
                processData: false,
                success: function (response) {
                    if (response.status == "success") {
                        $("#PdfDemoFileId").val(response.FileId);
                        $("#modalLoading").modal("hide");
                    } else {
                        createMessage(MESSAGE_TYPE_ERROR, "خطا", response.message);
                        $("#modalLoading").modal("hide");
                    }

                },
                error: function (xhr, ajaxOptions, thrownError) {
                    createMessage(MESSAGE_TYPE_ERROR, "خطا", "خطا در هنگام آپلود تصویر");
                    $("#modalLoading").modal("hide");
                }
            });

        });
    }
    function initCodeprocessUploadCdnMusic(Id) {

        $("#" + Id).dropify({
            messages: {
                'default': 'فایل را اینجا بیندازید',
                'replace': 'برای تغییر تصویر کلیک کنید',
                'remove': 'حذف',
                'error': 'خطا در هنگام ارسال تصویر'
            },
            error: {
                'fileSize': 'The file size is too big (1M max).'
            }
        });

        $("#" + Id).change(function () {
            if (document.getElementById(Id).files.length < 1)
                return;
            $("#modalLoading").modal("show");
            var formData = new FormData();
            for (var i = 0; i < document.getElementById(Id).files.length; i++)
                formData.append("files", document.getElementById(Id).files[i]);
            $.ajax({
                type: "POST",
                url: "/cdn/file/upload",
                data: formData,
                contentType: false,
                processData: false,
                success: function (response) {
                    if (response.status == "success") {
                        $("#MusicFileId").val(response.FileId);
                        $("#modalLoading").modal("hide");
                    } else {
                        createMessage(MESSAGE_TYPE_ERROR, "خطا", response.message);
                        $("#modalLoading").modal("hide");
                    }

                },
                error: function (xhr, ajaxOptions, thrownError) {
                    createMessage(MESSAGE_TYPE_ERROR, "خطا", "خطا در هنگام آپلود تصویر");
                    $("#modalLoading").modal("hide");
                }
            });

        });
    }
    function initCodeprocessUploadCdnMusicDemo(Id) {

        $("#" + Id).dropify({
            messages: {
                'default': 'فایل را اینجا بیندازید',
                'replace': 'برای تغییر تصویر کلیک کنید',
                'remove': 'حذف',
                'error': 'خطا در هنگام ارسال تصویر'
            },
            error: {
                'fileSize': 'The file size is too big (1M max).'
            }
        });

        $("#" + Id).change(function () {
            if (document.getElementById(Id).files.length < 1)
                return;
            $("#modalLoading").modal("show");
            var formData = new FormData();
            for (var i = 0; i < document.getElementById(Id).files.length; i++)
                formData.append("files", document.getElementById(Id).files[i]);
            $.ajax({
                type: "POST",
                url: "/cdn/file/upload",
                data: formData,
                contentType: false,
                processData: false,
                success: function (response) {
                    if (response.status == "success") {
                        $("#MusicDemoFileId").val(response.FileId);
                        $("#modalLoading").modal("hide");
                    } else {
                        createMessage(MESSAGE_TYPE_ERROR, "خطا", response.message);
                        $("#modalLoading").modal("hide");
                    }

                },
                error: function (xhr, ajaxOptions, thrownError) {
                    createMessage(MESSAGE_TYPE_ERROR, "خطا", "خطا در هنگام آپلود تصویر");
                    $("#modalLoading").modal("hide");
                }
            });

        });
    }
    function initCodeprocessUploadCdnZip(Id) {

        $("#" + Id).dropify({
            messages: {
                'default': 'فایل را اینجا بیندازید',
                'replace': 'برای تغییر تصویر کلیک کنید',
                'remove': 'حذف',
                'error': 'خطا در هنگام ارسال تصویر'
            },
            error: {
                'fileSize': 'The file size is too big (1M max).'
            }
        });

        $("#" + Id).change(function () {
            if (document.getElementById(Id).files.length < 1)
                return;
            $("#modalLoading").modal("show");
            var formData = new FormData();
            for (var i = 0; i < document.getElementById(Id).files.length; i++)
                formData.append("files", document.getElementById(Id).files[i]);
            $.ajax({
                type: "POST",
                url: "/cdn/file/upload",
                data: formData,
                contentType: false,
                processData: false,
                success: function (response) {
                    if (response.status == "success") {
                        $("#ZipFileId").val(response.FileId);
                        $("#modalLoading").modal("hide");
                    } else {
                        createMessage(MESSAGE_TYPE_ERROR, "خطا", response.message);
                        $("#modalLoading").modal("hide");
                    }

                },
                error: function (xhr, ajaxOptions, thrownError) {
                    createMessage(MESSAGE_TYPE_ERROR, "خطا", "خطا در هنگام آپلود تصویر");
                    $("#modalLoading").modal("hide");
                }
            });

        });
    }
    function initCodeprocessUploadCdnAudio(Id) {

        $("#" + Id).dropify({
            messages: {
                'default': 'فایل را اینجا بیندازید',
                'replace': 'برای تغییر تصویر کلیک کنید',
                'remove': 'حذف',
                'error': 'خطا در هنگام ارسال تصویر'
            },
            error: {
                'fileSize': 'The file size is too big (1M max).'
            }
        });

        $("#" + Id).change(function () {
            if (document.getElementById(Id).files.length < 1)
                return;
            $("#modalLoading").modal("show");
            var formData = new FormData();
            for (var i = 0; i < document.getElementById(Id).files.length; i++)
                formData.append("files", document.getElementById(Id).files[i]);
            $.ajax({
                type: "POST",
                url: "/cdn/file/upload",
                data: formData,
                contentType: false,
                processData: false,
                success: function (response) {
                    if (response.status == "success") {
                        $("#AudioFileId").val(response.FileId);
                        $("#modalLoading").modal("hide");
                    } else {
                        createMessage(MESSAGE_TYPE_ERROR, "خطا", response.message);
                        $("#modalLoading").modal("hide");
                    }

                },
                error: function (xhr, ajaxOptions, thrownError) {
                    createMessage(MESSAGE_TYPE_ERROR, "خطا", "خطا در هنگام آپلود تصویر");
                    $("#modalLoading").modal("hide");
                }
            });

        });
    }
    function initCodeprocessUploadCdnAudioDemo(Id) {

        $("#" + Id).dropify({
            messages: {
                'default': 'فایل را اینجا بیندازید',
                'replace': 'برای تغییر تصویر کلیک کنید',
                'remove': 'حذف',
                'error': 'خطا در هنگام ارسال تصویر'
            },
            error: {
                'fileSize': 'The file size is too big (1M max).'
            }
        });

        $("#" + Id).change(function () {
            if (document.getElementById(Id).files.length < 1)
                return;
            $("#modalLoading").modal("show");
            var formData = new FormData();
            for (var i = 0; i < document.getElementById(Id).files.length; i++)
                formData.append("files", document.getElementById(Id).files[i]);
            $.ajax({
                type: "POST",
                url: "/cdn/file/upload",
                data: formData,
                contentType: false,
                processData: false,
                success: function (response) {
                    if (response.status == "success") {
                        $("#AudioDemoFileId").val(response.FileId);
                        $("#modalLoading").modal("hide");
                    } else {
                        createMessage(MESSAGE_TYPE_ERROR, "خطا", response.message);
                        $("#modalLoading").modal("hide");
                    }

                },
                error: function (xhr, ajaxOptions, thrownError) {
                    createMessage(MESSAGE_TYPE_ERROR, "خطا", "خطا در هنگام آپلود تصویر");
                    $("#modalLoading").modal("hide");
                }
            });

        });
    }
    function initCodeprocessUpload(Id) {
        $("#" + Id).dropify({
            messages: {
                'default': 'فایل را اینجا بیندازید',
                'replace': 'برای تغییر تصویر کلیک کنید',
                'remove': 'حذف',
                'error': 'خطا در هنگام ارسال تصویر'
            },
            error: {
                'fileSize': 'The file size is too big (1M max).'
            }
        });

        $("#" + Id).change(function () {
            var fileUpload = $("#" + Id).get(0);
            var files = fileUpload.files;
            var uploadType = $(fileUpload).attr("uploadtype");
            var data = new FormData();
            for (var i = 0; i < files.length; i++) {
                data.append(files[i].name, files[i]);
            }
            var request = createRequest();
            request.url = base_admin_url + "/Upload/UploadPhoto";
            request.type = "POST";
            request.data = data,
                request.contentType = false;
            request.processData = false;
            request.beforeSend = function () {
                $("#modalLoading").modal("show");
            }
            request.error = function () {
                createMessage(MESSAGE_TYPE_ERROR, "خطا", "خطا در هنگام آپلود تصویر");
            };
            request.success = function (entity) {
                if (uploadType == 2) {
                    uploadIconId = entity.Id;
                }
                else if (uploadType == 3) {
                    uploadWriterPictureId = entity.Id;
                }
                else {
                    uploadPictureId = entity.Id;

                }
                $("#modalLoading").modal("hide");
            };
            $.ajax(request);
        });
    }

    function initCodeprocessDatetime(Id) {
        var objCal = new AMIB.persianCalendar(Id);

    }
}

function initCodeprocessEditor(Id) {

    tinymce.init({
        selector: "input#" + Id,
        height: 250,
        theme: 'modern',
        plugins: [
            'advlist autolink lists link image charmap print preview hr anchor pagebreak',
            'searchreplace wordcount visualblocks visualchars code fullscreen',
            'insertdatetime media nonbreaking save table contextmenu directionality',
            'emoticons template paste textcolor colorpicker textpattern imagetools image'
        ],
        toolbar1: 'insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link',
        toolbar2: 'print preview media | forecolor backcolor emoticons | image code',
        images_upload_url: 'upload.php',
        images_upload_handler: function (blobInfo, success, failure) {
            var data = new FormData();
            data.append('file', blobInfo.blob(), blobInfo.filename());
            var request = createRequest();
            request.url = base_admin_url + "/Upload/UploadPhoto";
            request.type = "POST";
            request.data = data,
                request.contentType = false;
            request.processData = false;
            request.error = function () {
                createMessage(MESSAGE_TYPE_ERROR, "خطا", "خطا در هنگام آپلود تصویر");
            };
            request.success = function (entity) {
                let imageUrl = entity.Url.replace(window.location.origin, "");
                success(imageUrl);
            };
            $.ajax(request);
        }
    });
}

function clearDropDown(Id, isDefault) {
    $("#" + Id).find("option").remove();
    if (isDefault === true) {
        $("#" + Id).append("<option value='0'>انتخاب</option>");
    }
}

function bindDropDown(Id, entity, name, value, isDefault, selectedItem) {
    $("#" + Id).find("option").remove();
    var result = entity;
    if (isDefault === true) {
        $("#" + Id).append("<option value='0'>انتخاب</option>");
    }
    for (var i = 0; i < result.length; i++) {
        var item = result[i];
        var itemValue = item[value];
        if (selectedItem != undefined && itemValue.toString() === selectedItem.toString()) {
            $("#" + Id).append("<option value='" + itemValue + "' selected='selected'>" + item[name] + "</option>");
        }
        else {
            $("#" + Id).append("<option value='" + itemValue + "'>" + item[name] + "</option>");
        }
    }
}

function getCustomFieldControl(item) {
    var itemValue = item.ProductFieldValue == null ? "" : item.ProductFieldValue;
    var itemId = item.ProductFieldItem == null ? 0 : item.ProductFieldItem;

    var html = "";
    html += "<div class='col-md-6'>";
    html += "<div class='form-group'>";
    html += "<label class='control-label col-md-4'>" + item.Name + "</label>";
    html += "<div class='col-md-8'>";

    if (item.Type.Label === "FIELD_TYPE_STRING") {
        html += "<input type='text' class='form-control' custom-id='" + item.Id + "' value='" + itemValue + "' custom-type='STRING' autocomplete='off' />";
    } else if (item.Type.Label === "FIELD_TYPE_INTEGER") {
        html += "<input type='text' class='form-control' custom-id='" + item.Id + "' value='" + itemValue + "' custom-type='INTEGER' autocomplete='off' />";
    } else if (item.Type.Label === "FIELD_TYPE_DATE") {
        html += "";
    } else if (item.Type.Label === "FIELD_TYPE_TEXT") {
        html += "<input type='text' class='form-control' custom-id='" + item.Id + "' value='" + itemValue + "' custom-type='STRING' autocomplete='off' />";
    } else if (item.Type.Label === "FIELD_TYPE_HTML") {
        html += "<input id='fieldId" + item.Id + "' type='text' class='form-control' custom-id='" + item.Id + "' value='" + itemValue + "' custom-type='HTML' autocomplete='off' />";
    } else if (item.Type.Label === "FIELD_TYPE_FILE") {
        html += "";
    } else if (item.Type.Label === "FIELD_TYPE_DROPDOWN") {
        html += "<select class='form-control' custom-type='DROPDOWN' custom-id='" + item.Id + "'>";
        html += "<option value='0'>انتخاب</option>";
        for (var i = 0; i < item.Items.length; i++) {
            var selectItem = item.Items[i];
            if (selectItem.Id == itemId) {
                html += "<option selected='selected' value='" + selectItem.Id + "'>" + selectItem.Value + "</option>";
            } else {
                html += "<option value='" + selectItem.Id + "'>" + selectItem.Value + "</option>";
            }
        }
        html += "<select>";
    } else if (item.Type.Label === "FIELD_TYPE_BOOLEAN") {
        var isSelected = itemValue == "True" ? " checked='checked' " : "";
        html += "<input type='checkbox' class='margin-checkbox' custom-id='" + item.Id + "' value='" + itemValue + "' " + isSelected + " custom-type='BOOLEAN' />";
    }
    html += "</div>";
    html += "</div>";
    html += "</div>";
    return html;
}

function getCheckedValue(obj) {
    return $(obj).is(':checked');
}

function getSelectedValue(obj) {
    var value = $(obj).find(":selected").attr("value");
    value = value === undefined ? parseInt($(obj).val()) : value;
    value = isNaN(value) ? 0 : value;
    return value;
}

function getCodeprocessBackUrl() {
    return $("[codeprocess-back-form]").attr("codeprocess-back-form");
}

function initializePermissionScript() {
    $("#btnSubmit").click(function () {
        var entity = {};
        var selectedItems = $("[aria-selected='true']");
        var selectedParents = $(".jstree-undetermined").closest("[role='treeitem']");

        entity.UserGroupId = $("[name='UserGroupId']").val();
        entity.PermissionList = [];

        for (var i = 0; i < selectedItems.length; i++) {
            var valueItem = parseInt($(selectedItems[i]).attr("value"));
            entity.PermissionList.push(valueItem);
        }

        for (var j = 0; j < selectedParents.length; j++) {
            var valueParent = parseInt($(selectedParents[j]).attr("value"));
            entity.PermissionList.push(valueParent);
        }

        var request = createRequest(entity);
        request.success = function (result) {
            if (result.Type === MESSAGE_ERROR) {
                createMessage(MESSAGE_TYPE_ERROR, result.Body);
            } else if (result.Type === MESSAGE_SUCCESS) {
                createMessage(MESSAGE_TYPE_SUCCESS, result.Body);
                var url = getCodeprocessBackUrl();
                if (url !== undefined) {
                    if (url.toUpperCase().startsWith("/PANEL") == false) {
                        url = base_admin_url + url;
                    }
                    document.location = url;
                }
            }
        }
        $.ajax(request);

    });
}

function initializeNewCategoryScript() {

    var category = {};
    $("#btnSubmit").click(function () {
        fillEntity();
        $.ajax(createRequest(category));
    });
    function fillEntity() {
        category.ID = $("#ID").val();
        category.WebsiteId = $("#WebsiteId").val();
        category.TypeId = $("#TypeId").val();
        category.Name = $("#Name").val();
        category.Label = $("#Label").val();
        category.PictureId = uploadPictureId == null ? $("#pictureUploadId").val() : uploadPictureId;
        category.WriterPictureId = uploadWriterPictureId == null ? $("#pictureWriterUploadId").val() : uploadWriterPictureId;
        category.URL = $("#URL").val();
        category.Body = tinymce.get('Body') != null ? tinymce.get('Body').getContent() : "";
        category.CategoryLanguage = [];
        var langValues = $("[lang-value]");
        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    LanguageId: parseInt($(langObject).find("[lang-name='LanguageId']").val()),
                    Name: $(langObject).find("[lang-name='Name']").val(),
                    Body: tinymce.get($(langObject).find("[lang-name='Body']").attr('id')).getContent(),
                    URL: $(langObject).find("[lang-name='URL']").val()
                };
                category.CategoryLanguage.push(langEntity);
            }
        }
    }
}

function initializeNewSliderScript() {
    var slider = {};
    $("#btnSubmit").click(function () {
        fillEntity();
        $.ajax(createRequest(slider));
    });
    function fillEntity() {
        slider.ID = $("#ID").val();
        slider.WebsiteId = $("#WebsiteId").val();
        slider.UserID = $("#UserID").val();
        slider.Name = $("#Name").val();
        slider.SecondName = $("#SecondName").val();
        slider.FileId = $("#FileId").val();

        slider.Link = $("#Link").val();
        slider.ShowNumber = $("#ShowNumber").val();
        slider.PictureId = uploadPictureId == null ? $("#pictureUploadId").val() : uploadPictureId;
        slider.WriterPictureId = uploadWriterPictureId == null ? $("#pictureWriterUploadId").val() : uploadWriterPictureId;
        slider.SliderLanguage = [];
        var langValues = $("[lang-value]");
        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    LanguageId: parseInt($(langObject).find("[lang-name='LanguageId']").val()),
                    Name: $(langObject).find("[lang-name='Name']").val(),
                    SecondName: $(langObject).find("[lang-name='SecondName']").val()
                };
                slider.SliderLanguage.push(langEntity);
            }
        }
    }
}

function initializeNewPostScript() {
    var post = {};
    fillCity();
    $("#StateId").change(function () {
        fillCity();

    });
    function fillCity() {

        var selected = getSelectedValue($("#StateId"));
        if (selected !== undefined) {
            var request = createRequest();
            request.type = REQUEST_TYPE_GET;
            request.url = base_admin_url + "/content/post/FillCity?stateId=" + selected;
            request.success = function (entity) {
                bindDropDown("CityId", entity, "Name", "Id", true, $("#LastCityId").val());

            }
            $.ajax(request);
        }
    }
    $("#btnSubmit").click(function () {

        fillEntity();

        $.ajax(createRequest(post));
    });

    $(".dropify-clear").click(function () {
        uploadPictureId = 0;
    });
    function fillEntity() {
        post.ID = $("#ID").val();
        post.StateId = $("#StateId").val();
        post.CityId = $("#CityId").val();
        post.ProductId = $("#ProductId").val();
        post.Schema = $("#Schema").val();

        post.WebsiteId = $("#WebsiteId").val();
        post.CategoryId = $("#CategoryId").val();
        post.Name = $("#Name").val();
        post.Keywords = $("#Keywords").val();
        post.Summary = $("#Summary").val();
        post.Phone = $("#Phone").val();

        post.Body = tinymce.get('Body') != null ? tinymce.get('Body').getContent() : "";
        post.UpdateDatetime = $("#UpdateDatetime").val();
        post.CreateDateTime = $("#CreateDateTime").val();
        post.ShowNumber = $("#ShowNumber").val();
        post.NoIndex = $('#NoIndex').prop('checked');
        post.NoFollow = $('#NoFollow').prop('checked');
        post.Canonical = $("#Canonical").val();
        //post.IsSlider = $("#IsSlider").val();
        //post.Active = $("#Active").val();
        if ($('#IsSlider').is(":checked")) {
            post.IsSlider = true;
        }
        else {
            post.IsSlider = false;
        }
        if ($('#Active').is(":checked")) {
            post.Active = true;
        }
        else {
            post.Active = false;
        }

        post.VisitCount = $("#VisitCount").val();
        post.CreateDateTime = $("#CreateDateTime").val();
        post.Writer = $("#Writer").val();
        post.WriterPostion = $("#WriterPostion").val();
        post.H1 = $("#H1").val();
        post.URl = $("#URL").val();
        post.PictureId = $("#pictureUpload").val();
        post.WriterPictureId = $("#pictureWriterUploadId").val();
        post.FileId = $("#FileUpload").val();
        post.PictureId = uploadPictureId == null ? $("#pictureUploadId").val() : uploadPictureId;
        post.WriterPictureId = uploadWriterPictureId == null ? $("#pictureWriterUploadId").val() : uploadWriterPictureId;
        post.Aparat = $("#Aparat").val();
        post.FileId = uploadDocumentId == null ? $("#FileUploadId").val() : uploadDocumentId;
        post.PostLanguage = [];
        post.TagPost = [];
        var tagInputs = $(".tag-container input");
        for (var j = 0; j < tagInputs.length; j++) {
            var thisInput = tagInputs[j];
            if ($(thisInput).prop("checked") === true) {
                var tagId = $(thisInput).attr("tag-id");
                var tagEntity = {
                    PostId: post.ID,
                    TagId: parseInt(tagId)
                };
                post.TagPost.push(tagEntity);
            }
        }
        var langValues = $("[lang-value]");
        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    LanguageId: parseInt($(langObject).find("[lang-name='LanguageId']").val()),
                    Name: $(langObject).find("[lang-name='Name']").val(),
                    Summary: $(langObject).find("[lang-name='Summary']").val(),
                    Body: tinymce.get($(langObject).find("[lang-name='Body']").attr('id')).getContent(),
                    Url: $(langObject).find("[lang-name='Url']").val(),
                    Keywords: $(langObject).find("[lang-name='Keywords']").val(),
                    H1: $(langObject).find("[lang-name='H1']").val()
                };
                post.PostLanguage.push(langEntity);
            }
        }
    }

}
function initializeNewEmailScript() {
    var email = {};
    $("#btnSubmit").click(function () {

        fillEntity();

        $.ajax(createRequest(email));
    });
    function fillEntity() {

        email.UserEmails = $("#UserEmails").val();
        email.UserEmails = email.UserEmails;
        email.Body = tinymce.get('Body') != null ? tinymce.get('Body').getContent() : "";

    }

}
function initializeNewProductScript() {
    $(".videodemo .dropify-clear").click(function () {
        $("#VideoDemoFileId").val("");
    });
    $(".video .dropify-clear").click(function () {
        $("#VideoFileId").val("");
    });
    $(".audiodemo .dropify-clear").click(function () {
        $("#AudioDemoFileId").val("");
    });
    $(".audio .dropify-clear").click(function () {
        $("#AudioFileId").val("");
    });
    $(".musicdemo .dropify-clear").click(function () {
        $("#MusicDemoFileId").val("");
    });
    $(".music .dropify-clear").click(function () {
        $("#MusicFileId").val("");
    });
    $(".pdfdemo .dropify-clear").click(function () {
        $("#PdfDemoFileId").val("");
    });
    $(".pdf .dropify-clear").click(function () {
        $("#PdfFileId").val("");
    });
    $(".zip .dropify-clear").click(function () {
        $("#ZipFileId").val("");
    });
    var product = {};
    fillProductType();
    fillCustomFields();
    removeRelatedPicture();

    $(".dropify-clear").click(function () {

        uploadPictureId = 0;
        uploadWriterPictureId = null;
    });

    $("#btnSubmit").click(function () {
        fillEntity();
        $.ajax(createRequest(product));
    });
    if ($('input[name=IsService]:checked').val() == "false") {

        $(".map-div").hide();
    }
    $("input[name=IsService]").change(function () {
        var elm = $(this);
        if (elm.val() == "true") {
            $(".map-div").show();
        }
        else {
            $(".map-div").hide();

        }
    });

    function getCustomValues(product) {
        product.ProductCustomValue = [];

        var customElements = $("[custom-id]");

        for (var i = 0; i < customElements.length; i++) {
            var element = customElements[i];
            var entity = {
                ID: 0,
                FieldId: $(element).attr("custom-id").GetInteger(),
                Value: getInputValue(element),
                ItemId: getSelectValue(element)
            };
            product.ProductCustomValue.push(entity);
        }
    }

    function getSelectValue(element) {
        var Value;
        var elementType = $(element).attr("custom-type");
        if (elementType === "DROPDOWN")
            Value = $(element).find(":selected").attr("value");
        else
            Value = null;
        return Value;
    }

    function getInputValue(element) {
        var Value;
        var elementType = $(element).attr("custom-type");
        if (elementType === "STRING")
            Value = $(element).val();
        else if (elementType === "BOOLEAN")
            Value = $(element).prop("checked") == true ? "True" : "False";
        else if (elementType === "INTEGER")
            Value = $(element).val();
        else if (elementType === "DROPDOWN")
            Value = "";
        else
            Value = "";
        return Value;
    }

    $("#ShopId").change(function () {
        fillProductType();
    });

    $("#ProductTypeId").change(function () {

        fillCategory();
        fillBrand();
        fillCustomFields();
        fillColors();
        fillSize();
        fillProductOption();

    });

    $("#ProductCategoryId").change(function () {
        fillSubCategory();
        fillCustomFields();
    });

    $("#ProductSubCategoryId").change(function () {
        fillCustomFields();
    });

    $("#BrandId").change(function () {
        fillCustomFields();
    });

    $("#uplOtherPicture").change(function () {

        var fileUpload = $("#uplOtherPicture").get(0);
        var files = fileUpload.files;

        var data = new FormData();
        for (var i = 0; i < files.length; i++) {
            data.append(files[i].name, files[i]);
        }
        var request = createRequest();
        request.url = base_admin_url + "/Upload/UploadPhoto";
        request.type = "POST";
        request.data = data,
            request.contentType = false;
        request.processData = false;
        request.beforeSend = function () {
            $("#uplOtherPicture").val("");
            $("#modalLoading").modal("show");
        }
        request.error = function () {
            createMessage(MESSAGE_TYPE_ERROR, "خطا", "خطا در هنگام آپلود تصویر");
        };
        request.success = function (entity) {
            addRelatedPicture(entity);
            $("#modalLoading").modal("hide");
        };
        $.ajax(request);
    });

    function removeRelatedPicture() {
        $("[other-picture-Id]").click(function () {
            $(this).remove();
        });
    }

    function addRelatedPicture(picture) {
        var panelUrl = $("#PanelUrl").val();
        var url = picture.Url;
        url = url.replace("SYSTEM_TYPE_PANEL", panelUrl);
        var picHtml = "<div other-picture-Id='" + picture.Id + "' class='col-pic-20'>";
        picHtml += "<label class='other-picture-item'>";
        picHtml += "<img src='" + url + "' />";
        picHtml += "<label class='delete-other-picture' picture-id='" + picture.Id + "'>";
        picHtml += "<i class='fa fa-close' aria-hidden='true'></i>";
        picHtml += "</label>";
        picHtml += "<label>";
        picHtml += "</div>";
        $(".related-picture-container > .row").prepend(picHtml);

        var allItems = $("[other-picture-Id]");
        if (allItems.length > 9) {
            $(".add-item").hide();
        }

        removeRelatedPicture();
    }

    function fillEntity() {
        product.ID = $("#ID").val();
        product.Schema = $("#Schema").val();


        product.StartDate = $("#StartDate").val();
        product.EndDate = $("#EndDate").val();
        product.ErrorLowCount = $("#ErrorLowCount").val();
        product.IsSpecialProduct = $("#IsSpecialProduct").val();
        product.IsAdvertising = $("#IsAdvertising").val();

        product.NoIndex = $('#NoIndex').prop('checked');
        product.NoFollow = $('#NoFollow').prop('checked');
        product.Canonical = $("#Canonical").val();

        product.VideoDemoFileId = $("#VideoDemoFileId").val();
        product.VideoFileId = $("#VideoFileId").val();
        product.MusicFileId = $("#MusicFileId").val();
        product.MusicDemoFileId = $("#MusicDemoFileId").val();
        product.AudioFileId = $("#AudioFileId").val();
        product.AudioDemoFileId = $("#AudioDemoFileId").val();
        product.ZipFileId = $("#ZipFileId").val();
        product.PdfFileId = $("#PdfFileId").val();
        product.PdfDemoFileId = $("#PdfDemoFileId").val();
        product.ShopId = $("#ShopId").selected() == null ? parseInt($("#ShopId").val()) : $("#ShopId").selected();
        product.ProductTypeId = $("#ProductTypeId").selected() == null ? parseInt($("#ProductTypeId").val()) : $("#ProductTypeId").selected();
        product.ProductCategoryId = $("#ProductCategoryId").selected() == null ? parseInt($("#ProductCategoryId").val()) : $("#ProductCategoryId").selected();
        product.ProductSubCategoryId = $("#ProductSubCategoryId").selected() == null ? parseInt($("#ProductSubCategoryId").val()) : $("#ProductSubCategoryId").selected();
        product.UnitId = $("#UnitId").selected() == null ? parseInt($("#UnitId").val()) : $("#UnitId").selected();
        product.CompanyId = $("#CompanyId").selected() == null ? parseInt($("#CompanyId").val()) : $("#CompanyId").selected();
        product.BrandId = $("#BrandId").selected();
        product.StatusId = $("#StatusId").val();
        product.Name = $("#Name").val();
        product.Summary = $("#Summary").val();
        product.CodeValue = $("#CodeValue").val();
        product.ShowNumber = $("#ShowNumber").val() != undefined ? $("#ShowNumber").val().GetInteger() : 0;
        product.Description = tinymce.get('Description') != null ? tinymce.get('Description').getContent() : "";
        product.Price = $("#Price").val() != undefined ? $("#Price").val().GetInteger() : 0;
        product.ShowHomePage = $("#ShowHomePage").prop("checked");
        product.ProductCustomValue = [];
        product.PictureId = uploadPictureId == null ? $("#LastPictureId").val() : uploadPictureId;
        product.DocId = uploadDocumentId == null ? $("#LastDocId").val() : uploadDocumentId;
        product.Active = ($("#Active").val() === "True");
        product.SyncDatetime = $("#SyncDatetime").val();
        product.CreateDateTime = $("#CreateDateTime").val();
        product.SyncId = $("#SyncId").val();
        product.Quantity = $("#Quantity").val();
        product.VisitCount = $("#VisitCount").val();
        product.UpdateDatetime = $("#UpdateDatetime").val();
        product.SyncDatetime = $("#SyncDatetime").val();
        product.IsService = $('input[name=IsService]:checked').val();
        product.IsCopy = $('input[name=IsCopy]:checked').val();

        product.H1 = $('#H1').val();
        product.Title = $('#Title').val();
        product.Url = $('#Url').val();
        product.Img = $('#Img').val();
        product.Latitude = $("#Latitude").val();
        product.Longitude = $("#Longitude").val();
        product.ProductOptionId = $("#ProductOptionId").selected();
        product.Weight = $('#Weight').val();
        product.MaxOrderCount = $('#MaxOrderCount').val();
        product.MinOrder = $('#MinOrder').val();

        getCustomValues(product);
        product.ProductPicture = [];
        var relatedPictures = $("[other-picture-Id]");
        for (var i = 0; i < relatedPictures.length; i++) {
            var picId = $(relatedPictures[i]).attr("other-picture-Id");
            var picEntity = {
                PictureId: parseInt(picId),
            };
            product.ProductPicture.push(picEntity);
        }
        product.ProductColor = [];
        product.ProductVideo = [];
        $(".rowVideo").each(function () {
            var filmEntity = {
                product: product.ID,
                PictureId: $(this).find(".pictureVideoId").val(),
                Url: $(this).find(".VideoContent").val()
            };
            product.ProductVideo.push(filmEntity);
        });
        var colorInputs = $(".color.chk-container input");
        for (var i = 0; i < colorInputs.length; i++) {
            var thisInput = colorInputs[i];
            if ($(thisInput).prop("checked") === true) {
                var colorId = $(thisInput).attr("color-id");
                var colorEntity = {
                    ProductId: product.ID,
                    ColorId: parseInt(colorId),
                };
                product.ProductColor.push(colorEntity);
            }
        }

        product.Review = [];
        $(".rowReview").each(function () {
            var reviwEntity = {
                ProductId: product.ID,
                PictureId: $(this).find(".pictureReviewId").val(),
                Contect: $(this).find(".reviewContent").val()
            };
            product.Review.push(reviwEntity);
        });
        product.ProductSize = [];
        var sizeInputs = $(".size.chk-container input");
        for (var j = 0; j < sizeInputs.length; j++) {
            thisInput = sizeInputs[j];
            if ($(thisInput).prop("checked") === true) {
                var sizeId = $(thisInput).attr("size-id");
                var sizeEntity = {
                    ProductId: product.ID,
                    SizeId: parseInt(sizeId)
                };
                product.ProductSize.push(sizeEntity);
            }
        }
        product.ProductLanguage = [];
        var langValues = $("[lang-value]");
        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    LanguageId: parseInt($(langObject).find("[lang-name='LanguageId']").val()),
                    Name: $(langObject).find("[lang-name='Name']").val(),
                    Summary: $(langObject).find("[lang-name='Summary']").val(),
                    Description: tinymce.get($(langObject).find("[lang-name='Description']").attr('id')).getContent(),
                    /* Body: tinymce.get($(langObject).find("[lang-name='Body']").attr('id')).getContent(),*/
                    Url: $(langObject).find("[lang-name='Url']").val(),
                    Title: $(langObject).find("[lang-name='Title']").val(),
                    H1: $(langObject).find("[lang-name='H1']").val()
                };
                product.ProductLanguage.push(langEntity);
            }
        }
        var HasMultiCategory = $("#HasMultiCategory").val();
        if (HasMultiCategory == 'True') {
            product.ProductsType = [];
            var typeId = $("#ProductTypeId").val();
            if (typeId != null) {
                const arrayTypeId = typeId.toString().split(',');

                for (var i in arrayTypeId) {
                    var typeIdEntity = {
                        ProductId: product.ID,
                        ProductTypeId: parseInt(arrayTypeId[i])
                    };
                    product.ProductsType.push(typeIdEntity);
                }
            }

            product.ProductsCategory = [];
            var categoryId = $("#ProductCategoryId").val();
            if (categoryId != null) {
                const arrayCategoryId = categoryId.toString().split(',');

                for (var j in arrayCategoryId) {
                    var categoryIdEntity = {
                        ProductId: product.ID,
                        ProductCategoryId: parseInt(arrayCategoryId[j])
                    };
                    product.ProductsCategory.push(categoryIdEntity);
                }
            }

            product.ProductsSubCategory = [];
            var subcategoryId = $("#ProductSubCategoryId").val();
            if (subcategoryId != null) {

                const arraySubCategoryId = subcategoryId.toString().split(',');

                for (var h in arraySubCategoryId) {
                    var subcategoryIdEntity = {
                        ProductId: product.ID,
                        ProductSubCategoryId: parseInt(arraySubCategoryId[h])
                    };
                    product.ProductsSubCategory.push(subcategoryIdEntity);
                }
            }
        }
    }

    function fillProductType() {
        //var selected = getSelectedValue($("#ShopId")) == undefined ? $("#ShopId").val() : getSelectedValue($("#ShopId"));
        var request = createRequest();
        var HasMultiCategory = $("#HasMultiCategory").val();
        request.type = REQUEST_TYPE_GET;
        request.url = base_admin_url + "/store/product/FillProductType";
        //request.url = base_admin_url + "/store/product/FillProductType?ShopId=" + selected;
        request.success = function (entity) {
            clearDropDown("ProductTypeId", true);
            clearDropDown("ProductCategoryId", true);
            clearDropDown("SubCategoryId", true);
            clearDropDown("BrandId", true);
            clearDropDown("fillProductOptionId", true);
            var isTypeSelected = $("#isTypeSelected").val() === "False" ? true : false;
            bindDropDown("ProductTypeId", entity, "Name", "Id", isTypeSelected, $("#LastProductTypeId").val());
            if (HasMultiCategory == 'True') {
                $('#ProductTypeId').selectpicker('refresh');
                var typeId = $("#LastProductTypeId").val();
                const arrayTypeId = typeId.toString().split(',');

                $("#ProductTypeId > option").each(function () {
                    for (var i in arrayTypeId) {
                        if (arrayTypeId[i] == this.value) {

                            $(this).attr("selected", "selected");

                        }
                    }

                });
            }
            fillCategory();
            fillBrand();
            fillCustomFields();
            fillColors();
            fillSize();
            fillProductOption();
            $('.selectpicker').selectpicker('refresh');
        }
        $.ajax(request);
    }
    function fillCustomFields() {

        var productId = $("#ID").val();
        var selectedType = getSelectedValue($("#ProductTypeId"));
        var selectedCategory = getSelectedValue($("#ProductCategoryId"));
        var selectedSubCategory = getSelectedValue($("#ProductSubCategoryId"));
        var selectedBrandId = getSelectedValue($("#BrandId"));
        if (selectedType !== undefined) {
            selectedCategory = selectedCategory === undefined ? 0 : selectedCategory;
            selectedSubCategory = selectedSubCategory === undefined ? 0 : selectedSubCategory;
            selectedBrandId = selectedBrandId === undefined ? 0 : selectedBrandId;
            var request = createRequest();
            request.type = REQUEST_TYPE_GET;
            request.url = base_admin_url + "/store/product/FillProductCustomFields?TypeId=" + selectedType + "&CategoryId=" + selectedCategory + "&SubCategoryId=" + selectedSubCategory + "&BrandId=" + selectedBrandId + "&ProductId=" + productId;
            request.success = function (entity) {
                var result = entity;
                $("#div-custom-part").empty();
                if (result.length > 0) {
                    $("#div-custom-part").show();
                    var html = "";
                    for (var i = 0; i < result.length; i++) {
                        html += getCustomFieldControl(result[i]);
                    }
                    $("#div-custom-part").append(html);
                } else {
                    $("#div-custom-part").hide();
                }
                fillEditors();
            }
            $.ajax(request);

        } else {
            $("#div-custom-part").hide();
        }
    }

    function fillColors() {
        var productId = $("#ID").val();
        productId = productId === undefined ? 0 : productId;
        var selected = getSelectedValue($("#ProductTypeId"));
        if (selected !== undefined) {
            var request = createRequest();
            request.type = REQUEST_TYPE_GET;
            request.url = base_admin_url + "/store/product/FillColors?TypeId=" + selected + "&ProductId=" + productId;
            request.success = function (entity) {
                $("#color-container").empty();
                var result = entity;
                if (result.length > 0) {
                    $("#div-color-part").show();
                    var html = "";
                    for (var i = 0; i < result.length; i++) {
                        var item = result[i];
                        var isSelected = item.IsSelected == true ? "checked='checked'" : "";
                        html += "<label class='chk-container color'>";
                        html += "<input color-id='" + item.Id + "' type='checkbox' " + isSelected + ">";
                        html += "<span class='checkmark' style='background-color:#" + item.Hex + "'></span>";
                        html += item.Name;
                        html += "</label>";
                    }
                    $("#color-container").html(html);
                } else {
                    $("#div-color-part").hide();
                }
            }
            $.ajax(request);
        } else {
            $("#div-color-part").hide();
        }
    }

    function fillSize() {
        var selected = getSelectedValue($("#ProductTypeId"));
        var productId = $("#ID").val();
        productId = productId === undefined ? 0 : productId;
        if (selected !== undefined) {
            var request = createRequest();
            request.type = REQUEST_TYPE_GET;
            request.url = base_admin_url + "/store/product/FillSize?TypeId=" + selected + "&ProductId=" + productId;
            request.success = function (entity) {
                $("#size-container").empty();
                var result = entity;
                if (result.length > 0) {
                    $("#div-size-part").show();
                    var html = "";
                    for (var i = 0; i < result.length; i++) {
                        var item = result[i];
                        var isSelected = item.IsSelected == true ? "checked='checked'" : "";
                        html += "<label class='chk-container size'>";
                        html += "<input size-id='" + item.Id + "' type='checkbox' " + isSelected + ">";
                        html += "<span class='checkmark' style='background-color:#" + item.Hex + "'></span>";
                        html += item.Name;
                        html += "</label>";

                    }
                    $("#size-container").html(html);
                } else {
                    $("#div-size-part").hide();
                }
            }
            $.ajax(request);
        } else {
            $("#div-size-part").hide();
        }
    }

    function fillBrand() {
        var selected = getSelectedValue($("#ProductTypeId"));
        if (selected !== undefined) {
            var request = createRequest();
            request.type = REQUEST_TYPE_GET;
            request.url = base_admin_url + "/store/product/FillProductBrand?TypeId=" + selected;
            request.success = function (entity) {
                clearDropDown("BrandId");
                bindDropDown("BrandId", entity, "Name", "Id", true, $("#LastBrandId").val());
                fillCustomFields();
            }
            $.ajax(request);
        }
    }
    function fillProductOption() {
        var selected = getSelectedValue($("#ProductTypeId"));
        if (selected !== undefined) {
            var request = createRequest();
            request.type = REQUEST_TYPE_GET;
            request.url = base_admin_url + "/store/product/FillProductOption?TypeId=" + selected;
            request.success = function (entity) {
                clearDropDown("ProductOptionId");
                bindDropDown("ProductOptionId", entity, "Name", "Id", true, $("#LastProductOptionId").val());
                fillCustomFields();
            };
            $.ajax(request);
        }
    }
    function fillCategory() {
        var selected = getSelectedValue($("#ProductTypeId"));
        var HasMultiCategory = $("#HasMultiCategory").val();
        if (selected !== undefined) {
            var request = createRequest();
            request.type = REQUEST_TYPE_GET;
            request.url = base_admin_url + "/store/product/FillProductCategory?TypeId=" + selected;
            request.success = function (entity) {
                clearDropDown("ProductCategoryId");
                clearDropDown("SubCategoryId");
                bindDropDown("ProductCategoryId", entity, "Name", "Id", true, $("#LastProductCategoryId").val());
                if (HasMultiCategory == 'True') {
                    $('#ProductCategoryId').selectpicker('refresh');
                    var categoryId = $("#LastProductCategoryId").val();
                    const arrayId = categoryId.toString().split(',');

                    $("#ProductCategoryId > option").each(function () {
                        for (var i in arrayId) {
                            if (arrayId[i] == this.value) {

                                $(this).attr("selected", "selected");

                            }
                        }

                    });
                }
                fillCustomFields();
                fillSubCategory();
                $('.selectpicker').selectpicker('refresh');
            }
            $.ajax(request);
        }
    }

    function fillSubCategory() {
        var HasMultiCategory = $("#HasMultiCategory").val();
        var selected = getSelectedValue($("#ProductCategoryId"));
        if (selected !== undefined) {
            var request = createRequest();
            request.type = REQUEST_TYPE_GET;
            request.url = base_admin_url + "/store/product/FillProductSubCategory?CategoryId=" + selected;
            request.success = function (entity) {
                bindDropDown("ProductSubCategoryId", entity, "Name", "Id", true, $("#LastProductSubCateogryId").val());
                if (HasMultiCategory == 'True') {
                    $('#ProductSubCategoryId').selectpicker('refresh');
                    var subcategoryId = $("#LastProductSubCateogryId").val();
                    const arrayId = subcategoryId.toString().split(',');

                    $("#ProductSubCategoryId > option").each(function () {
                        for (var i in arrayId) {
                            if (arrayId[i] == this.value) {

                                $(this).attr("selected", "selected");

                            }
                        }

                    });
                }
                fillCustomFields();
                $('.selectpicker').selectpicker('refresh');
            }
            $.ajax(request);
        }
    }

    function fillEditors() {
        var editors = $("[custom-type='HTML']");
        for (var i = 0; i < editors.length; i++) {
            var editorId = $(editors[i]).attr("id");
            var isFound = false;
            for (var j = 0; j < editorList.length; j++) {
                if (editorList[j] == editorId) {
                    isFound = true;
                }
            }
            if (isFound == false) {
                editorList.push(editorId);
                initCodeprocessEditor(editorId);
            }
        }
    }
}

function initializeNewAdvertisingScript() {
    $(".videodemo .dropify-clear").click(function () {
        $("#VideoDemoFileId").val("");
    });
    $(".video .dropify-clear").click(function () {
        $("#VideoFileId").val("");
    });
    $(".audiodemo .dropify-clear").click(function () {
        $("#AudioDemoFileId").val("");
    });
    $(".audio .dropify-clear").click(function () {
        $("#AudioFileId").val("");
    });
    $(".musicdemo .dropify-clear").click(function () {
        $("#MusicDemoFileId").val("");
    });
    $(".music .dropify-clear").click(function () {
        $("#MusicFileId").val("");
    });
    $(".pdfdemo .dropify-clear").click(function () {
        $("#PdfDemoFileId").val("");
    });
    $(".pdf .dropify-clear").click(function () {
        $("#PdfFileId").val("");
    });
    $(".zip .dropify-clear").click(function () {
        $("#ZipFileId").val("");
    });
    var product = {};
    fillProductType();
    fillCustomFields();
    fillState();

    removeRelatedPicture();

    $(".dropify-clear").click(function () {

        uploadPictureId = 0;
        uploadWriterPictureId = null;
    });

    $("#btnSubmit").click(function () {
        fillEntity();
        $.ajax(createRequest(product));
    });
    //if ($('input[name=IsService]:checked').val() == "false") {

    //    $(".map-div").hide();
    //}
    //$("input[name=IsService]").change(function () {
    //    var elm = $(this);
    //    if (elm.val() == "true") {
    //        $(".map-div").show();
    //    }
    //    else {
    //        $(".map-div").hide();

    //    }
    //});
    function fillState() {
        //var selected = getSelectedValue($("#ShopId")) == undefined ? $("#ShopId").val() : getSelectedValue($("#ShopId"));
        var request = createRequest();
        request.type = REQUEST_TYPE_GET;
        request.url = "/panel" + "/store/Advertising/fillState";
        //request.url = "/panel" + "/store/product/FillProductType?ShopId=" + selected;
        request.success = function (entity) {
            var isTypeSelected = $("#isTypeSelected").val() === "False" ? true : false;
            bindDropDown("StateId", entity, "Name", "Id", isTypeSelected, $("#LastStateId").val());
            fillCity();
        }
        $.ajax(request);
    }
    $("#StateId").change(function () {
        fillCity();
    });
    function fillCity() {
        var selected = getSelectedValue($("#StateId"));
        if (selected !== undefined) {
            var request = createRequest();
            request.type = REQUEST_TYPE_GET;
            request.url = "/panel" + "/store/Advertising/FillCity?stateId=" + selected;
            request.success = function (entity) {
                bindDropDown("CityId", entity, "Name", "Id", true, $("#LastCityId").val());
            }
            $.ajax(request);
        }
    }
    function getCustomValues(product) {
        product.ProductCustomValue = [];

        var customElements = $("[custom-id]");

        for (var i = 0; i < customElements.length; i++) {
            var element = customElements[i];
            var entity = {
                ID: 0,
                FieldId: $(element).attr("custom-id").GetInteger(),
                Value: getInputValue(element),
                ItemId: getSelectValue(element)
            };
            product.ProductCustomValue.push(entity);
        }
    }

    function getSelectValue(element) {
        var Value;
        var elementType = $(element).attr("custom-type");
        if (elementType === "DROPDOWN")
            Value = $(element).find(":selected").attr("value");
        else
            Value = null;
        return Value;
    }

    function getInputValue(element) {
        var Value;
        var elementType = $(element).attr("custom-type");
        if (elementType === "STRING")
            Value = $(element).val();
        else if (elementType === "BOOLEAN")
            Value = $(element).prop("checked") == true ? "True" : "False";
        else if (elementType === "INTEGER")
            Value = $(element).val();
        else if (elementType === "DROPDOWN")
            Value = "";
        else
            Value = "";
        return Value;
    }

    $("#ShopId").change(function () {
        fillProductType();
    });

    $("#ProductTypeId").change(function () {

        fillCategory();
        fillBrand();
        fillCustomFields();
        fillColors();
        fillSize();
        fillProductOption();

    });

    $("#ProductCategoryId").change(function () {
        fillSubCategory();
        fillCustomFields();
    });

    $("#ProductSubCategoryId").change(function () {
        fillCustomFields();
    });

    $("#BrandId").change(function () {
        fillCustomFields();
    });

    $("#uplOtherPicture").change(function () {

        var fileUpload = $("#uplOtherPicture").get(0);
        var files = fileUpload.files;

        var data = new FormData();
        for (var i = 0; i < files.length; i++) {
            data.append(files[i].name, files[i]);
        }
        var request = createRequest();
        request.url = base_admin_url + "/Upload/UploadPhoto";
        request.type = "POST";
        request.data = data,
            request.contentType = false;
        request.processData = false;
        request.beforeSend = function () {
            $("#uplOtherPicture").val("");
            $("#modalLoading").modal("show");
        }
        request.error = function () {
            createMessage(MESSAGE_TYPE_ERROR, "خطا", "خطا در هنگام آپلود تصویر");
        };
        request.success = function (entity) {
            addRelatedPicture(entity);
            $("#modalLoading").modal("hide");
        };
        $.ajax(request);
    });

    function removeRelatedPicture() {
        $("[other-picture-Id]").click(function () {
            $(this).remove();
        });
    }

    function addRelatedPicture(picture) {
        var panelUrl = $("#PanelUrl").val();
        var url = picture.Url;
        url = url.replace("SYSTEM_TYPE_PANEL", panelUrl);
        var picHtml = "<div other-picture-Id='" + picture.Id + "' class='col-pic-20'>";
        picHtml += "<label class='other-picture-item'>";
        picHtml += "<img src='" + url + "' />";
        picHtml += "<label class='delete-other-picture' picture-id='" + picture.Id + "'>";
        picHtml += "<i class='fa fa-close' aria-hidden='true'></i>";
        picHtml += "</label>";
        picHtml += "<label>";
        picHtml += "</div>";
        $(".related-picture-container > .row").prepend(picHtml);

        var allItems = $("[other-picture-Id]");
        if (allItems.length > 9) {
            $(".add-item").hide();
        }

        removeRelatedPicture();
    }

    function fillEntity() {
        product.ID = $("#ID").val();
        product.Status = $("#Status").val() == 0 ? parseInt($("#Status").val()) : $("#Status").val();

        product.StartDate = $("#StartDate").val();
        product.EndDate = $("#EndDate").val();
        product.ErrorLowCount = $("#ErrorLowCount").val();

        product.LadderActive = $("#LadderActive").val();
        product.LadderDate = $("#LadderDate").val();
        product.StateId = $("#StateId").val();
        product.CityId = $("#CityId").val();
        product.Phone = $("#Phone").val();
        product.Address = $("#Address").val();
        product.IsPublish = $("#IsPublish").val();
        product.AccountId = $("#AccountId").val();
        product.IsAdvertising = $("#IsAdvertising").val();


        product.VideoDemoFileId = $("#VideoDemoFileId").val();
        product.VideoFileId = $("#VideoFileId").val();
        product.MusicFileId = $("#MusicFileId").val();
        product.MusicDemoFileId = $("#MusicDemoFileId").val();
        product.AudioFileId = $("#AudioFileId").val();
        product.AudioDemoFileId = $("#AudioDemoFileId").val();
        product.ZipFileId = $("#ZipFileId").val();
        product.PdfFileId = $("#PdfFileId").val();
        product.PdfDemoFileId = $("#PdfDemoFileId").val();
        product.ShopId = $("#ShopId").selected() == null ? parseInt($("#ShopId").val()) : $("#ShopId").selected();
        product.ProductTypeId = $("#ProductTypeId").selected() == null ? parseInt($("#ProductTypeId").val()) : $("#ProductTypeId").selected();
        product.ProductCategoryId = $("#ProductCategoryId").selected() == null ? parseInt($("#ProductCategoryId").val()) : $("#ProductCategoryId").selected();
        product.ProductSubCategoryId = $("#ProductSubCategoryId").selected() == null ? parseInt($("#ProductSubCategoryId").val()) : $("#ProductSubCategoryId").selected();
        product.UnitId = $("#UnitId").selected() == null ? parseInt($("#UnitId").val()) : $("#UnitId").selected();
        product.CompanyId = $("#CompanyId").selected() == null ? parseInt($("#CompanyId").val()) : $("#CompanyId").selected();
        product.BrandId = $("#BrandId").selected();
        product.StatusId = $("#StatusId").val();
        product.Name = $("#Name").val();
        product.Summary = $("#Summary").val();
        product.CodeValue = $("#CodeValue").val();
        product.ShowNumber = $("#ShowNumber").val() != undefined ? $("#ShowNumber").val().GetInteger() : 0;
        product.Description = tinymce.get('Description') != null ? tinymce.get('Description').getContent() : "";
        product.Price = $("#Price").val() != undefined ? $("#Price").val().GetInteger() : 0;

        product.ShowHomePage = $("#ShowHomePage").prop("checked");
        product.ProductCustomValue = [];
        product.PictureId = uploadPictureId == null ? $("#LastPictureId").val() : uploadPictureId;
        product.DocId = uploadDocumentId == null ? $("#LastDocId").val() : uploadDocumentId;
        product.Active = ($("#Active").val() === "True");
        product.SyncDatetime = $("#SyncDatetime").val();
        product.CreateDateTime = $("#CreateDateTime").val();
        product.SyncId = $("#SyncId").val();
        product.Quantity = $("#Quantity").val();
        product.VisitCount = $("#VisitCount").val();
        product.UpdateDatetime = $("#UpdateDatetime").val();
        product.SyncDatetime = $("#SyncDatetime").val();
        product.IsService = $('input[name=IsService]:checked').val();
        product.IsCopy = $('input[name=IsCopy]:checked').val();

        product.H1 = $('#H1').val();
        product.Title = $('#Title').val();
        product.Url = $('#Url').val();
        product.Img = $('#Img').val();
        product.Latitude = $("#Latitude").val();
        product.Longitude = $("#Longitude").val();
        product.ProductOptionId = $("#ProductOptionId").selected();
        product.Weight = $('#Weight').val();
        product.MaxOrderCount = $('#MaxOrderCount').val();
        getCustomValues(product);
        product.ProductPicture = [];
        var relatedPictures = $("[other-picture-Id]");
        for (var i = 0; i < relatedPictures.length; i++) {
            var picId = $(relatedPictures[i]).attr("other-picture-Id");
            var picEntity = {
                PictureId: parseInt(picId),
            };
            product.ProductPicture.push(picEntity);
        }
        product.ProductColor = [];
        product.ProductVideo = [];
        $(".rowVideo").each(function () {
            var filmEntity = {
                product: product.ID,
                PictureId: $(this).find(".pictureVideoId").val(),
                Url: $(this).find(".VideoContent").val()
            };
            product.ProductVideo.push(filmEntity);
        });
        var colorInputs = $(".color.chk-container input");
        for (var i = 0; i < colorInputs.length; i++) {
            var thisInput = colorInputs[i];
            if ($(thisInput).prop("checked") === true) {
                var colorId = $(thisInput).attr("color-id");
                var colorEntity = {
                    ProductId: product.ID,
                    ColorId: parseInt(colorId),
                };
                product.ProductColor.push(colorEntity);
            }
        }

        product.Review = [];
        $(".rowReview").each(function () {
            var reviwEntity = {
                ProductId: product.ID,
                PictureId: $(this).find(".pictureReviewId").val(),
                Contect: $(this).find(".reviewContent").val()
            };
            product.Review.push(reviwEntity);
        });
        product.ProductSize = [];
        var sizeInputs = $(".size.chk-container input");
        for (var j = 0; j < sizeInputs.length; j++) {
            thisInput = sizeInputs[j];
            if ($(thisInput).prop("checked") === true) {
                var sizeId = $(thisInput).attr("size-id");
                var sizeEntity = {
                    ProductId: product.ID,
                    SizeId: parseInt(sizeId)
                };
                product.ProductSize.push(sizeEntity);
            }
        }
        product.ProductLanguage = [];
        var langValues = $("[lang-value]");
        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    LanguageId: parseInt($(langObject).find("[lang-name='LanguageId']").val()),
                    Name: $(langObject).find("[lang-name='Name']").val(),
                    Summary: $(langObject).find("[lang-name='Summary']").val(),
                    Description: tinymce.get($(langObject).find("[lang-name='Description']").attr('id')).getContent(),
                    /* Body: tinymce.get($(langObject).find("[lang-name='Body']").attr('id')).getContent(),*/
                    Url: $(langObject).find("[lang-name='Url']").val(),
                    Title: $(langObject).find("[lang-name='Title']").val(),
                    H1: $(langObject).find("[lang-name='H1']").val()
                };
                product.ProductLanguage.push(langEntity);
            }
        }
    }

    //function fillProductType() {
    //    //var selected = getSelectedValue($("#ShopId")) == undefined ? $("#ShopId").val() : getSelectedValue($("#ShopId"));
    //    var request = createRequest();
    //    request.type = REQUEST_TYPE_GET;
    //    request.url = base_admin_url + "/store/product/FillProductType";
    //    //request.url = base_admin_url + "/store/product/FillProductType?ShopId=" + selected;
    //    request.success = function (entity) {
    //        clearDropDown("ProductTypeId", true);
    //        clearDropDown("ProductCategoryId", true);
    //        clearDropDown("SubCategoryId", true);
    //        clearDropDown("BrandId", true);
    //        clearDropDown("fillProductOptionId", true);
    //        var isTypeSelected = $("#isTypeSelected").val() === "False" ? true : false;
    //        bindDropDown("ProductTypeId", entity, "Name", "Id", isTypeSelected, $("#LastProductTypeId").val());

    //        fillCategory();
    //        fillBrand();
    //        fillCustomFields();
    //        fillColors();
    //        fillSize();
    //        fillProductOption();
    //    }
    //    $.ajax(request);
    //}
    function fillProductType() {
        //var selected = getSelectedValue($("#ShopId")) == undefined ? $("#ShopId").val() : getSelectedValue($("#ShopId"));
        var request = createRequest();
        var HasMultiCategory = $("#HasMultiCategory").val();
        request.type = REQUEST_TYPE_GET;
        request.url = base_admin_url + "/store/product/FillProductType";
        //request.url = base_admin_url + "/store/product/FillProductType?ShopId=" + selected;
        request.success = function (entity) {
            clearDropDown("ProductTypeId", true);
            clearDropDown("ProductCategoryId", true);
            clearDropDown("SubCategoryId", true);
            clearDropDown("BrandId", true);
            clearDropDown("fillProductOptionId", true);
            var isTypeSelected = $("#isTypeSelected").val() === "False" ? true : false;
            bindDropDown("ProductTypeId", entity, "Name", "Id", isTypeSelected, $("#LastProductTypeId").val());
            if (HasMultiCategory == 'True') {
                $('#ProductTypeId').selectpicker('refresh');
                var typeId = $("#LastProductTypeId").val();
                const arrayTypeId = typeId.toString().split(',');

                $("#ProductTypeId > option").each(function () {
                    for (var i in arrayTypeId) {
                        if (arrayTypeId[i] == this.value) {

                            $(this).attr("selected", "selected");

                        }
                    }

                });
            }
            fillCategory();
            fillBrand();
            fillCustomFields();
            fillColors();
            fillSize();
            fillProductOption();
            $('.selectpicker').selectpicker('refresh');
        }
        $.ajax(request);
    }

    function fillCustomFields() {

        var productId = $("#ID").val();
        var selectedType = getSelectedValue($("#ProductTypeId"));
        var selectedCategory = getSelectedValue($("#ProductCategoryId"));
        var selectedSubCategory = getSelectedValue($("#ProductSubCategoryId"));
        var selectedBrandId = getSelectedValue($("#BrandId"));
        if (selectedType !== undefined) {
            selectedCategory = selectedCategory === undefined ? 0 : selectedCategory;
            selectedSubCategory = selectedSubCategory === undefined ? 0 : selectedSubCategory;
            selectedBrandId = selectedBrandId === undefined ? 0 : selectedBrandId;
            var request = createRequest();
            request.type = REQUEST_TYPE_GET;
            request.url = base_admin_url + "/store/product/FillProductCustomFields?TypeId=" + selectedType + "&CategoryId=" + selectedCategory + "&SubCategoryId=" + selectedSubCategory + "&BrandId=" + selectedBrandId + "&ProductId=" + productId;
            request.success = function (entity) {
                var result = entity;
                $("#div-custom-part").empty();
                if (result.length > 0) {
                    $("#div-custom-part").show();
                    var html = "";
                    for (var i = 0; i < result.length; i++) {
                        html += getCustomFieldControl(result[i]);
                    }
                    $("#div-custom-part").append(html);
                } else {
                    $("#div-custom-part").hide();
                }
                fillEditors();
            }
            $.ajax(request);

        } else {
            $("#div-custom-part").hide();
        }
    }

    function fillColors() {
        var productId = $("#ID").val();
        productId = productId === undefined ? 0 : productId;
        var selected = getSelectedValue($("#ProductTypeId"));
        if (selected !== undefined) {
            var request = createRequest();
            request.type = REQUEST_TYPE_GET;
            request.url = base_admin_url + "/store/product/FillColors?TypeId=" + selected + "&ProductId=" + productId;
            request.success = function (entity) {
                $("#color-container").empty();
                var result = entity;
                if (result.length > 0) {
                    $("#div-color-part").show();
                    var html = "";
                    for (var i = 0; i < result.length; i++) {
                        var item = result[i];
                        var isSelected = item.IsSelected == true ? "checked='checked'" : "";
                        html += "<label class='chk-container color'>";
                        html += "<input color-id='" + item.Id + "' type='checkbox' " + isSelected + ">";
                        html += "<span class='checkmark' style='background-color:#" + item.Hex + "'></span>";
                        html += item.Name;
                        html += "</label>";
                    }
                    $("#color-container").html(html);
                } else {
                    $("#div-color-part").hide();
                }
            }
            $.ajax(request);
        } else {
            $("#div-color-part").hide();
        }
    }

    function fillSize() {
        var selected = getSelectedValue($("#ProductTypeId"));
        var productId = $("#ID").val();
        productId = productId === undefined ? 0 : productId;
        if (selected !== undefined) {
            var request = createRequest();
            request.type = REQUEST_TYPE_GET;
            request.url = base_admin_url + "/store/product/FillSize?TypeId=" + selected + "&ProductId=" + productId;
            request.success = function (entity) {
                $("#size-container").empty();
                var result = entity;
                if (result.length > 0) {
                    $("#div-size-part").show();
                    var html = "";
                    for (var i = 0; i < result.length; i++) {
                        var item = result[i];
                        var isSelected = item.IsSelected == true ? "checked='checked'" : "";
                        html += "<label class='chk-container size'>";
                        html += "<input size-id='" + item.Id + "' type='checkbox' " + isSelected + ">";
                        html += "<span class='checkmark' style='background-color:#" + item.Hex + "'></span>";
                        html += item.Name;
                        html += "</label>";

                    }
                    $("#size-container").html(html);
                } else {
                    $("#div-size-part").hide();
                }
            }
            $.ajax(request);
        } else {
            $("#div-size-part").hide();
        }
    }

    function fillBrand() {
        var selected = getSelectedValue($("#ProductTypeId"));
        if (selected !== undefined) {
            var request = createRequest();
            request.type = REQUEST_TYPE_GET;
            request.url = base_admin_url + "/store/product/FillProductBrand?TypeId=" + selected;
            request.success = function (entity) {
                clearDropDown("BrandId");
                bindDropDown("BrandId", entity, "Name", "Id", true, $("#LastBrandId").val());
                fillCustomFields();
            }
            $.ajax(request);
        }
    }
    function fillProductOption() {
        var selected = getSelectedValue($("#ProductTypeId"));
        if (selected !== undefined) {
            var request = createRequest();
            request.type = REQUEST_TYPE_GET;
            request.url = base_admin_url + "/store/product/FillProductOption?TypeId=" + selected;
            request.success = function (entity) {
                clearDropDown("ProductOptionId");
                bindDropDown("ProductOptionId", entity, "Name", "Id", true, $("#LastProductOptionId").val());
                fillCustomFields();
            };
            $.ajax(request);
        }
    }
    function fillCategory() {
        var selected = getSelectedValue($("#ProductTypeId"));
        var HasMultiCategory = $("#HasMultiCategory").val();
        if (selected !== undefined) {
            var request = createRequest();
            request.type = REQUEST_TYPE_GET;
            request.url = base_admin_url + "/store/product/FillProductCategory?TypeId=" + selected;
            request.success = function (entity) {
                clearDropDown("ProductCategoryId");
                clearDropDown("SubCategoryId");
                bindDropDown("ProductCategoryId", entity, "Name", "Id", true, $("#LastProductCategoryId").val());
                if (HasMultiCategory == 'True') {
                    $('#ProductCategoryId').selectpicker('refresh');
                    var categoryId = $("#LastProductCategoryId").val();
                    const arrayId = categoryId.toString().split(',');

                    $("#ProductCategoryId > option").each(function () {
                        for (var i in arrayId) {
                            if (arrayId[i] == this.value) {

                                $(this).attr("selected", "selected");

                            }
                        }

                    });
                }
                fillCustomFields();
                fillSubCategory();
                $('.selectpicker').selectpicker('refresh');
            }
            $.ajax(request);
        }
    }

    function fillSubCategory() {
        var HasMultiCategory = $("#HasMultiCategory").val();
        var selected = getSelectedValue($("#ProductCategoryId"));
        if (selected !== undefined) {
            var request = createRequest();
            request.type = REQUEST_TYPE_GET;
            request.url = base_admin_url + "/store/product/FillProductSubCategory?CategoryId=" + selected;
            request.success = function (entity) {
                bindDropDown("ProductSubCategoryId", entity, "Name", "Id", true, $("#LastProductSubCateogryId").val());
                if (HasMultiCategory == 'True') {
                    $('#ProductSubCategoryId').selectpicker('refresh');
                    var subcategoryId = $("#LastProductSubCateogryId").val();
                    const arrayId = subcategoryId.toString().split(',');

                    $("#ProductSubCategoryId > option").each(function () {
                        for (var i in arrayId) {
                            if (arrayId[i] == this.value) {

                                $(this).attr("selected", "selected");

                            }
                        }

                    });
                }
                fillCustomFields();
                $('.selectpicker').selectpicker('refresh');
            }
            $.ajax(request);
        }
    }


    function fillEditors() {
        var editors = $("[custom-type='HTML']");
        for (var i = 0; i < editors.length; i++) {
            var editorId = $(editors[i]).attr("id");
            var isFound = false;
            for (var j = 0; j < editorList.length; j++) {
                if (editorList[j] == editorId) {
                    isFound = true;
                }
            }
            if (isFound == false) {
                editorList.push(editorId);
                initCodeprocessEditor(editorId);
            }
        }
    }
}
function initializeDashboardFilesScript() {
    $(".FileDemo .dropify-clear").click(function () {
        $("#FileId").val("");
    });

    var DashboardFiles = {};


    $(".dropify-clear").click(function () {

        uploadPictureId = 0;
        uploadWriterPictureId = null;
    });

    $("#btnSubmit").click(function () {
        fillEntity();
        $.ajax(createRequest(DashboardFiles));
    });


    function fillEntity() {
        DashboardFiles.ID = $("#ID").val();
        DashboardFiles.FileId = $("#FileId").val();
        DashboardFiles.Name = $("#Name").val();
        DashboardFiles.Description = $("#Description").val();
    }







}

function initializeProductTypeScript() {
    var entity = {};
    $("#btnSubmit").click(function () {

        fillEntity();
        $.ajax(createRequest(entity));
    });
    $(".attachFile1 .dropify-clear").click(function () {
        AttachFile1 = 0;
    });
    $(".attachFile2 .dropify-clear").click(function () {
        AttachFile2 = 0;
    });
    function fillEntity() {
        entity.CustomLabel = $("#CustomLabel").val();
        entity.ID = $("#ID").val();
        entity.Schema = $("#Schema").val();

        entity.Name = $("#Name").val();
        entity.SyncId = $("#SyncId").val();
        entity.SyncDatetime = $("#SyncDatetime").val();
        entity.HaveAddress = $("#HaveAddress").val();

        entity.Description = $("#Description").val(); /*tinymce.get('Description').getContent();*/
        entity.body = tinymce.get('body').getContent();
        entity.Url = $("#Url").val();
        entity.H1 = $("#H1").val();
        entity.Canonical = $("#Canonical").val();

        entity.Priority = $("#Priority").val();
        entity.AttachFileId2 = AttachFile2 == null ? $("#AttachFileId2").val() : AttachFile2;
        entity.AttachFileId1 = AttachFile1 == null ? $("#AttachFileId1").val() : AttachFile1;
        entity.title = $("#title").val();
        entity.IsService = $('#IsService').prop('checked');
        entity.NoIndex = $('#NoIndex').prop('checked');
        entity.NoFollow = $('#NoFollow').prop('checked');

        entity.Active = $('#Active').prop('checked');
        //entity.PictureId = uploadPictureId;
        entity.PictureId = uploadPictureId == null ? $("#LastPictureId").val() : uploadPictureId;
        entity.ProductTypeLanguage = [];
        var langValues = $("[lang-value]");

        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    Name: $(langObject).find("[lang-name='Name']").val(),
                    LanguageId: parseInt($(langObject).find("[lang-name='LanguageId']").val()),
                    Description: $(langObject).find("[lang-name='Description']").val(), /*tinymce.get($(langObject).find("[lang-name='Description']").attr('id')).getContent(),*/
                    H1: $(langObject).find("[lang-name='H1']").val(),
                    Title: $(langObject).find("[lang-name='Title']").val(),
                    Url: $(langObject).find("[lang-name='Url']").val(),
                    body: tinymce.get($(langObject).find("[lang-name='body']").attr('id')).getContent()
                };
                entity.ProductTypeLanguage.push(langEntity);
            }
        }

    }
}

function initializeColorScript() {
    var entity = {};
    $("#btnSubmit").click(function () {
        fillEntity();
        $.ajax(createRequest(entity));
    });

    function fillEntity() {
        entity.ID = $("#ID").val();
        entity.Name = $("#Name").val();
        entity.HexValue = $("#HexValue").val();
        entity.ProductTypeId = $("#ProductTypeId").val();
        entity.SyncId = $("#SyncId").val();
        entity.SyncDatetime = $("#SyncDatetime").val();
        entity.ColorLanguage = [];

        var langValues = $("[lang-value]");
        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    Name: $(langObject).find("[lang-name='Name']").val(),
                    LanguageId: parseInt($(langObject).find("[lang-name='LanguageId']").val()),
                    Description: ""
                };
                entity.ColorLanguage.push(langEntity);
            }
        }
    }
}
function initializeSizeScript() {
    var entity = {};
    $("#btnSubmit").click(function () {
        fillEntity();
        $.ajax(createRequest(entity));
    });

    function fillEntity() {
        entity.ID = $("#ID").val();
        entity.Name = $("#Name").val();
        entity.ProductTypeId = $("#ProductTypeId").val();
        entity.SyncId = $("#SyncId").val();
        entity.SyncDatetime = $("#SyncDatetime").val();
        entity.SizeLanguage = [];

        var langValues = $("[lang-value]");
        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    Name: $(langObject).find("[lang-name='Name']").val(),
                    LanguageId: parseInt($(langObject).find("[lang-name='LanguageId']").val()),
                    Description: ""
                };
                entity.SizeLanguage.push(langEntity);
            }
        }
    }
}

function initializeQuestionProductScript() {
    var entity = {};
    $("#btnSubmit").click(function () {
        fillEntity();
        $.ajax(createRequest(entity));
    });

    function fillEntity() {
        entity.ID = $("#ID").val();
        entity.Question = $("#Question").val();
        entity.ProductId = $("#ProductId").val();
        entity.Answer = $("#Answer").val();
        entity.QuestionProductLanguage = [];

        var langValues = $("[lang-value]");
        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    Question: $(langObject).find("[lang-name='Question']").val(),
                    Answer: $(langObject).find("[lang-name='Answer']").val(),
                    LanguageId: parseInt($(langObject).find("[lang-name='LanguageId']").val())
                };
                entity.QuestionProductLanguage.push(langEntity);
            }
        }
    }
}
function initializeQuestionProductTypeScript() {
    var entity = {};
    $("#btnSubmit").click(function () {
        fillEntity();
        $.ajax(createRequest(entity));
    });

    function fillEntity() {
        entity.ID = $("#ID").val();
        entity.Question = $("#Question").val();
        entity.ProductTypeId = $("#ProductTypeId").val();
        entity.Answer = $("#Answer").val();
        entity.QuestionProductTypeLanguage = [];

        var langValues = $("[lang-value]");
        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    Question: $(langObject).find("[lang-name='Question']").val(),
                    Answer: $(langObject).find("[lang-name='Answer']").val(),
                    LanguageId: parseInt($(langObject).find("[lang-name='LanguageId']").val())
                };
                entity.QuestionProductTypeLanguage.push(langEntity);
            }
        }
    }
}
function initializeQuestionPostScript() {
    var entity = {};
    $("#btnSubmit").click(function () {
        fillEntity();
        $.ajax(createRequest(entity));
    });

    function fillEntity() {
        entity.ID = $("#ID").val();
        entity.Question = $("#Question").val();
        entity.PostId = $("#PostId").val();
        entity.Answer = $("#Answer").val();
        entity.QuestionPostLanguage = [];

        var langValues = $("[lang-value]");
        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    Question: $(langObject).find("[lang-name='Question']").val(),
                    Answer: $(langObject).find("[lang-name='Answer']").val(),
                    LanguageId: parseInt($(langObject).find("[lang-name='LanguageId']").val())
                };
                entity.QuestionPostLanguage.push(langEntity);
            }
        }
    }
}
function initializeQuestionProductCategoryScript() {
    var entity = {};
    $("#btnSubmit").click(function () {
        fillEntity();
        $.ajax(createRequest(entity));
    });

    function fillEntity() {
        entity.ID = $("#ID").val();
        entity.Question = $("#Question").val();
        entity.CategoryId = $("#CategoryId").val();
        entity.Answer = $("#Answer").val();

    }
}
function initializeQuestionProductSubCategoryScript() {
    var entity = {};
    $("#btnSubmit").click(function () {
        fillEntity();
        $.ajax(createRequest(entity));
    });

    function fillEntity() {
        entity.ID = $("#ID").val();
        entity.Question = $("#Question").val();
        entity.SubCategoryId = $("#SubCategoryId").val();
        entity.Answer = $("#Answer").val();

    }
}

function initializeWarrantyScript() {
    var entity = {};
    $("#btnSubmit").click(function () {
        fillEntity();
        $.ajax(createRequest(entity));
    });

    function fillEntity() {
        entity.ID = $("#ID").val();
        entity.Config = $("#Config").val();
        entity.Serial = $("#Serial").val();
        entity.Name = $("#Name").val();
        entity.Date = $("#Date").val();

    }
}

function initializeProductCategoryScript() {
    var entity = {};
    $("#btnSubmit").click(function () {

        fillEntity();
        $.ajax(createRequest(entity));
    });

    function fillEntity() {
        entity.CustomLabel = $("#CustomLabel").val();
        entity.ID = $("#ID").val();
        entity.Schema = $("#Schema").val();
        entity.NoIndex = $('#NoIndex').prop('checked');
        entity.NoFollow = $('#NoFollow').prop('checked');
        entity.Canonical = $("#Canonical").val();
        entity.TypeId = getSelectedValue("#TypeId");
        entity.Name = $("#Name").val();
        entity.Url = $("#Url").val();
        entity.Fee = $('#Fee').val();
        entity.TitlePage = $("#TitlePage").val();
        entity.Description = tinymce.get('Description').getContent();
        entity.DescriptionMeta = $("#DescriptionMeta").val(); /*tinymce.get('DescriptionMeta').getContent();*/
        entity.H1 = $("#H1").val();
        entity.Priority = $("#Priority").val();
        entity.SyncId = $("#SyncId").val();
        entity.SyncDatetime = $("#SyncDatetime").val();
        entity.ProductCategoryLanguage = [];
        entity.PictureId = uploadPictureId == null ? $("#LastPictureId").val() : uploadPictureId;
        entity.IconId = uploadIconId == null ? $("#LastIconId").val() : uploadIconId;
        entity.Active = $('#Active').prop('checked');
        var langValues = $("[lang-value]");
        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    Name: $(langObject).find("[lang-name='Name']").val(),
                    LanguageId: parseInt($(langObject).find("[lang-name='LanguageId']").val()),
                    Url: $(langObject).find("[lang-name='Url']").val(),
                    DescriptionMeta: $(langObject).find("[lang-name='DescriptionMeta']").val(),
                    TitlePage: $(langObject).find("[lang-name='TitlePage']").val(),
                    H1: $(langObject).find("[lang-name='H1']").val(),
                    Description: tinymce.get($(langObject).find("[lang-name='Description']").attr('id')).getContent()
                };
                entity.ProductCategoryLanguage.push(langEntity);
            }
        }
    }
}

function initializeProductCustomFieldScript() {
    var entity = {};
    $("#btnSubmit").click(function () {
        fillEntity();
        $.ajax(createRequest(entity));
    });

    function fillEntity() {
        entity.ID = $("#ID").val();
        entity.ProductTypeId = $("#ProductTypeId").val();
        entity.ProductCategoryId = $("#ProductCategoryId").val();
        entity.ProductSubCategoryId = $("#ProductSubCategoryId").val();
        entity.ProductBrandId = $("#ProductBrandId").val();
        entity.TypeId = getSelectedValue("#TypeId");
        entity.Name = $("#Name").val();
        entity.SyncId = $("#SyncId").val();
        entity.SyncName = $("#SyncName").val();
        entity.SyncDatetime = $("#SyncDatetime").val();
        entity.ShowNumber = $("#ShowNumber").val();
        entity.IsRequired = getCheckedValue("#IsRequired");
        entity.ProductCustomFieldLanguage = [];

        var langValues = $("[lang-value]");
        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    Name: $(langObject).find("[lang-name='Name']").val(),
                    LanguageId: parseInt($(langObject).find("[lang-name='LanguageId']").val()),
                };
                entity.ProductCustomFieldLanguage.push(langEntity);
            }
        }
    }
}
function initializeAnswerSmartOfferScript() {
    var entity = {};
    $("#btnSubmit").click(function () {
        fillEntity();
        $.ajax(createRequest(entity));
    });
    function fillEntity() {
        entity.ID = $("#ID").val();
        entity.QuestionId = $("#QuestionId").val();
        entity.PictureId = uploadPictureId == null ? $("#LastPictureId").val() : uploadPictureId;
        entity.Text = $("#Text").val();
        entity.ShowNumber = $("#ShowNumber").val();
        entity.Active = $("#Active").prop('checked');
        entity.AnswerSmartOfferLanguage = [];
        var langValues = $("[lang-value]");

        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    Text: $(langObject).find("[lang-name='Text']").val(),

                };
                entity.AnswerSmartOfferLanguage.push(langEntity);
            }
        }
    }
}

function initializeProductCustomItemScript() {
    var entity = {};
    $("#btnSubmit").click(function () {
        fillEntity();
        $.ajax(createRequest(entity));
    });

    function fillEntity() {
        entity.ID = $("#ID").val();
        entity.FieldId = $("#FieldId").val();
        entity.SyncId = $("#SyncId").val();
        entity.SyncDatetime = $("#SyncDatetime").val();
        entity.Value = $("#Value").val();

        entity.ProductCustomItemLanguage = [];

        var langValues = $("[lang-value]");
        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    Value: $(langObject).find("[lang-name='Value']").val(),
                    LanguageId: parseInt($(langObject).find("[lang-name='LanguageId']").val())
                };
                entity.ProductCustomItemLanguage.push(langEntity);
            }
        }
    }
}

function initializeDiscountScript() {
    var request = createRequest();
    request.type = REQUEST_TYPE_GET;
    request.url = base_admin_url + "/store/product/SearchAjax";
    request.success = function (result) {
        var products = [];
        for (var i = 0; i < result.length; i++) {
            var entity = {
                value: result[i].Name,
                data: result[i].Id
            };
            products.push(entity);
        }
        $('#ProductBox').autocomplete({
            lookup: products,
            onSelect: function (suggestion) {
                $("#ProductId").val(suggestion.data);
            }
        });
    }
    $.ajax(request);
}

function initializeProductSubCategoryScript() {

    var entity = {};
    $("#btnSubmit").click(function () {
        fillEntity();
        $.ajax(createRequest(entity));
    });

    function fillEntity() {

        entity.CustomLabel = $("#CustomLabel").val();
        entity.Schema = $("#Schema").val();
        entity.NoIndex = $('#NoIndex').prop('checked');
        entity.NoFollow = $('#NoFollow').prop('checked');
        entity.Canonical = $("#Canonical").val();
        entity.ID = $("#ID").val();
        entity.CategoryId = getSelectedValue("#CategoryId");
        entity.Name = $("#Name").val();
        entity.H1 = $("#H1").val();
        entity.Description = $("#Description").val();
        entity.Body = tinymce.get('Body').getContent();
        entity.Title = $("#Title").val();
        entity.Url = $("#Url").val();
        entity.Priority = $("#Priority").val();
        entity.Active = $('#Active').prop('checked');
        entity.PictureId = uploadPictureId == null ? $("#LastPictureId").val() : uploadPictureId;
        entity.ProductSubCategoryLanguage = [];
        entity.TagSubCategory = [];
        var tagInputs = $(".tag-container input");
        for (var i = 0; i < tagInputs.length; i++) {
            var thisInput = tagInputs[i];
            if ($(thisInput).prop("checked") === true) {
                var tagId = $(thisInput).attr("tag-id");
                var tagEntity = {
                    SubCategory: entity.ID,
                    TagId: parseInt(tagId)
                };
                entity.TagSubCategory.push(tagEntity);
            }
        }
        entity.PictureId = uploadPictureId == null ? $("#LastPictureId").val() : uploadPictureId;
        var langValues = $("[lang-value]");
        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    Name: $(langObject).find("[lang-name='Name']").val(),
                    CategoryId: entity.CategoryId,
                    LanguageId: parseInt($(langObject).find("[lang-name='LanguageId']").val()),
                    Description: $(langObject).find("[lang-name='description']").val(),
                    body: tinymce.get($(langObject).find("[lang-name='body']").attr('id')).getContent(),
                    Url: $(langObject).find("[lang-name='Url']").val(),
                    H1: $(langObject).find("[lang-name='H1']").val(),
                    Title: $(langObject).find("[lang-name='Title']").val(),

                };
                entity.ProductSubCategoryLanguage.push(langEntity);
            }
        }

    }
}

function initializeShopProfileScript() {
    var entity = {};
    $("#btnSubmit").click(function () {
        fillEntity();
        $.ajax(createRequest(entity));
    });

    function fillEntity() {
        var shopId = parseInt($("#ID").val())
        entity.ID = shopId;
        entity.PictureId = uploadPictureId === null ? parseInt($("#PictureId").val()) : uploadPictureId;
        entity.WriterPictureId = uploadWriterPictureId === null ? parseInt($("#WriterPictureId").val()) : uploadWriterPictureId;
        entity.Label = $("#Label").val();
        entity.UserCreatorId = parseInt($("#UserCreatorId").val());
        entity.WebsiteId = parseInt($("#WebsiteId").val());
        entity.Approved = $("#Approved").val();
        entity.Active = $("#Active").val();
        entity.Description = $("#Description").val();

        entity.Name = $("#Name").val();
        entity.TypeId = getSelectedValue("#TypeId");
        entity.CityId = getSelectedValue("#CityId");

        entity.ShopAddress = [];
        entity.ShopContact = [];
        entity.ShopProductType = [];
        entity.ShopPaymentType = [];

        var contactItems = $("[contact-id]");
        if (contactItems.length > 0) {
            for (var i = 0; i < contactItems.length; i++) {
                var value = $(contactItems[i]).val();
                if (value !== "") {
                    var typeId = parseInt($(contactItems[i]).attr("contact-id"));
                    var contactEntity = {
                        ShopId: shopId,
                        TypeId: typeId,
                        Value: value
                    };
                    entity.ShopContact.push(contactEntity);
                }
            }
        }

        var selectedTypes = $("[name='ProductType']:checked");
        for (var i = 0; i < selectedTypes.length; i++) {
            var value = $(selectedTypes[i]).val();
            var typeEntity = {
                ShopId: shopId,
                ProductTypeId: value
            };
            entity.ShopProductType.push(typeEntity);
        }

        var selectedPayment = $("[name='PaymentType']:checked");
        for (var i = 0; i < selectedPayment.length; i++) {
            var value = $(selectedPayment[i]).val();
            var paymentEntity = {
                ShopId: shopId,
                PaymentTypeId: value
            };
            entity.ShopPaymentType.push(paymentEntity);
        }

        var addEntity = {
            ID: parseInt($("#AddressId").val()),
            Address: $("#Address").val()
        };

        entity.ShopAddress.push(addEntity);
    }
}

function initializeReportByWeekdayScript() {
    var labels = [];
    var series = [];
    var visitCount = [];
    visitCount.push(0);
    var request = createRequest();
    request.type = REQUEST_TYPE_GET;
    request.url = base_admin_url + "/report/websiteview/reportbyweekday";
    request.success = function (entity) {
        for (var i = 0; i < entity.length; i++) {
            var temp = entity[i];
            if (i == 0)
                visitCount[0] = parseInt(temp.Value);
            labels.push(temp.Key);
            visitCount.push(parseInt(temp.Value));
        }
        series.push({
            name: 'بازدید روزانه',
            data: visitCount
        });

        new Chartist.Line('#simple-line-chart', {
            labels: labels,
            series: series
        },
            {
                plugins: [
                    Chartist.plugins.tooltip()
                ]
            }
        );
    }
    $.ajax(request);
}

function initializeOrderListScript() {
    $(document).ready(function () {
        $("[order-id]").click(function () {
            var isSelected = $(this).prop("checked");
            var orderId = $(this).attr("order-id");
            var selectedRow = $("tr[row-order-id='" + orderId + "']");
            $(selectedRow).attr("row-selected", isSelected);

            var selectedItems = $("[row-selected='true']");
            var selectedIds = "";
            for (var i = 0; i < selectedItems.length; i++) {
                var newRowId = $(selectedItems[i]).attr("row-order-id");
                selectedIds = selectedIds + "," + $(selectedItems[i]).attr("row-order-id");
            }
            var hrefValueStore = $(".goto-store").attr("href");
            var hrefValueProcess = $(".goto-process").attr("href");
            var splitHrefStore = hrefValueStore.split("orderId=");
            var splitHrefProcess = hrefValueProcess.split("orderId=");
            hrefValueStore = splitHrefStore[0] + "orderId=0" + selectedIds;
            hrefValueProcess = splitHrefProcess[0] + "orderId=0" + selectedIds;
            $(".goto-store").attr("href", hrefValueStore);
            $(".goto-process").attr("href", hrefValueProcess);
        });

        $("[select-all='true']").click(function () {
            var ischecked = $("[order-id]").prop("checked");
            $("[order-id]").prop("checked", ischecked);
            $("[order-id]").trigger("click");
        });

        $("#chkCheckAll").click(function () {
            var doAction = $(this).attr("do-action");
            if (doAction == "true") {
                $("[name='statusId']").prop("checked", true);
                $(this).attr("do-action", "false");
            } else {
                $("[name='statusId']").prop("checked", false);
                $(this).attr("do-action", "true");
            }
        });
    });
}


function initializeClearingListScript() {
    $(document).ready(function () {
        $("#chkCheckAll").click(function () {
            var doAction = $(this).attr("do-action");
            if (doAction == "true") {
                $("[name='codeId']").prop("checked", true);
                $(this).attr("do-action", "false");
            } else {
                $("[name='codeId']").prop("checked", false);
                $(this).attr("do-action", "true");
            }
        });
    });
}


function initializeProductQuantityScript() {
    $("#btnSubmit").click(function () {

        var productId = $("[name=ProductId]").val();
        var listQuantiy = [];
        var rowList = $("[quantity-id]");
        for (var i = 0; i < rowList.length; i++) {
            var item = rowList[i];
            var entity = {
                ID: parseInt($(item).attr("quantity-id")),
                Price: parseInt($(item).find("input.price").val()),
                Count: parseInt($(item).find("input.count").val())
            };
            listQuantiy.push(entity);
        }
        var data = { productId: productId, listQuantiy: listQuantiy };
        $.ajax(createRequest(data));
    });
}

function initializeMenuPageScript() {
    var entity = {};
    $("#btnSubmit").click(function () {
        fillEntity();
        $.ajax(createRequest(entity));
    });

    function fillEntity() {
        entity.ID = $("#ID").val();
        entity.ParentId = getSelectedValue("#ParentId");
        entity.TypeId = getSelectedValue("#TypeId");
        entity.PostId = getSelectedValue("#PostId");
        entity.CategoryId = getSelectedValue("#CategoryId");
        entity.GalleryId = getSelectedValue("#GalleryId");
        entity.Active = getCheckedValue("#Active");
        entity.Name = $("#Name").val();
        entity.Link = $("#Link").val();
        entity.ShowNumber = parseInt($("#ShowNumber").val());
        entity.MenuLanguage = [];

        var langValues = $("[lang-value]");
        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    Name: $(langObject).find("[lang-name='Name']").val(),
                    CategoryId: entity.CategoryId,
                    LanguageId: parseInt($(langObject).find("[lang-name='LanguageId']").val()),
                    Description: ""
                };
                entity.MenuLanguage.push(langEntity);
            }
        }
    }
}

function initializeDashboardPageScript() {

    $("#checkRebateOnline").click(function () {
        var codeValue = $("[name='codevalue']").val();
        var entity = {
            CodeValue: codeValue
        };
        var response = createRequest(entity);
        response.url = "/store/rebate/checkrebate";
        response.success = function (result) {
            if (result.Type === MESSAGE_ERROR) {
                createMessage(MESSAGE_TYPE_ERROR, result.Body);
            } else if (result.Type === MESSAGE_SUCCESS) {
                $(".btn-container").show();
                $("#btnPrintRebate").attr("href", "/store/rebate/print/" + codeValue);
            }
        };
        $.ajax(response);
    });

    $("#btnCancelRebate").click(function () {
        var codeValue = $("[name='codevalue']").val();
        var entity = {
            CodeValue: codeValue
        };
        var response = createRequest(entity);
        response.url = "/store/rebate/cancelrebate";
        response.success = function (result) {
            if (result.Type === MESSAGE_ERROR) {
                createMessage(MESSAGE_TYPE_ERROR, result.Body);
            } else if (result.Type === MESSAGE_SUCCESS) {
                $(".btn-container").hide();
                $("#btnPrintRebate").attr("href", "");
                $("[name='codevalue']").val("");
            }
        };
        $.ajax(response);
    });

}

function initializeBannerPageScript() {
    $(".banner_page .category_id").change(function () {
        var request = createRequest();
        request.type = REQUEST_TYPE_GET;
        request.url = base_admin_url + "/content/category/GetCategoryPicture?CategoryId=" + $(this).val();
        request.success = function (entity) {
            $(".banner_page img.category_img").attr("src", entity);
        }
        $.ajax(request);
    })

    var banner = {};
    $("#btnSubmit").click(function () {
        fillEntity();
        $.ajax(createRequest(banner));
    });
    function fillEntity() {
        banner.ID = $("#ID").val();
        banner.Name = $("#Name").val();
        banner.CategoryId = $("#CategoryId").val();
        banner.Summary = $("#Summary").val();
        banner.Url = $("#Url").val();
        banner.FileId = $("#FileId").val();

        banner.ShowNumber = $("#ShowNumber").val();
        banner.PictureId = uploadPictureId == null ? $("#pictureUploadId").val() : uploadPictureId;
        banner.BannerLanguage = [];
        var langValues = $("[lang-value]");
        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    LanguageId: parseInt($(langObject).find("[lang-name='LanguageId']").val()),
                    Name: $(langObject).find("[lang-name='Name']").val(),
                    Summary: $(langObject).find("[lang-name='Summary']").val()
                };
                banner.BannerLanguage.push(langEntity);
            }
        }
    }


}

function initializeClearingPageScript() {
    $(".clearing_page #StoreId").change(function () {
        var request = createRequest();
        request.type = REQUEST_TYPE_GET;
        request.url = base_admin_url + "/clearing/GetStoreClearing?storeId=" + $(this).val();
        request.success = function (result) {
            $(".clearing_page #Price").val(result.Price);
            $(".clearing_page #ToAccount").val(result.ToAccount);
        };
        $.ajax(request);
    });
}

function initializeStoreProductScript() {
    $(".tr-column .change-status").click(function () {

        var storeProductId = $(this).parents(".tr-column").children(".td-store-productid").attr("store-productid");
        var ProductName = $(this).parents(".tr-column").children(".td-product-name").attr("product-name");
        var statusId = $(this).parents(".tr-column").children(".td-statusid").attr("statusid");
        $("#modal-change-status").find(".store-productid").val(storeProductId);
        $("#modal-change-status").find(".product-name").html(ProductName);
        $("#modal-change-status").find(".statusId").val(statusId);
        $("#modal-change-status").modal("show");
    });

    $("#modal-change-status .btn-submit").click(function () {
        var storeProductId = $("#modal-change-status").find(".store-productid").val();
        var statusId = $("#modal-change-status").find(".statusId").val();
        var request = createRequest();
        request.type = REQUEST_TYPE_GET;
        request.url = base_admin_url + "/storeproduct/ChangeStatus?storeProductId=" + storeProductId + "&statusId=" + statusId;
        request.success = function (result) {
            location.reload();
        };
        $.ajax(request);
    });


}



$(document).on('change', ".rowReview input", function () {

    var $t = $(this);
    var fileUpload = $t.get(0);
    var files = fileUpload.files;
    var data = new FormData();
    for (var i = 0; i < files.length; i++) {
        data.append(files[i].name, files[i]);
    }
    var request = createRequest();
    request.url = base_admin_url + "/Upload/UploadPhoto";
    request.type = "POST";
    request.data = data,
        request.contentType = false;
    request.processData = false;
    request.beforeSend = function () {
        $t.val("");
        $("#modalLoading").modal("show");
    }
    request.error = function () {
        createMessage(MESSAGE_TYPE_ERROR, "خطا", "خطا در هنگام آپلود تصویر");
    };
    request.success = function (entity) {
        entityobj = entity;
        var panelUrl = $("#PanelUrl").val();
        var url = entity.Url;
        url = url.replace("SYSTEM_TYPE_PANEL", panelUrl);
        $t.parent().parent(".rowReview").find(".reviewImg").attr("src", url);
        $t.parent().parent(".rowReview").find(".imgBox").removeClass("d-none");
        $t.parent().parent(".rowReview").find(".review-picture-item").addClass("d-none");
        $t.parent().parent(".rowReview").find(".delete-review-picture").removeClass("d-none");
        $t.parent().parent(".rowReview").find(".pictureReviewId").val(entity.Id);
        $("#modalLoading").modal("hide");
    };
    $.ajax(request);
});


function addReview() {
    var revHtml = "<div class='rowReview d-flex mt-3'><input type='hidden' class='pictureReviewId' value='123' />";
    revHtml += "<textarea class='reviewContent w-100 m-2' placeholder='توضیحات' rows='3'></textarea>";
    revHtml += "<div class='imgBox d-none'><img class='reviewImg m-2' />";
    revHtml += "<label class='delete-review-picture d-none m-2' >حذف تصویر</label></div><div class='review-picture-item m-2'><input class='upoladFile ' type='file' /></div>";
    revHtml += "<label class='remove-review-item m-2'><i class='fa fa-close' aria-hidden='true'></i></label></div>";
    $(".reviewBox").prepend(revHtml);
}


$(document).on('click', ".delete-review-picture", function () {
    $(this).parent().parent(".rowReview").find(".review-picture-item").removeClass("d-none");
    $(this).parent(".imgBox").addClass("d-none");
    //$(this).parent(".imgBox").find(".delete-review-picture").addClass("d-none");
    $(this).parent(".imgBox").find(".reviewImg").attr("src", "");
    $(this).parent().parent(".rowReview").find(".pictureReviewId").val("");

});
$(document).on('click', ".remove-review-item", function () {
    $(this).parent('.rowReview').remove();

});
$("#CategoryId").change(function () {

    var request = createRequest();
    var selectedCategoryId = $(this).children("option:selected").val();
    request.type = REQUEST_TYPE_GET;
    request.url = base_admin_url + "/store/ProductSubCategory/PriorityList?categoryId=" + selectedCategoryId;
    request.success = function (result) {

        $("#divprority").html(result);
    }
    $.ajax(request);
});
$("#TypeId").change(function () {

    var request = createRequest();
    var selectedTypeId = $(this).children("option:selected").val();
    request.type = REQUEST_TYPE_GET;
    request.url = base_admin_url + "/store/ProductCategory/PriorityList?typeId=" + selectedTypeId;
    request.success = function (result) {
        $("#divprority").html(result);
    }
    $.ajax(request);
});

function UpdateTagSubcategory(subcategoryId) {

    var request = createRequest();
    //var subcategoryId = $("#subcategoryId").val();

    request.type = REQUEST_TYPE_GET;
    request.url = base_admin_url + "/store/ProductSubCategory/GetTags?id=" + subcategoryId;

    request.success = function (result) {

        $("#divAllTag").html(result);
    }
    $.ajax(request);
}
function SubmitTage(subcategoryId) {

    var request = createRequest();
    var tageName = $("#tagText").val();
    request.type = REQUEST_TYPE_POST;
    request.url = base_admin_url + "/store/ProductSubCategory/AddTage?tageName=" + tageName;
    request.success = function (result) {

        if (result.Type == "2") {
            UpdateTagSubcategory(subcategoryId);
            $("#tagText").val("");
        }
        else {
            alert(result.Body);
        }

    }
    $.ajax(request);
}
function UpdateTagPost(postId) {

    var request = createRequest();
    //var subcategoryId = $("#subcategoryId").val();

    request.type = REQUEST_TYPE_GET;
    request.url = base_admin_url + "/content/Post/GetTags?id=" + postId;

    request.success = function (result) {

        $("#divAllTag").html(result);
    }
    $.ajax(request);
}


function initializeNewGalleryScript() {
    //$(".banner_page .category_id").change(function () {
    //    var request = createRequest();
    //    request.type = REQUEST_TYPE_GET;
    //    request.url = base_admin_url + "/content/category/GetCategoryPicture?CategoryId=" + $(this).val();
    //    request.success = function (entity) {
    //        $(".banner_page img.category_img").attr("src", entity);
    //    }
    //    $.ajax(request);
    //})



    var Gallery = {};
    $("#btnSubmit").click(function () {
        fillEntity();
        $.ajax(createRequest(Gallery));
    });
    function fillEntity() {
        debugger;
        Gallery.ID = $("#ID").val();
        Gallery.CategoryId = $("#CategoryId").val();
        Gallery.WebsiteId = $("#WebsiteId").val();
        Gallery.ShowNumber = $("#ShowNumber").val();
        Gallery.Name = $("#Name").val();
        Gallery.CreateDateTime = $("#CreateDateTime").val();
        Gallery.GalleryLanguage = [];
        var langValues = $("[lang-value]");
        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    LanguageId: parseInt($(langObject).find("[lang-name='LanguageId']").val()),
                    Name: $(langObject).find("[lang-name='Name']").val(),
                };
                Gallery.GalleryLanguage.push(langEntity);
            }
        }
    }
}

function initializeNewSurveyScript() {
    var survey = {};
    $("#btnSubmit").click(function () {
        fillEntity();
        $.ajax(createRequest(survey));
    });
    function fillEntity() {
        survey.ID = $("#ID").val();
        survey.DateTime = $("#DateTime").val();
        survey.Name = $("#Name").val();
        survey.Label = $("#Label").val();
        survey.SurveyLanguage = [];
        var langValues = $("[lang-value]");
        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    LanguageId: parseInt($(langObject).find("[lang-name='LanguageId']").val()),
                    Name: $(langObject).find("[lang-name='Name']").val(),
                };
                survey.SurveyLanguage.push(langEntity);
            }
        }
    }
}

function initializeSurveyQuestionScript() {
    var entity = {};
    $("#btnSubmit").click(function () {

        fillEntity();
        $.ajax(createRequest(entity));
    });

    function fillEntity() {
        entity.ID = $("#ID").val();
        entity.SurveyId = getSelectedValue("#SurveyId");
        entity.QuestionType = getSelectedValue("#QuestionType");
        entity.Question = $("#Question").val();
        entity.ShowNumber = $("#ShowNumber").val();
        entity.Active = $("#Active").val();

        entity.SurveyQuestionLanguage = [];
        //entity.Active = $('#Active').prop('checked');

        var langValues = $("[lang-value]");
        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    Question: $(langObject).find("[lang-name='Question']").val(),
                    LanguageId: parseInt($(langObject).find("[lang-name='LanguageId']").val()),

                };
                entity.SurveyQuestionLanguage.push(langEntity);
            }
        }
    }
}

function initializeSurveyQuestionItemScript() {
    var entity = {};
    $("#btnSubmit").click(function () {

        fillEntity();
        $.ajax(createRequest(entity));
    });

    function fillEntity() {
        entity.ID = $("#ID").val();
        entity.QuestionId = $("#QuestionId").val();
        /*getSelectedValue("#QuestionId");*/
        entity.Name = $("#Name").val();
        entity.ShowNumber = $("#ShowNumber").val();
        entity.Active = $("#Active").val();

        entity.SurveyQuestionItemLanguage = [];
        //entity.Active = $('#Active').prop('checked');

        var langValues = $("[lang-value]");
        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    Name: $(langObject).find("[lang-name='Name']").val(),
                    LanguageId: parseInt($(langObject).find("[lang-name='LanguageId']").val()),

                };
                entity.SurveyQuestionItemLanguage.push(langEntity);
            }
        }
    }
}
$(document).on('change', ".rowVideo input", function () {
    var $t = $(this);
    var fileUpload = $t.get(0);
    var files = fileUpload.files;
    var data = new FormData();
    for (var i = 0; i < files.length; i++) {
        data.append(files[i].name, files[i]);

    }

    var request = createRequest();
    request.url = base_admin_url + "/Upload/UploadPhoto";
    request.type = "POST";
    request.data = data,
        request.contentType = false;
    request.processData = false;
    request.beforeSend = function () {
        $t.val("");
        $("#modalLoading").modal("show");
    }
    request.error = function () {
        createMessage(MESSAGE_TYPE_ERROR, "خطا", "خطا در هنگام آپلود تصویر");
    };
    request.success = function (entity) {
        entityobj = entity;
        var panelUrl = $("#PanelUrl").val();
        var url = entity.Url;
        url = url.replace("SYSTEM_TYPE_PANEL", panelUrl);
        $t.parent().parent(".rowVideo").find(".VideoImg").attr("src", url);
        $t.parent().parent(".rowVideo").find(".imgBox").removeClass("d-none");
        $t.parent().parent(".rowVideo").find(".Video-picture-item").addClass("d-none");
        $t.parent().parent(".rowVideo").find(".delete-Video-picture").removeClass("d-none");
        $t.parent().parent(".rowVideo").find(".pictureVideoId").val(entity.Id);
        $("#modalLoading").modal("hide");
    };
    $.ajax(request);
});

function addVideo() {
    var revHtml = "<div class='rowVideo d-flex mt-3'><input type='hidden' class='pictureVideoId' value='' />";
    revHtml += "<input  class='VideoContent w-100 m-2' placeholder='لینک ویدئو' type='text' />";
    revHtml += "<div class='imgBox d-none'><img class='VideoImg m-2' />";
    revHtml += "<label class='delete-Video-picture d-none m-2' >حذف تصویر</label></div><div class='Video-picture-item m-2'><input class='upoladFile ' type='file' /></div>";
    revHtml += "<label class='remove-Video-item m-2'><i class='fa fa-close' aria-hidden='true'></i></label></div>";
    $(".VideoBox").prepend(revHtml);
}


$(document).on('click', ".delete-Video-picture", function () {
    $(this).parent().parent(".rowVideo").find(".Video-picture-item").removeClass("d-n+one");
    $(this).parent(".imgBox").addClass("d-none");
    //$(this).parent(".imgBox").find(".delete-Video-picture").addClass("d-none");
    $(this).parent(".imgBox").find(".VideoImg").attr("src", "");
    $(this).parent().parent(".rowVideo").find(".pictureVideoId").val("");

});
$(document).on('click', ".remove-Video-item", function () {
    $(this).parent('.rowVideo').remove();

});


function initializeCodeprocessUploadScript() {



    $("#uplFile").dropify({
        messages: {
            'default': 'فایل را اینجا بیندازید',
            'replace': 'برای تغییر تصویر کلیک کنید',
            'remove': 'حذف',
            'error': 'خطا در هنگام ارسال تصویر'
        },
        error: {
            'fileSize': 'The file size is too big (1M max).'
        }
    });
    $("#uplFile").change(function () {
        var files = document.getElementById("uplFile").files;
        var filesCount = files.length;
        if (files.length < 1)
            return;
        if (filesCount + viewModel.File.FileIds().length > 20)
            filesCount = 20 - viewModel.File.FileIds().length;
        var uploadedCount = 0;
        ajaxLoader(true);
        for (var i = 0; i < filesCount; i++) {
            var formData = new FormData();
            formData.append("files", files[i]);
            $.ajax({
                type: "POST",
                url: "/cdn/file/upload",
                data: formData,
                contentType: false,
                processData: false,
                success: function (response) {
                    if (response.status == "success") {
                        viewModel.File.FileIds.push(response);
                        console.log(response);
                        uploadedCount = uploadedCount + 1;
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                }
            });
        }
        ajaxLoader(false);
    });
}
function removeMedia(mediaId) {
    for (var i = 0; i < viewModel.File.FileIds().length; i++) {
        if (viewModel.File.FileIds()[i].FileId == mediaId) {
            viewModel.File.FileIds.splice(i, 1);
            createMessage('success', 'عملیات با موفقیت انجام شد');
            return;
        }
    }

}
function FillAccountFile(Id) {
    var request = createRequest();
    request.type = apirequest.type.Get;
    request.url = base_admin_url + "/crm/account/GetFile?accountId=" + Id;
    request.success = function (result) {
        if (result !== "Error") {
            console.log(result);
            viewModel.File.FileIds(result);
            //GetCurrentLocation();
        }
    };
    $.ajax(request);
}
function updateFile(Id) {

    var request = createRequest();
    request.type = REQUEST_TYPE_POST;
    request.data = JSON.stringify({
        accountId: Id,
        FileIds: viewModel.File.FileIds(),
        description: $("#DescriptionFile").val()
    });
    request.url = base_admin_url + "/crm/account/UpdateFileNew";/*?fileId=" + fileId + "&orderId=" + Id + "&voiceId=" + voiceId*/
    request.success = function (result) {
        if (result.Type == "2") {
            createMessage(MESSAGE_TYPE_SUCCESS);
            //createMessage(MESSAGE_TYPE_SUCCESS);
            setTimeout(function () {

                window.location.replace(base_admin_url + "/crm/account");
            }, 1000);

        }

    };

    $.ajax(request);

}

function initializeNewCityScript() {

    var city = {};
    $("#btnSubmit").click(function () {
        fillEntity();
        $.ajax(createRequest(city));
    });
    function fillEntity() {
        city.ID = $("#ID").val();
        city.StateId = $("#StateId").val();

        city.Name = $("#Name").val();

        city.CityLanguage = [];
        var langValues = $("[lang-value]");
        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    LanguageId: parseInt($(langObject).find("[lang-name='LanguageId']").val()),
                    Name: $(langObject).find("[lang-name='Name']").val(),

                };
                city.CityLanguage.push(langEntity);
            }
        }
    }
}
function initializeNewStateScript() {

    var state = {};
    $("#btnSubmit").click(function () {
        fillEntity();
        $.ajax(createRequest(state));
    });
    function fillEntity() {
        state.ID = $("#ID").val();


        state.Name = $("#Name").val();
        state.CountryId = $("#CountryId").val();


        state.StateLanguage = [];
        var langValues = $("[lang-value]");
        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    LanguageId: parseInt($(langObject).find("[lang-name='LanguageId']").val()),
                    Name: $(langObject).find("[lang-name='Name']").val(),

                };
                state.StateLanguage.push(langEntity);
            }
        }
    }
}
function initializeProductBrandScript() {
    var entity = {};
    $("#btnSubmit").click(function () {

        fillEntity();
        $.ajax(createRequest(entity));
    });

    function fillEntity() {
        entity.Label = $("#Label").val();
        entity.ID = $("#ID").val();
        entity.Name = $("#Name").val();
        entity.Description = tinymce.get('Description').getContent();
        entity.Url = $("#Url").val();
        entity.H1 = $("#H1").val();
        entity.ProductTypeId = $("#ProductTypeId").val();
        entity.Summary = $("#Summary").val();
        entity.NoIndex = $('#NoIndex').prop('checked');
        entity.NoFollow = $('#NoFollow').prop('checked');
        entity.Canonical = $("#Canonical").val();
        entity.ShowNumber = $("#ShowNumber").val();
        entity.Title = $("#Title").val();
        entity.IsPublic = $('#IsPublic').prop('checked');
        entity.Active = $('#Active').prop('checked');
        //entity.PictureId = uploadPictureId;
        entity.PictureId = uploadPictureId == null ? $("#LastPictureId").val() : uploadPictureId;
        entity.ProductBrandLanguage = [];
        var langValues = $("[lang-value]");

        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    Name: $(langObject).find("[lang-name='Name']").val(),
                    LanguageId: parseInt($(langObject).find("[lang-name='LanguageId']").val()),
                    Description: tinymce.get($(langObject).find("[lang-name='Description']").attr('id')).getContent(),
                    Title: $(langObject).find("[lang-name='Title']").val()
                };
                entity.ProductBrandLanguage.push(langEntity);
            }
        }

    }
}

function initializeQuestionSmartOffer() {
    var entity = {};
    $("#btnSubmit").click(function () {

        fillEntity();
        $.ajax(createRequest(entity));
    });

    function fillEntity() {
        entity.Text = $("#Text").val();
        entity.ID = $("#ID").val();
        entity.ShowNumber = $("#ShowNumber").val();

        entity.Active = $('#Active').val();

        entity.QuestionSmartOfferLanguage = [];
        var langValues = $("[lang-value]");

        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    Text: $(langObject).find("[lang-name='Text']").val(),

                };
                entity.QuestionSmartOfferLanguage.push(langEntity);
            }
        }

    }
}
