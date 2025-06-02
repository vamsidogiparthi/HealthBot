import { Injectable, signal } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Message } from '../models/message.model';
@Injectable({
  providedIn: 'root',
})
export class MessageRelayService {
  private hubConnection!: HubConnection;
  messages = signal<Message[]>([
    new Message(
      'Agent',
      'User',
      'Hello! Welcome to health care chat bot. How can I help you?',
      new Date()
    ),
  ]);
  connectionStatus = signal<'disconnected' | 'connecting' | 'connected'>(
    'disconnected'
  );

  init(url: string) {
    this.hubConnection = new HubConnectionBuilder().withUrl(url).build();

    this.hubConnection
      .start()
      .then(() => this.connectionStatus.set('connected'))
      .catch((err) => console.error('Connection failed:', err));

    // Reconnect logic
    this.hubConnection.onclose(() => {
      this.connectionStatus.set('disconnected');
      setTimeout(() => this.init(url), 5000);
    });

    // Incoming messages
    this.hubConnection.on('ReceiveMessage', (message: Message) => {
      console.log('Received message:', message);
      this.messages.update((msgs) => [...msgs, message]);
    });
  }

  sendMessage(message: Message) {
    this.hubConnection.invoke('SendMessage', message).then(() => {
      console.log('Message sent:', message);
      this.messages.set([...this.messages(), message]); // Update the signal with the new message
      console.log(this.messages());
    });
  }

  disconnect() {
    this.hubConnection.stop();
  }
}
