import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { Portfolio } from '../portfolio';
import { InvestmentsService } from '../investments.service';

@Component({
  selector: 'app-transactions-list',
  templateUrl: './transactions-list.component.html',
  styleUrls: ['./transactions-list.component.scss'],
  standalone: false
})
export class TransactionsListComponent implements OnInit {
  @Output() displaySummaryEvent = new EventEmitter<boolean>();

  portfolios: Portfolio[] = [];
  selectedPortfolioId: string | null = null;

  displayedColumns: string[] = ['name', 'description', 'createdAt'];

  constructor(private investmentsService: InvestmentsService) { }

  ngOnInit(): void {
    this.loadPortfolios();
  }

  loadPortfolios(): void {
    this.investmentsService.getPortfolios().subscribe(portfolios => {
      this.portfolios = portfolios;
      if (portfolios.length > 0) {
        this.selectedPortfolioId = portfolios[0].id;
      }
    });
  }

  goBack(): void {
    this.displaySummaryEvent.emit(true);
  }
}
