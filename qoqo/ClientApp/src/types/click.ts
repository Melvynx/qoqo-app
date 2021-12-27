export type Click = {
  user: {
    username: string;
    id: number;
  };
  clickCount: number;
};

export type ClickList = {
  count: number;
  offerTitle: string;
  offerId: number;
}[];
