import { Component } from '@angular/core';
import { OfferService } from '../services/offer.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  providers: [OfferService],
})
export class HomeComponent {
  _offerService: OfferService;
  constructor(offerService: OfferService) {
    this._offerService = offerService;
  }
}
