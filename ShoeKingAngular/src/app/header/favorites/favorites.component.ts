import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-favorites',
  templateUrl: './favorites.component.html',
  styleUrls: ['./favorites.component.css']
})
export class FavoritesComponent implements OnInit {
  //From header.component
  @Input() favoritesWin: boolean;
  //To header.component
  @Output() sendFavoritesWin : EventEmitter<boolean>;

  constructor() {
    this.sendFavoritesWin = new EventEmitter();
   }

  ngOnInit() {
  }

  closeFavoritesWin(): void {
    this.favoritesWin = false;
    this.sendFavoritesWin.emit(false);
  }

  onClickedOutsideFavoritesWin(): void {
    if(this.favoritesWin){
      this.favoritesWin = false;
      this.sendFavoritesWin.emit(false);
    }
  }

}
