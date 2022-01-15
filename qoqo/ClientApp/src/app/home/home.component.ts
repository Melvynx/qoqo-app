import { Component } from '@angular/core';
import { OfferService } from '../services/offer.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styles: [
    `
      .cover-image {
        width: 100%;
        max-height: 800px;
        object-fit: contain;
        border-radius: 4px;
      }
      .shadow-medium {
        box-shadow: var(--shadow-elevation-medium);
      }
    `,
  ],
  providers: [OfferService],
})
export class HomeComponent {
  _offerService: OfferService;

  constructor(offerService: OfferService) {
    this._offerService = offerService;
  }
}
