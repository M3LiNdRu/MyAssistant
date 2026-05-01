export interface PortfolioStrategy {
  stockType: string;
  strategyInfo: string;
}

export interface Portfolio {
  id: string;
  name: string;
  description: string;
  strategy: PortfolioStrategy[];
  createdAt: Date;
  updatedAt: Date;
}
