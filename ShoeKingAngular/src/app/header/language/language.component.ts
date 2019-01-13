import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-language',
  templateUrl: './language.component.html',
  styleUrls: ['./language.component.css']
})
export class LanguageComponent implements OnInit {
  //From header.component
  @Input() languageWin: boolean;
  //To header.component
  @Output() sendLanguageWin : EventEmitter<boolean>;
  languages: any;
  selectedLanguage: any;

  constructor() { 
    this.selectedLanguage = {
      id: 1, name: 'Engish'
    };

    this.languages = [
      {id: 1, name: 'Engish', disabled: true},
      {id: 2, name: 'Bulgarian'}
    ];

    this.sendLanguageWin = new EventEmitter();
  }

  ngOnInit() {
  }

  onChange($event) {
    this.selectedLanguage = $event;
    if($event.id == 1){
      this.languages = [
        {id: 1, name: 'Engish', disabled: true},
        {id: 2, name: 'Bulgarian'}
      ];
    }
    else{
      this.languages = [
        {id: 1, name: 'Engish'},
        {id: 2, name: 'Bulgarian', disabled: true}
      ];
    }
  }

  closeLanguageWin(): void {
    this.languageWin = false;
    this.sendLanguageWin.emit(false);
  }

  onClickedOutsideLanguageWin()
  {
    if(this.languageWin){
      this.languageWin = false;
      this.sendLanguageWin.emit(false);
    }
  }
}
