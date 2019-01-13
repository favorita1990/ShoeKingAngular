import { HomeService } from './../../services/home/home.service';
import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.css']
})
export class AboutComponent implements OnInit {
  about: any;
  changeTextBtnFirst: boolean;
  changeTextBtnSecond: boolean;
  userClaims: any;

  constructor(private homeService: HomeService, private userService: UserService) { }

  ngOnInit() {
    this.homeService.getAboutUs().subscribe((data: any) => {
      this.about = data;
    });

    this.userService.getUserClaims().subscribe((data: any) => {
      this.userClaims = data;
    });

    this.changeTextBtnFirst = false;
  }

  changePictureAboutFirst(event){
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
      this.homeService.editPictureFirst(fileToUpload).subscribe((resultImg: any) => {
        if(resultImg){
          this.homeService.getAboutUs().subscribe((data: any) => {
            this.about = data;
          });
        }
        else{
          alert('Something\'s wrong!');
        }
      });
    }
  }

  deletePicturefirst(){
    this.homeService.deletePictureFirst().subscribe((resultImg: any) => {
      if(resultImg){
        this.homeService.getAboutUs().subscribe((data: any) => {
          this.about = data;
          return;
        });
      }
      else{
        alert('Something\'s wrong!');
      }
    });
  }

  changeTextFirstPanel(){
    this.changeTextBtnFirst = true;
  }

  @ViewChild('textFirstHeader') textFirstHeader: ElementRef;
  @ViewChild('textFirst') textFirst: ElementRef;
  
  okTextFirstPanel(){
    this.changeTextBtnFirst = false;

    this.homeService.EditWhoFirst(this.textFirstHeader.nativeElement.textContent, this.textFirst.nativeElement.textContent)
    .subscribe((resultEdit: boolean) => {
      if(resultEdit){
        this.homeService.getAboutUs().subscribe((data: any) => {
          this.about = data;
          return;
        });
      }
      else{
        alert('Something\'s wrong!');
      }
    });
  }

  cancelTextFirstPanel(){
    this.changeTextBtnFirst = false;
  }

  changePictureAboutSecond(event){
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
      this.homeService.editPictureSecond(fileToUpload).subscribe((resultImg: any) => {
        if(resultImg){
          this.homeService.getAboutUs().subscribe((data: any) => {
            this.about = data;
          });
        }
        else{
          alert('Something\'s wrong!');
        }
      });
    }
  }

  deletePictureSecond(){
    this.homeService.deletePictureSecond().subscribe((resultImg: any) => {
      if(resultImg){
        this.homeService.getAboutUs().subscribe((data: any) => {
          this.about = data;
          return;
        });
      }
      else{
        alert('Something\'s wrong!');
      }
    });
  }

  changeTextSecondPanel(){
    this.changeTextBtnSecond = true;
  }

  @ViewChild('textSecondHeader') textSecondHeader: ElementRef;
  @ViewChild('textSecond') textSecond: ElementRef;
  
  okTextSecondPanel(){
    this.changeTextBtnSecond = false;

    this.homeService.EditWhoSecond(this.textSecondHeader.nativeElement.textContent, this.textSecond.nativeElement.textContent)
    .subscribe((resultEdit: boolean) => {
      if(resultEdit){
        this.homeService.getAboutUs().subscribe((data: any) => {
          this.about = data;
          return;
        });
      }
      else{
        alert('Something\'s wrong!');
      }
    });
  }

  cancelTextSecondPanel(){
    this.changeTextBtnSecond = false;
  }
}
