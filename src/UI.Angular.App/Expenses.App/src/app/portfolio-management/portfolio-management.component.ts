import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Portfolio } from '../portfolio';
import { Transaction } from '../investment';
import { InvestmentsService } from '../investments.service';
import { FeatureFlagsService } from '../feature-flags.service';

@Component({
  selector: 'app-portfolio-management',
  templateUrl: './portfolio-management.component.html',
  styleUrls: ['./portfolio-management.component.scss'],
  standalone: false
})
export class PortfolioManagementComponent implements OnInit {
  @Output() displaySummaryEvent = new EventEmitter<boolean>();

  portfolios: Portfolio[] = [];
  recentTransactions: Transaction[] = [];
  portfolioForm!: FormGroup;
  isFormVisible = false;
  isTransactionFormVisible = false;
  editingPortfolioId: string | null = null;

  portfolioColumns: string[] = ['name', 'description', 'createdAt', 'actions'];
  transactionColumns: string[] = ['date', 'type', 'stockType', 'symbol', 'quantity', 'price', 'totalAmount'];

  constructor(
    private fb: FormBuilder,
    private investmentsService: InvestmentsService,
    public featureFlags: FeatureFlagsService
  ) {
    this.createForm();
  }

  ngOnInit(): void {
    this.loadPortfolios();
    this.loadRecentTransactions();
  }

  createForm(): void {
    this.portfolioForm = this.fb.group({
      name: ['', Validators.required],
      description: ['']
    });
  }

  loadPortfolios(): void {
    this.investmentsService.getPortfolios().subscribe(portfolios => {
      this.portfolios = portfolios;
    });
  }

  loadRecentTransactions(): void {
    this.investmentsService.getRecentTransactions(10).subscribe(transactions => {
      this.recentTransactions = transactions;
    });
  }

  showForm(): void {
    this.isFormVisible = true;
    this.editingPortfolioId = null;
    this.portfolioForm.reset();
  }

  editPortfolio(portfolio: Portfolio): void {
    this.isFormVisible = true;
    this.editingPortfolioId = portfolio.id;
    this.portfolioForm.patchValue({ name: portfolio.name, description: portfolio.description });
  }

  savePortfolio(): void {
    if (this.portfolioForm.invalid) return;

    const data = this.portfolioForm.value;
    const action = this.editingPortfolioId
      ? this.investmentsService.updatePortfolio(this.editingPortfolioId, data)
      : this.investmentsService.createPortfolio(data);

    action.subscribe({
      next: () => {
        this.isFormVisible = false;
        this.portfolioForm.reset();
        this.loadPortfolios();
      },
      error: err => console.error('Error saving portfolio:', err)
    });
  }

  deletePortfolio(portfolioId: string): void {
    if (confirm('Are you sure you want to delete this portfolio?')) {
      this.investmentsService.deletePortfolio(portfolioId).subscribe({
        next: () => this.loadPortfolios(),
        error: err => console.error('Error deleting portfolio:', err)
      });
    }
  }

  cancelForm(): void {
    this.isFormVisible = false;
    this.portfolioForm.reset();
  }

  showTransactionForm(): void {
    this.isTransactionFormVisible = true;
  }

  onTransactionSaved(): void {
    this.isTransactionFormVisible = false;
    this.loadRecentTransactions();
  }
}
