import { OrderDashboard } from './order';

export type Offer = {
  offerId: number;
  title: string;
  description: string;
  barredPrice: number;
  price: number;
  clickObjective: number;
  specificationText?: string;
  winnerText?: string;
  imageUrl?: string;
  isOver?: boolean;
  isDraft?: boolean;
  isLive?: boolean;
  startAt?: string;
  endAt?: string;
  createdAt?: string;
};

export const EmptyOffer: Offer = {
  offerId: 0,
  title: '',
  description: '',
  imageUrl: '',
  specificationText: '',
  barredPrice: 0,
  price: 0,
  clickObjective: 0,
  isOver: false,
  isDraft: true,
  startAt: undefined,
  endAt: undefined,
};

export type Dashboard = {
  offerId?: number;
  offerTitle?: string;
  isOver?: boolean;
  clickCount?: number;
  clickObjective?: number;
  countOfActiveUser?: number;
  endAt?: string;
  order?: OrderDashboard;
  currentUserCount?: number;
  isNextOffer: boolean;
};
