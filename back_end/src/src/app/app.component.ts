import { Component } from '@angular/core';
import{QuestionarrayService} from "./questionarray.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers:[QuestionarrayService]
})
export class AppComponent {
 
}
