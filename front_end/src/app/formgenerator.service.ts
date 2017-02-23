import { Injectable } from '@angular/core';
import { Http, Response, Headers } from '@angular/http';
import { Observable, } from 'rxjs/Rx';
import 'rxjs/add/operator/map';

@Injectable()
export class FormgeneratorService {
  constructor(private http: Http) { }

  //check for valid user
  userLoginCheck(user: any) {
    const myJSON = JSON.stringify(user);
    const headers = new Headers();
    headers.append('Content-Type', 'application/json');
    return this.http.post("http://localhost:11680/login", myJSON, { headers: headers }).map((response: Response) => response.json());
  }

//store the user specification about the template
  createTemplate(template: any) {
    const myJSON = JSON.stringify(template);
    const headers = new Headers();
    headers.append('Content-Type', 'application/json');
    return this.http.post("http://localhost:11680/createTemplate", myJSON, { headers: headers }).map((response: Response) => response.json());
  }

//store the template name
  createTemplateName(template: any) {
    const myJSON = JSON.stringify(template);
    const headers = new Headers();
    headers.append('Content-Type', 'application/json');
    return this.http.post("http://localhost:11680/addTemplateName", myJSON, { headers: headers }).map((response: Response) => response.json());
  }

  getTemplateNames(userId: any) {
    return this.http.get("http://localhost:11680/templateName/" + userId).map((response: Response) => response.json());
  }

  getTemplate(templateId: any) {
    return this.http.get("http://localhost:11680/template/" + templateId).map((response: Response) => response.json());
  }

}
