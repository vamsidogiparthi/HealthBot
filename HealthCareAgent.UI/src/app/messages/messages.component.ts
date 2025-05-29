import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Message } from '../models/message.model';
import { ChatInputComponent } from '../chat-input/chat-input.component';
import { MessageService } from '../services/message.service';
import { Observable } from 'rxjs';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-messages',
  imports: [CommonModule, FormsModule, ChatInputComponent],
  templateUrl: './messages.component.html',
  styleUrl: './messages.component.scss',
})
export class MessagesComponent {
  // messages: Message[] = [
  //   new Message('Agent', 'User', 'Hello! Welcome to health care chat bot. How can I help you?', new Date()),
  //   new Message('User', 'Agent', 'Test user Message by vamsi for ', new Date()),
  // ];
  userMessage!: string;
  messages$!: Observable<Message[]>;

  constructor(public messageService: MessageService) {
    this.messages$ = messageService.messages$;
  }

  onAddMessage(userMessage: string): void {
    // console.log('Message from child:', message);
    // this.messages.push(message);
    // this.messages = [...this.messages]; // trigger change detection
    // console.log('Updated messages:', this.messages);
    const message = new Message("User", "Agent", userMessage, new Date())
    this.messageService.addMessage(message)
  }
}
