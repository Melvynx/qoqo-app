# QoQo

## To run the application

```bash
cd qoqo
dotnet run
```


## Setup up DB for dev

```bash
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet ef database update
```

To create a migration for the database:
```bash
dotnet ef migrations add SomeTableChange
```

## Websocket

### Backend

To call websocket, inject `IHubContext<OfferHub> hubContext` dependency in the controller.

Then you can call all listener with:

```c#
await _hubContext.Clients.All.SendAsync("ReceiveMessage", "Hello from the server");
```

### Fronted

To get the websocket connection, you need to add this in TypeScript:

```ts
 const hubConnection = new HubConnectionBuilder()
      .withUrl(`${baseUrl}offerHub`)
      .build();

hubConnection.start();

// The `ReceiveMessage` correspond to the name of the method in SendAsync
hubConnection.on('ReceiveMessage', (message) => {
    // Whatever you want to do with the message
});
```