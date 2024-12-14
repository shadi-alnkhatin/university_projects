import { Component, OnInit, Output } from '@angular/core';
import { MessageService } from '../../_services/message.service';
import { Message } from '../../_models/message';
import { CommonModule } from '@angular/common';
import { ChatListComponent } from '../chat-list/chat-list.component';
import { ChatBoxComponent } from '../chat-box/chat-box.component';
import { User } from '../../_models/user';

@Component({
  selector: 'app-messages',
  standalone: true,
  imports: [CommonModule, ChatListComponent, ChatBoxComponent],
  providers: [MessageService],
  templateUrl: './messages.component.html',
  styleUrl: './messages.component.css'
})
export class MessagesComponent implements OnInit {
  users: User[] = [];
 reciver?: string
  constructor(private messageService: MessageService) {

  }

  ngOnInit(): void {
     this.getUsers();
  }
  getUsers() {
    this.messageService.loadUersFromMessages().subscribe({
      next: res => {
        this.users = res;
        console.log(res)
      }
    })
  }

  injectReciver(id: string) {
    this.reciver = id;
  }
  clearReciver(): void {
    this.reciver = undefined;
  }
}
