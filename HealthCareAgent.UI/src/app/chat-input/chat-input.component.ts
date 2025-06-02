import { Component, ElementRef, output, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MessageService } from '../services/message.service';
import { Message } from '../models/message.model';

@Component({
  selector: 'app-chat-input',
  templateUrl: './chat-input.component.html',
  imports: [FormsModule, MatButtonModule, MatDividerModule, MatIconModule],
  styleUrls: ['./chat-input.component.scss'],
})
export class ChatInputComponent {
  message: string = '';

  sendMessageEvent = output<string>();
  @ViewChild('textArea') textArea!: ElementRef<HTMLTextAreaElement>;

  constructor(public messageService: MessageService) {}
  autoResize(): void {
    const textarea = this.textArea.nativeElement;
    textarea.style.height = 'auto';
    textarea.style.height = textarea.scrollHeight + 'px';
  }

  sendMessage(): void {
    if (!this.message.trim()) return;
    console.log('Sending message:', this.message);

    const messageClass = new Message('User', 'Agent', this.message, new Date());

    console.log('Sending message:', messageClass);    
    this.sendMessageEvent.emit(this.message); // Emit the message to parent component
    this.message = '';
    this.autoResize(); // reset height
  }
}
