$(document).ready(function () {
    $("body").on('keyup', function (e) {
        if (e.keyCode === 13) {
            UserLogin();
        }
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
                alert("Bạn cần nhập đủ Tên đăng nhập và Mật khẩu!");
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
                        alert("Tên đăng nhập hoặc Mật khẩu không đúng!");
                        HideLoadingScreen();
                        return;
                    }
                },
                error: function (response) {
                    alert(response);
                }
            });

        } catch (err) {
            HideLoadingScreen();
            alert(err);
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
                    alert(response);
                }
            });

        } catch (err) {
            HideLoadingScreen();
            alert(err);
        }
    }
});