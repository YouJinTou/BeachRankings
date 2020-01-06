import { Component, OnInit } from '@angular/core';
import { ReviewsService } from '../../services/reviews.service';
import { AddReviewModel } from '../../models/add-review-model';
import { ActivatedRoute, Router } from '@angular/router';
import { UsersService } from '../../services/users.service';

@Component({
  selector: 'app-add-review',
  templateUrl: './add-review.component.html',
  styleUrls: ['./add-review.component.css']
})
export class AddReviewComponent implements OnInit {
  model: AddReviewModel;

  constructor(
    private reviewsService: ReviewsService, 
    private usersService: UsersService,
    private route: ActivatedRoute, 
    private router: Router) { 
    this.model = new AddReviewModel();

    this.route.url.subscribe(u => this.model.beachId = u[1].path);

    this.usersService.currentUser.subscribe(u => this.model.userId = u.id);
  }

  ngOnInit() {
  }

  submit() {
    this.reviewsService.addReview(this.model).subscribe(
      r => this.router.navigate(['/beaches/' + r]));
  }
}
