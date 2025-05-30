import { Injectable, signal } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Message } from '../models/message.model';
@Injectable({
  providedIn: 'root',
})
export class MessageRelayService {
  private hubConnection!: HubConnection;
  messages = signal<Message[]>([]);
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
      this.messages.update((msgs) => [...msgs, message]);
    });
  }

  sendMessage(message: Message) {
    this.hubConnection.invoke('SendMessage', message).then(() => {
      this.messages.update((msgs) => [...msgs, message]);
    });
  }

  disconnect() {
    this.hubConnection.stop();
  }
}
