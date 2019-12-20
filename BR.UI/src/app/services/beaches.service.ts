import { Injectable } from '@angular/core';
import { CreateBeachModel } from '../models/create_beach_model';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class BeachesService {

  constructor(private httpClient: HttpClient) { }

  addBeach(model: CreateBeachModel) {
    console.log(model);

    console.log(environment.beachesUrl);

    this.httpClient.post(environment.beachesUrl, model).subscribe();
  }
}
