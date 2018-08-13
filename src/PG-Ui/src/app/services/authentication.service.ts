import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { BehaviorSubject } from 'rxjs';
import { Router } from '../../../node_modules/@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  private loggedIn = new BehaviorSubject<boolean>(false); 

  constructor(private http: HttpClient, private router: Router) { }

  login(email: string, password: string){
    return this.http.post<any>(`http://localhost:50178/account/login`, { email, password })
    .pipe(map(user => {
      if(user && user.auth_token){
        localStorage.setItem('currentUser', JSON.stringify(user));
      }
      this.loggedIn.next(true);
      return user;
    }));;
  }

  get isLoggedIn() {
    return this.loggedIn.asObservable();
  }

  logout(){
    localStorage.removeItem('currentUser');
    this.loggedIn.next(false);
    this.router.navigate(["/login"]);
  }
}
