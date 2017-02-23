import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormgeneratorService } from '../formgenerator.service';
import { VisibilityPipe } from '../visibility.pipe';


@Component({
  selector: 'sfg-formpreview',
  templateUrl: './view.component.html',
  styles: []
})
export class ViewComponent implements OnInit {
  templates: any[] = [];
  allTemplates: any[] = [];
  id: Number = 0;

  constructor(private formGenerator: FormgeneratorService, private router: Router) {
  }
  ngOnInit() {
    var userId = localStorage.getItem("userid");

    if (userId == "") {
      this.router.navigate(['']);
    }
    this.formGenerator.getTemplateNames(userId).subscribe(data => {
      for (let i in data.Data) {
        this.templates.push(data.Data[i]);
      }
    });
  }

  viewTemplate(f: any) {
    this.formGenerator.getTemplate(f).subscribe(data => {
      for (let i in data.Data) {
        this.allTemplates.push(data.Data[i]);
      }
    });

  }

}
