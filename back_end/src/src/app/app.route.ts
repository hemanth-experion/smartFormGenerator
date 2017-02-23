import {Routes,RouterModule} from "@angular/router";
import { MakequestionComponent } from './makequestion/makequestion.component';
import{ListcomponentComponent}from'./listcomponent/listcomponent.component';
import{DETAILROUTES} from'./listcomponent/listcomponent.route1';
const APPROUTES:Routes=[{path:'makequestion',component:MakequestionComponent},
{path:'makequestion',component:MakequestionComponent},
{path:'listcomponent',component:ListcomponentComponent,children:DETAILROUTES}];
export const router=RouterModule.forRoot(APPROUTES);
