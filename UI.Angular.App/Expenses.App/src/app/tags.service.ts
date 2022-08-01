import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';

import { Tag } from './tag';

@Injectable({
  providedIn: 'root'
})
export class TagsService {

  private tagsUrl = 'api/tags';

  constructor(private http: HttpClient,) { }

  /** GET expenses from the server */
  getTags(): Observable<Tag[]> {
    return this.http.get<Tag[]>(this.tagsUrl)
  }
}
