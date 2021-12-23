import { Component, Input, OnInit } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { OfferService } from 'src/app/services/offer.service';
import { getBaseUrl } from 'src/main';
import { Click } from 'src/types/click';
import { client } from 'src/utils/client';

@Component({
  selector: 'app-click',
  templateUrl: './click.component.html',
  styleUrls: ['./click.component.css'],
})
export class ClickComponent implements OnInit {
  @Input() variant: ClickComponentVariant = 'enabled';

  sentence =
    'Avec un score de 22 click, @JeanMichel a fait le 722ème click. Il a donc fait 3% des clicks total!';

  clickCounter = 0;
  loading = true;
  _offerService: OfferService;
  _hubConnection: HubConnection;

  constructor(offerService: OfferService) {
    this._offerService = offerService;
    this._hubConnection = new HubConnectionBuilder()
      .withUrl(`${getBaseUrl()}offerHub`)
      .build();
  }

  ngOnInit(): void {
    this._hubConnection
      .start()
      .then(() => {
        console.log('Connected....');
        client<{ click: number }>(
          `clicks/offers/${this._offerService.offer?.id}`
        )
          .then(({ click }) => {
            this.clickCounter = click;
            console.log({ click });
          })
          .catch((err) => {
            console.error('err', err);
          });
      })
      .catch((err) => console.error(err.toString()));

    this._hubConnection.on('CLICK', (data) => {
      const click: Click = JSON.parse(data);
      console.log('CLICK', click);
      this.handleNewClick(click);
    });
  }

  handleNewClick(click: Click) {
    this.clickCounter = click.clickCount;
  }

  setClickCounter(click: number) {
    if (this.loading) {
      this.loading = false;
    }
    this.clickCounter = click;
  }

  handleClick() {
    client(`clicks/offers/${this._offerService.offer?.id}`, {
      json: false,
      method: 'POST',
    }).then(() => {
      console.log('OK');
    });
  }
}

type ClickComponentVariant = 'disabled' | 'enabled' | 'wine' | 'lose';
