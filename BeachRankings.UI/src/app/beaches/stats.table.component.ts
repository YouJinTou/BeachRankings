import { Component, OnInit, OnDestroy } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Subject } from 'rxjs';
import { Beach } from '../beaches/beach';

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
        this.httpClient.get<Beach[]>('http://localhost:58124/api/search/beaches?prefix=B')
          .subscribe(beaches => {
            this.beaches = beaches;

            this.trigger.next();
          });
      }
    
      ngOnDestroy(): void {
        this.trigger.unsubscribe();
      }
}