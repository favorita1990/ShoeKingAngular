import { UserService } from './../services/user/user.service';
import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { HomeService } from '../services/home/home.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  homePage: any;
  newArrivals: any;
  // discounts: any;
  changeTextHeaderBtn: boolean;
  changeTextBtn: boolean;
  userClaims: any;

  constructor(private homeService: HomeService, private userService: UserService) { }

  ngOnInit() {
    this.homeService.GetHomePage().subscribe((data: any) => {
      this.homePage = data;
    });

    this.homeService.GetNewArrivals().subscribe((data: any) => {
      if(data.length > 0){
        this.newArrivals = data;
      }
    });

    // this.homeService.GetMostBought().subscribe((data: any) => {
    //   if(data.length > 0){
    //     this.discounts = data;
    //   }
    // });

    this.changeTextHeaderBtn = false;
    this.changeTextBtn = false;

    this.userService.getUserClaims().subscribe((data: any) => {
      this.userClaims = data;
    });
  }

  changeImgBtn(e){
    e.preventDefault();
  }
  
  changePictureBanner(event){
    var fileToUpload = event.srcElement.files[0];
    
    const $allow = new Array("image/gif", "image/x-png", "image/jpg", "image/jpeg");

    let checkAllow = false;
    $allow.forEach(element => {
      if(element === fileToUpload.type){
        checkAllow = true;
      }
    });

    if(!checkAllow){
      alert('Image file has to be to this format --> { gif, png, jpg, jpeg } <--!');
    }
    else{
      this.homeService.changeImgHome(fileToUpload).subscribe((resultImg: any) => {
        if(resultImg){
          this.homeService.GetHomePage().subscribe((data: any) => {
            this.homePage = data;
          });
        }
        else{
          alert('Something\'s wrong!');
        }
      });
    }
  }

  deleteImgBtn(event){
    event.preventDefault();
    this.homeService.deleteImgHome().subscribe((resultImg: any) => {
        if(resultImg){
          location.reload();
        }
        else{
          alert('Something\'s wrong!');
        }
    });
  }

  changeTextHeader(){
    this.changeTextHeaderBtn = true;
  }

  @ViewChild('textHeader') textHeader: ElementRef;
  
  okChangeTextHeader(){
    this.changeTextHeaderBtn = false;

    this.homeService.editHomeTextHeader(this.textHeader.nativeElement.textContent)
    .subscribe((resultEdit: boolean) => {
      if(resultEdit){
        this.homeService.GetHomePage().subscribe((data: any) => {
          this.homePage = data;
          return;
        });
      }
      else{
        alert('Something\'s wrong!');
      }
    });
  }

  cancelTextHeader(){
    this.changeTextHeaderBtn = false;
    this.textHeader.nativeElement.textContent = this.homePage.TextHeader;
  }

  changeText(){
    this.changeTextBtn = true;
  }

  @ViewChild('text') text: ElementRef;
  
  okChangeText(){
    this.changeTextBtn = false;

    this.homeService.editHomeText(this.text.nativeElement.textContent)
    .subscribe((resultEdit: boolean) => {
      if(resultEdit){
        this.homeService.GetHomePage().subscribe((data: any) => {
          this.homePage = data;
          return;
        });
      }
      else{
        alert('Something\'s wrong!');
      }
    });
  }

  cancelText(){
    this.changeTextBtn = false;
    this.text.nativeElement.textContent = this.homePage.Text;
  }
}
