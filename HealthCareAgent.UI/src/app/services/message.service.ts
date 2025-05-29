import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { Message } from '../models/message.model';

@Injectable({
  providedIn: 'root',
})
export class MessageService {
  messages = new BehaviorSubject<Message[]>([]);
  constructor() {
    this.messages.next([
      new Message(
        'Agent',
        'User',
        'Hello! Welcome to health care chat bot. How can I help you?',
        new Date()
      ),
      new Message(
        'User',
        'Agent',
        'Hi, I have a question about my health.',
        new Date()
      ),
    ]);
  }

  get messages$(): Observable<Message[]> {
    return this.messages;
  }

  addMessage(message: Message) {
    this.messages.next([...this.messages.getValue(), message]);
  }
}
