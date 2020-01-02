import { ActivatedRoute } from '@angular/router';
import { Subject } from 'rxjs';
import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { DataTableDirective } from 'angular-datatables';
import { BeachesService } from '../../services/beaches.service';
import { SearchService } from '../../services/search.service';
import { ViewBeachModel } from '../../models/view.beach.model';

@Component({
  selector: 'app-place-table',
  templateUrl: './place-table.component.html'
})
export class PlaceTableComponent implements OnInit, OnDestroy {
  beaches: ViewBeachModel[];
  dtTrigger: Subject<ViewBeachModel>;
  dtOptions: DataTables.Settings;
  @ViewChild(DataTableDirective, { static: false })
  dtElement: DataTableDirective;

  constructor(
    private searchService: SearchService,
    private beachesService: BeachesService,
    private route: ActivatedRoute) {
    this.dtTrigger = new Subject();
  }

  ngOnInit(): void {
    this.dtOptions = {
      pagingType: 'full_numbers',
      pageLength: 5,
      scrollX: true,
      responsive: true
    };

    this.route.params.subscribe(params => {
      let placeIid = params['id'].split('_')[0];
      let placeName = params['id'].split('_')[1];

      this.searchService.searchPlace(placeIid, placeName).subscribe(result => {
        this.beachesService.getPlaceBeaches(result.beachIds).subscribe(beaches => {
          if (this.dtElement && this.dtElement.dtInstance) {
            this.dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
              console.log('destroying...')
              dtInstance.destroy();
            });
          }

          this.beaches = beaches;

          this.dtTrigger.next();
        });
      });
    });
  }

  ngOnDestroy(): void {
    this.dtTrigger.unsubscribe();
  }
}
