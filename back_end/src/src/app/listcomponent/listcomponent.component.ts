import { Component, OnInit } from '@angular/core';
import{Myquestion}from"../myquestion"
import{QuestionarrayService} from "../questionarray.service";
@Component({
  selector: 'app-listcomponent',
  templateUrl: './listcomponent.component.html',
  styleUrls: ['./listcomponent.component.css'],
  
})
export class ListcomponentComponent 
  {
myQuestionArray:Myquestion[];
i:number=-1;
constructor(public questionarray:QuestionarrayService){

this.myQuestionArray=this.questionarray.get();
console.log(this.myQuestionArray);
}
  }
  