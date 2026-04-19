export interface Investment {
  id: string;
  portfolioId: string;
  assetType: string;
  symbol: string;
  quantity: number;
  purchasePrice: number;
  purchaseDate: Date;
  currentPrice: number;
  totalCost?: number;
  totalValue?: number;
  gainLoss?: number;
  gainLossPercent?: number;
  createdAt: Date;
  updatedAt: Date;
}

export interface PortfolioSummary {
  portfolioId: string;
  totalInvestments: number;
  totalCost: number;
  totalValue: number;
  gainLoss: number;
  gainLossPercent: number;
  assetAllocation: { [key: string]: AssetAllocation };
}

export interface AssetAllocation {
  type: string;
  value: number;
  percentage: number;
}
