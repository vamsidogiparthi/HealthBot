export class Message {
  sender: string;
  recipient: string;
  content: string;
  timestamp: Date;

  constructor(
    sender: string,
    recipient: string,
    content: string,
    timestamp: Date
  ) {
    this.sender = sender;
    this.recipient = recipient;
    this.content = content;
    this.timestamp = timestamp;
  }
}
