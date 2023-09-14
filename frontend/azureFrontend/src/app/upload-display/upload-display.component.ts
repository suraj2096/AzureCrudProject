import { Component } from '@angular/core';
import { UploadSeviceService } from '../Service/upload-sevice.service';

@Component({
  selector: 'app-upload-display',
  templateUrl: './upload-display.component.html',
  styleUrls: ['./upload-display.component.css']
})
export class UploadDisplayComponent {
 File:FormData = new FormData();
  constructor(private uploadService: UploadSeviceService){

  }

  setImage(event:any){
    
    this.File.append('file_upload', event.srcElement.files[0]);
  
  
  }

  uploadImage(){
   
   this.uploadService.uploadData(this.File).subscribe({
    next:(data)=>{
      console.log(data);
    },
    error:(err)=>{
      //doubt in this 
     if(err.status == 200){
      alert("Uploaded Successfully");
     }
     else{
      alert("Something went wrong");
     }
    }
   })
  }


}
