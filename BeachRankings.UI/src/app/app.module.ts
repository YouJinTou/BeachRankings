import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

import { DataTablesModule } from 'angular-datatables';

import { AppComponent } from './app.component';
import { SearchBarComponent } from './search/search.bar.component';
import { StatsTableComponent } from './beaches/stats.table.component';

@NgModule({
  declarations: [
    AppComponent,
    SearchBarComponent,
    StatsTableComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    DataTablesModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
