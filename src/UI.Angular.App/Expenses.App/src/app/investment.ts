export interface PortfolioDto {
  id: string;
  name: string;
}

export interface Transaction {
  id: string;
  portfolio: PortfolioDto;
  symbol: string;
  assetType: string;
  type: 'Buy' | 'Sell';
  quantity: number;
  price: number;
  date: Date;
  notes?: string;
  createdAt: Date;
  updatedAt: Date;
}

export interface TransactionRequest {
  portfolioId: string;
  symbol: string;
  assetType: string;
  type: 'Buy' | 'Sell';
  quantity: number;
  price: number;
  date: Date;
  notes?: string;
}
