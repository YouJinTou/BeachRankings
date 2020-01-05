import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BeachesService } from '../../services/beaches.service';
import { ViewBeachModel } from '../../models/view.beach.model';

@Component({
  selector: 'app-view-beach',
  templateUrl: './view-beach.component.html',
  styleUrls: ['./view-beach.component.css']
})
export class ViewBeachComponent implements OnInit {
  beach: ViewBeachModel;

  constructor(
    private beachesService: BeachesService, private route: ActivatedRoute, private router: Router) { 
    this.beach = new ViewBeachModel();
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.beachesService.getBeach(params['id']).subscribe(beach => this.beach = beach);
    })
  }

  redirectToLeaveReview() {
    this.router.navigate(['/beaches/' + this.beach.id + '/review']);
  }
}
