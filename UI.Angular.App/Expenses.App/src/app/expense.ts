import { Tag } from './tag';

export interface Expense {
    id: number;
    name: string;
    category: string;
    timestamp: Date;
    amount: number;
    currency: string;
    tags: Tag[]
  }