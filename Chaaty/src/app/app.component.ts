import { CommonModule } from '@angular/common';
import { HttpClientModule, HttpClient, } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NavBarComponent } from './_components/nav-bar/nav-bar.component';
import { LoginComponent } from './_components/login/login.component';
import { RouterModule } from '@angular/router';
import { NgxSpinner, NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { PresenceService } from './_services/presence.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    FormsModule, CommonModule, HttpClientModule, NavBarComponent, LoginComponent, RouterModule, NgxSpinnerModule],
  providers: [],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit, OnDestroy {

  title = 'ChaatyCLient';
  users: any;
  public onlineUsers: string[] = [];
  private onlineUsersSubscription!: Subscription;
  constructor(private presence: PresenceService) {

  }
  ngOnInit(): void {
    this.presence.CreateHubConnection();
  }

  ngOnDestroy(): void {
    this.presence.StopConnection();
  }


}
