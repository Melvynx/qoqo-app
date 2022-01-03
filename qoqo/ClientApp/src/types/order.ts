import { User } from './users';

export type Order = {
  orderId: number;
  status: OrderStatusKeys;
  user: User;
  offer: {
    title: string;
    offerId: number;
  };
  createdAt: string;
};

export const OrderStatus = {
  PENDING: 'PENDING',
  DELIVERED: 'DELIVERED',
  CANCELLED: 'CANCELLED',
};

export type OrderStatusKeys = keyof typeof OrderStatus;

export type OrderList = Omit<Order, 'user'>[];

export type OrderDashboard = {
  orderId: number;
  status: string;
  user: User;
  createdAt: string;
};
