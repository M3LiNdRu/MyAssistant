import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../environments/environment';
import { Transaction, TransactionRequest } from './investment';
import { Portfolio } from './portfolio';

@Injectable({
  providedIn: 'root'
})
export class InvestmentsService {
  private portfoliosUrl = environment.apiUrl + 'api/v1/portfolio';
  private transactionsUrl = environment.apiUrl + 'api/v1/transaction';
  private httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient) { }

  getPortfolios(): Observable<Portfolio[]> {
    return this.http.get<Portfolio[]>(environment.apiUrl + 'api/v1/portfolios');
  }

  getPortfolio(portfolioId: string): Observable<Portfolio> {
    return this.http.get<Portfolio>(`${this.portfoliosUrl}/${portfolioId}`);
  }

  createPortfolio(portfolio: Partial<Portfolio>): Observable<Portfolio> {
    return this.http.post<Portfolio>(this.portfoliosUrl, portfolio, this.httpOptions).pipe(
      catchError(this.handleError<Portfolio>('createPortfolio'))
    );
  }

  updatePortfolio(portfolioId: string, portfolio: Partial<Portfolio>): Observable<Portfolio> {
    return this.http.put<Portfolio>(`${this.portfoliosUrl}/${portfolioId}`, portfolio, this.httpOptions).pipe(
      catchError(this.handleError<Portfolio>('updatePortfolio'))
    );
  }

  deletePortfolio(portfolioId: string): Observable<void> {
    return this.http.delete<void>(`${this.portfoliosUrl}/${portfolioId}`).pipe(
      catchError(this.handleError<void>('deletePortfolio'))
    );
  }

  addTransaction(request: TransactionRequest): Observable<Transaction> {
    return this.http.post<Transaction>(this.transactionsUrl, request, this.httpOptions).pipe(
      catchError(this.handleError<Transaction>('addTransaction'))
    );
  }

  getRecentTransactions(limit: number = 10): Observable<Transaction[]> {
    return this.http.get<Transaction[]>(`${this.transactionsUrl}s/recent?limit=${limit}`).pipe(
      catchError(this.handleError<Transaction[]>('getRecentTransactions', []))
    );
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(`${operation} failed:`, error);
      return of(result as T);
    };
  }
}
