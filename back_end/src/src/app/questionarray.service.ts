import { Injectable } from '@angular/core';
import{Myquestion}from"./myquestion"

@Injectable()
export class QuestionarrayService {
myQuestionArray:Myquestion[]=[];
ans= [
   {index:-1,answer:[],count:0}
   ];
   i:number;
   flag:number;
   flag1:number=0;
   count1:number=0;
   ans1=[];
   count:number;

store(title:string, description:string,answer:string)
{
this.myQuestionArray.push( new Myquestion(title,description,answer));
  
}
 get(){
   
   return this.myQuestionArray;
 }

 getAnswer( id:number,answer1:string)
 {
for(this.i=0;this.i<this.ans.length;this.i++)
  {
   if(this.ans[this.i].index===id)
   {
 this.ans[this.i].answer.push(answer1);
 this.ans[this.i].count= this.ans[this.i].count+1
 
   this.flag=1;
   }
  }
  
    if(this.flag==0)
    {
 this.ans.push({index:id,answer:[answer1],count:1});
    }
 this.flag=0;
//alert(this.ans[this.i].count);
 }

 returnanswer(index1:number)
 {
 this.ans1=[];
for(this.i=0;this.i<this.ans.length;this.i++)
  {
   if(this.ans[this.i].index===index1)
   {
 
 this.ans1.push(this.ans[this.i].answer);
  }
  }

  return this.ans1;
 }

getCount(id:number)
 {
this.count=0;
  this.flag1=0;
for(this.i=0;this.i<this.ans.length;this.i++)
  {
   if(this.ans[this.i].index===id)
   {
  this.count=this.ans[this.i].count;
this.flag1=1;
return this.count;
   }
  }
  if(this.flag1==0)
  {
return this.count;
  }

 }

}
