// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {

    $(".dateType").each(function () {
        console.log($(this).html()); 
      let m =  moment($(this).html()).fromNow();
        $(this).html(m)
    })

});