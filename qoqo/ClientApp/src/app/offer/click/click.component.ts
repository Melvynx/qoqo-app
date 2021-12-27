import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
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
  variant: ClickComponentVariant = 'enabled';

  sentence =
    'Avec un score de 22 click, @JeanMichel a fait le 722ème click. Il a donc fait 3% des clicks total!';

  clickCounter = 0;
  remainingTime = 0;
  loading = true;
  _hubConnection: HubConnection;

  constructor(
    private _offerService: OfferService,
    private _matSnackBar: MatSnackBar
  ) {
    this._hubConnection = new HubConnectionBuilder()
      .withUrl(`${getBaseUrl()}offerHub`)
      .build();
  }

  ngOnInit(): void {
    this._hubConnection
      .start()
      .then(() => {
        console.log('Connected....');
        client<{ click: number; remainingTime: number }>(
          `clicks/offers/${this._offerService.offer?.id}`
        )
          .then(({ click, remainingTime }) => {
            this.clickCounter = click;
            this.setRemainingTime(remainingTime);
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

  decreaseRemainingTime() {
    this.remainingTime--;
    if (this.remainingTime === 0) {
      this.variant = 'enabled';
    } else {
      setTimeout(() => {
        this.decreaseRemainingTime();
      }, 1000);
    }
  }

  setRemainingTime(time: number) {
    this.remainingTime = time || 0;
    if (this.remainingTime > 0) {
      this.variant = 'disabled';
      setTimeout(() => this.decreaseRemainingTime(), 1000);
    }
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
      method: 'POST',
    })
      .then(() => {
        this.setRemainingTime(10);
      })
      .catch(({ message }) => {
        this._matSnackBar.open(message, 'Close', { duration: 5000 });
      });
  }
}

type ClickComponentVariant = 'disabled' | 'enabled' | 'wine' | 'lose';
