import { Component, OnInit } from '@angular/core';
import { Offer } from '../../../types/offer';
import { client } from '../../../utils/client';

@Component({
  selector: 'app-offers',
  templateUrl: './offers.component.html',
  styleUrls: ['./offers.component.css'],
})
export class OffersComponent {
  offers: Offer[] = [];

  constructor() {
    client<Offer[]>('offers').then((offers) => {
      this.offers = offers;
      console.log(offers)
    });
  }

  getLocaleDate(date: string) {
    return new Date(date).toLocaleString();
  }
}
