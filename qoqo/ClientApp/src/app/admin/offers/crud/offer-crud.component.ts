import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { client } from '../../../../utils/client';
import { EmptyOffer, Offer } from '../../../../types/offer';
import { FormBuilder } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-crud',
  templateUrl: './offer-crud.component.html',
  styleUrls: ['./offer-crud.component.css'],
})
export class OfferCrudComponent implements OnInit {
  @ViewChild('form') form?: ElementRef<HTMLFormElement>;

  offerId?: string | null;
  offer?: Offer = EmptyOffer;
  offerForm = this.formBuilder.group(EmptyOffer);

  constructor(
    private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    private matSnackBar: MatSnackBar,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe((paramMap) => {
      this.offerId = paramMap.get('id');
      this.getOffer();
    });
  }

  private getOffer() {
    if (this.isNewOffer()) {
      return;
    }
    client<Offer>(`offers/${this.offerId}`).then((offer) => {
      this.offerForm = this.formBuilder.group(offer);
    }).catch(err => {
      this.matSnackBar.open(err.message, 'OK', { duration: 5000 });
      this.router.navigateByUrl('/admin/offers');
    });
  }

  isNewOffer() {
    return this.offerId === 'new';
  }

  onSubmit() {
    const method = this.isNewOffer() ? 'POST' : 'PATCH';
    const url = 'offers' + (this.isNewOffer() ? '' : '/' + this.offerId);

    client<{ message?: string; offerId?: number }>(url, {
      method,
      data: this.offerForm.value,
    })
      .then((res) => {
        if (res.offerId) {
          this.router.navigateByUrl(`/admin/offers/${res.offerId}`);
          this.matSnackBar.open("Created successfully", 'OK', { duration: 2000 });
          return;
        }
        this.matSnackBar.open(res.message || '', 'OK', { duration: 2000 });
      })
      .catch((err) => {
        this.matSnackBar.open(err.message, 'OK', { duration: 10000 });
      });
  }
}
