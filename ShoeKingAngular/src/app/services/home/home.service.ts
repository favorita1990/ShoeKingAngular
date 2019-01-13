import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable }     from 'rxjs/Observable';

@Injectable({
  providedIn: 'root'
})
export class HomeService {
  readonly rootUrl = 'http://localhost:56445';

  constructor(private http: HttpClient) { }

  GetHomePage() {
    var reqHeader = new HttpHeaders({'No-Auth':'True'});
    return this.http.get(this.rootUrl + '/api/GetHomePage', {headers : reqHeader});
  }

  GetNewArrivals() {
    var reqHeader = new HttpHeaders({'No-Auth':'True'});
    return this.http.get(this.rootUrl + '/api/GetNewArrivals', {headers : reqHeader});
  }

  GetMostBought() {
    var reqHeader = new HttpHeaders({'No-Auth':'True'});
    return this.http.get(this.rootUrl + '/api/GetMostBought', {headers : reqHeader});
  }

  isValidEmail(email){
    let data = '?source=' + email;
    var reqHeader = new HttpHeaders({'No-Auth':'True'});
    return this.http.get(this.rootUrl + '/api/IsValidEmail' + data, {headers : reqHeader});
  }

  sendEmail(name, email, subject, text){
    var data = '?name=' + name + '&email=' + email + '&subject=' + subject + '&text=' + text;
    var reqHeader = new HttpHeaders({ 'No-Auth':'True' });
    return this.http.get(this.rootUrl + '/api/SendEmail' + data, {headers : reqHeader});
  }

  getAboutUs(){
    var reqHeader = new HttpHeaders({ 'No-Auth':'True' });
    return this.http.get(this.rootUrl + '/api/About', {headers : reqHeader});
  }

  editPictureFirst(file: any){
    var reqHeader = new HttpHeaders({ 'No-Auth':'False'});
    var formData = new FormData();
    formData.append('pictureFirst', file);
    // var xhr = new XMLHttpRequest();

    // xhr.upload.addEventListener("progress", (ev: ProgressEvent) => {
    //     //You can handle progress events here if you want to track upload progress (I used an observable<number> to fire back updates to whomever called this upload)
    // });
    // xhr.open("POST", this.rootUrl + '/api/EditPictureFirst', true);
    // xhr.send(formData);
    return this.http.post(this.rootUrl + '/api/EditPictureFirst', formData, {headers : reqHeader});
  }
  
  deletePictureFirst(){
    var reqHeader = new HttpHeaders({ 'No-Auth':'False'});
    return this.http.post(this.rootUrl + '/api/DeletePictureFirst', {headers : reqHeader});
  }

  EditWhoFirst(textHeader, textBody){
    var data = '?textHeader=' + textHeader + '&textBody=' + textBody;
    var reqHeader = new HttpHeaders({ 'No-Auth':'True' });
    return this.http.post(this.rootUrl + '/api/EditWhoFirst' + data, {headers : reqHeader});
  }

  editPictureSecond(file: any){
    var reqHeader = new HttpHeaders({ 'No-Auth':'False'});
    var formData = new FormData();
    formData.append('pictureSecond', file);
    return this.http.post(this.rootUrl + '/api/EditPictureSecond', formData, {headers : reqHeader});
  }
  
  deletePictureSecond(){
    var reqHeader = new HttpHeaders({ 'No-Auth':'False'});
    return this.http.post(this.rootUrl + '/api/DeletePictureSecond', {headers : reqHeader});
  }

  EditWhoSecond(textHeader, textBody){
    var data = '?textHeader=' + textHeader + '&textBody=' + textBody;
    var reqHeader = new HttpHeaders({ 'No-Auth':'True' });
    return this.http.post(this.rootUrl + '/api/EditWhoSecond' + data, {headers : reqHeader});
  }

  changeImgHome(file: any){
    var reqHeader = new HttpHeaders({ 'No-Auth':'False'});
    var formData = new FormData();
    formData.append('homeChangeImg', file);
    return this.http.post(this.rootUrl + '/api/EditHomePagePicture', formData, {headers : reqHeader});
  }

  deleteImgHome(){
    var reqHeader = new HttpHeaders({ 'No-Auth':'False'});
    return this.http.post(this.rootUrl + '/api/DeleteHomePageImg', {headers : reqHeader});
  }

  editHomeTextHeader(textHeader){
    var data = '?textHeader=' + textHeader;
    var reqHeader = new HttpHeaders({ 'No-Auth':'True' });
    return this.http.post(this.rootUrl + '/api/EditHomeTextHeader' + data, {headers : reqHeader});
  }

  editHomeText(text){
    var data = '?text=' + text;
    var reqHeader = new HttpHeaders({ 'No-Auth':'True' });
    return this.http.post(this.rootUrl + '/api/EditHomeText' + data, {headers : reqHeader});
  }
}
