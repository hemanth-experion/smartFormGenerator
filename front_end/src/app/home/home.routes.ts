import {Routes} from '@angular/router';
import {ViewComponent} from '../view/view.component';
import {CreateComponent} from '../create/create.component';


export const HOME_ROUTES: Routes = [
  {path:'view', component:ViewComponent},
  {path:'create',component:CreateComponent},
];
