import { Component, OnInit } from '@angular/core';
import { client } from '../../../utils/client';
import { OrderList } from '../../../types/order';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.css'],
})
export class OrdersComponent {
  orders: OrderList = [];

  constructor() {
    client<OrderList>('orders').then((orders) => {
      this.orders = orders;
    });
  }

  getLocaleDate(date: string) {
    return new Date(date).toLocaleString();
  }
}
