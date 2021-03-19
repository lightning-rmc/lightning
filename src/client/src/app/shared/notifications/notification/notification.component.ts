import { Component, OnInit, ChangeDetectionStrategy, Input } from '@angular/core';
import { Notification } from '../Notification.model';

@Component({
	selector: 'app-notification',
	templateUrl: './notification.component.html',
	styleUrls: ['./notification.component.scss'],
	changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NotificationComponent {
	constructor() {}

	@Input()
	notification!: Notification;
}
