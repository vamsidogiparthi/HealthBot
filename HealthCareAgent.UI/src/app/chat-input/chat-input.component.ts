import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-chat-input',
  templateUrl: './chat-input.component.html',
  imports: [FormsModule, MatButtonModule, MatDividerModule, MatIconModule],
  styleUrls: ['./chat-input.component.scss'],
})
export class ChatInputComponent {
  message: string = '';

  @ViewChild('textArea') textArea!: ElementRef<HTMLTextAreaElement>;

  autoResize(): void {
    const textarea = this.textArea.nativeElement;
    textarea.style.height = 'auto';
    textarea.style.height = textarea.scrollHeight + 'px';
  }

  sendMessage(): void {
    if (!this.message.trim()) return;
    console.log('Sending message:', this.message);
    this.message = '';
    this.autoResize(); // reset height
  }
}
