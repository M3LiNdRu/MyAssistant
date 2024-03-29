import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';

import { environment } from '../environments/environment';

import { Category } from './category';

@Injectable({
  providedIn: 'root'
})
export class CategoriesService {

  private categoriesUrl = environment.apiUrl + 'api/v1/categories';
  private httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient,) { }

  /** GET expenses from the server */
  getCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(this.categoriesUrl)
  }

  addCategory(category: Category): Observable<boolean> {
    return this.http.post<Category>(environment.apiUrl + 'api/v1/category', category, this.httpOptions).pipe(
      tap(_ => console.log(`added category w/ id=${category.id}`)),
      catchError(this.handleError<any>('addCategory'))
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
