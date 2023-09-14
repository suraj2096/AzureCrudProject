import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';



@Injectable({
  providedIn: 'root'
})
export class UploadSeviceService {
data:any;
  constructor(private httpClient :HttpClient,private router:Router) { 
   this.data=JSON.parse(localStorage.getItem("userData")??"null");
   if(this.data == "null"){
    router.navigateByUrl("/login");
   }
  }

 

  uploadData(formdata:FormData):Observable<any>{
    return this.httpClient.post<any>(`http://localhost:7266/api/FileUpload`,formdata,{headers:{userId:this.data.id}});
  }

  DownloadData(fileName:string,imageExtension:any):Observable<any>{
   debugger

   var encodedData = encodeURIComponent(fileName+imageExtension);
  // here blob is used to support file on the user system.
   return this.httpClient.get<Blob>(`http://localhost:7266/api/DownloadImage/${encodedData}`,{responseType: 'blob' as 'json'});
  }
}
