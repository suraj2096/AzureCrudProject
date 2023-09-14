import { Component } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  DisplayUpload = false;
  FirstLoad= false;

  uploadShow(){
    this.FirstLoad = false;
   this.DisplayUpload = true;
   return ;
  }
  displayShow(){
    this.FirstLoad = true;
    this.DisplayUpload = false;
    return;
  }
}
