import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NavigationService {
  private pageSubject = new BehaviorSubject<string>(''); // Initialize with an empty string or a default value
  private webSocketMessage = new BehaviorSubject<string>(''); // Initialize with an empty string or a default value

  page$ = this.pageSubject.asObservable(); // Observable to subscribe to
  wsmsg$ = this.webSocketMessage.asObservable(); // Observable to subscribe to

  setCommanderPages(page: string): void {
    this.pageSubject.next(page); // Emit a new value
  }

  webSocketCommunications(message: string): void {
    this.webSocketMessage.next(message); // Emit a new value
  }
}