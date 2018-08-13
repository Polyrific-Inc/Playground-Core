import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { RegisterUser } from '../models/register-user';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class RegisterService {

  constructor(private http: HttpClient) { }

  register(user: RegisterUser){
    return this.http.post<any>('http://localhost:50178/account/register', user)
      .pipe(map(regUser => {
        return regUser;
      }));
  }
}
