import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import 'rxjs/add/operator/map';
import { User } from '../../models/user/user';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  readonly rootUrl = 'http://localhost:56445';

  constructor(private http: HttpClient) { }

  registerUser(user: User) {
    const body: User = {
      Email: user.Email,
      FirstName: user.FirstName,
      LastName: user.LastName,
      Gender: user.Gender,
      Password: user.Password,
      ConfirmPass: user.ConfirmPass
    }
    
    var reqHeader = new HttpHeaders({'No-Auth':'True'});

    return this.http.post(this.rootUrl + '/api/User/Register', body, {headers : reqHeader});
  }

  userAuthentication(userName, password) {
    var data = "username=" + userName + "&password=" + password + "&grant_type=password";
    var reqHeader = new HttpHeaders({ 'Content-Type': 'application/x-www-urlencoded', 'No-Auth':'True' });
    return this.http.post(this.rootUrl + '/token', data, { headers: reqHeader });
  }

  getUserClaims(){
    var reqHeader = new HttpHeaders({ 'No-Auth':'True' });

    if(localStorage.getItem('userToken') !== null){
     reqHeader = new HttpHeaders({ 'No-Auth':'False' });
    }

    return this.http.get(this.rootUrl + '/api/GetUserClaims', {headers : reqHeader});
  }

  checkUserPassword(password){
    var data = "?password=" + password;
    var reqHeader = new HttpHeaders({ 'No-Auth':'False' });
    return this.http.post(this.rootUrl + '/api/CheckUserPassword' + data, {headers : reqHeader});
  }

  changePassword(oldPassword, newPassword){
    let data = '?oldPassword=' + oldPassword + '&newPassword=' + newPassword;
    var reqHeader = new HttpHeaders({ 'No-Auth':'False' });
    return this.http.post(this.rootUrl + '/api/ChangePassword' + data, {headers : reqHeader});
  }
}
