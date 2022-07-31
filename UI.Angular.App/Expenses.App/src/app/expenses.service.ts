import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';

import { Expense } from './expense'

@Injectable({
  providedIn: 'root'
})
export class ExpensesService {
  private expensesUrl = 'api/heroes';  // URL to web api

  constructor(private http: HttpClient,) { }

  /** GET heroes from the server */
  getHeroes(): Observable<Expense[]> {
    return this.http.get<Expense[]>(this.expensesUrl)
  }
}
