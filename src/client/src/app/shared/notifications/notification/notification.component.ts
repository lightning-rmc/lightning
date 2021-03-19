import { Component, OnInit, ChangeDetectionStrategy, Input, HostBinding } from '@angular/core';
import { Notification } from '../Notification.model';

@Component({
	selector: 'app-notification',
	templateUrl: './notification.component.html',
	styleUrls: ['./notification.component.scss'],
	changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NotificationComponent {
	constructor() {}

	@HostBinding('class.error')
	get isError(): boolean {
		return this.notification.type === 'ERROR';
	}

	@HostBinding('class.info')
	get isInfo(): boolean {
		return this.notification.type === 'INFO';
	}

	@Input()
	notification!: Notification;


	get formattedMessage(): string {
		return this.notification.message.replace('\n', '<br>');
	}
}
