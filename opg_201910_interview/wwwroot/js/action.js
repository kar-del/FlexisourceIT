$(document).ready(function () {
    $("#btnSubmit").click(function () {
        if ($("#clients").val() == "") {
            $("#clients").focus();
            alert("Please select a client");
        } else {
            var _client = $("#clients").val();
            $.ajax({
                type: "POST",
                url: "/Action/ClientList",
                async: true,
                data: { "strClient": _client },
                traditional: true,
                success: function (data) {
                    alert(data);
                }
            });
        }
    });
});