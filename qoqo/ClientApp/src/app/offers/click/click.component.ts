import {Component, OnInit, ViewChild} from '@angular/core';
import {MatSnackBar} from '@angular/material/snack-bar';
import {AuthService} from 'src/app/services/auth.service';
import {OfferService} from 'src/app/services/offer.service';
import {Click, ClickFinishResult, ClickState} from 'src/types/click';
import {client} from 'src/utils/client';
import {ClickButtonComponent} from '../click-button/click-button.component';
import {ClickHubService} from '../../services/click-hub.service';

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

  constructor(
    public offerService: OfferService,
    public clickHubService: ClickHubService,
    private _matSnackBar: MatSnackBar,
    private _authService: AuthService
  ) {
    this.sentence = this.offerService.offer?.winnerText;
    this.offerService.offerEvent.subscribe(
      ({ click, userId, remainingTime }: ClickState) => {
        this.clickCounter = click;
        if (userId) {
          this.handleFinishVariant(userId, true);
        } else {
          this.setRemainingTime(remainingTime);
        }
      }
    );
  }

  ngOnInit(): void {
    this.clickHubService.hubConnection.on('CLICK', (data) => {
      const click: Click = JSON.parse(data);
      this.handleNewClick(click);
    });
    this.clickHubService.hubConnection.on('FINISH', (data) => {
      const finishInformation: ClickFinishResult = JSON.parse(data);
      this.handleFinish(finishInformation);
    });
  }

  handleFinish(finishInformation: ClickFinishResult) {
    this.sentence = finishInformation.finishSentence;
    this.clickCounter = finishInformation.clickCount;
    this.handleFinishVariant(finishInformation.userId);
  }

  handleFinishVariant(userId: number, isSave = false) {
    const newVariant =
      userId === this._authService.user?.userId ? 'wine' : 'lose';
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
    if (click.user.id === this._authService.user?.userId) {
      this.setRemainingTime(10);
    }
    this.clickCounter = click.clickCount;
  }

  handleClick() {
    client<{ confetti: boolean }>(
      `clicks/offers/${this.offerService.offer?.offerId}`,
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
