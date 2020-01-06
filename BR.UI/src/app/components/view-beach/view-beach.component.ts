import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BeachesService } from '../../services/beaches.service';
import { ViewBeachModel } from '../../models/view.beach.model';
import { ViewReviewModel } from '../../models/view-review-model';
import { UsersService } from 'src/app/services/users.service';
import { LoginResult } from 'src/app/models/login.result';
import { timingSafeEqual } from 'crypto';

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

  redirectToLeaveReview() {
    this.router.navigate(['/beaches/' + this.beach.id + '/review']);
  }

  redirectToEditReview(id: string) {
    this.router.navigate(['/reviews/' + id + '/edit']);
  }

  redirectToDeleteReview(id: string) {
    this.router.navigate(['/reviews/' + id]);
  }

  canEditReview(review: ViewReviewModel) {
    return this.user.id == review.userId;
  }
}
