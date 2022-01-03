import { User } from './users';

export type OrderList = {
  orderId: number;
  offer: {
    title: string;
    offerId: number;
  };
  createdAt: string;
  status: string;
}[];

export type OrderDashboard = {
  orderId: number;
  status: string;
  user: User;
  createdAt: string;
};
