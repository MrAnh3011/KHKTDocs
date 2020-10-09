$(document).ready(function () {
    $("body").on('keyup', function (e) {
        if (e.keyCode === 13) {
            UserLogin();
        }
    });

    $("#btnLogin").click(function () {
        UserLogin();
    });

    function ShowLoadingScreen() {
        $(".loading-screen").css({ "display": "block" });
    }

    function HideLoadingScreen() {
        $(".loading-screen").css({ "display": "none" });
    }

    function UserLogin() {
        try {
            ShowLoadingScreen()
            var userName = $('#userName').val();
            var password = $('#password').val();
            if (userName === '' || password === '') {
                Swal.fire("Chú ý", "Bạn cần nhập đủ Tên đăng nhập và Mật khẩu!", "warning");
                HideLoadingScreen();
                return;
            }
            $.ajax({
                url: "http://api.apec.com.vn/session/login",
                data: JSON.stringify({
                    username: userName,
                    password: password
                }),
                dataType: "json",
                type: "POST",
                contentType: 'application/json',
                success: function (response) {
                    if (response.status == 'Ok') {
                        var rs = response.result;
                        var loginModel = {
                            SessionKey: rs.SessionKey,
                            Username: rs.Username,
                            DisplayName: rs.DisplayName,
                            AllowDevelop: rs.AllowDevelop,
                            AllowViewAllData: rs.AllowViewAllData
                        }
                        PostUserSession(loginModel);
                        HideLoadingScreen();
                    } else {
                        Swal.fire("Lỗi", "Tên đăng nhập hoặc mật khẩu không đúng: " + response.message, "error");
                        HideLoadingScreen();
                        return;
                    }
                },
                error: function (e) {
                    Swal.fire("Lỗi", "Vui lòng kiểm tra lại: " + e, "error");
                }
            });

        } catch (err) {
            HideLoadingScreen();
            Swal.fire("Lỗi", "Vui lòng kiểm tra lại: " + err, "error");
        }
    }

    function PostUserSession(loginModel) {
        try {
            ShowLoadingScreen();
            $.ajax({
                url: "/PostUserSession",
                data: loginModel,
                dataType: "json",
                type: "POST",
                success: function (response) {
                    if (response.status == "success") {
                        window.location.replace("/Home/Index");
                    }
                },
                error: function (response) {
                    HideLoadingScreen();
                    Swal.fire("Lỗi", "Vui lòng kiểm tra lại: " + response, "error");
                }
            });

        } catch (err) {
            HideLoadingScreen();
            Swal.fire("Lỗi", "Vui lòng kiểm tra lại: " + err, "error");
        }
    }
});