import { ProductService } from './../../services/product/product.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute  } from '@angular/router';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent implements OnInit {
  product: any;

  constructor(private activatedRoute: ActivatedRoute, private productService: ProductService) { }

  ngOnInit() {
    let productId = this.activatedRoute.snapshot.params['id'];
    this.productService.getProductById(productId).subscribe((data: any) => {
      if(data !== null){
        this.product = data;
      }
      else{
        location.href = '/';
      }
    });
  }
}
