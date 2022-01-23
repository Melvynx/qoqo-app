import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { client } from '../../../../utils/client';
import { EmptyOffer, Offer } from '../../../../types/offer';
import { FormBuilder } from '@angular/forms';
import { ClientMessage } from '../../../../types/api';
import { SnackbarService } from '../../../services/snackbar.service';

@Component({
  selector: 'app-crud',
  templateUrl: './offer-crud.component.html',
  styles: [
    `
      .prices > * {
        width: 31%;
      }
    `,
  ],
})
export class OfferCrudComponent implements OnInit {
  @ViewChild('form') form?: ElementRef<HTMLFormElement>;

  offerId?: string | null;
  offer?: Offer = EmptyOffer;
  offerForm = this.formBuilder.group(EmptyOffer);

  constructor(
    private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    private snackbar: SnackbarService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe((paramMap) => {
      this.offerId = paramMap.get('id');
      this.getOffer();
    });
  }

  isNewOffer() {
    return this.offerId === 'new';
  }

  onSubmit() {
    const method = this.isNewOffer() ? 'POST' : 'PUT';
    const url = 'offers' + (this.isNewOffer() ? '' : '/' + this.offerId);

    client<ClientMessage & { offerId?: number }>(url, {
      method,
      data: this.offerForm.value,
    })
      .then((res) => {
        if (res.offerId) {
          this.router.navigateByUrl(`/admin/offers/${res.offerId}`);
          this.snackbar.openMessage('Created successfully');
          return;
        }
        this.snackbar.openMessage(res.message);
      })
      .catch((err) => {
        this.snackbar.openError(err.message);
      });
  }

  setTodayDate(event: Event) {
    event.preventDefault();

    const today = new Date();
    const msInOneDay = 60 * 60 * 24 * 1000;

    this.offerForm.setValue({
      ...this.offerForm.value,
      startAt: today,
      endAt: new Date(today.getTime() + msInOneDay),
    });
  }

  private getOffer() {
    if (this.isNewOffer()) {
      return;
    }
    client<Offer>(`offers/${this.offerId}`)
      .then((offer) => {
        this.offerForm = this.formBuilder.group(offer);
      })
      .catch((err) => {
        this.snackbar.openError(err.message);
        this.router.navigateByUrl('/admin/offers');
      });
  }
}
