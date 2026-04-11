import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { Investment, Portfolio, PortfolioSummary } from '../investment';
import { InvestmentsService } from '../investments.service';

@Component({
  selector: 'app-investments-list',
  templateUrl: './investments-list.component.html',
  styleUrls: ['./investments-list.component.scss'],
  standalone: false
})
export class InvestmentsListComponent implements OnInit {
  @Output() displaySummaryEvent = new EventEmitter<boolean>();

  portfolios: Portfolio[] = [];
  selectedPortfolioId: string | null = null;
  investments: Investment[] = [];
  portfolioSummary: PortfolioSummary | null = null;

  displayedColumns: string[] = ['symbol', 'assetType', 'quantity', 'purchasePrice', 'totalCost', 'currentPrice', 'totalValue', 'gainLoss', 'gainLossPercent', 'actions'];

  constructor(private investmentsService: InvestmentsService) { }

  ngOnInit(): void {
    this.loadPortfolios();
  }

  loadPortfolios(): void {
    this.investmentsService.getPortfolios().subscribe(portfolios => {
      this.portfolios = portfolios;
      if (portfolios.length > 0) {
        this.selectPortfolio(portfolios[0].id);
      }
    });
  }

  selectPortfolio(portfolioId: string): void {
    this.selectedPortfolioId = portfolioId;
    this.loadInvestments();
    this.loadPortfolioSummary();
  }

  loadInvestments(): void {
    if (!this.selectedPortfolioId) return;
    this.investmentsService.getInvestments(this.selectedPortfolioId).subscribe(investments => {
      this.investments = investments;
    });
  }

  loadPortfolioSummary(): void {
    if (!this.selectedPortfolioId) return;
    this.investmentsService.getPortfolioSummary(this.selectedPortfolioId).subscribe(summary => {
      this.portfolioSummary = summary;
    });
  }

  deleteInvestment(investmentId: string): void {
    if (confirm('Are you sure you want to delete this investment?')) {
      this.investmentsService.deleteInvestment(investmentId).subscribe(() => {
        this.loadInvestments();
        this.loadPortfolioSummary();
      });
    }
  }

  goBack(): void {
    this.displaySummaryEvent.emit(true);
  }
}
