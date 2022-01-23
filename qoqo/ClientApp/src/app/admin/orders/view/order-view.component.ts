import { Component } from '@angular/core';
import { client } from '../../../../utils/client';
import { Order, OrderStatusKeys } from '../../../../types/order';
import { ActivatedRoute } from '@angular/router';
import { SnackbarService } from '../../../services/snackbar.service';

@Component({
  selector: 'app-view',
  templateUrl: './order-view.component.html',
})
export class OrderViewComponent {
  orderId?: string | null;
  order?: Order;

  constructor(
    private route: ActivatedRoute,
    private snackbar: SnackbarService
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
      method: 'PUT',
      data: { status },
    })
      .then((r) => {
        if (this.order) {
          this.order.status = status;
        }
        this.snackbar.openMessage(r.message);
      })
      .catch((e) => {
        this.snackbar.openMessage(e.message);
      });
  }
}
