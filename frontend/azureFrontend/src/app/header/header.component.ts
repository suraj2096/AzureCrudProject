import { Component,OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LoginServiceService } from '../Service/login-service.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  constructor(public loginService:LoginServiceService,private router:Router){}
  ngOnInit(): void {
    console.log("hello");
    try{

 
      const checkTokenExist = localStorage.getItem('userData')!=null?JSON.parse(localStorage.getItem('userData')?.toString()??""):"";
      if(checkTokenExist!=""){
        this.loginService.loginUser = true;
      }
      else{
        this.loginService.loginUser = false;
      }
    }
    catch(err){
      this.router.navigate(['/login']);
    }   
  }
  userLogout(){
    this.loginService.loginUser = false;
    localStorage.clear();
    this.router.navigateByUrl("/login");
  }
 
}
