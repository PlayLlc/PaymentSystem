 
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
                    // Modal
                    $("#myModal #myModalLabel").html("Email Successfully Sent!")
                    $("#myModal #modalBody").html("Your email has been sent to the Play team and we will respond to you shortly");
                    $('#myModal').modal('toggle');
                    $('#myModal').modal('show'); 
                },
                error: function (err) {
                    $("#myModalLabel").html("Email Was Unsuccessful")
                    $("#modalBody").html("Please review your email details and ensure that everything is correct");
                    $('#myModal').modal('toggle');
                    $('#myModal').modal('show'); 
                }
            })

        })
    })

     