import { User } from './users';

export type OrderList = {
  offer: {
    title: string;
    id: number;
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
