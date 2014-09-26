(function ($) {
    $.fn.PreviewImage = function (options) {
        var Default = $.extend({}, $.fn.PreviewImage.defaults, options);
        function init() {
            var divContainer = document.getElementById(Default.DivPreviewId);
            divContainer.style.width = Default.MaxWidth + "px";
            divContainer.style.height = Default.MaxHeight + "px";
            divContainer.style.border = "solid 1px #d2e2e2";
            divContainer.style.backgroundColor = "#ccc";
            divContainer.style.display = "block";
        }
        return this.each(function () {
            if (Default.ImageClientId != "") {
                init(); //初始化
                $(this).unbind("change");
                $(this).change(function () {
                    var imgPreviewId = Default.ImageClientId;
                    var divPreviewId = Default.DivPreviewId;
                    var fileObj = this;
                    var allowExtention = Default.AllowExtention;//允许上传文件的后缀名document.getElementById("hfAllowPicSuffix").value;  
                    var extention = fileObj.value.substring(fileObj.value.lastIndexOf(".") + 1).toLowerCase();
                    var browserVersion = window.navigator.userAgent.toUpperCase();
                    if (allowExtention.indexOf(extention) > -1) {
                        var imgobj = document.getElementById(imgPreviewId);
                        if (fileObj.files) {//HTML5实现预览，兼容chrome、火狐7+等  
                            if (window.FileReader) {
                                var reader = new FileReader();
                                reader.onload = function (e) {
                                    imgobj.setAttribute("src", e.target.result);
                                    imgobj.style.display = "inline";
                                }
                                reader.readAsDataURL(fileObj.files[0]);
                            } else if (browserVersion.indexOf("SAFARI") > -1) {
                                alert("不支持Safari6.0以下浏览器的图片预览!");
                            }
                        } else if (browserVersion.indexOf("MSIE") > -1) {
                            if (browserVersion.indexOf("MSIE 6") > -1) {//ie6  
                                imgobj.setAttribute("src", fileObj.value);
                                imgobj.style.display = "inline";
                            } else {//ie[7-9]  
                                fileObj.select();
                                if (browserVersion.indexOf("MSIE 9") > -1)
                                    fileObj.blur();//不加上document.selection.createRange().text在ie9会拒绝访问  
                                var newPreview = document.getElementById(divPreviewId + "New");
                                if (newPreview == null) {
                                    newPreview = document.createElement("div");
                                    newPreview.setAttribute("id", divPreviewId + "New");
                                    newPreview.style.width = Default.MaxWidth + "px";
                                    newPreview.style.height = Default.MaxHeight + "px";
                                    newPreview.style.border = "solid 1px #d2e2e2";
                                }
                                newPreview.style.filter = "progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod='scale',src='" + document.selection.createRange().text + "')";
                                var tempDivPreview = document.getElementById(divPreviewId);
                                tempDivPreview.parentNode.insertBefore(newPreview, tempDivPreview);
                                tempDivPreview.style.display = "none";
                            }
                        } else if (browserVersion.indexOf("FIREFOX") > -1) {//firefox  
                            var firefoxVersion = parseFloat(browserVersion.toLowerCase().match(/firefox\/([\d.]+)/)[1]);
                            if (firefoxVersion < 7) {//firefox7以下版本  
                                imgobj.setAttribute("src", fileObj.files[0].getAsDataURL());
                                imgobj.style.display = "inline";
                            } else {//firefox7.0+                      
                                imgobj.setAttribute("src", window.URL.createObjectURL(fileObj.files[0]));
                                imgobj.style.display = "inline";
                            }
                        } else {
                            imgobj.setAttribute("src", fileObj.value);
                            imgobj.style.display = "inline";
                        }
                    } else {
                        alert("仅支持" + allowExtention + "为后缀名的文件!");
                        fileObj.value = "";//清空选中文件  
                        if (browserVersion.indexOf("MSIE") > -1) {
                            fileObj.select();
                            document.selection.clear();
                        }
                        fileObj.outerHTML = fileObj.outerHTML;
                    }
                });
                $("#" + Default.ImageClientId).load(function () {
                    var image = new Image();
                    image.src = $(this).attr("src");
                    $(this).attr("width", Default.MaxWidth);
                    $(this).attr("height", Default.MaxHeight);
                    $(this).attr("alt", Default.MaxWidth + "x" + Default.MaxHeight);
                });
            }
        });
    };
    $.fn.PreviewImage.defaults = {
        ImageClientId: "",
        MaxWidth: 300,
        MaxHeight: 300,
        DivPreviewId: "",
        AllowExtention: ".jpg,.bmp,.gif,.png"
    };
})(jQuery);