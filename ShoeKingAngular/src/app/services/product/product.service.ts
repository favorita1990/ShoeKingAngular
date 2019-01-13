import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  readonly rootUrl = 'http://localhost:56445';

  constructor(private http: HttpClient) { }

  createEditProduct(productForm: any, categoryForm: any, mainProductImage: any, 
    slidesProductImages: any, categoryImage: any, productId: any){
    var reqHeader = new HttpHeaders({ 'No-Auth':'False'});
    var productFormData = new FormData();
    
    var inputListSize = productForm.value.ProductSize.split(",").map(function (item) {
          return item.trim();
      });;

    productFormData.append("inputProductId", productId);
    productFormData.append("inputName", productForm.value.ProductName);
    productFormData.append("inputBgName", productForm.value.BgProductName);
    productFormData.append("inputPrice", productForm.value.Price);
    productFormData.append("inputSpecials", productForm.value.ProductSpecials);
    productFormData.append("productMainImg", mainProductImage);

    var inputListSizeNew = [];
    var inputListQuantityNew = [];

    for (var i = 0; i < inputListSize.length; i++) {
      if (inputListSize[i] !== '') {
        var inputSizeQuantity = inputListSize[i].split("-").map(function (item) {
            return item.trim();
        });;

        if (isNaN(Number(inputSizeQuantity[0]))) {
            alert('Add number size!');
        }
        if (isNaN(Number(inputSizeQuantity[1]))) {
            alert('Add number quanity!');

        }
        if (inputSizeQuantity[0] !== '') {

            if (Number(inputSizeQuantity[0]) < 10 || Number(inputSizeQuantity[0]) > 55) {
                alert('Add number between 10 and 55!');
            }
            else {
                inputListSizeNew.push(inputSizeQuantity[0]);
            }
        }
        if (inputSizeQuantity[1] !== '') {

            if (Number(inputSizeQuantity[1]) < 0 || Number(inputSizeQuantity[1]) > 1000) {
                alert('Add quanity between 0 and 1000!');
            }
            else {
                inputListQuantityNew.push(inputSizeQuantity[1]);
            }
        }
    }
  }

    for (var x = 0; x < slidesProductImages.length; x++) {
      productFormData.append("productBodyImg" + x, slidesProductImages[x], slidesProductImages.length);
    }

    productFormData.append("productCategoryImg", categoryImage);

    productFormData.append("inputStatus", productForm.value.ProductStatus);
    productFormData.append("inputPromotionPercent", productForm.value.PromotionPercent);
    productFormData.append("inputCategories", productForm.value.categoryDropdown);

    if(productForm.value.categoryDropdown === '0'){
      productFormData.append("inputCategoryStatus", categoryForm.value.CategoryStatus);
      productFormData.append("inputCategoryGender", categoryForm.value.GenderStatus);
      productFormData.append("inputCategoryText", categoryForm.value.ProductCategory);
      productFormData.append("inputCategoryBgText", categoryForm.value.BgProductCategory);
    }

    productFormData.append("inputListSize", inputListSizeNew.toString());
    productFormData.append("inputListQuantity", inputListQuantityNew.toString());
    productFormData.append("inputDescription", productForm.value.Description);
    productFormData.append("inputBgDescription", productForm.value.BgDescription);

    return this.http.post(this.rootUrl + '/api/CreateEditProduct', productFormData, {headers : reqHeader});
  }

  getAllProductsGrid(){
    var reqHeader = new HttpHeaders({ 'No-Auth':'False'});
    return this.http.get(this.rootUrl + '/api/GetAllProductsGrid', {headers : reqHeader});
  }

  deleteProductById(productId){
    var reqHeader = new HttpHeaders({ 'No-Auth':'False'});
    let data = "?productId=" + productId;
    return this.http.post(this.rootUrl + '/api/DeleteProductById' + data, {headers : reqHeader});
  }

  getProductById(productId){
    var reqHeader = new HttpHeaders({ 'No-Auth':'True' });

    if(localStorage.getItem('userToken') !== null){
     reqHeader = new HttpHeaders({ 'No-Auth':'False' });
    }

    let data = "?id=" + productId;
    return this.http.get(this.rootUrl + '/api/GetProductById' + data, {headers : reqHeader});
  }

   getRatings(productId){
    var reqHeader = new HttpHeaders({ 'No-Auth':'True' });

    if(localStorage.getItem('userToken') !== null){
     reqHeader = new HttpHeaders({ 'No-Auth':'False' });
    }

    let data = "?productId=" + productId;
    return this.http.get(this.rootUrl + '/api/GetRatings' + data, {headers : reqHeader});
  }

  addRating(rateNumber, productId){
    var reqHeader = new HttpHeaders({ 'No-Auth':'True' });

    if(localStorage.getItem('userToken') !== null){
     reqHeader = new HttpHeaders({ 'No-Auth':'False' });
    }

    let data = "?rateNumber=" + rateNumber + "&productId=" + productId;
    return this.http.post(this.rootUrl + '/api/AddRating' + data, {headers : reqHeader});
  }

  getComments(productId){
    var reqHeader = new HttpHeaders({ 'No-Auth':'True' });

    if(localStorage.getItem('userToken') !== null){
     reqHeader = new HttpHeaders({ 'No-Auth':'False' });
    }

    let data = "?productId=" + productId;
    return this.http.get(this.rootUrl + '/api/GetComments' + data, {headers : reqHeader});
  }

  addComment(text, productId){
    var reqHeader = new HttpHeaders({ 'No-Auth':'True' });

    if(localStorage.getItem('userToken') !== null){
     reqHeader = new HttpHeaders({ 'No-Auth':'False' });
    }

    let data = "?text=" + text + "&productId=" + productId;
    return this.http.post(this.rootUrl + '/api/AddComment' + data, {headers : reqHeader});
  }

  removeComment(commentId, productId){
    var reqHeader = new HttpHeaders({ 'No-Auth':'True' });

    if(localStorage.getItem('userToken') !== null){
     reqHeader = new HttpHeaders({ 'No-Auth':'False' });
    }

    let data = "?commentId=" + commentId + "&productId=" + productId;
    return this.http.post(this.rootUrl + '/api/RemoveComment' + data, {headers : reqHeader});
  }
}
