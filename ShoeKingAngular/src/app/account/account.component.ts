import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from '../services/user/user.service';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit {
  //From header.component
  @Input() loginWin: boolean;
  //To header.component
  @Output() sendLoginWin : EventEmitter<boolean>;
  loginForm: FormGroup;
  userName: FormGroup;
  password: FormGroup;
  loginBtn: boolean;
  loginUserName: boolean;

  constructor(private formBuilder: FormBuilder, private userService : UserService, private router : Router) { 
    this.sendLoginWin = new EventEmitter();
  }

  ngOnInit() {
    this.loginForm = this.formBuilder.group({
      userName: ['', Validators.required],
      pass: ['', Validators.required]
    });

    this.loginBtn = false;
    this.loginUserName = false;
  }

  login() {
    this.loginBtn = true;

    if (this.loginForm.invalid) {
      return;
    }

    this.userService.userAuthentication(this.loginForm.value.userName, this.loginForm.value.pass)
    .subscribe((data : any)=>{
      this.loginWin = false;
      this.sendLoginWin.emit(false);

      localStorage.setItem('userToken', data.access_token);
      // this.router.navigate(['']);
      window.location.reload();
    },
    (err : HttpErrorResponse)=>{
      this.loginUserName = true;
      return;
    });
  }

  closeLoginWin(): void {
    this.loginWin = false;
    this.sendLoginWin.emit(false);
  }

  onClickedOutsideLoginWin(){
    if(this.loginWin){
      this.loginWin = false;
      this.sendLoginWin.emit(false);
    }
  }
}
