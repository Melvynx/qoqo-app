import { Component, OnInit, ViewChild } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { OfferService } from 'src/app/services/offer.service';
import { Click, ClickFinishResult, ClickState } from 'src/types/click';
import { client } from 'src/utils/client';
import { ClickButtonComponent } from '../click-button/click-button.component';
import { ClickHubService } from '../../services/click-hub.service';
import { SnackbarService } from '../../services/snackbar.service';
import { getLocalStorage, setLocalStorage } from '../../../utils/localStorage';

const Sounds = {
  click: new Audio('assets/sounds/click.mp3'),
  ready: new Audio('assets/sounds/ready.mp3'),
  soundon: new Audio('assets/sounds/soundon.wav'),
};

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
  isSoundEnable = getLocalStorage('isSoundEnable', true);

  get isOn() {
    return this.variant === 'enabled' || this.variant === 'disabled';
  }

  constructor(
    public offerService: OfferService,
    public clickHubService: ClickHubService,
    private snackbar: SnackbarService,
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
      this.playSound('ready');
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
    if (
      click.clickObjective !== this.offerService.offer?.clickObjective &&
      this.offerService.offer
    ) {
      this.offerService.offer.clickObjective = click.clickObjective;
    }
    if (click.clickCount !== 0) {
      this.clickCounter = click.clickCount;
    }
  }

  handleClick() {
    this.playSound('click');
    client<{ confetti: boolean }>(
      `clicks/offers/${this.offerService.offer?.offerId}`,
      {
        method: 'POST',
      }
    )
      .then(({ confetti }) => {
        confetti && this.clickButton?.runConfetti();
        this.setRemainingTime(10);
      })
      .catch(({ message }) => {
        this.snackbar.openMessage(message);
      });
  }

  playSound(name: keyof typeof Sounds) {
    if (!this.isSoundEnable) {
      return;
    }
    Sounds[name].volume = 0.5;
    Sounds[name].play().then(() => console.log('PLAYED'));
  }

  handleSound() {
    this.isSoundEnable = !this.isSoundEnable;
    this.playSound('soundon');
    setLocalStorage('isSoundEnable', this.isSoundEnable);
  }

  get soundIcon() {
    return `assets/icon/sound-${this.isSoundEnable ? 'on' : 'off'}.svg`;
  }
}

type ClickComponentVariant = 'disabled' | 'enabled' | 'wine' | 'lose';
