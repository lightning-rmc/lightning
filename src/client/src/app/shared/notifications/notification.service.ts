import { ERROR_COMPONENT_TYPE } from '@angular/compiler';
import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { Notification } from './Notification.model';
import { NotificationType } from './NotificationType.enum';

@Injectable({
	providedIn: 'root',
})
export class NotificationService {
	constructor() {}

	private onNewNotificationSubject = new Subject<Notification>();
	public onNewNotification$ = this.onNewNotificationSubject.asObservable();

	notify(message: string, type: NotificationType, duration = 4000) {
		console.log(`${type}: ${message}`);
		this.onNewNotificationSubject.next({
			message,
			type,
			duration,
		});
	}

	info(message: string, duration = 3000) {
		this.notify(message, 'INFO', duration);
	}

	error(message: string, duration = 3000) {
		this.notify(message, 'ERROR', duration);
	}
}
