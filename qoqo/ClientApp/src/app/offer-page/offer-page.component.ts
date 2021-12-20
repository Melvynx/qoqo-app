import { Component, Inject, OnInit } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-offer-page',
  templateUrl: './offer-page.component.html',
  styleUrls: ['./offer-page.component.css'],
})
export class OfferPageComponent implements OnInit {
  private _hubConnection: HubConnection;
  private _http: HttpClient;
  private _baseUrl: string;
  public messages: string[] = [];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this._http = http;
    this._baseUrl = baseUrl;
    console.log('Tring to start the hub connection');
    this._hubConnection = new HubConnectionBuilder()
      .withUrl(`${baseUrl}offerHub`)
      .build();
  }

  ngOnInit() {
    console.log('Init SignaR pute');
    this._hubConnection
      .start()
      .then(() => console.log('Je suis une salope a beurre'))
      .catch((err) => console.error(err.toString()));

    this._hubConnection.on('ReceiveMessage', (message) => {
      this.handleHubConnectionMessage(message);
    });
  }

  handleHubConnectionMessage(message: string) {
    this.messages.push(message);
  }

  onSendButtonClick(): void {
    this._http.get(`${this._baseUrl}api/clicks`).subscribe((v) => {
      console.log('v', v);
    });
  }
}
