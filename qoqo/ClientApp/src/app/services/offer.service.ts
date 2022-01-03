import { EventEmitter, Injectable } from '@angular/core';
import { Offer } from 'src/types/offer';
import { client } from '../../utils/client';
import { ClickState } from '../../types/click';
import {ActivatedRoute, Router, Routes} from '@angular/router';

@Injectable({
  providedIn: 'platform',
})
export class OfferService {
  offerEvent = new EventEmitter();
  offer?: Offer = undefined;
  offerId?: string | null;

  constructor(private activatedRoute: ActivatedRoute, private router: Router) {
    this.activatedRoute.paramMap.subscribe((paramMap) => {
      this.offerId = paramMap.get('id') || 'current';

      client<Offer>(`offers/${this.offerId}`)
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
          router.navigateByUrl('/');
        });
    });
  }
}
