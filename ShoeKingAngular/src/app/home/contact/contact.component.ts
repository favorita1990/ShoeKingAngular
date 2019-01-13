import { HomeService } from './../../services/home/home.service';
import { Validators } from '@angular/forms';
import { FormGroup, FormBuilder } from '@angular/forms';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.css']
})
export class ContactComponent implements OnInit {
  contactForm: FormGroup;
  contactBtn: boolean;

  constructor(private formBuilder: FormBuilder, private homeService: HomeService) { }

  ngOnInit() {
    this.contactForm = this.formBuilder.group({
      subject: ['', Validators.required],
      name: ['', Validators.required],
      email: ['', Validators.required],
      message: ['', Validators.required]
    });

    this.contactBtn = false;
  }

  sendContactMsgForm(){
    this.contactBtn = true;

    if (this.contactForm.invalid) {
      return;
    }

    let email = this.contactForm.value.email;

    this.homeService.isValidEmail(email).subscribe((data : boolean) => {
      if(data){
        this.homeService.sendEmail(this.contactForm.value.name, this.contactForm.value.email, 
          this.contactForm.value.subject, this.contactForm.value.message).subscribe((data : any) => {
            alert('Your message was sent!');
          },
          (err : any) => {
            alert('Something\'s wrong!');
          });
      }
      else{
        alert('Your email is not valid!');
      }
    },
    (err : any) => {
      alert('Your email is not valid!');
    });
  }
}
