import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-bascket',
  templateUrl: './bascket.component.html',
  styleUrls: ['./bascket.component.css']
})
export class BascketComponent implements OnInit {
  //From header.component
  @Input() bascketWin: boolean;
  //To header.component
  @Output() sendBascketWin : EventEmitter<boolean>;

  constructor() {
    this.sendBascketWin = new EventEmitter();
  } 

  ngOnInit() {
  }

  closeBascketWin(): void {
    this.bascketWin = false;
    this.sendBascketWin.emit(false);
  }

  onClickedOutsideBascketWin(): void {
    if(this.bascketWin){
      this.bascketWin = false;
      this.sendBascketWin.emit(false);
    }
  }
}
