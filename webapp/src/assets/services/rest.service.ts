import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json',
    'Access-Control-Allow-Origin': '*',
  }),
};

@Injectable({ providedIn: 'root' })
export class RestService {
  lab: boolean = true;
  hostName: string = '';
  url: string = '';
  httpData: Object = {};
  isDevelopment: boolean = true;

  constructor(private http: HttpClient) {
    localStorage.setItem('targetHostName', 'uc.paytequtils.com');
    this.hostName =
      localStorage.getItem('targetHostName') || 'uc.paytequtils.com';

    if (this.isDevelopment) {
      this.url = `https://${this.hostName}:5116/api/Services/`;
    } else {
      this.url = `https://${this.hostName}/api/Services/`;
    }
  }

  // GET Request
  sendGetRequest(target: string) {
    return this.http.get(this.url + target, httpOptions);
  }

  // POST Request
  sendPostRequest(target: string, payload: Object) {
    return this.http.post(this.url + target, payload, httpOptions);
  }
}
