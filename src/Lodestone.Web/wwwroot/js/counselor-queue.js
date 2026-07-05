// SignalR client for the counselor risk queue.
const queueConnection = new signalR.HubConnectionBuilder()
    .withUrl('/hubs/counselor-queue')
    .withAutomaticReconnect()
    .build();

queueConnection.on('QueueUpdated', payload => {
    console.log('Queue updated', payload);
});

queueConnection.start().catch(err => console.error(err));
