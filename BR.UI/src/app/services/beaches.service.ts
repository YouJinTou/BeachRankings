import { Injectable } from '@angular/core';
import { CreateBeachModel } from '../models/create.beach.model';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { ViewBeachModel } from '../models/view.beach.model';

@Injectable({
  providedIn: 'root'
})
export class BeachesService {

  constructor(private httpClient: HttpClient) { }

  public getBeach(id: string): Observable<ViewBeachModel> {
    return this.httpClient.get<ViewBeachModel>(environment.beachesUrl + id);
  }

  addBeach(model: CreateBeachModel) {
    this.httpClient.post(environment.beachesUrl, model).subscribe();
  }
}
