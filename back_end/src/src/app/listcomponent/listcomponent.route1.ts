import {Routes,RouterModule} from "@angular/router";
import{ListdetailComponent}from'./listdetail/listdetail.component';
import{ANSWER} from'./listdetail/app.listdetail.route';

export const DETAILROUTES:Routes=[
    {path:'detail/:i',component:ListdetailComponent},
    {path:'detail/:i',component:ListdetailComponent,children:ANSWER}

];

