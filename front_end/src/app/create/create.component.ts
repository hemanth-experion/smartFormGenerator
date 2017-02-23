import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { FormgeneratorService } from '../formgenerator.service';



@Component({
  selector: 'sfg-createtemplate',
  templateUrl: './create.component.html',
  styles: []
})
export class CreateComponent implements OnInit{
  orders: Number[] = [1, 2];
  fields: String[] = ['0', '1'];
  tempArray: Number[] = [];
  status: Number = 0;
  name: String = "";
  templateId: Number = -1;
  constructor(private formGenerator: FormgeneratorService, private router: Router) {
  }

  //check for user is logged in or not
  ngOnInit() {
    var status = localStorage.getItem('userid');
    if (status == "") {
      this.router.navigate(['']);
    }
  }

  //check if order field is unique
  orderChange(i) {
    if (this.tempArray.indexOf(i) != -1) {
      alert("Enter a unique order");
    }
    else {
      this.tempArray.push(i);
    }
  }

  onSubmit(f) {
    var uid = localStorage.getItem("userid");
    this.formGenerator.createTemplateName({
      TemplateName: f.tname,
      UserId: uid
    }).subscribe(data => {
      if (data) {
        for (let field in this.fields) {
          this.formGenerator.createTemplate({
            TemplateName: f.tname,
            UserId: uid,
            Label: f[field].label,
            ControlType: f[field].controlType,
            IsReadOnly: f[field].isReadOnly,
            IsVisible: f[field].isVisible,
            Order: f[field].order
          }).subscribe(data => {
            if (data.Data.IsValid) {
              this.router.navigate(['home/view']);

            }

          });
        }

      }
    });
  }
}
