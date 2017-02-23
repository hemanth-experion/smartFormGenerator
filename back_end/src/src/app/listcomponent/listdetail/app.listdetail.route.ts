import {Routes,RouterModule} from "@angular/router";
import{AnswerComponent}from'./answer/answer.component';
export const ANSWER:Routes=[
    {path:'answer/:id',component:AnswerComponent},
];

