"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
var userchatselected = "";
var chathistory = "";

var chatBox = document.getElementById("txtChatBox");
if (!!chatBox) {
    chatBox.disabled = true;
    chatBox.addEventListener("keypress", function (event) {
        if (event.keyCode === 13) {
            var user = localStorage.getItem(userchatselected);
            var message = chatBox.value;
            sendMessage(user, message);

            chatBox.value = "";
            event.preventDefault();
        }
    });
}

connection.on("ReceiveMessage", function (message) {
    appendChatMessage(message);

    var userSelected = localStorage.getItem(userchatselected);
    bindMessageToList(message, userSelected);
    if (userSelected === message.from) {
        markMessagesRead(userSelected);
    }
    checkNewMessage((message.from));
});

connection.on("OnlineMessage", function (userName) {
    var statusElement = '#status-' + userName;
    if (!!$(statusElement)) {
        $(statusElement).removeClass('vd_bg-grey');
        $(statusElement).addClass('vd_bg-green');
    }
});

connection.on("OfflineMessage", function (userName) {
    var statusElement = '#status-' + userName;
    if (!!$(statusElement)) {
        $(statusElement).removeClass('vd_bg-green');
        $(statusElement).addClass('vd_bg-grey');
    }
});

connection.on('OnConnectedSuccess', (userName) => {
    localStorage.setItem('loggedinUser', userName);
    userchatselected = userName + '-selectedUserChat';
    chathistory = userName + '-chathistory';
    checkNewMessageForAllUsers();
    sendMessage(userName, 'getunreadmessage');

    if (!!localStorage.getItem(userchatselected)) {
        selectUserChat(localStorage.getItem(userchatselected));
    }
});

connection.on('UnreadMessages', (messages) => {
    appendChatMessages(messages);
    checkNewMessageForAllUsers();
    sendMessage(localStorage.getItem('loggedinUser'), 'deleteunreadmessage');
});

connection.start().then(function (a) {
    // document.getElementById("txtChatBox").readOnly = false;
    //localStorage.getItem('loggedinUser') 
}).catch(function (err) {
    return console.error(err.toString());
});

const selectUserChat = (userName) => {
    if (!userName)
        return;

    var selectionTag = '.list-wrapper #' + userName;
    $('.list-wrapper li').removeClass('active');
    $(selectionTag).addClass('active');

    //set selected user for chating to localStorage
    localStorage.setItem(userchatselected, userName);

    openChatWindow(userName);
}

const openChatWindow = (userName) => {
    if (!!document.getElementById('selectedUserChat'))
        document.getElementById('selectedUserChat').innerText = userName;
    else
        return;
    chatBox.disabled = false;
    chatBox.value = "";
    loadMessages(userName);
    markMessagesRead(userName);
    checkNewMessage(userName);
}

const appendChatMessage = (message) => {
    var messages = [];
    if (localStorage.getItem(chathistory)) {
        messages = JSON.parse(localStorage.getItem(chathistory));
    }
    messages.push(message);
    saveMessageToLocalStorage(messages);
}

const appendChatMessages = (messages) => {
    var localMessages = [];
    if (localStorage.getItem(chathistory)) {
        localMessages = JSON.parse(localStorage.getItem(chathistory));
    }
    messages.forEach(message => localMessages.push(message));
    saveMessageToLocalStorage(localMessages);
}

const saveMessageToLocalStorage = (messages) => {
    localStorage.setItem(chathistory, JSON.stringify(messages));
}

const loadMessages = (userName) => {
    $('#lstChatDetail > li').remove();
    var messages = getMessagesFromUser(userName);

    if (messages.length > 0) {

        messages.forEach(m => {
            bindMessageToList(m, userName);
        });
    } else {
        bindEmptyItemToChatWindow();
    }
}

const bindMessageToList = (m, userName) => {
    var isTextRight = m.from !== userName ? 'text-right' : '';
    var owner = m.from !== userName ? 'you' : userName;
    var src = "../images/user-icon.png";
    if (m.from !== userName) {
        if ($('input#loggedInUserChat').val() === 'True')
            src = '../images/useravatar_' + m.from + '.png';
    } else {
        if ($('li#' + m.from).attr('hasAvatar') === 'True')
            src = '../images/useravatar_' + userName + '.png';
    }    

    var element = `
          <li class=${isTextRight}>
            <a href="#">
                <div class="menu-icon">
                    <img alt="example image"
                            src="${src}" class="rounded-circle" width="32" height="32">
                </div>
                <div class="menu-text">
                    ${m.message}
                    <div class="menu-info">
                        <span class="menu-date">${owner} </span>
                    </div>
                </div>
            </a>
        </li>
        `;

    $('#lstChatDetail').append(element);
    scrollToBottom('chatScrollWindow');
    
}

const bindEmptyItemToChatWindow = () => {
    var element = `
          <li>
            <a href="#">            
                <div class="menu-text ml-0">
                    There are no messages yet....
                </div>
            </a>
        </li>
        `;

    $('#lstChatDetail').append(element);
}

const scrollToBottom = (id) => {
    var element = document.getElementById(id);
    element.scrollTop = element.scrollHeight - element.clientHeight;
}

const getMessagesFromUser = (userName) => {
    var messages = [];
    if (!!localStorage.getItem(chathistory)) {
        messages = JSON.parse(localStorage.getItem(chathistory));
    }
    return messages.filter(x => x.from === userName || x.to === userName);
}

const checkNewMessage = (userName) => {
    var messageNotiferTag = $('#messageNotifier-' + userName);
    if (!!getMessagesFromUser(userName).find(x => !x.isRead && x.from === userName)) {
        if (!!messageNotiferTag) {
            messageNotiferTag.show();
        }
    } else {
        if (!!messageNotiferTag) {
            messageNotiferTag.hide();
        }
    }
}

const markMessagesRead = (userName) => {
    var messages = getMessagesFromUser(userName).filter(x => !x.isRead && x.from === userName);
    if (messages.length > 0) {
        var localMessages = JSON.parse(localStorage.getItem(chathistory));
        localMessages = localMessages.map(x => {
            x.isRead = (x.from === userName ? true : x.isRead);
            return x;
        });
        saveMessageToLocalStorage(localMessages);
    }
}

const checkNewMessageForAllUsers = () => {
    $('#chatList li').each((index, value) => { checkNewMessage(value.id); });
}

const sendMessage = (user, message) => {
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
}
