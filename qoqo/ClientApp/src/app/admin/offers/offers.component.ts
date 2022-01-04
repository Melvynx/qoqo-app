import { Component } from '@angular/core';
import { Offer } from '../../../types/offer';
import { client } from '../../../utils/client';

@Component({
  selector: 'app-offers',
  templateUrl: './offers.component.html',
})
export class OffersComponent {
  offers: Offer[] = [];

  constructor() {
    client<Offer[]>('offers').then((offers) => {
      this.offers = offers;
    });
  }

  getLocaleDate(date: string) {
    return new Date(date).toLocaleString();
  }
}
