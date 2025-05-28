import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Message } from '../models/message.model';
import { ChatInputComponent } from '../chat-input/chat-input.component';

@Component({
  selector: 'app-messages',
  imports: [CommonModule, ChatInputComponent],
  templateUrl: './messages.component.html',
  styleUrl: './messages.component.scss',
})
export class MessagesComponent {
  messages: Message[] = [
    new Message('Agent', 'User', 'Hello! Welcome to health care chat bot. How can I help you?', new Date()),
    new Message('User', 'Agent', 'Test user Message by vamsi for ', new Date()),
  ];
}
