import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  indexOrPhotos: boolean;

  constructor() { }

  ngOnInit() {
    this.indexOrPhotos = true;
  }

  clickProfilePhotos(event) {
    this.indexOrPhotos = event;
  }
}
