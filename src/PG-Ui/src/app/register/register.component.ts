import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { RegisterService } from '../services/register.service';
import { RegisterUser } from '../models/register-user';
import { first } from 'rxjs/operators';

@Component({
  selector: 'pg-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  submitted = false;
  returnUrl = "/login";
  error = '';  

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private registerService: RegisterService
  ) { }

  get frm() { return this.registerForm.controls; }

  ngOnInit() {
    this.registerForm = this.formBuilder.group({
      email: ['', Validators.required],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      password: ['', Validators.required],
      confirmPassword: ['', Validators.required]
    });
  }

  onRegisterSubmit(){
    this.submitted = true;
    if(this.registerForm.invalid){
      return;
    }

    var user = new RegisterUser(this.frm.email.value, this.frm.firstName.value, this.frm.lastName.value, this.frm.password.value, this.frm.confirmPassword.value);
    this.registerService.register(user)
      .pipe(first())
      .subscribe(
        data => {
          this.router.navigate([this.returnUrl])
        },
        error => {
          this.error = error;         
        }
      );
  }

}
