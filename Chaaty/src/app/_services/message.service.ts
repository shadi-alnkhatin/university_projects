import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { apiUrl, hubUrl } from '../app.config';
import { Message } from '../_models/message';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject, Observable, ReplaySubject, take } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  private hubConnection!: HubConnection
  messageSource = new BehaviorSubject<Message[]>([]);
  messageThread = this.messageSource.asObservable();
  constructor(private http: HttpClient, private toast: ToastrService) { }

  CreateHubConnection(recepientId: string) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(hubUrl + "message?userId=" + recepientId, {
        accessTokenFactory: () => localStorage.getItem("jwt_token")?.toString() || ""
      }).withAutomaticReconnect().build();
    this.hubConnection.start()
      .then(() => console.log("Hub connection started"))
      .catch(err => console.log("Hub connection error: ", err));

    this.hubConnection.on("ReceiveMessageThread", (messages: Message[]) => {
      console.log(messages);
      this.toast.success("Message hub ON");
      this.messageSource.next(messages);
    });
    this.hubConnection.on("NewMessage", message => {
      this.messageThread.pipe(take(1)).subscribe({
        next: res => {
          this.messageSource.next([...res, message])
        }
      })
    });
  }

  stopHubConnection() {
    this.hubConnection.stop()
      .then(() => console.log("Hub connection stopped"))
      .catch(err => console.log("Error stopping hub connection: ", err));
  }

  loadUersFromMessages() {
    return this.http.get<User[]>(apiUrl + "messages/my-messages");
  }

  loadChat(id: string) {
    return this.http.get<Message[]>(apiUrl + "messages/chat/" + id)
  }
  async sendMessage(content: string, userId: string) {
    return this.hubConnection.invoke("SendMessage", { userId, content }).catch(err => console.log(err))
  }
}
