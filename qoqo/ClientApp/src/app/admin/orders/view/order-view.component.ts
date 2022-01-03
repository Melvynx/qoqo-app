import { Component } from '@angular/core';
import { client } from '../../../../utils/client';
import { Order, OrderStatusKeys } from '../../../../types/order';
import { ActivatedRoute } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-view',
  templateUrl: './order-view.component.html',
  styleUrls: ['./order-view.component.css'],
})
export class OrderViewComponent {
  orderId?: string | null;
  order?: Order;

  constructor(
    private route: ActivatedRoute,
    private matSnackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe((paramMap) => {
      this.orderId = paramMap.get('id') || '';
      this.getOrder();
    });
  }

  getOrder() {
    client<Order>(`orders/${this.orderId}`).then((order) => {
      this.order = order;
    });
  }

  setStatus(status: OrderStatusKeys) {
    client<{ message: string }>(`orders/${this.orderId}`, {
      method: 'PATCH',
      data: { status },
    })
      .then((r) => {
        if (this.order) {
          this.order.status = status;
        }
        this.matSnackBar.open(r.message, 'OK', { duration: 2000 });
      })
      .catch((e) => {
        this.matSnackBar.open(e.message, 'OK', { duration: 2000 });
      });
  }
}
