import { Component } from '@angular/core';
import { Dashboard } from '../../../types/offer';
import { client } from '../../../utils/client';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
})
export class DashboardComponent {
  dashboard?: Dashboard;

  constructor() {
    client<Dashboard>('offers/dashboard')
      .then((dashboard) => {
        this.dashboard = dashboard;
      })
      .catch((err) => {
        console.error(err);
      });
  }

  getPercentage() {
    if (!this.dashboard) return 0;
    return (this.dashboard?.clickCount * 100) / this.dashboard?.clickObjective;
  }
}
