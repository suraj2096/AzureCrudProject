import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Login } from '../Model/login';
@Injectable({
  providedIn: 'root'
})
export class LoginServiceService {

  constructor(private httpClient:HttpClient) { }
  loginUser:boolean=false;

  validateUser(userData:Login):Observable<any>{
      return this.httpClient.get(`http://localhost:35033/login/${userData.UserName}/${userData.Password}`);
  }
}
