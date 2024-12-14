import { Component, EventEmitter, Input, Output } from '@angular/core';
import { User } from '../../_models/user';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-chat-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './chat-list.component.html',
  styleUrl: './chat-list.component.css'
})
export class ChatListComponent {
  @Input() users!: User[];
  @Output() reciverId = new EventEmitter<string>();

  injectUserId(id: string) {
    this.reciverId.emit(id);
  }

}
