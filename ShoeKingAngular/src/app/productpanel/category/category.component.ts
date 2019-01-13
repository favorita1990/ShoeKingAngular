import { Component, OnInit } from '@angular/core';
import { CategoryService } from 'src/app/services/categories/category.service';
import { Validators, FormGroup, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.css']
})
export class CategoryComponent implements OnInit {
  categories: any;
  openCreateCategory: any;
  category: any;
  categoryForm: FormGroup; 
  categoryImageInput: any;
  submitted: boolean;

  constructor(private categoryService: CategoryService, private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.categoryService.getAllCategories().subscribe((data: any) => {
      this.categories = data;
    });

    this.categoryForm = this.formBuilder.group({
      ProductCategory: ['', Validators.required],
      BgProductCategory: ['', Validators.required],
      CategoryStatus: ['', Validators.required],
      GenderStatus: ['', Validators.required],
      categoryImageInput: ['', Validators.required],
    });

    this.submitted = false;
  }

  createCategoryWindow(){
    this.openCreateCategory = true;
    this.categoryForm.controls['CategoryStatus'].setValue('');
    this.categoryForm.controls['CategoryStatus'].setValue('');
  }

  editCategoryWindow(categoryEl){
    this.category = categoryEl;
    this.categoryForm.controls['CategoryStatus'].setValue(categoryEl.Status ? 1 : 0);
    this.categoryForm.controls['GenderStatus'].setValue(categoryEl.Gender ? 1 : 0);
  }

  categoryImageChangeEvent(fileInput: any){
  if (fileInput.target.files && fileInput.target.files[0]) {
    let fileToUpload = fileInput.target.files[0];
    if(fileToUpload.type.indexOf("image")==-1){
      return;
      }  
      this.categoryImageInput = fileInput.target.files[0];
    }
  }

  
  closeWindowCategory(){
    this.category = null;
    this.openCreateCategory = false;
  }

  saveCategory(){
    this.submitted = true;

    if (this.categoryForm.invalid) {
      return;
    }
  
    let categoryId = null;

    let endMsg = '';
    if(this.category !== null && this.category !== undefined){
      categoryId = this.category.CategoryId;
      endMsg = 'Category was changed!';
    }
    else{
      endMsg = 'Category was created!';
    }

    this.categoryService.createEditCategory(this.categoryForm, this.categoryImageInput, categoryId).subscribe((data: boolean) => {
        if(data){
          alert(endMsg);
          this.category = null;
          this.openCreateCategory = false;
          this.categoryService.getAllCategories().subscribe((data: any) => {
            this.categories = data;
          });
        }
        else{
          alert('Something\'s wrong!');
        }
      });
  }

  deleteCategory(product){
    this.categoryService.deleteCategoryById(product.CategoryId).subscribe((data: boolean) => {
      if(data){
        alert('Category was deleted!');
        this.categoryService.getAllCategories().subscribe((data: any) => {
          this.categories = data;
        });
      }
      else{
        alert('Something\'s wrong!');
      }
    });
  }
}
