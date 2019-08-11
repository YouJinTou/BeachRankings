import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule, Routes } from '@angular/router';

import { DataTablesModule } from 'angular-datatables';

import { AppComponent } from './app.component';
import { SearchBarComponent } from './search/search.bar.component';
import { BeachComponent } from './beach/beach.component';
import { PlaceTableComponent } from './place/place.table.component';

const appRoutes: Routes = [
  { path: 'continents/:name', component: PlaceTableComponent },
  { path: 'countries/:name', component: PlaceTableComponent },
  { path: 'l1s/:name', component: PlaceTableComponent },
  { path: 'l2s/:name', component: PlaceTableComponent },
  { path: 'l3s/:name', component: PlaceTableComponent },
  { path: 'l4s/:name', component: PlaceTableComponent },
  { path: 'waterbodies/:name', component: PlaceTableComponent },
  { path: 'beaches/:id', component: BeachComponent }
]

@NgModule({
  declarations: [
    AppComponent,
    SearchBarComponent,
    BeachComponent,
    PlaceTableComponent
  ],
  imports: [
    BrowserModule,
    RouterModule.forRoot(appRoutes, {
      enableTracing: false
    }),
    HttpClientModule,
    DataTablesModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
