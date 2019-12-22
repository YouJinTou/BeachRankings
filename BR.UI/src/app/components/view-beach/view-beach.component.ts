import { Component, OnInit } from '@angular/core';
import { BeachesService } from '../../services/beaches.service';
import { ViewBeachModel } from '../../models/view.beach.model';

@Component({
  selector: 'app-view-beach',
  templateUrl: './view-beach.component.html',
  styleUrls: ['./view-beach.component.css']
})
export class ViewBeachComponent implements OnInit {
  beach: ViewBeachModel;

  constructor(private beachesService: BeachesService) { 
    this.beach = new ViewBeachModel();
  }

  ngOnInit() {
    this.beachesService.getBeach(
      'coral cove beach_asia_thailand_surat thani_ko samui district_ko samui_bo phut_gulf of thailand')
      .subscribe(result => {console.log(result); this.beach = result;});
  }
}
