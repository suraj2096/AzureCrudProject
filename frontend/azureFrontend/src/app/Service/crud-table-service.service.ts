import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { FileData } from '../Model/file-data';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class CrudTableServiceService {
  
  constructor(private httpClient :HttpClient,private router:Router) { 
 
  }




getAllData():Observable<any>{
  let data;
  data=JSON.parse(localStorage.getItem("userData")??"null");
  if(data == "null"){
   this.router.navigateByUrl("/login");
  }
return this.httpClient.get(`http://localhost:35033/getAllData/${data.id}`);
}



updateData(data:FileData):Observable<any>{
  return  this.httpClient.put("http://localhost:35033/UpdateData",data);
}


deleteData(data:any):Observable<any>{
return this.httpClient.delete(`http://localhost:35033/DeleteData/${data.partitionKey}/${data.rowKey}`);
}

}
