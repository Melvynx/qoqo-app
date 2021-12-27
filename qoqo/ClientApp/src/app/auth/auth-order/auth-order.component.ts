import { Component } from '@angular/core';
import { ClickList } from 'src/types/click';
import { OrderList } from 'src/types/order';
import { client } from 'src/utils/client';

@Component({
  selector: 'app-auth-click',
  templateUrl: './auth-order.component.html',
})
export class AuthOrderComponent {
  orders: OrderList = [];

  constructor() {
    client<OrderList>('orders').then((orders) => {
      console.log('clicks', orders);
      this.orders = [...orders, ...orders, ...orders, ...orders, ...orders];
    });
  }

  getCreatedAt(createdAt: string) {
    return new Date(createdAt).toLocaleString();
  }
}
