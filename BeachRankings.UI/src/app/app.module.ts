import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule, Routes } from '@angular/router';

import { DataTablesModule } from 'angular-datatables';

import { AppComponent } from './app.component';
import { SearchBarComponent } from './search/search.bar.component';
import { StatsTableComponent } from './beaches/stats/stats.table.component';
import { BeachDetailsComponent } from './beaches/details/beach.details.component';

const appRoutes: Routes = [
  { path: 'beaches/:id/details', component: BeachDetailsComponent }
]

@NgModule({
  declarations: [
    AppComponent,
    SearchBarComponent,
    StatsTableComponent,
    BeachDetailsComponent
  ],
  imports: [
    BrowserModule,
    RouterModule.forRoot(appRoutes, {
      enableTracing: true
    }),
    HttpClientModule,
    DataTablesModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
