import { Routes } from '@angular/router';
import { LoginComponent } from './_components/login/login.component';
import { HomeComponent } from './_components/home/home.component';
import { MessagesComponent } from './_components/messages/messages.component';
import { UserListComponent } from './_components/user-list/user-list.component';
import { UserDetailsComponent } from './_components/user-details/user-details.component';
import { ErrorComponent } from './_components/error/error.component';
import { Error404Component } from './_components/error404/error404.component';
import { Error500Component } from './_components/error500/error500.component';
import { Component } from '@angular/core';
import { UserEditComponent } from './_components/user-edit/user-edit.component';
import { RegisterComponent } from './_components/register/register.component';
import { FriendRequestsComponent } from './_components/friend-requests/friend-requests.component';
import { FriendsComponent } from './_components/friends/friends.component';
import { ChatBoxComponent } from './_components/chat-box/chat-box.component';


export const routes: Routes = [

  { path: 'home', component: HomeComponent },
  { path: 'chat', component: ChatBoxComponent },
  { path: '', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'messages', component: MessagesComponent },
  { path: 'users', component: UserListComponent },
  { path: 'user/edit', component: UserEditComponent },
  { path: 'user-details/:id', component: UserDetailsComponent },
  { path: 'friends', component: FriendsComponent },
  { path: 'friend-request', component: FriendRequestsComponent },
  { path: 'errors', component: ErrorComponent },
  { path: 'not-found', component: Error404Component },
  { path: 'server-error', component: Error500Component },



  { path: '**', pathMatch: "full", redirectTo: "/not-found" },





];
