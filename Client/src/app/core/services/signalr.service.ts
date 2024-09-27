import { Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import { Order } from '../../shared/models/order';

/**
 * Service responsible for managing the SignalR connection and real-time events.
*/
@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  /** The base URL for the SignalR hub */  
  hubUrl = environment.hubUrl;

  /** The current hub connection, if established */
  hubConnection?: HubConnection;

  /** Signal holding the current order data received through the hub */
  orderSignal = signal<Order | null>(null);

  /**
   * Creates a new SignalR hub connection.
   */
  createHubConnection(): void
  {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl, {
        withCredentials: true
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start()
      .catch(error => console.log(error));

    this.hubConnection.on('OrderCompleteNotification', (order: Order) => {
      this.orderSignal.set(order);
    });
  }

  /**
   * Stops the SignalR hub connection.
  */
  stopHubConnection(): void
  {
    if(this.hubConnection?.state === HubConnectionState.Connected)
    {
      this.hubConnection.stop().catch(error => console.log(error));
    }
  }
}
