import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { AuthService } from 'src/app/services/auth.service';
import { OfferService } from 'src/app/services/offer.service';
import { getBaseUrl } from 'src/main';
import { Click, ClickFinishResult } from 'src/types/click';
import { client } from 'src/utils/client';
import { ClickButtonComponent } from '../click-button/click-button.component';

@Component({
  selector: 'app-click',
  templateUrl: './click.component.html',
  styleUrls: ['./click.component.css'],
})
export class ClickComponent implements OnInit {
  @ViewChild('clickButton') clickButton?: ClickButtonComponent;
  variant: ClickComponentVariant = 'enabled';

  sentence?: string = '';

  clickCounter = 0;
  remainingTime = 0;
  loading = true;
  _hubConnection: HubConnection;

  constructor(
    public offerService: OfferService,
    private _matSnackBar: MatSnackBar,
    private _authService: AuthService
  ) {
    this.sentence = this.offerService.offer?.winnerText;
    this._hubConnection = new HubConnectionBuilder()
      .withUrl(`${getBaseUrl()}offerHub`)
      .build();
  }

  ngOnInit(): void {
    this._hubConnection
      .start()
      .then(() => {
        console.log('Connected....');
        client<{ click: number; remainingTime: number; userId: number }>(
          `clicks/offers/${this.offerService.offer?.id}`
        )
          .then(({ click, remainingTime, userId }) => {
            this.clickCounter = click;
            if (userId) {
              this.handleFinishVariant(userId, true);
            } else {
              this.setRemainingTime(remainingTime);
            }
          })
          .catch((err) => {
            console.error('err', err);
          });
      })
      .catch((err) => console.error(err.toString()));

    this._hubConnection.on('CLICK', (data) => {
      const click: Click = JSON.parse(data);
      this.handleNewClick(click);
    });
    this._hubConnection.on('FINISH', (data) => {
      const finishInformations: ClickFinishResult = JSON.parse(data);
      this.handleFinish(finishInformations);
    });
  }

  handleFinish(finishInformations: ClickFinishResult) {
    this.sentence = finishInformations.finishSentence;
    this.clickCounter = finishInformations.clickCount;
    this.handleFinishVariant(finishInformations.userId);
  }

  handleFinishVariant(userId: number, isSave = false) {
    console.log({ userId, x: this._authService.user?.id });
    const newVariant = userId === this._authService.user?.id ? 'wine' : 'lose';
    if (newVariant === 'wine' && !isSave) {
      setTimeout(() => {
        this.variant = newVariant;
      }, 2500);
    } else {
      this.variant = newVariant;
    }
  }

  decreaseRemainingTime() {
    this.remainingTime--;
    if (this.remainingTime === 0 && this.variant === 'disabled') {
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
    if (click.user.id === this._authService.user?.id) {
      this.setRemainingTime(10);
    }
    this.clickCounter = click.clickCount;
  }

  setClickCounter(click: number) {
    if (this.loading) {
      this.loading = false;
    }
    this.clickCounter = click;
  }

  handleClick() {
    client<{ confetti: boolean }>(
      `clicks/offers/${this.offerService.offer?.id}`,
      {
        method: 'POST',
      }
    )
      .then(({ confetti }) => {
        confetti && this.clickButton?.runConfetti();
      })
      .catch(({ message }) => {
        this._matSnackBar.open(message, 'Close', { duration: 5000 });
      });
  }
}

type ClickComponentVariant = 'disabled' | 'enabled' | 'wine' | 'lose';
