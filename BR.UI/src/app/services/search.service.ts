import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { environment } from '../../environments/environment';
import { SearchPlaceResult } from '../models/search.place.result';

@Injectable({
  providedIn: 'root'
})
export class SearchService {

  constructor(private httpClient: HttpClient) { }

  search(query: string): Observable<any> {
    if (!query) {
      return of([]);
    }

    return this.httpClient.get<any[]>(environment.searchUrl + query);
  }

  searchPlace(id: string, name: string, type: string): Observable<SearchPlaceResult> {
    if (!id || !name || !type) {
      return of(new SearchPlaceResult());
    }

    let url = environment.searchPlaceUrl
      .replace('{id}', id).replace('{name}', name).replace('{type}', type);
    let encodedUrl = encodeURI(url);

    return this.httpClient.get<SearchPlaceResult>(encodedUrl);
  }
}
