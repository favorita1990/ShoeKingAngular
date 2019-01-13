import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { UserService } from '../../services/user/user.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  //From header.component
  @Input() registerWin: boolean;
  //To header.component
  @Output() sendRegisterWin : EventEmitter<boolean>;

  emailPattern = "^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$";
  registerForm: FormGroup;
  Email: FormGroup;
  registerBtn: boolean;
  passConfirmPassCheck: boolean;

  constructor(private userService: UserService, private formBuilder: FormBuilder) {
    this.sendRegisterWin = new EventEmitter();
   }

  ngOnInit() {
    this.registerForm = this.formBuilder.group({
      Email: ['', Validators.required],
      FirstName: ['', Validators.required],
      LastName: ['', Validators.required],
      Gender: ['0'],
      Password: ['', Validators.required],
      ConfirmPass: ['', Validators.required]
    });

    this.registerBtn = false;
    this.passConfirmPassCheck = false;
  }

  register() {
    this.registerBtn = true;

    // this.registerWin = false;
    // this.sendRegisterWin.emit(false);

    let password = this.registerForm.controls.Password.value;
    let passwordConfirmation = this.registerForm.controls.ConfirmPass.value;

    if(password !== passwordConfirmation)
    {
      this.passConfirmPassCheck = true;
      return;
    }

    if (this.registerForm.invalid) {
      this.passConfirmPassCheck = false;
      return;
    }
    
    this.userService.registerUser(this.registerForm.value)
    .subscribe((data: any) => {
      if (data.Succeeded == true) {
        this.userService.userAuthentication(this.registerForm.value.Email, this.registerForm.value.Password)
        .subscribe((data : any)=>{
          localStorage.setItem('userToken', data.access_token);
          window.location.reload();
        })
      }
    });
  }

  closeRegisterWin(): void {
    this.registerWin = false;
    this.sendRegisterWin.emit(false);
  }

  onClickedOutsideRegisterWin(){
    if(this.registerWin){
      this.registerWin = false;
      this.sendRegisterWin.emit(false);
    }
  }

}
