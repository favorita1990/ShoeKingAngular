import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
  //From header.component
  @Input() messagesWin: boolean;
  //To header.component
  @Output() sendMessagesWin : EventEmitter<boolean>;

  constructor() { 
    this.sendMessagesWin = new EventEmitter();
  }

  ngOnInit() {
  }

  onClickedOutsideMessagesWin(): void {
    if(this.messagesWin){
      this.messagesWin = false;
      this.sendMessagesWin.emit(false);
    }
  }
}
