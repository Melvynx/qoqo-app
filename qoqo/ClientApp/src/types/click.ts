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

export type ClickFinishResult = {
  userId: number;
  clickCount: number;
  userName: string;
  finishSentence: string;
};
