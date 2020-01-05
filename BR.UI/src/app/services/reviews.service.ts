import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AddReviewModel } from '../models/add-review-model';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ReviewsService {

  constructor(private httpClient: HttpClient) { }

  public addReview(review: AddReviewModel): Observable<string> {
    return this.httpClient.post<string>(environment.reviewsUrl, review);
  }
}
