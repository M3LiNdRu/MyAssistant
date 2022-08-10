import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';

import { environment } from '../environments/environment';

import { Expense } from './expense'

@Injectable({
  providedIn: 'root'
})
export class ExpensesService {
  private expensesUrl = environment.apiUrl + 'api/expenses';
  private httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };
  
  constructor(private http: HttpClient,) { }

  /** GET expenses from the server */
  getExpenses(): Observable<Expense[]> {
    return this.http.get<Expense[]>(this.expensesUrl)
  }

  addExpense(expense: Expense): Observable<boolean> {
    return this.http.post<Expense>(this.expensesUrl, expense, this.httpOptions).pipe(
      tap((newExpense: Expense) => console.log(`added expense w/ id=${newExpense.id}`)),
      catchError(this.handleError<any>('addExpense'))
    );
  }

    /**
   * Handle Http operation that failed.
   * Let the app continue.
   *
   * @param operation - name of the operation that failed
   * @param result - optional value to return as the observable result
   */
    private handleError<T>(operation = 'operation', result?: T) {
      return (error: any): Observable<T> => {

        // TODO: send the error to remote logging infrastructure
        console.error(error); // log to console instead

        // TODO: better job of transforming error for user consumption
        console.log(`${operation} failed: ${error.message}`);

        // Let the app keep running by returning an empty result.
        return of(result as T);
      };
    }
}
