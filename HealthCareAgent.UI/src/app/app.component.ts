import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from './header/header.component';
import { MessagesComponent } from "./messages/messages.component";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, HeaderComponent, MessagesComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {
  title = 'HealthCareAgent.UI';
}
