import { Component, OnInit } from '@angular/core';
import{NgForm} from'@angular/forms'
import{Myquestion}from"../myquestion"
import{QuestionarrayService} from "../questionarray.service";
@Component({
  selector: 'app-makequestion',
  templateUrl: './makequestion.component.html',
  styleUrls: ['./makequestion.component.css'],
  
})
export class MakequestionComponent {
  myQuestionArray:Myquestion[]=[];
  
    title:string="";
    description:string="";
    answer:string="";
    constructor(private questionarrayservice:QuestionarrayService )
    {

    }

  make(form:NgForm)
  {
  this. title=form.value.title;
  this. description=form.value.description;
  this.questionarrayservice.store(this.title,this.description,this.answer);
  }
  
}

