import { NotificationType } from './NotificationType.enum';

export interface Notification {
	message: string;
	type: NotificationType;
	duration?: number;
}
