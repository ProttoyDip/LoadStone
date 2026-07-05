// SignalR client for peer support chat.
const chatConnection = new signalR.HubConnectionBuilder()
    .withUrl('/hubs/peer-chat')
    .withAutomaticReconnect()
    .build();

chatConnection.on('ReceiveMessage', (user, message) => {
    console.log(`${user}: ${message}`);
});

chatConnection.start().catch(err => console.error(err));
