import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';

import { AppComponent } from './app.component';
import { MakequestionComponent } from './makequestion/makequestion.component';
import {router } from './app.route';
import { ListcomponentComponent } from './listcomponent/listcomponent.component';
import { ListdetailComponent } from './listcomponent/listdetail/listdetail.component';
import { AnswerComponent } from './listcomponent/listdetail/answer/answer.component';


@NgModule({
  declarations: [
    AppComponent,
    MakequestionComponent,
    ListcomponentComponent,
    ListdetailComponent,
    AnswerComponent

  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    router
    
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
