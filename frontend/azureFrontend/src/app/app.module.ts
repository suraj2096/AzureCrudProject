import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule} from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { UploadDisplayComponent } from './upload-display/upload-display.component';
import { DataDisplayComponent } from './data-display/data-display.component';
import { FormsModule } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { HeaderComponent } from './header/header.component';
import { LoginComponent } from './login/login.component';
import { HomeComponent } from './home/home.component';


@NgModule({
  declarations: [
    AppComponent,
    UploadDisplayComponent,
    DataDisplayComponent,
    HeaderComponent,
    LoginComponent,
    HomeComponent,

  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
 
  ],
  providers: [DatePipe],
  bootstrap: [AppComponent]
})
export class AppModule { }
