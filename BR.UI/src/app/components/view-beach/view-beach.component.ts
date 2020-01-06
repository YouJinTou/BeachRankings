import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BeachesService } from '../../services/beaches.service';
import { ViewBeachModel } from '../../models/view.beach.model';
import { ViewReviewModel } from '../../models/view-review-model';
import { UsersService } from '../../services/users.service';
import { LoginResult } from '../../models/login.result';
import { ReviewsService } from '../../services/reviews.service';

@Component({
  selector: 'app-view-beach',
  templateUrl: './view-beach.component.html',
  styleUrls: ['./view-beach.component.css']
})
export class ViewBeachComponent implements OnInit {
  private user: LoginResult;
  beach: ViewBeachModel;

  constructor(
    private beachesService: BeachesService,
    private usersService: UsersService,
    private reviewsService: ReviewsService,
    private route: ActivatedRoute,
    private router: Router) {
    this.beach = new ViewBeachModel();

    this.usersService.currentUser.subscribe(u => this.user = u);
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.beachesService.getBeach(params['id']).subscribe(beach => this.beach = beach);
    })
  }

  redirectToEditBeach() {
    this.router.navigate(['/beaches/' + this.beach.id + '/edit'])
  }

  redirectToLeaveReview() {
    this.router.navigate(['/beaches/' + this.beach.id + '/review']);
  }

  redirectToEditReview(id: string) {
    this.router.navigate(['/reviews/' + id + '/edit']);
  }

  redirectToDeleteReview(id: string) {
    this.reviewsService.deleteReview(id).subscribe(r => {
      this.router.navigate(['/beaches/' + this.beach.id]);
    });
  }

  canEditBeach() {
    return this.user.id == this.beach.addedBy;
  }

  canEditReview(review: ViewReviewModel) {
    return this.user.id == review.userId;
  }
}
