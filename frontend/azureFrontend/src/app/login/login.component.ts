import { Component } from '@angular/core';
import { Login } from '../Model/login';
import { LoginServiceService } from '../Service/login-service.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
 loginData:any;
 constructor(private loginService:LoginServiceService ,private route:Router){
  this.loginData = new Login();
 }

 loginUser(){
  this.loginService.validateUser(this.loginData).subscribe((data)=>{
    alert(data.message);
    // now we will store this in owr localstorage.
    localStorage.setItem("userData",JSON.stringify(data.data));
   // now we will navigate the user to the hone page .
     this.loginService.loginUser = true;
     this.route.navigateByUrl("home");
  },(err)=>{
      alert(err.message);
  })
 }
}
