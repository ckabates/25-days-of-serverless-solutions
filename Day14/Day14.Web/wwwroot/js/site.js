let chatUser = {
    handle: ""
};

const connection = new signalR.HubConnectionBuilder()
    .withUrl(`${apiBaseUrl}/api`)
    .configureLogging(signalR.LogLevel.Information)
    .build();

connection.start()
    .then((response) => {
        $("#txtHandle").removeAttr('disabled');
        $("#btnChat").removeAttr('disabled');
    })
    .catch(() => {
        redirectToError();
    });

connection.on('newMessage', newMessage);

function newMessage(message) {
    let newMessage = `<li><div class="float-left"><img title="${message.sender}" src="https://api.adorable.io/avatars/16/${message.sender}.png" class="userIcon rounded" /></div><div style="display:flex"><p> ${message.text}<br/> <span class="messageDate">${moment.utc(message.dateTime).local().format('MMMM Do YYYY, h:mm:ss a')}</span></p> </div></li>`;
    $('#discussion').append(newMessage);
    $("#discussion").animate({ scrollTop: $("#discussion")[0].scrollHeight }, 100);
}

function sendMessage() {
    var msg = $('#txtMessage').val();
    if (msg.length > 0) {
        createMessage(chatUser.handle, msg);
    }
    $('#txtMessage').val('');
}

function createMessage(sender, messageText) {
    $.ajax({
        type: "POST",
        url: `${apiBaseUrl}/api/messages`,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            sender: sender,
            text: messageText,
            dateTime: moment().valueOf()
        }),
        success: (resp) => {
            // Extra actions on a successful post.
        },
        error: (XMLHttpRequest, textStatus, errorThrown) => {
            redirectToError();
        } 
    });
}

function setHandle() {
    setChatWindowHeight();
    chatUser.handle = $('#txtHandle').val();
    $('img.logo').attr('src', `https://api.adorable.io/avatars/32/${chatUser.handle}.png`);
    $('img.logo').attr('title', chatUser.handle);
    $('#spanHandle').text(chatUser.handle);
    $('#welcome').toggleClass('d-none');
    $('#chatWindow').toggleClass('d-none');
    $('header').toggleClass('d-none');
    $('body').toggleClass('text-center');
    $('#holder').toggleClass('w-100');
    $('main').toggleClass('cover');
}

function setChatWindowHeight() {
    $("#discussion").height($(window).height() - 210);
}

function redirectToError() {
    location.href = "error";
}

$(window).resize(function () {
    setChatWindowHeight();
});

$("#txtMessage").keypress(function (event) {
    if (event.which == 13) {
        sendMessage();
        event.preventDefault();
    }
});