 
$(document).ready(function () {
    $("#contact-form").submit(function (e) {
        e.preventDefault(); //prevent normal form submission
            
            var actionUrl = $(this).attr("action")
            $.ajax({
                type: "POST",
                url: actionUrl,
                data: $(this).serialize(),
                success: function (obj) {
                    $("#Name").val("");
                    $("#Email").val("");
                    $("#EmailBody").val("");  
                },
                error: function (err) {
                    alert(err);
                }
            })

        })
    })

     