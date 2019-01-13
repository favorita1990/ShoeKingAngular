import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../services/user/user.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})

export class HeaderComponent implements OnInit {
  //Language window
  languageWin: boolean;

  //Login window
  loginWin: boolean;

  //Login window
  registerWin: boolean;

  //Search window
  searchWin: boolean;

  //Messages window
  messagesWin: boolean;

  //Favorites window
  favoritesWin: boolean;
  
  //Bascket window
  bascketWin: boolean;
  
  closeResult: string;
  statusLogOut: boolean;
  statusMessageWinOpen: boolean;
  messageWinClosed:string;
  statusFavoritesWinOpen: boolean;
  favoritesWinClosed:string;
  statusBascketWinOpen: boolean;
  bascketWinClosed:string;

  userClaims: any;

  constructor(private router: Router, private userService: UserService) {

    //Language window
    this.languageWin = false;

    //Login window
    this.loginWin = false;

    //Login window
    this.registerWin = false;

    //Search window
    this.searchWin = false;

    //Messages window
    this.messagesWin = false;
    
    //Favorites window
    this.favoritesWin = false;
    
    //Bascket window
    this.bascketWin = false;
    
    this.statusLogOut = false;
   }

  ngOnInit() {
    this.userService.getUserClaims().subscribe((data: any) => {
      this.userClaims = data;
    });
  }

  //Login window
  showLogin($event){
    $event.stopImmediatePropagation();
    this.loginWin = true;
  }

  loginWinWasClosed(comingLogin: boolean): void {
    this.loginWin = comingLogin;
  }
  //End login window

  //Register window
  showRegister($event){
    $event.stopImmediatePropagation();
    this.registerWin = true;
  }

  registerWinWasClosed(comingRegister: boolean): void {
    this.registerWin = comingRegister;
  }
  //End register window
  
  //Search window
  showSearch($event) {
    $event.stopImmediatePropagation();
    this.searchWin = true;
  }

  searchWinWasClosed(comingSearch: boolean): void {
    this.searchWin = comingSearch;
  }
  //End search window

   //Language window
   showLanguage($event) 
   {
     $event.stopImmediatePropagation();
     this.languageWin = true;
   }
 
   languageWinWasClosed(comingLanugage: boolean): void {
     this.languageWin = comingLanugage;
   }
  //End language window

  //messages window
  showMessages($event) : void
  {
    $event.stopImmediatePropagation();
    this.messagesWin = true;
  }

  messagesWinWasClosed(comingMessages: boolean): void {
    this.messagesWin = comingMessages;
  }
  //End messages window


  //Favorites window
  showFavorites($event) : void
  {
    $event.stopImmediatePropagation();
    this.favoritesWin = true;
  }

  favoritesWinWasClosed(comingFavorites: boolean): void {
    this.favoritesWin = comingFavorites;
  }
  //End favorites window

  //Bascket window
  showBascket($event) : void
  {
    $event.stopImmediatePropagation();
    this.bascketWin = true;
  }

  bascketWinWasClosed(comingBascket: boolean): void {
    this.bascketWin = comingBascket;
  }
  //End bascket window
  
  //Logout window
  clickEventLogOut()
  {
    this.statusLogOut = !this.statusLogOut;
  }

  onClickedOutsideLogOut($event)
  {
    this.statusLogOut = false;
  }

  Logout() {
    this.userClaims = null;
    localStorage.removeItem('userToken');
    this.router.navigate(['']);
  }
  //End logout window

  windowMessagesOpen($event) 
  {
    $event.stopImmediatePropagation();
    this.statusMessageWinOpen = true;
    this.messageWinClosed = "block";
  }

  onClickedOutsideMessagesWin()
  {
    if(this.statusMessageWinOpen){
      this.messageWinClosed = "none";
      this.statusMessageWinOpen = false;
    }
  }

  windowFavoritesOpen($event) 
  {
    $event.stopImmediatePropagation();
    this.statusFavoritesWinOpen = true;
    this.favoritesWinClosed = "block";
  }

  onClickedOutsideFavoritesWin()
  {
    if(this.statusFavoritesWinOpen){
      this.favoritesWinClosed = "none";
      this.statusFavoritesWinOpen = false;
    }
  }

  closeFavoritesWin()
  {
    this.statusFavoritesWinOpen = false;
    this.favoritesWinClosed = "none";
  }


  windowBascketOpen($event) 
  {
    $event.stopImmediatePropagation();
    this.statusBascketWinOpen = true;
    this.bascketWinClosed = "block";
  }

  onClickedOutsideBascketWin()
  {
    if(this.statusBascketWinOpen){
      this.bascketWinClosed = "none";
      this.statusBascketWinOpen = false;
    }
  }

  closeBascketWin()
  {
    this.bascketWinClosed = "none";
    this.statusBascketWinOpen = false;
  }
}
