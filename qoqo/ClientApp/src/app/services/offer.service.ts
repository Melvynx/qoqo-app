import { EventEmitter, Injectable } from '@angular/core';
import { Offer } from 'src/types/offer';
import { client } from '../../utils/client';
import { ClickState } from '../../types/click';

@Injectable({
  providedIn: 'platform',
})
export class OfferService {
  offerEvent = new EventEmitter();
  offer?: Offer = undefined;

  constructor() {
    client<Offer>('offers/current')
      .then((offer) => {
        this.offer = offer;
        client<ClickState>(`clicks/offers/${offer.id}`)
          .then((result) => {
            this.offerEvent.emit(result);
          })
          .catch((err) => {
            console.error('err', err);
          });
      })
      .catch((err) => {
        console.error(err);
      });
  }
}
