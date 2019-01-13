import { CategoryService } from './../services/categories/category.service';
import { ProductService } from './../services/product/product.service';
import { Observable } from 'rxjs/Observable';
import { Component, ViewChild, AfterViewInit, OnInit, ElementRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";

@Component({
  selector: 'app-productpanel',
  templateUrl: './productpanel.component.html',
  styleUrls: ['./productpanel.component.css']
})
export class ProductpanelComponent implements OnInit {
  product: any;
  name: string;
  language: string;
  productPrice: string;
  errorMainImage: boolean = false;
  errorSlideImages: boolean = false;
  openCreateProduct: boolean = false;
  selectedCategoryDropDown: number;
  selectedProductSpecials: boolean;
  noneSelectedCategoryDropDown: boolean;
  errorCategoryImage: boolean;
  mainImageInput: any;
  slideImagesInput: any;
  categoryImageInput: any;
  categories: any;
  gridView: any;

  productForm: FormGroup;
  categoryForm: FormGroup;
  submitted: boolean = false;
  invalidLogin: boolean = false;

  constructor(private formBuilder: FormBuilder, 
    private productService: ProductService, private categoryService: CategoryService) { }

  public ngOnInit(): void {
    this.productForm = this.formBuilder.group({
      ProductName: ['', Validators.required],
      BgProductName: ['', Validators.required],
      MainImage: ['', Validators.required],
      SlideImages: ['', Validators.required],
      Price: ['', Validators.required],
      PromotionPercent: [''],
      ProductStatus: ['', Validators.required],
      ProductSpecials: ['', Validators.required],
      ProductSize: ['', Validators.required],
      Description: ['', Validators.required],
      BgDescription: ['', Validators.required],
      categoryDropdown: ['-1']
    });

    this.categoryForm = this.formBuilder.group({
      ProductCategory: ['', Validators.required],
      BgProductCategory: ['', Validators.required],
      CategoryStatus: ['', Validators.required],
      GenderStatus: ['', Validators.required]
    });

    this.categoryService.allProductCategories().subscribe((data: any) => {
      this.categories = data;
    });

    this.productService.getAllProductsGrid().subscribe((data: any) => {
      this.gridView = data;
    });
  }
  
  createNewProduct() {
      return;
  }

  createProductWindow(){
    this.openCreateProduct = true;
    this.productForm.controls['ProductSpecials'].setValue('');
    this.productForm.controls['ProductStatus'].setValue('');
    this.productForm.controls['categoryDropdown'].setValue('');
  }

  editProductWindow(productEl){
    this.product = productEl;
    let specialsTemp = productEl.Specials ? 1 : 0;
    this.productForm.controls['ProductSpecials'].setValue(specialsTemp);
    if(specialsTemp === 1){
      this.selectedProductSpecials = true;
    }
    else{
      this.selectedProductSpecials = false;
    }
    this.productForm.controls['ProductStatus'].setValue(productEl.Status ? 1 : 0);
    this.productForm.controls['categoryDropdown'].setValue(productEl.CategoryId);
  }

  closeWindowProduct(){
    this.product = null;
    this.openCreateProduct = false;
  }

  mainImageChangeEvent(fileInput: any){
    if (fileInput.target.files && fileInput.target.files[0]) {
      let fileToUpload = fileInput.target.files[0];
      if(fileToUpload.type.indexOf("image")==-1){
        this.errorMainImage = true;
        return;
    }  

    this.mainImageInput = fileInput.target.files[0];
  }
}

slideImagesChangeEvent(fileInput: any){
  if (fileInput.target.files && fileInput.target.files.length > 0) {
    var files = fileInput.target.files;
    for (let index = 0; index < files.length; index++) {
      const fileToUpload = files[index];
      if(fileToUpload.type.indexOf("image")==-1){
        this.errorSlideImages = true;
        return;
      }  
    }

    this.slideImagesInput = fileInput.target.files;
  }
}

categoryImageChangeEvent(fileInput: any){
  if (fileInput.target.files && fileInput.target.files[0]) {
    let fileToUpload = fileInput.target.files[0];
    if(fileToUpload.type.indexOf("image")==-1){
      this.errorCategoryImage = true;
      return;
      }  
      this.categoryImageInput = fileInput.target.files[0];
    }
  }

  saveProduct(){
    this.submitted = true;

    if(undefined !== this.categoryImageInput && 
      this.categoryImageInput.length === 0){
      this.errorCategoryImage = true;
      return;
    }


    if (this.productForm.value.categoryDropdown === '-1') {
      this.noneSelectedCategoryDropDown = true;
      return;
    }


    this.errorMainImage = false;
    this.errorSlideImages = false;
    this.errorCategoryImage = false;

    let inputImage = new FormData();
    if(this.categoryForm != null){
      if (this.categoryForm.invalid && (this.categoryForm.value.categoryDropdown === '0')) {
        return;
      }
    }
    if (this.productForm.invalid) {
      return;
    }

    let productId = null;

    let endMsg = '';
    if(this.product !== null && this.product !== undefined){
      productId = this.product.ProductId;
      endMsg = 'Product was changed!';
    }
    else{
      endMsg = 'Product was created!';
    }

    this.productService.createEditProduct(this.productForm, this.categoryForm, 
      this.mainImageInput, this.slideImagesInput, this.categoryImageInput, productId).subscribe((data: boolean) => {
        if(data){
          alert(endMsg);
          this.product = null;
          this.openCreateProduct = false;
          this.productService.getAllProductsGrid().subscribe((data: any) => {
            this.gridView = data;
          });
        }
        else{
          alert('Something\'s wrong!');
        }
      });
  }

  onChangeProductSpecials(event) {
    if(event === '1'){
      this.selectedProductSpecials = true;
    }
    else{
      this.selectedProductSpecials = false;
    }
  }

  onChangeCategories(categoryId) {
    if(categoryId === '0'){
      this.noneSelectedCategoryDropDown = false;
      this.selectedCategoryDropDown = 0;
    }
    else if(categoryId === '-1'){
      this.selectedCategoryDropDown = null;
      this.noneSelectedCategoryDropDown = true;
    }
    else{
      this.noneSelectedCategoryDropDown = false;
      this.selectedCategoryDropDown = null;
    }
  }

  deleteProduct(product){
    this.productService.deleteProductById(product.ProductId).subscribe((data: boolean) => {
      if(data){
        alert('Product was deleted!');
        this.productService.getAllProductsGrid().subscribe((data: any) => {
          this.gridView = data;
        });
      }
      else{
        alert('Something\'s wrong!');
      }
    });
  }
}