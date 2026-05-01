import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { Portfolio } from '../portfolio';
import { InvestmentsService } from '../investments.service';

function totalAmountValidator(control: AbstractControl): ValidationErrors | null {
  const quantity: number = control.get('stock.quantity')?.value ?? 0;
  const priceAmount: number = control.get('stock.price.amount')?.value ?? 0;
  const totalAmount: number = control.get('totalAmount.amount')?.value ?? 0;
  const fees = control.get('fees') as FormArray | null;
  const feesSum = fees
    ? (fees.controls as FormGroup[]).reduce((sum, f) => sum + (f.get('fee.amount')?.value ?? 0), 0)
    : 0;

  const expected = quantity * priceAmount + feesSum;
  if (totalAmount !== 0 && Math.abs(totalAmount - expected) > 0.0001) {
    return { totalAmountMismatch: { expected } };
  }
  return null;
}

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

  types = ['Cryptocurrency', 'ETF', 'Stock', 'Bond', 'Gold', 'Deposit', 'Real Estate', 'Other'];
  transactionTypes: ('Buy' | 'Sell')[] = ['Buy', 'Sell'];
  brokers = ['Revolut', 'MetaMask', 'CaixaEnginyers'];

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
      broker: ['', Validators.required],
      stock: this.fb.group({
        symbol: ['', Validators.required],
        type: ['', Validators.required],
        quantity: [null, [Validators.required, Validators.min(0.000001)]],
        price: this.fb.group({
          amount: [null, [Validators.required, Validators.min(0)]],
          currencyCode: ['EUR', Validators.required]
        })
      }),
      totalAmount: this.fb.group({
        amount: [null, [Validators.required, Validators.min(0)]],
        currencyCode: ['EUR', Validators.required]
      }),
      fees: this.fb.array([]),
      date: [new Date(), Validators.required],
      notes: ['']
    }, { validators: totalAmountValidator });
  }

  get totalAmountMismatch(): boolean {
    return this.transactionForm.hasError('totalAmountMismatch');
  }

  get expectedTotalAmount(): number {
    return this.transactionForm.getError('totalAmountMismatch')?.expected ?? 0;
  }

  get fees(): FormArray {
    return this.transactionForm.get('fees') as FormArray;
  }

  addFee(): void {
    this.fees.push(this.fb.group({
      description: ['', Validators.required],
      fee: this.fb.group({
        amount: [null, [Validators.required, Validators.min(0)]],
        currencyCode: ['EUR', Validators.required]
      })
    }));
  }

  removeFee(index: number): void {
    this.fees.removeAt(index);
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
