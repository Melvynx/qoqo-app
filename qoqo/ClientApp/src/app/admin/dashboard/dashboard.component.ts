import { Component } from '@angular/core';
import { Dashboard } from '../../../types/offer';
import { client } from '../../../utils/client';
import { ClickHubService } from '../../services/click-hub.service';
import { Click, ClickFinishResult } from '../../../types/click';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
})
export class DashboardComponent {
  dashboard?: Dashboard;

  constructor(private clickHubService: ClickHubService) {
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
      this.dashboard.clickCount = click.clickCount;
    });
    this.clickHubService.hubConnection.on('FINISH', (data) => {
      const finishInformation: ClickFinishResult = JSON.parse(data);
      if (!this.dashboard) return;
      this.dashboard.clickCount = finishInformation.clickCount;
    });
  }

  getPercentage() {
    if (!this.dashboard?.clickCount || !this.dashboard?.clickObjective)
      return 0;
    return (this.dashboard?.clickCount * 100) / this.dashboard?.clickObjective;
  }
}
