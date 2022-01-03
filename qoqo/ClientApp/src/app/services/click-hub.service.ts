import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { getBaseUrl } from '../../main';

@Injectable({
  providedIn: 'root',
})
export class ClickHubService {
  hubConnection: HubConnection;

  constructor() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${getBaseUrl()}offerHub`)
      .build();

    this.hubConnection
      .start()
      .then(() => {
        console.info('[Offer Click Socket] Connection started');
      })
      .catch((err) => {
        console.error(err);
      });
  }
}
