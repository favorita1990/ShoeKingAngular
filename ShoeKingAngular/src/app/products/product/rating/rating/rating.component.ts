import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductService } from 'src/app/services/product/product.service';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-rating',
  templateUrl: './rating.component.html',
  styleUrls: ['./rating.component.css']
})
export class RatingComponent implements OnInit {
  rating: any;
  userClaims: any;
  productId: number;

  constructor(private activatedRoute: ActivatedRoute, private productService: ProductService,
    private userService: UserService) { }

  ngOnInit() {
    this.productId = this.activatedRoute.snapshot.params['id'];
    this.productService.getRatings(this.productId).subscribe((data: any) => {
      this.rating = data;
    });

    this.userService.getUserClaims().subscribe((data: any) => {
      this.userClaims = data;
    });
  }

  AddRating(rateNumber){
    this.productService.addRating(rateNumber, this.productId).subscribe((data: any) => {
      this.rating = data;
    });
  }
}
