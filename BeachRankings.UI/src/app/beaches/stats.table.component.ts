import { Component, OnInit, OnDestroy, Input, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Subject } from 'rxjs';
import { Beach } from '../beaches/beach';
import { environment } from '../../environments/environment'
import { DataTableDirective } from 'angular-datatables';

@Component({
  templateUrl: 'stats.table.component.html',
  selector: 'stats-table'
})
export class StatsTableComponent implements OnInit, OnDestroy {
  beaches: Beach[];
  options: DataTables.Settings;
  trigger: Subject<Beach>;
  @ViewChild(DataTableDirective, {static: false})
  dtElement: DataTableDirective;

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
  }

  @Input()
  set searchQuery(query: any) {
    if (!query) {
      return;
    }
    
    this.httpClient.get<Beach[]>(environment.searchBeachesEndpoint + this.buildQuery(query))
      .subscribe(beaches => {
        this.beaches = beaches;
          
        this.trigger.next();
      });
  }

  ngOnDestroy(): void {
    this.trigger.unsubscribe();
  }

  buildQuery(query: any): any {
    let searchQuery = `continent=${query.continent}`;

    return searchQuery;
  }
}