import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule, Routes } from '@angular/router';

import { AppComponent } from './app.component';
import { CreateBeachComponent } from './components/create-beach/create-beach.component';
import { LoginComponent } from './components/login/login.component';
import { CookieService } from 'ngx-cookie-service';
import { HeaderComponent } from './components/header/header.component';
import { AuthGuardService } from './services/auth-guard.service';
import { ViewBeachComponent } from './components/view-beach/view-beach.component';
import { SearchBarComponent } from './components/search-bar/search-bar.component';

const appRoutes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'add-beach', component: CreateBeachComponent, canActivate: [AuthGuardService] },
  { path: 'beaches/:id', component: ViewBeachComponent }
]

@NgModule({
  declarations: [
    AppComponent,
    CreateBeachComponent,
    LoginComponent,
    HeaderComponent,
    ViewBeachComponent,
    SearchBarComponent
  ],
  imports: [
    BrowserModule,
    RouterModule.forRoot(appRoutes, {
      enableTracing: false
    }),
    FormsModule,
    HttpClientModule
  ],
  providers: [CookieService],
  bootstrap: [AppComponent]
})
export class AppModule { }
