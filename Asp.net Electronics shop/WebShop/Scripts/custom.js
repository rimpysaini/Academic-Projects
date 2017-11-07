$(window).scroll(function () {

    if ($(this).scrollTop() > 0) {
        $('.search').fadeOut();
    }
    else {
        if ($(this).width() > 750) {
            $('.search').fadeIn();
        }
    }
});
$(window).resize(function () {

    if ($(this).width() < 750) {
        $('#myCarousel').hide();
        $('#serachWidget').hide();
        
    } else {
        $('#myCarousel').show();
        $('#serachWidget').show();
       
    }

});

$(function () {
    // codeproject.com/Tips/826002/Bootstrap-Modal-Dialog-Loading-Content-from-MVC-Pa
    $('body').on('click', '.modal-link', function (e) {
        e.preventDefault();
        $(this).attr('data-target', '#myModalLogin');
        $(this).attr('data-toggle', 'modal');
    });
    // Attach listener to .modal-close-btn's so that when the button is pressed the modal dialog disappears
    $('body').on('click', '.modal-close-btn', function () {
        $('#myModalLogin').modal('hide');
    });
    //clear modal cache, so that new content can be loaded
    $('#myModalLogin').on('hidden.bs.modal', function () {
        $(this).removeData('bs.modal');
    });
    $('#CancelModal').on('click', function () {
        return false;
    });
});
$(function () {
    $('#approve-btn').click(function () {
        $('#myModalLogin').modal('hide');
    });
});


// drop down manager
$('ul.nav li.dropdown').hover(function() {
  $(this).find('.dropdown-menu').stop(true, true).delay(200).fadeIn(500);
}, function() {
  $(this).find('.dropdown-menu').stop(true, true).delay(200).fadeOut(500);
    });
$('.dropdown-toggle').click(function () {
    $(this).css("background-color", "#343434");
});

$(document).ready(
    function ShowSuccess() {   
        $('#categoryUpdateSuccessModal').modal('show');
});
