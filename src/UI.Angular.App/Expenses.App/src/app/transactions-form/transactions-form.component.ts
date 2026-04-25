import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Portfolio } from '../portfolio';
import { InvestmentsService } from '../investments.service';

@Component({
  selector: 'app-transactions-form',
  templateUrl: './transactions-form.component.html',
  styleUrls: ['./transactions-form.component.scss'],
  standalone: false
})
export class TransactionsFormComponent implements OnInit {
  @Output() displaySummaryEvent = new EventEmitter<boolean>();

  transactionForm!: FormGroup;
  portfolios: Portfolio[] = [];

  assetTypes = ['Cryptocurrency', 'ETF', 'Stock', 'Bond', 'Gold', 'Deposit', 'Real Estate', 'Other'];
  transactionTypes: ('Buy' | 'Sell')[] = ['Buy', 'Sell'];

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
    this.transactionForm = this.fb.group({
      portfolioId: ['', Validators.required],
      type: ['Buy', Validators.required],
      assetType: ['', Validators.required],
      symbol: ['', Validators.required],
      quantity: [null, [Validators.required, Validators.min(0.000001)]],
      price: [null, [Validators.required, Validators.min(0)]],
      date: [new Date(), Validators.required],
      notes: ['']
    });
  }

  loadPortfolios(): void {
    this.investmentsService.getPortfolios().subscribe(portfolios => {
      this.portfolios = portfolios;
      if (portfolios.length > 0) {
        this.transactionForm.patchValue({ portfolioId: portfolios[0].id });
      }
    });
  }

  submit(): void {
    if (this.transactionForm.invalid) return;

    this.investmentsService.addTransaction(this.transactionForm.value).subscribe({
      next: () => {
        this.transactionForm.reset({ type: 'Buy', date: new Date() });
        this.displaySummaryEvent.emit(true);
      },
      error: err => console.error('Error adding transaction:', err)
    });
  }

  cancel(): void {
    this.displaySummaryEvent.emit(true);
  }
}
