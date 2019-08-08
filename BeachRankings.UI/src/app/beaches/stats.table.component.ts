import { Component, OnInit, OnDestroy } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Subject } from 'rxjs';
import { Beach } from '../beaches/beach';

@Component({
    templateUrl: 'stats.table.component.html',
    selector: 'stats-table'
})
export class StatsTableComponent implements OnInit {
    beaches: Beach[];
    dtOptions: DataTables.Settings = {};
    dtTrigger: Subject<Beach> = new Subject();

    constructor(private httpClient: HttpClient) { }

    ngOnInit(): void {
        this.dtOptions = {
          pagingType: 'full_numbers',
          pageLength: 2
        };
        this.httpClient.get<Beach[]>('http://localhost:58124/api/search/places?prefix=B')
          .subscribe(b => {
            this.beaches = b;
            this.dtTrigger.next();
          });
      }
    
      ngOnDestroy(): void {
        this.dtTrigger.unsubscribe();
      }
}