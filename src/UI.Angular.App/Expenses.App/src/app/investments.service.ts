import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable, of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

import { environment } from '../environments/environment';
import { Portfolio, Investment, PortfolioSummary } from './investment';

@Injectable({
  providedIn: 'root'
})
export class InvestmentsService {
  private portfoliosUrl = environment.apiUrl + 'api/v1/portfolio';
  private investmentsUrl = environment.apiUrl + 'api/v1/investment';
  private httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient) { }

  // Portfolio endpoints
  getPortfolios(): Observable<Portfolio[]> {
    return this.http.get<Portfolio[]>(environment.apiUrl + 'api/v1/portfolios');
  }

  getPortfolio(portfolioId: string): Observable<Portfolio> {
    return this.http.get<Portfolio>(`${this.portfoliosUrl}/${portfolioId}`);
  }

  createPortfolio(portfolio: Partial<Portfolio>): Observable<Portfolio> {
    return this.http.post<Portfolio>(this.portfoliosUrl, portfolio, this.httpOptions).pipe(
      tap(_ => console.log(`added portfolio w/ id=${portfolio.id}`)),
      catchError(this.handleError<Portfolio>('createPortfolio'))
    );
  }

  updatePortfolio(portfolioId: string, portfolio: Partial<Portfolio>): Observable<Portfolio> {
    return this.http.put<Portfolio>(`${this.portfoliosUrl}/${portfolioId}`, portfolio, this.httpOptions).pipe(
      tap(_ => console.log(`updated portfolio w/ id=${portfolioId}`)),
      catchError(this.handleError<Portfolio>('updatePortfolio'))
    );
  }

  deletePortfolio(portfolioId: string): Observable<void> {
    return this.http.delete<void>(`${this.portfoliosUrl}/${portfolioId}`).pipe(
      tap(_ => console.log(`deleted portfolio w/ id=${portfolioId}`)),
      catchError(this.handleError<void>('deletePortfolio'))
    );
  }

  // Investment endpoints
  getInvestments(portfolioId: string): Observable<Investment[]> {
    return this.http.get<Investment[]>(`${environment.apiUrl}api/v1/portfolio/${portfolioId}/investments`);
  }

  getInvestment(investmentId: string): Observable<Investment> {
    return this.http.get<Investment>(`${this.investmentsUrl}/${investmentId}`);
  }

  createInvestment(investment: Partial<Investment>): Observable<Investment> {
    return this.http.post<Investment>(this.investmentsUrl, investment, this.httpOptions).pipe(
      tap(_ => console.log(`added investment w/ id=${investment.id}`)),
      catchError(this.handleError<Investment>('createInvestment'))
    );
  }

  updateInvestment(investmentId: string, investment: Partial<Investment>): Observable<Investment> {
    return this.http.put<Investment>(`${this.investmentsUrl}/${investmentId}`, investment, this.httpOptions).pipe(
      tap(_ => console.log(`updated investment w/ id=${investmentId}`)),
      catchError(this.handleError<Investment>('updateInvestment'))
    );
  }

  deleteInvestment(investmentId: string): Observable<void> {
    return this.http.delete<void>(`${this.investmentsUrl}/${investmentId}`).pipe(
      tap(_ => console.log(`deleted investment w/ id=${investmentId}`)),
      catchError(this.handleError<void>('deleteInvestment'))
    );
  }

  // Summary endpoint
  getPortfolioSummary(portfolioId: string): Observable<PortfolioSummary> {
    return this.http.get<PortfolioSummary>(`${environment.apiUrl}api/v1/portfolio/${portfolioId}/summary`);
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(error);
      console.log(`${operation} failed: ${error.message}`);
      return of(result as T);
    };
  }
}
