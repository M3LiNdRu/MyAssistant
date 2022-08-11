import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';

import { environment } from '../environments/environment';

import { Tags } from './tag';

@Injectable({
  providedIn: 'root'
})
export class TagsService {

  private tagsUrl = environment.apiUrl + 'api/v1/tags';

  constructor(private http: HttpClient,) { }

  /** GET expenses from the server */
  getTags(): Observable<Tags> {
    return this.http.get<Tags>(this.tagsUrl)
  }
}
