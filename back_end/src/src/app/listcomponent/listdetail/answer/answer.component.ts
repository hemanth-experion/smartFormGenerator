
import { Component, OnDestroy} from '@angular/core';
import{Myquestion}from"../../../myquestion"
import{QuestionarrayService} from "../../../questionarray.service";
import{ Router,ActivatedRoute} from'@angular/router';
import{ Subscription } from "rxjs/Rx"


@Component({
  selector: 'app-answer',
  templateUrl: './answer.component.html',
  styleUrls: ['./answer.component.css']
})
export class AnswerComponent {
 id:number;
 i:number;
 flag:number=0;
 
 private subscription: Subscription;

 constructor(public questionarray:QuestionarrayService,private router:Router,private activatedRoute:ActivatedRoute,){
 }
 ngOnInit(){
     this.subscription=this.activatedRoute.params.subscribe(
  (param: any)=> {
    this.id=param['id'];
 }

);

}
  answerpush(answer1:string)
  {
  
this.questionarray. getAnswer(this.id,answer1);

}
}