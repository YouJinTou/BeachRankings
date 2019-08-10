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
  dtOptions: DataTables.Settings;
  dtTrigger: Subject<Beach>;
  @ViewChild(DataTableDirective, { static: false })
  dtElement: DataTableDirective;

  constructor(private httpClient: HttpClient) {
    this.dtTrigger = new Subject();
  }

  ngOnInit(): void {
    this.dtOptions = {
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

        if (this.dtElement.dtInstance) {
          this.dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
            dtInstance.destroy();
          });
        }

        this.dtTrigger.next();
      });
  }

  ngOnDestroy(): void {
    this.dtTrigger.unsubscribe();
  }

  private buildQuery(query: any): any {
    let searchQuery = `continent=${query.continent}`;

    return searchQuery;
  }
}