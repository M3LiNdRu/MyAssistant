import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Investment } from '../investment';
import { Portfolio } from '../portfolio';
import { InvestmentsService } from '../investments.service';

@Component({
  selector: 'app-investments-form',
  templateUrl: './investments-form.component.html',
  styleUrls: ['./investments-form.component.scss'],
  standalone: false
})
export class InvestmentsFormComponent implements OnInit {
  @Output() displaySummaryEvent = new EventEmitter<boolean>();

  investmentForm!: FormGroup;
  portfolios: Portfolio[] = [];

  assetTypes = [
    'Cryptocurrency',
    'ETF',
    'Stock',
    'Bond',
    'Gold',
    'Deposit',
    'Real Estate',
    'Other'
  ];

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
    this.investmentForm = this.fb.group({
      portfolioId: ['', Validators.required],
      assetType: ['', Validators.required],
      symbol: ['', Validators.required],
      quantity: ['', [Validators.required, Validators.min(0)]],
      purchasePrice: ['', [Validators.required, Validators.min(0)]],
      purchaseDate: [new Date(), Validators.required],
      currentPrice: ['', [Validators.required, Validators.min(0)]]
    });
  }

  loadPortfolios(): void {
    this.investmentsService.getPortfolios().subscribe(portfolios => {
      this.portfolios = portfolios;
      if (portfolios.length > 0) {
        this.investmentForm.patchValue({ portfolioId: portfolios[0].id });
      }
    });
  }

  addInvestment(): void {
    if (this.investmentForm.valid) {
      const investment = this.investmentForm.value;
      this.investmentsService.createInvestment(investment).subscribe(
        () => {
          console.log('Investment added successfully');
          this.investmentForm.reset();
          this.displaySummaryEvent.emit(true);
        },
        error => {
          console.error('Error adding investment:', error);
        }
      );
    }
  }

  cancel(): void {
    this.displaySummaryEvent.emit(true);
  }
}
