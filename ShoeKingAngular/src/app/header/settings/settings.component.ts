import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements OnInit {
  changePasswordPage : boolean;
  settingsForm: FormGroup;
  settingsBtn: boolean;
  checkPasswordResult: boolean;

  constructor(private formBuilder: FormBuilder, private userService : UserService) { }

  ngOnInit() {
    this.changePasswordPage = true;

    this.settingsForm = this.formBuilder.group({
      oldPassword: ['', Validators.required],
      newPassword: ['', Validators.required],
      confirmPassword: ['', Validators.required]
    });

    this.checkPasswordResult = false;
  }

  changeSettingsPage(event){
    this.changePasswordPage = false;
  }

  backMainPage(event){
    this.changePasswordPage = true;
  }

  changePasswordForm(){
    this.settingsBtn = true;

    if(this.settingsForm.invalid){
      return;
    }
    else if(this.settingsForm.value.newPassword !== this.settingsForm.value.confirmPassword){
      this.settingsForm.controls.confirmPassword.setErrors({'nomatch': true});
      this.settingsForm.controls.newPassword.setErrors({'nomatch': true});
      return;
    }

    this.settingsForm.controls.confirmPassword.setErrors(null);
    this.settingsForm.controls.newPassword.setErrors(null);

    let newPassword = this.settingsForm.value.newPassword;
    let oldPassword = this.settingsForm.value.oldPassword;

    this.userService.checkUserPassword(oldPassword).subscribe((data : boolean) => {
      this.checkPasswordResult = data;

      if(!this.checkPasswordResult){
        this.settingsForm.controls.newPassword.setErrors({'noMatchPassword': true});
        return;
      }

      this.settingsForm.controls.newPassword.setErrors(null);

      this.userService.changePassword(oldPassword, newPassword).subscribe((data : any) => {
        alert('Password was changed!');
        this.changePasswordPage = true;
      },
      (error : any) => {
        alert('Something\'s wrong!');
        }
      );
    });
  }
}
