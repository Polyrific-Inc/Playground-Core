import { RouterModule, Routes } from "@angular/router";
import { UserProfileComponent } from "./user-profile/user-profile.component";
import { AuthGuard } from "./guards/auth.guard";
import { LoginComponent } from "./login/login.component";
import { SiteComponent } from "./site/site.component";
import { FacilityComponent } from "./facility/facility.component";
import { RegisterComponent } from "./register/register.component";

const appRoutes: Routes = [
    { path: '', component: UserProfileComponent, canActivate: [AuthGuard] },
    { path: 'user-profile', component: UserProfileComponent, canActivate: [AuthGuard] },
    { path: 'site', component: SiteComponent, canActivate: [AuthGuard] },
    { path: 'facility', component: FacilityComponent, canActivate: [AuthGuard] },
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },

    // others
    { path: '**', redirectTo: '' }
];

export const routing = RouterModule.forRoot(appRoutes);