import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ReviewsService } from '../../services/reviews.service';
import { ViewReviewModel } from '../../models/view-review-model';

@Component({
  selector: 'app-edit-review',
  templateUrl: './edit-review.component.html',
  styleUrls: ['./edit-review.component.css']
})
export class EditReviewComponent implements OnInit {
  review: ViewReviewModel;

  constructor(
    private reviewsService: ReviewsService, private route: ActivatedRoute, private router: Router) {
  }

  ngOnInit() {
    this.review = new ViewReviewModel();

    this.route.params.subscribe(p => {
      this.reviewsService.getReview(p['id']).subscribe(r => this.review = r);
    });
  }

  submit() {
    this.reviewsService.editReview(this.review)
      .subscribe(r => this.router.navigate(['/beaches/' + this.review.beachId]));
  }
}
