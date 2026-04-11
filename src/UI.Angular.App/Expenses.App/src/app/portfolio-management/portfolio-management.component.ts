import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Portfolio } from '../investment';
import { InvestmentsService } from '../investments.service';

@Component({
  selector: 'app-portfolio-management',
  templateUrl: './portfolio-management.component.html',
  styleUrls: ['./portfolio-management.component.scss'],
  standalone: false
})
export class PortfolioManagementComponent implements OnInit {
  @Output() displaySummaryEvent = new EventEmitter<boolean>();
  @Output() displayInvestmentsEvent = new EventEmitter<boolean>();

  portfolios: Portfolio[] = [];
  portfolioForm!: FormGroup;
  isFormVisible = false;
  editingPortfolioId: string | null = null;

  displayedColumns: string[] = ['name', 'description', 'createdAt', 'actions'];

  constructor(
    private fb: FormBuilder,
    private investmentsService: InvestmentsService
  ) {
    this.createForm();
  }

  ngOnInit(): void {
    this.loadPortfolios();
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

  showForm(): void {
    this.isFormVisible = true;
    this.editingPortfolioId = null;
    this.portfolioForm.reset();
  }

  editPortfolio(portfolio: Portfolio): void {
    this.isFormVisible = true;
    this.editingPortfolioId = portfolio.id;
    this.portfolioForm.patchValue({
      name: portfolio.name,
      description: portfolio.description
    });
  }

  savePortfolio(): void {
    if (this.portfolioForm.valid) {
      const portfolioData = this.portfolioForm.value;

      if (this.editingPortfolioId) {
        this.investmentsService.updatePortfolio(this.editingPortfolioId, portfolioData).subscribe(
          () => {
            console.log('Portfolio updated successfully');
            this.isFormVisible = false;
            this.portfolioForm.reset();
            this.loadPortfolios();
          },
          error => {
            console.error('Error updating portfolio:', error);
          }
        );
      } else {
        this.investmentsService.createPortfolio(portfolioData).subscribe(
          () => {
            console.log('Portfolio created successfully');
            this.isFormVisible = false;
            this.portfolioForm.reset();
            this.loadPortfolios();
          },
          error => {
            console.error('Error creating portfolio:', error);
          }
        );
      }
    }
  }

  deletePortfolio(portfolioId: string): void {
    if (confirm('Are you sure you want to delete this portfolio and all its investments?')) {
      this.investmentsService.deletePortfolio(portfolioId).subscribe(
        () => {
          console.log('Portfolio deleted successfully');
          this.loadPortfolios();
        },
        error => {
          console.error('Error deleting portfolio:', error);
        }
      );
    }
  }

  viewPortfolio(portfolioId: string): void {
    this.displayInvestmentsEvent.emit(true);
  }

  cancelForm(): void {
    this.isFormVisible = false;
    this.portfolioForm.reset();
  }

  goBack(): void {
    this.displaySummaryEvent.emit(true);
  }
}
