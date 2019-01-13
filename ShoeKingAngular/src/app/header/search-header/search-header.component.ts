import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-search-header',
  templateUrl: './search-header.component.html',
  styleUrls: ['./search-header.component.css']
})
export class SearchHeaderComponent implements OnInit {
  //From header.component
  @Input() searchWin: boolean;
  //To header.component
  @Output() sendSearchWin : EventEmitter<boolean>;

  constructor() {
    this.sendSearchWin = new EventEmitter();
   }

  ngOnInit() {
  }

  closeSearchWin(): void {
    this.searchWin = false;
    this.sendSearchWin.emit(false);
  }

  onClickedOutsideSearchWin(): void {
    if(this.searchWin){
      this.searchWin = false;
      this.sendSearchWin.emit(false);
    }
  }
}
