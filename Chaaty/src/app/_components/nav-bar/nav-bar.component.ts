import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { AuthinticationService } from '../../_services/authintication-service.service';
import { CommonModule } from '@angular/common';
import { User } from '../../_models/user';

@Component({
  selector: 'app-nav-bar',
  standalone: true,
  imports: [RouterModule, CommonModule],
  providers: [AuthinticationService],
  templateUrl: './nav-bar.component.html',
  styleUrl: './nav-bar.component.css'
})
export class NavBarComponent implements OnInit {
  user: User | null = null;

  constructor(public auth: AuthinticationService) {

  }
  ngOnInit() {
    console.log(this.auth.isAuthenticated());
  }

 public logout(){
    this.auth.logout();
  }

}


