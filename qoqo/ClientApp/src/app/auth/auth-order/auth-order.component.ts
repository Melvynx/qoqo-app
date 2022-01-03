import { Component } from '@angular/core';
import { ClickList } from 'src/types/click';
import { OrderList } from 'src/types/order';
import { client } from 'src/utils/client';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-auth-click',
  templateUrl: './auth-order.component.html',
})
export class AuthOrderComponent {
  orders: OrderList = [];

  constructor(private authService: AuthService) {
    client<OrderList>(`orders/users/${authService.user?.userId}`).then(
      (orders) => {
        this.orders = orders;
      }
    );
  }

  getLocaleDate(createdAt: string) {
    return new Date(createdAt).toLocaleString();
  }
}
