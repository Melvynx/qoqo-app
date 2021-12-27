import { Component } from '@angular/core';
import { ClickList } from 'src/types/click';
import { client } from 'src/utils/client';

@Component({
  selector: 'app-auth-click',
  templateUrl: './auth-click.component.html',
})
export class AuthClickComponent {
  clicks: ClickList = [];

  constructor() {
    client<ClickList>('clicks').then((clicks) => {
      console.log('clicks', clicks);
      this.clicks = [...clicks, ...clicks, ...clicks, ...clicks, ...clicks];
    });
  }
}
