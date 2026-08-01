export interface CategoryBreakdown {
  months: string[];
  categories: CategoryRow[];
  totalAverage: number;
}

export interface CategoryRow {
  category: string;
  monthlyAmounts: number[];
  average: number;
}
