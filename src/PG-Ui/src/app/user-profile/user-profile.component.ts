import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';
import { UserProfile } from '../models/userProfile';

@Component({
  selector: 'pg-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent implements OnInit {

  userProfile: UserProfile;

  constructor(private userService: UserService) { }

  ngOnInit() {
    this.getProfile();
  }

  getProfile(){
    var user = JSON.parse(localStorage.getItem('currentUser')) 
    this.userService.getUserProfile(user.id).subscribe(profile => this.userProfile = profile);
  }

}
