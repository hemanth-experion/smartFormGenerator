import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { FormgeneratorService } from '../formgenerator.service';
import { User } from '../user';
import { Router } from '@angular/router';


@Component({
  selector: 'sfg-userlogin',
  templateUrl: './userlogin.component.html',
  styleUrls: ['./userlogin.component.css']
})
export class UserloginComponent implements OnInit {

  constructor(private formGenerator: FormgeneratorService, private router: Router) { }

  ngOnInit() {
  }
  onSubmit(f: NgForm) {
    this.formGenerator.userLoginCheck({ UserName: f.value.lg_username, Password: f.value.lg_password }).subscribe(data => {
      var valid = data.Data.IsValid;
      if (!valid) {
        document.getElementById("errorMessage").textContent = "Incorrect username or password!!!";
      }
      else {
        localStorage.setItem("userid",data.Data.Id);
        this.router.navigate(['/home/create']);
      }
    });
  }

}
