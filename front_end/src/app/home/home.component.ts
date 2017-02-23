import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'sfg-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  constructor(private router: Router) { }

  ngOnInit() {
    var status = localStorage.getItem('userid');
    if (status == "") {
      this.router.navigate(['']);
    }
  }

  clearStorage() {
    localStorage.setItem("userid", "");
    this.router.navigate(['login']);

  }

}
