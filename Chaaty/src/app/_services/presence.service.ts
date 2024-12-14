import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { User } from '../_models/user';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { hubUrl } from '../app.config';
import { IAuthResponse } from '../_models/IAuthResponse';
import { BehaviorSubject, Subject, first } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {

  private hubConnection!: HubConnection;
  private onlineUsersSubject = new BehaviorSubject<string[]>([]);
  onlineUsers$ = this.onlineUsersSubject.asObservable();
  token = localStorage.getItem("jwt_token");
  constructor(private toastr: ToastrService) { }

  CreateHubConnection() {
    this.hubConnection = new HubConnectionBuilder().withUrl(hubUrl + "presence", { accessTokenFactory: () => this.token ? this.token : '' })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch(
      err => console.log(err)
    )

    this.hubConnection.on("UserOnline", user => {
      this.toastr.success(user);
      this.updateOnlineUsersList(user, true)
    });
    this.hubConnection.on("UserOfline", user => {
      this.toastr.warning(user);
      this.updateOnlineUsersList(user, false)
    });
    this.hubConnection.on("GetOnlineUsers", (users: string[]) => {
      this.onlineUsersSubject.next(users);
    });
    this.hubConnection.on("NewMessageNotify", ( firstName: string ) => {
      console.log(`Received firstName: ${firstName}`); // Debugging line
      this.toastr.show("you recived a new message from " + firstName )
    })


  }

  fetchOnlineUsers() {
    this.hubConnection.invoke('GetOnlineUsers')
      .catch(err => console.error('Error fetching online users: ' + err));
  }
  StopConnection() {
    this.hubConnection.stop().catch(err => console.log(err));
  }

  private updateOnlineUsersList(user: string, isOnline: boolean) {
    const currentUsers = this.onlineUsersSubject.value;
    const updatedUsers = isOnline ? [...currentUsers, user] : currentUsers.filter(u => u !== user);
    this.onlineUsersSubject.next(updatedUsers);
  }
}
