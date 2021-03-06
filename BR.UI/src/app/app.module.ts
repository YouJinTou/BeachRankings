import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule, Routes } from '@angular/router';

import { DataTablesModule } from 'angular-datatables';

import { AppComponent } from './app.component';
import { CreateBeachComponent } from './components/create-beach/create-beach.component';
import { LoginComponent } from './components/login/login.component';
import { CookieService } from 'ngx-cookie-service';
import { HeaderComponent } from './components/header/header.component';
import { AuthGuardService } from './services/auth-guard.service';
import { ViewBeachComponent } from './components/view-beach/view-beach.component';
import { SearchBarComponent } from './components/search-bar/search-bar.component';
import { PlaceTableComponent } from './components/place-table/place-table.component';
import { RegisterComponent } from './components/register/register.component';
import { AddReviewComponent } from './components/add-review/add-review.component';
import { EditReviewComponent } from './components/edit-review/edit-review.component';
import { EditBeachComponent } from './components/edit-beach/edit-beach.component';

const appRoutes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'beaches/new', component: CreateBeachComponent, canActivate: [AuthGuardService] },
  { path: 'beaches/:id', component: ViewBeachComponent },
  { path: 'beaches/:id/edit', component: CreateBeachComponent, canActivate: [AuthGuardService] },
  { path: 'beaches/:id/review', component: AddReviewComponent, canActivate: [AuthGuardService] },
  { path: 'reviews/:id/edit', component: EditReviewComponent, canActivate: [AuthGuardService] },
  { path: 'places/:id', component: PlaceTableComponent }
]

@NgModule({
  declarations: [
    AppComponent,
    CreateBeachComponent,
    LoginComponent,
    HeaderComponent,
    ViewBeachComponent,
    SearchBarComponent,
    PlaceTableComponent,
    RegisterComponent,
    AddReviewComponent,
    EditReviewComponent,
    EditBeachComponent
  ],
  imports: [
    BrowserModule,
    RouterModule.forRoot(appRoutes, {
      enableTracing: false
    }),
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    DataTablesModule,
  ],
  providers: [CookieService],
  bootstrap: [AppComponent]
})
export class AppModule { }
