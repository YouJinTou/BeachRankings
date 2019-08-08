import { Component, OnInit, OnDestroy, Query } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Subject } from 'rxjs';
import { Beach } from '../beaches/beach';
import { environment } from '../../environments/environment'

@Component({
    templateUrl: 'stats.table.component.html',
    selector: 'stats-table'
})
export class StatsTableComponent implements OnInit, OnDestroy {
    beaches: Beach[];
    options: DataTables.Settings;
    trigger: Subject<Beach>;

    constructor(private httpClient: HttpClient) { 
      this.trigger = new Subject();
    }

    ngOnInit(): void {
        this.options = {
          pagingType: 'full_numbers',
          pageLength: 5,
          scrollX: true,
          responsive: true
        };
        this.httpClient.get<Beach[]>(environment.searchBeachesEndpoint + this.buildQuery())
          .subscribe(beaches => {
            this.beaches = beaches;

            this.trigger.next();
          });
      }
    
      ngOnDestroy(): void {
        this.trigger.unsubscribe();
      }

      buildQuery() {
        let query = 'prefix=b';

        return query;
      }
}