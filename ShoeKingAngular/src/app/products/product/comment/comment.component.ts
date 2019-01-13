import { ProductService } from './../../../services/product/product.service';
import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-comment',
  templateUrl: './comment.component.html',
  styleUrls: ['./comment.component.css']
})
export class CommentComponent implements OnInit {
  comments: any;
  productId: number;
  userClaims: any;
  text: string;

  constructor(private activatedRoute: ActivatedRoute, private productService: ProductService,
    private userService: UserService) { }

  ngOnInit() {
    this.productId = this.activatedRoute.snapshot.params['id'];

    this.productService.getComments(this.productId).subscribe((data: any) => {
      this.comments = data;
    });

    this.userService.getUserClaims().subscribe((data: any) => {
      this.userClaims = data;
    });
  }

  AddRating(){
    this.productService.addComment(this.text, this.productId).subscribe((data: any) => {
      this.comments = data;
      this.text = '';
    });
  }

  removeComment(commentId){
    this.productService.removeComment(commentId, this.productId).subscribe((data: any) => {
      this.comments = data;
    });
  }
}
