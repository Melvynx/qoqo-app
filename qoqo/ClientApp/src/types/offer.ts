export type Offer = {
  id: number;
  title: string;
  description: string;
  barredPrice: number;
  price: number;
  clickObjective: string;
  specificationText: string;
  winnerText: string;
  imageUrl: string;
  isOver: boolean;
  isDraft: boolean;
  startAt: Date;
  endAt: Date;
  createdAt: Date;
};
