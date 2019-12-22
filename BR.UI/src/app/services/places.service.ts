import { Injectable } from '@angular/core';
import { Place } from '../models/place';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators'
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PlacesService {

  constructor(private httpClient: HttpClient) { }

  public getCountries(): Observable<Place[]> {
    return this.httpClient.get<string[]>(environment.countriesUrl)
      .pipe(map(countries => {
        return countries.map<Place>(c => new Place(c, c));
      }));
  }

  public getNextLevel(id: string): Observable<Place[]> {
    return this.httpClient.get<any[]>(environment.placeChildrenUrl.replace('{id}', id))
      .pipe(map(ls => {
        if (!ls) {
          return [];
        }

        return ls.map<Place>(l => new Place(l['id'], l['name']));
      }));
  }
}
