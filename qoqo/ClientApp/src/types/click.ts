export type Click = {
  user: {
    username: string;
    id: number;
  };
  clickCount: number;
  clickObjective: number;
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

export type ClickState = {
  click: number;
  remainingTime: number;
  userId: number;
};
