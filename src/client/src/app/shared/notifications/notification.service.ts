import { ERROR_COMPONENT_TYPE } from '@angular/compiler';
import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { Notification } from './Notification.model';
import { NotificationType } from './NotificationType.enum';

const DEFAULT_TIME = 7000;
@Injectable({
	providedIn: 'root',
})
export class NotificationService {
	constructor() {}


	private onNewNotificationSubject = new Subject<Notification>();
	public onNewNotification$ = this.onNewNotificationSubject.asObservable();

	notify(message: string, type: NotificationType, duration = DEFAULT_TIME): void {
		console.log(`${type}: ${message}`);
		this.onNewNotificationSubject.next({
			message,
			type,
			duration,
		});
	}

	info(message: string, duration?: number): void {
		this.notify(message, 'INFO', duration);
	}

	error(message: string, duration?: number): void {
		this.notify(message, 'ERROR', duration);
	}
}
