import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { environment as env } from 'src/environments/environment';
import { NotificationService } from '../shared/notifications/notification.service';

@Injectable({
	providedIn: 'root',
})
export class LiveService {
	connection: HubConnection;

	private layerActivationChangedSubject = new Subject<{ layerId: string; isActive: boolean }>();
	public layerActivationChanged$ = this.layerActivationChangedSubject.asObservable();

	constructor(private notify: NotificationService) {
		this.connection = new HubConnectionBuilder().withUrl(`${env.controller.url}/hubs/live`).build();

		this.connect();
	}

	private async connect() {
		this.connection.on('layerActivationChanged', this.handleLayerActivationChanged);
		try {
			await this.connection.start();
		} catch (error) {
			this.notify.error('Could not establish live connection to hub:\n' + error.message);
		}
	}

	private handleLayerActivationChanged(layerId: string, isActive: boolean): void {
		this.layerActivationChangedSubject.next({ layerId, isActive });
	}

	async setLayerActivation(layerId: string, isActive: boolean): Promise<void> {
		return await this.connection.invoke('setLayerActivation', layerId, isActive);
	}

	async setLiveState(state: 'LIVE' | 'NOTLIVE') {
		switch (state) {
			case 'LIVE':
				await this.connection.invoke('activateLive');
				break;
			case 'NOTLIVE':
				await this.connection.invoke('deactivateLive');
				break;
			default:
				this.notify.info(`Unhandled controller state change to '${state} in live.service'`);
		}
	}
}
