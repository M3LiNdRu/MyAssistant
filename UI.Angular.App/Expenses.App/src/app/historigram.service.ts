import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';


import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';

import { environment } from '../environments/environment';
import { Historigram } from './historigram';

@Injectable({
  providedIn: 'root'
})
export class HistorigramService {

  private historigramUrl = environment.apiUrl + 'api/v1/historigram/savings';
  private httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };
  constructor(private http: HttpClient,) { }

  // GET api/v1/historigram/savings
  getHistorigram(): Observable<Historigram> {
    return this.http.get<Historigram>(this.historigramUrl, this.httpOptions).pipe(
      tap(historigram => console.log(`getting historigram`)),
      catchError(this.handleError<any>('getting historigram'))
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





