// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

//const { start } = require("@popperjs/core");

var interval = null;
function StartTimer() {
    if (interval == null) {
        var counter = 0;
        interval = setInterval(function () {
            counter += 1;
            document.getElementById("lblCount").innerHTML = counter;
        }, 1000);
    }
}

function StopTimer() {
    clearInterval(interval);
    interval = null;
    document.getElementById("lblCount").innerHTML = counter;
}

window.onload = function (e) {
    StartTimer();
}

// Write your JavaScript code.
$(function () {
    console.log("Page is ready");

    $(document).bind("contextmenu", function (event) {
        event.preventDefault();
    })

    $(document).on("mousedown", ".cell", function (event) {
        switch (event.which) {
            case 1:
                event.preventDefault();
                var btn = $(this).val();
                console.log("Left clicked");
                UpdateButton(btn, "/Game/ShowOneButton");
                break;
            case 2:
                console.log("Middle mouse button clicked.");
                break;
            case 3:
                event.preventDefault();
                var btn = $(this).val();
                console.log("Left clicked");
                UpdateButton(btn, "/Game/RightClick");
                break;


        }
    })


    function UpdateButton(rowcol, url) {
        $.ajax({
            type: 'json',
            method: 'POST',
            url: url,
            data: {
                "rowcol": rowcol
            },
            success: function (data) {
                console.log(data.part4);
                $(".game-board").html(data.part1);
                $(".messageArea").html(data.part2);
                $(".player-stats").html(data.part3);
                if (data.part4 === "1") {
                    StopTimer();
                }
            }
        })
    }
})