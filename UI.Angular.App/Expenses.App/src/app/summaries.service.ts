import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';

import { environment } from 'src/environments/environment';
import { CurrentSummary } from './currentSummary';

@Injectable({
  providedIn: 'root'
})
export class SummariesService {

  private httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };
  
  constructor(private http: HttpClient) { }

  /** GET summary from the server */
  getSummary(): Observable<CurrentSummary> {
    return this.http.get<CurrentSummary>(environment.apiUrl + 'api/v1/summary/current', this.httpOptions).pipe(
      tap(summary => console.log(`getting summary month=${summary.month}`)),
      catchError(this.handleError<any>('getting summary'))
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
