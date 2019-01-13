import { Routes } from '@angular/router'
import { HomeComponent } from './home/home.component';
import { AboutComponent } from './home/about/about.component';
import { ContactComponent } from './home/contact/contact.component';
import { CustomersComponent } from './customers/customers.component';
import { ProductsComponent } from './products/products.component';
import { MenComponent } from './products/men/men.component';
import { WomenComponent } from './products/women/women.component';
import { ProductComponent } from './products/product/product.component';
import { SearchComponent } from './products/search/search.component';
import { ProfileComponent } from './profile/profile.component';
import { SettingsComponent } from './header/settings/settings.component';
import { ProductpanelComponent } from './productpanel/productpanel.component';


import { AuthGuard } from './auth/auth.guard';


export const appRoutes: Routes = [
    { 
        path: 'home', 
        children: [
            { path: '', component: HomeComponent },
            { path: 'index', component: HomeComponent },
            { path: 'about', component: AboutComponent },
            { path: 'contact', component: ContactComponent }
        ]
    },
    { 
        path: 'Home', 
        children: [
            { path: '', component: HomeComponent },
            { path: 'Index', component: HomeComponent },
            { path: 'About', component: AboutComponent },
            { path: 'Contact', component: ContactComponent }
        ]
    },
    { 
        path: 'customers', 
        children: [
            { path: '', component: CustomersComponent },
            { path: 'index', component: CustomersComponent }
        ]
    },
    { 
        path: 'Customers', 
        children: [
            { path: '', component: CustomersComponent },
            { path: 'Index', component: CustomersComponent }
        ]
    },
    { 
        path: 'products', 
        children: [
            { path: '', component: ProductsComponent },
            { path: 'index', component: HomeComponent },
            { path: 'men', component: MenComponent},
            { path: 'women', component: WomenComponent },
            { path: 'product', component: ProductComponent },
            { path: 'search', component: SearchComponent }
        ],
    },
    { 
        path: 'Products', 
        children: [
            { path: '', component: ProductsComponent },
            { path: 'Index', component: HomeComponent },
            { path: 'Men', component: MenComponent },
            { path: 'Women', component: WomenComponent },
            { path: 'Product', component: ProductComponent },
            { path: 'Search', component: SearchComponent }
        ]
    },
    { 
        path: 'profile', 
        children: [
            { path: '', component: ProfileComponent },
            { path: 'index', component: ProfileComponent }
        ]
    },
    { 
        path: 'Profile', 
        children: [
            { path: '', component: ProfileComponent },
            { path: 'Index', component: ProfileComponent }
        ]
    },
    { 
        path: 'account', 
        children: [
            { path: '', component: HomeComponent },
            { path: 'settings', component: SettingsComponent }
        ]
    },
    { 
        path: 'Account', 
        children: [
            { path: '', component: HomeComponent },
            { path: 'Settings', component: SettingsComponent }
        ]
    },
    { 
        path: 'productpanel', 
        children: [
            { path: '', component: ProductpanelComponent }
        ]
    },
    { 
        path: 'ProductPanel', 
        children: [
            { path: '', component: ProductpanelComponent }
        ]
    },
    {
        path: 'Product', 
        children: [
            {
              path: ':id',
              component: ProductComponent
            }
          ]
    },
    {
        path: 'product', 
        children: [
            {
              path: ':id',
              component: ProductComponent
            }
          ]
    },
    { path : '', component: HomeComponent}
];