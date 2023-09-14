import { Component,OnInit } from '@angular/core';
import { CrudTableServiceService } from '../Service/crud-table-service.service';
import { UploadSeviceService } from '../Service/upload-sevice.service';
import { saveAs } from 'file-saver';
import { FileData } from '../Model/file-data';
import { DatePipe } from '@angular/common';
import { HubConnectionBuilder,LogLevel,HubConnection,HttpTransportType } from '@microsoft/signalr';


@Component({
  selector: 'app-data-display',
  templateUrl: './data-display.component.html',
  styleUrls: ['./data-display.component.css']
})
export class DataDisplayComponent implements OnInit  {
  constructor(private curdTableService:CrudTableServiceService,private uploadService:UploadSeviceService ,public datePipe:DatePipe){

  }
  GetData:any=[];
  private hubConnectionBuilder!:HubConnection;
  updateData:any;
  ngOnInit(): void {
    debugger
    this.getAll();
    this.updateData = new FileData();
    let getUserId = JSON.parse(localStorage.getItem("userData")??"null")
    if(getUserId == "null") return;
   //here we will write the code for signal r in which it will get all the data without sending the request to the server .
    this.hubConnectionBuilder = new HubConnectionBuilder().withUrl("http://localhost:35033/getDataSignal",{
      skipNegotiation: true,
      transport: HttpTransportType.WebSockets
    }).build();
    this.hubConnectionBuilder.start().then(()=>{console.log("Connection Started")}).catch((err)=>{console.log(`something went wrong ${err}`)});
    this.hubConnectionBuilder.on('SendMessage',(result:any)=>{
      if(result[0].userId !=getUserId.id ) return;
      result.filter((ele:any)=>{
        let date = new Date(ele.fileCreated);
        ele.fileCreated = date.toDateString();
      });
      this.GetData = result; 
    });
  }

  getAll(){
    debugger
    return this.curdTableService.getAllData().subscribe(
      (response)=>{
        response.filter((ele:any)=>{
          var date = new Date(ele.fileCreated);
          ele.fileCreated = date.toDateString();
        });
        this.GetData = response;
      },(err)=>{
         console.log(err);
      }
    )
    
  }


  downloadImage(imagename:any,imageExtension:any){
    debugger
    var imaeExtensions = `.${imageExtension}`;
       this.uploadService.DownloadData(imagename,imaeExtensions).subscribe((result)=>{
        debugger
        console.log(result);
        if (result.type != 'text/plain') {
          var blob = new Blob([result]);
          let file = imagename+imaeExtensions;
          saveAs(blob,file);
        }
        else {
          alert('File not found in Blob!');
        }
       },(err)=>{
        console.log(err);
      
       });
  }


  editButton(data:any){
    debugger
    let myDate = new Date(); 
    this.updateData.ImageExtension = data.imageExtension;
    this.updateData.ImageName = data.imageName;
    this.updateData.PartitionKey = data.partitionKey;
    this.updateData.RowKey = data.rowKey;
    this.updateData.ETag = data.eTag;
    this.updateData.publish = this.datePipe.transform(myDate,'MM/dd/yyyy');
    this.updateData.UserId = data.userId;
  }

  updateButton(){
    this.curdTableService.updateData(this.updateData).subscribe((data)=>{
   alert(data.message);
   this.getAll();
    },(err)=>{
     console.log(err);
    });
  };

  deleteData(data:any){
   this.curdTableService.deleteData(data).subscribe((data)=>{
        alert(data.message);
        this.getAll();
   },(err)=>{
       console.log(err);
   })
  }



}
