import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Subject } from 'rxjs';
import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { environment } from '../../environments/environment';
import { Beach } from '../models/search/beach';
import { DataTableDirective } from 'angular-datatables';

@Component({
  selector: 'place-table',
  templateUrl: './place.table.component.html'
})
export class PlaceTableComponent implements OnInit, OnDestroy {
  beaches: Beach[];
  dtTrigger: Subject<Beach>;
  dtOptions: DataTables.Settings;
  @ViewChild(DataTableDirective, { static: false })
  dtElement: DataTableDirective;

  constructor(private route: ActivatedRoute, private httpClient: HttpClient) {
    this.dtTrigger = new Subject();
  }

  ngOnInit(): void {
    this.dtOptions = {
      pagingType: 'full_numbers',
      pageLength: 5,
      scrollX: true,
      responsive: true
    };

    this.route.url.subscribe(url => {
      this.httpClient.get<Beach[]>(environment.searchBeachesEndpoint + this.buildQuery(url))
        .subscribe(beaches => {
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
  }

  ngOnDestroy(): void {
    this.dtTrigger.unsubscribe();
  }

  private buildQuery(url: any): any {
    let queryParam = '';

    switch (url[0].path) {
      case 'countries':
        queryParam = 'country';

        break;
      case 'waterbodies':
        queryParam = 'waterBody';

        break;
      default:
        queryParam = url[0].path.slice(0, -1);
    }

    let searchQuery = `${queryParam}=${url[1]}`;

    return searchQuery;
  }
}
