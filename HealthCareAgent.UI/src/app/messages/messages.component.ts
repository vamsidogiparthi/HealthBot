import { CommonModule } from '@angular/common';
import { Component, computed, OnInit } from '@angular/core';
import { Message } from '../models/message.model';
import { ChatInputComponent } from '../chat-input/chat-input.component';
import { MessageService } from '../services/message.service';
import { Observable } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { MessageRelayService } from '../services/message-relay-service.service';

@Component({
  selector: 'app-messages',
  imports: [CommonModule, FormsModule, ChatInputComponent],
  templateUrl: './messages.component.html',
  styleUrl: './messages.component.scss',
})
export class MessagesComponent implements OnInit {
  ngOnInit(): void {
    this.messageRelayService.init('http://localhost:5271/ChatHub');
  }
  // messages: Message[] = [
  //   new Message('Agent', 'User', 'Hello! Welcome to health care chat bot. How can I help you?', new Date()),
  //   new Message('User', 'Agent', 'Test user Message by vamsi for ', new Date()),
  // ];
  userMessage!: string;
  messages = computed<Message[]>(() =>
    this.messageRelayService
      .messages()
      .sort((a, b) => new Date(a.timestamp).getTime() - new Date(b.timestamp).getTime())
  );

  constructor(
    public messageService: MessageService,
    public messageRelayService: MessageRelayService
  ) {
    // Initialize messages$ from the message service
  }

  onAddMessage(userMessage: string): void {
    // console.log('Message from child:', message);
    // this.messages.push(message);
    // this.messages = [...this.messages]; // trigger change detection
    // console.log('Updated messages:', this.messages);
    const message = new Message('User', 'Agent', userMessage, new Date());
    this.messageService.addMessage(message);
  }

  sendMessage(message: string): void {
    if (!message.trim()) return;
    console.log('[Send Message] Sending message:', message);

    const messageClass = new Message('User', 'Agent', message, new Date());
    console.log('Sending message:', messageClass);
    this.messageRelayService.sendMessage(messageClass);
  }
}
