import { Component, Input, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
import { MessageService } from '../../_services/message.service';
import { Message } from '../../_models/message';
import { CommonModule } from '@angular/common';
import { FormControl, FormsModule } from '@angular/forms';
import { TimeagoModule } from 'ngx-timeago';

@Component({
  selector: 'app-chat-box',
  standalone: true,
  imports: [CommonModule, FormsModule, TimeagoModule],
  providers: [MessageService],
  templateUrl: './chat-box.component.html',
  styleUrl: './chat-box.component.css'
})
export class ChatBoxComponent implements OnInit, OnDestroy, OnChanges {
  @Input() recipentId!: string
  messages: Message[] = [];
  content!: string

  constructor(public message: MessageService) { }

  ngOnInit(): void {
    this.getChat();
  }
  ngOnChanges(changes: SimpleChanges): void {
    if (changes['recipentId'] && !changes['recipentId'].isFirstChange()) {
      this.getChat();
    }
  }
  getChat() {
    try {
      this.message.CreateHubConnection(this.recipentId);
    } catch (err) {
      console.error("Error during ngOnInit: ", err);
    }
  }
  onSubmit() {
    if (this.content != null) {
      this.message.sendMessage(this.content, this.recipentId).then(

      )
    }
  }

  ngOnDestroy(): void {
    this.message.stopHubConnection();
  }
}
