import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';

import { FormgeneratorService } from './formgenerator.service';
import { AppComponent } from './app.component';
import { UserloginComponent } from './userlogin/userlogin.component';
import { HomeComponent } from './home/home.component';
import { ViewComponent } from './view/view.component';
import { CreateComponent } from './create/create.component';
import { ModalModule } from 'ng2-bootstrap';
import {routing} from './app.routing';
import {VisibilityPipe} from './visibility.pipe';

@NgModule({
  declarations: [
    AppComponent,
    UserloginComponent,
    HomeComponent,
    ViewComponent,
    CreateComponent,
    VisibilityPipe,

  ],
  imports: [
    routing,
    BrowserModule,
    FormsModule,
    HttpModule,
    ModalModule.forRoot()
  ],
  providers: [FormgeneratorService],
  bootstrap: [AppComponent]
})
export class AppModule { }
