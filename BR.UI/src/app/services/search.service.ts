import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable, of } from 'rxjs';

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
}
