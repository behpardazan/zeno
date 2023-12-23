$(function () {

    $('.image-editor').cropit({
        imageState: {
            src: '1.jpg',
        },
    });
    $('.rotate-cw').click(function () {
        $('.image-editor').cropit('rotateCW');
    });
    $('.rotate-ccw').click(function () {
        $('.image-editor').cropit('rotateCCW');
    });
    $('.export').click(function () {
        var imageData = $('.image-editor').cropit('export');
        uploadBase65(imageData);
    });
    function dataURLtoFile(dataurl) {
        var filename = null;
        if (dataurl.indexOf("image/png") > -1) {
            filename = "imagefile.png";
        }
        else if (dataurl.indexOf("image/jpg") > -1) {
            filename = "imagefile.jpg";
        }
        else if (dataurl.indexOf("image/jpeg") > -1) {
            filename = "imagefile.jpeg";
        }
        else if (dataurl.indexOf("image/bmp") > -1) {
            filename = "imagefile.bmp";
        }
        else {
            return null;
        }
        var arr = dataurl.split(','),
            mime = arr[0].match(/:(.*?);/)[1],
            bstr = atob(arr[1]),
            n = bstr.length,
            u8arr = new Uint8Array(n);

        while (n--) {
            u8arr[n] = bstr.charCodeAt(n);
        }

        return new File([u8arr], filename, { type: mime });
    }
    function uploadBase65(imageData) {
        ;
        var file = dataURLtoFile(imageData);
        if (file != null) {
            var data = new FormData();
            data.append('file', file);

            var request = createRequest();
            request.url = "/panel/Upload/UploadPhoto";
            request.type = "POST";
            request.data = data,
                request.contentType = false;
            request.processData = false;
            request.success = function (entity) {
                var url = entity.Url.replace("SYSTEM_TYPE_PANEL", "");
                $("#profileImg").attr("src", url);
                UploadPicture(entity.Id);
                $(".modal").modal("hide");
            };
            $.ajax(request);
        }
        else {
            createMessage("erorr");
        }
    }
})