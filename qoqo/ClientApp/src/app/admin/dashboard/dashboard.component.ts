import { Component } from '@angular/core';
import { Dashboard } from '../../../types/offer';
import { client } from '../../../utils/client';
import { ClickHubService } from '../../services/click-hub.service';
import { Click, ClickFinishResult } from '../../../types/click';
import { ClientMessage } from '../../../types/api';
import { SnackbarService } from '../../services/snackbar.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
})
export class DashboardComponent {
  dashboard?: Dashboard;

  constructor(
    private clickHubService: ClickHubService,
    private snackbar: SnackbarService
  ) {
    client<Dashboard>('offers/dashboard')
      .then((dashboard) => {
        this.dashboard = dashboard;
      })
      .catch((err) => {
        console.error(err);
      });

    this.clickHubService.hubConnection.on('CLICK', (data) => {
      const click: Click = JSON.parse(data);
      if (!this.dashboard) return;
      if (click.clickCount !== 0) {
        this.dashboard.clickCount = click.clickCount;
      }
    });
    this.clickHubService.hubConnection.on('FINISH', (data) => {
      const finishInformation: ClickFinishResult = JSON.parse(data);
      if (!this.dashboard) return;
      this.dashboard.clickCount = finishInformation.clickCount;
      this.dashboard.isOver = true;
    });
  }

  getPercentage() {
    if (!this.dashboard?.clickCount || !this.dashboard?.clickObjective)
      return 0;
    return (this.dashboard?.clickCount * 100) / this.dashboard?.clickObjective;
  }

  safeNumber(value?: number) {
    return value || 0;
  }

  increaseClick() {
    client<ClientMessage & { offerId?: number }>(
      `offers/${this.dashboard?.offerId}/increase_click`,
      {
        method: 'PATCH',
      }
    )
      .then((res) => {
        if (this.dashboard?.clickObjective) {
          this.dashboard.clickObjective = 1 + this.dashboard.clickObjective;
        }
        this.snackbar.openMessage(res.message);
      })
      .catch((err) => {
        this.snackbar.openError(err.message);
      });
  }

  endTheOffer() {
    client<ClientMessage>(`offers/${this.dashboard?.offerId}/end_offer`, {
      method: 'PATCH',
    })
      .then((res) => {
        this.snackbar.openMessage(res.message);
      })
      .catch((err) => {
        this.snackbar.openError(err.message);
      });
  }

  getLocaleDate(date?: string) {
    if (!date) return 'unknown';
    return new Date(date).toLocaleDateString();
  }
}
