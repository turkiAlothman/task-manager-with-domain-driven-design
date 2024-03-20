(Dropzone.autoDiscover = !1),
(function ($) {
	'use strict';
	
	jQuery(document).ready(function () {
        
        $("#dpz-single-file").dropzone({
            paramName: "file", 
            maxFiles: 1,
            addRemoveLinks: !0, 
            dictRemoveFile: "<i class='bi bi-x-circle'></i>",
            url: "/file/post"
        });
        $("#dpz-multiple-files").dropzone({
            url:"#",
            paramName: "file",
            maxFiles: 6,
            autoProcessQueue: true,
            acceptedFiles: '.jpg, .jpeg, .png, .pdf, .docx, .txt',
            maxFilesize: 5000000,
            clickable: !0,
            addRemoveLinks: !0,
            dictRemoveFile: "<i class='bi bi-x-circle'></i>",
            init: function () {
               $(this).on("queuecomplete", function (file) {
                    alert("All files have uploaded ");
                });
            }

        });
        $("#dpz-btn-select-files").dropzone({
            clickable: "#select-files",
            addRemoveLinks: !0, 
            dictRemoveFile: "<i class='bi bi-x-circle'></i>",
            url: "/file/post"
        });
        $("#dpz-file-limits").dropzone({
            paramName: "file", 
            maxFilesize: 1000, 
            maxFiles: 5, 
            maxThumbnailFilesize: 1,
            addRemoveLinks: !0, 
            dictRemoveFile: "<i class='bi bi-x-circle'></i>",
            url: "/file/post"
        });
        $("#dpz-accept-files").dropzone({
            paramName: "file", 
            maxFilesize: 1000, 
            acceptedFiles: "image/*" ,
            addRemoveLinks: !0, 
            dictRemoveFile: "<i class='bi bi-x-circle'></i>",
            url: "/file/post"
        });
        $("#dpz-remove-thumb").dropzone({
            paramName: "file", 
            maxFilesize: 1000, 
            addRemoveLinks: !0, 
            dictRemoveFile: "<i class='bi bi-x-circle'></i>",
            url: "/file/post"
        });
        $("#dpz-remove-all-thumb").dropzone({
            paramName: "file",
            maxFilesize: 1000,
            addRemoveLinks: !0, 
            dictRemoveFile: "<i class='bi bi-x-circle'></i>",
            url: "/file/post",
            init: function () {
                var e = this;
                $("#clear-dropzone").on("click", function () {
                    e.removeAllFiles();
                });
            }, 
        });
    });

})(jQuery);