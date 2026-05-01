export interface PortfolioDto {
  id: string;
  name: string;
}

export interface Stock {
  symbol: string;
  type: string;
  quantity: number;
  price: Money;
}

export interface Money {
  amount: number;
  currencyCode: string;
}

export interface TransactionFee {
  description: string;
  fee: Money;
}

export interface Transaction {
  id: string;
  portfolio: PortfolioDto;
  type: 'Buy' | 'Sell';
  broker: string;
  stock: Stock;
  totalAmount: Money;
  fees: TransactionFee[];
  date: Date;
  notes?: string;
  createdAt: Date;
  updatedAt: Date;
}

export interface TransactionRequest {
  portfolioId: string;
  type: 'Buy' | 'Sell';
  broker: string;
  stock: Stock;
  totalAmount: Money;
  fees?: TransactionFee[];
  date: Date;
  notes?: string;
}
