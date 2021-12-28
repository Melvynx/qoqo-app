export type User = {
  id: number;
  userName: string;
  firstName?: string;
  lastName?: string;
  email: string;
  token?: string;
  isAdmin?: boolean;
};
