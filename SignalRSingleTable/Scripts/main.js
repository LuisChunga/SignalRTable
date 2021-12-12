
////// GET NOTIFICATIONS AND SETUP SIGNALR

////// Click on notification icon to show notification
////$('span.noti').click(function (e) {
////    e.stopPropagation();
////    $('.noti-content').show();
////    var count = 0;
////    count = parseInt($('span.count').html()) || 0;
////    // Only load notification if not already loaded
////    if (count > 0) {
////        updateNotification();
////    }
////    $('span.count', this).html('&nbsp;');
////});

////// Hide notifications
////$('html').click(function () {
////    $('.noti-content').hide();
////});

////// Update notifications
////function updateNotification() {
////    $('#notiContent').empty();
////    $('#notiContent').append($('<li>Loading...</li>'));
////    $.ajax({
////        type: 'GET',
////        url: '/home/GetNotificationContacts',
////        success: function (response) {
////            $('#notiContent').empty();
////            if (response.length === 0) {
////                $('#notiContent').append($('<li>No data available</li>'));
////            }
////            $.each(response, function (index, value) {
////                $('#notiContent').append($('<li>New contact : ' + value.ContactName + ' (' + value.ContctNo + ') added</li>'));
////            });
////        },
////        error: function (error) {
////            console.log(error);
////        }
////    });
////}

////// Update notifications count
////function updateNotificationCount() {
////    var count = 0;
////    count = parseInt($('span.count').html()) || 0;
////    count++;
////    $('span.count').html(count);
////}

////// SignalR js code for start hub and send receive notification(s) from the server side
////var notificationHub = $.connection.myhub;
////$.connection.hub.start().done(function () {
////    console.log('Notification hub started');
////});

////// SignalR method for push server message to client
////notificationHub.client.notify = function (message) {
////    if (message && message.toLowerCase() === "added") {
////        updateNotificationCount();
////    }
////};



$(function () {
    // Declare a proxy to reference the hub.
    var notifications = $.connection.myHub;  // <-- myhub is declare in my Hubs folder

    //debugger;
    // Create a function that the hub can call to broadcast messages.
    notifications.client.updateMessages = function () {
        getAllMessages()

    };
    // Start the connection.
    $.connection.hub.start().done(function () {
        //alert("connection started")
        getAllMessages();
    }).fail(function (e) {
        alert(e);
    });
});




function getAllMessages() {
    var tbl = $('#messagesTable');
    $.ajax({
        url: '/home/GetMessages',           //<-- The function is in HomeController
        contentType: 'application/html ; charset:utf-8',
        type: 'GET',
        dataType: 'html',
        success: function (result) {
            tbl.empty().append(result);
        }

    });
}