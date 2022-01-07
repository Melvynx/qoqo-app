import { Component } from '@angular/core';
import { ClickList } from 'src/types/click';
import { client } from 'src/utils/client';

@Component({
  selector: 'profile-click',
  templateUrl: './click-profile.component.html',
})
export class ClickProfileComponent {
  clicks: ClickList = [];

  constructor() {
    client<ClickList>('clicks').then((clicks) => {
      this.clicks = clicks;
    });
  }
}
