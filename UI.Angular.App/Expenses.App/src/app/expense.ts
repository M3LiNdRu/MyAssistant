import { Tag } from './tag';export interface Expense {
    id: string;
    name: string;
    category: string;
    timestamp: Date;
    amount: number;
    currency: string;
    tags: string[]
  }