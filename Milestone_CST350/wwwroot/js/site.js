// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
    console.log("Page is ready");
    /*
    $(document).on("click", ".cell", function (event) {
        event.preventDefault();

        var rowcol = $(this).val();
        
        UpdateButton(rowcol);
        //console.log("button row+col: " + rowcol);
    })*/
    $(document).bind("contextmenu", function (event) {
        event.preventDefault();
    })

    $(document).on("mousedown", ".cell", function (event) {
        switch (event.which) {
            case 1:
                var btn = $(this).val();
                console.log("Left clicked");
                event.preventDefault();
                UpdateButton(btn, "/Game/ShowOneButton");
                break;
            case 2:
                console.log("Middle mouse button clicked.");
                break;
            case 3:
                var btn = $(this).val();
                console.log("Left clicked");
                event.preventDefault();
                UpdateButton(btn, "/Game/RightClick");
                break;


        }

        //UpdateButton(rowcol);
        //console.log("button row+col: " + rowcol);
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
                console.log(data);
                $(".game-board").html(data.part1);
                $(".messageArea").html(data.part2);
                $(".player-stats").html(data.part3);
            }
        })
    }
})