export type User = {
  userId: number;
  userName: string;
  firstName?: string;
  lastName?: string;
  email: string;
  token?: string;
  isAdmin?: boolean;
  fullAddress?: string;
};
