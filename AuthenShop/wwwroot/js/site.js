// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
// Write your JavaScript code.

//Ajax for Login
$(function () {
    let form = $("#account");
    form.on('submit', function (e) {
        e.preventDefault();
        let returnURL = location.search;
        let url = location.origin + '/Identity/Account/Login' + returnURL;
        let data = form.serialize();
        $.ajax({
            type: "POST",
            url: url,
            data: data,
            success: function (res, text, xhr) {
                if (xhr.status === 200) {
                    if (xhr.responseText === "Invalid") {
                        Swal.fire({
                            icon: 'error',
                            title: 'Sorry',
                            text: 'Email or password is incorrect',
                        });
                    } if (xhr.responseText.includes("LoginWith2fa")) {
                        $(location).attr('href', location.origin + xhr.responseText);
                    } if (xhr.responseText.startsWith("Succeeded")) {
                        $(location).attr('href', location.origin + xhr.responseText.slice(9));
                    } if (xhr.responseText.includes("Lockout")) {
                        Swal.fire({
                            icon: 'error',
                            title: 'Sorry',
                            text: 'Co cai lon',
                            scrollbarPadding: false,
                        });
                    }
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    });
});

$(function () {
    let form = $("#resend-email");
    form.on('submit', function (e) {
        e.preventDefault();
        let url = location.href;
        let data = form.serialize();
        $.ajax({
            type: "POST",
            url: url,
            data: data,
            success: function (res, text, xhr) {
                if (xhr.status === 200) {
                    if (xhr.responseText.includes("Email does not exist")) {
                        Swal.fire({
                            icon: 'error',
                            title: 'Sorry',
                            text: 'Email does not exist. Please register your account.',
                            scrollbarPadding: false,
                        });
                    } if (xhr.responseText.includes("Verification email sent")) {
                        Swal.fire({
                            icon: 'success',
                            title: 'Successful',
                            text: 'Verification email sent. Please check your email.',
                            scrollbarPadding: false,
                        });
                    }
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    });
});

/*Google Button*/
$("#external-account button[value = 'Google']").attr('class', 'btn btn-block text-white').css("background-color", "#dd5044");

/*Custom Clothes Price*/
$(document).ready(function () {
    console.log($('.price').text().slice(1, 3));
})