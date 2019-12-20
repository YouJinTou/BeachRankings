import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { CreateBeachComponent } from './components/create-beach/create-beach.component';

@NgModule({
  declarations: [
    AppComponent,
    CreateBeachComponent
  ],
  imports: [
    BrowserModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }