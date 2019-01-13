import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  readonly rootUrl = 'http://localhost:56445';

  constructor(private http: HttpClient) { }

  allProductCategories(){
    var reqHeader = new HttpHeaders({ 'No-Auth':'False'});
    return this.http.get(this.rootUrl + '/api/AllProductCategories', {headers : reqHeader});
  }

 getAllCategories(){
    var reqHeader = new HttpHeaders({ 'No-Auth':'False'});
    return this.http.get(this.rootUrl + '/api/GetAllCategories', {headers : reqHeader});
  }

  
  createEditCategory(categoryForm: any, categoryImage: any, categoryId: any){
    var reqHeader = new HttpHeaders({ 'No-Auth':'False'});
    var categoryFormData = new FormData();
    
    categoryFormData.append("inputCategoryId", categoryId);
    categoryFormData.append("productCategoryImg", categoryImage);
    categoryFormData.append("inputCategoryStatus", categoryForm.value.CategoryStatus);
    categoryFormData.append("inputCategoryGender", categoryForm.value.GenderStatus);
    categoryFormData.append("inputCategoryText", categoryForm.value.ProductCategory);
    categoryFormData.append("inputCategoryBgText", categoryForm.value.BgProductCategory);

    return this.http.post(this.rootUrl + '/api/CreateEditCategory', categoryFormData, {headers : reqHeader});
  }

  deleteCategoryById(categoryId){
    var reqHeader = new HttpHeaders({ 'No-Auth':'False'});
    let data = "?categoryId=" + categoryId;
    return this.http.post(this.rootUrl + '/api/DeleteCategoryById' + data, {headers : reqHeader});
  }
}
