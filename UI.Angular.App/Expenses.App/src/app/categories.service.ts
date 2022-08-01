import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';

import { Category } from './category';

@Injectable({
  providedIn: 'root'
})
export class CategoriesService {

  private categoriesUrl = 'api/categories';

  constructor(private http: HttpClient,) { }

  /** GET expenses from the server */
  getCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(this.categoriesUrl)
  }
}
