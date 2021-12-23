import { Injectable } from '@angular/core';
import { Offer } from 'src/types/offer';
import { client } from '../../utils/client';

@Injectable({
  providedIn: 'platform',
})
export class OfferService {
  offer?: Offer = undefined;

  constructor() {
    client<Offer>('offers/current')
      .then((offer) => {
        this.offer = offer;
        console.log(this.offer);
      })
      .catch((err) => {
        console.error(err);
      });
  }
}
