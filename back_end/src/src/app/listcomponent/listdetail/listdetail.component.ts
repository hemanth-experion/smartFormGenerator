import { Component, OnDestroy} from '@angular/core';
import{Myquestion}from"../../myquestion"
import{QuestionarrayService} from "../../questionarray.service";
import{ Router,ActivatedRoute} from'@angular/router';
import{ Subscription } from "rxjs/Rx"
@Component({
  selector: 'app-listdetail',
  templateUrl: './listdetail.component.html',
  styleUrls: ['./listdetail.component.css']
})
export class ListdetailComponent implements OnDestroy {
myQuestionArray:Myquestion[]=[];
private subscription: Subscription;
index:number;
title:string="";
description:string="";
count:number;
ans= [
   {index:0,answer:[],count:0}
   ];
    ans1=[]=[];
  constructor(public questionarray:QuestionarrayService,private router:Router,private activatedRoute:ActivatedRoute){


}

ngOnInit(){
  this.myQuestionArray=this.questionarray.get();
this.subscription=this.activatedRoute.params.subscribe(
  (param: any)=> {
    this.index=param['i'];
    this.title=this.myQuestionArray[this.index].title;
    this.description=this.myQuestionArray[this.index].description;
    this.ans1 =this. questionarray.returnanswer(this.index);
  console.log(this.ans1);
  this.count=this.questionarray.getCount(this.index);
  
  }

);

}

ngOnDestroy(){
  this.subscription.unsubscribe();
}
}
