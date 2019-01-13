import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { NgSelectModule } from '@ng-select/ng-select';
import { FormsModule, ReactiveFormsModule} from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { GridModule } from '@progress/kendo-angular-grid';
import { ClickOutsideModule } from 'ng-click-outside';
import { NgbModal, NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
import { RouterModule } from '@angular/router'
import { WindowModule } from '@progress/kendo-angular-dialog';
import { ButtonsModule } from '@progress/kendo-angular-buttons';

import { AppComponent } from './app.component';
import { UserService } from './services/user/user.service';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { HeaderComponent } from './header/header.component';
import { FooterComponent } from './footer/footer.component';
import { HomeComponent } from './home/home.component';
import { appRoutes } from './routes';
import { AuthGuard } from './auth/auth.guard';
import { AuthInterceptor } from './auth/auth.interceptor';
import { AboutComponent } from './home/about/about.component';
import { ContactComponent } from './home/contact/contact.component';
import { CustomersComponent } from './customers/customers.component';
import { ProductsComponent } from './products/products.component';
import { MenComponent } from './products/men/men.component';
import { WomenComponent } from './products/women/women.component';
import { ProductComponent } from './products/product/product.component';
import { SearchComponent } from './products/search/search.component';
import { ProfileComponent } from './profile/profile.component';
import { PhotosComponent } from './profile/photos/photos.component';
import { IndexComponent } from './profile/index/index.component';
import { SettingsComponent } from './header/settings/settings.component';
import { ProductpanelComponent } from './productpanel/productpanel.component';
import { CategoryComponent } from './productpanel/category/category.component';
import { AccountComponent } from './account/account.component';
import { RegisterComponent } from './account/register/register.component';
import { SearchHeaderComponent } from './header/search-header/search-header.component';
import { LanguageComponent } from './header/language/language.component';
import { MessagesComponent } from './header/messages/messages.component';
import { FavoritesComponent } from './header/favorites/favorites.component';
import { BascketComponent } from './header/bascket/bascket.component';
import { RatingComponent } from './products/product/rating/rating/rating.component';
import { CommentComponent } from './products/product/comment/comment.component';


@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    FooterComponent,
    HomeComponent,
    AboutComponent,
    ContactComponent,
    CustomersComponent,
    ProductsComponent,
    MenComponent,
    WomenComponent,
    ProductComponent,
    SearchComponent,
    ProfileComponent,
    PhotosComponent,
    IndexComponent,
    SettingsComponent,
    ProductpanelComponent,
    CategoryComponent,
    AccountComponent,
    RegisterComponent,
    SearchHeaderComponent,
    LanguageComponent,
    MessagesComponent,
    FavoritesComponent,
    BascketComponent,
    RatingComponent,
    CommentComponent 
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AngularFontAwesomeModule,
    NgSelectModule, 
    NgbModalModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    ClickOutsideModule,
    RouterModule.forRoot(appRoutes),
    GridModule,
    WindowModule,
    ButtonsModule
  ],
  providers: [NgbModal,UserService,AuthGuard,
    ,
    {
      provide : HTTP_INTERCEPTORS,
      useClass : AuthInterceptor,
      multi : true
    }],
  bootstrap: [AppComponent]
})
export class AppModule { }
