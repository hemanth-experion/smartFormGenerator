import {Routes, RouterModule} from '@angular/router';
import {HomeComponent} from './home/home.component';
import {UserloginComponent} from './userlogin/userlogin.component';
import {HOME_ROUTES} from './home/home.routes';
import {VIEW_ROUTES} from './view/view.routes';
import {ViewComponent} from './view/view.component';

const APP_ROUTES: Routes = [
  { path: '', component: UserloginComponent },
  { path: 'login', component: UserloginComponent },
  { path: 'home', component: HomeComponent },
  { path: 'home', component: HomeComponent, children: HOME_ROUTES }
];
export const routing = RouterModule.forRoot(APP_ROUTES);
